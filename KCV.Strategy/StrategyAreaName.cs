using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyAreaName : MonoBehaviour
	{
		[SerializeField]
		private UISprite AreaNameSprite;

		private TweenAlpha tweenAlpha;

		private void Start()
		{
			this.AreaNameSprite.alpha = 0f;
		}

		public void setAreaName(int areaID)
		{
			this.AreaNameSprite.spriteName = "map_txt" + areaID.ToString("D2");
			this.AreaNameSprite.MakePixelPerfect();
		}

		public void StartAnimation()
		{
			this.AreaNameSprite.get_transform().set_localPosition(Vector3.get_zero());
			this.AreaNameSprite.alpha = 0f;
			if (this.tweenAlpha != null)
			{
				this.tweenAlpha.ResetToBeginning();
				this.tweenAlpha.from = 0f;
			}
			this.tweenAlpha = TweenAlpha.Begin(this.AreaNameSprite.get_gameObject(), 0.2f, 1f);
		}

		private void Animation()
		{
			this.tweenAlpha = TweenAlpha.Begin(this.AreaNameSprite.get_gameObject(), 0.2f, 1f);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", -50);
			hashtable.Add("easetype", iTween.EaseType.easeOutQuint);
			hashtable.Add("islocal", true);
			hashtable.Add("time", 0.2f);
			iTween.MoveFrom(this.AreaNameSprite.get_gameObject(), hashtable);
		}
	}
}
