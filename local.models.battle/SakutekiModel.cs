using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models.battle
{
	public class SakutekiModel
	{
		private SearchInfo _info_f;

		private SearchInfo _info_e;

		private List<List<SlotitemModel_Battle>> _planes_f;

		private List<List<SlotitemModel_Battle>> _planes_e;

		public BattleSearchValues value_f
		{
			get
			{
				return this._info_f.SearchValue;
			}
		}

		public BattleSearchValues value_e
		{
			get
			{
				return this._info_e.SearchValue;
			}
		}

		public List<List<SlotitemModel_Battle>> planes_f
		{
			get
			{
				return this._planes_f;
			}
		}

		public List<List<SlotitemModel_Battle>> planes_e
		{
			get
			{
				return this._planes_e;
			}
		}

		public SakutekiModel(SearchInfo[] infoSet, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e)
		{
			this._info_f = infoSet[0];
			this._info_e = infoSet[1];
			this._planes_f = this._createPlaneList(ships_f, Enumerable.ToDictionary<SearchUsePlane, int, List<int>>(this._info_f.UsePlane, (SearchUsePlane x) => x.Rid, (SearchUsePlane y) => y.MstIds));
			this._planes_e = this._createPlaneList(ships_e, Enumerable.ToDictionary<SearchUsePlane, int, List<int>>(this._info_e.UsePlane, (SearchUsePlane x) => x.Rid, (SearchUsePlane y) => y.MstIds));
		}

		[Obsolete("value_f を使用してください", false)]
		public bool IsSuccess_f()
		{
			return this._IsSuccess(this.value_f);
		}

		[Obsolete("value_f を使用してください", false)]
		public bool HasPlane_f()
		{
			return this._planes_f.FindAll((List<SlotitemModel_Battle> x) => x.get_Count() > 0).get_Count() > 0;
		}

		[Obsolete("value_f を使用してください", false)]
		public bool ExistLost_f()
		{
			return this._ExistLost(this.value_f);
		}

		private bool _IsSuccess(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success || value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Found;
		}

		private bool _HasPlane(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success || value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Lost || value == BattleSearchValues.Faile;
		}

		private bool _ExistLost(BattleSearchValues value)
		{
			return value == BattleSearchValues.Success_Lost || value == BattleSearchValues.Lost;
		}

		private List<List<SlotitemModel_Battle>> _createPlaneList(List<ShipModel_BattleAll> ships, Dictionary<int, List<int>> org)
		{
			List<List<SlotitemModel_Battle>> list = new List<List<SlotitemModel_Battle>>();
			for (int i = 0; i < ships.get_Count(); i++)
			{
				List<SlotitemModel_Battle> list2 = new List<SlotitemModel_Battle>();
				ShipModel_BattleAll shipModel_BattleAll = ships.get_Item(i);
				List<int> list3;
				if (shipModel_BattleAll != null && org.TryGetValue(shipModel_BattleAll.TmpId, ref list3))
				{
					for (int j = 0; j < list3.get_Count(); j++)
					{
						list2.Add(new SlotitemModel_Battle(list3.get_Item(j)));
					}
				}
				list.Add(list2);
			}
			return list;
		}

		public override string ToString()
		{
			bool flag = this._HasPlane(this.value_f);
			string text = string.Format("[味方側 索敵{0} 索敵機{1} 未帰投機{2}]\n", (!this._IsSuccess(this.value_f)) ? "失敗" : "成功", (!flag) ? "無" : "有", (!this._ExistLost(this.value_f)) ? "無" : "有");
			if (flag)
			{
				for (int i = 0; i < this._planes_f.get_Count(); i++)
				{
					for (int j = 0; j < this._planes_f.get_Item(i).get_Count(); j++)
					{
						text += string.Format("[{0}][{1}] {2}", i, j, this._planes_f.get_Item(i).get_Item(j));
					}
				}
				text += "\n";
			}
			bool flag2 = this._HasPlane(this.value_e);
			text += string.Format("[相手側 索敵{0} 索敵機{1} 未帰投機{2}]\n", (!this._IsSuccess(this.value_e)) ? "失敗" : "成功", (!flag2) ? "無" : "有", (!this._ExistLost(this.value_e)) ? "無" : "有");
			if (flag2)
			{
				for (int k = 0; k < this._planes_e.get_Count(); k++)
				{
					for (int l = 0; l < this._planes_e.get_Item(k).get_Count(); l++)
					{
						text += string.Format("[{0}][{1}] {2}", k, l, this._planes_e.get_Item(k).get_Item(l));
					}
				}
				text += "\n";
			}
			return text;
		}
	}
}
