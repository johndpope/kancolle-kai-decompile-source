using Server_Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_mapcomp", Namespace = "")]
	public class Mem_mapcomp : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private int _mapinfo_no;

		[DataMember]
		private HashSet<int> _no;

		private static string _tableName = "mem_mapcomp";

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

		public int Mapinfo_no
		{
			get
			{
				return this._mapinfo_no;
			}
			private set
			{
				this._mapinfo_no = value;
			}
		}

		public HashSet<int> No
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

		public static string tableName
		{
			get
			{
				return Mem_mapcomp._tableName;
			}
		}

		public Mem_mapcomp()
		{
			this.No = new HashSet<int>();
		}

		public Mem_mapcomp(int mapinfo_id, int maparea_id, int mapinfo_no) : this()
		{
			this.Rid = mapinfo_id;
			this.Maparea_id = maparea_id;
			this.Mapinfo_no = mapinfo_no;
		}

		public void Insert()
		{
			if (Comm_UserDatas.Instance.User_mapcomp.ContainsKey(this.Rid))
			{
				return;
			}
			Comm_UserDatas.Instance.User_mapcomp.Add(this.Rid, this);
		}

		public void AddPassCell(int cell_no)
		{
			this.No.Add(cell_no);
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Maparea_id = int.Parse(element.Element("_maparea_id").get_Value());
			this.Mapinfo_no = int.Parse(element.Element("_mapinfo_no").get_Value());
			using (IEnumerator<XElement> enumerator = element.Element("_no").Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					this.No.Add(int.Parse(current.get_Value()));
				}
			}
		}
	}
}
