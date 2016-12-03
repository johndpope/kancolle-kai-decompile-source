using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.SaveManager
{
	public class SaveTarget
	{
		public Type ClassType;

		public object Data;

		public string TableName;

		public bool IsCollection;

		public SaveTarget(Type type, object data, string table_name)
		{
			this.ClassType = type;
			this.Data = data;
			this.TableName = table_name;
			this.IsCollection = false;
		}

		public SaveTarget(Type type, IList data, string table_name)
		{
			this.ClassType = type;
			this.Data = data;
			this.TableName = table_name;
			this.IsCollection = true;
		}

		public SaveTarget(IEnumerable<Mem_ship> ship_datas)
		{
			this.ClassType = typeof(List<Mem_shipBase>);
			List<Mem_shipBase> list = new List<Mem_shipBase>();
			using (IEnumerator<Mem_ship> enumerator = ship_datas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Mem_shipBase mem_shipBase = new Mem_shipBase(current);
					list.Add(mem_shipBase);
				}
			}
			this.Data = list;
			this.TableName = Mem_ship.tableName;
			this.IsCollection = true;
		}
	}
}
