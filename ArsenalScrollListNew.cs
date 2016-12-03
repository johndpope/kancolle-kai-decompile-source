using Common.Enum;
using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class ArsenalScrollListNew : UIScrollList<ShipModel, ArsenalScrollListChildNew>
{
	[SerializeField]
	private UIShipSortButton mUIShipSortButton;

	[SerializeField]
	private Transform MessageShip;

	private KeyControl mKeyController;

	private bool _isFirst = true;

	private int _before_focus;

	private Action<ArsenalScrollListChildNew> mOnSelectedListener;

	private bool mCallFirstInitialized;

	public ShipModel SelectedShip
	{
		get;
		private set;
	}

	protected override void OnUpdate()
	{
		if (base.mState == UIScrollList<ShipModel, ArsenalScrollListChildNew>.ListState.Waiting && this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(3).down)
			{
				this.mUIShipSortButton.OnClickSortButton();
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				if (this.mModels == null)
				{
					return;
				}
				if (this.mModels.Length <= 0)
				{
					return;
				}
				base.Select();
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				base.PrevPageOrHeadFocus();
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				base.NextPageOrTailFocus();
			}
			else if (this.mKeyController.keyState.get_Item(12).down)
			{
				base.NextFocus();
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				base.PrevFocus();
			}
		}
	}

	public void SetOnSelectedListener(Action<ArsenalScrollListChildNew> onSelectedListener)
	{
		this.mOnSelectedListener = onSelectedListener;
	}

	protected override void OnSelect(ArsenalScrollListChildNew view)
	{
		this.SelectedShip = view.GetModel();
		base.RefreshViews();
		if (this.mOnSelectedListener != null)
		{
			this.mOnSelectedListener.Invoke(view);
		}
	}

	protected override void OnAwake()
	{
		this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSorted));
		ArsenalScrollListChildNew[] mViews = this.mViews;
		for (int i = 0; i < mViews.Length; i++)
		{
			ArsenalScrollListChildNew arsenalScrollListChildNew = mViews[i];
			arsenalScrollListChildNew.SetCheckSelectedDelegate(new ArsenalScrollListChildNew.CheckSelectedDelegate(this.OnCheckSelected));
		}
	}

	private bool OnCheckSelected(ShipModel shipModel)
	{
		return this.SelectedShip != null && shipModel != null && this.SelectedShip.MemId == shipModel.MemId;
	}

	public void Initialize(ShipModel[] ships)
	{
		this._before_focus = 0;
		this._isFirst = true;
		this.SelectedShip = null;
		this.Message(ships);
		if (!this.mCallFirstInitialized)
		{
			this.mUIShipSortButton.SetSortKey(SortKey.LEVEL);
			this.mCallFirstInitialized = true;
		}
		this.mUIShipSortButton.Initialize(ships);
		ShipModel[] models = this.mUIShipSortButton.SortModels(this.mUIShipSortButton.CurrentSortKey);
		base.ChangeImmediateContentPosition(UIScrollList<ShipModel, ArsenalScrollListChildNew>.ContentDirection.Hell);
		base.Initialize(models);
		base.HeadFocus();
	}

	public void SetCamera(Camera camera)
	{
		base.SetSwipeEventCamera(camera);
	}

	public void Refresh(ShipModel[] models)
	{
		this.Message(models);
		this.SelectedShip = null;
		this.mUIShipSortButton.RefreshModels(models);
		this.mModels = this.mUIShipSortButton.SortModels(this.mUIShipSortButton.CurrentSortKey);
		base.RefreshViews();
		if (this.mCurrentFocusView.GetModel() == null && this.mModels.Length != 0)
		{
			base.TailFocus();
		}
		else if (this.mCurrentFocusView.GetRealIndex() == this.mModels.Length - 1)
		{
			base.TailFocus();
		}
		else if (this.mViews_WorkSpace[this.mUserViewCount - 1].GetModel() == null && this.mModels.Length != 0)
		{
			int num = Array.IndexOf<ArsenalScrollListChildNew>(this.mViews_WorkSpace, this.mCurrentFocusView);
			base.TailFocusPage();
			base.ChangeFocusView(this.mViews_WorkSpace[num]);
		}
	}

	public void Message(ShipModel[] models)
	{
		if (models.Length == 0)
		{
			this.MessageShip.set_localScale(Vector3.get_one());
		}
		else
		{
			this.MessageShip.set_localScale(Vector3.get_zero());
		}
	}

	protected override void OnChangedFocusView(ArsenalScrollListChildNew focusToView)
	{
		if (this.mModels == null)
		{
			return;
		}
		if (this.mModels.Length <= 0)
		{
			return;
		}
		if (base.mState == UIScrollList<ShipModel, ArsenalScrollListChildNew>.ListState.Waiting)
		{
			if (!this._isFirst && this._before_focus != focusToView.GetRealIndex())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this._isFirst = false;
			if (this.mModels == null)
			{
				return;
			}
			if (this.mModels.Length <= 0)
			{
				return;
			}
			this._before_focus = focusToView.GetRealIndex();
			CommonPopupDialog.Instance.StartPopup(this.mCurrentFocusView.GetRealIndex() + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
		}
	}

	public UIScrollList<ShipModel, ArsenalScrollListChildNew>.ListState GetCurrentState()
	{
		return base.mState;
	}

	private void OnSorted(ShipModel[] sortedShipModels)
	{
		base.Refresh(sortedShipModels, true);
	}

	public void RefreshViews()
	{
		base.RefreshViews();
	}

	public void LockControl()
	{
		base.LockControl();
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void StartControl()
	{
		base.StartControl();
	}

	public void ResumeControl()
	{
		this.mKeyController.ClearKeyAll();
		this.mKeyController.firstUpdate = true;
		base.StartControl();
	}

	protected override void OnCallDestroy()
	{
		this.mUIShipSortButton = null;
		this.MessageShip = null;
		this.mKeyController = null;
	}

	internal bool HasSelectedShip()
	{
		return this.SelectedShip != null;
	}

	internal void RemoveSelectedShip()
	{
		this.SelectedShip = null;
	}

	internal int GetFocusModelIndex()
	{
		return Array.IndexOf<ShipModel>(this.mModels, this.mCurrentFocusView.GetModel());
	}

	internal void ClearSelected()
	{
		this.RemoveSelectedShip();
		base.RefreshViews();
	}
}
