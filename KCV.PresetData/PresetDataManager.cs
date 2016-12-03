using System;
using UnityEngine;

namespace KCV.PresetData
{
	public class PresetDataManager
	{
		private Entity_PresetData PresetData;

		private Entity_PresetDeck PresetDeck;

		private Entity_PresetShip PresetShip;

		public PresetDataManager()
		{
			this.PresetData = Resources.Load<Entity_PresetData>("Data/PresetData");
			this.PresetDeck = Resources.Load<Entity_PresetDeck>("Data/PresetDeck");
			this.PresetShip = Resources.Load<Entity_PresetShip>("Data/PresetShip");
		}

		public Entity_PresetData.Param GetPresetData(int PresetDataNo)
		{
			return this.PresetData.sheets.get_Item(0).list.get_Item(PresetDataNo);
		}

		public Entity_PresetDeck.Param GetPresetDeck(int PresetDeckNo)
		{
			return this.PresetDeck.sheets.get_Item(0).list.get_Item(PresetDeckNo - 1);
		}

		public int GetPresetShipMstID(string PresetShipName)
		{
			return this.PresetShip.sheets.get_Item(0).list.Find((Entity_PresetShip.Param ship) => ship.ShipName == PresetShipName).MstID;
		}

		public Entity_PresetShip.Param GetPresetShipParam(string PresetShipName)
		{
			return this.PresetShip.sheets.get_Item(0).list.Find((Entity_PresetShip.Param ship) => ship.ShipName == PresetShipName);
		}
	}
}
