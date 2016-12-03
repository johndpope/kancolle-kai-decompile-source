using System;
using System.Collections.Generic;
using UnityEngine;

public class mst_shipgraphbattle : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int id;

		public int version;

		public int foot_x;

		public int foot_y;

		public int foot_d_x;

		public int foot_d_y;

		public int cutin_x;

		public int cutin_y;

		public int cutin_d_x;

		public int cutin_d_y;

		public int pog_x;

		public int pog_y;

		public int pog_d_x;

		public int pog_d_y;

		public int pog_sp_x;

		public int pog_sp_y;

		public int pog_sp_d_x;

		public int pog_sp_d_y;
	}

	public List<mst_shipgraphbattle.Param> param = new List<mst_shipgraphbattle.Param>();
}
