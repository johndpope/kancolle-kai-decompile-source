using local.models;
using System;

namespace KCV.Arsenal
{
	public class ArsenalScrollSlotItemListChoiceModel
	{
		private SlotitemModel mSlotItemModel;

		public bool Selected
		{
			get;
			private set;
		}

		public ArsenalScrollSlotItemListChoiceModel(SlotitemModel slotItemModel, bool selected)
		{
			this.mSlotItemModel = slotItemModel;
			this.Selected = selected;
		}

		public SlotitemModel GetSlotItemModel()
		{
			return this.mSlotItemModel;
		}
	}
}
