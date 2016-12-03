using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuEngageButton : UIPortMenuButton, UIPortMenuButton.CompositeMenu
	{
		[SerializeField]
		private UIPortMenuButton.UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap_SubMenu;

		protected override void OnInitialize(bool isSelectable)
		{
			base.alpha = 0f;
		}

		public UIPortMenuButton.UIPortMenuButtonKeyMap GetSubMenuKeyMap()
		{
			return this.mUIPortMenuButtonKeyMap_SubMenu;
		}

		protected override void OnCallDestroy()
		{
			if (this.mUIPortMenuButtonKeyMap_SubMenu != null)
			{
				this.mUIPortMenuButtonKeyMap_SubMenu.Release();
			}
			this.mUIPortMenuButtonKeyMap_SubMenu = null;
		}
	}
}
