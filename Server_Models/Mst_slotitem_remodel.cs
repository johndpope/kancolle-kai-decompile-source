using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem_remodel : Model_Base
	{
		private int _id;

		private int _position;

		private int _slotitem_id;

		private int _req_material1;

		private int _req_material2;

		private int _req_material3;

		private int _req_material4;

		private int _req_material5;

		private int _req_material6;

		private int _voice_ship_id;

		private int _voice_id;

		private int _enabled;

		private int _priority;

		private string _p_ship_yomi;

		private int _p_ship_id;

		private int _p_stype;

		private static string _tableName = "mst_slotitem_remodel";

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

		public int Position
		{
			get
			{
				return this._position;
			}
			private set
			{
				this._position = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return this._slotitem_id;
			}
			private set
			{
				this._slotitem_id = value;
			}
		}

		public int Req_material1
		{
			get
			{
				return this._req_material1;
			}
			private set
			{
				this._req_material1 = value;
			}
		}

		public int Req_material2
		{
			get
			{
				return this._req_material2;
			}
			private set
			{
				this._req_material2 = value;
			}
		}

		public int Req_material3
		{
			get
			{
				return this._req_material3;
			}
			private set
			{
				this._req_material3 = value;
			}
		}

		public int Req_material4
		{
			get
			{
				return this._req_material4;
			}
			private set
			{
				this._req_material4 = value;
			}
		}

		public int Req_material5
		{
			get
			{
				return this._req_material5;
			}
			private set
			{
				this._req_material5 = value;
			}
		}

		public int Req_material6
		{
			get
			{
				return this._req_material6;
			}
			private set
			{
				this._req_material6 = value;
			}
		}

		public int Voice_id
		{
			get
			{
				return this._voice_id;
			}
			private set
			{
				this._voice_id = value;
			}
		}

		public int Enabled
		{
			get
			{
				return this._enabled;
			}
			private set
			{
				this._enabled = value;
			}
		}

		public int Priority
		{
			get
			{
				return this._priority;
			}
			private set
			{
				this._priority = value;
			}
		}

		public string P_ship_yomi
		{
			get
			{
				return this._p_ship_yomi;
			}
			private set
			{
				this._p_ship_yomi = value;
			}
		}

		public int P_ship_id
		{
			get
			{
				return this._p_ship_id;
			}
			private set
			{
				this._p_ship_id = value;
			}
		}

		public int P_stype
		{
			get
			{
				return this._p_stype;
			}
			private set
			{
				this._p_stype = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_slotitem_remodel._tableName;
			}
		}

		public int GetVoiceShipId(int deckSecondShipId)
		{
			if (this.Voice_id == 0)
			{
				return 0;
			}
			if (this._voice_ship_id > 0)
			{
				return this._voice_ship_id;
			}
			return deckSecondShipId;
		}

		public bool ValidShipId(List<Mem_ship> ships)
		{
			return this.P_ship_id != 0 && ships.get_Count() >= 2 && ships.get_Item(1).Ship_id == this.P_ship_id;
		}

		public bool ValidYomi(List<Mem_ship> ships)
		{
			return !(this.P_ship_yomi == string.Empty) && ships.get_Count() >= 2 && string.Equals(Mst_DataManager.Instance.Mst_ship.get_Item(ships.get_Item(1).Ship_id).Yomi, this._p_ship_yomi);
		}

		public bool ValidStype(List<Mem_ship> ships)
		{
			return this.P_stype != 0 && ships.get_Count() >= 2 && ships.get_Item(1).Stype == this.P_stype;
		}

		public bool IsRemodelBase(List<Mem_ship> ships)
		{
			return this.P_ship_id == 0 && this.P_ship_yomi == string.Empty && this.P_stype == 0;
		}

		public List<Mst_slotitem_remodel> GetPriority(List<Mst_slotitem_remodel> remodel_list)
		{
			if (remodel_list.get_Count() >= 2)
			{
				IOrderedEnumerable<Mst_slotitem_remodel> orderedEnumerable = Enumerable.OrderByDescending<Mst_slotitem_remodel, int>(remodel_list, (Mst_slotitem_remodel s) => s.Priority);
				return Enumerable.ToList<Mst_slotitem_remodel>(orderedEnumerable);
			}
			return remodel_list;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			char c = ',';
			string[] array = element.Element("RemodelData").get_Value().Split(new char[]
			{
				c
			});
			this.Enabled = int.Parse(array[10]);
			if (this.Enabled == 0)
			{
				return;
			}
			this.Position = int.Parse(array[0]);
			this.Slotitem_id = int.Parse(array[1]);
			this.Req_material1 = int.Parse(array[2]);
			this.Req_material2 = int.Parse(array[3]);
			this.Req_material3 = int.Parse(array[4]);
			this.Req_material4 = int.Parse(array[5]);
			this.Req_material5 = int.Parse(array[6]);
			this.Req_material6 = int.Parse(array[7]);
			this._voice_ship_id = int.Parse(array[8]);
			this.Voice_id = int.Parse(array[9]);
			this.Enabled = int.Parse(array[10]);
			this.Priority = int.Parse(array[11]);
			this.P_ship_yomi = array[12];
			this.P_ship_id = int.Parse(array[13]);
			this.P_stype = int.Parse(array[14]);
		}
	}
}
