using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapenemy2 : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _enemy_list_id;

		private int _boss;

		private int _deck_id;

		private string _deck_name;

		private int _formation_id;

		private int _e1_id;

		private int _e1_lv;

		private int _e2_id;

		private int _e2_lv;

		private int _e3_id;

		private int _e3_lv;

		private int _e4_id;

		private int _e4_lv;

		private int _e5_id;

		private int _e5_lv;

		private int _e6_id;

		private int _e6_lv;

		private int _geth;

		private int _experience;

		private static string _tableName = "mst_mapenemy2";

		public int Id
		{
			get
			{
				return this._id;
			}
			private set
			{
				this._id = value;
			}
		}

		public int Maparea_id
		{
			get
			{
				return this._maparea_id;
			}
			private set
			{
				this._maparea_id = value;
			}
		}

		public int Mapinfo_no
		{
			get
			{
				return this._mapinfo_no;
			}
			private set
			{
				this._mapinfo_no = value;
			}
		}

		public int Enemy_list_id
		{
			get
			{
				return this._enemy_list_id;
			}
			private set
			{
				this._enemy_list_id = value;
			}
		}

		public int Boss
		{
			get
			{
				return this._boss;
			}
			private set
			{
				this._boss = value;
			}
		}

		public int Deck_id
		{
			get
			{
				return this._deck_id;
			}
			private set
			{
				this._deck_id = value;
			}
		}

		public string Deck_name
		{
			get
			{
				return this._deck_name;
			}
			private set
			{
				this._deck_name = value;
			}
		}

		public int Formation_id
		{
			get
			{
				return this._formation_id;
			}
			private set
			{
				this._formation_id = value;
			}
		}

		public int E1_id
		{
			get
			{
				return this._e1_id;
			}
			private set
			{
				this._e1_id = value;
			}
		}

		public int E1_lv
		{
			get
			{
				return this._e1_lv;
			}
			private set
			{
				this._e1_lv = value;
			}
		}

		public int E2_id
		{
			get
			{
				return this._e2_id;
			}
			private set
			{
				this._e2_id = value;
			}
		}

		public int E2_lv
		{
			get
			{
				return this._e2_lv;
			}
			private set
			{
				this._e2_lv = value;
			}
		}

		public int E3_id
		{
			get
			{
				return this._e3_id;
			}
			private set
			{
				this._e3_id = value;
			}
		}

		public int E3_lv
		{
			get
			{
				return this._e3_lv;
			}
			private set
			{
				this._e3_lv = value;
			}
		}

		public int E4_id
		{
			get
			{
				return this._e4_id;
			}
			private set
			{
				this._e4_id = value;
			}
		}

		public int E4_lv
		{
			get
			{
				return this._e4_lv;
			}
			private set
			{
				this._e4_lv = value;
			}
		}

		public int E5_id
		{
			get
			{
				return this._e5_id;
			}
			private set
			{
				this._e5_id = value;
			}
		}

		public int E5_lv
		{
			get
			{
				return this._e5_lv;
			}
			private set
			{
				this._e5_lv = value;
			}
		}

		public int E6_id
		{
			get
			{
				return this._e6_id;
			}
			private set
			{
				this._e6_id = value;
			}
		}

		public int E6_lv
		{
			get
			{
				return this._e6_lv;
			}
			private set
			{
				this._e6_lv = value;
			}
		}

		public int Geth
		{
			get
			{
				return this._geth;
			}
			private set
			{
				this._geth = value;
			}
		}

		public int Experience
		{
			get
			{
				return this._experience;
			}
			private set
			{
				this._experience = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapenemy2._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.Mapinfo_no = int.Parse(element.Element("Mapinfo_no").get_Value());
			this.Enemy_list_id = int.Parse(element.Element("Enemy_list_id").get_Value());
			this.Boss = int.Parse(element.Element("Boss").get_Value());
			this.Deck_id = int.Parse(element.Element("Deck_id").get_Value());
			this.Deck_name = element.Element("Deck_name").get_Value();
			this.Formation_id = int.Parse(element.Element("Formation_id").get_Value());
			this.E1_id = int.Parse(element.Element("E1_id").get_Value());
			this.E1_lv = int.Parse(element.Element("E1_lv").get_Value());
			this.E2_id = int.Parse(element.Element("E2_id").get_Value());
			this.E2_lv = int.Parse(element.Element("E2_lv").get_Value());
			this.E3_id = int.Parse(element.Element("E3_id").get_Value());
			this.E3_lv = int.Parse(element.Element("E3_lv").get_Value());
			this.E4_id = int.Parse(element.Element("E4_id").get_Value());
			this.E4_lv = int.Parse(element.Element("E4_lv").get_Value());
			this.E5_id = int.Parse(element.Element("E5_id").get_Value());
			this.E5_lv = int.Parse(element.Element("E5_lv").get_Value());
			this.E6_id = int.Parse(element.Element("E6_id").get_Value());
			this.E6_lv = int.Parse(element.Element("E6_lv").get_Value());
			this.Geth = int.Parse(element.Element("Geth").get_Value());
			this.Experience = int.Parse(element.Element("Experience").get_Value());
		}

		public void GetEnemyShips(out List<Mem_ship> out_ship, out List<List<Mst_slotitem>> out_slot)
		{
			out_ship = new List<Mem_ship>();
			out_slot = new List<List<Mst_slotitem>>();
			if (this.E1_id > 0)
			{
				out_ship.Add(this.getMemShip(-1, this.E1_id, this.E1_lv));
			}
			if (this.E2_id > 0)
			{
				out_ship.Add(this.getMemShip(-2, this.E2_id, this.E2_lv));
			}
			if (this.E3_id > 0)
			{
				out_ship.Add(this.getMemShip(-3, this.E3_id, this.E3_lv));
			}
			if (this.E4_id > 0)
			{
				out_ship.Add(this.getMemShip(-4, this.E4_id, this.E4_lv));
			}
			if (this.E5_id > 0)
			{
				out_ship.Add(this.getMemShip(-5, this.E5_id, this.E5_lv));
			}
			if (this.E6_id > 0)
			{
				out_ship.Add(this.getMemShip(-6, this.E6_id, this.E6_lv));
			}
			using (List<Mem_ship>.Enumerator enumerator = out_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					List<Mst_slotitem> slot = new List<Mst_slotitem>();
					current.Slot.ForEach(delegate(int x)
					{
						Mst_slotitem mst_slotitem = null;
						if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(x, ref mst_slotitem))
						{
							slot.Add(mst_slotitem);
						}
					});
					out_slot.Add(slot);
				}
			}
		}

		private Mem_ship getMemShip(int rid, int mst_id, int level)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mst_id);
			Array values = Enum.GetValues(typeof(Mem_ship.enumKyoukaIdx));
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			using (IEnumerator enumerator = values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					dictionary.Add((Mem_ship.enumKyoukaIdx)((int)current), 0);
				}
			}
			Mem_shipBase baseData = new Mem_shipBase(rid, mst_ship, level, dictionary);
			Mem_ship mem_ship = new Mem_ship();
			mem_ship.Set_ShipParam(baseData, mst_ship, true);
			return mem_ship;
		}
	}
}
