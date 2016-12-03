using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleShelling : BaseBattleTask
	{
		private ProdShellingFormationJudge _prodShellingFormationJudge;

		private ProdShellingAttack _prodShellingAttack;

		private ProdShellingTorpedo _prodShellingTorpedo;

		private HougekiListModel _clsNowHougekiList;

		private RaigekiModel _clsNowRaigeki;

		private List<CmdActionPhaseModel> _listCmdActionList;

		private int _nCurrentShellingCnt;

		private bool _isFriendActionExit;

		private bool _isEnemyActionExit;

		private Action _actOnFleetAction;

		private int shellingCnt
		{
			get
			{
				if (this._clsNowHougekiList == null)
				{
					return -1;
				}
				return this._clsNowHougekiList.Count;
			}
		}

		private bool isNextAttack
		{
			get
			{
				return this.shellingCnt != this._nCurrentShellingCnt;
			}
		}

		private CmdActionPhaseModel currentCmdActionPhase
		{
			get
			{
				return this._listCmdActionList.get_Item(this._nCurrentShellingCnt);
			}
		}

		protected override bool Init()
		{
			if (!BattleTaskManager.GetBattleManager().IsExistHougekiPhase_Day())
			{
				base.ImmediateTermination();
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.Shelling));
			}
			else
			{
				this._listCmdActionList = BattleTaskManager.GetBattleManager().GetHougekiData_Day();
				this._nCurrentShellingCnt = 0;
				this._actOnFleetAction = null;
				this._prodShellingFormationJudge = ProdShellingFormationJudge.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdShellingFormationJudge.GetComponent<ProdShellingFormationJudge>(), BattleTaskManager.GetBattleManager(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
				this._prodShellingAttack = new ProdShellingAttack();
				this._clsState = new StatementMachine();
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitFormationJudge), new StatementMachine.StatementMachineUpdate(this.UpdateFormationJudge));
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			this._prodShellingFormationJudge = null;
			if (this._prodShellingAttack != null)
			{
				this._prodShellingAttack.Dispose();
			}
			this._prodShellingAttack = null;
			this._clsNowHougekiList = null;
			Mem.DelIDisposableSafe<ProdShellingTorpedo>(ref this._prodShellingTorpedo);
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.Shelling);
		}

		private bool InitFormationJudge(object data)
		{
			this._prodShellingFormationJudge.Play(delegate
			{
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
				BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				BattleTaskManager.GetBattleShips().RadarDeployment(false);
			}, new Action(this.OnFormationJudgeFinished));
			return false;
		}

		private bool UpdateFormationJudge(object data)
		{
			return true;
		}

		private void OnFormationJudgeFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitCommandBuffer), new StatementMachine.StatementMachineUpdate(this.UpdateCommandBuffer));
		}

		private bool InitCommandBuffer(object data)
		{
			if (this._nCurrentShellingCnt == this._listCmdActionList.get_Count())
			{
				this.OnShellingPhaseFinished();
				return false;
			}
			this._isFriendActionExit = false;
			this._isEnemyActionExit = false;
			EffectModel effectModel = BattleTaskManager.GetBattleManager().GetEffectData(this._nCurrentShellingCnt);
			if (effectModel != null)
			{
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer = ProdBattleCommandBuffer.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdBattleCommandBuffer.GetComponent<ProdBattleCommandBuffer>(), BattleTaskManager.GetStage(), effectModel, this._nCurrentShellingCnt);
				BattleTaskManager.GetPrefabFile().prodBattleCommandBuffer.Play(delegate
				{
					if (effectModel.Withdrawal)
					{
						BattleTaskManager.ReqPhase(BattlePhase.WithdrawalDecision);
					}
					else
					{
						this.CheckNextAction();
					}
				});
			}
			else
			{
				this.CheckNextAction();
			}
			return false;
		}

		private bool UpdateCommandBuffer(object data)
		{
			return true;
		}

		private void CheckNextAction()
		{
			if ((this._isFriendActionExit && this._isEnemyActionExit) || this.currentCmdActionPhase == null)
			{
				this._nCurrentShellingCnt++;
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitCommandBuffer), new StatementMachine.StatementMachineUpdate(this.UpdateCommandBuffer));
				return;
			}
			this._actOnFleetAction = null;
			this._clsNowHougekiList = null;
			this._clsNowRaigeki = null;
			if (!this._isFriendActionExit)
			{
				if (this.currentCmdActionPhase.Action_f != null)
				{
					if (this.currentCmdActionPhase.Action_f is HougekiListModel)
					{
						this._actOnFleetAction = new Action(this.CheckNextAction);
						this._clsNowHougekiList = (this.currentCmdActionPhase.Action_f as HougekiListModel);
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShelling), new StatementMachine.StatementMachineUpdate(this.UpdateShelling));
					}
					else
					{
						this._actOnFleetAction = new Action(this.CheckNextAction);
						this._clsNowRaigeki = (this.currentCmdActionPhase.Action_f as RaigekiModel);
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitTorpedo), new StatementMachine.StatementMachineUpdate(this.UpdateTorpedo));
					}
					this._isFriendActionExit = true;
					return;
				}
				this._isFriendActionExit = true;
				this.CheckNextAction();
				return;
			}
			else
			{
				if (this._isEnemyActionExit)
				{
					return;
				}
				if (this.currentCmdActionPhase.Action_e != null)
				{
					if (this.currentCmdActionPhase.Action_e is HougekiListModel)
					{
						this._actOnFleetAction = new Action(this.CheckNextAction);
						this._clsNowHougekiList = (this.currentCmdActionPhase.Action_e as HougekiListModel);
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShelling), new StatementMachine.StatementMachineUpdate(this.UpdateShelling));
					}
					else
					{
						this._actOnFleetAction = new Action(this.CheckNextAction);
						this._clsNowRaigeki = (this.currentCmdActionPhase.Action_e as RaigekiModel);
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitTorpedo), new StatementMachine.StatementMachineUpdate(this.UpdateTorpedo));
					}
					this._isEnemyActionExit = true;
					return;
				}
				this._isEnemyActionExit = true;
				this.CheckNextAction();
				return;
			}
		}

		protected bool InitShelling(object data)
		{
			HougekiModel nextData = this._clsNowHougekiList.GetNextData();
			if (nextData == null)
			{
				Dlg.Call(ref this._actOnFleetAction);
			}
			else
			{
				this._prodShellingAttack.Play(nextData, this._nCurrentShellingCnt, this.isNextAttack, null);
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			}
			return false;
		}

		protected bool UpdateShelling(object data)
		{
			if (this._prodShellingAttack.isFinished)
			{
				this._prodShellingAttack.Clear();
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShelling), new StatementMachine.StatementMachineUpdate(this.UpdateShelling));
				return true;
			}
			if (this._prodShellingAttack != null)
			{
				this._prodShellingAttack.Update();
			}
			return false;
		}

		protected void OnShellingFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitCommandBuffer), new StatementMachine.StatementMachineUpdate(this.UpdateCommandBuffer));
		}

		private bool InitTorpedo(object data)
		{
			this._prodShellingTorpedo = new ProdShellingTorpedo(this._clsNowRaigeki);
			this._prodShellingTorpedo.Play(delegate
			{
				this.OnTorpedoTerminate();
			});
			return false;
		}

		private bool UpdateTorpedo(object data)
		{
			if (this._prodShellingTorpedo != null)
			{
				this._prodShellingTorpedo.Update();
			}
			return false;
		}

		private void OnTorpedoTerminate()
		{
			this._clsState.Clear();
			this.PlayProdDamage(this._clsNowRaigeki, delegate
			{
				UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
				circleHPGauge.get_transform().localScaleZero();
				Mem.DelIDisposableSafe<ProdShellingTorpedo>(ref this._prodShellingTorpedo);
				Dlg.Call(ref this._actOnFleetAction);
			});
		}

		private void OnShellingPhaseFinished()
		{
			this.EndPhase(BattleUtils.NextPhase(BattlePhase.Shelling));
		}
	}
}
