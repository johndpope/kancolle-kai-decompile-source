using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV
{
	public class UiStarManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite[] _uiStar;

		public void init(int num)
		{
			this._uiStar = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				this._uiStar[i] = base.get_transform().FindChild("Star" + (i + 1)).GetComponent<UISprite>();
				this._uiStar[i].spriteName = "star";
			}
			this.SetStar(num);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this._uiStar);
		}

		public void SetStar(int num)
		{
			for (int i = 0; i < 5; i++)
			{
				if (i <= num)
				{
					this._uiStar[i].spriteName = "star_on";
				}
				else
				{
					this._uiStar[i].spriteName = "star";
				}
			}
		}
	}
}
