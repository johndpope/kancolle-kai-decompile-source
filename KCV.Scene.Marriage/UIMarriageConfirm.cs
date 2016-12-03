using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Marriage
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIMarriageConfirm : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UILabel mLabel_FromRingValue;

		[SerializeField]
		private UILabel mLabel_ToRingValue;

		private KeyControl mKeyController;

		private Action mOnNegativeListener;

		private Action mOnPositiveListener;

		private UIButton mFocusButton;

		private void Awake()
		{
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				this.ChangeFocus(this.mButtonManager.nowForcusButton);
			};
			this.mButton_Negative.OnEnableAndOnDisableChangeState = true;
			this.mButton_Positive.OnEnableAndOnDisableChangeState = true;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocus(this.mButton_Positive);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.ChangeFocus(this.mButton_Negative);
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mFocusButton != null)
					{
						if (this.mFocusButton.Equals(this.mButton_Negative))
						{
							SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
							this.ClickNegativeEvent();
						}
						else if (this.mFocusButton.Equals(this.mButton_Positive))
						{
							SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
							this.ClickPositiveEvent();
						}
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.ClickNegativeEvent();
				}
				else if (this.mKeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (this.mKeyController.IsLDown())
				{
					this.ClickNegativeEvent();
				}
			}
		}

		public void Initialize(int fromRingValue, int toRingValue)
		{
			this.ChangeFocus(this.mButton_Positive);
			this.mLabel_FromRingValue.text = fromRingValue.ToString();
			this.mLabel_ToRingValue.text = toRingValue.ToString();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnNegativeListener(Action onNegativeListener)
		{
			this.mOnNegativeListener = onNegativeListener;
		}

		public void SetOnPositiveListener(Action onPositiveListener)
		{
			this.mOnPositiveListener = onPositiveListener;
		}

		[Obsolete("Inspector上で使用します")]
		public void TouchNegativeEvent()
		{
			if (this.mKeyController != null)
			{
				this.ClickNegativeEvent();
			}
		}

		[Obsolete("Inspector上で使用します")]
		public void TouchPositiveEvent()
		{
			if (this.mKeyController != null)
			{
				this.ClickPositiveEvent();
			}
		}

		private void ClickPositiveEvent()
		{
			if (this.mOnPositiveListener != null)
			{
				this.mOnPositiveListener.Invoke();
			}
		}

		private void ClickNegativeEvent()
		{
			if (this.mOnNegativeListener != null)
			{
				this.mOnNegativeListener.Invoke();
			}
		}

		private void ChangeFocus(UIButton target)
		{
			if (this.mFocusButton != null)
			{
				if (!this.mFocusButton.Equals(target))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
			}
			this.mFocusButton = target;
			if (this.mFocusButton != null)
			{
				this.mFocusButton.SetActive(false);
				this.mFocusButton.SetActive(true);
				this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
			}
		}

		private void OnDestroy()
		{
			this.mButtonManager = null;
			this.mButton_Negative = null;
			this.mButton_Positive = null;
			this.mLabel_FromRingValue = null;
			this.mLabel_ToRingValue = null;
			this.mKeyController = null;
			this.mOnNegativeListener = null;
			this.mOnPositiveListener = null;
			this.mFocusButton = null;
		}
	}
}
