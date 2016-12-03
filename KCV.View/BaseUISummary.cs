using System;
using UnityEngine;

namespace KCV.View
{
	[RequireComponent(typeof(UIButton)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(UIPanel))]
	public class BaseUISummary<Model> : MonoBehaviour where Model : class
	{
		protected UIPanel mPanelThis;

		private UIButton mButtonThis;

		[SerializeField]
		private int mIndex;

		private Model mModel;

		public UIPanel GetPanel()
		{
			return this.mPanelThis;
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mButtonThis = base.GetComponent<UIButton>();
			this.mPanelThis.alpha = 0.01f;
		}

		public virtual void Initialize(int index, Model model)
		{
			this.mIndex = index;
			this.mModel = model;
		}

		public virtual void InitializeDefault(int index, Model model)
		{
			this.mIndex = index;
			this.mModel = model;
		}

		public virtual void Show()
		{
			this.mPanelThis.alpha = 1f;
		}

		public virtual void Hide()
		{
			this.mPanelThis.alpha = 0.01f;
		}

		public Model GetModel()
		{
			return this.mModel;
		}

		public int GetIndex()
		{
			return this.mIndex;
		}

		public virtual KeyControl Focus()
		{
			return null;
		}

		public virtual void RemoveFocus()
		{
		}

		public virtual void Hover()
		{
		}

		public virtual void RemoveHover()
		{
		}

		public virtual bool CanFocus()
		{
			return false;
		}

		public virtual void Clear()
		{
		}

		public void DepthFront()
		{
			this.mPanelThis.depth++;
		}

		public void SetDepth(int depth)
		{
			this.mPanelThis.depth = depth;
		}

		public void DepthBack()
		{
			this.mPanelThis.depth--;
		}
	}
}
