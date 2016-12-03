using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PresetDeck : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Entity_PresetDeck.Param> list = new List<Entity_PresetDeck.Param>();
	}

	[Serializable]
	public class Param
	{
		public int No;

		public string Name;

		public string[] PresetShip;
	}

	public List<Entity_PresetDeck.Sheet> sheets = new List<Entity_PresetDeck.Sheet>();
}
