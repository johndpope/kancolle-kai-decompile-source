using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class User_BookFmt<T> where T : IBookData, new()
	{
		public int IndexNo;

		public List<int> Ids;

		public List<List<int>> State;

		public T Detail;

		public User_BookFmt(int table_id, List<int> ids, List<List<int>> state, string info, string cname, object hosoku)
		{
			this.Detail = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			this.IndexNo = this.Detail.GetSortNo(table_id);
			this.Ids = ids;
			this.State = state;
			this.Detail.SetBookData(table_id, info, cname, hosoku);
		}
	}
}
