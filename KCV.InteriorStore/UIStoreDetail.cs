using local.models;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIStoreDetail : BaseUIFurnitureDetail
	{
		[Serializable]
		protected new class Preview : BaseUIFurnitureDetail.Preview
		{
			public Preview(Transform parent, string objName) : base(parent, objName)
			{
				Util.FindParentToChild<Transform>(ref this._traObj, parent, objName);
				Util.FindParentToChild<UIPanel>(ref this._uiMaskPanel, this._traObj, "Mask");
				Util.FindParentToChild<UITexture>(ref this._uiFurnitureTex, this._uiMaskPanel.get_transform(), "Offs/FurnitureTex");
				Util.FindParentToChild<UISprite>(ref this._uiWorker, this._uiMaskPanel.get_transform(), "Worker");
				this._uiWorker.SetActive(false);
				this._uiStars = new UISprite[BaseUIFurnitureDetail.RARE_STAR_MAX];
				for (int i = 0; i < this._uiStars.Length; i++)
				{
					Util.FindParentToChild<UISprite>(ref this._uiStars[i], this._uiMaskPanel.get_transform().FindChild("RareStars").get_transform(), string.Format("Star{0}", i + 1));
					this._uiStars[i].spriteName = "icon_star_bg";
				}
			}

			public void SetFurniture(FurnitureModel model)
			{
				this._setFurnitureTex(model);
				bool isActive = model.IsNeedWorker();
				this._uiWorker.SetActive(isActive);
				this._setRare(model);
			}

			private void _setRare(FurnitureModel model)
			{
				for (int i = 0; i < this._uiStars.Length; i++)
				{
					if (i < model.Rarity)
					{
						this._uiStars[i].spriteName = "icon_star";
					}
					else
					{
						this._uiStars[i].spriteName = "icon_star_bg";
					}
				}
			}
		}

		[Serializable]
		private class FCoinInfo
		{
			private Transform _traObj;

			private UISprite _uiBg;

			private UISprite _uiLabel;

			private UILabel _uiVal;

			private UILabel _uiSoldOut;

			public FCoinInfo(Transform parent, string objName)
			{
				Util.FindParentToChild<Transform>(ref this._traObj, parent, objName);
				Util.FindParentToChild<UISprite>(ref this._uiBg, this._traObj, "BG");
				Util.FindParentToChild<UISprite>(ref this._uiLabel, this._traObj, "Label");
				Util.FindParentToChild<UILabel>(ref this._uiVal, this._traObj, "Val");
				Util.FindParentToChild<UILabel>(ref this._uiSoldOut, this._traObj, "SoldOut");
			}

			public void SetFCoinInfo(FurnitureModel model)
			{
				this._uiVal.textInt = model.Price;
				this._uiVal.SetActive(!model.IsPossession());
				this._uiSoldOut.SetActive(model.IsPossession());
			}
		}

		private UIStoreDetail.Preview _uiPreviwe;

		private UIStoreDetail.FCoinInfo _uiFCoinInfo;

		private FurnitureModel _clsFStoreItemModel;

		public FurnitureModel DetailItem
		{
			get
			{
				return this._clsFStoreItemModel;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this._uiPreviwe = new UIStoreDetail.Preview(base.get_transform(), "Preview");
			this._uiFCoinInfo = new UIStoreDetail.FCoinInfo(base.get_transform(), "FCoinInfo");
		}

		public void SetDetail(FurnitureModel model)
		{
			base.SetDetail(model);
			this._uiPreviwe.SetFurniture(model);
			this._uiFCoinInfo.SetFCoinInfo(model);
			this._clsFStoreItemModel = model;
		}

		public void Clear()
		{
			this._clsFStoreItemModel = null;
		}
	}
}
