using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class Arsenal_DevKit : MonoBehaviour
	{
		public UIButtonManager _buttonManager;

		[SerializeField]
		private UIButton[] _switches;

		[SerializeField]
		private UISprite _selecter;

		public void Init()
		{
			if (this._buttonManager == null)
			{
				this._buttonManager = base.get_transform().FindChild("Switches").GetComponent<UIButtonManager>();
			}
			this._switches = new UIButton[3];
			for (int i = 0; i < 3; i++)
			{
				if (this._switches[i] == null)
				{
					this._switches[i] = this._buttonManager.get_transform().FindChild("Switch" + (i + 1)).GetComponent<UIButton>();
				}
			}
			if (this._selecter == null)
			{
				this._selecter = base.get_transform().FindChild("FrameSelect").GetComponent<UISprite>();
			}
			this.setSwitch(0);
			this._buttonManager.IsFocusButtonAlwaysHover = true;
		}

		public void setSwitch(int switchNo)
		{
			this._buttonManager.nowForcusIndex = switchNo;
			this._switches[switchNo].SetState(UIButtonColor.State.Hover, true);
		}

		public void nextSwitch()
		{
			this._buttonManager.nowForcusIndex = (int)Util.LoopValue(this._buttonManager.nowForcusIndex + 1, 0f, 2f);
			this.setSwitch(this._buttonManager.nowForcusIndex);
		}

		public void SetSelecter(bool show)
		{
			this._selecter.SetActive(show);
		}

		public int getUseDevKitNum()
		{
			switch (this._buttonManager.nowForcusIndex)
			{
			case 0:
				return 1;
			case 1:
				return 20;
			case 2:
				return 100;
			default:
				Debug.LogWarning("開発資材のインデックスが不正です");
				return -1;
			}
		}
	}
}
