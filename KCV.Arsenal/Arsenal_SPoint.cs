using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class Arsenal_SPoint : MonoBehaviour
	{
		public UIButtonManager _buttonManager;

		[SerializeField]
		private UISprite _selecter;

		private bool _isStarted;

		public bool SPointStarted()
		{
			return this._isStarted;
		}

		public void Init()
		{
			if (this._buttonManager == null)
			{
				this._buttonManager = base.get_transform().FindChild("Switches").GetComponent<UIButtonManager>();
			}
			if (this._selecter == null)
			{
				this._selecter = base.get_transform().FindChild("FrameSelect").GetComponent<UISprite>();
			}
			this._buttonManager.setFocus(0);
			this._buttonManager.IsFocusButtonAlwaysHover = true;
			this._buttonManager.isLoopIndex = true;
			this._isStarted = true;
		}

		public void NextSwitch()
		{
			this._buttonManager.moveNextButton();
		}

		public void SetSelecter(bool show)
		{
			this._selecter.SetActive(show);
		}

		public int GetUseSpointNum()
		{
			switch (this._buttonManager.nowForcusIndex)
			{
			case 0:
				return 50;
			case 1:
				return 100;
			case 2:
				return 250;
			case 3:
				return 400;
			default:
				Debug.LogWarning("戦略ポイントのインデックスが不正です");
				return -1;
			}
		}

		private void OnDestroy()
		{
			this._buttonManager = null;
			this._selecter = null;
		}
	}
}
