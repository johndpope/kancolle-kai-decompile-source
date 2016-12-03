using Common.Enum;
using KCV;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Linq;
using UnityEngine;

public class UIFurnitureStoreTabList : UIScrollList<FurnitureModel, UIFurnitureStoreTabListChild>
{
	[SerializeField]
	private Transform mTransform_NextButton;

	[SerializeField]
	private Transform mTransform_PrevButton;

	[SerializeField]
	private Transform mTransform_SoldOut;

	private FurnitureKinds mNowCategory = FurnitureKinds.Wall;

	private FurnitureStoreManager mFurnitureStoreManager;

	private KeyControl mKeyController;

	private Action mOnRequestChangeMode;

	private Action mOnBackListener;

	private Action<UIFurnitureStoreTabListChild> mOnSelectedFurnitureListener;

	protected override void OnAwake()
	{
		base.OnAwake();
		UIFurnitureStoreTabListChild[] mViews = this.mViews;
		for (int i = 0; i < mViews.Length; i++)
		{
			UIFurnitureStoreTabListChild uIFurnitureStoreTabListChild = mViews[i];
			uIFurnitureStoreTabListChild.SetOnReleaseRequestFurnitureTextureListener(new Action<Texture>(this.ReleaseRequestFurnitureTextureFromChild));
		}
		this.mTransform_SoldOut.SetActive(false);
	}

	private void ReleaseRequestFurnitureTextureFromChild(Texture texture)
	{
		if (texture == null)
		{
			return;
		}
		int num = 0;
		UIFurnitureStoreTabListChild[] mViews = this.mViews;
		for (int i = 0; i < mViews.Length; i++)
		{
			UIFurnitureStoreTabListChild uIFurnitureStoreTabListChild = mViews[i];
			UITexture furnitureTextureView = uIFurnitureStoreTabListChild.GetFurnitureTextureView();
			if (furnitureTextureView != null && furnitureTextureView.mainTexture != null && furnitureTextureView.mainTexture.Equals(texture))
			{
				num++;
			}
		}
		if (num < 1)
		{
			Resources.UnloadAsset(texture);
		}
	}

	public void ChangeCategory(FurnitureKinds kinds)
	{
		if (this.mNowCategory == kinds)
		{
			return;
		}
		base.KillScrollAnimation();
		this.mNowCategory = kinds;
		FurnitureModel[] models = Enumerable.ToArray<FurnitureModel>(Enumerable.Take<FurnitureModel>(this.mFurnitureStoreManager.GetStoreItem(kinds), 10));
		base.ChangeImmediateContentPosition(UIScrollList<FurnitureModel, UIFurnitureStoreTabListChild>.ContentDirection.Hell);
		base.Refresh(models, true);
		base.HeadFocus();
		base.StopFocusBlink();
	}

	private void Update()
	{
		if (this.mKeyController != null && base.mState == UIScrollList<FurnitureModel, UIFurnitureStoreTabListChild>.ListState.Waiting)
		{
			if (this.mKeyController.IsUpDown())
			{
				base.PrevFocus();
			}
			else if (this.mKeyController.IsDownDown())
			{
				base.NextFocus();
			}
			else if (this.mKeyController.IsLeftDown())
			{
				base.PrevPageOrHeadFocus();
			}
			else if (this.mKeyController.IsRightDown())
			{
				base.NextPageOrTailFocus();
			}
			else if (this.mKeyController.IsBatuDown())
			{
				this.Back();
			}
			else if (this.mKeyController.IsMaruDown())
			{
				base.Select();
			}
		}
	}

	public void Refresh()
	{
		FurnitureModel[] mModels = Enumerable.ToArray<FurnitureModel>(Enumerable.Take<FurnitureModel>(this.mFurnitureStoreManager.GetStoreItem(this.mNowCategory), 10));
		this.mModels = mModels;
		base.RefreshViews();
		if (this.mCurrentFocusView.GetModel() == null && this.mModels.Length != 0)
		{
			base.TailFocus();
		}
		else if (this.mCurrentFocusView.GetRealIndex() == this.mModels.Length - 1)
		{
			base.TailFocus();
		}
		if (this.mModels.Length == 0)
		{
			this.mTransform_NextButton.SetActive(false);
			this.mTransform_PrevButton.SetActive(false);
			this.mTransform_SoldOut.SetActive(true);
		}
		else
		{
			this.mTransform_NextButton.SetActive(false);
			this.mTransform_PrevButton.SetActive(false);
			this.mTransform_SoldOut.SetActive(false);
		}
	}

	protected override bool EqualsModel(FurnitureModel targetA, FurnitureModel targetB)
	{
		return targetA != null && targetB != null && targetA.MstId == targetB.MstId;
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void Initialize(FurnitureStoreManager manager)
	{
		this.mFurnitureStoreManager = manager;
		this.mNowCategory = FurnitureKinds.Wall;
		FurnitureModel[] models = Enumerable.ToArray<FurnitureModel>(Enumerable.Take<FurnitureModel>(this.mFurnitureStoreManager.GetStoreItem(this.mNowCategory), 10));
		base.ChangeImmediateContentPosition(UIScrollList<FurnitureModel, UIFurnitureStoreTabListChild>.ContentDirection.Hell);
		base.Initialize(models);
		if (this.mModels.Length == 0)
		{
			this.mTransform_NextButton.SetActive(false);
			this.mTransform_PrevButton.SetActive(false);
			this.mTransform_SoldOut.SetActive(true);
		}
		else
		{
			if (3 < this.mModels.Length)
			{
				this.mTransform_NextButton.SetActive(true);
			}
			else
			{
				this.mTransform_NextButton.SetActive(false);
			}
			this.mTransform_PrevButton.SetActive(false);
			this.mTransform_SoldOut.SetActive(false);
		}
	}

	protected override void OnChangedFocusView(UIFurnitureStoreTabListChild focusToView)
	{
		bool flag = 0 == focusToView.GetRealIndex();
		if (flag)
		{
			this.mTransform_PrevButton.SetActive(false);
		}
		else
		{
			this.mTransform_PrevButton.SetActive(true);
		}
		bool flag2 = this.mModels.Length == 0;
		if (flag2)
		{
			this.mTransform_NextButton.SetActive(false);
		}
		else
		{
			bool flag3 = this.mModels.Length - 1 <= this.mCurrentFocusView.GetRealIndex();
			if (flag3)
			{
				this.mTransform_NextButton.SetActive(false);
			}
			else
			{
				this.mTransform_NextButton.SetActive(true);
			}
		}
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
	}

	protected override bool OnSelectable(UIFurnitureStoreTabListChild view)
	{
		bool result = false;
		if (view != null)
		{
			FurnitureModel model = view.GetModel();
			if (model != null && !model.IsPossession())
			{
				result = true;
			}
		}
		return result;
	}

	protected override void OnSelect(UIFurnitureStoreTabListChild view)
	{
		this.mKeyController.ClearKeyAll();
		this.mKeyController.firstUpdate = true;
		if (this.mOnSelectedFurnitureListener != null)
		{
			this.mOnSelectedFurnitureListener.Invoke(view);
		}
	}

	public void SetOnRequestChangeMode(Action onRequestChangeMode)
	{
		this.mOnRequestChangeMode = onRequestChangeMode;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		this.mOnBackListener = onBackListener;
	}

	public void SetOnSelectedFurnitureListener(Action<UIFurnitureStoreTabListChild> onSelectedFurnitureListener)
	{
		this.mOnSelectedFurnitureListener = onSelectedFurnitureListener;
	}

	private void Back()
	{
		base.StopFocusBlink();
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke();
		}
	}

	private void OnRequestChangeMode()
	{
		if (this.mOnRequestChangeMode != null)
		{
			this.mOnRequestChangeMode.Invoke();
		}
	}

	protected override void OnCallDestroy()
	{
		this.mFurnitureStoreManager = null;
		this.mKeyController = null;
		this.mOnRequestChangeMode = null;
	}

	internal void StartControl()
	{
		base.HeadFocus();
		base.StartFocusBlink();
		base.StartControl();
	}

	internal void LockControl()
	{
		base.LockControl();
	}

	internal void ResumeControl()
	{
		if (this.mCurrentFocusView == null)
		{
			base.HeadFocus();
		}
		base.StartFocusBlink();
		base.ResumeFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPrev()
	{
		base.PrevFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchNext()
	{
		base.NextFocus();
	}

	internal void SetSwipeEventCamera(Camera camera)
	{
		base.SetSwipeEventCamera(camera);
	}
}
