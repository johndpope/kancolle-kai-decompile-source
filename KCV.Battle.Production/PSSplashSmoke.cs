using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSSplashSmoke : MonoBehaviour
	{
		public Vector3 randMinPos;

		public Vector3 randMaxPos;

		public float moveTime;

		public int count;

		public bool isPlayAwake;

		private Bezier[] _clsBezier;

		private float _timer;

		private List<ParticleSystem> _listPS;

		private List<Vector3> _listPos;

		private ParticleSystem _par;

		private bool isPlay;

		private Action _actCallback;

		private void Awake()
		{
			this._actCallback = null;
			if (this.isPlayAwake)
			{
				this.setSmoke();
				this.play();
			}
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
			this._actCallback = null;
			if (this._listPS != null)
			{
				using (List<ParticleSystem>.Enumerator enumerator = this._listPS.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ParticleSystem current = enumerator.get_Current();
						if (current.get_gameObject() != null)
						{
							current.Stop();
							Object.Destroy(current.get_gameObject());
						}
					}
				}
				this._listPS.Clear();
			}
			this._listPS = null;
		}

		private void Update()
		{
			if (this.isPlay && this._timer < this.moveTime)
			{
				this._timer += Time.get_deltaTime();
				float num = this._timer * 2f;
				for (int i = 0; i < this.count; i++)
				{
					this._listPS.get_Item(i).get_transform().set_position(this._clsBezier[i].Interpolate(this._timer));
				}
				if (this._timer > this.moveTime)
				{
					this.SetDestroy();
				}
			}
		}

		public void SetDestroy()
		{
			for (int i = 0; i < this._listPS.get_Count(); i++)
			{
				this._listPS.get_Item(i).Stop();
			}
		}

		public void setSmoke()
		{
			if (this.count <= 5)
			{
				this.count = 5;
			}
			this._listPS = new List<ParticleSystem>();
			this._listPos = new List<Vector3>();
			for (int i = 0; i < this.count; i++)
			{
				this._listPS.Add(this.Instantiate());
			}
			Vector3 position = base.get_transform().get_position();
			float fMin = this.randMinPos.x + (this.randMaxPos.x - this.randMinPos.x) * 0.8f;
			float fMin2 = this.randMinPos.x + (this.randMaxPos.x - this.randMinPos.x) * 0.5f;
			float fMin3 = this.randMinPos.y + (this.randMaxPos.y - this.randMinPos.y) * 0.8f;
			float fMin4 = this.randMinPos.y + (this.randMaxPos.y - this.randMinPos.y) * 0.5f;
			this._listPos.Add(new Vector3(position.x, position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z + this.randMaxPos.z));
			this._listPos.Add(new Vector3(position.x + XorRandom.GetFLim(fMin, this.randMaxPos.x), position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z + XorRandom.GetFLim(fMin4, this.randMaxPos.y)));
			this._listPos.Add(new Vector3(position.x - XorRandom.GetFLim(fMin, this.randMaxPos.x), position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z + XorRandom.GetFLim(fMin4, this.randMaxPos.y)));
			this._listPos.Add(new Vector3(position.x + XorRandom.GetFLim(fMin2, this.randMaxPos.x), position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z - XorRandom.GetFLim(fMin3, this.randMaxPos.y)));
			this._listPos.Add(new Vector3(position.x - XorRandom.GetFLim(fMin2, this.randMaxPos.x), position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z - XorRandom.GetFLim(fMin3, this.randMaxPos.y)));
			float[] array = new float[2];
			float[] array2 = new float[2];
			for (int j = 0; j < this.count - 5; j++)
			{
				array[0] = XorRandom.GetFLim(this.randMinPos.x, this.randMaxPos.x);
				array[1] = XorRandom.GetFLim(this.randMaxPos.x * -1f, this.randMinPos.x * -1f);
				array2[0] = XorRandom.GetFLim(this.randMinPos.z, this.randMaxPos.z);
				array2[1] = XorRandom.GetFLim(this.randMaxPos.z * -1f, this.randMinPos.z * -1f);
				this._listPos.Add(new Vector3(position.x + array[XorRandom.GetILim(0, 1)], position.y + XorRandom.GetFLim(this.randMinPos.y, this.randMaxPos.y), position.z + array2[XorRandom.GetILim(0, 1)]));
			}
		}

		public void play()
		{
			this._timer = 0f;
			this._clsBezier = new Bezier[this.count];
			for (int i = 0; i < this.count; i++)
			{
				Vector3 vector = Vector3.Lerp(base.get_transform().get_position(), this._listPos.get_Item(i), 0.3f);
				Vector3 vector2 = Vector3.Lerp(base.get_transform().get_position(), this._listPos.get_Item(i), 0.5f);
				Vector3 end = new Vector3(this._listPos.get_Item(i).x, base.get_transform().get_position().y - 5f, this._listPos.get_Item(i).z);
				this._clsBezier[i] = new Bezier(Bezier.BezierType.Cubic, base.get_transform().get_position(), end, new Vector3(vector.x, this._listPos.get_Item(i).y, vector.z), new Vector3(vector2.x, this._listPos.get_Item(i).y - 2f, vector2.z));
			}
			this.isPlay = true;
		}

		private void _onFinishedInjection()
		{
			base.StartCoroutine(this._delayDiscard(0.1f));
		}

		[DebuggerHidden]
		private IEnumerator _delayDiscard(float delay)
		{
			PSSplashSmoke.<_delayDiscard>c__IteratorD0 <_delayDiscard>c__IteratorD = new PSSplashSmoke.<_delayDiscard>c__IteratorD0();
			<_delayDiscard>c__IteratorD.delay = delay;
			<_delayDiscard>c__IteratorD.<$>delay = delay;
			<_delayDiscard>c__IteratorD.<>f__this = this;
			return <_delayDiscard>c__IteratorD;
		}

		private ParticleSystem Instantiate()
		{
			if (this._par == null)
			{
				this._par = ParticleFile.Load<ParticleSystem>(ParticleFileInfos.BattlePSSplashSmoke);
			}
			ParticleSystem particleSystem = Object.Instantiate<ParticleSystem>(this._par);
			particleSystem.get_transform().set_parent(base.get_transform());
			particleSystem.get_transform().set_position(base.get_transform().get_position());
			return particleSystem;
		}
	}
}
