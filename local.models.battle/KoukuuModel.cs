using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class KoukuuModel : KoukuuModelBase
	{
		private List<ShipModel_Attacker> _attackers_f;

		private List<ShipModel_Attacker> _attackers_e;

		public BattleSeikuKinds SeikuKind
		{
			get
			{
				return (this._data.Air1 != null) ? this._data.Air1.SeikuKind : BattleSeikuKinds.None;
			}
		}

		public KoukuuModel(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, AirBattle data) : base(ships_f, ships_e, data)
		{
			this._attackers_f = new List<ShipModel_Attacker>();
			int j;
			for (j = 0; j < this._data.F_PlaneFrom.get_Count(); j++)
			{
				ShipModel_BattleAll shipModel_BattleAll = this._ships_f.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == this._data.F_PlaneFrom.get_Item(j));
				if (shipModel_BattleAll != null)
				{
					this._attackers_f.Add(shipModel_BattleAll.__CreateAttacker__());
				}
			}
			this._attackers_e = new List<ShipModel_Attacker>();
			int i;
			for (i = 0; i < this._data.E_PlaneFrom.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = this._ships_e.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == this._data.E_PlaneFrom.get_Item(i));
				if (shipModel_BattleAll2 != null)
				{
					this._attackers_e.Add(shipModel_BattleAll2.__CreateAttacker__());
				}
			}
			base._Initialize();
		}

		public bool ExistKoukuuBattle()
		{
			return this._data.F_PlaneFrom.get_Count() > 0 || this._data.E_PlaneFrom.get_Count() > 0;
		}

		public ShipModel_Attacker GetCaptainShip(bool is_friend)
		{
			if (is_friend)
			{
				return (this._attackers_f.get_Count() != 0) ? this._attackers_f.get_Item(0) : null;
			}
			return (this._attackers_e.get_Count() != 0) ? this._attackers_e.get_Item(0) : null;
		}

		public ShipModel_Battle GetFirstActionShip()
		{
			ShipModel_Battle captainShip = this.GetCaptainShip(true);
			if (captainShip == null)
			{
				captainShip = this.GetCaptainShip(false);
			}
			return captainShip;
		}

		public List<ShipModel_Attacker> GetAttackers(bool is_friend)
		{
			if (is_friend)
			{
				return this._attackers_f.GetRange(0, this._attackers_f.get_Count());
			}
			return this._attackers_e.GetRange(0, this._attackers_e.get_Count());
		}

		public PlaneModelBase[] GetPlane(int ship_tmp_id)
		{
			ShipModel_Battle shipModel_Battle = this._ships_f.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == ship_tmp_id);
			if (shipModel_Battle == null)
			{
				shipModel_Battle = this._ships_e.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == ship_tmp_id);
			}
			if (shipModel_Battle == null)
			{
				return new PlaneModelBase[0];
			}
			return this._GetPlane(shipModel_Battle).ToArray();
		}

		public SlotitemModel_Battle GetTouchPlane(bool is_friend)
		{
			if (this._data.Air1 != null)
			{
				int num = (!is_friend) ? this._data.Air1.E_TouchPlane : this._data.Air1.F_TouchPlane;
				if (num > 0)
				{
					return new SlotitemModel_Battle(num);
				}
			}
			return null;
		}

		private List<PlaneModel> _GetPlane(ShipModel_Battle ship)
		{
			List<PlaneModel> list;
			if (ship.IsFriend())
			{
				list = this._planes_f.ConvertAll<PlaneModel>((PlaneModelBase plane) => (PlaneModel)plane);
			}
			else
			{
				list = this._planes_e.ConvertAll<PlaneModel>((PlaneModelBase plane) => (PlaneModel)plane);
			}
			return list.FindAll((PlaneModel plane) => plane.Parent.TmpId == ship.TmpId);
		}

		protected override void _CreatePlanes()
		{
			this._planes_f = base.__CreatePlanes(this._attackers_f, this._data.F_PlaneFrom);
			this._planes_e = base.__CreatePlanes(this._attackers_e, this._data.E_PlaneFrom);
		}

		private string ToString_Plane(List<ShipModel_BattleAll> ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships.get_Item(i);
				if (shipModel_BattleAll != null)
				{
					PlaneModelBase[] plane = this.GetPlane(shipModel_BattleAll.TmpId);
					text += string.Format("[{0}]{1}:", i, shipModel_BattleAll.Name);
					if (plane.Length == 0)
					{
						text += string.Format("航空機無し", new object[0]);
					}
					else
					{
						for (int j = 0; j < plane.Length; j++)
						{
							text += string.Format("({0}) ", plane[j]);
						}
					}
					text += "\n";
				}
			}
			return text;
		}

		private string ToString_TouchPlane(bool is_friend)
		{
			string text = string.Empty;
			SlotitemModel_Battle touchPlane = this.GetTouchPlane(is_friend);
			if (touchPlane == null)
			{
				text += string.Format("{0}側触接:無し ", (!is_friend) ? "相手" : "味方");
			}
			else
			{
				text += string.Format("{0}側触接:{1} ", (!is_friend) ? "相手" : "味方", touchPlane);
			}
			return text;
		}

		public override string ToString()
		{
			string text = "[航空戦データ]\n";
			text += string.Format("--味方側航空機\n", new object[0]);
			text += this.ToString_Plane(this._ships_f);
			text += string.Format("--相手側航空機\n", new object[0]);
			text += this.ToString_Plane(this._ships_e);
			text += string.Format("--制空権:{0} ", this.SeikuKind);
			text += this.ToString_TouchPlane(true);
			text += this.ToString_TouchPlane(false);
			text += string.Format("味方側カットイン対象艦:{0} ", this.GetCaptainShip(true));
			text += string.Format("相手側カットイン対象艦:{0}\n", this.GetCaptainShip(false));
			text += base.ToString_Stage1();
			text += base.ToString_Stage2();
			return text + base.ToString_Stage3();
		}
	}
}
