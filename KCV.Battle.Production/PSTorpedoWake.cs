using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class PSTorpedoWake : MonoBehaviour
	{
		private int _attakerIndex;

		private float _moveTime;

		private bool _isDetonation;

		private bool _isMiss;

		private ParticleSystem _psTorpedoWake;

		private Vector3 _vecTarget;

		private Action _actCallback;

		[SerializeField]
		private ParticleSystem splashT;

		public bool GetMiss()
		{
			return this._isMiss;
		}

		public int GetAttakerIndex()
		{
			return this._attakerIndex;
		}

		private bool _init()
		{
			this._attakerIndex = 0;
			this._moveTime = 0f;
			this._isDetonation = false;
			this._isMiss = false;
			this._actCallback = null;
			this._psTorpedoWake = base.get_gameObject().SafeGetComponent<ParticleSystem>();
			return true;
		}

		private void OnDestroy()
		{
			if (this.splashT != null)
			{
				this.splashT.Stop();
				this.splashT.Clear();
				Object.Destroy(this.splashT.get_gameObject());
			}
			this._psTorpedoWake.Stop();
			this._psTorpedoWake.Clear();
			this._psTorpedoWake.SetActive(false);
			Object.Destroy(this._psTorpedoWake.get_gameObject());
			Mem.Del<Vector3>(ref this._vecTarget);
			Mem.Del(ref this._psTorpedoWake);
			Mem.Del(ref this.splashT);
			Mem.Del<Action>(ref this._actCallback);
		}

		public void Injection(iTween.EaseType eType, bool isPlaySE, bool isTC, Action callback)
		{
			this._actCallback = callback;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", this._vecTarget);
			hashtable.Add("isLocal", false);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("time", this._moveTime);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			base.get_gameObject().MoveTo(hashtable);
			base.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 0f, 0f)));
			this._psTorpedoWake.Play();
			if (isPlaySE)
			{
				if (isTC)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_905);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_904);
				}
			}
		}

		public void Reset()
		{
			base.get_transform().iTweenStop();
			this._psTorpedoWake.Stop();
		}

		public void ReStart(float time, iTween.EaseType eType)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", this._vecTarget);
			hashtable.Add("isLocal", false);
			hashtable.Add("time", time);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			base.get_gameObject().MoveTo(hashtable);
			base.get_transform().set_rotation(Quaternion.Euler(Vector3.get_zero()));
			this._psTorpedoWake.Play();
		}

		public void ReStart(Vector3 fromPos, Vector3 toPos, float time, iTween.EaseType eType, Action callback)
		{
			this._actCallback = callback;
			base.get_transform().set_position(fromPos);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", toPos);
			hashtable.Add("isLocal", false);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("time", time);
			hashtable.Add("easeType", eType);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			base.get_gameObject().MoveTo(hashtable);
			base.get_transform().set_rotation(Quaternion.Euler(Vector3.get_zero()));
			this._psTorpedoWake.Play();
		}

		public void PlaySplash()
		{
			this.splashT = ((!(BattleTaskManager.GetParticleFile().splash == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
			this.splashT.SetActive(true);
			this.splashT.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
			this.splashT.get_transform().set_position(this._vecTarget);
			this.splashT.Play();
		}

		public void PlaySplash(Vector3 fromPos)
		{
			this.splashT = ((!(BattleTaskManager.GetParticleFile().splash == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
			this.splashT.SetActive(true);
			this.splashT.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
			this.splashT.get_transform().set_position(fromPos);
			this.splashT.Play();
		}

		private void _onFinishedInjection()
		{
			if (this._isDetonation)
			{
				this.splashT = ((!(BattleTaskManager.GetParticleFile().splash == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splash) : BattleTaskManager.GetParticleFile().splash);
				this.splashT.SetActive(true);
				this.splashT.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
				this.splashT.get_transform().set_position(this._vecTarget);
				this.splashT.Play();
			}
			this._psTorpedoWake.Stop();
			this._psTorpedoWake.set_time(0f);
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}

		[DebuggerHidden]
		private IEnumerator _delayDiscard(float delay)
		{
			PSTorpedoWake.<_delayDiscard>c__IteratorD1 <_delayDiscard>c__IteratorD = new PSTorpedoWake.<_delayDiscard>c__IteratorD1();
			<_delayDiscard>c__IteratorD.delay = delay;
			<_delayDiscard>c__IteratorD.<$>delay = delay;
			<_delayDiscard>c__IteratorD.<>f__this = this;
			return <_delayDiscard>c__IteratorD;
		}

		public static PSTorpedoWake Instantiate(PSTorpedoWake prefab, Transform parent, Vector3 injectionVec, Vector3 target, int attacker, float _time, bool isDet, bool isMiss)
		{
			PSTorpedoWake pSTorpedoWake = Object.Instantiate<PSTorpedoWake>(prefab);
			pSTorpedoWake._init();
			pSTorpedoWake._psTorpedoWake.get_transform().set_parent(parent);
			pSTorpedoWake.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
			pSTorpedoWake.get_transform().set_position(injectionVec);
			pSTorpedoWake.get_transform().get_transform().set_rotation(Quaternion.Euler(new Vector3(-180f, 0f, 0f)));
			pSTorpedoWake._vecTarget = target;
			pSTorpedoWake._attakerIndex = attacker;
			pSTorpedoWake._moveTime = _time;
			pSTorpedoWake._psTorpedoWake.Stop();
			pSTorpedoWake._isDetonation = isDet;
			pSTorpedoWake._isMiss = isMiss;
			return pSTorpedoWake;
		}
	}
}
