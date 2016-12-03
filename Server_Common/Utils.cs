using Common.Enum;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Server_Common
{
	public static class Utils
	{
		private static string masterCurrentPath = string.Empty;

		public static void initMasterPath()
		{
			if (Utils.masterCurrentPath != string.Empty)
			{
				return;
			}
			Utils.masterCurrentPath = "//" + Application.get_streamingAssetsPath() + "/Xml/tables";
			Utils.masterCurrentPath += "/master/";
			if (!Directory.Exists(Utils.masterCurrentPath))
			{
				Directory.CreateDirectory(Utils.masterCurrentPath);
			}
		}

		public static string getTableDirMaster(string tableName)
		{
			if (!tableName.Contains("mst"))
			{
				return string.Empty;
			}
			if (Utils.masterCurrentPath == string.Empty)
			{
				Utils.initMasterPath();
			}
			return Utils.masterCurrentPath;
		}

		public static IEnumerable<XElement> Xml_Result(string tableName, string recordName, string sortName)
		{
			string tableDirMaster = Utils.getTableDirMaster(tableName);
			string text = tableName + ".xml";
			string text2 = tableDirMaster + text;
			if (!File.Exists(text2))
			{
				return null;
			}
			IEnumerable<XElement> result;
			try
			{
				if (string.IsNullOrEmpty(sortName))
				{
					result = Enumerable.Select<XElement, XElement>(XElement.Load(text2).Elements(recordName), (XElement datas) => datas);
				}
				else
				{
					result = Enumerable.OrderBy<XElement, int>(XElement.Load(text2).Elements(recordName), (XElement datas) => int.Parse(datas.Element(sortName).get_Value()));
				}
			}
			catch
			{
				return null;
			}
			return result;
		}

		public static IEnumerable<XElement> Xml_Result_Where(string tableName, string recordName, Dictionary<string, string> where_dict)
		{
			string tableDirMaster = Utils.getTableDirMaster(tableName);
			string text = tableName + ".xml";
			string text2 = tableDirMaster + text;
			if (!File.Exists(text2))
			{
				return null;
			}
			IEnumerable<XElement> enumerable = Enumerable.Where<XElement>(XElement.Load(text2).Elements(recordName), delegate(XElement x)
			{
				using (Dictionary<string, string>.Enumerator enumerator = where_dict.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> current = enumerator.get_Current();
						if (!x.Element(current.get_Key()).get_Value().Equals(current.get_Value()))
						{
							return false;
						}
					}
				}
				return true;
			});
			if (Enumerable.Count<XElement>(enumerable) == 0)
			{
				enumerable = null;
			}
			return enumerable;
		}

		public static IEnumerable<XElement> Xml_Result_To_Path(string path, string recordName, string sortName)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			IEnumerable<XElement> result;
			try
			{
				if (string.IsNullOrEmpty(sortName))
				{
					result = Enumerable.Select<XElement, XElement>(XElement.Load(path).Elements(recordName), (XElement datas) => datas);
				}
				else
				{
					result = Enumerable.OrderBy<XElement, int>(XElement.Load(path).Elements(recordName), (XElement datas) => int.Parse(datas.Element(sortName).get_Value()));
				}
			}
			catch
			{
				return null;
			}
			return result;
		}

		public static bool IsBattleWin(BattleWinRankKinds rank)
		{
			return rank >= BattleWinRankKinds.B;
		}

		public static double GetRandDouble(double min, double max, double up_keisu, int scale)
		{
			List<int> list = new List<int>();
			int num = (int)Math.Pow(10.0, (double)scale);
			int num2 = (int)(min * (double)num);
			int num3 = (int)(max * (double)num);
			int num4 = (int)(up_keisu * (double)num);
			for (int i = num2; i <= num3; i += num4)
			{
				list.Add(i);
			}
			var <>__AnonType = Enumerable.First(Enumerable.OrderBy(Enumerable.Select(list, (int value) => new
			{
				value
			}), x => Guid.NewGuid()));
			return (double)<>__AnonType.value / (double)num;
		}

		public static int GetRandomRateIndex(List<double> rateValues)
		{
			double num = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			int result = 0;
			for (int i = 0; i < rateValues.get_Count(); i++)
			{
				num -= rateValues.get_Item(i);
				if (num <= 0.0)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public static bool IsValidNewGamePlus()
		{
			return Comm_UserDatas.Instance.User_plus.GetLapNum() > 0;
		}

		public static bool IsGameOver()
		{
			return Comm_UserDatas.Instance.User_kdock.get_Count() == 0 || Utils.IsTurnOver();
		}

		public static bool IsTurnOver()
		{
			return Comm_UserDatas.Instance.User_turn.Total_turn >= 3600;
		}

		public static bool IsGameClear()
		{
			Dictionary<int, Mem_mapclear> user_mapclear = Comm_UserDatas.Instance.User_mapclear;
			if (user_mapclear == null)
			{
				return false;
			}
			int num = 17;
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			int mapinfo_no = Mst_maparea.MaxMapNum(difficult, num);
			int num2 = Mst_mapinfo.ConvertMapInfoId(num, mapinfo_no);
			return user_mapclear.ContainsKey(num2) && user_mapclear.get_Item(num2).Cleared;
		}

		public static int GetBookRegNum(int type)
		{
			if (type == 1)
			{
				int maxNo = Mst_DataManager.Instance.Mst_const.get_Item(MstConstDataIndex.Book_max_ships).Int_value;
				Dictionary<int, Mst_ship> mst = Mst_DataManager.Instance.Mst_ship;
				return Enumerable.Count<Mem_book>(Comm_UserDatas.Instance.Ship_book.get_Values(), (Mem_book x) => mst.ContainsKey(x.Table_id) && maxNo >= mst.get_Item(x.Table_id).Bookno);
			}
			return Comm_UserDatas.Instance.Slot_book.get_Count();
		}

		public static Dictionary<int, Mst_mapinfo> GetActiveMap()
		{
			Dictionary<int, Mst_mapinfo> dictionary = new Dictionary<int, Mst_mapinfo>();
			using (Dictionary<int, Mst_mapinfo>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_mapinfo.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_mapinfo current = enumerator.get_Current();
					User_MapinfoFmt user_MapinfoData = current.GetUser_MapinfoData();
					if (user_MapinfoData != null && user_MapinfoData.IsGo)
					{
						dictionary.Add(current.Id, current);
					}
				}
			}
			return dictionary;
		}
	}
}
