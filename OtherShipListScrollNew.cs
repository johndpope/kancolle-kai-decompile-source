using Common.Enum;
using KCV;
using KCV.Supply;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OtherShipListScrollNew : UIScrollList<ShipModel, OtherShipListChildNew>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	[SerializeField]
	private Camera touchEventCamera;

	private Vector3 SHOW_LOCAL_POSITION = new Vector3(-163f, 115f);

	private Vector3 HIDE_LOCAL_POSITION = new Vector3(-1000f, 115f);

	private Dictionary<ShipModel, bool> selectedModels;

	private KeyControl mKeyController;

	private bool mCallFirstInitialized;

	private ShipModel nextFocusShipModel;

	public int GetShipCount()
	{
		return this.mModels.Length;
	}

	public void StartState()
	{
		this.mUIShipSortButton.SetCheckClicableDelegate(new UIShipSortButton.CheckClickable(this.CheckSortButtonClickable));
		this.mUIShipSortButton.SetClickable(true);
		base.KillScrollAnimation();
		if (this.nextFocusShipModel == null)
		{
			base.HeadFocus();
		}
		else
		{
			base.ChangePageFromModel(this.nextFocusShipModel);
		}
		base.StartControl();
	}

	protected override void OnUpdate()
	{
		if (this.mKeyController != null && base.mState == UIScrollList<ShipModel, OtherShipListChildNew>.ListState.Waiting)
		{
			if (this.mKeyController.keyState.get_Item(12).down)
			{
				base.NextFocus();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				base.PrevFocus();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				base.PrevPageOrHeadFocus();
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				base.NextPageOrTailFocus();
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				base.Select();
			}
			else if (this.mKeyController.keyState.get_Item(3).down)
			{
				this.mUIShipSortButton.OnClickSortButton();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.mKeyController = null;
			}
		}
	}

	protected override void OnChangedFocusView(OtherShipListChildNew focusToView)
	{
		if (this.mModels == null)
		{
			return;
		}
		if (this.mModels.Length <= 0)
		{
			return;
		}
		if (base.mState == UIScrollList<ShipModel, OtherShipListChildNew>.ListState.Waiting)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_014);
			if (this.mModels == null)
			{
				return;
			}
			if (this.mModels.Length <= 0)
			{
				return;
			}
			CommonPopupDialog.Instance.StartPopup(this.mCurrentFocusView.GetRealIndex() + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
		}
	}

	protected override void OnSelect(OtherShipListChildNew view)
	{
		if (view == null)
		{
			return;
		}
		if (view.shipBanner.IsSelectable() && SupplyMainManager.Instance.IsShipSelectableStatus())
		{
			SupplyMainManager.Instance.change_2_SHIP_SELECT(true);
			this.SwitchSelected(view);
		}
	}

	public void Initialize(KeyControl keyController, ShipModel[] otherShipModels)
	{
		this.selectedModels = new Dictionary<ShipModel, bool>();
		base.SetSwipeEventCamera(this.touchEventCamera);
		if (!this.mCallFirstInitialized)
		{
			this.mUIShipSortButton.SetSortKey(SortKey.LEVEL);
			this.mCallFirstInitialized = true;
		}
		this.mUIShipSortButton.Initialize(otherShipModels);
		this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSortedShipsListener));
		this.mUIShipSortButton.ReSort();
		this.mKeyController = keyController;
		List<ShipModel> list = new List<ShipModel>();
		SupplyMainManager.Instance.SupplyManager.Ships.ForEach(delegate(ShipModel e)
		{
			if (e != null)
			{
				list.Add(e);
			}
		});
		ShipModel[] array = list.ToArray();
		this.mUIShipSortButton.SetClickable(true);
		base.get_gameObject().SetActive(false);
		base.get_gameObject().SetActive(true);
	}

	private void OnSortedShipsListener(ShipModel[] refreshOtherShipModels)
	{
		base.Refresh(refreshOtherShipModels, true);
	}

	public void SwitchSelected(OtherShipListChildNew view)
	{
		ShipModel ship = view.shipBanner.Ship;
		if (view.shipBanner.SwitchSelected())
		{
			this.selectedModels.set_Item(ship, true);
		}
		else
		{
			this.selectedModels.Remove(ship);
		}
		SupplyMainManager.Instance.UpdateRightPain();
	}

	public List<ShipModel> getSeletedModelList()
	{
		List<ShipModel> list = new List<ShipModel>();
		for (int i = 0; i < this.mModels.Length; i++)
		{
			ShipModel shipModel = this.mModels[i];
			if (this.selectedModels.ContainsKey(shipModel))
			{
				list.Add(shipModel);
			}
		}
		return list;
	}

	public bool isSelected(ShipModel model)
	{
		return model != null && this.selectedModels.ContainsKey(model);
	}

	public void Show(bool animation = true)
	{
		base.get_transform().set_localPosition(this.SHOW_LOCAL_POSITION);
	}

	public void Hide(bool animation = true)
	{
		base.KillScrollAnimation();
		this.LockControl();
		base.get_transform().set_localPosition(this.HIDE_LOCAL_POSITION);
		this.mKeyController = null;
	}

	public void ProcessRecoveryAnimation()
	{
		Debug.Log("回復アニメーション");
		for (int i = 0; i < this.mViews.Length; i++)
		{
			if (this.isSelected(this.mViews[i].GetModel()))
			{
				this.mViews[i].shipBanner.ProcessRecoveryAnimation();
			}
		}
	}

	internal void LockControl()
	{
		this.mUIShipSortButton.SetClickable(false);
		base.KillScrollAnimation();
		base.LockControl();
	}

	public void StartControl()
	{
		this.mUIShipSortButton.SetClickable(true);
		this.mKeyController.ClearKeyAll();
		this.mKeyController.firstUpdate = true;
		base.StartControl();
	}

	private bool CheckSortButtonClickable()
	{
		return base.mState == UIScrollList<ShipModel, OtherShipListChildNew>.ListState.Waiting;
	}

	internal void MemoryNextFocus()
	{
		this.nextFocusShipModel = null;
		if (this.mCurrentFocusView == null || this.mCurrentFocusView.GetModel() == null)
		{
			return;
		}
		int num = Array.IndexOf<ShipModel>(this.mModels, this.mCurrentFocusView.GetModel());
		if (0 < num)
		{
			this.nextFocusShipModel = this.mModels[num - 1];
		}
		else
		{
			this.nextFocusShipModel = null;
		}
	}
}
