using KCV;
using KCV.Utils;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(UIButtonManager))]
public class UIItemUseConfirm : MonoBehaviour
{
	private UIPanel mPanelThis;

	private UIButtonManager mButtonManager;

	private KeyControl mKeyController;

	[SerializeField]
	private UIButton mButton_Negative;

	[SerializeField]
	private UIButton mButton_Positive;

	[SerializeField]
	private DialogAnimation mDialogAnimation;

	private UIButton mFocusButton;

	private Action mOnPositiveCallBack;

	private Action mOnNegativeCallBack;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mButtonManager = base.GetComponent<UIButtonManager>();
		this.mPanelThis.alpha = 0f;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
			{
				this.ChangeFocus(this.mButton_Positive, true);
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				this.ChangeFocus(this.mButton_Negative, true);
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				if (this.mButton_Negative.Equals(this.mFocusButton))
				{
					this.Cancel();
				}
				else if (this.mButton_Positive.Equals(this.mFocusButton))
				{
					this.Use();
				}
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.Cancel();
			}
		}
	}

	public void Initialize()
	{
		this.ChangeFocus(this.mButton_Positive, false);
		this.mButtonManager.IndexChangeAct = delegate
		{
			this.ChangeFocus(this.mButtonManager.nowForcusButton, false);
		};
	}

	public void Show(Action onFinished)
	{
		this.ChangeFocus(this.mButton_Positive, false);
		this.mPanelThis.alpha = 1f;
		if (!this.mDialogAnimation.IsOpen)
		{
			this.mDialogAnimation.OpenAction = delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			};
			this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, true);
		}
	}

	public void Close(Action onFinished)
	{
		if (this.mDialogAnimation.IsOpen)
		{
			this.mDialogAnimation.CloseAction = delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
				this.mPanelThis.alpha = 0f;
			};
			this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, false);
		}
	}

	public void SetOnPositiveCallBack(Action onBuyCallBack)
	{
		this.mOnPositiveCallBack = onBuyCallBack;
	}

	private void OnUse()
	{
		bool flag = this.mKeyController != null;
		if (flag)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
			if (this.mOnPositiveCallBack != null)
			{
				this.mOnPositiveCallBack.Invoke();
			}
		}
	}

	public void SetOnNegativeCallBack(Action onBuyCancel)
	{
		this.mOnNegativeCallBack = onBuyCancel;
	}

	private void OnCancel()
	{
		bool flag = this.mKeyController != null;
		if (flag && this.mOnNegativeCallBack != null)
		{
			this.mOnNegativeCallBack.Invoke();
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	private void ChangeFocus(UIButton targetButton, bool playSE)
	{
		if (this.mFocusButton != null)
		{
			if (this.mFocusButton.Equals(targetButton))
			{
				return;
			}
			this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
		}
		this.mFocusButton = targetButton;
		if (this.mFocusButton != null)
		{
			if (playSE)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_018);
			}
			this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
		}
	}

	public void OnClickPositive()
	{
		this.ChangeFocus(this.mButton_Positive, false);
		this.Use();
	}

	private void Cancel()
	{
		bool flag = this.mKeyController != null;
		if (flag)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.OnCancel();
		}
	}

	private void Use()
	{
		this.OnUse();
	}

	public void OnClickNegative()
	{
		this.ChangeFocus(this.mButton_Negative, false);
		this.Cancel();
	}

	public void Release()
	{
		this.ChangeFocus(null, false);
	}

	public void OnTouchOther()
	{
		this.Cancel();
	}
}
