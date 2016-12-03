using KCV;
using KCV.Dialog;
using KCV.Mission.Header;
using KCV.Strategy;
using KCV.View.PopUp.Mission;
using KCV.View.Scroll;
using KCV.View.Scroll.Mission;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UserInterfaceMissionManager : MonoBehaviour
{
	[SerializeField]
	private UIMissionScrollListParent mUIMissionScrollListParent;

	[SerializeField]
	private UIMissionWithTankerDescriptionPopUp mPrefab_UIMissionWithTankerDescriptionPopUp;

	[SerializeField]
	private UIMissionWithTankerConfirmPopUp mPrefab_UIMissionWithTankerConfirmPopUp;

	[SerializeField]
	private UIMissionStateChangedCutin mPrefab_UIMissionStateChangedCutIn;

	[SerializeField]
	private ModalCamera mModalCamera;

	[SerializeField]
	private UIMissionHeader mUIMissionHeader;

	private int mAreaId;

	private int mDeckId;

	private MissionManager mMissionManager;

	private KeyControl mFocusKeyController;

	private UIMissionWithTankerConfirmPopUp ConfirmPopUp;

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UserInterfaceMissionManager.<Start>c__Iterator1B1 <Start>c__Iterator1B = new UserInterfaceMissionManager.<Start>c__Iterator1B1();
		<Start>c__Iterator1B.<>f__this = this;
		return <Start>c__Iterator1B;
	}

	private void Update()
	{
		if (this.mFocusKeyController != null)
		{
			if (this.mFocusKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			this.mFocusKeyController.Update();
		}
	}

	private void UIScrollListParentAction(ActionType actionType, UIMissionScrollListParent calledObject, UIMissionScrollListChild actionChild)
	{
		KeyControl nextFocusKeyController = null;
		switch (actionType)
		{
		case ActionType.OnButtonSelect:
		case ActionType.OnTouch:
			this.mUIMissionScrollListParent.EnableTouchControl = false;
			actionChild.Hover();
			this.mModalCamera.Show();
			this.ShowMissionWithTankerConfirmPopUp(actionChild.Model);
			this.ChangeFocusKeyController(nextFocusKeyController);
			break;
		case ActionType.OnBack:
			this.BackToStrategy();
			break;
		}
	}

	private void BackToStrategy()
	{
		this.mFocusKeyController = null;
		Object.Destroy(base.get_gameObject());
		StrategyTaskManager.SceneCallBack();
		PortObjectManager.SceneChangeAct = null;
	}

	private void ShowMissionWithTankerConfirmPopUp(MissionModel missionModel)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		this.mUIMissionScrollListParent.EnableTouchControl = false;
		UIMissionWithTankerConfirmPopUp component = Util.Instantiate(this.mPrefab_UIMissionWithTankerConfirmPopUp.get_gameObject(), this.mModalCamera.get_gameObject(), false, false).GetComponent<UIMissionWithTankerConfirmPopUp>();
		component.SetOnUIMissionWithTankerConfirmPopUpAction(new UIMissionWithTankerConfirmPopUp.UIMissionWithTankerConfirmPopUpAction(this.UIMissionWithTankerConfirmPopUpAction));
		this.ChangeFocusKeyController(null);
		component.Initialize(this.mMissionManager.UserInfo.GetDeck(this.mDeckId), missionModel, this.mMissionManager.TankerCount);
	}

	private void UIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUp.ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject)
	{
		this.ConfirmPopUp = calledObject;
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (actionType)
		{
		case UIMissionWithTankerConfirmPopUp.ActionType.StartMission:
			if (calledObject.Opend)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				Object.Destroy(calledObject.get_gameObject());
				this.mModalCamera.Close();
				this.ChangeFocusKeyController(null);
				if (this.mMissionManager.IsValidMissionStart(calledObject.MissionStartDeckModel.Id, calledObject.MissionStartTargetModel.Id, calledObject.SettingTankerCount).get_Count() == 0)
				{
					this.mMissionManager.MissionStart(calledObject.MissionStartDeckModel.Id, calledObject.MissionStartTargetModel.Id, calledObject.SettingTankerCount);
					this.ShowCutin(this.mMissionManager.UserInfo.GetDeck(this.mDeckId), delegate
					{
						this.BackToStrategy();
					});
				}
			}
			break;
		case UIMissionWithTankerConfirmPopUp.ActionType.NotStartMission:
			if (calledObject.Opend)
			{
				Object.Destroy(calledObject.get_gameObject());
				this.mModalCamera.Close();
				this.mUIMissionScrollListParent.EnableTouchControl = true;
				this.ChangeFocusKeyController(this.mUIMissionScrollListParent.GetKeyController());
			}
			break;
		case UIMissionWithTankerConfirmPopUp.ActionType.Shown:
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			KeyControl keyController = calledObject.GetKeyController();
			this.ChangeFocusKeyController(keyController);
			break;
		}
		case UIMissionWithTankerConfirmPopUp.ActionType.ShowDetail:
			if (calledObject.Opend)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.ShowMissionWithTankerDescriptionPopUp(calledObject.MissionStartTargetModel);
			}
			break;
		}
	}

	private void OnDestroy()
	{
		this.mMissionManager = null;
		this.mFocusKeyController = null;
		this.ConfirmPopUp = null;
		this.mUIMissionScrollListParent = null;
		this.mPrefab_UIMissionWithTankerDescriptionPopUp = null;
		this.mPrefab_UIMissionWithTankerConfirmPopUp = null;
		this.mPrefab_UIMissionStateChangedCutIn = null;
		this.mModalCamera = null;
		this.mUIMissionHeader = null;
	}

	private void ShowCutin(DeckModel deckModel, Action onFinishedCallBack)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		UIMissionStateChangedCutin component = Util.Instantiate(this.mPrefab_UIMissionStateChangedCutIn.get_gameObject(), this.mModalCamera.get_gameObject(), false, false).GetComponent<UIMissionStateChangedCutin>();
		component.Initialize(deckModel);
		component.PlayStartCutin(onFinishedCallBack);
	}

	private void ShowMissionWithTankerDescriptionPopUp(MissionModel model)
	{
		this.mUIMissionScrollListParent.EnableTouchControl = false;
		UIMissionWithTankerDescriptionPopUp component = Util.Instantiate(this.mPrefab_UIMissionWithTankerDescriptionPopUp.get_gameObject(), this.mModalCamera.get_gameObject(), false, false).GetComponent<UIMissionWithTankerDescriptionPopUp>();
		this.mModalCamera.Show();
		component.Initialize(model);
		component.SetOnUIMissionWithTankerDescriptionPopUpAction(new UIMissionWithTankerDescriptionPopUp.UIMissionWithTankerDescriptionPopUpAction(this.UIMissionWithTankerDescriptionPopUpAction));
		component.Show();
		this.ChangeFocusKeyController(component.GetKeyController());
	}

	private void UIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUp.ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		if (actionType != UIMissionWithTankerDescriptionPopUp.ActionType.Shown)
		{
			if (actionType == UIMissionWithTankerDescriptionPopUp.ActionType.Hiden)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				KeyControl keyController = this.ConfirmPopUp.GetKeyController();
				this.ChangeFocusKeyController(keyController);
				Object.Destroy(calledObject.get_gameObject());
				this.mModalCamera.Close();
				TweenAlpha.Begin(this.ConfirmPopUp.get_gameObject(), 0f, 1f);
			}
		}
		else
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			TweenAlpha.Begin(this.ConfirmPopUp.get_gameObject(), 0.2f, 0f);
		}
	}

	private void ChangeFocusKeyController(KeyControl nextFocusKeyController)
	{
		if (this.mFocusKeyController != null)
		{
			this.mFocusKeyController.firstUpdate = true;
			this.mFocusKeyController.ClearKeyAll();
		}
		this.mFocusKeyController = nextFocusKeyController;
		if (this.mFocusKeyController != null)
		{
			this.mFocusKeyController.firstUpdate = true;
			this.mFocusKeyController.ClearKeyAll();
		}
	}
}
