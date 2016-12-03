using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeSummary : BaseUISummary<RevampRecipeModel>
	{
		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UIButton mButton_Select;

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

		public bool Disabled
		{
			get;
			private set;
		}

		public override void Initialize(int index, RevampRecipeModel model)
		{
			base.Initialize(index, model);
			this.mLabel_WeaponName.text = model.Slotitem.Name;
			this.mLabel_Fuel.text = model.Fuel.ToString();
			this.mLabel_Steel.text = model.Steel.ToString();
			this.mLabel_DevKit.text = model.DevKit.ToString();
			this.mLabel_Ammo.text = model.Ammo.ToString();
			this.mLabel_Bauxite.text = model.Baux.ToString();
			this.mLabel_RevampKit.text = model.RevKit.ToString();
		}

		public override void Hover()
		{
			base.Hover();
			if (!this.Disabled)
			{
				this.mButton_Select.SetState(UIButtonColor.State.Hover, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this.mButton_Select, true);
			}
			else
			{
				Debug.LogWarning("TODO:選択不可時のHover表示");
			}
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
			if (!this.Disabled)
			{
				this.mButton_Select.SetState(UIButtonColor.State.Normal, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this.mButton_Select, false);
			}
			else
			{
				Debug.LogWarning("TODO:選択不可時のRemoveHover表示");
			}
		}

		public void OnDisabled()
		{
			this.Disabled = true;
			this.mButton_Select.set_enabled(false);
			this.mButton_Select.SetState(UIButtonColor.State.Disabled, true);
		}
	}
}
