using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class _TankerManager
	{
		private Dictionary<int, List<Mem_tanker>> _data;

		public _TankerManager()
		{
			this.Update();
		}

		public int GetAllCount()
		{
			int num = 0;
			using (Dictionary<int, List<Mem_tanker>>.ValueCollection.Enumerator enumerator = this._data.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_tanker> current = enumerator.get_Current();
					num += current.get_Count();
				}
			}
			return num;
		}

		public int GetMoveCount()
		{
			int num = 0;
			using (Dictionary<int, List<Mem_tanker>>.ValueCollection.Enumerator enumerator = this._data.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_tanker> current = enumerator.get_Current();
					num += current.FindAll((Mem_tanker tanker) => tanker.IsBlingShip()).get_Count();
				}
			}
			return num;
		}

		public AreaTankerModel GetCounts(int area_id)
		{
			List<Mem_tanker> tankers = this._GetTankers(area_id);
			Mst_maparea mst_maparea;
			if (Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, ref mst_maparea))
			{
				return new AreaTankerModel(area_id, tankers, 30, mst_maparea.GetUIMaterialLimitTankerNum());
			}
			return null;
		}

		public AreaTankerModel GetCounts()
		{
			List<Mem_tanker> tankers = this._GetTankers(0);
			return new AreaTankerModel(0, tankers, this.GetAllCount(), 0);
		}

		public bool Update()
		{
			Api_Result<Dictionary<int, List<Mem_tanker>>> api_Result = new Api_get_Member().Tanker();
			if (api_Result.state == Api_Result_State.Success)
			{
				this._data = api_Result.data;
				return true;
			}
			this._data = new Dictionary<int, List<Mem_tanker>>();
			return false;
		}

		private List<Mem_tanker> _GetTankers(int area_id)
		{
			List<Mem_tanker> result;
			if (this._data.TryGetValue(area_id, ref result))
			{
				return result;
			}
			return new List<Mem_tanker>();
		}
	}
}
