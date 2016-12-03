using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutySummary : BaseUISummary<DutyModel>
	{
		public enum SelectType
		{
			Action,
			CallDetail,
			Back,
			Hover
		}

		public delegate void UIDutySummaryAction(UIDutySummary.SelectType type, UIDutySummary summary);

		[SerializeField]
		private UISprite mSpriteType;

		[SerializeField]
		private UILabel mLabelTitle;

		[SerializeField]
		private UIDutyStatus mDutyStatus;

		[SerializeField]
		private UIButton mButtonAction;

		[SerializeField]
		private TweenScale IconAnim;

		private KeyControl mKeyController;

		private UIButton mFocusButton;

		private UIDutySummary.UIDutySummaryAction mDutySummaryActionCallBack;

		public void SetCallBackSummaryAction(UIDutySummary.UIDutySummaryAction callBack)
		{
			this.mDutySummaryActionCallBack = callBack;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mFocusButton != null)
					{
						this.mButtonAction.SendMessage("OnClick");
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.CallBackAction(UIDutySummary.SelectType.Back);
				}
			}
		}

		private void InitializeButtonColor(UIButton target)
		{
			target.hover = Util.CursolColor;
			target.defaultColor = Color.get_white();
			target.pressed = Color.get_white();
			target.disabledColor = Color.get_white();
		}

		public override void Initialize(int index, DutyModel model)
		{
			base.Initialize(index, model);
			this.InitializeButtonColor(this.mButtonAction);
			this.mLabelTitle.text = model.Title;
			this.mDutyStatus.Initialize(model);
			this.mSpriteType.spriteName = this.GetSpriteNameDutyType(model.Category);
		}

		private string GetSpriteNameDutyType(int category)
		{
			int num = 0;
			switch (category)
			{
			case 1:
				num = 6;
				break;
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			case 4:
				num = 3;
				break;
			case 5:
				num = 4;
				break;
			case 6:
				num = 7;
				break;
			case 7:
				num = 5;
				break;
			}
			return string.Format("duty_tag{0}", num);
		}

		public override void Hover()
		{
			base.Hover();
			this.mButtonAction.SafeGetTweenScale(base.get_gameObject().get_transform().get_localScale(), new Vector3(1.04f, 1.04f, 1f), 0.1f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			this.mButtonAction.ResetDefaultColor();
			this.mButtonAction.SetState(UIButtonColor.State.Hover, true);
			this.mButtonAction.defaultColor = this.mButtonAction.hover;
			this.IconAnim.set_enabled(true);
			UISelectedObject.SelectedOneObjectBlink(this.mButtonAction.get_transform().FindChild("Background").get_gameObject(), true);
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			this.mButtonAction.SafeGetTweenScale(new Vector3(1f, 1f, 1f), base.get_gameObject().get_transform().get_localScale(), 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			this.mButtonAction.ResetDefaultColor();
			this.mButtonAction.SetState(UIButtonColor.State.Normal, true);
			this.IconAnim.set_enabled(false);
			this.IconAnim.get_transform().set_localScale(Vector3.get_one());
			UISelectedObject.SelectedOneObjectBlink(this.mButtonAction.get_transform().FindChild("Background").get_gameObject(), false);
		}

		public void OnClickAction()
		{
			this.CallBackAction(UIDutySummary.SelectType.Action);
		}

		public void OnClickCallDetail()
		{
			this.CallBackAction(UIDutySummary.SelectType.CallDetail);
		}

		private void CallBackAction(UIDutySummary.SelectType type)
		{
			if (this.mDutySummaryActionCallBack != null)
			{
				this.mDutySummaryActionCallBack(type, this);
			}
		}
	}
}
