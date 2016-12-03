using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MapModel
	{
		private Mst_mapinfo _mst_map_data;

		private User_MapinfoFmt _mem_map_data;

		private MapHPModel _mapHP;

		public int MstId
		{
			get
			{
				return this._mst_map_data.Id;
			}
		}

		public int AreaId
		{
			get
			{
				return this._mst_map_data.Maparea_id;
			}
		}

		public int No
		{
			get
			{
				return this._mst_map_data.No;
			}
		}

		public string Name
		{
			get
			{
				return this._mst_map_data.Name;
			}
		}

		public int Level
		{
			get
			{
				return this._mst_map_data.Level;
			}
		}

		public string Opetext
		{
			get
			{
				return this._mst_map_data.Opetext;
			}
		}

		public string Infotext
		{
			get
			{
				return this._mst_map_data.Infotext;
			}
		}

		public bool Cleared
		{
			get
			{
				return this._mem_map_data != null && this._mem_map_data.Cleared;
			}
		}

		public bool ClearedOnce
		{
			get
			{
				Mem_mapclear mem_mapclear;
				return this._mem_map_data != null && Comm_UserDatas.Instance.User_mapclear.TryGetValue(this.MstId, ref mem_mapclear) && mem_mapclear.Cleared;
			}
		}

		public MapHPModel MapHP
		{
			get
			{
				return this._mapHP;
			}
		}

		public bool Map_Possible
		{
			get
			{
				return this._mem_map_data != null && this._mem_map_data.IsGo;
			}
		}

		public MapModel(Mst_mapinfo mst_map, User_MapinfoFmt mem_map)
		{
			this._mst_map_data = mst_map;
			this._mem_map_data = mem_map;
			this._mapHP = new MapHPModel(this._mst_map_data, this._mem_map_data);
			if (this._mapHP.MaxValue == 0)
			{
				this._mapHP = null;
			}
		}

		public int[] GetRewardItemIds()
		{
			List<int> list = new List<int>();
			list.Add(this._mst_map_data.Item1);
			list.Add(this._mst_map_data.Item2);
			list.Add(this._mst_map_data.Item3);
			list.Add(this._mst_map_data.Item4);
			List<int> list2 = list;
			return list2.FindAll((int itemID) => itemID > 0).ToArray();
		}

		public string ToShortString()
		{
			return string.Format("#{0}-{1}(Id:{2})  {3} {4} {5} {6}", new object[]
			{
				this.AreaId,
				this.No,
				this.MstId,
				this.Name,
				(!this.Cleared) ? string.Empty : "[Cleared]",
				(!this.Map_Possible) ? "[出撃不可]" : string.Empty,
				(this.MapHP == null) ? string.Empty : this.MapHP.ToString()
			});
		}

		public override string ToString()
		{
			string text = this.Name + "(ID:" + this.MstId.ToString() + ")\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"マスタID:",
				this.MstId,
				"  海域ID:",
				this.AreaId,
				"  マップ番号:",
				this.No,
				"  地名:",
				this.Name,
				"\n"
			});
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"作戦内容:",
				this.Opetext,
				"    エリア情報:",
				this.Infotext,
				"\n"
			});
			text += string.Format("レベル:{0}    取得可能アイテム:", this.Level);
			int[] rewardItemIds = this.GetRewardItemIds();
			for (int i = 0; i < rewardItemIds.Length; i++)
			{
				text += string.Format("{0}", rewardItemIds[i]);
				if (i == rewardItemIds.Length - 1)
				{
					text += "\n";
				}
				else
				{
					text += "/";
				}
			}
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"クリア状態:",
				this.Cleared,
				"    出撃可能なマップ(マップ表示の有無):",
				this.Map_Possible,
				"\n"
			});
			if (this.MapHP != null)
			{
				text = text + this.MapHP + "\n";
			}
			return text;
		}
	}
}
