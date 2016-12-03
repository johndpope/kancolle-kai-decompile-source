using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class HougekiListModel : ICommandAction
	{
		private List<HougekiModel> _list;

		private int _phase_index;

		public int Count
		{
			get
			{
				return this._list.get_Count();
			}
		}

		public HougekiListModel(List<Hougeki<BattleAtackKinds_Day>> hougeki, Dictionary<int, ShipModel_BattleAll> ships)
		{
			this._list = new List<HougekiModel>();
			for (int i = 0; i < hougeki.get_Count(); i++)
			{
				HougekiModel hougekiModel = new HougekiModel(hougeki.get_Item(i), ships);
				this._list.Add(hougekiModel);
			}
		}

		public HougekiListModel(List<Hougeki<BattleAtackKinds_Night>> hougeki, Dictionary<int, ShipModel_BattleAll> ships)
		{
			this._list = new List<HougekiModel>();
			for (int i = 0; i < hougeki.get_Count(); i++)
			{
				HougekiModel hougekiModel = new HougekiModel(hougeki.get_Item(i), ships);
				this._list.Add(hougekiModel);
			}
		}

		public HougekiModel[] GetData()
		{
			return this._list.ToArray();
		}

		public HougekiModel GetData(int index)
		{
			return this._list.get_Item(index);
		}

		public HougekiModel GetNextData()
		{
			if (this._list.get_Count() > this._phase_index)
			{
				HougekiModel result = this._list.get_Item(this._phase_index);
				this._phase_index++;
				return result;
			}
			return null;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < this._list.get_Count(); i++)
			{
				text += string.Format("[{0}:{1}]", i, this._list.get_Item(i));
				if (i < this._list.get_Count() - 1)
				{
					text += "\n";
				}
			}
			return text;
		}
	}
}
