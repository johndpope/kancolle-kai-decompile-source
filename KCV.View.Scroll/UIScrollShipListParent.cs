using local.models;
using System;

namespace KCV.View.Scroll
{
	public class UIScrollShipListParent : UIScrollListParent<ShipModel, UIScrollShipListChild>
	{
		public void Initialize(ShipModel[] models)
		{
			base.Initialize(models);
		}
	}
}
