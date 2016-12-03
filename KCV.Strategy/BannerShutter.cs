using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class BannerShutter : MonoBehaviour
	{
		private UISprite ShutterL;

		private UISprite ShutterR;

		private void Awake()
		{
			this.ShutterL = base.get_transform().FindChild("BannerShutter_L").GetComponent<UISprite>();
			this.ShutterR = base.get_transform().FindChild("BannerShutter_R").GetComponent<UISprite>();
		}

		public void SetFocusLight(bool isEnable)
		{
			if (this.ShutterL != null)
			{
				UISelectedObject.SelectedOneObjectBlink(this.ShutterL.get_gameObject(), isEnable);
				UISelectedObject.SelectedOneObjectBlink(this.ShutterR.get_gameObject(), isEnable);
			}
		}
	}
}
