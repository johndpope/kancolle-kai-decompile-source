using local.models;
using System;
using UnityEngine;

namespace KCV.Supply
{
	public class UISupplyDeckShipBanner : UISupplyCommonShipBanner
	{
		[SerializeField]
		private CommonShipBanner _shipBanner;

		[SerializeField]
		private UITexture _shutterL;

		[SerializeField]
		private UITexture _shutterR;

		public void Init(Vector3 pos)
		{
			base.Init();
			base.get_transform().set_localPosition(pos);
		}

		public override void SetBanner(ShipModel ship, int idx)
		{
			base.SetBanner(ship, idx);
			if (base.get_enabled())
			{
				this._shipBanner.SetActive(true);
				this._shipBanner.SetShipData(ship);
			}
		}

		public override void SetEnabled(bool enabled)
		{
			base.SetEnabled(enabled);
			this._shipBanner.SetActive(enabled);
			this._shutterL.SetActive(!enabled);
			this._shutterR.SetActive(!enabled);
		}

		public void OnClick()
		{
			if (base.IsSelectable() && SupplyMainManager.Instance.IsShipSelectableStatus())
			{
				SupplyMainManager.Instance.change_2_SHIP_SELECT(true);
				SupplyMainManager.Instance._shipBannerContainer.UpdateCurrentItem(this.idx);
				SupplyMainManager.Instance._shipBannerContainer.SwitchCurrentSelected();
			}
		}
	}
}
