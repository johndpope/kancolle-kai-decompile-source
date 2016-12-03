using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class SearchLightController : MonoBehaviour
	{
		private bool _isAnimating;

		private Animation _animation;

		private void OnDestroy()
		{
			Mem.Del<bool>(ref this._isAnimating);
			Mem.Del<Animation>(ref this._animation);
		}

		private void Start()
		{
			this._animation = base.GetComponent<Animation>();
			for (int i = 0; i < base.get_transform().get_childCount(); i++)
			{
				base.get_transform().GetChild(i).get_gameObject().SetActive(false);
			}
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			SearchLightController.<AnimationCoroutine>c__IteratorE6 <AnimationCoroutine>c__IteratorE = new SearchLightController.<AnimationCoroutine>c__IteratorE6();
			<AnimationCoroutine>c__IteratorE.observer = observer;
			<AnimationCoroutine>c__IteratorE.<$>observer = observer;
			<AnimationCoroutine>c__IteratorE.<>f__this = this;
			return <AnimationCoroutine>c__IteratorE;
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}
	}
}
