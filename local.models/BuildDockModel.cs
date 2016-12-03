using Common.Enum;
using Server_Models;
using System;

namespace local.models
{
	public class BuildDockModel
	{
		private Mem_kdock _kdock;

		public int Id
		{
			get
			{
				return this._kdock.Rid;
			}
		}

		public KdockStates State
		{
			get
			{
				return this._kdock.State;
			}
		}

		public int ShipMstId
		{
			get
			{
				return this._kdock.Ship_id;
			}
		}

		public int TankerCount
		{
			get
			{
				return this._kdock.Tunker_num;
			}
		}

		public ShipModelMst Ship
		{
			get
			{
				if (this.ShipMstId != 0)
				{
					return new ShipModelMst(this.ShipMstId);
				}
				return null;
			}
		}

		public int Fuel
		{
			get
			{
				return this._kdock.Item1;
			}
		}

		public int Ammo
		{
			get
			{
				return this._kdock.Item2;
			}
		}

		public int Steel
		{
			get
			{
				return this._kdock.Item3;
			}
		}

		public int Baux
		{
			get
			{
				return this._kdock.Item4;
			}
		}

		public int Devkit
		{
			get
			{
				return this._kdock.Item5;
			}
		}

		public int StartTurn
		{
			get
			{
				return this._kdock.StartTime;
			}
		}

		public int CompleteTurn
		{
			get
			{
				return this._kdock.CompleteTime;
			}
		}

		public BuildDockModel(Mem_kdock mem_kdock)
		{
			this.__Update__(mem_kdock);
		}

		public bool IsLarge()
		{
			return this._kdock.IsLargeDock();
		}

		public bool IsTunker()
		{
			return this._kdock.IsTunkerDock();
		}

		public int GetTurn()
		{
			return Math.Max(this._kdock.GetRequireCreateTime(), 0);
		}

		public void __Update__(Mem_kdock mem_kdock)
		{
			this._kdock = mem_kdock;
		}

		public override string ToString()
		{
			string text = string.Format("ID:{0} 状態:{1} ", this.Id, this.State);
			if (this.State == KdockStates.CREATE || this.State == KdockStates.COMPLETE)
			{
				if (this.IsTunker())
				{
					text += string.Format("建造数:{0} 開始:{1} 終了:{2} [輸送船建造]", this.TankerCount, this.StartTurn, this.CompleteTurn);
				}
				else
				{
					text += string.Format("艦:{0} 開始:{1} 終了:{2}{3}", new object[]
					{
						this.ShipMstId,
						this.StartTurn,
						this.CompleteTurn,
						(!this.IsLarge()) ? string.Empty : " [大型艦建造]"
					});
					if (this.ShipMstId != 0)
					{
						ShipModelMst shipModelMst = new ShipModelMst(this.ShipMstId);
						text += string.Format("(建造艦:{0})", shipModelMst.Name);
					}
				}
			}
			return text;
		}
	}
}
