using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models.battle;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleResult : BaseBattleTask
	{
		private ProdVeteransReport _prodVeteransReport;

		private BattleResultModel _clsBattleResult;

		protected override bool Init()
		{
			BattleTaskManager.GetTorpedoHpGauges().SetDestroy();
			SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				BattleTaskManager.GetBattleCameras().SetFieldCameraEnabled(false);
				KCV.Utils.SoundUtils.StopFadeBGM(0.25f, null);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleTaskManager.DestroyUnneccessaryObject2Result();
				Observable.FromCoroutine(new Func<IEnumerator>(BattleUtils.ClearMemory), false).Subscribe(delegate(Unit _)
				{
					this._clsBattleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					cutInEffectCamera.glowEffect.set_enabled(false);
					cutInEffectCamera.isCulling = true;
					this._prodVeteransReport = ProdVeteransReport.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdVeteransReport.GetComponent<ProdVeteransReport>(), cutInEffectCamera.get_transform(), this._clsBattleResult);
					this._clsState = new StatementMachine();
					this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitProdVeteransReport), new StatementMachine.StatementMachineUpdate(this.UpdateProdVeteransReport));
				});
			});
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			this._prodVeteransReport = null;
			this._clsBattleResult = null;
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.Result);
		}

		private bool InitProdVeteransReport(object data)
		{
			Observable.FromCoroutine(() => this._prodVeteransReport.CreateInstance(BattleTaskManager.GetBattleManager() is PracticeBattleManager), false).Subscribe(delegate(Unit _)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					this._prodVeteransReport.PlayVeteransReport();
				});
			});
			return false;
		}

		private bool UpdateProdVeteransReport(object data)
		{
			if (!this._prodVeteransReport.Run())
			{
				return false;
			}
			if (BattleTaskManager.GetBattleManager().IsPractice)
			{
				this.EndPhase(BattlePhase.AdvancingWithdrawal);
			}
			else
			{
				BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.Result));
			}
			return true;
		}
	}
}
