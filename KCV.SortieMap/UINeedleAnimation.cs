using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class UINeedleAnimation : MonoBehaviour
	{
		private UISprite _uiSprite;

		private IDisposable _disAnimation;

		private UISprite sprite
		{
			get
			{
				return this.GetComponentThis(ref this._uiSprite);
			}
		}

		private void Start()
		{
			this.sprite.alpha = 1f;
			this.Play();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiSprite);
			if (this._disAnimation != null)
			{
				this._disAnimation.Dispose();
			}
			Mem.Del<IDisposable>(ref this._disAnimation);
		}

		public void Play()
		{
			this._disAnimation = Observable.Interval(TimeSpan.FromSeconds(0.10000000149011612)).Subscribe(delegate(long x)
			{
				base.get_transform().LTRotateLocal(Vector3.get_forward() * XorRandom.GetFLim(0.45f, 3.3f), 0.1f);
			});
		}

		public void Stop()
		{
			if (this._disAnimation != null)
			{
				this._disAnimation.Dispose();
			}
		}
	}
}
