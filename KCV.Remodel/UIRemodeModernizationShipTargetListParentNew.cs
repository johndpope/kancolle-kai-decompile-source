using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRemodeModernizationShipTargetListParentNew : UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>, UIRemodelView, IBannerResourceManage
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private Transform mTransform_TouchBack;

		[SerializeField]
		private Transform mMessage;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private Vector3 showPos = new Vector3(270f, 250f, 0f);

		private Vector3 hidePos = new Vector3(730f, 250f, 0f);

		private bool mIsFirstInitialized;

		private bool mCallFirstInitialize;

		private KeyControl mKeyController;

		private ShipModel mTargetExchangeShipModel;

		private bool isShown;

		private float ANIMATION_DURATION = 0.2f;

		protected override void OnAwake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void SetCamera(Camera swipeEventCamera)
		{
			base.SetSwipeEventCamera(swipeEventCamera);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (this.mKeyController != null && base.mState == UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>.ListState.Waiting && this.isShown)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					base.NextFocus();
				}
				else if (this.mKeyController.keyState.get_Item(3).down)
				{
					this.mUIShipSortButton.OnClickSortButton();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					base.Select();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.Back();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					base.NextPageOrTailFocus();
				}
			}
		}

		protected override void OnChangedFocusView(UIRemodeModernizationShipTargetListChildNew focusToView)
		{
			if (base.mState == UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>.ListState.Waiting)
			{
				base.OnChangedFocusView(focusToView);
				if (this.isShown)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
		}

		public void Initialize(KeyControl keyController, ShipModel targetExchangeShipModel, List<ShipModel> exceptShipModels)
		{
			UIPanel component = this.mTransform_ContentPosition.get_parent().GetComponent<UIPanel>();
			if (component != null)
			{
				component.clipping = UIDrawCall.Clipping.None;
			}
			base.ChangeImmediateContentPosition(UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>.ContentDirection.Hell);
			this.mTargetExchangeShipModel = targetExchangeShipModel;
			this.mKeyController = keyController;
			UserInterfaceRemodelManager.instance.mRemodelManager.PowupTargetShip = UserInterfaceRemodelManager.instance.focusedShipModel;
			ShipModel[] candidateShips = UserInterfaceRemodelManager.instance.mRemodelManager.GetCandidateShips(exceptShipModels);
			List<RemodeModernizationShipTargetListChildNew> list = new List<RemodeModernizationShipTargetListChildNew>();
			if (targetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet, null));
			}
			if (targetExchangeShipModel == null && candidateShips.Length == 0)
			{
				this.mMessage.set_localScale(Vector3.get_one());
			}
			else
			{
				this.mMessage.set_localScale(Vector3.get_zero());
			}
			ShipModel[] array = candidateShips;
			for (int i = 0; i < array.Length; i++)
			{
				ShipModel model = array[i];
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.Model, model));
			}
			if (!this.mCallFirstInitialize)
			{
				this.mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				this.mCallFirstInitialize = true;
			}
			this.mUIShipSortButton.Initialize(candidateShips);
			this.mUIShipSortButton.SetClickable(true);
			this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.onSortedShipsListener));
			this.mUIShipSortButton.ReSort();
			base.HeadFocus();
			base.StartControl();
		}

		private void onSortedShipsListener(ShipModel[] shipModels)
		{
			List<RemodeModernizationShipTargetListChildNew> list = new List<RemodeModernizationShipTargetListChildNew>();
			if (this.mTargetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet, null));
			}
			for (int i = 0; i < shipModels.Length; i++)
			{
				ShipModel model = shipModels[i];
				list.Add(new RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption.Model, model));
			}
			if (!this.mIsFirstInitialized)
			{
				base.Initialize(list.ToArray());
				if (shipModels.Length == 0)
				{
					this.mMessage.SetActive(true);
				}
				else
				{
					this.mMessage.SetActive(false);
				}
				this.mIsFirstInitialized = true;
			}
			else
			{
				base.Refresh(list.ToArray(), true);
				if (shipModels.Length == 0)
				{
					this.mMessage.SetActive(true);
				}
				else
				{
					this.mMessage.SetActive(false);
				}
				base.get_gameObject().SetActive(true);
			}
			base.ChangeImmediateContentPosition(UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>.ContentDirection.Hell);
			base.HeadFocus();
			base.StartControl();
		}

		public void Back()
		{
			if (this.isShown)
			{
				this.Hide();
				UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
			}
		}

		public void OnTouchHide()
		{
			this.Back();
		}

		public void Show()
		{
			base.get_transform().SetActive(false);
			base.get_transform().SetActive(true);
			this.mTransform_TouchBack.SetActive(true);
			this.mPanelThis.widgetsAreStatic = true;
			base.StartStaticWidgetChildren();
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, this.ANIMATION_DURATION, delegate
			{
				this.isShown = true;
				this.mPanelThis.widgetsAreStatic = false;
				base.StopStaticWidgetChildren();
			});
		}

		public void Hide()
		{
			base.RemoveFocus();
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			base.StartStaticWidgetChildren();
			this.SetKeyController(null);
			this.mTransform_TouchBack.SetActive(false);
			this.isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, this.ANIMATION_DURATION, delegate
				{
					base.StopStaticWidgetChildren();
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.StopStaticWidgetChildren();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		protected override void OnSelect(UIRemodeModernizationShipTargetListChildNew view)
		{
			if (this.isShown && base.mState == UIScrollList<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>.ListState.Waiting)
			{
				base.RemoveFocus();
				ShipModel selectedShipModel = null;
				if (view != null && view.GetModel() != null)
				{
					RemodeModernizationShipTargetListChildNew.ListItemOption mOption = view.GetModel().mOption;
					if (mOption != RemodeModernizationShipTargetListChildNew.ListItemOption.Model)
					{
						if (mOption == RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet)
						{
							selectedShipModel = null;
						}
					}
					else
					{
						selectedShipModel = view.GetModel().mShipModel;
					}
				}
				this.Hide();
				UserInterfaceRemodelManager.instance.SelectKindaikaKaishuSozai(selectedShipModel);
			}
		}

		protected override void OnCallDestroy()
		{
			this.mTransform_TouchBack = null;
			this.mMessage = null;
			this.mUIShipSortButton = null;
			this.mKeyController = null;
			this.mTargetExchangeShipModel = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodeModernizationShipTargetListChildNew[] mViews = this.mViews;
			for (int i = 0; i < mViews.Length; i++)
			{
				UIRemodeModernizationShipTargetListChildNew uIRemodeModernizationShipTargetListChildNew = mViews[i];
				CommonShipBanner shipBanner = uIRemodeModernizationShipTargetListChildNew.GetShipBanner();
				list.Add(shipBanner);
			}
			return list.ToArray();
		}
	}
}
