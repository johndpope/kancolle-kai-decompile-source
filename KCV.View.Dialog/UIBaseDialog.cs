using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.Dialog
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class UIBaseDialog : MonoBehaviour
	{
		public enum EventType
		{
			BeginInitialize,
			BeginShow,
			BeginHide,
			Initialized,
			Shown,
			Hidden
		}

		private const float HIDE_ANIMATION_TIME = 0.4f;

		[SerializeField]
		private Vector3 SHOW_START_SCALE = new Vector3(0.9f, 0.9f, 1f);

		[SerializeField]
		private Vector3 SHOWN_SCALE = new Vector3(1f, 1f, 1f);

		private Coroutine mInitializeCoroutine;

		private Coroutine mShowCoroutine;

		private Coroutine mHideCoroutine;

		private UIPanel mPanelThis;

		private void Awake()
		{
			DOTween.Init(default(bool?), default(bool?), default(LogBehaviour?));
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
		}

		public void Begin()
		{
			this.RemoveInitializeCoroutine();
			this.mInitializeCoroutine = base.StartCoroutine(this.InitializeCoroutine(delegate
			{
				this.RemoveInitializeCoroutine();
				this.OnCallEventCoroutine(UIBaseDialog.EventType.Initialized, this);
			}));
		}

		public void Show()
		{
			this.RemoveShowCoroutine();
			this.mShowCoroutine = base.StartCoroutine(this.ShowCoroutine(delegate
			{
				this.RemoveShowCoroutine();
				base.get_transform().set_localScale(this.SHOW_START_SCALE);
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), this.SHOWN_SCALE, 0.4f), 21), delegate
				{
					this.OnCallEventCoroutine(UIBaseDialog.EventType.Shown, this);
				});
				DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.4f, delegate(float alpha)
				{
					this.mPanelThis.alpha = alpha;
				});
			}));
		}

		public void Hide()
		{
			this.RemoveHideCoroutine();
			this.mHideCoroutine = base.StartCoroutine(this.HideCoroutine(delegate
			{
				this.RemoveHideCoroutine();
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), this.SHOW_START_SCALE, 0.4f), 20), delegate
				{
					this.OnCallEventCoroutine(UIBaseDialog.EventType.Hidden, this);
				});
				DOVirtual.Float(this.mPanelThis.alpha, 0.01f, 0.4f, delegate(float alpha)
				{
					this.mPanelThis.alpha = alpha;
				});
			}));
		}

		[DebuggerHidden]
		private IEnumerator InitializeCoroutine(Action onFinished)
		{
			UIBaseDialog.<InitializeCoroutine>c__Iterator48 <InitializeCoroutine>c__Iterator = new UIBaseDialog.<InitializeCoroutine>c__Iterator48();
			<InitializeCoroutine>c__Iterator.onFinished = onFinished;
			<InitializeCoroutine>c__Iterator.<$>onFinished = onFinished;
			<InitializeCoroutine>c__Iterator.<>f__this = this;
			return <InitializeCoroutine>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ShowCoroutine(Action onFinished)
		{
			UIBaseDialog.<ShowCoroutine>c__Iterator49 <ShowCoroutine>c__Iterator = new UIBaseDialog.<ShowCoroutine>c__Iterator49();
			<ShowCoroutine>c__Iterator.onFinished = onFinished;
			<ShowCoroutine>c__Iterator.<$>onFinished = onFinished;
			<ShowCoroutine>c__Iterator.<>f__this = this;
			return <ShowCoroutine>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator HideCoroutine(Action onFinished)
		{
			UIBaseDialog.<HideCoroutine>c__Iterator4A <HideCoroutine>c__Iterator4A = new UIBaseDialog.<HideCoroutine>c__Iterator4A();
			<HideCoroutine>c__Iterator4A.onFinished = onFinished;
			<HideCoroutine>c__Iterator4A.<$>onFinished = onFinished;
			<HideCoroutine>c__Iterator4A.<>f__this = this;
			return <HideCoroutine>c__Iterator4A;
		}

		private void RemoveShowCoroutine()
		{
			if (this.mShowCoroutine != null)
			{
				base.StopCoroutine(this.mShowCoroutine);
				this.mShowCoroutine = null;
			}
		}

		private void RemoveInitializeCoroutine()
		{
			if (this.mInitializeCoroutine != null)
			{
				base.StopCoroutine(this.mInitializeCoroutine);
				this.mInitializeCoroutine = null;
			}
		}

		private void RemoveHideCoroutine()
		{
			if (this.mHideCoroutine != null)
			{
				base.StopCoroutine(this.mHideCoroutine);
				this.mHideCoroutine = null;
			}
		}

		protected abstract Coroutine OnCallEventCoroutine(UIBaseDialog.EventType actionType, UIBaseDialog calledObject);
	}
}
