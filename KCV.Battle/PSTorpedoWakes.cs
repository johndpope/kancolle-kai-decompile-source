using KCV.Battle.Production;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class PSTorpedoWakes : IDisposable
	{
		private PSTorpedoWake _torpedoWake;

		private PSTorpedoWake _torpedoWakeD;

		private List<PSTorpedoWake> _listTorpedoWake;

		private Vector3[] _vecProtectTorpedo;

		public PSTorpedoWakes()
		{
			this.Init();
		}

		public bool Init()
		{
			this._torpedoWake = null;
			this._torpedoWakeD = null;
			this._vecProtectTorpedo = new Vector3[6];
			this._listTorpedoWake = new List<PSTorpedoWake>();
			return true;
		}

		public void SetDestroy()
		{
			this.Dispose();
			Mem.Del<PSTorpedoWake>(ref this._torpedoWake);
			Mem.Del<PSTorpedoWake>(ref this._torpedoWakeD);
			Mem.DelListSafe<PSTorpedoWake>(ref this._listTorpedoWake);
		}

		public void Dispose()
		{
			if (this._listTorpedoWake != null)
			{
				using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSTorpedoWake current = enumerator.get_Current();
						if (current != null)
						{
							Object.Destroy(current);
						}
					}
				}
				this._listTorpedoWake.Clear();
			}
			this._listTorpedoWake = null;
		}

		public void AddInstantiates(Transform parent, Vector3 injectionVec, Vector3 targetVec, bool isFull, int attackerIndex, float time, bool isDet, bool isMiss)
		{
			if (this._torpedoWake == null)
			{
				this._torpedoWake = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			}
			if (this._torpedoWakeD == null)
			{
				this._torpedoWakeD = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD);
			}
			this._listTorpedoWake.Add(PSTorpedoWake.Instantiate((!isFull) ? this._torpedoWake : this._torpedoWakeD, parent, injectionVec, targetVec, attackerIndex, time, isDet, isMiss));
		}

		public void InitProtectVector()
		{
			if (this._vecProtectTorpedo == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				this._vecProtectTorpedo[i] = Vector3.get_zero();
			}
		}

		public void SetProtectVector(int index, Vector3 pos)
		{
			if (this._vecProtectTorpedo == null)
			{
				return;
			}
			if (index < 0 || index >= 6)
			{
				return;
			}
			this._vecProtectTorpedo[index] = pos;
		}

		public void InjectionAll()
		{
			using (List<PSTorpedoWake>.Enumerator enumerator = this._listTorpedoWake.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PSTorpedoWake current = enumerator.get_Current();
					current.Injection(iTween.EaseType.easeInCubic, false, false, delegate
					{
					});
				}
			}
			if (this._listTorpedoWake.get_Count() > 0)
			{
				SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
			}
		}

		public void ReStartAll()
		{
			if (this._listTorpedoWake == null)
			{
				return;
			}
			for (int i = 0; i < this._listTorpedoWake.get_Count(); i++)
			{
				this._listTorpedoWake.get_Item(i).ReStart(0.9f, iTween.EaseType.linear);
			}
		}

		public void PlaySplashAll()
		{
			if (this._listTorpedoWake == null)
			{
				return;
			}
			for (int i = 0; i < this._listTorpedoWake.get_Count(); i++)
			{
				if (!this._listTorpedoWake.get_Item(i).GetMiss())
				{
					if (this._vecProtectTorpedo[i] != Vector3.get_zero())
					{
						this._listTorpedoWake.get_Item(i).PlaySplash(this._vecProtectTorpedo[i]);
					}
					else
					{
						this._listTorpedoWake.get_Item(i).PlaySplash();
					}
				}
			}
		}
	}
}
