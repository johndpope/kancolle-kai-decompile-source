using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_useitem : Model_Base
	{
		public const int __USEITEM_DOCKKEY__ = 49;

		public const int __USEITEM_KAGUSYOKUNIN__ = 52;

		public const int __USEITEM_BOKOKAKUCHO__ = 53;

		public const int __USEITEM_YUBIWA__ = 55;

		public const int __USEITEM_SEKKEISHO__ = 58;

		public const int __USEITEM_SHIREIBU__ = 63;

		private int _id;

		private int _usetype;

		private int _category;

		private string _name;

		private string _description;

		private string _description2;

		private int _price;

		private static string _tableName = "mst_useitem";

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

		public int Usetype
		{
			get
			{
				return this._usetype;
			}
			private set
			{
				this._usetype = value;
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

		public string Description
		{
			get
			{
				return this._description;
			}
			private set
			{
				this._description = value;
			}
		}

		public string Description2
		{
			get
			{
				return this._description2;
			}
			private set
			{
				this._description2 = value;
			}
		}

		public int Price
		{
			get
			{
				return this._price;
			}
			private set
			{
				this._price = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_useitem._tableName;
			}
		}

		public int GetItemExchangeNum(ItemExchangeKinds exchange_type)
		{
			int result;
			if (exchange_type == ItemExchangeKinds.NONE)
			{
				result = 1;
			}
			else if (exchange_type == ItemExchangeKinds.PLAN && this.Id == 57)
			{
				result = 4;
			}
			else if (exchange_type == ItemExchangeKinds.REMODEL && this.Id == 57)
			{
				result = 1;
			}
			else if (exchange_type == ItemExchangeKinds.PRESENT_MATERIAL && this.Id == 60)
			{
				result = 1;
			}
			else if (exchange_type == ItemExchangeKinds.PRESENT_IRAKO && this.Id == 60)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Usetype = int.Parse(element.Element("Usetype").get_Value());
			this.Category = int.Parse(element.Element("Category").get_Value());
			this.Name = element.Element("Name").get_Value();
			this.Description = element.Element("Description").get_Value();
			this.Description2 = element.Element("Description2").get_Value();
			this.Price = int.Parse(element.Element("Price").get_Value());
		}
	}
}
