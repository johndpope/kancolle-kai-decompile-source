using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreListChild : UIScrollListChild<FurnitureModel>
	{
		[SerializeField]
		private UILabel CoinValue;

		[SerializeField]
		private UILabel Name;

		[SerializeField]
		private UILabel Detail;

		[SerializeField]
		private UISprite[] Stars;

		[SerializeField]
		private UILabel SoldOut;

		[SerializeField]
		private UITexture texture;

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			this.CoinValue.textInt = model.Price;
			this.Name.text = model.Name;
			this.Detail.text = model.Description;
			this.SetStars(model.Rarity);
			this.SoldOut.set_enabled(model.IsPossession());
			this.mButton_Action.isEnabled = clickable;
			this.texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(model.Type, model.MstId);
		}

		private void SetStars(int num)
		{
			for (int i = 0; i < this.Stars.Length; i++)
			{
				this.Stars[i].SetActive(num > i);
			}
		}
	}
}
