using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PresetShip : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Entity_PresetShip.Param> list = new List<Entity_PresetShip.Param>();
	}

	[Serializable]
	public class Param
	{
		public int No;

		public string ShipName;

		public int MstID;

		public int MemID;

		public int SlotItem1;

		public int SlotItem2;

		public int SlotItem3;

		public int SlotItem4;
	}

	public List<Entity_PresetShip.Sheet> sheets = new List<Entity_PresetShip.Sheet>();
}
