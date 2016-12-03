using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelSlotItemChangePreviewInParameter : MonoBehaviour
	{
		[SerializeField]
		private UISprite icon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelValue;

		private Color zeroColor = new Color(0.192156866f, 0.192156866f, 0.192156866f);

		private Color plusColor = new Color(0f, 0.6431373f, 1f);

		private Color minusColor = new Color(1f, 0f, 0f);

		public void Init(string name, int value, int sabun)
		{
			string text = string.Empty;
			if (name != string.Empty)
			{
				if (sabun > 0)
				{
					this.icon.spriteName = "icon_up";
					this.labelName.color = this.plusColor;
					this.labelValue.color = this.plusColor;
					text = "＋";
				}
				else if (sabun == 0)
				{
					this.icon.spriteName = "icon_steady";
					this.labelName.color = this.zeroColor;
					this.labelValue.color = this.zeroColor;
				}
				else
				{
					this.icon.spriteName = "icon_down";
					this.labelName.color = this.minusColor;
					this.labelValue.color = this.minusColor;
					text = "－";
				}
				this.labelValue.text = text + value.ToString();
				this.labelName.text = name;
			}
			else
			{
				this.icon.spriteName = string.Empty;
				this.labelName.color = this.zeroColor;
				this.labelValue.color = this.zeroColor;
				this.labelName.text = string.Empty;
				this.labelValue.text = string.Empty;
			}
		}

		private void OnDestroy()
		{
			this.icon = null;
			this.labelName = null;
			this.labelValue = null;
		}
	}
}
