using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapinfo : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private int _no;

		private List<int> _required_ids;

		private int _level;

		private int _item1;

		private int _item2;

		private int _item3;

		private int _item4;

		private int _maxcell;

		private int _ship_exp;

		private int _member_exp;

		private int _clear_exp;

		private string _name;

		private string _opetext;

		private string _infotext;

		private static string _tableName = "mst_mapinfo";

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

		public int No
		{
			get
			{
				return this._no;
			}
			private set
			{
				this._no = value;
			}
		}

		public List<int> Required_ids
		{
			get
			{
				return this.getRequiredIds();
			}
			private set
			{
				this._required_ids = value;
			}
		}

		public int Level
		{
			get
			{
				return this._level;
			}
			private set
			{
				this._level = value;
			}
		}

		public int Item1
		{
			get
			{
				return this._item1;
			}
			private set
			{
				this._item1 = value;
			}
		}

		public int Item2
		{
			get
			{
				return this._item2;
			}
			private set
			{
				this._item2 = value;
			}
		}

		public int Item3
		{
			get
			{
				return this._item3;
			}
			private set
			{
				this._item3 = value;
			}
		}

		public int Item4
		{
			get
			{
				return this._item4;
			}
			private set
			{
				this._item4 = value;
			}
		}

		public int Maxcell
		{
			get
			{
				return this._maxcell;
			}
			private set
			{
				this._maxcell = value;
			}
		}

		public int Ship_exp
		{
			get
			{
				return this._ship_exp;
			}
			private set
			{
				this._ship_exp = value;
			}
		}

		public int Member_exp
		{
			get
			{
				return this._member_exp;
			}
			private set
			{
				this._member_exp = value;
			}
		}

		public int Clear_exp
		{
			get
			{
				return this._clear_exp;
			}
			private set
			{
				this._clear_exp = value;
			}
		}

		public int Clear_spoint
		{
			get
			{
				return this.getSpoint();
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		public string Opetext
		{
			get
			{
				return this._opetext;
			}
			private set
			{
				this._opetext = value;
			}
		}

		public string Infotext
		{
			get
			{
				return this._infotext;
			}
			private set
			{
				this._infotext = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapinfo._tableName;
			}
		}

		public static int ConvertMapInfoId(int maparea_id, int mapinfo_no)
		{
			string text = maparea_id.ToString() + mapinfo_no.ToString();
			int result = 0;
			int.TryParse(text, ref result);
			return result;
		}

		public User_MapinfoFmt GetUser_MapinfoData()
		{
			if (!this.IsOpenMapSys())
			{
				return null;
			}
			Dictionary<int, Mem_mapclear> user_mapclear = Comm_UserDatas.Instance.User_mapclear;
			Mem_mapclear mem_mapclear = null;
			Mem_mapclear mem_mapclear2 = null;
			int num = Mst_mapinfo.ConvertMapInfoId(this.Maparea_id, 1);
			if (user_mapclear.TryGetValue(num, ref mem_mapclear2) && mem_mapclear2.State == MapClearState.InvationClose)
			{
				return null;
			}
			bool cleared = false;
			if (user_mapclear.TryGetValue(this.Id, ref mem_mapclear))
			{
				if (mem_mapclear.State == MapClearState.InvationClose)
				{
					return null;
				}
				cleared = (mem_mapclear.State == MapClearState.Cleard);
			}
			Mem_mapclear mem_mapclear3 = null;
			bool flag = false;
			if (this.No != 1)
			{
				int num2 = Mst_mapinfo.ConvertMapInfoId(this.Maparea_id, this.No - 1);
				user_mapclear.TryGetValue(num2, ref mem_mapclear3);
				if (mem_mapclear3 != null && mem_mapclear3.State != MapClearState.Cleard)
				{
					return null;
				}
				if (mem_mapclear != null && mem_mapclear.State == MapClearState.InvationOpen)
				{
					flag = true;
				}
			}
			if (this.Required_ids.get_Count() != 0)
			{
				using (List<int>.Enumerator enumerator = this.Required_ids.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						Mem_mapclear mem_mapclear4 = null;
						if (!user_mapclear.TryGetValue(current, ref mem_mapclear4))
						{
							User_MapinfoFmt result = null;
							return result;
						}
						if (!mem_mapclear4.Cleared)
						{
							User_MapinfoFmt result = null;
							return result;
						}
						if (flag && mem_mapclear4.State != MapClearState.Cleard)
						{
							User_MapinfoFmt result = null;
							return result;
						}
					}
				}
			}
			return new User_MapinfoFmt
			{
				Id = this.Id,
				Cleared = cleared,
				IsGo = true
			};
		}

		public bool IsOpenMapSys()
		{
			Mst_maparea mst_maparea = null;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(this.Maparea_id, ref mst_maparea))
			{
				return false;
			}
			if (!mst_maparea.IsOpenArea())
			{
				return false;
			}
			if (this.Required_ids.get_Count() > 0 && this.Required_ids.get_Item(0) == 999)
			{
				return false;
			}
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int num = Mst_maparea.MaxMapNum(difficult, this.Maparea_id);
			return this.No >= 5 || this.No <= num;
		}

		private int getSpoint()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int num = Mst_maparea.MaxMapNum(difficult, this.Maparea_id);
			if (this.Maparea_id == 17 && this.No == num)
			{
				return 0;
			}
			if (this.No == num)
			{
				return 1000;
			}
			return 300;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.No = int.Parse(element.Element("No").get_Value());
			this.Required_ids = this.getRequiredIds(element.Element("Required_ids").get_Value());
			this.Level = int.Parse(element.Element("Level").get_Value());
			this.Item1 = int.Parse(element.Element("Item1").get_Value());
			this.Item2 = int.Parse(element.Element("Item2").get_Value());
			this.Item3 = int.Parse(element.Element("Item3").get_Value());
			this.Item4 = int.Parse(element.Element("Item4").get_Value());
			this.Maxcell = int.Parse(element.Element("Maxcell").get_Value());
			this.Ship_exp = int.Parse(element.Element("Ship_exp").get_Value());
			this.Member_exp = int.Parse(element.Element("Member_exp").get_Value());
			this.Clear_exp = int.Parse(element.Element("Clear_exp").get_Value());
			this.Name = element.Element("Name").get_Value();
			this.Opetext = element.Element("Opetext").get_Value();
			this.Infotext = element.Element("Infotext").get_Value();
		}

		private List<int> getRequiredIds(string target)
		{
			if (string.IsNullOrEmpty(target) || target.Equals("0"))
			{
				return new List<int>();
			}
			return Enumerable.ToList<int>(Array.ConvertAll<string, int>(target.Split(new char[]
			{
				','
			}), (string x) => int.Parse(x)));
		}

		private List<int> getRequiredIds()
		{
			if (Comm_UserDatas.Instance.User_basic == null)
			{
				return this._required_ids;
			}
			List<int> result = null;
			if (this.Maparea_id == 14 && this.No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, this._required_ids);
				Dictionary<int, List<int>> arg_66_0 = dictionary;
				int arg_66_1 = 2;
				List<int> list = new List<int>();
				list.Add(54);
				list.Add(43);
				arg_66_0.Add(arg_66_1, list);
				Dictionary<int, List<int>> dictionary2 = dictionary;
				result = dictionary2.get_Item(1);
				using (Dictionary<int, List<int>>.Enumerator enumerator = dictionary2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, List<int>> current = enumerator.get_Current();
						if (this.isReqIdComplete(current.get_Value()))
						{
							result = current.get_Value();
							break;
						}
					}
				}
			}
			else if (this.Maparea_id == 16 && this.No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, this._required_ids);
				Dictionary<int, List<int>> arg_125_0 = dictionary;
				int arg_125_1 = 2;
				List<int> list = new List<int>();
				list.Add(63);
				list.Add(122);
				list.Add(141);
				arg_125_0.Add(arg_125_1, list);
				Dictionary<int, List<int>> arg_154_0 = dictionary;
				int arg_154_1 = 3;
				list = new List<int>();
				list.Add(63);
				list.Add(122);
				list.Add(151);
				arg_154_0.Add(arg_154_1, list);
				Dictionary<int, List<int>> dictionary3 = dictionary;
				result = dictionary3.get_Item(1);
				using (Dictionary<int, List<int>>.Enumerator enumerator2 = dictionary3.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, List<int>> current2 = enumerator2.get_Current();
						if (this.isReqIdComplete(current2.get_Value()))
						{
							result = current2.get_Value();
							break;
						}
					}
				}
			}
			else if (this.Maparea_id == 17 && this.No == 1)
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				dictionary.Add(1, this._required_ids);
				Dictionary<int, List<int>> arg_20E_0 = dictionary;
				int arg_20E_1 = 2;
				List<int> list = new List<int>();
				list.Add(124);
				list.Add(153);
				arg_20E_0.Add(arg_20E_1, list);
				Dictionary<int, List<int>> arg_234_0 = dictionary;
				int arg_234_1 = 3;
				list = new List<int>();
				list.Add(124);
				list.Add(163);
				arg_234_0.Add(arg_234_1, list);
				Dictionary<int, List<int>> arg_25D_0 = dictionary;
				int arg_25D_1 = 4;
				list = new List<int>();
				list.Add(152);
				list.Add(162);
				arg_25D_0.Add(arg_25D_1, list);
				Dictionary<int, List<int>> dictionary4 = dictionary;
				result = dictionary4.get_Item(1);
				using (Dictionary<int, List<int>>.Enumerator enumerator3 = dictionary4.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, List<int>> current3 = enumerator3.get_Current();
						if (this.isReqIdComplete(current3.get_Value()))
						{
							result = current3.get_Value();
							break;
						}
					}
				}
			}
			else
			{
				result = this._required_ids;
			}
			return result;
		}

		private bool isReqIdComplete(List<int> mapinfo_ids)
		{
			Dictionary<int, Mem_mapclear> clearDict = Comm_UserDatas.Instance.User_mapclear;
			return mapinfo_ids.TrueForAll(delegate(int x)
			{
				Mem_mapclear mem_mapclear = null;
				return clearDict.TryGetValue(x, ref mem_mapclear) && mem_mapclear.State == MapClearState.Cleard;
			});
		}
	}
}
