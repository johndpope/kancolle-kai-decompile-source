using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_const : Model_Base
	{
		private MstConstDataIndex _id;

		private int _int_value;

		private static string _tableName = "mst_const";

		public MstConstDataIndex Id
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

		public int Int_value
		{
			get
			{
				return this._int_value;
			}
			private set
			{
				this._int_value = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_const._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = (MstConstDataIndex)int.Parse(element.Element("Id").get_Value());
			this.Int_value = int.Parse(element.Element("Int_value").get_Value());
		}
	}
}
