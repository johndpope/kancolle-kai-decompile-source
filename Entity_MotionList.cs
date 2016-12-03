using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_MotionList : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Entity_MotionList.Param> list = new List<Entity_MotionList.Param>();
	}

	[Serializable]
	public class Param
	{
		public int MstID;

		public string Name;

		public int Motion1;

		public int Motion2;

		public int Motion3;

		public int Motion4;
	}

	public List<Entity_MotionList.Sheet> sheets = new List<Entity_MotionList.Sheet>();
}
