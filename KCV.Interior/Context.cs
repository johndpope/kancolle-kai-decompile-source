using Common.Enum;
using local.models;
using System;

namespace KCV.Interior
{
	internal class Context
	{
		private FurnitureKinds mSelectedKind;

		private FurnitureModel mSelectedFurnitureModel;

		public FurnitureKinds CurrentCategory
		{
			get
			{
				return this.mSelectedKind;
			}
		}

		public FurnitureModel SelectedFurniture
		{
			get
			{
				return this.mSelectedFurnitureModel;
			}
		}

		public void SetSelectFurnitureKind(FurnitureKinds furnitureKind)
		{
			this.mSelectedKind = furnitureKind;
		}

		public void SetSelectedFurniture(FurnitureModel furnitureModel)
		{
			this.mSelectedFurnitureModel = furnitureModel;
		}
	}
}
