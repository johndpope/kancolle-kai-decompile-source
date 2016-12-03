using Common.Enum;
using Common.Struct;
using Server_Models;
using System;

namespace Server_Common.Formats
{
	public class User_HistoryFmt
	{
		public HistoryType Type;

		public TurnString DateString;

		public string AreaName;

		public Mst_mapinfo MapInfo;

		public Mst_ship FlagShip;

		public User_HistoryFmt()
		{
		}

		public User_HistoryFmt(Mem_history memObj)
		{
			this.setHistoryType(memObj);
			this.DateString = Comm_UserDatas.Instance.User_turn.GetTurnString(memObj.Turn);
			if (Mst_DataManager.Instance.Mst_mapinfo.TryGetValue(memObj.MapinfoId, ref this.MapInfo))
			{
				this.AreaName = Mst_DataManager.Instance.Mst_maparea.get_Item(this.MapInfo.Maparea_id).Name;
			}
			Mst_DataManager.Instance.Mst_ship.TryGetValue(memObj.FlagShipId, ref this.FlagShip);
		}

		private void setHistoryType(Mem_history mem_history)
		{
			if (mem_history.Type == 1)
			{
				if (mem_history.MapClearNum == 1)
				{
					this.Type = HistoryType.MapClear1;
				}
				else if (mem_history.MapClearNum == 2)
				{
					this.Type = HistoryType.MapClear2;
				}
				else
				{
					this.Type = HistoryType.MapClear3;
				}
				return;
			}
			if (mem_history.Type == 2)
			{
				this.Type = ((!mem_history.TankerLostAll) ? HistoryType.TankerLostHalf : HistoryType.TankerLostAll);
				return;
			}
			if (mem_history.Type == 3)
			{
				this.Type = HistoryType.NewAreaOpen;
				return;
			}
			if (mem_history.Type == 4)
			{
				int gameEndType = mem_history.GameEndType;
				if (gameEndType == 1)
				{
					this.Type = HistoryType.GameClear;
				}
				else if (gameEndType == 2)
				{
					this.Type = HistoryType.GameOverLost;
				}
				else if (gameEndType == 3)
				{
					this.Type = HistoryType.GameOverTurn;
				}
				return;
			}
		}
	}
}
