using Server_Models;
using System;

namespace Server_Common.Formats
{
	public class User_MapCellInfo
	{
		private Mst_mapcell2 _mst_mapcell;

		public bool Passed;

		public Mst_mapcell2 Mst_mapcell
		{
			get
			{
				return this._mst_mapcell;
			}
			set
			{
				this._mst_mapcell = value;
			}
		}

		public User_MapCellInfo(Mst_mapcell2 cell, bool passed)
		{
			this.Mst_mapcell = cell;
			this.Passed = passed;
		}
	}
}
