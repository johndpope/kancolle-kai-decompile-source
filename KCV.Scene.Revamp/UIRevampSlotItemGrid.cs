using KCV.View;
using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemGrid : BaseUISummaryGrid<UIRevampSlotItemSummary, SlotitemModel>
	{
		public enum ActionType
		{
			Back,
			Select
		}

		public delegate void UIRevampSlotItemGridAction(UIRevampSlotItemGrid.ActionType actionType, UIRevampSlotItemGrid calledObject, UIRevampSlotItemSummary summary);

		private KeyControl mKeyController;

		private UIRevampSlotItemSummary mFocusSummary;

		private UIRevampSlotItemGrid.UIRevampSlotItemGridAction mRevampSlotItemGridActionCallBack;

		private RevampRecipeModel mRevampRecipeModel;

		[Obsolete]
		public override void Initialize(SlotitemModel[] models)
		{
		}

		public void Initialize(SlotitemModel[] models, RevampRecipeModel revampRecipe)
		{
			base.Initialize(models);
			this.mRevampRecipeModel = revampRecipe;
		}

		public RevampRecipeModel GetRevampRecipe()
		{
			return this.mRevampRecipeModel;
		}

		public void SetOnRevampSlotItemGridActionCallBack(UIRevampSlotItemGrid.UIRevampSlotItemGridAction callBack)
		{
			this.mRevampSlotItemGridActionCallBack = callBack;
		}

		private void ChangeFocusSummary(UIRevampSlotItemSummary summary)
		{
			if (this.mFocusSummary != null)
			{
				this.mFocusSummary.RemoveHover();
			}
			this.mFocusSummary = summary;
			if (this.mFocusSummary != null)
			{
				this.mFocusSummary.Hover();
			}
		}

		public KeyControl GetKeyController()
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.ChangeFocusSummary(this.GetSummaryView(0));
			return this.mKeyController;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					int num = this.mFocusSummary.GetIndex() - 1;
					if (0 <= num)
					{
						this.ChangeFocusSummary(this.GetSummaryView(num));
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					int num2 = this.mFocusSummary.GetIndex() + 1;
					if (num2 < base.GetCurrentViewCount())
					{
						this.ChangeFocusSummary(this.GetSummaryView(num2));
					}
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					int num3 = base.GetCurrentPageIndex() - 1;
					if (0 <= num3)
					{
						this.GoToPage(num3);
						this.ChangeFocusSummary(this.GetSummaryView(0));
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num4 = base.GetCurrentPageIndex() + 1;
					if (num4 < base.GetPageSize())
					{
						this.GoToPage(num4);
						this.ChangeFocusSummary(this.GetSummaryView(0));
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnCallBack(UIRevampSlotItemGrid.ActionType.Select, this.mFocusSummary);
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnCallBack(UIRevampSlotItemGrid.ActionType.Back, this.mFocusSummary);
				}
			}
		}

		public void OnCallBack(UIRevampSlotItemGrid.ActionType actionType, UIRevampSlotItemSummary summary)
		{
			if (this.mRevampSlotItemGridActionCallBack != null)
			{
				this.mRevampSlotItemGridActionCallBack(actionType, this, summary);
			}
		}
	}
}
