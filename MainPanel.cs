using KCV;
using KCV.Remodel;
using local.models;
using System;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
	[SerializeField]
	public UIRemodelParameter[] mUIRemodelParameters;

	[SerializeField]
	private UILabel mLabel_ShipName;

	[SerializeField]
	private UILabel mLabel_Level;

	[SerializeField]
	private UIRemodelLevel mUIRemodelLevel_RemodelLevel;

	[SerializeField]
	private CommonShipBanner mCommonShipBanner_ShipBanner;

	private ShipModel mShipModel;

	public void Initialize(ShipModel shipModel)
	{
		this.mShipModel = shipModel;
		this.mLabel_Level.text = shipModel.Level.ToString();
		this.mLabel_ShipName.text = shipModel.Name;
		this.mCommonShipBanner_ShipBanner.SetShipData(shipModel);
	}
}
