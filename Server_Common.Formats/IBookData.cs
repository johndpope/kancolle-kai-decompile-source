using System;

namespace Server_Common.Formats
{
	public interface IBookData
	{
		void SetBookData(int mst_id, string info, string cname, object hosokuInfo);

		int GetSortNo(int mst_id);
	}
}
