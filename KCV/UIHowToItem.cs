using System;
using UnityEngine;

namespace KCV
{
	public class UIHowToItem : MonoBehaviour
	{
		[SerializeField]
		private UISprite icon;

		[SerializeField]
		private UILabel label;

		public int GetWidth()
		{
			return this.icon.width + this.label.width;
		}

		public void Init(string spriteName, string labelText, int depth)
		{
			this.icon.spriteName = spriteName;
			this.icon.MakePixelPerfect();
			this.label.text = labelText;
			this.icon.depth = depth;
			this.label.depth = depth;
			this.label.get_transform().localPositionX((float)(this.icon.width + 4));
		}

		private void OnDestroy()
		{
			this.icon = null;
			this.label = null;
		}
	}
}
