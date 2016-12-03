using DG.Tweening;
using KCV;
using KCV.View.Dialog;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UICheckBaseDialog : UIBaseDialog
{
	public enum ActionType
	{
		Shown,
		BeginHide,
		Hidden
	}

	public delegate void UICheckBaseDialogAction(UICheckBaseDialog.ActionType actionType, UICheckBaseDialog dialog);

	private const float ANIMATION_TIME_HIDE = 0.8f;

	private KeyControl mKeyController;

	[SerializeField]
	private UITexture[] mTexture_Ships;

	[SerializeField]
	private UITexture[] mTexture_Cards;

	[SerializeField]
	private UILabel mLabel_Message;

	private DeckModel mDeckModel;

	private UICheckBaseDialog.UICheckBaseDialogAction mUICheckBaseDialogAction;

	public KeyControl GetKeyController()
	{
		if (this.mKeyController == null)
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
		}
		return this.mKeyController;
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(1).down)
			{
				base.Hide();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				base.Hide();
			}
		}
	}

	public void Begin(DeckModel deckModel)
	{
		this.mDeckModel = deckModel;
		base.Begin();
	}

	public void SetOnUICheckBaseDialogAction(UICheckBaseDialog.UICheckBaseDialogAction callBack)
	{
		this.mUICheckBaseDialogAction = callBack;
	}

	private void CallBackAction(UICheckBaseDialog.ActionType actionType, UICheckBaseDialog dialog)
	{
		if (this.mUICheckBaseDialogAction != null)
		{
			this.mUICheckBaseDialogAction(actionType, dialog);
		}
	}

	protected override Coroutine OnCallEventCoroutine(UIBaseDialog.EventType eventType, UIBaseDialog dialog)
	{
		switch (eventType)
		{
		case UIBaseDialog.EventType.BeginInitialize:
			return base.StartCoroutine(this.OnBeginInitializeCoroutine());
		case UIBaseDialog.EventType.BeginShow:
			return base.StartCoroutine(this.OnBeginShowCoroutine());
		case UIBaseDialog.EventType.BeginHide:
			return base.StartCoroutine(this.OnBeginHideCoroutine(delegate
			{
				this.CallBackAction(UICheckBaseDialog.ActionType.BeginHide, this);
			}));
		case UIBaseDialog.EventType.Initialized:
			this.OnInitialized(dialog);
			return null;
		case UIBaseDialog.EventType.Shown:
			this.OnShown();
			this.CallBackAction(UICheckBaseDialog.ActionType.Shown, this);
			return null;
		case UIBaseDialog.EventType.Hidden:
			this.CallBackAction(UICheckBaseDialog.ActionType.Hidden, this);
			return null;
		default:
			return null;
		}
	}

	private void OnInitialized(UIBaseDialog dialog)
	{
		dialog.Show();
	}

	private void OnShown()
	{
		Vector3 localPosition = new Vector3(this.mLabel_Message.get_transform().get_localPosition().x - 50f, this.mLabel_Message.get_transform().get_localPosition().y, this.mLabel_Message.get_transform().get_localPosition().z);
		Vector3 localPosition2 = this.mLabel_Message.get_transform().get_localPosition();
		this.mLabel_Message.get_transform().set_localPosition(localPosition);
		ShortcutExtensions.DOLocalMove(this.mLabel_Message.get_transform(), localPosition2, 0.3f, false);
		this.mLabel_Message.alpha = 1f;
	}

	[DebuggerHidden]
	private IEnumerator OnBeginInitializeCoroutine()
	{
		UICheckBaseDialog.<OnBeginInitializeCoroutine>c__Iterator4B <OnBeginInitializeCoroutine>c__Iterator4B = new UICheckBaseDialog.<OnBeginInitializeCoroutine>c__Iterator4B();
		<OnBeginInitializeCoroutine>c__Iterator4B.<>f__this = this;
		return <OnBeginInitializeCoroutine>c__Iterator4B;
	}

	[DebuggerHidden]
	private IEnumerator OnBeginShowCoroutine()
	{
		UICheckBaseDialog.<OnBeginShowCoroutine>c__Iterator4C <OnBeginShowCoroutine>c__Iterator4C = new UICheckBaseDialog.<OnBeginShowCoroutine>c__Iterator4C();
		<OnBeginShowCoroutine>c__Iterator4C.<>f__this = this;
		return <OnBeginShowCoroutine>c__Iterator4C;
	}

	[DebuggerHidden]
	private IEnumerator OnBeginHideCoroutine(Action onFinished)
	{
		UICheckBaseDialog.<OnBeginHideCoroutine>c__Iterator4D <OnBeginHideCoroutine>c__Iterator4D = new UICheckBaseDialog.<OnBeginHideCoroutine>c__Iterator4D();
		<OnBeginHideCoroutine>c__Iterator4D.onFinished = onFinished;
		<OnBeginHideCoroutine>c__Iterator4D.<$>onFinished = onFinished;
		<OnBeginHideCoroutine>c__Iterator4D.<>f__this = this;
		return <OnBeginHideCoroutine>c__Iterator4D;
	}
}
