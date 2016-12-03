using DG.Tweening;
using System;
using UnityEngine;

namespace KCV.Base
{
	[RequireComponent(typeof(UIPanel))]
	public class UICircleCategory<Model> : MonoBehaviour
	{
		private UIPanel mPanelThis;

		public int mDefaultDepth
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public Model Category
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
			this.mDefaultDepth = this.mPanelThis.depth;
		}

		public virtual void Initialize(int index, Model category)
		{
			this.Index = index;
			this.Category = category;
		}

		public void Show()
		{
			this.mPanelThis.alpha = 1f;
		}

		public void Hide()
		{
			this.mPanelThis.alpha = 0.01f;
		}

		public void SetDepth(int depth, bool addDefaultDepth)
		{
			this.mPanelThis.depth = depth + ((!addDefaultDepth) ? 0 : this.mDefaultDepth);
		}

		public int GetDepth()
		{
			return this.mPanelThis.depth;
		}

		public virtual void OnCenter(float animationTime, Ease easeType)
		{
		}

		public virtual void OnOutDisplay(float animationTime, Ease easeType, Action onFinished)
		{
		}

		public virtual void OnInDisplay(float animationTime, Ease easeType, Action onFinished)
		{
		}

		public virtual void OnOtherThanCenter(float animationTime, Ease easeType)
		{
		}

		public virtual void OnFirstDisplay(float animationTime, bool isCenter, Ease easeType)
		{
		}

		public virtual void OnSelectAnimation(Action onFinishedAnimation)
		{
		}
	}
}
