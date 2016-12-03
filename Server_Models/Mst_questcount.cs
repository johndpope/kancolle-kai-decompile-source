using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_questcount : Model_Base
	{
		private int _id;

		private HashSet<int> _counter_id;

		private Dictionary<int, int> _clear_num;

		private Dictionary<int, bool> _reset_flag;

		private static string _tableName = "mst_questcount";

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

		public HashSet<int> Counter_id
		{
			get
			{
				return this._counter_id;
			}
			private set
			{
				this._counter_id = value;
			}
		}

		public Dictionary<int, int> Clear_num
		{
			get
			{
				return this._clear_num;
			}
			private set
			{
				this._clear_num = value;
			}
		}

		public Dictionary<int, bool> Reset_flag
		{
			get
			{
				return this._reset_flag;
			}
			private set
			{
				this._reset_flag = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_questcount._tableName;
			}
		}

		public Mst_questcount()
		{
			this.Counter_id = new HashSet<int>();
			this.Clear_num = new Dictionary<int, int>();
			this.Reset_flag = new Dictionary<int, bool>();
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Id = int.Parse(element.Element("Id").get_Value());
			string[] array = element.Element("Counter_id").get_Value().Split(new char[]
			{
				c
			});
			int[] array2 = Array.ConvertAll<string, int>(array, (string x) => int.Parse(x));
			string[] array3 = element.Element("Clear_num").get_Value().Split(new char[]
			{
				c
			});
			int[] array4 = Array.ConvertAll<string, int>(array3, (string x) => int.Parse(x));
			byte[] array5 = null;
			if (element.Element("Reset_flag") != null)
			{
				string[] array6 = element.Element("Reset_flag").get_Value().Split(new char[]
				{
					c
				});
				array5 = Array.ConvertAll<string, byte>(array6, (string x) => byte.Parse(x));
			}
			for (int i = 0; i < array2.Length; i++)
			{
				int num = array2[i];
				this.Counter_id.Add(num);
				if (i < array4.Length)
				{
					this.Clear_num.Add(num, array4[i]);
				}
				if (array5 != null)
				{
					bool flag = array5[i] != 0;
					this.Reset_flag.Add(num, flag);
				}
			}
		}
	}
}
