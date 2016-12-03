using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleFlagshipWreck : BaseBattleTask
	{
		private ProdFlagshipWreck _prodFlagshipWreck;

		protected override bool Init()
		{
			base.Init();
			this._clsState = new StatementMachine();
			BattleTaskManager.GetPrefabFile().battleShutter.Init(BaseShutter.ShutterMode.Close);
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initFlagshipWreck), new StatementMachine.StatementMachineUpdate(this._updateFlagshipWreck));
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			if (this._prodFlagshipWreck != null)
			{
				this._prodFlagshipWreck.Discard();
			}
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.FlagshipWreck);
		}

		private bool _initFlagshipWreck(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateFlagshipWreck(observer)).Subscribe(delegate(bool _)
			{
				this._prodFlagshipWreck.Play(new Action(this._onFlagshipWreckFinished));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateFlagshipWreck(IObserver<bool> observer)
		{
			TaskBattleFlagshipWreck.<CreateFlagshipWreck>c__IteratorF8 <CreateFlagshipWreck>c__IteratorF = new TaskBattleFlagshipWreck.<CreateFlagshipWreck>c__IteratorF8();
			<CreateFlagshipWreck>c__IteratorF.observer = observer;
			<CreateFlagshipWreck>c__IteratorF.<$>observer = observer;
			<CreateFlagshipWreck>c__IteratorF.<>f__this = this;
			return <CreateFlagshipWreck>c__IteratorF;
		}

		private bool _updateFlagshipWreck(object data)
		{
			return this._prodFlagshipWreck.Run();
		}

		private void _onFlagshipWreckFinished()
		{
			if (BattleTaskManager.IsSortieBattle() && SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataFlagshipWreck(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
		}
	}
}
