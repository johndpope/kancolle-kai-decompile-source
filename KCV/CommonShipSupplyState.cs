using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class CommonShipSupplyState : MonoBehaviour
	{
		public enum SupplyState
		{
			None,
			Green,
			Yellow,
			Red
		}

		[SerializeField]
		private UISprite FuelState;

		[SerializeField]
		private UISprite AmmoState;

		private CommonShipSupplyState.SupplyState _iFuelState;

		private CommonShipSupplyState.SupplyState _iAmmoState;

		public CommonShipSupplyState.SupplyState fuelState
		{
			get
			{
				return this._iFuelState;
			}
		}

		public CommonShipSupplyState.SupplyState ammoState
		{
			get
			{
				return this._iAmmoState;
			}
		}

		public bool isEitherSupplyNeeds
		{
			get
			{
				return this._iFuelState != CommonShipSupplyState.SupplyState.Green || this._iAmmoState != CommonShipSupplyState.SupplyState.Green;
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref this.FuelState);
			Mem.Del(ref this.AmmoState);
			Mem.Del<CommonShipSupplyState.SupplyState>(ref this._iFuelState);
			Mem.Del<CommonShipSupplyState.SupplyState>(ref this._iAmmoState);
		}

		public void setSupplyState(ShipModel ship)
		{
			if (ship.AmmoRate >= 100.0)
			{
				this.AmmoState.spriteName = "icon_green";
				this._iAmmoState = CommonShipSupplyState.SupplyState.Green;
			}
			else if (ship.AmmoRate > 50.0 && ship.AmmoRate < 100.0)
			{
				this.AmmoState.spriteName = "icon_yellow";
				this._iAmmoState = CommonShipSupplyState.SupplyState.Yellow;
			}
			else
			{
				this._iAmmoState = CommonShipSupplyState.SupplyState.Red;
				this.AmmoState.spriteName = "icon_red";
			}
			if (ship.FuelRate >= 100.0)
			{
				this.FuelState.spriteName = "icon_green";
				this._iFuelState = CommonShipSupplyState.SupplyState.Green;
			}
			else if (ship.FuelRate > 50.0 && ship.FuelRate < 100.0)
			{
				this.FuelState.spriteName = "icon_yellow";
				this._iFuelState = CommonShipSupplyState.SupplyState.Yellow;
			}
			else
			{
				this.FuelState.spriteName = "icon_red";
				this._iFuelState = CommonShipSupplyState.SupplyState.Red;
			}
		}
	}
}
