using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class UIStartupHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private List<Animation> _listAnimation;

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this._uiLabel);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.DelList<Animation>(ref this._listAnimation);
		}

		public void ClearMessage()
		{
			this._uiLabel.text = string.Empty;
		}

		public void SetMessage(string title)
		{
			this._uiLabel.text = title;
		}
	}
}
