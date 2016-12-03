using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeScrollChild : UIScrollListChild<RevampRecipeModel>
	{
		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UILabel mLabel_WeaponName;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_DevKit;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		[SerializeField]
		private UIButton mButton_Select;

		[SerializeField]
		private UISprite mSprite_ButtonState;

		protected override void InitializeChildContents(RevampRecipeModel model, bool clickable)
		{
			base.StopBlink();
			if (model != null)
			{
				this.mTexture_WeaponThumbnail.mainTexture = (Resources.Load("Textures/SlotItems/" + model.Slotitem.MstId + "/2") as Texture);
				this.mLabel_WeaponName.text = model.Slotitem.Name;
				this.mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Slotitem.Type4;
				this.mLabel_Fuel.text = model.Fuel.ToString();
				this.mLabel_Steel.text = model.Steel.ToString();
				this.mLabel_DevKit.text = model.DevKit.ToString();
				this.mLabel_Ammo.text = model.Ammo.ToString();
				this.mLabel_Bauxite.text = model.Baux.ToString();
				this.mLabel_RevampKit.text = model.RevKit.ToString();
				if (clickable)
				{
					this.mSprite_ButtonState.spriteName = "btn_select";
					this.mButton_Action.isEnabled = true;
					this.mButton_Select.isEnabled = true;
				}
				else
				{
					this.mSprite_ButtonState.spriteName = "btn_select_off";
					this.mButton_Action.isEnabled = false;
					this.mButton_Select.SetEnableCollider2D(false);
				}
			}
			else
			{
				this.mLabel_WeaponName.text = "-";
				this.mSprite_WeaponTypeIcon.spriteName = "icon_slot_notset";
			}
		}

		public override void Hover()
		{
			base.Hover();
			if (base.mIsClickable)
			{
				this.mSprite_ButtonState.spriteName = "btn_select_on";
			}
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			if (base.mIsClickable)
			{
				this.mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				this.mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}
	}
}
