using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampSetting : MonoBehaviour
	{
		public enum ActionType
		{
			CancelRevamp,
			StartRevamp
		}

		public delegate void UIRevampSettingAction(UIRevampSetting.ActionType actionType, UIRevampSetting calledObject);

		public delegate RevampValidationResult UIRevampSettingStateCheck(RevampRecipeDetailModel revampDetailModel);

		private const int LEVEL_MAX = 10;

		private UIRevampSetting.UIRevampSettingAction mUIRevampSettingActionCallBack;

		private UIRevampSetting.UIRevampSettingStateCheck mRevampSettingStateCheckDelegate;

		[SerializeField]
		private UISprite mSprite_RequireSlotItemState;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_Devkit;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UIButton mButton_Cancel;

		[SerializeField]
		private UIButton mButton_Switch;

		[SerializeField]
		private UIRevampIcon mRevampIcon;

		[SerializeField]
		private UIYouseiSwitch mYousei_Switch;

		[SerializeField]
		private UISprite[] mSprites_Star;

		[SerializeField]
		private Vector3 mVector3_HidePosition;

		[SerializeField]
		private Vector3 mVector3_ShowPosition;

		private UIPanel mPanelThis;

		private UIButton[] mButtonsFocusable;

		private RevampRecipeDetailModel mRevampRecipeDetailModel;

		private UIButton mButtonFocus;

		private KeyControl mKeyController;

		private UIButton _uiOverlayButton;

		private UIYouseiSwitch.ActionType mSwitchState;

		public void SetOnRevampSettingActionCallBack(UIRevampSetting.UIRevampSettingAction callBack)
		{
			this.mUIRevampSettingActionCallBack = callBack;
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
		}

		private void Start()
		{
			this.mPanelThis.alpha = 1f;
			this.mYousei_Switch.SetYouseiSwitchActionCallBack(new UIYouseiSwitch.UIYouseiSwitchAction(this.UIYouseiSwitchActionCallBack));
			this.mYousei_Switch.Enabled = true;
			this._uiOverlayButton = GameObject.Find("UIRevampSetting/OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton.onClick, new EventDelegate.Callback(this._onClickOverlayButton));
		}

		private void _onClickOverlayButton()
		{
			this.OnCallBack(UIRevampSetting.ActionType.CancelRevamp);
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.mButton_Cancel.SendMessage("OnClick");
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					int num = Array.IndexOf<UIButton>(this.mButtonsFocusable, this.mButtonFocus);
					int num2 = num - 1;
					if (0 <= num2)
					{
						this.ChangeFocusButton(this.mButtonsFocusable[num2]);
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num3 = Array.IndexOf<UIButton>(this.mButtonsFocusable, this.mButtonFocus);
					int num4 = num3 + 1;
					if (num4 < this.mButtonsFocusable.Length)
					{
						this.ChangeFocusButton(this.mButtonsFocusable[num4]);
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.mButtonFocus.SendMessage("OnClick");
				}
				else if (this.mKeyController.IsShikakuDown())
				{
					this.ChangeFocusButton(this.mButton_Switch);
					this.mYousei_Switch.ClickSwitch();
				}
			}
		}

		public void Initialize(RevampRecipeDetailModel recipeDetail, UIRevampSetting.UIRevampSettingStateCheck stateCheckDelegate, Camera prodCamera)
		{
			this.mSwitchState = UIYouseiSwitch.ActionType.OFF;
			this.mRevampSettingStateCheckDelegate = stateCheckDelegate;
			this.mRevampRecipeDetailModel = recipeDetail;
			this.mLabel_Name.text = recipeDetail.Slotitem.Name;
			if (0 < recipeDetail.RequiredSlotitemCount)
			{
				this.mSprite_RequireSlotItemState.spriteName = "txt_need_on";
			}
			else
			{
				this.mSprite_RequireSlotItemState.spriteName = "txt_need_off";
			}
			for (int i = 0; i < this.mRevampRecipeDetailModel.Slotitem.Level; i++)
			{
				this.mSprites_Star[i].spriteName = "icon_star";
			}
			this.mRevampIcon.Initialize(recipeDetail.Slotitem.MstId, recipeDetail.Slotitem.Level, prodCamera);
			this.UpdateRevampRecipeDetail(this.mRevampRecipeDetailModel);
			this.ChangeFocusButton(this.mButtonsFocusable[0]);
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (this.mButtonFocus != null)
			{
				this.mButtonFocus.SetState(UIButtonColor.State.Normal, true);
				if (this.mButtonFocus.get_name() == "Button_Switch")
				{
					UISelectedObject.SelectedOneObjectBlink(this.mButtonFocus, false);
				}
				else
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(this.mButtonFocus, false);
				}
			}
			this.mButtonFocus = target;
			if (this.mButtonFocus != null)
			{
				this.mButtonFocus.SetState(UIButtonColor.State.Hover, true);
				if (this.mButtonFocus.get_name() == "Button_Switch")
				{
					UISelectedObject.SelectedOneObjectBlink(this.mButtonFocus, true);
				}
				else
				{
					UISelectedObject.SelectedOneButtonZoomUpDown(this.mButtonFocus, true);
				}
			}
		}

		public bool IsDetermined()
		{
			UIYouseiSwitch.ActionType actionType = this.mSwitchState;
			return actionType != UIYouseiSwitch.ActionType.OFF && actionType == UIYouseiSwitch.ActionType.ON;
		}

		private void UIYouseiSwitchActionCallBack(UIYouseiSwitch.ActionType actionType)
		{
			if (actionType != UIYouseiSwitch.ActionType.OFF)
			{
				if (actionType == UIYouseiSwitch.ActionType.ON)
				{
					this.mSwitchState = UIYouseiSwitch.ActionType.ON;
					this.mRevampRecipeDetailModel.Determined = true;
				}
			}
			else
			{
				this.mSwitchState = UIYouseiSwitch.ActionType.OFF;
				this.mRevampRecipeDetailModel.Determined = false;
			}
			this.UpdateRevampRecipeDetail(this.mRevampRecipeDetailModel);
		}

		private void UpdateRevampRecipeDetail(RevampRecipeDetailModel recipeDetail)
		{
			RevampValidationResult revampValidationResult = this.mRevampSettingStateCheckDelegate(recipeDetail);
			List<UIButton> list = new List<UIButton>();
			RevampValidationResult revampValidationResult2 = revampValidationResult;
			if (revampValidationResult2 != RevampValidationResult.OK)
			{
				this.mButton_Start.SetState(UIButtonColor.State.Disabled, true);
				this.mButton_Start.isEnabled = false;
				list.Add(this.mButton_Cancel);
				list.Add(this.mButton_Switch);
				this.mButtonsFocusable = list.ToArray();
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
			else
			{
				this.mButton_Start.SetState(UIButtonColor.State.Normal, true);
				this.mButton_Start.isEnabled = true;
				list.Add(this.mButton_Cancel);
				list.Add(this.mButton_Switch);
				list.Add(this.mButton_Start);
				this.mButtonsFocusable = list.ToArray();
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			this.mLabel_Fuel.text = recipeDetail.Fuel.ToString();
			this.mLabel_Steel.text = recipeDetail.Steel.ToString();
			this.mLabel_Devkit.text = recipeDetail.DevKit.ToString();
			this.mLabel_Ammo.text = recipeDetail.Ammo.ToString();
			this.mLabel_Bauxite.text = recipeDetail.Baux.ToString();
			this.mLabel_RevampKit.text = recipeDetail.RevKit.ToString();
		}

		private void OnCallBack(UIRevampSetting.ActionType actionType)
		{
			if (this.mUIRevampSettingActionCallBack != null)
			{
				this.mUIRevampSettingActionCallBack(actionType, this);
			}
		}

		public void OnClickStartRevamp()
		{
			this.mKeyController = null;
			this._uiOverlayButton.isEnabled = false;
			this.OnCallBack(UIRevampSetting.ActionType.StartRevamp);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void OnClickCancelRevamp()
		{
			this.mKeyController = null;
			this._uiOverlayButton.isEnabled = false;
			this.OnCallBack(UIRevampSetting.ActionType.CancelRevamp);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public KeyControl GetKeyController()
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			return this.mKeyController;
		}

		public void Show(Action shownCallBack)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.3f);
			tweenPosition.from = this.mVector3_HidePosition;
			tweenPosition.to = this.mVector3_ShowPosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				if (shownCallBack != null)
				{
					shownCallBack.Invoke();
				}
			});
			tweenPosition.PlayForward();
		}

		public void Hide(Action hiddenCallBack)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.3f);
			tweenPosition.from = base.get_gameObject().get_transform().get_localPosition();
			tweenPosition.to = this.mVector3_HidePosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				if (hiddenCallBack != null)
				{
					hiddenCallBack.Invoke();
				}
			});
		}

		public RevampRecipeDetailModel GetRevampRecipeDetailModel()
		{
			return this.mRevampRecipeDetailModel;
		}

		private void OnDestroy()
		{
			this.mUIRevampSettingActionCallBack = null;
			this.mRevampSettingStateCheckDelegate = null;
			this.mSprite_RequireSlotItemState = null;
			this.mLabel_Name = null;
			this.mLabel_Fuel = null;
			this.mLabel_Steel = null;
			this.mLabel_Devkit = null;
			this.mLabel_Ammo = null;
			this.mLabel_Bauxite = null;
			this.mLabel_RevampKit = null;
			this.mButton_Start = null;
			this.mButton_Cancel = null;
			this.mButton_Switch = null;
			this.mRevampIcon = null;
			this.mYousei_Switch = null;
			this.mSprites_Star = null;
			this.mPanelThis = null;
			this.mButtonsFocusable = null;
			this.mRevampRecipeDetailModel = null;
			this.mButtonFocus = null;
			this.mKeyController = null;
			this._uiOverlayButton = null;
		}
	}
}
