using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuCenterButton : UIPortMenuButton, UIPortMenuButton.CompositeMenu
	{
		public enum State
		{
			MainMenu,
			SubMenu
		}

		[SerializeField]
		private UIPortMenuButton.UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap_SubMenu;

		[SerializeField]
		private Texture mTexture_MainBase;

		[SerializeField]
		private Texture mTexture_SubBase;

		[SerializeField]
		private float intervalAction = 1f;

		private UIPortMenuCenterButton.State mState;

		private float nextTime;

		private Action mOnLongPressListener;

		private bool LongPressed;

		public bool pressed
		{
			get;
			private set;
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_MainBase, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_SubBase, false);
			this.mUIPortMenuButtonKeyMap_SubMenu = null;
			this.mOnLongPressListener = null;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			this.mTexture_Base.mainTexture = this.mTexture_MainBase;
		}

		public void ChangeState(UIPortMenuCenterButton.State state)
		{
			this.mState = state;
			UIPortMenuCenterButton.State state2 = this.mState;
			if (state2 != UIPortMenuCenterButton.State.MainMenu)
			{
				if (state2 == UIPortMenuCenterButton.State.SubMenu)
				{
					Color color = new Color(1f, 0.5921569f, 0.196078435f, this.mTexture_Glow_Back.alpha);
					this.mTexture_Glow_Back.color = color;
					this.mTexture_Glow_Front.color = color;
					this.mTexture_Text.color = color;
					this.mTexture_Base.mainTexture = this.mTexture_SubBase;
				}
			}
			else
			{
				Color color2 = new Color(0.196078435f, 0.6039216f, 0.8392157f, this.mTexture_Glow_Back.alpha);
				this.mTexture_Glow_Back.color = color2;
				this.mTexture_Glow_Front.color = color2;
				this.mTexture_Text.color = color2;
				this.mTexture_Base.mainTexture = this.mTexture_MainBase;
			}
		}

		public UIPortMenuButton.UIPortMenuButtonKeyMap GetSubMenuKeyMap()
		{
			return this.mUIPortMenuButtonKeyMap_SubMenu;
		}

		private void Update()
		{
			if (!this.LongPressed && this.pressed && this.nextTime < Time.get_realtimeSinceStartup())
			{
				this.LongPressed = true;
				this.nextTime = Time.get_realtimeSinceStartup() + this.intervalAction;
				UICamera.selectedObject = null;
				if (this.mOnLongPressListener != null)
				{
					this.mOnLongPressListener.Invoke();
				}
			}
		}

		private void OnPress(bool pressed)
		{
			this.LongPressed = false;
			this.pressed = pressed;
			this.nextTime = Time.get_realtimeSinceStartup() + this.intervalAction;
		}

		public void SetOnLongPressListener(Action onLongPressListener)
		{
			this.mOnLongPressListener = onLongPressListener;
		}
	}
}
