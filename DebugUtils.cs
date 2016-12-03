using KCV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugUtils : SingletonMonoBehaviour<DebugUtils>
{
	private static bool isDebug = false;

	private static bool isDebugMethodName = true;

	private static List<string> strLines = new List<string>();

	private static UILabel _Log;

	private static int _lineCnt = 0;

	private static float checkTime;

	public static bool IsDebug
	{
		get
		{
			return DebugUtils.isDebug;
		}
	}

	private void OnDestroy()
	{
		if (GameObject.Find("_DebugUtils"))
		{
			Object.Destroy(GameObject.Find("_DebugUtils").get_gameObject());
		}
	}

	public static void Log(object msg)
	{
		if (DebugUtils.isDebug)
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance != null && !SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw)
			{
				Debug.Log(msg);
			}
			else
			{
				DebugUtils.SLog(msg);
			}
		}
	}

	public static void Log(object className, object msg)
	{
		if (DebugUtils.isDebug)
		{
			string name = new StackTrace().GetFrame(1).GetMethod().get_Name();
			Debug.Log(string.Concat(new object[]
			{
				"[",
				className,
				"::",
				name,
				"]",
				msg
			}));
		}
	}

	public static void Log(object msg, Object context)
	{
		if (DebugUtils.isDebug)
		{
			Debug.Log(msg, context);
		}
	}

	public static void MethodLog(object methodName)
	{
		if (DebugUtils.isDebugMethodName)
		{
			Debug.Log(methodName);
		}
	}

	public static void SLog(object text)
	{
		if (DebugUtils.isDebug)
		{
			if (SingletonMonoBehaviour<AppInformation>.instance == null)
			{
				return;
			}
			if (!SingletonMonoBehaviour<AppInformation>.Instance.SlogDraw)
			{
				DebugUtils.Log(text);
			}
			else if (Application.get_isPlaying())
			{
				if (DebugUtils.strLines.get_Count() > 20)
				{
					DebugUtils.strLines.RemoveAt(0);
				}
				if (!GameObject.Find("_DebugUtils"))
				{
					GameObject gameObject = new GameObject("_DebugUtils");
					SingletonMonoBehaviour<DebugUtils>.instance = gameObject.AddComponent<DebugUtils>();
					gameObject.get_transform().set_position(new Vector3(9999f, 9999f, 0f));
					UIRoot uIRoot = gameObject.AddComponent<UIRoot>();
					uIRoot.scalingStyle = UIRoot.Scaling.Constrained;
					uIRoot.fitHeight = (uIRoot.fitWidth = true);
					uIRoot.manualWidth = 960;
					uIRoot.manualHeight = 544;
					uIRoot.SetLayer(Generics.Layers.UI2D.IntLayer());
					UIPanel component = gameObject.AddComponent<UIPanel>();
					component.SetLayer(Generics.Layers.UI2D.IntLayer());
					gameObject.get_transform().AddChild("_DebugUtils Camera");
					Camera component2 = gameObject.get_transform().FindChild("_DebugUtils Camera").GetComponent<Camera>();
					component2.SetLayer(Generics.Layers.UI2D.IntLayer());
					component2.set_clearFlags(3);
					component2.set_depth(100f);
					component2.set_cullingMask(512);
					component2.set_orthographic(true);
					component2.set_nearClipPlane(-0.5f);
					component2.set_farClipPlane(0.5f);
					component2.set_orthographicSize(1f);
					UICamera uICamera = component2.AddComponent<UICamera>();
					gameObject.get_transform().AddChild("_Logs");
					DebugUtils._Log = gameObject.get_transform().FindChild("_Logs").GetComponent<UILabel>();
					DebugUtils._Log.SetLayer(Generics.Layers.UI.IntLayer());
					DebugUtils._Log.get_transform().set_localPosition(new Vector3(-477f, 265f, 0f));
					DebugUtils._Log.fontSize = 16;
					DebugUtils._Log.MakePixelPerfect();
					DebugUtils._Log.trueTypeFont = (Resources.Load("Fonts/A-OTF-ShinGoPro-Regular") as Font);
					DebugUtils._Log.effectStyle = UILabel.Effect.Outline;
					DebugUtils._Log.color = Color.get_white();
					DebugUtils._Log.effectColor = Color.get_black();
					DebugUtils._Log.overflowMethod = UILabel.Overflow.ResizeFreely;
					DebugUtils._Log.alpha = 0.85f;
					DebugUtils._Log.pivot = UIWidget.Pivot.TopLeft;
					Object.DontDestroyOnLoad(gameObject);
				}
				UILabel expr_270 = DebugUtils._Log;
				expr_270.text += string.Format("{0}[{1}]\n", text, Time.get_time());
				DebugUtils._lineCnt++;
				if (DebugUtils._lineCnt > 18)
				{
					int num = DebugUtils._Log.text.IndexOf("\n", 0) + 1;
					DebugUtils._Log.text = DebugUtils._Log.text.Remove(0, num);
				}
			}
			else
			{
				DebugUtils.Log(text);
			}
		}
	}

	public static void ClearSLog()
	{
		if (DebugUtils._Log != null)
		{
			DebugUtils._Log.text = string.Empty;
		}
		DebugUtils._lineCnt = 0;
	}

	public static void Error(string msg)
	{
		if (DebugUtils.isDebug)
		{
			Debug.LogError(msg);
		}
	}

	public static void Warning(string msg)
	{
		if (DebugUtils.isDebug)
		{
			Debug.LogWarning(msg);
		}
	}

	public static void NullReferenceException(string msg)
	{
		if (DebugUtils.isDebug)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void FindChkNull(object obj, string msg)
	{
		if (DebugUtils.isDebug && obj == null)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void FindChkNull(string msg)
	{
		if (DebugUtils.isDebug)
		{
			throw new NullReferenceException(msg);
		}
	}

	public static void ExceptionError(Exception e)
	{
		if (DebugUtils.isDebug)
		{
			Debug.Log(e.ToString());
		}
	}

	public static void dbgPause()
	{
	}

	public static void dbgBreak()
	{
	}

	public static void dbgAssert(bool bi)
	{
		if (!bi)
		{
			DebugUtils.Log("dbgAssert : Faild.\n");
			DebugUtils.dbgBreak();
		}
	}

	public static void dbgAssert(bool bi, string msg)
	{
		if (!bi)
		{
			DebugUtils.Log("dbgAssert : Faild. _msg::" + msg);
			DebugUtils.dbgBreak();
		}
	}

	public static void WaitForSecond(MonoBehaviour mono, float time, Action callback)
	{
		if (DebugUtils.isDebug)
		{
			mono.StartCoroutine(DebugUtils.WaitForSecond(time, callback));
		}
	}

	[DebuggerHidden]
	private static IEnumerator WaitForSecond(float time, Action callback)
	{
		DebugUtils.<WaitForSecond>c__Iterator1A8 <WaitForSecond>c__Iterator1A = new DebugUtils.<WaitForSecond>c__Iterator1A8();
		<WaitForSecond>c__Iterator1A.time = time;
		<WaitForSecond>c__Iterator1A.callback = callback;
		<WaitForSecond>c__Iterator1A.<$>time = time;
		<WaitForSecond>c__Iterator1A.<$>callback = callback;
		return <WaitForSecond>c__Iterator1A;
	}

	public static void WaitForSecond(MonoBehaviour mono, float time, Action<bool> callback = null)
	{
		mono.StartCoroutine(DebugUtils.WaitForSecond(time, callback));
	}

	[DebuggerHidden]
	private static IEnumerator WaitForSecond(float time, Action<bool> callback)
	{
		DebugUtils.<WaitForSecond>c__Iterator1A9 <WaitForSecond>c__Iterator1A = new DebugUtils.<WaitForSecond>c__Iterator1A9();
		<WaitForSecond>c__Iterator1A.time = time;
		<WaitForSecond>c__Iterator1A.callback = callback;
		<WaitForSecond>c__Iterator1A.<$>time = time;
		<WaitForSecond>c__Iterator1A.<$>callback = callback;
		return <WaitForSecond>c__Iterator1A;
	}

	public static void Trace()
	{
		if (DebugUtils.isDebug)
		{
			StackFrame stackFrame = new StackFrame(1, true);
			string text = stackFrame.GetFileName();
			string text2 = stackFrame.GetMethod().ToString();
			int fileLineNumber = stackFrame.GetFileLineNumber();
			text = text.Replace(Application.get_dataPath(), string.Empty);
			Debug.Log(string.Concat(new object[]
			{
				text,
				"\n|Method=",
				text2,
				"|Line=",
				fileLineNumber
			}));
		}
	}

	public static void StartCheckTime()
	{
		DebugUtils.checkTime = 0f;
		DebugUtils.checkTime = Time.get_realtimeSinceStartup();
	}

	public static void CheckTimeLap()
	{
		DebugUtils.Log("check time Lap: " + (Time.get_realtimeSinceStartup() - DebugUtils.checkTime).ToString("0.00000"));
	}

	public static void EndCheckTime()
	{
		DebugUtils.checkTime = Time.get_realtimeSinceStartup() - DebugUtils.checkTime;
		DebugUtils.Log("check time End: " + DebugUtils.checkTime.ToString("0.00000000000"));
	}
}
