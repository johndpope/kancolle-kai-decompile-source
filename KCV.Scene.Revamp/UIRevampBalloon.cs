using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampBalloon : MonoBehaviour
	{
		public class MessageBuilder
		{
			private string mMessage;

			public MessageBuilder(int defaultR, int defaultG, int defaultB)
			{
				this.mMessage = this.GenerateColorTag(defaultR, defaultG, defaultB);
			}

			public UIRevampBalloon.MessageBuilder AddMessage(string message)
			{
				return this.AddMessage(message, true);
			}

			public UIRevampBalloon.MessageBuilder AddMessage(string message, bool lineBreak)
			{
				this.mMessage += message;
				if (lineBreak)
				{
					this.mMessage += "\n";
				}
				return this;
			}

			public UIRevampBalloon.MessageBuilder AddMessage(string message, bool lineBreak, int r, int g, int b)
			{
				this.mMessage = this.mMessage + this.GenerateColorTag(r, g, b) + message + "[-]";
				if (lineBreak)
				{
					this.mMessage += "\n";
				}
				return this;
			}

			public UIRevampBalloon.MessageBuilder AddWait(int waitSecond)
			{
				this.mMessage += "[W]";
				return this;
			}

			public UIRevampBalloon.MessageBuilder AddLineBreak(int value)
			{
				for (int i = 0; i < value; i++)
				{
					this.mMessage += "\n";
				}
				return this;
			}

			private string GenerateColorTag(int defaultR, int defaultG, int defaultB)
			{
				return string.Format("[{0:X2}{1:X2}{2:X2}]", defaultR, defaultG, defaultB);
			}

			public string Build()
			{
				return this.mMessage;
			}
		}

		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UISprite mSprite_Balloon;

		private Coroutine mAnimationCoroutine;

		[SerializeField]
		private Transform mTransform_TouchNextArea;

		public float alpha
		{
			set
			{
				if (this.mPanelThis != null)
				{
					this.mPanelThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
		}

		public UIRevampBalloon.MessageBuilder GetMessageBuilder()
		{
			return new UIRevampBalloon.MessageBuilder(0, 0, 0);
		}

		public void SayMessage(string message)
		{
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			this.mAnimationCoroutine = base.StartCoroutine(this.SayMessageCoroutine(message, delegate
			{
				this.mAnimationCoroutine = null;
			}));
		}

		public KeyControl SayMessage(string message, Action keyActionCallBack)
		{
			KeyControl keyController2 = new KeyControl(0, 0, 0.4f, 0.1f);
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			KeyControl keyController = keyController2;
			this.mAnimationCoroutine = base.StartCoroutine(this.SayMessageCoroutine(message, delegate
			{
				this.mAnimationCoroutine = null;
				this.StartCoroutine(this.WaitKey(keyController, KeyControl.KeyName.MARU, keyActionCallBack));
			}));
			return keyController;
		}

		public void OnTouchNextArea()
		{
			this.mTransform_TouchNextArea.SetActive(false);
		}

		[DebuggerHidden]
		private IEnumerator WaitKey(KeyControl keyController, KeyControl.KeyName waitKey, Action callBack)
		{
			UIRevampBalloon.<WaitKey>c__IteratorC4 <WaitKey>c__IteratorC = new UIRevampBalloon.<WaitKey>c__IteratorC4();
			<WaitKey>c__IteratorC.keyController = keyController;
			<WaitKey>c__IteratorC.waitKey = waitKey;
			<WaitKey>c__IteratorC.callBack = callBack;
			<WaitKey>c__IteratorC.<$>keyController = keyController;
			<WaitKey>c__IteratorC.<$>waitKey = waitKey;
			<WaitKey>c__IteratorC.<$>callBack = callBack;
			<WaitKey>c__IteratorC.<>f__this = this;
			return <WaitKey>c__IteratorC;
		}

		public bool IsAnimationNow()
		{
			return this.mAnimationCoroutine != null;
		}

		[DebuggerHidden]
		private IEnumerator SayMessageCoroutine(string message, Action finished)
		{
			UIRevampBalloon.<SayMessageCoroutine>c__IteratorC5 <SayMessageCoroutine>c__IteratorC = new UIRevampBalloon.<SayMessageCoroutine>c__IteratorC5();
			<SayMessageCoroutine>c__IteratorC.message = message;
			<SayMessageCoroutine>c__IteratorC.finished = finished;
			<SayMessageCoroutine>c__IteratorC.<$>message = message;
			<SayMessageCoroutine>c__IteratorC.<$>finished = finished;
			<SayMessageCoroutine>c__IteratorC.<>f__this = this;
			return <SayMessageCoroutine>c__IteratorC;
		}

		private void OnDestroy()
		{
			this.mPanelThis = null;
			this.mLabel_Message = null;
			this.mSprite_Balloon = null;
			this.mAnimationCoroutine = null;
		}
	}
}
