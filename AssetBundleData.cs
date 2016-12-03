using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AssetBundleData
{
	public static T Load<T>(string path, int version) where T : class
	{
		T result = (T)((object)null);
		using (WWW wWW = WWW.LoadFromCacheOrDownload(path, version))
		{
			if (wWW.get_error() != null)
			{
				return (T)((object)null);
			}
			AssetBundle assetBundle = wWW.get_assetBundle();
			result = (assetBundle.get_mainAsset() as T);
			assetBundle.Unload(false);
		}
		return result;
	}

	public static T LoadStreamingAssets<T>(string path, int version) where T : class
	{
		return AssetBundleData.Load<T>("file://" + string.Format("{0}/StreamingAssets/{1}", Application.get_dataPath(), path), version);
	}

	[DebuggerHidden]
	public static IEnumerator LoadAsync<T>(string path, int version, Action<T> onComplate) where T : class
	{
		AssetBundleData.<LoadAsync>c__Iterator55<T> <LoadAsync>c__Iterator = new AssetBundleData.<LoadAsync>c__Iterator55<T>();
		<LoadAsync>c__Iterator.path = path;
		<LoadAsync>c__Iterator.version = version;
		<LoadAsync>c__Iterator.onComplate = onComplate;
		<LoadAsync>c__Iterator.<$>path = path;
		<LoadAsync>c__Iterator.<$>version = version;
		<LoadAsync>c__Iterator.<$>onComplate = onComplate;
		return <LoadAsync>c__Iterator;
	}

	public static IEnumerator LoadAsyncStreamingAssets<T>(string path, int version, Action<T> onComplate) where T : class
	{
		return AssetBundleData.LoadAsync<T>("file://" + string.Format("{0}/StreamingAssets/{1}", Application.get_dataPath(), path), version, onComplate);
	}
}
