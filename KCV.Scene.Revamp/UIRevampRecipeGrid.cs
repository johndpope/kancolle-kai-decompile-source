using KCV.View;
using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeGrid : BaseUISummaryGrid<UIRevampRecipeSummary, RevampRecipeModel>
	{
		public enum ActionType
		{
			Back,
			Select
		}

		public delegate bool UIRevampRecipeGridCheckDelegate(UIRevampRecipeSummary summary);

		public delegate void UIRevampRecipeGridAction(UIRevampRecipeGrid.ActionType actionType, UIRevampRecipeGrid calledObject, UIRevampRecipeSummary summary);

		private UIRevampRecipeGrid.UIRevampRecipeGridCheckDelegate mRevampSummarySelectableCheckDelegate;

		private UIRevampRecipeGrid.UIRevampRecipeGridAction mRevampSummaryActionCallBack;

		private KeyControl mKeyController;

		private UIRevampRecipeSummary mFocusSummary;

		[Obsolete]
		public override void Initialize(RevampRecipeModel[] models)
		{
		}

		public void Initialize(RevampRecipeModel[] models, UIRevampRecipeGrid.UIRevampRecipeGridCheckDelegate summarySelectableCheckDelegate)
		{
			this.mRevampSummarySelectableCheckDelegate = summarySelectableCheckDelegate;
			base.Initialize(models);
		}

		public void SetOnRevampGridActionDelegate(UIRevampRecipeGrid.UIRevampRecipeGridAction gridActionDelegate)
		{
			this.mRevampSummaryActionCallBack = gridActionDelegate;
		}

		public override void OnFinishedCreateViews()
		{
			UIRevampRecipeSummary[] summaryViews = this.GetSummaryViews();
			for (int i = 0; i < summaryViews.Length; i++)
			{
				UIRevampRecipeSummary uIRevampRecipeSummary = summaryViews[i];
				if (!this.mRevampSummarySelectableCheckDelegate(uIRevampRecipeSummary))
				{
					uIRevampRecipeSummary.OnDisabled();
				}
				iTween.MoveTo(uIRevampRecipeSummary.get_gameObject(), iTween.Hash(new object[]
				{
					"x",
					20f,
					"isLocal",
					true,
					"time",
					0.3f,
					"delay",
					(float)uIRevampRecipeSummary.GetIndex() * 0.1f
				}));
				TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(uIRevampRecipeSummary.get_gameObject(), 0.3f);
				tweenAlpha.from = 0.1f;
				tweenAlpha.to = 1f;
				tweenAlpha.delay = (float)uIRevampRecipeSummary.GetIndex() * 0.1f;
				tweenAlpha.PlayForward();
			}
		}

		public KeyControl GetKeyController(int firstFocusIndex)
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.ChangeFocusSummary(this.GetSummaryView(firstFocusIndex));
			return this.mKeyController;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					if (0 <= this.mFocusSummary.GetIndex() - 1)
					{
						UIRevampRecipeSummary summaryView = this.GetSummaryView(this.mFocusSummary.GetIndex() - 1);
						this.ChangeFocusSummary(summaryView);
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					if (this.mFocusSummary.GetIndex() + 1 < base.GetCurrentViewCount())
					{
						UIRevampRecipeSummary summaryView2 = this.GetSummaryView(this.mFocusSummary.GetIndex() + 1);
						this.ChangeFocusSummary(summaryView2);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnCallBack(UIRevampRecipeGrid.ActionType.Select, this.mFocusSummary);
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnCallBack(UIRevampRecipeGrid.ActionType.Back, this.mFocusSummary);
				}
			}
		}

		private void RecipeSelectAnimation(UIRevampRecipeSummary selectedSummary)
		{
			switch (selectedSummary.GetIndex())
			{
			}
		}

		private void OnCallBack(UIRevampRecipeGrid.ActionType actionType, UIRevampRecipeSummary summary)
		{
			if (this.mRevampSummaryActionCallBack != null)
			{
				this.mRevampSummaryActionCallBack(actionType, this, summary);
			}
		}

		private void ChangeFocusSummary(UIRevampRecipeSummary summary)
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
	}
}
