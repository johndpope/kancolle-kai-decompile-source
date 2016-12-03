using KCV.Utils;
using System;
using UnityEngine;

namespace KCV
{
	public class YesNoButton : MonoBehaviour
	{
		[SerializeField]
		private UIButtonManager mUIButtonManager;

		private KeyControl mKeyController;

		private Action mOnSelectNegativeListener;

		private Action mOnSelectPositiveListener;

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.mUIButtonManager.movePrevButton();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.mUIButtonManager.moveNextButton();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.mUIButtonManager.Decide();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnSelectNegative();
				}
			}
		}

		[Obsolete("Inspector上でボタンに設定する為に使用します")]
		public void OnTouchPositive()
		{
			this.OnSelectPositive();
		}

		[Obsolete("Inspector上でボタンに設定する為に使用します")]
		public void OnTouchNegative()
		{
			this.OnSelectNegative();
		}

		public void SetKeyController(KeyControl keyController, bool isFocusLeft = true)
		{
			this.mKeyController = keyController;
			App.OnlyController = this.mKeyController;
			int focus = (!isFocusLeft) ? 1 : 0;
			this.mUIButtonManager.setFocus(focus);
			this.mUIButtonManager.isPlaySE = true;
		}

		public void SetOnSelectNegativeListener(Action action)
		{
			this.mOnSelectNegativeListener = action;
		}

		public void SetOnSelectPositiveListener(Action action)
		{
			this.mOnSelectPositiveListener = action;
		}

		private void OnSelectNegative()
		{
			if (this.mOnSelectNegativeListener != null && this.mKeyController != null)
			{
				this.mKeyController = null;
				App.OnlyController = null;
				this.mOnSelectNegativeListener.Invoke();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		private void OnSelectPositive()
		{
			if (this.mOnSelectPositiveListener != null && this.mKeyController != null)
			{
				this.mKeyController = null;
				App.OnlyController = null;
				this.mOnSelectPositiveListener.Invoke();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		private void OnDestroy()
		{
			this.mKeyController = null;
			this.mUIButtonManager = null;
		}
	}
}
