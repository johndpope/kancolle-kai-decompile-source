using Common.Enum;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.SortieMap
{
	public class TaskSortieMoveShip : Task
	{
		private IDisposable _disShipMoveObserver;

		public TaskSortieMoveShip()
		{
			this._disShipMoveObserver = null;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.DelIDisposableSafe<IDisposable>(ref this._disShipMoveObserver);
		}

		protected override bool Init()
		{
			UISortieShipCharacter uIShipCharacter = SortieMapTaskManager.GetUIShipCharacter();
			uIShipCharacter.SetShipData(SortieBattleTaskManager.GetMapManager().Deck.GetFlagShip());
			TutorialModel tutorial = SortieBattleTaskManager.GetMapManager().UserInfo.Tutorial;
			if (SortieBattleTaskManager.GetMapManager().UserInfo.StartMapCount >= 5)
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.BattleShortCutInfo, null, delegate
				{
					this._disShipMoveObserver = Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.ShipMove(observer)).Subscribe(delegate(bool __)
					{
						SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Event);
					});
				});
			}
			else
			{
				this._disShipMoveObserver = Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.ShipMove(observer)).Subscribe(delegate(bool __)
				{
					SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Event);
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelIDisposableSafe<IDisposable>(ref this._disShipMoveObserver);
			return true;
		}

		protected override bool Update()
		{
			return SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF || SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST;
		}

		[DebuggerHidden]
		private IEnumerator ShipMove(IObserver<bool> observer)
		{
			TaskSortieMoveShip.<ShipMove>c__Iterator122 <ShipMove>c__Iterator = new TaskSortieMoveShip.<ShipMove>c__Iterator122();
			<ShipMove>c__Iterator.observer = observer;
			<ShipMove>c__Iterator.<$>observer = observer;
			<ShipMove>c__Iterator.<>f__this = this;
			return <ShipMove>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ChkUnderwayReplenishment(MapManager manager)
		{
			TaskSortieMoveShip.<ChkUnderwayReplenishment>c__Iterator123 <ChkUnderwayReplenishment>c__Iterator = new TaskSortieMoveShip.<ChkUnderwayReplenishment>c__Iterator123();
			<ChkUnderwayReplenishment>c__Iterator.manager = manager;
			<ChkUnderwayReplenishment>c__Iterator.<$>manager = manager;
			return <ChkUnderwayReplenishment>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ChkCompass(MapManager manager, UIMapManager uiManager, UISortieShip ship)
		{
			TaskSortieMoveShip.<ChkCompass>c__Iterator124 <ChkCompass>c__Iterator = new TaskSortieMoveShip.<ChkCompass>c__Iterator124();
			<ChkCompass>c__Iterator.manager = manager;
			<ChkCompass>c__Iterator.ship = ship;
			<ChkCompass>c__Iterator.uiManager = uiManager;
			<ChkCompass>c__Iterator.<$>manager = manager;
			<ChkCompass>c__Iterator.<$>ship = ship;
			<ChkCompass>c__Iterator.<$>uiManager = uiManager;
			return <ChkCompass>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ChkProduction(MapManager manager, UIMapManager uiManager, UISortieShip ship)
		{
			TaskSortieMoveShip.<ChkProduction>c__Iterator125 <ChkProduction>c__Iterator = new TaskSortieMoveShip.<ChkProduction>c__Iterator125();
			<ChkProduction>c__Iterator.manager = manager;
			<ChkProduction>c__Iterator.uiManager = uiManager;
			<ChkProduction>c__Iterator.ship = ship;
			<ChkProduction>c__Iterator.<$>manager = manager;
			<ChkProduction>c__Iterator.<$>uiManager = uiManager;
			<ChkProduction>c__Iterator.<$>ship = ship;
			return <ChkProduction>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ChkComment(MapManager manager, UISortieShip ship)
		{
			TaskSortieMoveShip.<ChkComment>c__Iterator126 <ChkComment>c__Iterator = new TaskSortieMoveShip.<ChkComment>c__Iterator126();
			<ChkComment>c__Iterator.manager = manager;
			<ChkComment>c__Iterator.ship = ship;
			<ChkComment>c__Iterator.<$>manager = manager;
			<ChkComment>c__Iterator.<$>ship = ship;
			return <ChkComment>c__Iterator;
		}

		private void CheckNextBossCell(MapManager manager)
		{
			UIShortCutSwitch shortCutSwitch = SortieMapTaskManager.GetShortCutSwitch();
			if (manager.NextCategory != enumMapEventType.War_Boss)
			{
				shortCutSwitch.SetIsValid(true, true);
			}
			else
			{
				shortCutSwitch.SetIsValid(manager.Map.ClearedOnce, true);
			}
		}
	}
}
