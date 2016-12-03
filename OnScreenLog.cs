using System;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenLog : MonoBehaviour
{
	private static int msgCount = 0;

	private static List<string> log = new List<string>();

	private static int maxLines = 16;

	private static int fontSize = 24;

	private int frameCount;

	private void Start()
	{
		if (Application.get_platform() == 25)
		{
			OnScreenLog.maxLines = 38;
		}
	}

	private void Update()
	{
		this.frameCount++;
	}

	private void OnGUI()
	{
		GUIStyle style = GUI.get_skin().GetStyle("Label");
		style.set_fontSize(OnScreenLog.fontSize);
		style.set_alignment(0);
		style.set_wordWrap(false);
		float num = 0f;
		string text = string.Empty;
		using (List<string>.Enumerator enumerator = OnScreenLog.log.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.get_Current();
				text = text + " " + current;
				text += "\n";
				num += style.get_lineHeight();
			}
		}
		num += 6f;
		GUI.Label(new Rect(0f, 0f, (float)(Screen.get_width() - 1), num), text, style);
		num = style.get_lineHeight() + 4f;
		GUI.Label(new Rect((float)(Screen.get_width() - 100), (float)(Screen.get_height() - 100), (float)(Screen.get_width() - 1), num), this.frameCount.ToString());
	}

	public static void Add(string msg)
	{
		string text = msg.Replace("\r", " ");
		text = text.Replace("\n", " ");
		Console.WriteLine("[APP] " + text);
		OnScreenLog.log.Add(text);
		OnScreenLog.msgCount++;
		if (OnScreenLog.msgCount > OnScreenLog.maxLines)
		{
			OnScreenLog.log.RemoveAt(0);
		}
	}
}
