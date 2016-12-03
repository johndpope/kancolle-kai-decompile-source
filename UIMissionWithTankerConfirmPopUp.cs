using Common.Enum;
using KCV;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIMissionWithTankerConfirmPopUp : MonoBehaviour
{
	public enum ActionType
	{
		StartMission,
		NotStartMission,
		Shown,
		Hiden,
		ShowDetail
	}

	public enum CheckType
	{
		CallTankerCountUp,
		CallTankerCountDown,
		CanStartCheck
	}

	public delegate void UIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUp.ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject);

	public delegate bool UIMissionWithTankerConfirmPopUpCheck(UIMissionWithTankerConfirmPopUp.CheckType actionType, UIMissionWithTankerConfirmPopUp calledObject);

	[SerializeField]
	private UIButtonManager mButtonManager;

	[SerializeField]
	private UILabel mLabel_RequireDay;

	[SerializeField]
	private UILabel mLabel_RequireTanker;

	[SerializeField]
	private UILabel mLabel_SettingTankerValue;

	[SerializeField]
	private UILabel mLabel_HasTankerCount;

	[SerializeField]
	private UIButton mButton_Positive;

	[SerializeField]
	private UIButton mButton_Negative;

	[SerializeField]
	private UIButton mButton_Description;

	[SerializeField]
	private UIButton mButton_CountUp;

	[SerializeField]
	private UIButton mButton_CountDown;

	[SerializeField]
	private UIMissionShipBanner[] mUIMissionShipBanners;

	[SerializeField]
	private UILabel mLabel_Message;

	private UIButton mFocusButton;

	private KeyControl mKeyController;

	private UIMissionWithTankerConfirmPopUp.UIMissionWithTankerConfirmPopUpAction mUIMissionWithTankerConfirmPopUpAction;

	private Coroutine mInitializeCoroutine;

	private int mHasTankerCount;

	private UIButton[] mSelectableButtons;

	private bool isGoCondition;

	public int SettingTankerCount
	{
		get;
		private set;
	}

	public DeckModel MissionStartDeckModel
	{
		get;
		private set;
	}

	public MissionModel MissionStartTargetModel
	{
		get;
		private set;
	}

	public bool Opend
	{
		get;
		private set;
	}

	public void Initialize(DeckModel deckModel, MissionModel missionModel, int hasTankerCount)
	{
		this.mLabel_Message.alpha = 0.01f;
		this.MissionStartTargetModel = missionModel;
		this.MissionStartDeckModel = deckModel;
		this.mHasTankerCount = hasTankerCount;
		List<UIButton> list = new List<UIButton>();
		list.Add(this.mButton_Negative);
		bool flag = missionModel.TankerMinCount <= hasTankerCount;
		if (flag)
		{
			list.Add(this.mButton_Positive);
		}
		list.Add(this.mButton_Description);
		this.mSelectableButtons = list.ToArray();
		this.mButtonManager.IndexChangeAct = delegate
		{
			UIButton uIButton = this.mButtonManager.GetFocusableButtons()[this.mButtonManager.nowForcusIndex];
			bool flag2 = 0 <= Array.IndexOf<UIButton>(this.mSelectableButtons, uIButton);
			if (flag2)
			{
				this.ChangeFocusButton(uIButton);
			}
		};
		if (this.mInitializeCoroutine != null)
		{
			base.StopCoroutine(this.mInitializeCoroutine);
			this.mInitializeCoroutine = null;
		}
		this.mInitializeCoroutine = base.StartCoroutine(this.InitailizeCoroutine(deckModel, missionModel, hasTankerCount, delegate
		{
			this.mInitializeCoroutine = null;
			this.CallBackAction(UIMissionWithTankerConfirmPopUp.ActionType.Shown, this);
			this.Opend = true;
		}));
	}

	[DebuggerHidden]
	private IEnumerator InitailizeCoroutine(DeckModel deckModel, MissionModel missionModel, int hasTankerCount, Action callBack)
	{
		UIMissionWithTankerConfirmPopUp.<InitailizeCoroutine>c__Iterator1AF <InitailizeCoroutine>c__Iterator1AF = new UIMissionWithTankerConfirmPopUp.<InitailizeCoroutine>c__Iterator1AF();
		<InitailizeCoroutine>c__Iterator1AF.missionModel = missionModel;
		<InitailizeCoroutine>c__Iterator1AF.hasTankerCount = hasTankerCount;
		<InitailizeCoroutine>c__Iterator1AF.deckModel = deckModel;
		<InitailizeCoroutine>c__Iterator1AF.callBack = callBack;
		<InitailizeCoroutine>c__Iterator1AF.<$>missionModel = missionModel;
		<InitailizeCoroutine>c__Iterator1AF.<$>hasTankerCount = hasTankerCount;
		<InitailizeCoroutine>c__Iterator1AF.<$>deckModel = deckModel;
		<InitailizeCoroutine>c__Iterator1AF.<$>callBack = callBack;
		<InitailizeCoroutine>c__Iterator1AF.<>f__this = this;
		return <InitailizeCoroutine>c__Iterator1AF;
	}

	public string GoConditionToString(IsGoCondition condition)
	{
		switch (condition)
		{
		case IsGoCondition.AnotherArea:
			return "艦隊は他の海域にいます";
		case IsGoCondition.ActionEndDeck:
			return "行動終了している艦隊です";
		case IsGoCondition.Mission:
			return "艦隊は遠征中です";
		case IsGoCondition.Deck1:
			return "第一艦隊です";
		case IsGoCondition.HasBling:
			return "回航艦を含んでいます";
		case IsGoCondition.HasRepair:
			return "艦隊は入渠中の艦を含んでいます";
		case IsGoCondition.FlagShipTaiha:
			return "旗艦が大破しています";
		case IsGoCondition.ReqFullSupply:
			return "燃料/弾薬が最大の必要があります";
		case IsGoCondition.NeedSupply:
			return "燃料/弾薬が0の艦を含んでいます(補給が必要です)";
		case IsGoCondition.ConditionRed:
			return "疲労度-赤の艦を含んでいます";
		case IsGoCondition.Tanker:
			return "輸送船が不足しています";
		case IsGoCondition.NecessaryStype:
			return "特定の艦種が必要です";
		case IsGoCondition.InvalidOrganization:
			return "艦隊の編成は条件を満たしていません";
		case IsGoCondition.OtherDeckMissionRunning:
			return "既に他の艦隊が遠征しています";
		}
		return string.Empty;
	}

	public KeyControl GetKeyController()
	{
		if (this.mKeyController == null)
		{
			this.mKeyController = new KeyControl(0, 2, 0.4f, 0.1f);
			this.mKeyController.setChangeValue(0f, 1f, 0f, -1f);
		}
		return this.mKeyController;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UIButton>(this.mSelectableButtons, this.mFocusButton);
				int num2 = num - 1;
				if (0 <= num2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					UIButton uIButton = this.mSelectableButtons[num2];
					if (uIButton.isEnabled)
					{
						this.ChangeFocusButton(uIButton);
					}
					else
					{
						int num3 = num2 - 1;
						if (0 <= num3)
						{
							UIButton uIButton2 = this.mSelectableButtons[num3];
							if (uIButton2.isEnabled)
							{
								this.ChangeFocusButton(uIButton2);
							}
						}
					}
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num4 = Array.IndexOf<UIButton>(this.mSelectableButtons, this.mFocusButton);
				int num5 = num4 + 1;
				if (num5 < this.mSelectableButtons.Length)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					UIButton uIButton3 = this.mSelectableButtons[num5];
					if (uIButton3.isEnabled)
					{
						this.ChangeFocusButton(uIButton3);
					}
					else
					{
						int num6 = num5 + 1;
						if (num6 < this.mSelectableButtons.Length)
						{
							UIButton uIButton4 = this.mSelectableButtons[num6];
							if (uIButton4.isEnabled)
							{
								this.ChangeFocusButton(uIButton4);
							}
						}
					}
				}
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.mFocusButton.SendMessage("OnClick");
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this.mButton_Negative.SendMessage("OnClick");
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				this.mButton_CountUp.SendMessage("OnClick");
			}
			else if (this.mKeyController.keyState.get_Item(12).down)
			{
				this.mButton_CountDown.SendMessage("OnClick");
			}
		}
	}

	public void OnClickCountUpTanker()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		this.CountUpTanker();
	}

	public void OnClickCountDownTanker()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		this.CountDownTanker();
	}

	public void OnClickPositiveButton()
	{
		if (this.isGoCondition)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.CallBackAction(UIMissionWithTankerConfirmPopUp.ActionType.StartMission, this);
		}
	}

	public void OnClickNegativeButton()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		this.CallBackAction(UIMissionWithTankerConfirmPopUp.ActionType.NotStartMission, this);
	}

	public void OnClickDescriptionButton()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		this.CallBackAction(UIMissionWithTankerConfirmPopUp.ActionType.ShowDetail, this);
	}

	public void SetOnUIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUp.UIMissionWithTankerConfirmPopUpAction action)
	{
		this.mUIMissionWithTankerConfirmPopUpAction = action;
	}

	private void CountUpTanker()
	{
		UIUtil.AnimationOnFocus(this.mButton_CountUp.get_transform(), null);
		bool flag = this.SettingTankerCount + 1 <= this.MissionStartTargetModel.TankerMaxCount && this.SettingTankerCount + 1 <= this.mHasTankerCount;
		if (flag)
		{
			this.SettingTankerCount++;
			this.UpdateSettingTankerCountLabel(this.SettingTankerCount, false);
		}
	}

	private void CountDownTanker()
	{
		UIUtil.AnimationOnFocus(this.mButton_CountDown.get_transform(), null);
		bool flag = this.MissionStartTargetModel.TankerMinCount <= this.SettingTankerCount - 1;
		if (flag)
		{
			this.SettingTankerCount--;
			this.UpdateSettingTankerCountLabel(this.SettingTankerCount, false);
		}
	}

	private void UpdateSettingTankerCountLabel(int value, bool isPoor)
	{
		this.mLabel_SettingTankerValue.text = value.ToString();
		if (isPoor)
		{
			this.mLabel_SettingTankerValue.color = Color.get_red();
		}
	}

	private void ChangeFocusButton(UIButton target)
	{
		if (this.mFocusButton != null && this.mFocusButton.isEnabled)
		{
			this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
		}
		this.mFocusButton = target;
		if (this.mFocusButton != null && this.mFocusButton.isEnabled)
		{
			this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
		}
	}

	private void CallBackAction(UIMissionWithTankerConfirmPopUp.ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject)
	{
		if (this.mUIMissionWithTankerConfirmPopUpAction != null)
		{
			this.mUIMissionWithTankerConfirmPopUpAction(actionType, calledObject);
		}
	}

	private void OnDestroy()
	{
		this.mButtonManager = null;
		this.mLabel_RequireDay = null;
		this.mLabel_RequireTanker = null;
		this.mLabel_SettingTankerValue = null;
		this.mLabel_HasTankerCount = null;
		this.mButton_Positive = null;
		this.mButton_Negative = null;
		this.mButton_Description = null;
		this.mButton_CountUp = null;
		this.mButton_CountDown = null;
		this.mLabel_Message = null;
		this.mFocusButton = null;
		this.mUIMissionWithTankerConfirmPopUpAction = null;
		this.mInitializeCoroutine = null;
		this.mSelectableButtons = null;
		this.mUIMissionShipBanners = null;
	}
}
