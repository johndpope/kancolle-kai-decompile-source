using System;

namespace Server_Models
{
	public interface IRebellionPointOperator
	{
		void AddRebellionPoint(int area_id, int addNum);

		void SubRebellionPoint(int area_id, int subNum);
	}
}
