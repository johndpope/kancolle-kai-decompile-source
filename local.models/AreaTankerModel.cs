using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class AreaTankerModel
	{
		private int _area_id;

		private List<Mem_tanker> _tankers;

		private int _max_count;

		private int _req_count;

		public int AreaId
		{
			get
			{
				return this._area_id;
			}
		}

		public AreaTankerModel(int area_id, List<Mem_tanker> tankers, int max_count, int req_count)
		{
			this._area_id = area_id;
			this._tankers = tankers;
			this._max_count = max_count;
			this._req_count = req_count;
		}

		public int GetCount()
		{
			return this._tankers.get_Count() - this.GetCountInMission();
		}

		public int GetCountNoMove()
		{
			return this.GetCount() - this.GetCountMove();
		}

		public int GetCountMove()
		{
			return this._tankers.FindAll((Mem_tanker tanker) => tanker.IsBlingShip()).get_Count();
		}

		public int GetCountInMission()
		{
			return this._tankers.FindAll((Mem_tanker tanker) => tanker.Disposition_status == DispositionStatus.MISSION).get_Count();
		}

		public int GetMaxCount()
		{
			return this._max_count;
		}

		public int GetReqCount()
		{
			return this._req_count;
		}
	}
}
