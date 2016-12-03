using System;
using UnityEngine;

namespace KCV.Battle
{
	public class UIAdvancingWithDrawalButton : UIHexButtonEx
	{
		[SerializeField]
		private UISprite _uiLabelSprite;

		[SerializeField]
		private UISprite _uiLabelSubSprite;

		protected override void OnInit()
		{
			switch (this.index)
			{
			case 0:
				this._uiLabelSprite.spriteName = "txt_escape_off";
				break;
			case 1:
				this._uiLabelSprite.spriteName = "txt_go_off";
				break;
			case 2:
				this._uiLabelSprite.spriteName = "txt_go_off";
				this._uiLabelSubSprite.spriteName = "txt_kessen_off";
				break;
			}
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref this._uiLabelSprite);
			Mem.Del(ref this._uiLabelSubSprite);
		}

		protected override void SetForeground()
		{
			switch (this.index)
			{
			case 0:
				this._uiLabelSprite.spriteName = string.Format("txt_escape_{0}", (!this.toggle.value) ? "off" : "on");
				break;
			case 1:
				this._uiLabelSprite.spriteName = string.Format("txt_go_{0}", (!this.toggle.value) ? "off" : "on");
				break;
			case 2:
				this._uiLabelSprite.spriteName = string.Format("txt_go_{0}", (!this.toggle.value) ? "off" : "on");
				this._uiLabelSubSprite.spriteName = string.Format("txt_kessen_{0}", (!this.toggle.value) ? "off" : "on");
				break;
			}
		}
	}
}
