using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class SearchUsePlane
	{
		public int Rid;

		public List<int> MstIds;

		public SearchUsePlane()
		{
		}

		public SearchUsePlane(int rid, List<int> mst_ids)
		{
			this.Rid = rid;
			this.MstIds = mst_ids;
		}
	}
}
