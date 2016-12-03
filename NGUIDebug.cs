using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	private static bool mRayDebug = false;

	private static List<string> mLines = new List<string>();

	private static NGUIDebug mInstance = null;

	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			if (Application.get_isPlaying())
			{
				NGUIDebug.mRayDebug = value;
				if (value)
				{
					NGUIDebug.CreateInstance();
				}
			}
		}
	}

	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	private static void LogString(string text)
	{
		if (Application.get_isPlaying())
		{
			if (NGUIDebug.mLines.get_Count() > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
		}
		else
		{
			Debug.Log(text);
		}
	}

	public static void Log(params object[] objs)
	{
		string text = string.Empty;
		for (int i = 0; i < objs.Length; i++)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text = text + ", " + objs[i].ToString();
			}
		}
		NGUIDebug.LogString(text);
	}

	public static void Clear()
	{
		NGUIDebug.mLines.Clear();
	}

	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.get_center();
		Vector3 vector = b.get_center() - b.get_extents();
		Vector3 vector2 = b.get_center() + b.get_extents();
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.get_red());
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.get_red());
		Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.get_red());
		Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.get_red());
	}

	private void OnGUI()
	{
		if (NGUIDebug.mLines.get_Count() == 0)
		{
			if (NGUIDebug.mRayDebug && UICamera.hoveredObject != null && Application.get_isPlaying())
			{
				GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty), new GUILayoutOption[0]);
			}
		}
		else
		{
			int i = 0;
			int count = NGUIDebug.mLines.get_Count();
			while (i < count)
			{
				GUILayout.Label(NGUIDebug.mLines.get_Item(i), new GUILayoutOption[0]);
				i++;
			}
		}
	}
}
