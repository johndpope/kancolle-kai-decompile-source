using System;
using System.Collections.Generic;
using UnityEngine;

public class RecordShipLocation : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int MstID;

		public int Rec_X;

		public int Rec_Y;

		public int Rec_dam_X;

		public int Rec_dam_Y;

		public int Rec_X2;

		public int Rec_Y2;

		public int Rec_dam_X2;

		public int Rec_dam_Y2;
	}

	public List<RecordShipLocation.Param> param = new List<RecordShipLocation.Param>();
}
