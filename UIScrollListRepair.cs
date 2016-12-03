using Common.Enum;
using KCV;
using KCV.Port.Repair;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIScrollListRepair : UIScrollList<ShipModel, UIScrollListRepairChild>
{
	private KeyControl mKeyController;

	[SerializeField]
	private UIShipSortButton SortButton;

	[SerializeField]
	private Transform noShips;

	private int _NowShipLength;

	private string _before_s;

	private repair _rep;

	private bool mCallFirstInitialze;

	private Action mOnBackListener;

	private Action<UIScrollListRepairChild> mOnUIScrollListRepairSelectedListener;

	public KeyControl keyController
	{
		get
		{
			return this.mKeyController;
		}
	}

	protected override void OnUpdate()
	{
		if (this.mKeyController != null && base.mState == UIScrollList<ShipModel, UIScrollListRepairChild>.ListState.Waiting && this._rep.now_mode() == 2)
		{
			if (this.mKeyController.keyState.get_Item(12).down)
			{
				base.NextFocus();
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				base.PrevFocus();
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
				if (this._NowShipLength == 0)
				{
					return;
				}
				base.Select();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.mKeyController = null;
			}
			else if (this.mKeyController.keyState.get_Item(3).down)
			{
				this.SortButton.OnClickSortButton();
			}
		}
	}

	protected override void OnChangedFocusView(UIScrollListRepairChild focusToView)
	{
		if (focusToView == null || this._rep.now_mode() != 2)
		{
			return;
		}
		if (base.mState == UIScrollList<ShipModel, UIScrollListRepairChild>.ListState.Waiting)
		{
			string text = (focusToView.GetRealIndex() + 1).ToString() + "/" + this.mModels.Length.ToString();
			if (this._before_s != text)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this._before_s = text;
			if (CommonPopupDialog.Instance != null)
			{
				CommonPopupDialog.Instance.StartPopup(text);
			}
		}
	}

	protected override void OnSelect(UIScrollListRepairChild view)
	{
		if (view == null)
		{
			return;
		}
		if (this.mOnUIScrollListRepairSelectedListener != null)
		{
			this.mOnUIScrollListRepairSelectedListener.Invoke(view);
		}
	}

	internal void SetCamera(Camera camera)
	{
		base.SetSwipeEventCamera(camera);
	}

	internal void Initialize(ShipModel[] ships)
	{
		this._NowShipLength = ships.Length;
		this.noShips.get_transform().set_localScale((this._NowShipLength != 0) ? Vector3.get_zero() : Vector3.get_one());
		this._before_s = string.Empty;
		this._rep = base.get_gameObject().get_transform().get_parent().get_parent().get_parent().GetComponent<repair>();
		this.mKeyController.ClearKeyAll();
		if (this.SortButton.get_isActiveAndEnabled())
		{
			this.SortButton.Initialize(ships);
			this.SortButton.SetClickable(true);
			if (!this.mCallFirstInitialze)
			{
				this.SortButton.SetSortKey(SortKey.DAMAGE);
				this.mCallFirstInitialze = true;
			}
			this.SortButton.SetCheckClicableDelegate(new UIShipSortButton.CheckClickable(this.CheckSortButtonClickable));
			this.SortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSorted));
			this.SortButton.ReSort();
		}
	}

	private void OnSorted(ShipModel[] ships)
	{
		if (ships.Length == 0)
		{
			return;
		}
		base.Refresh(ships, true);
		base.ChangeImmediateContentPosition(UIScrollList<ShipModel, UIScrollListRepairChild>.ContentDirection.Hell);
	}

	private bool CheckSortButtonClickable()
	{
		return base.mState == UIScrollList<ShipModel, UIScrollListRepairChild>.ListState.Waiting;
	}

	internal void SetOnBackListener(Action OnUIScrollListRepairBackListener)
	{
		this.mOnBackListener = OnUIScrollListRepairBackListener;
	}

	internal void SetOnSelectedListener(Action<UIScrollListRepairChild> OnUIScrollListRepairSelectedListener)
	{
		this.mOnUIScrollListRepairSelectedListener = OnUIScrollListRepairSelectedListener;
	}

	internal void SetKeyController(KeyControl dockSelectController)
	{
		this.mKeyController = dockSelectController;
	}

	public void StartControl()
	{
		base.StartControl();
	}

	public void LockControl()
	{
		base.LockControl();
	}

	public void ResumeControl()
	{
		base.StartControl();
	}
}
