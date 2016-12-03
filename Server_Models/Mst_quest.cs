using Common.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_quest : Model_Base
	{
		private int _id;

		private int _category;

		private int _type;

		private int _torigger;

		private int _sub_torigger;

		private string _name;

		private string _details;

		private int _get_1_type;

		private int _get_1_count;

		private int _get_1_id;

		private int _get_2_type;

		private int _get_2_count;

		private int _get_2_id;

		private int _mat1;

		private int _mat2;

		private int _mat3;

		private int _mat4;

		private static string _tableName = "mst_quest";

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

		public int Category
		{
			get
			{
				return this._category;
			}
			private set
			{
				this._category = value;
			}
		}

		public int Type
		{
			get
			{
				return this._type;
			}
			private set
			{
				this._type = value;
			}
		}

		public int Torigger
		{
			get
			{
				return this._torigger;
			}
			private set
			{
				this._torigger = value;
			}
		}

		public int Sub_torigger
		{
			get
			{
				return this._sub_torigger;
			}
			private set
			{
				this._sub_torigger = value;
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

		public string Details
		{
			get
			{
				return this._details;
			}
			private set
			{
				this._details = value;
			}
		}

		public int Get_1_type
		{
			get
			{
				return this._get_1_type;
			}
			private set
			{
				this._get_1_type = value;
			}
		}

		public int Get_1_count
		{
			get
			{
				return this._get_1_count;
			}
			private set
			{
				this._get_1_count = value;
			}
		}

		public int Get_1_id
		{
			get
			{
				return this._get_1_id;
			}
			private set
			{
				this._get_1_id = value;
			}
		}

		public int Get_2_type
		{
			get
			{
				return this._get_2_type;
			}
			private set
			{
				this._get_2_type = value;
			}
		}

		public int Get_2_count
		{
			get
			{
				return this._get_2_count;
			}
			private set
			{
				this._get_2_count = value;
			}
		}

		public int Get_2_id
		{
			get
			{
				return this._get_2_id;
			}
			private set
			{
				this._get_2_id = value;
			}
		}

		public int Mat1
		{
			get
			{
				return this._mat1;
			}
			private set
			{
				this._mat1 = value;
			}
		}

		public int Mat2
		{
			get
			{
				return this._mat2;
			}
			private set
			{
				this._mat2 = value;
			}
		}

		public int Mat3
		{
			get
			{
				return this._mat3;
			}
			private set
			{
				this._mat3 = value;
			}
		}

		public int Mat4
		{
			get
			{
				return this._mat4;
			}
			private set
			{
				this._mat4 = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_quest._tableName;
			}
		}

		public Dictionary<enumMaterialCategory, int> GetMaterialValues()
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(enumMaterialCategory)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumMaterialCategory enumMaterialCategory = (enumMaterialCategory)((int)enumerator.get_Current());
					dictionary.Add(enumMaterialCategory, 0);
				}
			}
			dictionary.set_Item(enumMaterialCategory.Fuel, this.Mat1);
			dictionary.set_Item(enumMaterialCategory.Bull, this.Mat2);
			dictionary.set_Item(enumMaterialCategory.Steel, this.Mat3);
			dictionary.set_Item(enumMaterialCategory.Bauxite, this.Mat4);
			dictionary.set_Item(enumMaterialCategory.Build_Kit, this.getItemToMaterialBounus(enumMaterialCategory.Build_Kit));
			dictionary.set_Item(enumMaterialCategory.Repair_Kit, this.getItemToMaterialBounus(enumMaterialCategory.Repair_Kit));
			dictionary.set_Item(enumMaterialCategory.Dev_Kit, this.getItemToMaterialBounus(enumMaterialCategory.Dev_Kit));
			dictionary.set_Item(enumMaterialCategory.Revamp_Kit, this.getItemToMaterialBounus(enumMaterialCategory.Revamp_Kit));
			return dictionary;
		}

		public int GetSpointNum()
		{
			int num = 101;
			if (this.Get_1_type == num)
			{
				return this.Get_1_count;
			}
			if (this.Get_2_type == num)
			{
				return this.Get_2_count;
			}
			return 0;
		}

		private int getItemToMaterialBounus(enumMaterialCategory getType)
		{
			int num = 1;
			if (this.Get_1_type == num && getType == (enumMaterialCategory)this.Get_1_id)
			{
				return this.Get_1_count;
			}
			if (this.Get_2_type == num && getType == (enumMaterialCategory)this.Get_2_id)
			{
				return this.Get_2_count;
			}
			return 0;
		}

		public int GetSlotModelChangeId(int type)
		{
			int result = 0;
			if (type == 1 && this.Get_1_type == 15)
			{
				result = this.Get_1_id;
			}
			if (type == 2 && this.Get_2_type == 15)
			{
				result = this.Get_2_id;
			}
			return result;
		}

		public bool IsLevelMax(List<int> mst_slotitemchange)
		{
			return mst_slotitemchange.get_Item(2) == 1;
		}

		public bool IsUseCrew(List<int> mst_slotitemchange)
		{
			return mst_slotitemchange.get_Item(3) == 1;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Category = int.Parse(element.Element("Category").get_Value());
			this.Type = int.Parse(element.Element("Type").get_Value());
			if (element.Element("Torigger") != null)
			{
				string[] array = element.Element("Torigger").get_Value().Split(new char[]
				{
					c
				});
				int[] array2 = Array.ConvertAll<string, int>(array, (string x) => int.Parse(x));
				this.Torigger = array2[0];
				if (array2.Length > 1)
				{
					this.Sub_torigger = array2[1];
				}
			}
			this.Name = element.Element("Name").get_Value();
			this.Details = element.Element("Details").get_Value();
			if (element.Element("Get_1") != null)
			{
				string[] array3 = element.Element("Get_1").get_Value().Split(new char[]
				{
					c
				});
				int[] array4 = Array.ConvertAll<string, int>(array3, (string x) => int.Parse(x));
				this.Get_1_type = array4[0];
				this.Get_1_count = array4[1];
				this.Get_1_id = array4[2];
			}
			if (element.Element("Get_2") != null)
			{
				string[] array5 = element.Element("Get_2").get_Value().Split(new char[]
				{
					c
				});
				int[] array6 = Array.ConvertAll<string, int>(array5, (string x) => int.Parse(x));
				this.Get_2_type = array6[0];
				this.Get_2_count = array6[1];
				this.Get_2_id = array6[2];
			}
			string[] array7 = element.Element("Mat").get_Value().Split(new char[]
			{
				c
			});
			int[] array8 = Array.ConvertAll<string, int>(array7, (string x) => int.Parse(x));
			this.Mat1 = array8[0];
			this.Mat2 = array8[1];
			this.Mat3 = array8[2];
			this.Mat4 = array8[3];
		}
	}
}
