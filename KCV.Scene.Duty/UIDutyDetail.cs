using Common.Enum;
using KCV.Scene.Port;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyDetail : BoardDialog
	{
		public enum SelectType
		{
			Positive,
			Negative
		}

		public delegate void UIDutyDetailAction(UIDutyDetail.SelectType type);

		[SerializeField]
		private UILabel mLabelTitle;

		[SerializeField]
		private UILabel mLabelDescription;

		[SerializeField]
		private UILabel mLabelFuelValue;

		[SerializeField]
		private UILabel mLabelSteelValue;

		[SerializeField]
		private UILabel mLabelAmmoValue;

		[SerializeField]
		private UILabel mLabelBauxiteValue;

		[SerializeField]
		private UIDutyStartButton mDutyStartButton;

		[SerializeField]
		private UISprite[] mSprites_RewardMaterials;

		private UIDutyDetail.UIDutyDetailAction mDutyDetailActionCallBack;

		private DutyModel mDutyModel;

		private KeyControl mKeyController;

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.mDutyStartButton.FocusNegative(true);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.mDutyStartButton.FocusPositive();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.mKeyController = null;
					this.mDutyDetailActionCallBack(UIDutyDetail.SelectType.Negative);
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.mKeyController = null;
					this.mDutyStartButton.ClickFocusButton();
				}
			}
		}

		public KeyControl Show()
		{
			base.Show();
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			if (this.mDutyModel.Category == 2)
			{
				ShipUtils.PlayPortVoice(2);
			}
			else if (this.mDutyModel.Category == 5)
			{
				ShipUtils.PlayPortVoice(3);
			}
			return this.mKeyController;
		}

		public void SetDutyDetailCallBack(UIDutyDetail.UIDutyDetailAction action)
		{
			this.mDutyDetailActionCallBack = action;
		}

		public void Initialize(DutyModel dutyModel)
		{
			this.mDutyModel = dutyModel;
			base.StartCoroutine(this.InitializeCoroutine(this.mDutyModel));
		}

		[DebuggerHidden]
		private IEnumerator InitializeCoroutine(DutyModel dutyModel)
		{
			UIDutyDetail.<InitializeCoroutine>c__Iterator77 <InitializeCoroutine>c__Iterator = new UIDutyDetail.<InitializeCoroutine>c__Iterator77();
			<InitializeCoroutine>c__Iterator.dutyModel = dutyModel;
			<InitializeCoroutine>c__Iterator.<$>dutyModel = dutyModel;
			<InitializeCoroutine>c__Iterator.<>f__this = this;
			return <InitializeCoroutine>c__Iterator;
		}

		public void Hide(Action action)
		{
			base.Hide(action);
		}

		private int MaterialEnumToMasterId(enumMaterialCategory category)
		{
			int result = 0;
			switch (category)
			{
			case enumMaterialCategory.Fuel:
				result = 31;
				break;
			case enumMaterialCategory.Bull:
				result = 32;
				break;
			case enumMaterialCategory.Steel:
				result = 33;
				break;
			case enumMaterialCategory.Bauxite:
				result = 34;
				break;
			case enumMaterialCategory.Build_Kit:
				result = 2;
				break;
			case enumMaterialCategory.Repair_Kit:
				result = 1;
				break;
			case enumMaterialCategory.Dev_Kit:
				result = 3;
				break;
			case enumMaterialCategory.Revamp_Kit:
				result = 4;
				break;
			}
			return result;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mSprites_RewardMaterials);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelDescription);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelFuelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelSteelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelAmmoValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelBauxiteValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			this.mDutyStartButton = null;
			this.mDutyDetailActionCallBack = null;
			this.mDutyModel = null;
			this.mKeyController = null;
		}
	}
}
