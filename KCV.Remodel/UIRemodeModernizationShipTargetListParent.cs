using Common.Enum;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodeModernizationShipTargetListParent : UIScrollListParent<RemodeModernizationShipTargetListChild, UIRemodeModernizationShipTargetListChild>, UIRemodelView
	{
		[SerializeField]
		private UIButton mButton_TouchBack;

		[SerializeField]
		private Transform mMessage;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private Vector3 showPos = new Vector3(270f, 0f, 0f);

		private Vector3 hidePos = new Vector3(700f, 0f, 0f);

		private bool mIsFirstInitialized;

		private KeyControl originKeyController;

		private ShipModel mTargetExchangeShipModel;

		private bool mCallFirstInitialized;

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Initialize(KeyControl keyController, ShipModel targetExchangeShipModel, List<ShipModel> exceptShipModels)
		{
			this.mTargetExchangeShipModel = targetExchangeShipModel;
			this.originKeyController = keyController;
			UserInterfaceRemodelManager.instance.mRemodelManager.PowupTargetShip = UserInterfaceRemodelManager.instance.focusedShipModel;
			ShipModel[] candidateShips = UserInterfaceRemodelManager.instance.mRemodelManager.GetCandidateShips(exceptShipModels);
			if (!this.mCallFirstInitialized)
			{
				this.mUIShipSortButton.SetSortKey(SortKey.LEVEL);
				this.mCallFirstInitialized = true;
			}
			this.mUIShipSortButton.Initialize(candidateShips);
			this.mUIShipSortButton.SetClickable(true);
			this.mUIShipSortButton.SetOnSortedShipsListener(new Action<ShipModel[]>(this.onSortedShipsListener));
			List<RemodeModernizationShipTargetListChild> list = new List<RemodeModernizationShipTargetListChild>();
			if (targetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.UnSet, null));
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
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.Model, model));
			}
			if (!this.mIsFirstInitialized)
			{
				base.Initialize(list.ToArray());
				this.mIsFirstInitialized = true;
			}
			else
			{
				base.RefreshAndFirstFocus(list.ToArray());
			}
			base.SetOnUIScrollListParentAction(delegate(ActionType actionType, UIScrollListParent<RemodeModernizationShipTargetListChild, UIRemodeModernizationShipTargetListChild> calledObject, UIScrollListChild<RemodeModernizationShipTargetListChild> actionChild)
			{
				this.OnScrollAction(actionType, (UIRemodeModernizationShipTargetListChild)actionChild);
			});
		}

		protected override void OnKeyPressTriangle()
		{
			this.mUIShipSortButton.OnClickSortButton();
		}

		private void onSortedShipsListener(ShipModel[] shipModels)
		{
			List<RemodeModernizationShipTargetListChild> list = new List<RemodeModernizationShipTargetListChild>();
			if (this.mTargetExchangeShipModel != null)
			{
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.UnSet, null));
			}
			for (int i = 0; i < shipModels.Length; i++)
			{
				ShipModel model = shipModels[i];
				list.Add(new RemodeModernizationShipTargetListChild(RemodeModernizationShipTargetListChild.ListItemOption.Model, model));
			}
			base.Initialize(list.ToArray());
		}

		public void Back()
		{
			this.Hide();
			UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
		}

		public void OnTouchHide()
		{
			this.Back();
		}

		public void Show()
		{
			this.SetKeyController(this.originKeyController);
			this.mButton_TouchBack.SetActive(true);
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, null);
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			this.SetKeyController(null);
			this.mButton_TouchBack.SetActive(false);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, null);
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
			}
		}

		public override void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.firstUpdate = true;
				keyController.ClearKeyAll();
			}
			base.SetKeyController(keyController);
		}

		private void OnScrollAction(ActionType actionType, UIRemodeModernizationShipTargetListChild actionChild)
		{
			switch (actionType)
			{
			case ActionType.OnButtonSelect:
			case ActionType.OnTouch:
			{
				ShipModel selectedShipModel = null;
				if (actionChild != null && actionChild.Model.mOption == RemodeModernizationShipTargetListChild.ListItemOption.Model)
				{
					selectedShipModel = actionChild.Model.mShipModel;
				}
				this.Hide();
				UserInterfaceRemodelManager.instance.SelectKindaikaKaishuSozai(selectedShipModel);
				break;
			}
			case ActionType.OnChangeFocus:
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				break;
			case ActionType.OnBack:
				this.Back();
				break;
			}
		}

		private void OnDestroy()
		{
			this.mButton_TouchBack = null;
		}
	}
}
