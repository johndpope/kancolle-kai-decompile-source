using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PracticeVoice : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Entity_PracticeVoice.Param> list = new List<Entity_PracticeVoice.Param>();
	}

	[Serializable]
	public class Param
	{
		public int MstID;

		public string Name;

		public int VoiceNo;
	}

	public List<Entity_PracticeVoice.Sheet> sheets = new List<Entity_PracticeVoice.Sheet>();
}
