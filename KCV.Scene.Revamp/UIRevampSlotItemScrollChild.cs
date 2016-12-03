using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampSlotItemScrollChild : UIScrollListChild<SlotitemModel>
	{
		private const int LEVEL_MAX = 10;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_LevelMax;

		[SerializeField]
		private GameObject mLevel;

		protected override void InitializeChildContents(SlotitemModel model, bool clickable)
		{
			if (model != null)
			{
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
				this.mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Type4;
			}
			else
			{
				this.mLabel_Name.text = "-";
				this.mSprite_WeaponTypeIcon.spriteName = "icon_slot_notset";
			}
		}
	}
}
