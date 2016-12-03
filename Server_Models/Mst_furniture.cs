using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_furniture : Model_Base
	{
		private int _id;

		private int _type;

		private int _no;

		private string _title;

		private string _description;

		private int _rarity;

		private int _price;

		private int _season;

		private int _sale_from;

		private int _sale_to;

		private static string _tableName = "mst_furniture";

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

		public string Title
		{
			get
			{
				return this._title;
			}
			private set
			{
				this._title = value;
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

		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			private set
			{
				this._rarity = value;
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

		public int Season
		{
			get
			{
				return this._season;
			}
			private set
			{
				this._season = value;
			}
		}

		public int Sale_from
		{
			get
			{
				return this._sale_from;
			}
			private set
			{
				this._sale_from = value;
			}
		}

		public int Sale_to
		{
			get
			{
				return this._sale_to;
			}
			private set
			{
				this._sale_to = value;
			}
		}

		public int Saleflg
		{
			get
			{
				return this.IsSale(Comm_UserDatas.Instance.User_turn.GetDateTime().get_Month());
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_furniture._tableName;
			}
		}

		public static List<Mst_furniture> getSaleFurnitureList()
		{
			int month = Comm_UserDatas.Instance.User_turn.GetDateTime().get_Month();
			return Enumerable.ToList<Mst_furniture>(Enumerable.OrderBy<Mst_furniture, int>(Enumerable.Where<Mst_furniture>(Mst_DataManager.Instance.Mst_furniture.get_Values(), (Mst_furniture data) => data.IsSale(month) == 1), (Mst_furniture data) => data.Id));
		}

		public int IsSale(int nowMonth)
		{
			int result = 0;
			if (this.Sale_from > this.Sale_to)
			{
				result = ((this.Sale_from <= nowMonth || this.Sale_to >= nowMonth) ? 1 : 0);
			}
			else if (nowMonth >= this.Sale_from && nowMonth <= this.Sale_to)
			{
				result = 1;
			}
			return result;
		}

		public bool IsRequireWorker()
		{
			return this.Price >= 2000 && this.Price < 20000;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Type = int.Parse(element.Element("Type").get_Value());
			this.No = int.Parse(element.Element("No").get_Value());
			this.Title = element.Element("Title").get_Value();
			this.Description = string.Empty;
			this.Rarity = int.Parse(element.Element("Rarity").get_Value());
			this.Price = int.Parse(element.Element("Price").get_Value());
			this.Season = int.Parse(element.Element("Season").get_Value());
			this.Sale_from = int.Parse(element.Element("Sale_from").get_Value());
			this.Sale_to = int.Parse(element.Element("Sale_to").get_Value());
		}
	}
}
