using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.PopUp.Mission
{
	[RequireComponent(typeof(UIPanel))]
	public class UIMissionWithTankerDescriptionPopUp : MonoBehaviour
	{
		public enum ActionType
		{
			Shown,
			Hiden
		}

		public delegate void UIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUp.ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject);

		private UIMissionWithTankerDescriptionPopUp.UIMissionWithTankerDescriptionPopUpAction mUIMissionWithTankerDescriptionPopUpAction;

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UILabel mLabel_RequireDay;

		[SerializeField]
		private UILabel mLabel_RequireTransportCraft;

		[SerializeField]
		private UILabel mLabel_RequireFuel;

		[SerializeField]
		private UILabel mLabel_RequireAmmo;

		[SerializeField]
		private UITexture mTexture_BackgroundDesign;

		[SerializeField]
		private UISprite mSprite_Reward_00;

		[SerializeField]
		private UISprite mSprite_Reward_01;

		private UIPanel mPanelThis;

		private MissionModel mModel;

		private KeyControl mKeyController;

		private Coroutine mInitializeCoroutine;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
		}

		public KeyControl GetKeyController()
		{
			if (this.mKeyController == null)
			{
				this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
			return this.mKeyController;
		}

		private void Update()
		{
			if (this.mKeyController != null && (this.mKeyController.keyState.get_Item(0).down || this.mKeyController.keyState.get_Item(1).down))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this.Hide();
			}
		}

		public void Show()
		{
			this.mPanelThis.alpha = 1f;
			this.CallBackAction(UIMissionWithTankerDescriptionPopUp.ActionType.Shown, this);
		}

		public void Hide()
		{
			this.mPanelThis.alpha = 0.01f;
			this.CallBackAction(UIMissionWithTankerDescriptionPopUp.ActionType.Hiden, this);
		}

		public void Initialize(MissionModel model)
		{
			this.mModel = model;
			this.mInitializeCoroutine = base.StartCoroutine(this.InitializeCoroutine(model));
		}

		private string AreaIdToSeaSpriteName(int areaId)
		{
			switch (areaId)
			{
			case 1:
			case 8:
			case 9:
			case 11:
			case 12:
				return "popup_sea" + 1;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "popup_sea" + 2;
			case 3:
			case 13:
				return "popup_sea" + 3;
			case 15:
			case 16:
			case 17:
				return "popup_sea" + 4;
			default:
				return string.Empty;
			}
		}

		[DebuggerHidden]
		private IEnumerator InitializeCoroutine(MissionModel model)
		{
			UIMissionWithTankerDescriptionPopUp.<InitializeCoroutine>c__Iterator1B0 <InitializeCoroutine>c__Iterator1B = new UIMissionWithTankerDescriptionPopUp.<InitializeCoroutine>c__Iterator1B0();
			<InitializeCoroutine>c__Iterator1B.model = model;
			<InitializeCoroutine>c__Iterator1B.<$>model = model;
			<InitializeCoroutine>c__Iterator1B.<>f__this = this;
			return <InitializeCoroutine>c__Iterator1B;
		}

		public void SetOnUIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUp.UIMissionWithTankerDescriptionPopUpAction action)
		{
			this.mUIMissionWithTankerDescriptionPopUpAction = action;
		}

		private void CallBackAction(UIMissionWithTankerDescriptionPopUp.ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject)
		{
			if (this.mUIMissionWithTankerDescriptionPopUpAction != null)
			{
				this.mUIMissionWithTankerDescriptionPopUpAction(actionType, calledObject);
			}
		}

		private string getDisplayText(double rate)
		{
			if (rate == 0.0)
			{
				return "なし";
			}
			if (rate <= 0.30000001192092896)
			{
				return "少量";
			}
			return "普通";
		}

		private void OnDestroy()
		{
			this.mUIMissionWithTankerDescriptionPopUpAction = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Title);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Description);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_RequireDay);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_RequireTransportCraft);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_RequireFuel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_RequireAmmo);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_BackgroundDesign, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_00);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Reward_01);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Description);
			this.mModel = null;
			this.mKeyController = null;
			this.mInitializeCoroutine = null;
		}
	}
}
