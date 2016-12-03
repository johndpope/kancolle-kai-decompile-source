using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace UniRx
{
	public static class AsyncOperationExtensions
	{
		public static IObservable<AsyncOperation> AsObservable(this AsyncOperation asyncOperation, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AsyncOperation>((IObserver<AsyncOperation> observer, CancellationToken cancellation) => AsyncOperationExtensions.AsObservableCore<AsyncOperation>(asyncOperation, observer, progress, cancellation));
		}

		public static IObservable<T> AsAsyncOperationObservable<T>(this T asyncOperation, IProgress<float> progress = null) where T : AsyncOperation
		{
			return Observable.FromCoroutine<T>((IObserver<T> observer, CancellationToken cancellation) => AsyncOperationExtensions.AsObservableCore<T>(asyncOperation, observer, progress, cancellation));
		}

		[DebuggerHidden]
		private static IEnumerator AsObservableCore<T>(T asyncOperation, IObserver<T> observer, IProgress<float> reportProgress, CancellationToken cancel) where T : AsyncOperation
		{
			AsyncOperationExtensions.<AsObservableCore>c__Iterator1B<T> <AsObservableCore>c__Iterator1B = new AsyncOperationExtensions.<AsObservableCore>c__Iterator1B<T>();
			<AsObservableCore>c__Iterator1B.asyncOperation = asyncOperation;
			<AsObservableCore>c__Iterator1B.cancel = cancel;
			<AsObservableCore>c__Iterator1B.reportProgress = reportProgress;
			<AsObservableCore>c__Iterator1B.observer = observer;
			<AsObservableCore>c__Iterator1B.<$>asyncOperation = asyncOperation;
			<AsObservableCore>c__Iterator1B.<$>cancel = cancel;
			<AsObservableCore>c__Iterator1B.<$>reportProgress = reportProgress;
			<AsObservableCore>c__Iterator1B.<$>observer = observer;
			return <AsObservableCore>c__Iterator1B;
		}
	}
}
