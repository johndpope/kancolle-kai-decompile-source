using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class EscortDeckModel : DeckModelBase
	{
		protected Mem_esccort_deck _mem_escort_deck;

		public override int Id
		{
			get
			{
				return this._mem_escort_deck.Rid;
			}
		}

		public override int AreaId
		{
			get
			{
				return this._mem_escort_deck.Maparea_id;
			}
		}

		public override string Name
		{
			get
			{
				string text = (this._mem_escort_deck != null) ? this._mem_escort_deck.Name : string.Empty;
				if (text == string.Empty)
				{
					string name = Mst_DataManager.Instance.Mst_maparea.get_Item(this.Id).Name;
					text = name.Replace("海域", string.Empty) + "航路護衛隊";
				}
				return text;
			}
		}

		public virtual int Turn
		{
			get
			{
				return this._mem_escort_deck.GetBlingTurn();
			}
		}

		public virtual string __Name__
		{
			get
			{
				return this._mem_escort_deck.Name;
			}
		}

		public EscortDeckModel(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships)
		{
			this.__Update__(mem_escort_deck, ships);
		}

		public bool IsMove()
		{
			return this.Turn > 0;
		}

		public void __Update__(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships)
		{
			this._mem_escort_deck = mem_escort_deck;
			if (this._mem_escort_deck != null)
			{
				base._Update(this._mem_escort_deck.Ship, ships);
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			ShipModel[] ships = base.GetShips();
			if (ships.Length == 0)
			{
				text += string.Format("{0}({1}) 未配備", this.Name, this.Id);
			}
			else
			{
				text += string.Format("[{0}({1})][", this.Name, this.Id);
				for (int i = 0; i < ships.Length; i++)
				{
					text += string.Format("{0}({1},{2})", ships[i].Name, ships[i].MstId, ships[i].MemId);
					if (i + 1 < ships.Length)
					{
						text += string.Format(", ", new object[0]);
					}
				}
				if (this.Turn > 0)
				{
					text += string.Format("  移動中:残り{0}ターン", this.Turn);
				}
				text += string.Format("]", new object[0]);
			}
			return text;
		}
	}
}
