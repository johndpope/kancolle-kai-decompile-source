using KCV;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UICheckBaseDialogManager : MonoBehaviour
{
	public enum KEY_FOCUS_TYPE
	{
		NONE,
		THIS,
		DIALOG
	}

	private KeyControl mKeyController;

	[SerializeField]
	private UICheckBaseDialog mPrefab_UICheckBaseDialog;

	[SerializeField]
	private Camera mModalCamera;

	private UICheckBaseDialogManager.KEY_FOCUS_TYPE mKeyFocusType;

	private DeckModel mDeckModel;

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UICheckBaseDialogManager.<Start>c__Iterator4E <Start>c__Iterator4E = new UICheckBaseDialogManager.<Start>c__Iterator4E();
		<Start>c__Iterator4E.<>f__this = this;
		return <Start>c__Iterator4E;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			this.mKeyController.Update();
			if (this.mKeyFocusType == UICheckBaseDialogManager.KEY_FOCUS_TYPE.THIS)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					base.StartCoroutine(this.UICheckBaseDialogBegin());
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					Application.LoadLevel("Strategy");
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator UICheckBaseDialogBegin()
	{
		UICheckBaseDialogManager.<UICheckBaseDialogBegin>c__Iterator4F <UICheckBaseDialogBegin>c__Iterator4F = new UICheckBaseDialogManager.<UICheckBaseDialogBegin>c__Iterator4F();
		<UICheckBaseDialogBegin>c__Iterator4F.<>f__this = this;
		return <UICheckBaseDialogBegin>c__Iterator4F;
	}

	private void UICheckBaseDialogAction(UICheckBaseDialog.ActionType actionType, UICheckBaseDialog dialog)
	{
		switch (actionType)
		{
		case UICheckBaseDialog.ActionType.Shown:
			this.ChangeKeyController(UICheckBaseDialogManager.KEY_FOCUS_TYPE.DIALOG);
			dialog.SetKeyController(this.mKeyController);
			break;
		case UICheckBaseDialog.ActionType.BeginHide:
			dialog.SetKeyController(null);
			break;
		case UICheckBaseDialog.ActionType.Hidden:
			this.ChangeKeyController(UICheckBaseDialogManager.KEY_FOCUS_TYPE.THIS);
			Object.Destroy(dialog.get_gameObject());
			break;
		}
	}

	private void ChangeKeyController(UICheckBaseDialogManager.KEY_FOCUS_TYPE nextKeyFocusType)
	{
		this.mKeyFocusType = nextKeyFocusType;
		if (this.mKeyController != null)
		{
			this.mKeyController.firstUpdate = true;
		}
	}
}
