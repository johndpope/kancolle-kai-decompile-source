using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUIMapManager : MonoBehaviour
	{
		[SerializeField]
		private StrategyHexTileManager tileManager;

		[SerializeField]
		private StrategyShipManager shipIconManager;

		public StrategyHexTileManager TileManager
		{
			get
			{
				return this.tileManager;
			}
			private set
			{
				this.tileManager = value;
			}
		}

		public StrategyShipManager ShipIconManager
		{
			get
			{
				return this.shipIconManager;
			}
			private set
			{
				this.shipIconManager = value;
			}
		}
	}
}
