using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_maparea : Model_Base
	{
		private int _id;

		private string _no;

		private string _name;

		private int _evt_flag;

		private int _material_1;

		private int _material_2;

		private int _material_3;

		private int _material_4;

		private int _req_tanker_num;

		private int _ndocks_init;

		private int _ndocks_max;

		private int _erc_air_rate;

		private int _erc_submarine_rate;

		private int _distance;

		private List<int> _neighboring_area;

		private static string _tableName = "mst_maparea";

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

		public string No
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

		public int Evt_flag
		{
			get
			{
				return this._evt_flag;
			}
			private set
			{
				this._evt_flag = value;
			}
		}

		public int Material_1
		{
			get
			{
				return this._material_1;
			}
			private set
			{
				this._material_1 = value;
			}
		}

		public int Material_2
		{
			get
			{
				return this._material_2;
			}
			private set
			{
				this._material_2 = value;
			}
		}

		public int Material_3
		{
			get
			{
				return this._material_3;
			}
			private set
			{
				this._material_3 = value;
			}
		}

		public int Material_4
		{
			get
			{
				return this._material_4;
			}
			private set
			{
				this._material_4 = value;
			}
		}

		public int Req_tanker_num
		{
			get
			{
				return this._req_tanker_num;
			}
			private set
			{
				this._req_tanker_num = value;
			}
		}

		public int Ndocks_init
		{
			get
			{
				return this._ndocks_init;
			}
			private set
			{
				this._ndocks_init = value;
			}
		}

		public int Ndocks_max
		{
			get
			{
				return this._ndocks_max;
			}
			private set
			{
				this._ndocks_max = value;
			}
		}

		public int Erc_air_rate
		{
			get
			{
				return this._erc_air_rate;
			}
			private set
			{
				this._erc_air_rate = value;
			}
		}

		public int Erc_submarine_rate
		{
			get
			{
				return this._erc_submarine_rate;
			}
			private set
			{
				this._erc_submarine_rate = value;
			}
		}

		public int Distance
		{
			get
			{
				return this._distance;
			}
			private set
			{
				this._distance = value;
			}
		}

		public List<int> Neighboring_area
		{
			get
			{
				return this._neighboring_area;
			}
			private set
			{
				this._neighboring_area = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_maparea._tableName;
			}
		}

		public int GetMaterialLimitTankerNum()
		{
			return (int)((double)this.Req_tanker_num * 1.2);
		}

		public int GetUIMaterialLimitTankerNum()
		{
			int materialLimitTankerNum = this.GetMaterialLimitTankerNum();
			return (this.Req_tanker_num != materialLimitTankerNum) ? materialLimitTankerNum : (materialLimitTankerNum + 1);
		}

		public bool IsActiveArea()
		{
			int num = Mst_mapinfo.ConvertMapInfoId(this.Id, 1);
			return Mst_DataManager.Instance.Mst_mapinfo.get_Item(num).GetUser_MapinfoData() != null;
		}

		public static int MaxMapNum(DifficultKind kind, int area_id)
		{
			if (area_id == 15)
			{
				return (kind < DifficultKind.KOU) ? 3 : 4;
			}
			if (area_id == 16)
			{
				return (kind < DifficultKind.OTU) ? 3 : 4;
			}
			if (area_id == 17)
			{
				return (kind < DifficultKind.SHI) ? 3 : 4;
			}
			return 4;
		}

		protected override void setProperty(XElement element)
		{
			char[] array = new char[]
			{
				','
			};
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.No = element.Element("No").get_Value();
			this.Name = element.Element("Name").get_Value();
			this.Evt_flag = int.Parse(element.Element("Evt_flag").get_Value());
			this.Material_1 = int.Parse(element.Element("Material_1").get_Value());
			this.Material_2 = int.Parse(element.Element("Material_2").get_Value());
			this.Material_3 = int.Parse(element.Element("Material_3").get_Value());
			this.Material_4 = int.Parse(element.Element("Material_4").get_Value());
			this.Req_tanker_num = int.Parse(element.Element("Req_tanker_num").get_Value());
			this.Ndocks_init = int.Parse(element.Element("Ndocks_init").get_Value());
			this.Ndocks_max = int.Parse(element.Element("Ndocks_max").get_Value());
			this.Erc_air_rate = int.Parse(element.Element("Erc_air_rate").get_Value());
			this.Erc_submarine_rate = int.Parse(element.Element("Erc_submarine_rate").get_Value());
			this.Distance = int.Parse(element.Element("Distance").get_Value());
			string[] array2 = element.Element("Neighboring_area").get_Value().Split(array);
			this.Neighboring_area = Enumerable.ToList<int>(Array.ConvertAll<string, int>(array2, (string x) => int.Parse(x)));
		}

		public bool IsOpenArea()
		{
			return this.Evt_flag == 0;
		}

		public void TakeMaterialNum(Dictionary<int, Mem_mapclear> mapclear, int tankerNum, ref Dictionary<enumMaterialCategory, int> addValues, bool randMaxFlag, DeckShips deckShip)
		{
			if (tankerNum == 0)
			{
				return;
			}
			if (!this.IsActiveArea())
			{
				return;
			}
			int num = Enumerable.Count<Mem_ship>(deckShip.getMemShip(), (Mem_ship x) => x.IsEscortDeffender());
			double num2 = (double)tankerNum;
			double num3 = (double)this.Req_tanker_num;
			Dictionary<enumMaterialCategory, double> dictionary = new Dictionary<enumMaterialCategory, double>();
			dictionary.Add(enumMaterialCategory.Fuel, 0.0);
			dictionary.Add(enumMaterialCategory.Bull, 0.0);
			dictionary.Add(enumMaterialCategory.Steel, 0.0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0.0);
			Dictionary<enumMaterialCategory, double> dictionary2 = dictionary;
			int materialLimitTankerNum = this.GetMaterialLimitTankerNum();
			if (tankerNum <= this.Req_tanker_num)
			{
				dictionary2.set_Item(enumMaterialCategory.Fuel, (double)((int)((double)this.Material_1 * num2 / num3)));
				dictionary2.set_Item(enumMaterialCategory.Bull, (double)((int)((double)this.Material_2 * num2 / num3)));
				dictionary2.set_Item(enumMaterialCategory.Steel, (double)((int)((double)this.Material_3 * num2 / num3)));
				dictionary2.set_Item(enumMaterialCategory.Bauxite, (double)((int)((double)this.Material_4 * num2 / num3)));
				if (num == 0)
				{
					double min = (!randMaxFlag) ? 0.5 : 0.75;
					dictionary2.set_Item(enumMaterialCategory.Fuel, dictionary2.get_Item(enumMaterialCategory.Fuel) * Utils.GetRandDouble(min, 0.75, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bull, dictionary2.get_Item(enumMaterialCategory.Bull) * Utils.GetRandDouble(min, 0.75, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Steel, dictionary2.get_Item(enumMaterialCategory.Steel) * Utils.GetRandDouble(min, 0.75, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bauxite, dictionary2.get_Item(enumMaterialCategory.Bauxite) * Utils.GetRandDouble(min, 0.75, 0.01, 2));
				}
				int num4 = num * 2;
				if (num4 < tankerNum && num < 6)
				{
					double min2 = (!randMaxFlag) ? 0.75 : 1.0;
					dictionary2.set_Item(enumMaterialCategory.Fuel, dictionary2.get_Item(enumMaterialCategory.Fuel) * Utils.GetRandDouble(min2, 1.0, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bull, dictionary2.get_Item(enumMaterialCategory.Bull) * Utils.GetRandDouble(min2, 1.0, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Steel, dictionary2.get_Item(enumMaterialCategory.Steel) * Utils.GetRandDouble(min2, 1.0, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bauxite, dictionary2.get_Item(enumMaterialCategory.Bauxite) * Utils.GetRandDouble(min2, 1.0, 0.01, 2));
				}
			}
			else if (tankerNum >= materialLimitTankerNum)
			{
				double min3 = (!randMaxFlag) ? 1.0 : 1.3;
				dictionary2.set_Item(enumMaterialCategory.Fuel, (double)this.Material_1 * Utils.GetRandDouble(min3, 1.3, 0.1, 1));
				dictionary2.set_Item(enumMaterialCategory.Bull, (double)this.Material_2 * Utils.GetRandDouble(min3, 1.3, 0.1, 1));
				dictionary2.set_Item(enumMaterialCategory.Steel, (double)this.Material_3 * Utils.GetRandDouble(min3, 1.3, 0.1, 1));
				dictionary2.set_Item(enumMaterialCategory.Bauxite, (double)this.Material_4 * Utils.GetRandDouble(min3, 1.3, 0.1, 1));
				if (num == 0)
				{
					min3 = ((!randMaxFlag) ? 0.5 : 0.85);
					dictionary2.set_Item(enumMaterialCategory.Fuel, dictionary2.get_Item(enumMaterialCategory.Fuel) * Utils.GetRandDouble(min3, 0.85, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bull, dictionary2.get_Item(enumMaterialCategory.Bull) * Utils.GetRandDouble(min3, 0.85, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Steel, dictionary2.get_Item(enumMaterialCategory.Steel) * Utils.GetRandDouble(min3, 0.85, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bauxite, dictionary2.get_Item(enumMaterialCategory.Bauxite) * Utils.GetRandDouble(min3, 0.85, 0.01, 2));
				}
				int num5 = num * 2;
				if (num5 < tankerNum && num < 6)
				{
					min3 = ((!randMaxFlag) ? 0.75 : 0.95);
					dictionary2.set_Item(enumMaterialCategory.Fuel, dictionary2.get_Item(enumMaterialCategory.Fuel) * Utils.GetRandDouble(min3, 0.95, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bull, dictionary2.get_Item(enumMaterialCategory.Bull) * Utils.GetRandDouble(min3, 0.95, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Steel, dictionary2.get_Item(enumMaterialCategory.Steel) * Utils.GetRandDouble(min3, 0.95, 0.01, 2));
					dictionary2.set_Item(enumMaterialCategory.Bauxite, dictionary2.get_Item(enumMaterialCategory.Bauxite) * Utils.GetRandDouble(min3, 0.95, 0.01, 2));
				}
			}
			double num6 = 1.0;
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			if (difficult == DifficultKind.SHI)
			{
				num6 = 1.0;
			}
			else if (difficult == DifficultKind.KOU)
			{
				num6 = 2.0;
			}
			else if (difficult == DifficultKind.OTU)
			{
				num6 = 2.5;
			}
			else if (difficult == DifficultKind.HEI)
			{
				num6 = 3.0;
			}
			else if (difficult == DifficultKind.TEI)
			{
				num6 = 4.0;
			}
			using (Dictionary<enumMaterialCategory, double>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, double> current = enumerator.get_Current();
					double num7 = (current.get_Key() != enumMaterialCategory.Bauxite) ? 1.0 : 0.65;
					double num8 = current.get_Value() * num6 * num7;
					int num9 = (int)(Math.Ceiling(num8) / 5.0);
					int num10 = 5 * num9;
					Dictionary<enumMaterialCategory, int> dictionary3;
					Dictionary<enumMaterialCategory, int> expr_605 = dictionary3 = addValues;
					enumMaterialCategory key;
					enumMaterialCategory expr_60F = key = current.get_Key();
					int num11 = dictionary3.get_Item(key);
					expr_605.set_Item(expr_60F, num11 + num10);
				}
			}
		}
	}
}
