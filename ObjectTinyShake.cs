using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

public class ObjectTinyShake : MonoBehaviour
{
	public float _shakePower = 20f;

	[Tooltip("シェイクのパワーパラメータ")]
	public AnimationCurve _shakeCurve;

	[Tooltip("デバッグ用にループ起動するか")]
	public bool _startLoop;

	private bool _isAnimating;

	private void Start()
	{
		if (this._startLoop)
		{
			this.PlayAnimation().DelayFrame(10, FrameCountType.Update).Repeat<int>().Subscribe(delegate(int _)
			{
			});
		}
	}

	private void OnDestroy()
	{
		Mem.Del<float>(ref this._shakePower);
		Mem.Del<AnimationCurve>(ref this._shakeCurve);
		Mem.Del<bool>(ref this._startLoop);
		Mem.Del<bool>(ref this._isAnimating);
	}

	public IObservable<int> PlayAnimation()
	{
		return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
	}

	[DebuggerHidden]
	private IEnumerator AnimationCoroutine(IObserver<int> observer)
	{
		ObjectTinyShake.<AnimationCoroutine>c__IteratorE1 <AnimationCoroutine>c__IteratorE = new ObjectTinyShake.<AnimationCoroutine>c__IteratorE1();
		<AnimationCoroutine>c__IteratorE.observer = observer;
		<AnimationCoroutine>c__IteratorE.<$>observer = observer;
		<AnimationCoroutine>c__IteratorE.<>f__this = this;
		return <AnimationCoroutine>c__IteratorE;
	}
}
