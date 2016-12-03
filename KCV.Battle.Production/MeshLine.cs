using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class MeshLine : MonoBehaviour
	{
		private Transform _traTorpedoTarget;

		private Vector3 _vecTorpedoTarget;

		private float _moveTime;

		private float _delayTime;

		private Action _actCallback;

		private void Awake()
		{
			this._actCallback = null;
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
			this._traTorpedoTarget = null;
			this._actCallback = null;
		}

		private void Update()
		{
		}

		public void SetDestroy()
		{
			Vector3 eulerAngles = base.get_transform().get_rotation().get_eulerAngles();
			Vector3 vector = new Vector3(eulerAngles.x, eulerAngles.y + 180f, eulerAngles.z);
			base.get_transform().set_rotation(Quaternion.Euler(vector));
			base.get_transform().set_position(this._vecTorpedoTarget);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("z", 0f);
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.5f);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_onFinishedInjection");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			base.get_gameObject().ScaleTo(hashtable);
		}

		public void Extend(bool isVec, Action callback)
		{
			this._actCallback = callback;
			base.get_transform().LookAt(this._vecTorpedoTarget);
			Vector3 vector = Mathe.Direction(base.get_transform().get_position(), this._vecTorpedoTarget);
			Hashtable hashtable = new Hashtable();
			if (isVec)
			{
				hashtable.Add("z", vector.z);
			}
			else
			{
				hashtable.Add("z", vector.z * -1f);
			}
			hashtable.Add("isLocal", false);
			hashtable.Add("time", this._moveTime);
			hashtable.Add("delay", this._delayTime);
			hashtable.Add("easeType", iTween.EaseType.linear);
			base.get_gameObject().ScaleTo(hashtable);
		}

		private void _onFinishedInjection()
		{
			base.StartCoroutine(this._delayDiscard(0.1f));
		}

		[DebuggerHidden]
		private IEnumerator _delayDiscard(float delay)
		{
			MeshLine.<_delayDiscard>c__IteratorF1 <_delayDiscard>c__IteratorF = new MeshLine.<_delayDiscard>c__IteratorF1();
			<_delayDiscard>c__IteratorF.delay = delay;
			<_delayDiscard>c__IteratorF.<$>delay = delay;
			<_delayDiscard>c__IteratorF.<>f__this = this;
			return <_delayDiscard>c__IteratorF;
		}

		public static MeshLine Instantiate(MeshLine prefab, Transform parent, Vector3 injectionVec, Vector3 target, float _time, float _delay)
		{
			MeshLine meshLine = Object.Instantiate<MeshLine>(prefab);
			meshLine.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
			meshLine.get_transform().set_position(injectionVec);
			Vector3 vecTorpedoTarget = new Vector3(target.x, 1f, target.z);
			meshLine._vecTorpedoTarget = vecTorpedoTarget;
			meshLine._moveTime = _time;
			meshLine._delayTime = _delay;
			return meshLine;
		}
	}
}
