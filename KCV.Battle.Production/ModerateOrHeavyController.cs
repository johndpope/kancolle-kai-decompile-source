using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class ModerateOrHeavyController : MonoBehaviour
	{
		public enum Mode
		{
			Moderate,
			Heavy
		}

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		private ExplodeChild _moderate;

		[SerializeField]
		private Transform _moderateFlat;

		[SerializeField]
		private ExplodeChild _heavy;

		[SerializeField]
		private Transform _heavyFlat;

		[SerializeField, Tooltip("アニメーションクリップ用スケール")]
		private float _scale = 1f;

		[SerializeField]
		private float _defaultScale = 272f;

		[SerializeField, Tooltip("デバッグ用にループ起動するか")]
		private bool _startLoop;

		private bool _isAnimating;

		private ModerateOrHeavyController.Mode _mode;

		private Subject<int> _shakeSubject = new Subject<int>();

		public IObservable<int> ShakeObservable
		{
			get
			{
				return this._shakeSubject;
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isAnimating;
			}
		}

		private void Start()
		{
			DebugUtils.dbgAssert(this._animation != null);
			if (this._startLoop)
			{
				this.PlayAnimation(ModerateOrHeavyController.Mode.Heavy).DelayFrame(10, FrameCountType.Update).Repeat<int>().Subscribe(delegate(int _)
				{
				});
			}
		}

		private void OnDestroy()
		{
			Mem.DelMeshSafe(ref this._heavyFlat);
			Mem.DelMeshSafe(ref this._moderateFlat);
			Mem.Del<Animation>(ref this._animation);
			Mem.Del<ExplodeChild>(ref this._moderate);
			Mem.Del<Transform>(ref this._moderateFlat);
			Mem.Del<ExplodeChild>(ref this._heavy);
			Mem.Del<Transform>(ref this._heavyFlat);
			Mem.Del<float>(ref this._scale);
			Mem.Del<float>(ref this._defaultScale);
			Mem.Del<bool>(ref this._startLoop);
			Mem.Del<bool>(ref this._isAnimating);
			Mem.Del<ModerateOrHeavyController.Mode>(ref this._mode);
			if (this._shakeSubject != null)
			{
				this._shakeSubject.OnCompleted();
			}
			Mem.Del<Subject<int>>(ref this._shakeSubject);
		}

		public bool LateRun()
		{
			if (this._isAnimating)
			{
				base.get_transform().set_localScale(Vector3.get_one() * this._scale);
			}
			if (this._mode == ModerateOrHeavyController.Mode.Moderate)
			{
				this._moderate.LateRun();
			}
			else
			{
				this._heavy.LateRun();
			}
			return true;
		}

		private void ShakeEvent()
		{
			this._shakeSubject.OnNext(0);
		}

		public IObservable<int> PlayAnimation(ModerateOrHeavyController.Mode mode)
		{
			this._mode = mode;
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			ModerateOrHeavyController.<AnimationCoroutine>c__IteratorE0 <AnimationCoroutine>c__IteratorE = new ModerateOrHeavyController.<AnimationCoroutine>c__IteratorE0();
			<AnimationCoroutine>c__IteratorE.observer = observer;
			<AnimationCoroutine>c__IteratorE.<$>observer = observer;
			<AnimationCoroutine>c__IteratorE.<>f__this = this;
			return <AnimationCoroutine>c__IteratorE;
		}
	}
}
