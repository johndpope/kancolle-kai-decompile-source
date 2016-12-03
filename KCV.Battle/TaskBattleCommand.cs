using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleCommand : BaseBattleTask
	{
		private ProdBattleCommandSelect _prodBattleCommandSelect;

		private RationModel _clsRationModel;

		private ProdCombatRation _prodCombatRation;

		public ProdBattleCommandSelect prodBattleCommandSelect
		{
			get
			{
				return this._prodBattleCommandSelect;
			}
		}

		protected override bool Init()
		{
			this._prodBattleCommandSelect = ProdBattleCommandSelect.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdBattleCommandSelect.GetComponent<ProdBattleCommandSelect>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), BattleTaskManager.GetBattleManager().GetCommandPhaseModel());
			this._clsRationModel = BattleTaskManager.GetBattleManager().GetRationModel();
			if (this._clsRationModel != null)
			{
				this._prodCombatRation = ProdCombatRation.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdCombatRation.GetComponent<ProdCombatRation>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), this._clsRationModel);
				this._prodCombatRation.Play(new Action(this.OnCombatRationFinished));
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					Mem.DelComponentSafe<ProdCombatRation>(ref this._prodCombatRation);
				});
			}
			else
			{
				this.OnCombatRationFinished();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del<RationModel>(ref this._clsRationModel);
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.Command);
		}

		private void OnCombatRationFinished()
		{
			this._clsState = new StatementMachine();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitCommandSelect), new StatementMachine.StatementMachineUpdate(this.UpdateCommandSelect));
		}

		private bool InitCommandSelect(object data)
		{
			Observable.FromCoroutine(() => this._prodBattleCommandSelect.PlayShowAnimation(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitCommandBuffer), new StatementMachine.StatementMachineUpdate(this.UpdateCommandBuffer));
			}), false).Subscribe<Unit>();
			return false;
		}

		private bool UpdateCommandSelect(object data)
		{
			if (this._prodBattleCommandSelect != null)
			{
				this._prodBattleCommandSelect.Run();
				return false;
			}
			return true;
		}

		private bool InitCommandBuffer(object data)
		{
			EffectModel effectModel = BattleTaskManager.GetBattleManager().GetOpeningEffectData();
			if (effectModel != null)
			{
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer = ProdBattleCommandBuffer.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdBattleCommandBuffer.GetComponent<ProdBattleCommandBuffer>(), BattleTaskManager.GetStage(), effectModel, 0);
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer.Play(delegate
				{
					if (effectModel.Withdrawal)
					{
						this.OnCommandBufferFinished2Withdrawal();
					}
					else
					{
						this.OnCommandBufferFinished();
					}
				});
				this._prodBattleCommandSelect.DiscardAfterFadeIn().setOnComplete(delegate
				{
					Mem.DelComponentSafe<ProdBattleCommandSelect>(ref this._prodBattleCommandSelect);
				});
			}
			else
			{
				this.OnCommandBufferFinished();
				Observable.TimerFrame(20, FrameCountType.EndOfFrame).Subscribe(delegate(long _)
				{
					this._prodBattleCommandSelect.DiscardAfterFadeIn().setOnComplete(delegate
					{
						Mem.DelComponentSafe<ProdBattleCommandSelect>(ref this._prodBattleCommandSelect);
					});
				});
			}
			return false;
		}

		private bool UpdateCommandBuffer(object data)
		{
			return true;
		}

		private void OnCommandBufferFinished2Withdrawal()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			BattleTaskManager.ReqPhase(BattlePhase.WithdrawalDecision);
		}

		private void OnCommandBufferFinished()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.eventMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.Command));
		}
	}
}
