using local.models;
using System;

namespace KCV.Battle.Production
{
	public class ProdStarShell
	{
		public ProdStarShell(ShipModel_Battle friend, ShipModel_Battle enemy)
		{
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			return true;
		}

		public bool Update()
		{
			return true;
		}

		public void Play(Action callback)
		{
			if (callback != null)
			{
				callback.Invoke();
			}
		}
	}
}
