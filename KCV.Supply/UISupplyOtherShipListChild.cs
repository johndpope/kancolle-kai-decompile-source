using KCV.View.Scroll;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Supply
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class UISupplyOtherShipListChild : UIScrollListChildNew<ShipModel, UISupplyOtherShipListChild>
	{
		[SerializeField]
		public UISupplyOtherShipBanner shipBanner;

		protected override IEnumerator InitializeCoroutine(ShipModel ship)
		{
			base.set_enabled(true);
			this.shipBanner.Init();
			this.shipBanner.SetBanner(ship, base.modelIndex);
			this.shipBanner.Select(SupplyMainManager.Instance._otherListParent.isSelected(ship));
			return base.InitializeCoroutine(base.model);
		}
	}
}
