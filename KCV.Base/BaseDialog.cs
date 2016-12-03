using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class BaseDialog : MonoBehaviour
	{
		private UIPanel mPanel;

		private void Awake()
		{
			this.mPanel = base.GetComponent<UIPanel>();
			this.mPanel.alpha = 0.01f;
		}

		protected void Show()
		{
			this.mPanel.alpha = 1f;
			new BaseDialogPopup().Open(base.get_gameObject(), 0f, 0f, 1f, 1f);
		}

		protected void Hide(Action callBack)
		{
			this.AnimationClose(callBack);
		}

		private void AnimationClose(Action action)
		{
			TweenScale scaleTween = UITweener.Begin<TweenScale>(base.get_gameObject(), 0.4f);
			Vector3 localScale = base.get_gameObject().get_transform().get_localScale();
			Vector3 zero = Vector3.get_zero();
			scaleTween.ignoreTimeScale = false;
			scaleTween.from = localScale;
			scaleTween.to = zero;
			scaleTween.SetOnFinished(delegate
			{
				if (action != null)
				{
					action.Invoke();
				}
				Object.Destroy(scaleTween);
			});
		}
	}
}
