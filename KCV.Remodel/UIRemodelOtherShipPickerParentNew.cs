using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelOtherShipPickerParentNew : UIScrollList<ShipModel, UIRemodelOtherShipPickerChildNew>, UIRemodelView, IBannerResourceManage
	{
		[SerializeField]
		private Collider2D TouchBackArea;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private KeyControl mKeyController;

		private Vector3 showPos = new Vector3(233f, 250f);

		private Vector3 hidePos = new Vector3(800f, 250f);

		private bool mCallFirstInitialized;

		private bool isShown;

		protected override void OnAwake()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Initialize(KeyControl keyController)
		{
			UIPanel component = this.mTransform_ContentPosition.get_parent().GetComponent<UIPanel>();
			if (component != null)
			{
				component.clipping = UIDrawCall.Clipping.None;
			}
			if (!this.mCallFirstInitialized)
			{
				this.mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				this.mCallFirstInitialized = true;
			}
			this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.OnSortedShipsListener));
			this.SetKeyController(keyController);
			ShipModel[] otherShipList = UserInterfaceRemodelManager.instance.mRemodelManager.GetOtherShipList();
			this.mUIShipSortButton.SetClickable(true);
			if (this.mModels == null)
			{
				base.Initialize(otherShipList);
				this.mUIShipSortButton.Initialize(otherShipList);
			}
			else
			{
				this.mUIShipSortButton.Initialize(otherShipList);
			}
			this.mUIShipSortButton.ReSort();
			base.ChangeFocusToUserViewHead();
		}

		protected override void OnUpdate()
		{
			if (base.mState == UIScrollList<ShipModel, UIRemodelOtherShipPickerChildNew>.ListState.Waiting && this.isShown && this.mKeyController != null)
			{
				if (this.mKeyController.IsSankakuDown())
				{
					this.mUIShipSortButton.OnClickSortButton();
				}
				else if (this.mKeyController.IsDownDown())
				{
					base.NextFocus();
				}
				else if (this.mKeyController.IsUpDown())
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.IsLeftDown())
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.IsRightDown())
				{
					base.NextPageOrTailFocus();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					base.Select();
				}
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnSortedShipsListener(ShipModel[] shipModels)
		{
			base.ChangeImmediateContentPosition(UIScrollList<ShipModel, UIRemodelOtherShipPickerChildNew>.ContentDirection.Hell);
			base.Refresh(shipModels, true);
		}

		public void Refresh(ShipModel ship)
		{
			ShipModel[] otherShipList = UserInterfaceRemodelManager.instance.mRemodelManager.GetOtherShipList();
			this.mUIShipSortButton.RefreshModels(otherShipList);
			this.mUIShipSortButton.ReSort();
			base.ChangePageFromModel(ship);
		}

		public void SetSwipeEventCamera(Camera camera)
		{
			base.SetSwipeEventCamera(camera);
		}

		private void Start()
		{
			this.Hide(false);
		}

		public void Show()
		{
			base.RefreshViews();
			base.get_gameObject().SetActive(false);
			base.get_gameObject().SetActive(true);
			base.set_enabled(true);
			base.StartStaticWidgetChildren();
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, delegate
			{
				this.isShown = true;
				base.StopStaticWidgetChildren();
			});
			this.TouchBackArea.set_enabled(true);
			base.StartControl();
			this.ChangeShip(this.mCurrentFocusView.GetModel());
		}

		public void Hide()
		{
			this.Hide(true);
			base.LockControl();
		}

		public void Hide(bool animation)
		{
			this.isShown = false;
			base.StartStaticWidgetChildren();
			base.set_enabled(false);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
				{
					base.StopStaticWidgetChildren();
				}).PlayForward();
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
			}
			this.TouchBackArea.set_enabled(false);
		}

		private void ChangeShip(ShipModel model)
		{
			if (model != null && base.get_enabled())
			{
				UserInterfaceRemodelManager.instance.ChangeFocusShip(model);
			}
		}

		private void Forward()
		{
			this.Hide();
			UserInterfaceRemodelManager.instance.Forward2ModeSelect();
		}

		protected override void OnSelect(UIRemodelOtherShipPickerChildNew view)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.Forward();
		}

		protected override void OnChangedFocusView(UIRemodelOtherShipPickerChildNew focusToView)
		{
			if (base.mState == UIScrollList<ShipModel, UIRemodelOtherShipPickerChildNew>.ListState.Waiting && base.get_gameObject().get_activeSelf())
			{
				int num = Array.IndexOf<ShipModel>(this.mModels, focusToView.GetModel());
				int num2 = this.mModels.Length;
				CommonPopupDialog.Instance.StartPopup(num + 1 + "/" + num2);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.ChangeShip(focusToView.GetModel());
			}
		}

		protected override int GetModelIndex(ShipModel model)
		{
			if (model == null)
			{
				return -1;
			}
			int num = 0;
			ShipModel[] mModels = this.mModels;
			for (int i = 0; i < mModels.Length; i++)
			{
				ShipModel shipModel = mModels[i];
				if (shipModel != null && shipModel.MemId == model.MemId)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		protected override bool EqualsModel(ShipModel targetA, ShipModel targetB)
		{
			return targetA != null && targetB != null && targetA.MemId == targetB.MemId;
		}

		protected override void OnCallDestroy()
		{
			this.TouchBackArea = null;
			this.mUIShipSortButton = null;
		}

		internal void RefreshViews()
		{
			base.RefreshViews();
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodelOtherShipPickerChildNew[] mViews = this.mViews;
			for (int i = 0; i < mViews.Length; i++)
			{
				UIRemodelOtherShipPickerChildNew uIRemodelOtherShipPickerChildNew = mViews[i];
				CommonShipBanner banner = uIRemodelOtherShipPickerChildNew.GetBanner();
				list.Add(banner);
			}
			return list.ToArray();
		}
	}
}
