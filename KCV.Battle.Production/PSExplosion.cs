using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSExplosion : MonoBehaviour
	{
		public enum ExplosionType
		{
			Large,
			Middle,
			Small
		}

		private ParticleSystem _psExplosion;

		private void Awake()
		{
			this._psExplosion = this.SafeGetComponent<ParticleSystem>();
			this._psExplosion.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._psExplosion);
		}

		public void Explode(PSExplosion.ExplosionType iType)
		{
			this._psExplosion.Play();
			SoundUtils.PlaySE(SEFIleInfos.SE_044);
		}
	}
}
