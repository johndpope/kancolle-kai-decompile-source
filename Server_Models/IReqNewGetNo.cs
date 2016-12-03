using System;

namespace Server_Models
{
	internal interface IReqNewGetNo
	{
		int GetSortNo();

		void ChangeSortNo(int no);
	}
}
