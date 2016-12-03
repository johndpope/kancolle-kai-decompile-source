using Common.Enum;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System;

namespace KCV.SortieMap
{
	public class TaskSortieEvent : Task
	{
		private Events _clsEvents;

		private Action<ShipRecoveryType> _actOnGoNext;

		public TaskSortieEvent(Action<ShipRecoveryType> onGoNext)
		{
			this._actOnGoNext = onGoNext;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.DelIDisposableSafe<Events>(ref this._clsEvents);
			Mem.Del<Action<ShipRecoveryType>>(ref this._actOnGoNext);
		}

		protected override bool Init()
		{
			this._clsEvents = new Events();
			SortieMapTaskManager.GetUIAreaMapFrame().ClearMessage();
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			this._clsEvents.Play(mapManager.NextCategory, mapManager.NextEventType, new Action<bool>(this.OnEventFinished));
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelIDisposableSafe<Events>(ref this._clsEvents);
			return true;
		}

		protected override bool Update()
		{
			return SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF || SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Event;
		}

		private void OnEventFinished(bool isBattle)
		{
			if (isBattle)
			{
				SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Formation);
			}
			else
			{
				MapManager mapManager = SortieBattleTaskManager.GetMapManager();
				if (!mapManager.IsNextFinal())
				{
					Dlg.Call<ShipRecoveryType>(ref this._actOnGoNext, ShipRecoveryType.None);
				}
				else
				{
					SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Result);
				}
			}
		}
	}
}
