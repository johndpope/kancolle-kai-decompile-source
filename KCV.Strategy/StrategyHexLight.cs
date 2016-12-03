using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexLight : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private Animation _anim;

		private Action _callback;

		private void OnDestroy()
		{
			this._callback = null;
			if (this._uiLightPar != null)
			{
				this._uiLightPar.Stop();
			}
			this._uiLightPar = null;
			if (this._anim != null)
			{
				this._anim.Stop();
			}
			this._anim = null;
		}

		public void Play(Action callback)
		{
			Util.FindParentToChild<ParticleSystem>(ref this._uiLightPar, base.get_transform(), "Particle");
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			this._callback = null;
			this._anim.Stop();
			this._uiLightPar.SetActive(false);
			this._callback = callback;
			this._anim.Stop();
			this._anim.Play();
			if (this._uiLightPar != null)
			{
				this._uiLightPar.SetActive(true);
				this._uiLightPar.Play();
			}
		}

		private void stopParticle()
		{
			if (this._uiLightPar == null)
			{
				return;
			}
			this._uiLightPar.Stop();
		}

		private void onAnimationFinished()
		{
			if (this._callback != null)
			{
				this._callback.Invoke();
			}
			if (this._uiLightPar != null)
			{
				this._uiLightPar.SetActive(false);
			}
		}

		public static StrategyHexLight Instantiate(StrategyHexLight prefab, Transform fromParent)
		{
			StrategyHexLight strategyHexLight = Object.Instantiate<StrategyHexLight>(prefab);
			strategyHexLight.get_transform().set_parent(fromParent);
			strategyHexLight.get_transform().set_localScale(Vector3.get_one());
			strategyHexLight.get_transform().set_localPosition(Vector3.get_zero());
			return strategyHexLight;
		}
	}
}
