using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class AsyncLoadScene
{
	private static AsyncOperation _async;

	public static AsyncOperation Async
	{
		get
		{
			return AsyncLoadScene._async;
		}
	}

	public static float Progress
	{
		get;
		private set;
	}

	public static bool IsLoadComplate
	{
		get
		{
			return AsyncLoadScene._async != null && AsyncLoadScene._async.get_progress() >= 0.9f;
		}
		private set
		{
		}
	}

	public static void Begin(string sceneName, bool allowSceneActivation = false, int priority = 0)
	{
		AsyncLoadScene._async = Application.LoadLevelAsync(sceneName);
		AsyncLoadScene._async.set_priority(priority);
		AsyncLoadScene._async.set_allowSceneActivation(allowSceneActivation);
		if (AsyncLoadScene._async != null)
		{
			Debug.Log("nullじゃない");
		}
		else
		{
			Debug.Log("null");
		}
		if (AsyncLoadScene._async != null && !AsyncLoadScene._async.get_isDone())
		{
			AsyncLoadScene.Progress = AsyncLoadScene._async.get_progress();
		}
	}

	[DebuggerHidden]
	public static IEnumerator AsyncBegin(string sceneName, bool allowSceneActivation = false, int priority = 0)
	{
		AsyncLoadScene.<AsyncBegin>c__Iterator1A5 <AsyncBegin>c__Iterator1A = new AsyncLoadScene.<AsyncBegin>c__Iterator1A5();
		<AsyncBegin>c__Iterator1A.sceneName = sceneName;
		<AsyncBegin>c__Iterator1A.priority = priority;
		<AsyncBegin>c__Iterator1A.allowSceneActivation = allowSceneActivation;
		<AsyncBegin>c__Iterator1A.<$>sceneName = sceneName;
		<AsyncBegin>c__Iterator1A.<$>priority = priority;
		<AsyncBegin>c__Iterator1A.<$>allowSceneActivation = allowSceneActivation;
		return <AsyncBegin>c__Iterator1A;
	}

	public static bool ScemeActivation()
	{
		if ((double)AsyncLoadScene._async.get_progress() >= 0.9)
		{
			AsyncLoadScene._async.set_allowSceneActivation(true);
			AsyncLoadScene.Release();
			return true;
		}
		return false;
	}

	private static void Release()
	{
		AsyncLoadScene._async = null;
		AsyncLoadScene.Progress = 0f;
		AsyncLoadScene.IsLoadComplate = false;
	}

	public static void LoadLevelAsyncScene(MonoBehaviour mono, string sceneName, Action callback)
	{
		mono.StartCoroutine(AsyncLoadScene.LoadLevelAsyncScene(sceneName, callback));
	}

	public static void LoadLevelAsyncScene(MonoBehaviour mono, Generics.Scene scene, Action callback)
	{
		mono.StartCoroutine(AsyncLoadScene.LoadLevelAsyncScene(scene.ToString(), callback));
	}

	[DebuggerHidden]
	private static IEnumerator LoadLevelAsyncScene(string sceneName, Action callback)
	{
		AsyncLoadScene.<LoadLevelAsyncScene>c__Iterator1A6 <LoadLevelAsyncScene>c__Iterator1A = new AsyncLoadScene.<LoadLevelAsyncScene>c__Iterator1A6();
		<LoadLevelAsyncScene>c__Iterator1A.sceneName = sceneName;
		<LoadLevelAsyncScene>c__Iterator1A.callback = callback;
		<LoadLevelAsyncScene>c__Iterator1A.<$>sceneName = sceneName;
		<LoadLevelAsyncScene>c__Iterator1A.<$>callback = callback;
		return <LoadLevelAsyncScene>c__Iterator1A;
	}

	public static void LoadAdditiveAsyncScene(MonoBehaviour mono, string sceneName, Action callback)
	{
		mono.StartCoroutine(AsyncLoadScene.AdditiveAsyncScene(sceneName, callback));
	}

	[DebuggerHidden]
	private static IEnumerator AdditiveAsyncScene(string sceneName, Action callback)
	{
		AsyncLoadScene.<AdditiveAsyncScene>c__Iterator1A7 <AdditiveAsyncScene>c__Iterator1A = new AsyncLoadScene.<AdditiveAsyncScene>c__Iterator1A7();
		<AdditiveAsyncScene>c__Iterator1A.sceneName = sceneName;
		<AdditiveAsyncScene>c__Iterator1A.callback = callback;
		<AdditiveAsyncScene>c__Iterator1A.<$>sceneName = sceneName;
		<AdditiveAsyncScene>c__Iterator1A.<$>callback = callback;
		return <AdditiveAsyncScene>c__Iterator1A;
	}
}
