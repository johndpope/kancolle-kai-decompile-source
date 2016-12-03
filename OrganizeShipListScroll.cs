using Common.Enum;
using KCV;
using KCV.Organize;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

public class OrganizeShipListScroll : UIScrollListParent<ShipModel, OrganizeShipListChild>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	private void Start()
	{
		this.mUIShipSortButton.SetSortKey(SortKey.UNORGANIZED);
		this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSorted));
	}

	public void Init(ShipModel[] models)
	{
		this.mUIShipSortButton.InitializeForOrganize(models);
		this.mUIShipSortButton.SetClickable(true);
		this.mUIShipSortButton.ReSort();
	}

	public void SwitchFocusShipLockState()
	{
		this.ViewFocus.SwitchShipLockState();
	}

	protected override void OnAction(ActionType actionType, UIScrollListParent<ShipModel, OrganizeShipListChild> calledObject, OrganizeShipListChild actionChild)
	{
		base.OnAction(actionType, calledObject, actionChild);
		if (actionType != ActionType.OnChangeFocus)
		{
			if (actionType != ActionType.OnChangeFirstFocus)
			{
			}
		}
		else
		{
			if (0 < base.GetModelSize())
			{
				CommonPopupDialog.Instance.StartPopup(string.Format("{0}/{1}", actionChild.SortIndex + 1, base.GetModelSize()), 0, CommonPopupDialogMessage.PlayType.Short);
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
	}

	private void OnSorted(ShipModel[] sortedShipModels)
	{
		base.Initialize(sortedShipModels);
	}

	protected override void OnKeyPressTriangle()
	{
		base.OnKeyPressTriangle();
		this.mUIShipSortButton.OnClickSortButton();
	}

	private void OnDestroy()
	{
		this.mUIShipSortButton = null;
	}
}
