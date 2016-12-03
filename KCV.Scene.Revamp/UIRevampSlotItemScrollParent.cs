using KCV.View.Scroll;
using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemScrollParent : UIScrollListParent<SlotitemModel, UIRevampSlotItemScrollChild>
	{
		private RevampRecipeModel mRevampRecipeModel;

		public void Initialize(RevampRecipeModel recipeModel, SlotitemModel[] slotItemModels)
		{
			this.mRevampRecipeModel = recipeModel;
			base.Initialize(slotItemModels);
		}

		public RevampRecipeModel GetRevampRecipeModel()
		{
			return this.mRevampRecipeModel;
		}
	}
}
