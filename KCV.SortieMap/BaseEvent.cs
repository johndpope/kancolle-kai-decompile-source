using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.SortieMap
{
	public class BaseEvent : IDisposable
	{
		private bool _isDisposed;

		public BaseEvent()
		{
			this._isDisposed = false;
		}

		public void Dispose()
		{
			this.Dispose(true);
			Mem.Del<bool>(ref this._isDisposed);
		}

		public IObservable<bool> PlayAnimation()
		{
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		[DebuggerHidden]
		protected virtual IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			BaseEvent.<AnimationObserver>c__Iterator119 <AnimationObserver>c__Iterator = new BaseEvent.<AnimationObserver>c__Iterator119();
			<AnimationObserver>c__Iterator.observer = observer;
			<AnimationObserver>c__Iterator.<$>observer = observer;
			return <AnimationObserver>c__Iterator;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (disposing)
			{
			}
			this._isDisposed = true;
		}
	}
}
