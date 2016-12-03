using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public class PageDialog : MonoBehaviour
	{
		protected UIPanel mPanel;

		private Vector3 mStartLocalPosition;

		private Vector3 mOutDisplayPosition;

		private void Awake()
		{
			this.mPanel = base.GetComponent<UIPanel>();
			this.mPanel.alpha = 0.01f;
			this.mStartLocalPosition = base.get_gameObject().get_transform().get_localPosition();
			this.mOutDisplayPosition = new Vector3(this.mStartLocalPosition.x + 960f, this.mStartLocalPosition.y, 0f);
		}

		private void Start()
		{
			base.get_transform().set_localPosition(this.mOutDisplayPosition);
		}

		protected void Show()
		{
			this.mPanel.alpha = 1f;
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_transform().get_gameObject(), 0.4f);
			tweenPosition.from = this.mOutDisplayPosition;
			tweenPosition.to = this.mStartLocalPosition;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				this.FinishedShowAnimation();
			});
		}

		protected void Show(Vector3 to)
		{
			this.mPanel.alpha = 1f;
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_transform().get_gameObject(), 0.4f);
			tweenPosition.from = this.mOutDisplayPosition;
			tweenPosition.to = to;
			tweenPosition.ignoreTimeScale = false;
			tweenPosition.SetOnFinished(delegate
			{
				this.FinishedShowAnimation();
			});
		}

		private void FinishedShowAnimation()
		{
			this.OnFinishedShowAnimation();
		}

		protected virtual void OnFinishedShowAnimation()
		{
		}

		protected void Hide(Action callBack)
		{
			this.AnimationClose(callBack);
		}

		private void AnimationClose(Action action)
		{
			TweenPosition moveTween = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.4f);
			Vector3 from = this.mStartLocalPosition;
			Vector3 to = this.mOutDisplayPosition;
			moveTween.ignoreTimeScale = false;
			moveTween.from = from;
			moveTween.to = to;
			moveTween.SetOnFinished(delegate
			{
				if (action != null)
				{
					action.Invoke();
				}
				Object.Destroy(moveTween);
			});
		}
	}
}
