using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	public class ProdShipRipple : MonoBehaviour
	{
		[SerializeField]
		private UISprite Hammon1;

		[SerializeField]
		private UISprite Hammon2;

		private Vector3 MaxHammonScale;

		private List<Action> setEndAnimation;

		public float duration;

		public float SecondHammonDelay;

		private void Awake()
		{
			this.setEndAnimation = new List<Action>();
			this.MaxHammonScale = new Vector3(7f, 7f, 1f);
		}

		public void Play(Color color)
		{
			this.Hammon1.color = color;
			this.Hammon2.color = color;
			this.StartHamonnEffect(this.Hammon1.get_gameObject(), 0f);
			this.StartHamonnEffect(this.Hammon2.get_gameObject(), this.SecondHammonDelay);
		}

		private void StartHamonnEffect(GameObject go, float delay)
		{
			TweenScale ts = TweenScale.Begin(go, this.duration, this.MaxHammonScale);
			TweenAlpha ta = TweenAlpha.Begin(go, this.duration, 0f);
			ts.style = UITweener.Style.Loop;
			ta.style = UITweener.Style.Loop;
			ts.delay = delay;
			ta.delay = delay;
			this.setEndAnimation.Add(delegate
			{
				ts.style = UITweener.Style.Once;
				ta.style = UITweener.Style.Once;
			});
		}

		public void Stop()
		{
			using (List<Action>.Enumerator enumerator = this.setEndAnimation.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Action current = enumerator.get_Current();
					current.Invoke();
				}
			}
		}
	}
}
