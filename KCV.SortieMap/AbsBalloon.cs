using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public abstract class AbsBalloon : MonoBehaviour
	{
		protected UISprite _uiSprite;

		public UISprite sprite
		{
			get
			{
				return this.GetComponentThis(ref this._uiSprite);
			}
		}

		protected virtual void OnDestroy()
		{
			Mem.Del(ref this._uiSprite);
		}

		protected abstract void SetBalloonPos(UISortieShip.Direction iDirection);

		public LTDescr ShowHide()
		{
			base.get_transform().LTScale(Vector3.get_one(), 0.3f).setEase(LeanTweenType.easeInExpo);
			return base.get_transform().LTScale(Vector3.get_zero(), 0.3f).setEase(LeanTweenType.easeInExpo).setDelay(1.5f);
		}
	}
}
