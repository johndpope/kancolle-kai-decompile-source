using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelModernizationStartConfirmSlot : MonoBehaviour
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		private UIWidget mWidgetThis;

		private ShipModel mBaitShipModel;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0.01f;
		}

		public void Initialize(ShipModel baitShipModel)
		{
			UITexture uITexture = this.mCommonShipBanner.GetUITexture();
			Texture mainTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
			this.mBaitShipModel = baitShipModel;
			if (baitShipModel == null)
			{
				this.mWidgetThis.alpha = 0.01f;
			}
			else
			{
				this.mCommonShipBanner.SetShipData(baitShipModel);
				this.mLabel_Level.text = baitShipModel.Level.ToString();
				this.mWidgetThis.alpha = 1f;
			}
		}

		public CommonShipBanner GetShipBanner()
		{
			return this.mCommonShipBanner;
		}

		public void StopKira()
		{
			this.mCommonShipBanner.StopParticle();
		}

		internal void Release()
		{
			this.mCommonShipBanner = null;
			if (this.mLabel_Level != null)
			{
				this.mLabel_Level.RemoveFromPanel();
			}
			this.mLabel_Level = null;
			if (this.mWidgetThis != null)
			{
				this.mWidgetThis.RemoveFromPanel();
			}
			this.mWidgetThis = null;
			this.mBaitShipModel = null;
		}
	}
}
