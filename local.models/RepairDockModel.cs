using Common.Enum;
using Server_Models;
using System;

namespace local.models
{
	public class RepairDockModel
	{
		private Mem_ndock _mem_data;

		public int Id
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Rid : 0;
			}
		}

		public NdockStates State
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.State : NdockStates.NOTUSE;
			}
		}

		public int ShipId
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Ship_id : 0;
			}
		}

		public int StartTurn
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.StartTime : 0;
			}
		}

		public int CompleteTurn
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.CompleteTime : 0;
			}
		}

		public int RemainingTurns
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.GetRequireTime() : 0;
			}
		}

		public int Fuel
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Item1 : 0;
			}
		}

		public int Ammo
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Item2 : 0;
			}
		}

		public int Steel
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Item3 : 0;
			}
		}

		public int Baux
		{
			get
			{
				return (this._mem_data != null) ? this._mem_data.Item4 : 0;
			}
		}

		public RepairDockModel(Mem_ndock mem_ndock)
		{
			this.__Update__(mem_ndock);
		}

		public ShipModel GetShip()
		{
			if (this.State == NdockStates.RESTORE)
			{
				return new ShipModel(this.ShipId);
			}
			return null;
		}

		public void __Update__(Mem_ndock mem_ndock)
		{
			this._mem_data = mem_ndock;
		}

		public override string ToString()
		{
			string text = string.Format("ID:{0} 状態:{1} ", this.Id, this.State);
			if (this.State == NdockStates.RESTORE)
			{
				ShipModel ship = this.GetShip();
				text += string.Format("艦:{0} 開始:{1} 終了:{2}", ship.ShortName, this.StartTurn, this.CompleteTurn);
			}
			return text;
		}
	}
}
