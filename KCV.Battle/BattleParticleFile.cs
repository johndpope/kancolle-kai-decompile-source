using System;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class BattleParticleFile : IDisposable
	{
		[SerializeField]
		private Transform _prefabPSExplosionB2;

		[SerializeField]
		private Transform _prefabPSExplosionB3WhiteSmoke;

		[SerializeField]
		private Transform _prefabPSExplosionAntiGround;

		[SerializeField]
		private Transform _prefabPSDustDepthCharge;

		[SerializeField]
		private Transform _prefabPSSplashMiss;

		[SerializeField]
		private Transform _prefabPSSplash;

		[SerializeField]
		private Transform _prefabPSExplosionAerial;

		private ParticleSystem _psExplosionB2;

		private ParticleSystem _psExplosionB3WhiteSmoke;

		private ParticleSystem _psExplosionAntiGround;

		private ParticleSystem _psDustDepthCharge;

		private ParticleSystem _psSplashMiss;

		private ParticleSystem _psSplash;

		private ParticleSystem _psExplosionAerial;

		public ParticleSystem explosionB2
		{
			get
			{
				if (this._psExplosionB2 == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psExplosionB2, ref this._prefabPSExplosionB2);
				}
				return this._psExplosionB2;
			}
		}

		public ParticleSystem explosionB3WhiteSmoke
		{
			get
			{
				if (this._psExplosionB3WhiteSmoke == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psExplosionB3WhiteSmoke, ref this._prefabPSExplosionB3WhiteSmoke);
				}
				return this._psExplosionB3WhiteSmoke;
			}
		}

		public ParticleSystem explosionAntiGround
		{
			get
			{
				if (this._psExplosionAntiGround == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psExplosionAntiGround, ref this._prefabPSExplosionAntiGround);
				}
				return this._psExplosionAntiGround;
			}
		}

		public ParticleSystem dustDepthCharge
		{
			get
			{
				if (this._psDustDepthCharge == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psDustDepthCharge, ref this._prefabPSDustDepthCharge);
				}
				return this._psDustDepthCharge;
			}
		}

		public ParticleSystem splashMiss
		{
			get
			{
				if (this._psSplashMiss == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psSplashMiss, ref this._prefabPSSplashMiss);
				}
				return this._psSplashMiss;
			}
		}

		public ParticleSystem splash
		{
			get
			{
				if (this._psSplash == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psSplash, ref this._prefabPSSplash);
				}
				return this._psSplash;
			}
		}

		public ParticleSystem explosionAerial
		{
			get
			{
				if (this._psExplosionAerial == null)
				{
					BattleParticleFile.InstantiateParticle(ref this._psExplosionAerial, ref this._prefabPSExplosionAerial);
				}
				return this._psExplosionAerial;
			}
		}

		public void Dispose()
		{
			Mem.Del<Transform>(ref this._prefabPSExplosionB2);
			Mem.Del<Transform>(ref this._prefabPSExplosionB3WhiteSmoke);
			Mem.Del<Transform>(ref this._prefabPSExplosionAntiGround);
			Mem.Del<Transform>(ref this._prefabPSDustDepthCharge);
			Mem.Del<Transform>(ref this._prefabPSSplashMiss);
			Mem.Del<Transform>(ref this._prefabPSSplash);
			Mem.Del<Transform>(ref this._prefabPSExplosionAerial);
			Mem.Del(ref this._psExplosionB2);
			Mem.Del(ref this._psExplosionB3WhiteSmoke);
			Mem.Del(ref this._psExplosionAntiGround);
			Mem.Del(ref this._psDustDepthCharge);
			Mem.Del(ref this._psSplashMiss);
			Mem.Del(ref this._psSplash);
			Mem.Del(ref this._psExplosionAerial);
		}

		public static ParticleSystem InstantiateParticle(ref ParticleSystem system, ref Transform prefab)
		{
			return BattleParticleFile.InstantiateParticle(ref system, ref prefab, BattleTaskManager.GetBattleField().get_transform());
		}

		public static ParticleSystem InstantiateParticle(ref ParticleSystem system, ref Transform prefab, Transform parent)
		{
			system = Util.Instantiate(prefab.get_gameObject(), parent.get_gameObject(), false, false).GetComponent<ParticleSystem>();
			system.SetActive(false);
			prefab = null;
			return system;
		}

		public static void ReleaseParticle(ref ParticleSystem system)
		{
			if (system == null)
			{
				return;
			}
			if (system.get_gameObject() != null)
			{
				Object.Destroy(system.get_gameObject());
			}
			system = null;
		}
	}
}
