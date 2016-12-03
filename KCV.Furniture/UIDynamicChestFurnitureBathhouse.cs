using Common.Enum;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicChestFurnitureBathhouse : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Clothes;

		[SerializeField]
		private Texture mTexture2d_BattleShipClothes;

		[SerializeField]
		private Texture mTexture2d_AircraftCarrierClothes;

		[SerializeField]
		private Texture mTexture2d_LightAircraftCarrierClothes;

		[SerializeField]
		private Texture mTexture2d_DestroyterClothes;

		[SerializeField]
		private Texture mTexture2d_SubmarineClothes;

		[SerializeField]
		private Texture mTexture2d_DefaultClothes;

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			ShipModel flagShip = uiFurnitureModel.GetDeck().GetFlagShip();
			SType shipType = (SType)flagShip.ShipType;
			this.InitializeClothes(shipType);
		}

		private void InitializeClothes(SType shipType)
		{
			switch (shipType)
			{
			case SType.LightAircraftCarrier:
				this.mTexture_Clothes.mainTexture = this.mTexture2d_LightAircraftCarrierClothes;
				return;
			case SType.BattleCruiser:
				this.mTexture_Clothes.mainTexture = this.mTexture2d_BattleShipClothes;
				return;
			case SType.BattleShip:
			case SType.AviationBattleShip:
			case SType.SuperDreadnought:
				IL_26:
				if (shipType == SType.Destroyter)
				{
					this.mTexture_Clothes.mainTexture = this.mTexture2d_DestroyterClothes;
					return;
				}
				if (shipType != SType.SubmarineTender)
				{
					this.mTexture_Clothes.mainTexture = this.mTexture2d_DefaultClothes;
					return;
				}
				goto IL_92;
			case SType.AircraftCarrier:
				this.mTexture_Clothes.mainTexture = this.mTexture2d_AircraftCarrierClothes;
				return;
			case SType.Submarine:
				goto IL_92;
			}
			goto IL_26;
			IL_92:
			this.mTexture_Clothes.mainTexture = this.mTexture2d_SubmarineClothes;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Clothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_BattleShipClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_AircraftCarrierClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_LightAircraftCarrierClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_DestroyterClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_SubmarineClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_DefaultClothes, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Clothes, false);
		}
	}
}
