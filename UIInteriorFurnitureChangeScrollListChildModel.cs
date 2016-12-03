using local.models;
using System;

public class UIInteriorFurnitureChangeScrollListChildModel
{
	private int mDeckId;

	private FurnitureModel mModel;

	public UIInteriorFurnitureChangeScrollListChildModel(int deckId, FurnitureModel model)
	{
		this.mDeckId = deckId;
		this.mModel = model;
	}

	public string GetName()
	{
		return this.mModel.Name;
	}

	public int GetDeckId()
	{
		return this.mDeckId;
	}

	public bool IsConfiguredInDeck()
	{
		return this.mModel.GetSettingFlg(this.mDeckId);
	}

	public FurnitureModel GetFurnitureModel()
	{
		return this.mModel;
	}
}
