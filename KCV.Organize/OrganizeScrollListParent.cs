using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	[RequireComponent(typeof(UIPanel))]
	public class OrganizeScrollListParent : UIScrollList<ShipModel, OrganizeScrollListChild>
	{
		public delegate void OnSelectCallBack(ShipModel ship);

		private UIPanel mPanelThis;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private UIShipSortButton SortButton;

		[SerializeField]
		private UIButtonMessage BackButton;

		private KeyControl mKeyController;

		private Action _OnCancelCallBack;

		private OrganizeScrollListParent.OnSelectCallBack _OnSelectCallBack;

		private bool _isFirst = true;

		private int _before_focus;

		public bool isOpen;

		private bool mFirstCalledInitialize;

		protected override void OnAwake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			if (this.TweenPos == null)
			{
				this.TweenPos = base.GetComponent<TweenPosition>();
			}
		}

		protected override void OnStart()
		{
		}

		public void Initialize(IOrganizeManager manager, Camera camera)
		{
			if (!this.mFirstCalledInitialize)
			{
				this.SortButton.SetSortKey(SortKey.UNORGANIZED);
				this.mFirstCalledInitialize = true;
			}
			this._isFirst = true;
			this._before_focus = 0;
			base.SetSwipeEventCamera(camera);
			OrganizeScrollListChild[] mViews = this.mViews;
			for (int i = 0; i < mViews.Length; i++)
			{
				OrganizeScrollListChild organizeScrollListChild = mViews[i];
				organizeScrollListChild.setManager(manager);
			}
			this.SortButton.InitializeForOrganize(manager.GetShipList());
			this.SortButton.SetClickable(true);
			this.SortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSorted));
			this.SortButton.SetCheckClicableDelegate(new UIShipSortButton.CheckClickable(this.CheckSortButtonClickable));
			this.SortButton.ReSort();
			base.get_gameObject().SetActive(false);
			base.get_gameObject().SetActive(true);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnDestroy()
		{
			this.mKeyController = null;
			this.TweenPos = null;
			this.SortButton = null;
			this.BackButton = null;
		}

		public void SetBackButtonEnable(bool isEnable)
		{
			this.BackButton.GetComponent<BoxCollider2D>().set_enabled(isEnable);
		}

		private void OnSorted(ShipModel[] ships)
		{
			base.Refresh(ships, true);
			base.ChangeImmediateContentPosition(UIScrollList<ShipModel, OrganizeScrollListChild>.ContentDirection.Hell);
		}

		public void MovePosition(int x, bool isOpen = false, Action Onfinished = null)
		{
			if (base.get_gameObject().get_activeSelf())
			{
				base.get_gameObject().SetActive(false);
				base.get_gameObject().SetActive(true);
			}
			if (this.mPanelThis != null)
			{
				this.mPanelThis.widgetsAreStatic = true;
			}
			this.isOpen = isOpen;
			this.TweenPos.onFinished.Clear();
			this.TweenPos.from = this.TweenPos.get_transform().get_localPosition();
			this.TweenPos.to = new Vector3((float)x, this.TweenPos.get_transform().get_localPosition().y, 0f);
			this.TweenPos.ResetToBeginning();
			this.TweenPos.PlayForward();
			this.TweenPos.ignoreTimeScale = true;
			if (Onfinished != null)
			{
				this.TweenPos.SetOnFinished(delegate
				{
					if (this.mPanelThis != null)
					{
						this.mPanelThis.widgetsAreStatic = false;
					}
					Onfinished.Invoke();
				});
			}
		}

		protected override void OnUpdate()
		{
			if (this.mKeyController != null && base.mState == UIScrollList<ShipModel, OrganizeScrollListChild>.ListState.Waiting)
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
					this.mKeyController.firstUpdate = true;
					this.mKeyController.ClearKeyAll();
					base.Select();
				}
				else if (this.mKeyController.keyState.get_Item(2).down)
				{
					this.mCurrentFocusView.SwitchShipLockState();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnCancel();
				}
				else if (this.mKeyController.keyState.get_Item(3).down)
				{
					this.SortButton.OnClickSortButton();
				}
			}
		}

		public void OnCancel()
		{
			this.mKeyController = null;
			base.LockControl();
			this._OnCancelCallBack.Invoke();
		}

		protected override void OnChangedFocusView(OrganizeScrollListChild focusToView)
		{
			if (base.mState == UIScrollList<ShipModel, OrganizeScrollListChild>.ListState.Waiting)
			{
				if (this.mModels == null)
				{
					return;
				}
				if (this.mModels.Length <= 0)
				{
					return;
				}
				if (!this._isFirst && this._before_focus != focusToView.GetRealIndex())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this._isFirst = false;
				this._before_focus = focusToView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}

		protected override void OnSelect(OrganizeScrollListChild view)
		{
			if (view.GetModel() != null && this.isOpen)
			{
				this._OnSelectCallBack(view.GetModel());
				this.mKeyController = null;
				base.LockControl();
			}
		}

		protected override void OnCallDestroy()
		{
		}

		protected override bool OnSelectable(OrganizeScrollListChild view)
		{
			return true;
		}

		public void SetOnSelect(OrganizeScrollListParent.OnSelectCallBack onSelect)
		{
			this._OnSelectCallBack = onSelect;
		}

		public void SetOnCancel(Action onCancel)
		{
			this._OnCancelCallBack = onCancel;
		}

		public void ChangeLockBtnState()
		{
			if (this.mCurrentFocusView != null)
			{
				this.mCurrentFocusView.SwitchShipLockState();
			}
		}

		private bool CheckSortButtonClickable()
		{
			return base.mState == UIScrollList<ShipModel, OrganizeScrollListChild>.ListState.Waiting;
		}

		public ShipModel GetFocusModel()
		{
			return this.mCurrentFocusView.GetModel();
		}

		public void SetBackButton(GameObject target, string FunctionName)
		{
			this.BackButton.target = target;
			this.BackButton.functionName = FunctionName;
		}

		internal void ResumeControl()
		{
			base.StartControl();
		}

		internal void StartControl()
		{
			base.StartControl();
		}

		internal void RefreshViews()
		{
			if (this.SortButton.CurrentSortKey == SortKey.UNORGANIZED)
			{
				ShipModel[] mModels = this.SortButton.SortModels(SortKey.UNORGANIZED);
				this.mModels = mModels;
			}
			base.RefreshViews();
		}
	}
}
