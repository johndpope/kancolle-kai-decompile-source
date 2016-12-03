using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_PresetData : ScriptableObject
{
	[Serializable]
	public class Sheet
	{
		public string name = string.Empty;

		public List<Entity_PresetData.Param> list = new List<Entity_PresetData.Param>();
	}

	[Serializable]
	public class Param
	{
		public int No;

		public string Name;

		public int[] Deck;

		public int[] Area;

		public int Tanker;

		public int Fuel;

		public int Bull;

		public int Steel;

		public int Baux;

		public int Dev_Kit;

		public int RepairKit;

		public int BuildKit;

		public int Revamp_Kit;

		public int Items;

		public int Spoint;

		public int Coin;

		public int TeitokuLV;

		public int AllShipLevel;

		public int RebellionPhase;

		public bool AddAllShip;
	}

	public List<Entity_PresetData.Sheet> sheets = new List<Entity_PresetData.Sheet>();
}
