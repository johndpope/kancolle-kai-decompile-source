using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UITitleBackground : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiHexBG;

		[SerializeField]
		private UITexture _uiCloud;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiHexBG);
			Mem.Del<UITexture>(ref this._uiCloud);
		}

		[DebuggerHidden]
		public IEnumerator StartBackgroundAnim()
		{
			UITitleBackground.<StartBackgroundAnim>c__Iterator1A4 <StartBackgroundAnim>c__Iterator1A = new UITitleBackground.<StartBackgroundAnim>c__Iterator1A4();
			<StartBackgroundAnim>c__Iterator1A.<>f__this = this;
			return <StartBackgroundAnim>c__Iterator1A;
		}
	}
}
