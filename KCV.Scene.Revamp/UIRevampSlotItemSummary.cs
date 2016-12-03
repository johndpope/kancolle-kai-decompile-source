using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemSummary : BaseUISummary<SlotitemModel>
	{
		private const int LEVEL_MAX = 10;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_LevelMax;

		[SerializeField]
		private UIButton mButton_Action;

		[SerializeField]
		private GameObject mLevel;

		public override void Initialize(int index, SlotitemModel model)
		{
			base.Initialize(index, model);
			if (model.Level == 10)
			{
				this.mSprite_LevelMax.SetActive(true);
			}
			else if (0 < model.Level && model.Level < 10)
			{
				this.mSprite_LevelMax.SetActive(false);
				this.mLevel.SetActive(true);
				this.mLabel_Level.text = model.Level.ToString();
			}
			else
			{
				this.mLevel.SetActive(false);
			}
			this.mLabel_Name.text = model.Name;
		}

		public override void Hover()
		{
			base.Hover();
			this.mButton_Action.SetState(UIButtonColor.State.Hover, true);
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			this.mButton_Action.SetState(UIButtonColor.State.Normal, true);
		}
	}
}
