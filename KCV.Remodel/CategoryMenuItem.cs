using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UITexture))]
	public class CategoryMenuItem : UIToggle
	{
		private const float notFocusedScale = 0.9f;

		private const float focusedScale = 1.1f;

		private const int focusedDepth = 50;

		[SerializeField]
		private Texture onTexture;

		[SerializeField]
		private Texture offTexture;

		private CategoryMenu parent;

		private UITexture texture;

		private int originDepth;

		public int index
		{
			get;
			private set;
		}

		public void Awake()
		{
			this.texture = base.get_transform().GetComponent<UITexture>();
			this.originDepth = this.texture.depth;
		}

		public void Init(CategoryMenu parent, int index, bool enabled)
		{
			this.parent = parent;
			this.index = index;
			this.group = parent.group;
			base.set_enabled(enabled);
			this.onChange.Clear();
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.OnValueChange));
			this.texture.mainTexture = ((!enabled) ? this.offTexture : this.onTexture);
			base.get_transform().localScaleX(0.9f);
			base.get_transform().localScaleY(0.9f);
		}

		public virtual void OnClick()
		{
			if (base.get_enabled())
			{
				this.parent.OnItemClick(this);
			}
		}

		public void OnValueChange()
		{
			if (base.value)
			{
				base.get_transform().localScaleX(1.1f);
				base.get_transform().localScaleY(1.1f);
				this.texture.depth = 50;
			}
			else
			{
				base.get_transform().localScaleX(0.9f);
				base.get_transform().localScaleY(0.9f);
				this.texture.depth = this.originDepth;
			}
		}

		internal void Release()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.onTexture, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.offTexture, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.texture, false);
			this.parent = null;
		}
	}
}
