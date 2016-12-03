using Common.Enum;
using KCV.SortieBattle;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.SortieMap
{
	public class Events : IDisposable
	{
		private bool _isNormalBattle;

		private Action<bool> _actOnFinished;

		public Events()
		{
			this._actOnFinished = null;
			this._isNormalBattle = false;
		}

		public void Dispose()
		{
			Mem.Del<bool>(ref this._isNormalBattle);
			Mem.Del<Action<bool>>(ref this._actOnFinished);
		}

		public void Play(enumMapEventType iEventType, enumMapWarType iWarType, Action<bool> onFinished)
		{
			this._actOnFinished = onFinished;
			switch (iEventType)
			{
			case enumMapEventType.NOT_USE:
				this.OnFinished();
				break;
			case enumMapEventType.None:
				this.OnFinished();
				break;
			case enumMapEventType.ItemGet:
			{
				UIMapManager uIMapManager = SortieMapTaskManager.GetUIMapManager();
				uIMapManager.UpdateCellState(uIMapManager.nextCell.cellModel.CellNo, true);
				MapEventItemModel itemEvent = SortieBattleTaskManager.GetMapManager().GetItemEvent();
				EventItemGet eig = new EventItemGet(itemEvent);
				eig.PlayAnimation().Subscribe(delegate(bool _)
				{
					eig.Dispose();
					Mem.Del<EventItemGet>(ref eig);
					this.OnFinished();
				});
				break;
			}
			case enumMapEventType.Uzushio:
			{
				UIMapManager uIMapManager2 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager2.UpdateCellState(uIMapManager2.nextCell.cellModel.CellNo, true);
				MapEventHappeningModel happeningEvent = SortieBattleTaskManager.GetMapManager().GetHappeningEvent();
				EventMailstrom em = new EventMailstrom(happeningEvent);
				em.PlayAnimation().Subscribe(delegate(bool _)
				{
					em.Dispose();
					Mem.Del<EventMailstrom>(ref em);
					this.OnFinished();
				});
				break;
			}
			case enumMapEventType.War_Normal:
			case enumMapEventType.War_Boss:
			{
				UIMapManager uIMapManager3 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager3.UpdateCellState(uIMapManager3.nextCell.cellModel.CellNo, true);
				Observable.FromCoroutine(() => this.EventEnemy(iEventType), false).Subscribe<Unit>();
				break;
			}
			case enumMapEventType.Stupid:
				Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayStupid(observer, iWarType)).Subscribe(delegate(bool _)
				{
					this.OnFinished();
				});
				break;
			case enumMapEventType.AirReconnaissance:
			{
				UIMapManager uIMapManager4 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager4.UpdateCellState(uIMapManager4.nextCell.cellModel.CellNo, true);
				MapEventAirReconnaissanceModel airReconnaissanceEvent = SortieBattleTaskManager.GetMapManager().GetAirReconnaissanceEvent();
				EventAirReconnaissance ear = new EventAirReconnaissance(airReconnaissanceEvent);
				ear.PlayAnimation().Subscribe(delegate(bool _)
				{
					ear.Dispose();
					Mem.Del<EventAirReconnaissance>(ref ear);
					this.OnFinished();
				});
				break;
			}
			case enumMapEventType.PortBackEo:
			{
				UIMapManager uIMapManager5 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager5.UpdateCellState(uIMapManager5.nextCell.cellModel.CellNo, true);
				Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayPortBackEo(observer)).Subscribe(delegate(bool _)
				{
					this.OnFinished();
				});
				break;
			}
			}
		}

		[DebuggerHidden]
		private IEnumerator EventEnemy(enumMapEventType iEventType)
		{
			Events.<EventEnemy>c__Iterator11D <EventEnemy>c__Iterator11D = new Events.<EventEnemy>c__Iterator11D();
			<EventEnemy>c__Iterator11D.<>f__this = this;
			return <EventEnemy>c__Iterator11D;
		}

		[DebuggerHidden]
		private IEnumerator PlayStupid(IObserver<bool> observer, enumMapWarType iWarType)
		{
			Events.<PlayStupid>c__Iterator11E <PlayStupid>c__Iterator11E = new Events.<PlayStupid>c__Iterator11E();
			<PlayStupid>c__Iterator11E.iWarType = iWarType;
			<PlayStupid>c__Iterator11E.observer = observer;
			<PlayStupid>c__Iterator11E.<$>iWarType = iWarType;
			<PlayStupid>c__Iterator11E.<$>observer = observer;
			return <PlayStupid>c__Iterator11E;
		}

		[DebuggerHidden]
		private IEnumerator PlayPortBackEo(IObserver<bool> observer)
		{
			Events.<PlayPortBackEo>c__Iterator11F <PlayPortBackEo>c__Iterator11F = new Events.<PlayPortBackEo>c__Iterator11F();
			<PlayPortBackEo>c__Iterator11F.observer = observer;
			<PlayPortBackEo>c__Iterator11F.<$>observer = observer;
			return <PlayPortBackEo>c__Iterator11F;
		}

		private void OnFinished()
		{
			Dlg.Call<bool>(ref this._actOnFinished, this._isNormalBattle);
		}
	}
}
