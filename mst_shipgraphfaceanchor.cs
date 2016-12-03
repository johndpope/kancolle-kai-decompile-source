using System;
using System.Collections.Generic;
using UnityEngine;

public class mst_shipgraphfaceanchor : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int Id;

		public int facea9_x;

		public int facea9_y;

		public int faceb9_x;

		public int faceb9_y;

		public double facea10_x;

		public int facea10_y;

		public double faceb10_x;

		public int faceb10_y;
	}

	public List<mst_shipgraphfaceanchor.Param> param = new List<mst_shipgraphfaceanchor.Param>();
}
