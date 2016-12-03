using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodeModernizationShipTargetListChild : AbstractUIRemodelListChild<RemodeModernizationShipTargetListChild>
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private Transform mLEVEL;

		[SerializeField]
		private Transform mUNSET;

		[SerializeField]
		private UISprite mListBar;

		protected override void InitializeChildContents(RemodeModernizationShipTargetListChild childData, bool clickable)
		{
			RemodeModernizationShipTargetListChild.ListItemOption mOption = childData.mOption;
			if (mOption != RemodeModernizationShipTargetListChild.ListItemOption.Model)
			{
				if (mOption == RemodeModernizationShipTargetListChild.ListItemOption.UnSet)
				{
					this.InitializeUnset();
				}
			}
			else
			{
				this.InitializeModel(childData.mShipModel);
			}
		}

		private void InitializeUnset()
		{
			this.mCommonShipBanner.SetActive(false);
			this.mSprite_Karyoku.alpha = 0f;
			this.mSprite_Raisou.alpha = 0f;
			this.mSprite_Soukou.alpha = 0f;
			this.mSprite_Taikuu.alpha = 0f;
			this.mSprite_Luck.alpha = 0f;
			this.mLabel_Name.alpha = 0f;
			this.mLabel_Level.alpha = 0f;
			this.mLabel_Level.text = "はずす";
			this.mLabel_Name.text = string.Empty;
			this.mLEVEL.set_localScale(Vector3.get_zero());
			this.mUNSET.set_localScale(Vector3.get_one());
			this.mListBar.color = Color.get_white();
		}

		private void InitializeModel(ShipModel shipModel)
		{
			this.mCommonShipBanner.SetActive(true);
			this.mSprite_Karyoku.alpha = 1f;
			this.mSprite_Raisou.alpha = 1f;
			this.mSprite_Soukou.alpha = 1f;
			this.mSprite_Taikuu.alpha = 1f;
			this.mSprite_Luck.alpha = 1f;
			this.mLabel_Name.alpha = 1f;
			this.mLabel_Level.alpha = 1f;
			this.mLEVEL.set_localScale(Vector3.get_one());
			this.mUNSET.set_localScale(Vector3.get_zero());
			this.mListBar.color = Color.get_white();
			if (0 < shipModel.PowUpKaryoku)
			{
				this.mSprite_Karyoku.spriteName = "icon_1_on";
			}
			else
			{
				this.mSprite_Karyoku.spriteName = "icon_1";
			}
			if (0 < shipModel.PowUpRaisou)
			{
				this.mSprite_Raisou.spriteName = "icon_2_on";
			}
			else
			{
				this.mSprite_Raisou.spriteName = "icon_2";
			}
			if (0 < shipModel.PowUpSoukou)
			{
				this.mSprite_Soukou.spriteName = "icon_3_on";
			}
			else
			{
				this.mSprite_Soukou.spriteName = "icon_3";
			}
			if (0 < shipModel.PowUpTaikuu)
			{
				this.mSprite_Taikuu.spriteName = "icon_4_on";
			}
			else
			{
				this.mSprite_Taikuu.spriteName = "icon_4";
			}
			if (0 < shipModel.PowUpLucky)
			{
				this.mSprite_Luck.spriteName = "icon_5_on";
			}
			else
			{
				this.mSprite_Luck.spriteName = "icon_5";
			}
			this.mCommonShipBanner.SetShipDataWithDisableParticle(shipModel);
			this.mLabel_Level.text = shipModel.Level.ToString();
			this.mLabel_Name.text = shipModel.Name;
		}

		public override void OnTouchScrollListChild()
		{
			if (base.IsShown)
			{
				base.OnTouchScrollListChild();
			}
		}
	}
}
