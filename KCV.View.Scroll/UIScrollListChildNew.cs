using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class UIScrollListChildNew<MODEL, VIEW> : UIToggle where MODEL : class where VIEW : UIScrollListChildNew<MODEL, VIEW>
	{
		[SerializeField]
		private UIWidget background;

		protected UIScrollListParentNew<MODEL, VIEW> parent;

		private Coroutine initCoroutine;

		public MODEL model
		{
			get;
			private set;
		}

		public int modelIndex
		{
			get;
			private set;
		}

		public bool visible
		{
			get;
			private set;
		}

		protected void Awake()
		{
			EventDelegate eventDelegate = new EventDelegate();
			eventDelegate.target = this;
			eventDelegate.methodName = "UpdateHover";
			this.onChange.Clear();
			this.onChange.Add(eventDelegate);
		}

		public void Init(UIScrollListParentNew<MODEL, VIEW> parent, MODEL model, int modelIndex)
		{
			this.parent = parent;
			this.model = model;
			this.modelIndex = modelIndex;
			if (model != null)
			{
				if (this.initCoroutine != null)
				{
					base.StopCoroutine(this.initCoroutine);
					this.initCoroutine = null;
				}
				this.initCoroutine = base.StartCoroutine(this.InitializeCoroutine(model));
			}
		}

		[DebuggerHidden]
		protected virtual IEnumerator InitializeCoroutine(MODEL model)
		{
			return new UIScrollListChildNew<MODEL, VIEW>.<InitializeCoroutine>c__Iterator51();
		}

		public virtual void Show()
		{
			this.visible = true;
			for (int i = 0; i < base.get_gameObject().get_transform().get_childCount(); i++)
			{
				base.get_gameObject().get_transform().GetChild(i).SetActive(true);
			}
		}

		public virtual void Hide()
		{
			this.visible = false;
			for (int i = 0; i < base.get_gameObject().get_transform().get_childCount(); i++)
			{
				base.get_gameObject().get_transform().GetChild(i).SetActive(false);
			}
		}

		public void UpdateLocalPosition(float x, float y, float z)
		{
			base.get_transform().set_localPosition(new Vector3(x, y, z));
		}

		public void DoSelect()
		{
			base.Set(true);
		}

		public void OnClick()
		{
			if (this.visible)
			{
				base.Set(true);
				this.parent.OnChildSelect(this);
			}
		}

		public void UpdateHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.background.get_gameObject(), base.value);
		}

		public void HideHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.background.get_gameObject(), false);
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void OnDestroy()
		{
			this.OnCallDestroy();
			if (this.background != null)
			{
				this.background.RemoveFromPanel();
			}
			this.background = null;
			this.parent = null;
			if (this.initCoroutine != null)
			{
				base.StopCoroutine(this.initCoroutine);
			}
			this.initCoroutine = null;
			this.model = (MODEL)((object)null);
		}
	}
}
