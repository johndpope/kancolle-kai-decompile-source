using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UniRx
{
	public static class ObservableWWW
	{
		public static IObservable<string> Get(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<string>((IObserver<string> observer, CancellationToken cancellation) => ObservableWWW.FetchText(new WWW(url, null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<byte[]> GetAndGetBytes(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<byte[]>((IObserver<byte[]> observer, CancellationToken cancellation) => ObservableWWW.FetchBytes(new WWW(url, null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<WWW> GetWWW(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<WWW>((IObserver<WWW> observer, CancellationToken cancellation) => ObservableWWW.Fetch(new WWW(url, null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<string>((IObserver<string> observer, CancellationToken cancellation) => ObservableWWW.FetchText(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<string>((IObserver<string> observer, CancellationToken cancellation) => ObservableWWW.FetchText(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<string>((IObserver<string> observer, CancellationToken cancellation) => ObservableWWW.FetchText(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.get_headers();
			return Observable.FromCoroutine<string>((IObserver<string> observer, CancellationToken cancellation) => ObservableWWW.FetchText(new WWW(url, content.get_data(), ObservableWWW.MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<byte[]>((IObserver<byte[]> observer, CancellationToken cancellation) => ObservableWWW.FetchBytes(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<byte[]>((IObserver<byte[]> observer, CancellationToken cancellation) => ObservableWWW.FetchBytes(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<byte[]>((IObserver<byte[]> observer, CancellationToken cancellation) => ObservableWWW.FetchBytes(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.get_headers();
			return Observable.FromCoroutine<byte[]>((IObserver<byte[]> observer, CancellationToken cancellation) => ObservableWWW.FetchBytes(new WWW(url, content.get_data(), ObservableWWW.MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<WWW>((IObserver<WWW> observer, CancellationToken cancellation) => ObservableWWW.Fetch(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<WWW>((IObserver<WWW> observer, CancellationToken cancellation) => ObservableWWW.Fetch(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<WWW>((IObserver<WWW> observer, CancellationToken cancellation) => ObservableWWW.Fetch(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.get_headers();
			return Observable.FromCoroutine<WWW>((IObserver<WWW> observer, CancellationToken cancellation) => ObservableWWW.Fetch(new WWW(url, content.get_data(), ObservableWWW.MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AssetBundle>((IObserver<AssetBundle> observer, CancellationToken cancellation) => ObservableWWW.FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, version), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, uint crc, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AssetBundle>((IObserver<AssetBundle> observer, CancellationToken cancellation) => ObservableWWW.FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, version, crc), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AssetBundle>((IObserver<AssetBundle> observer, CancellationToken cancellation) => ObservableWWW.FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, hash128), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, uint crc, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine<AssetBundle>((IObserver<AssetBundle> observer, CancellationToken cancellation) => ObservableWWW.FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, hash128, crc), observer, progress, cancellation));
		}

		private static Dictionary<string, string> MergeHash(Dictionary<string, string> wwwFormHeaders, Dictionary<string, string> externalHeaders)
		{
			using (Dictionary<string, string>.Enumerator enumerator = externalHeaders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					wwwFormHeaders.set_Item(current.get_Key(), current.get_Value());
				}
			}
			return wwwFormHeaders;
		}

		[DebuggerHidden]
		private static IEnumerator Fetch(WWW www, IObserver<WWW> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			ObservableWWW.<Fetch>c__Iterator1D <Fetch>c__Iterator1D = new ObservableWWW.<Fetch>c__Iterator1D();
			<Fetch>c__Iterator1D.www = www;
			<Fetch>c__Iterator1D.cancel = cancel;
			<Fetch>c__Iterator1D.reportProgress = reportProgress;
			<Fetch>c__Iterator1D.observer = observer;
			<Fetch>c__Iterator1D.<$>www = www;
			<Fetch>c__Iterator1D.<$>cancel = cancel;
			<Fetch>c__Iterator1D.<$>reportProgress = reportProgress;
			<Fetch>c__Iterator1D.<$>observer = observer;
			return <Fetch>c__Iterator1D;
		}

		[DebuggerHidden]
		private static IEnumerator FetchText(WWW www, IObserver<string> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			ObservableWWW.<FetchText>c__Iterator1E <FetchText>c__Iterator1E = new ObservableWWW.<FetchText>c__Iterator1E();
			<FetchText>c__Iterator1E.www = www;
			<FetchText>c__Iterator1E.cancel = cancel;
			<FetchText>c__Iterator1E.reportProgress = reportProgress;
			<FetchText>c__Iterator1E.observer = observer;
			<FetchText>c__Iterator1E.<$>www = www;
			<FetchText>c__Iterator1E.<$>cancel = cancel;
			<FetchText>c__Iterator1E.<$>reportProgress = reportProgress;
			<FetchText>c__Iterator1E.<$>observer = observer;
			return <FetchText>c__Iterator1E;
		}

		[DebuggerHidden]
		private static IEnumerator FetchBytes(WWW www, IObserver<byte[]> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			ObservableWWW.<FetchBytes>c__Iterator1F <FetchBytes>c__Iterator1F = new ObservableWWW.<FetchBytes>c__Iterator1F();
			<FetchBytes>c__Iterator1F.www = www;
			<FetchBytes>c__Iterator1F.cancel = cancel;
			<FetchBytes>c__Iterator1F.reportProgress = reportProgress;
			<FetchBytes>c__Iterator1F.observer = observer;
			<FetchBytes>c__Iterator1F.<$>www = www;
			<FetchBytes>c__Iterator1F.<$>cancel = cancel;
			<FetchBytes>c__Iterator1F.<$>reportProgress = reportProgress;
			<FetchBytes>c__Iterator1F.<$>observer = observer;
			return <FetchBytes>c__Iterator1F;
		}

		[DebuggerHidden]
		private static IEnumerator FetchAssetBundle(WWW www, IObserver<AssetBundle> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			ObservableWWW.<FetchAssetBundle>c__Iterator20 <FetchAssetBundle>c__Iterator = new ObservableWWW.<FetchAssetBundle>c__Iterator20();
			<FetchAssetBundle>c__Iterator.www = www;
			<FetchAssetBundle>c__Iterator.cancel = cancel;
			<FetchAssetBundle>c__Iterator.reportProgress = reportProgress;
			<FetchAssetBundle>c__Iterator.observer = observer;
			<FetchAssetBundle>c__Iterator.<$>www = www;
			<FetchAssetBundle>c__Iterator.<$>cancel = cancel;
			<FetchAssetBundle>c__Iterator.<$>reportProgress = reportProgress;
			<FetchAssetBundle>c__Iterator.<$>observer = observer;
			return <FetchAssetBundle>c__Iterator;
		}
	}
}
