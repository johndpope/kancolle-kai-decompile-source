using System;
using UnityEngine;

namespace KCV.View
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class BoardDialog : MonoBehaviour
	{
		private UIPanel mPanel;

		private void Awake()
		{
			this.mPanel = base.GetComponent<UIPanel>();
			this.mPanel.alpha = 0.01f;
		}

		protected void Show()
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(base.get_gameObject(), 0.2f);
			tweenAlpha.from = 0.01f;
			tweenAlpha.to = 1f;
			tweenAlpha.ignoreTimeScale = false;
			tweenAlpha.PlayForward();
		}

		protected void Hide(Action callBack)
		{
			this.AnimationClose(callBack);
		}

		private void AnimationClose(Action action)
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(base.get_gameObject(), 0.3f);
			tweenAlpha.from = 1f;
			tweenAlpha.to = 0.01f;
			tweenAlpha.ignoreTimeScale = false;
			tweenAlpha.SetOnFinished(delegate
			{
				if (action != null)
				{
					action.Invoke();
				}
				Object.Destroy(tweenAlpha);
			});
			tweenAlpha.PlayForward();
		}

		private void OnDestroy()
		{
			this.mPanel = null;
		}
	}
}
