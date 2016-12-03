using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_furniture", Namespace = "")]
	public class Mem_furniture : Model_Base
	{
		[DataMember]
		private int _rid;

		private static string _tableName = "mem_furniture";

		public int Rid
		{
			get
			{
				return this._rid;
			}
			private set
			{
				this._rid = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_furniture._tableName;
			}
		}

		public Mem_furniture()
		{
		}

		public Mem_furniture(int mst_id)
		{
			this.Rid = mst_id;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
		}
	}
}
