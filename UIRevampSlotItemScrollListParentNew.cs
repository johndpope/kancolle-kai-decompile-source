using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIRevampSlotItemScrollListParentNew : UIScrollList<SlotitemModel, UIRevampSlotItemScrollListChildNew>
{
	[SerializeField]
	private UIButton mOverlayBtn2;

	private KeyControl mKEyController;

	private Action mOnBackListener;

	private Action<UIRevampSlotItemScrollListChildNew> mOnSelectedSlotItemListener;

	public void Initialize(SlotitemModel[] models)
	{
		base.Initialize(models);
		base.ChangeImmediateContentPosition(UIScrollList<SlotitemModel, UIRevampSlotItemScrollListChildNew>.ContentDirection.Hell);
		base.HeadFocus();
	}

	internal void SetCamera(Camera cameraTouchEventCatch)
	{
		base.SetSwipeEventCamera(cameraTouchEventCatch);
	}

	internal KeyControl GetKeyController()
	{
		if (this.mKEyController == null)
		{
			this.mKEyController = new KeyControl(0, 0, 0.4f, 0.1f);
		}
		return this.mKEyController;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		this.mOnBackListener = onBackListener;
	}

	private void Back()
	{
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke();
		}
	}

	public void SetOnSelectedSlotItemListener(Action<UIRevampSlotItemScrollListChildNew> onSelectedSlotItemListener)
	{
		this.mOnSelectedSlotItemListener = onSelectedSlotItemListener;
	}

	protected override void OnSelect(UIRevampSlotItemScrollListChildNew view)
	{
		if (this.mOnSelectedSlotItemListener != null)
		{
			this.mOnSelectedSlotItemListener.Invoke(view);
		}
	}

	private void Update()
	{
		if (this.mKEyController != null && base.mState == UIScrollList<SlotitemModel, UIRevampSlotItemScrollListChildNew>.ListState.Waiting)
		{
			if (this.mKEyController.IsUpDown())
			{
				base.PrevFocus();
			}
			else if (this.mKEyController.IsDownDown())
			{
				base.NextFocus();
			}
			else if (this.mKEyController.IsLeftDown())
			{
				base.PrevPageOrHeadFocus();
			}
			else if (this.mKEyController.IsRightDown())
			{
				base.NextPageOrTailFocus();
			}
			else if (this.mKEyController.IsMaruDown())
			{
				base.Select();
			}
			else if (this.mKEyController.IsBatuDown())
			{
				this.Back();
			}
		}
	}

	protected override void OnChangedFocusView(UIRevampSlotItemScrollListChildNew focusToView)
	{
		if (0 < this.mModels.Length && base.mState == UIScrollList<SlotitemModel, UIRevampSlotItemScrollListChildNew>.ListState.Waiting && this.mCurrentFocusView != null)
		{
			int realIndex = this.mCurrentFocusView.GetRealIndex();
			CommonPopupDialog.Instance.StartPopup(realIndex + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Long);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
	}

	internal void StartControl()
	{
		base.StartControl();
	}

	public UIButton GetOverlayBtn2()
	{
		return this.mOverlayBtn2;
	}

	protected override void OnCallDestroy()
	{
		this.mOverlayBtn2 = null;
		this.mKEyController = null;
		this.mOnBackListener = null;
		this.mOnSelectedSlotItemListener = null;
	}
}
