using KCV.View.Scroll;
using System;
using UnityEngine;

public class UIInteriorFurnitureChangeScrollListChild : UIScrollListChild<UIInteriorFurnitureChangeScrollListChildModel>
{
	[SerializeField]
	private Transform mEquipMark;

	[SerializeField]
	private UILabel mLabel_Name;

	protected override void InitializeChildContents(UIInteriorFurnitureChangeScrollListChildModel model, bool isClickable)
	{
		base.InitializeChildContents(model, isClickable);
		this.mLabel_Name.text = base.Model.GetName();
		bool flag = model.IsConfiguredInDeck();
		if (flag)
		{
			this.mEquipMark.SetActive(true);
		}
		else
		{
			this.mEquipMark.SetActive(false);
		}
	}
}
