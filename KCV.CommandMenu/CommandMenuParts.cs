using System;
using UnityEngine;

namespace KCV.CommandMenu
{
	public class CommandMenuParts : MonoBehaviour
	{
		public enum MenuState
		{
			Forcus,
			NonForcus,
			Disable
		}

		[SerializeField]
		private UISprite Lamp;

		[SerializeField]
		private UISprite MenuNameSprite;

		[SerializeField]
		private UISprite MenuBase;

		[SerializeField]
		private UIPanel panel;

		[SerializeField]
		private BoxCollider2D col;

		public string MenuNameStr;

		private int nowRotateZ;

		public TweenRotation tweenRot;

		private TweenAlpha LampAnim;

		public bool isDontGo;

		public CommandMenuParts.MenuState menuState
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.menuState = CommandMenuParts.MenuState.NonForcus;
			this.MenuNameStr = this.MenuNameSprite.spriteName;
			this.LampAnim = this.Lamp.AddComponent<TweenAlpha>();
			this.LampAnim.from = 0.2f;
			this.LampAnim.to = 1f;
			this.LampAnim.duration = 0.8f;
			this.LampAnim.style = UITweener.Style.PingPong;
			this.LampAnim.set_enabled(false);
		}

		public void SetMenuState(CommandMenuParts.MenuState state)
		{
			this.menuState = state;
			switch (this.menuState)
			{
			case CommandMenuParts.MenuState.Forcus:
				this.MenuNameSprite.spriteName = this.MenuNameStr + "_on";
				this.MenuNameSprite.MakePixelPerfect();
				this.MenuBase.spriteName = "menu_bar";
				this.Lamp.spriteName = "lamp_on";
				this.LampAnim.set_enabled(true);
				this.panel.depth = 3;
				this.col.set_enabled(true);
				break;
			case CommandMenuParts.MenuState.NonForcus:
				this.MenuNameSprite.spriteName = this.MenuNameStr;
				this.MenuNameSprite.MakePixelPerfect();
				this.Lamp.spriteName = "lamp_off";
				this.LampAnim.set_enabled(false);
				this.panel.depth = 2;
				this.col.set_enabled(false);
				break;
			case CommandMenuParts.MenuState.Disable:
				this.MenuBase.spriteName = "menu_bar_off";
				this.MenuNameSprite.spriteName = this.MenuNameStr + "_off";
				this.LampAnim.set_enabled(false);
				this.Lamp.spriteName = "lamp_off";
				this.panel.depth = -1;
				this.col.set_enabled(false);
				break;
			}
			if (this.isDontGo)
			{
				this.MenuBase.alpha = 0.8f;
			}
		}

		public void updateSprite(bool isFocus)
		{
			string text = (!isFocus) ? "_off" : "_on";
			this.MenuBase.spriteName = "menu_bar" + text;
			this.MenuNameSprite.spriteName = this.MenuNameStr + text;
			this.MenuNameSprite.MakePixelPerfect();
		}

		public void setColider(bool isEnable)
		{
			this.col.set_enabled(isEnable);
		}
	}
}
