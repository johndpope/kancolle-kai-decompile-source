using Common.Enum;
using local.models;
using System;

namespace KCV.InteriorStore
{
	internal class Context
	{
		public FurnitureKinds SelectedCategory
		{
			get;
			private set;
		}

		public FurnitureModel SelectedFurniture
		{
			get;
			private set;
		}

		public void SetSelectedCategory(FurnitureKinds furnitureKind)
		{
			this.SelectedCategory = furnitureKind;
		}

		public void SetSelectedFurniture(FurnitureModel furnitureModel)
		{
			this.SelectedFurniture = furnitureModel;
		}
	}
}
