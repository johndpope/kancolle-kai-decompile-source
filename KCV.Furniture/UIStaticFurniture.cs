using Common.Enum;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIStaticFurniture : UIFurniture
	{
		[SerializeField]
		private UITexture mTexture_Furniture;

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Furniture, false);
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			switch (uiFurnitureModel.GetFurnitureModel().Type)
			{
			case FurnitureKinds.Floor:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.Bottom;
				break;
			case FurnitureKinds.Wall:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.Top;
				break;
			case FurnitureKinds.Window:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.TopRight;
				break;
			case FurnitureKinds.Hangings:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.TopLeft;
				break;
			case FurnitureKinds.Chest:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.Right;
				break;
			case FurnitureKinds.Desk:
				this.mTexture_Furniture.pivot = UIWidget.Pivot.Left;
				break;
			}
			this.mTexture_Furniture.get_transform().set_localPosition(Vector3.get_zero());
			FurnitureModel furnitureModel = uiFurnitureModel.GetFurnitureModel();
			this.mTexture_Furniture.mainTexture = UIFurniture.LoadTexture(furnitureModel);
			Vector2 vector = this.GenerateDimensionForFurniture(furnitureModel);
			this.mTexture_Furniture.SetDimensions((int)vector.x, (int)vector.y);
		}

		private Vector2 GenerateDimensionForFurniture(FurnitureModel furnitureModel)
		{
			Vector2 result = Vector2.get_zero();
			switch (furnitureModel.Type)
			{
			case FurnitureKinds.Floor:
				result = this.GenerateDimensionForFloor(furnitureModel);
				break;
			case FurnitureKinds.Wall:
				result = this.GenerateDimensionForWall(furnitureModel);
				break;
			case FurnitureKinds.Window:
				result = this.GenerateDimensionForWindow(furnitureModel);
				break;
			case FurnitureKinds.Hangings:
				result = this.GenerateDimensionForHangings(furnitureModel);
				break;
			case FurnitureKinds.Chest:
				result = this.GenerateDimensionForChest(furnitureModel);
				break;
			case FurnitureKinds.Desk:
				result = this.GenerateDimensionForDesk(furnitureModel);
				break;
			}
			return result;
		}

		private Vector2 GenerateDimensionForFloor(FurnitureModel furnitureModel)
		{
			return new Vector2(960f, 236f);
		}

		private Vector2 GenerateDimensionForChest(FurnitureModel furnitureModel)
		{
			return new Vector2(495f, 544f);
		}

		private Vector2 GenerateDimensionForDesk(FurnitureModel furnitureModel)
		{
			int num = furnitureModel.NoInType + 1;
			int num2 = num;
			if (num2 != 26 && num2 != 37)
			{
				return new Vector2(530f, 544f);
			}
			return new Vector2(562f, 544f);
		}

		private Vector2 GenerateDimensionForHangings(FurnitureModel furnitureModel)
		{
			return new Vector2(258f, 394f);
		}

		private Vector2 GenerateDimensionForWall(FurnitureModel furnitureModel)
		{
			return new Vector2(960f, 438f);
		}

		private Vector2 GenerateDimensionForWindow(FurnitureModel furnitureModel)
		{
			int num = furnitureModel.NoInType + 1;
			int num2 = num;
			if (num2 != 8)
			{
				return new Vector2(684f, 400f);
			}
			return new Vector2(731f, 400f);
		}
	}
}
