using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class MonobehaviourLogExtensions
{
	public static void Log(this MonoBehaviour self, string msg)
	{
		string name = new StackTrace().GetFrame(1).GetMethod().get_Name();
		string text = string.Format("{2} - {0} {1}", self, name, msg);
		Debug.Log(text, self);
	}

	public static void Log(this MonoBehaviour self, object msg)
	{
		string name = new StackTrace().GetFrame(1).GetMethod().get_Name();
		string text = string.Format("{2} - {0} {1}", self, name, msg.ToString());
		Debug.Log(text, self);
	}

	public static Coroutine DelayAction(this MonoBehaviour self, float delayTime, Action callback)
	{
		return self.StartCoroutine(MonobehaviourLogExtensions._delayAction(delayTime, callback));
	}

	[DebuggerHidden]
	private static IEnumerator _delayAction(float delayTime, Action callback)
	{
		MonobehaviourLogExtensions.<_delayAction>c__Iterator1AA <_delayAction>c__Iterator1AA = new MonobehaviourLogExtensions.<_delayAction>c__Iterator1AA();
		<_delayAction>c__Iterator1AA.delayTime = delayTime;
		<_delayAction>c__Iterator1AA.callback = callback;
		<_delayAction>c__Iterator1AA.<$>delayTime = delayTime;
		<_delayAction>c__Iterator1AA.<$>callback = callback;
		return <_delayAction>c__Iterator1AA;
	}

	public static void DelayActionFrame(this MonoBehaviour self, int delayFrame, Action callback)
	{
		self.StartCoroutine(MonobehaviourLogExtensions._delayActionFrame(delayFrame, callback));
	}

	[DebuggerHidden]
	private static IEnumerator _delayActionFrame(int delayFrame, Action callback)
	{
		MonobehaviourLogExtensions.<_delayActionFrame>c__Iterator1AB <_delayActionFrame>c__Iterator1AB = new MonobehaviourLogExtensions.<_delayActionFrame>c__Iterator1AB();
		<_delayActionFrame>c__Iterator1AB.delayFrame = delayFrame;
		<_delayActionFrame>c__Iterator1AB.callback = callback;
		<_delayActionFrame>c__Iterator1AB.<$>delayFrame = delayFrame;
		<_delayActionFrame>c__Iterator1AB.<$>callback = callback;
		return <_delayActionFrame>c__Iterator1AB;
	}

	public static void DelayActionCoroutine(this MonoBehaviour self, Coroutine cor, Action callback)
	{
		self.StartCoroutine(MonobehaviourLogExtensions._delayActionCoroutine(cor, callback));
	}

	[DebuggerHidden]
	private static IEnumerator _delayActionCoroutine(Coroutine cor, Action callback)
	{
		MonobehaviourLogExtensions.<_delayActionCoroutine>c__Iterator1AC <_delayActionCoroutine>c__Iterator1AC = new MonobehaviourLogExtensions.<_delayActionCoroutine>c__Iterator1AC();
		<_delayActionCoroutine>c__Iterator1AC.cor = cor;
		<_delayActionCoroutine>c__Iterator1AC.callback = callback;
		<_delayActionCoroutine>c__Iterator1AC.<$>cor = cor;
		<_delayActionCoroutine>c__Iterator1AC.<$>callback = callback;
		return <_delayActionCoroutine>c__Iterator1AC;
	}

	public static T GetComponentThis<T>(this MonoBehaviour self, ref T instance)
	{
		if (instance != null)
		{
			return instance;
		}
		return instance = self.GetComponent<T>();
	}
}
