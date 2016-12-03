using KCV;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIMissionShipBanner : MonoBehaviour
{
	[SerializeField]
	private CommonShipBanner mCommonShipBanner;

	[SerializeField]
	private CommonShipSupplyState mCommonShipSupplyState;

	[SerializeField]
	private UILabel mLabel_ShipPosition;

	private UIPanel mPanelThis;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 0.01f;
	}

	public void Initialize(int position, ShipModel shipModel)
	{
		this.mLabel_ShipPosition.text = position.ToString();
		this.mCommonShipBanner.SetShipData(shipModel);
		if (this.mCommonShipSupplyState != null)
		{
			this.mCommonShipSupplyState.setSupplyState(shipModel);
		}
	}

	public void Show()
	{
		this.mPanelThis.alpha = 1f;
	}

	private void OnDestroy()
	{
		this.mCommonShipBanner = null;
		this.mCommonShipSupplyState = null;
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_ShipPosition);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
	}
}
