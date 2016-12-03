using Common.Enum;
using KCV.Scene.Port;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelEquipListParentNew : UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>, UIRemodelView
	{
		[SerializeField]
		private Transform mTransform_TouchBack;

		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private Transform mMessage;

		private KeyControl mKeyController;

		private Vector3 showPos = new Vector3(270f, 250f);

		private Vector3 hidePos = new Vector3(775f, 250f);

		private SlotitemModel mCurrentEquipSlotitemModel;

		private int mSelectedSlotIndex;

		private ShipModel mTargetShipModel;

		private SlotitemCategory slotitemCategory;

		private RemodelManager mRemodelManager;

		private UIRemodelShipStatus mUIRemodelShipStatus;

		private bool isShown;

		protected override void OnAwake()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void SetSwipeEventCatchCamera(Camera swipeEventCatchCamera)
		{
			base.SetSwipeEventCamera(swipeEventCatchCamera);
		}

		public void Initialize(KeyControl keyController, UIRemodelShipStatus uiRemodelShipStatus, UIRemodelEquipSlotItems uiRemodelEquipSlotItems, ShipModel targetShipModel, SlotitemCategory slotitemCategory)
		{
			this.mUIRemodelShipStatus = uiRemodelShipStatus;
			int currentSlotIndex = uiRemodelEquipSlotItems.GetCurrentSlotIndex();
			bool isExSlot = uiRemodelEquipSlotItems.currentFocusItem.isExSlot;
			this.mTargetShipModel = targetShipModel;
			this.mSelectedSlotIndex = currentSlotIndex;
			this.slotitemCategory = slotitemCategory;
			this.mRemodelManager = UserInterfaceRemodelManager.instance.mRemodelManager;
			this.SetKeyController(keyController);
			this.SetTitle(slotitemCategory);
			SlotitemModel[] models = this.CreateModelArray(isExSlot);
			base.Initialize(models);
			base.get_gameObject().SetActive(false);
			base.get_gameObject().SetActive(true);
			base.ChangeFocusToUserViewHead();
			base.ChangeImmediateContentPosition(UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>.ContentDirection.Hell);
			base.StartControl();
		}

		private void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private SlotitemModel[] CreateModelArray(bool isExSlot)
		{
			SlotitemModel[] array;
			if (!isExSlot)
			{
				array = this.mRemodelManager.GetSlotitemList(this.mTargetShipModel.MemId, this.slotitemCategory);
			}
			else
			{
				array = this.mRemodelManager.GetSlotitemExList(this.mTargetShipModel.MemId);
			}
			List<SlotitemModel> list = new List<SlotitemModel>(array);
			SlotitemUtil.Sort(list, SlotitemUtil.SlotitemSortKey.Type3);
			if (array.Length == 0)
			{
				this.mMessage.set_localScale(Vector3.get_one());
			}
			else
			{
				this.mMessage.set_localScale(Vector3.get_zero());
			}
			return list.ToArray();
		}

		private void SetTitle(SlotitemCategory category)
		{
			string text = string.Empty;
			switch (category)
			{
			case SlotitemCategory.Syuhou:
				text = "主砲";
				break;
			case SlotitemCategory.Fukuhou:
				text = "副砲";
				break;
			case SlotitemCategory.Gyorai:
				text = "魚雷";
				break;
			case SlotitemCategory.Kiju:
				text = "機銃";
				break;
			case SlotitemCategory.Kanjouki:
				text = "艦上機";
				break;
			case SlotitemCategory.Suijouki:
				text = "水上機";
				break;
			case SlotitemCategory.Dentan:
				text = "電探";
				break;
			case SlotitemCategory.Other:
				text = "その他";
				break;
			}
			this.titleLabel.text = "装備選択\u3000- " + text + " -";
		}

		protected override void OnUpdate()
		{
			if (this.mKeyController != null && base.mState == UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>.ListState.Waiting && this.isShown)
			{
				if (this.mKeyController.IsShikakuDown())
				{
					this.SwitchLockItem();
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

		public void Show()
		{
			this.mTransform_TouchBack.SetActive(true);
			base.StartStaticWidgetChildren();
			base.get_gameObject().SetActive(false);
			base.get_gameObject().SetActive(true);
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, delegate
			{
				this.isShown = true;
				base.StopStaticWidgetChildren();
			});
			if (this.mKeyController != null)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				base.StartControl();
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			this.mTransform_TouchBack.SetActive(false);
			base.LockControl();
			base.StartStaticWidgetChildren();
			this.isShown = false;
			if (animation)
			{
				base.StartStaticWidgetChildren();
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
				{
					base.StopStaticWidgetChildren();
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.StopStaticWidgetChildren();
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}

		public void OnTouchHide()
		{
			if (this.isShown)
			{
				this.Back();
			}
		}

		protected override void OnSelect(UIRemodelEquipListChildNew child)
		{
			if (this.isShown && base.mState == UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>.ListState.Waiting)
			{
				if (child == null)
				{
					return;
				}
				if (child.GetModel() == null)
				{
					return;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouPreview(this.mTargetShipModel, this.mSelectedSlotIndex, child);
				this.Hide();
			}
		}

		private void Back()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this.Hide();
				UserInterfaceRemodelManager.instance.Back2SoubiHenkouTypeSelect();
			}
		}

		protected override void OnChangedFocusView(UIRemodelEquipListChildNew focusToView)
		{
			if (base.mState == UIScrollList<SlotitemModel, UIRemodelEquipListChildNew>.ListState.Waiting)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void SwitchLockItem()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.SlotLock(this.mCurrentFocusView.GetModel().MemId);
			this.mCurrentFocusView.SwitchLockedIcon(false);
			base.RefreshViews();
		}

		public void OnCancel()
		{
			this.Back();
		}

		internal void Release()
		{
			this.mTransform_TouchBack = null;
			this.titleLabel = null;
			this.mUIRemodelShipStatus = null;
			this.mCurrentEquipSlotitemModel = null;
			this.mTargetShipModel = null;
			this.mRemodelManager = null;
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.titleLabel);
			this.mTransform_TouchBack = null;
			this.mMessage = null;
			this.mKeyController = null;
			this.mCurrentEquipSlotitemModel = null;
			this.mTargetShipModel = null;
			this.mRemodelManager = null;
			this.mUIRemodelShipStatus = null;
		}
	}
}
