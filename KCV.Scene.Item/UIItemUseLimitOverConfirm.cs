using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIPanel))]
	public class UIItemUseLimitOverConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

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
			this.mPanelThis.alpha = 0f;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocus(this.mButton_Negative);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.ChangeFocus(this.mButton_Positive);
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
			this.ChangeFocus(this.mButton_Negative);
		}

		public void Show(Action onFinished)
		{
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
			if (this.mOnPositiveCallBack != null)
			{
				this.mOnPositiveCallBack.Invoke();
			}
		}

		public void SetOnNegativeCallBack(Action onBuyCancel)
		{
			this.mOnNegativeCallBack = onBuyCancel;
		}

		private void OnCancel()
		{
			if (this.mOnNegativeCallBack != null)
			{
				this.mOnNegativeCallBack.Invoke();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void ChangeFocus(UIButton targetButton)
		{
			if (this.mFocusButton != null)
			{
				this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
			}
			this.mFocusButton = targetButton;
			if (this.mFocusButton != null)
			{
				this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
			}
		}

		public void OnClickPositive()
		{
			this.ChangeFocus(this.mButton_Positive);
			this.Use();
		}

		private void Cancel()
		{
			this.OnCancel();
		}

		private void Use()
		{
			this.OnUse();
		}

		public void OnClickNegative()
		{
			this.ChangeFocus(this.mButton_Negative);
			this.Cancel();
		}

		public void OnTouchOther()
		{
			this.Cancel();
		}

		public void Release()
		{
			this.ChangeFocus(null);
		}
	}
}
