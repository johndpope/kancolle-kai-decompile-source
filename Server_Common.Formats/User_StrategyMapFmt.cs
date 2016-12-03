using Common.Enum;
using Server_Models;
using System;

namespace Server_Common.Formats
{
	public class User_StrategyMapFmt
	{
		private bool isActiveArea;

		private Mst_maparea maparea;

		private RebellionState rebellionState;

		public bool IsActiveArea
		{
			get
			{
				return this.isActiveArea;
			}
			set
			{
				this.isActiveArea = value;
			}
		}

		public Mst_maparea Maparea
		{
			get
			{
				return this.maparea;
			}
			set
			{
				this.maparea = value;
			}
		}

		public RebellionState RebellionState
		{
			get
			{
				return this.rebellionState;
			}
			set
			{
				this.rebellionState = value;
			}
		}

		public User_StrategyMapFmt(Mst_maparea mst_maparea, bool flag)
		{
			this.maparea = mst_maparea;
			this.IsActiveArea = flag;
		}
	}
}
