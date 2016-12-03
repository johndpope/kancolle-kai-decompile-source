using Common.Enum;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class HistoryModel_AreaClear : HistoryModel_AreaStart
	{
		private ShipModelMst _mst_ship;

		public int ClearCount
		{
			get
			{
				if (base.Type == HistoryType.MapClear1)
				{
					return 1;
				}
				if (base.Type == HistoryType.MapClear2)
				{
					return 2;
				}
				if (base.Type == HistoryType.MapClear3)
				{
					return 3;
				}
				return 0;
			}
		}

		public ShipModelMst FlagShip
		{
			get
			{
				return this._mst_ship;
			}
		}

		public HistoryModel_AreaClear(User_HistoryFmt fmt) : base(fmt)
		{
			this._mst_ship = new ShipModelMst(fmt.FlagShip);
		}
	}
}
