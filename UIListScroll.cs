using KCV.InteriorStore;
using KCV.View.Scroll;
using local.models;
using System;

public class UIListScroll : UIScrollListParent<FurnitureModel, UIInteriorList>
{
	public UIInteriorList[] GetChild()
	{
		return this.Views;
	}

	public FurnitureModel[] GetModels()
	{
		return this.Models;
	}

	public void Initialize(FurnitureModel[] models)
	{
		this.SetActive(true);
		base.Initialize(models);
	}
}
