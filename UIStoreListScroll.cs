using KCV.InteriorStore;
using KCV.View.Scroll;
using local.models;
using System;

public class UIStoreListScroll : UIScrollListParent<FurnitureModel, UIStoreList>
{
	public void Refresh(FurnitureModel[] furnitureModels)
	{
		base.Refresh(furnitureModels);
	}

	public UIStoreList[] GetChild()
	{
		return this.Views;
	}

	public FurnitureModel[] GetModels()
	{
		return this.Models;
	}
}
