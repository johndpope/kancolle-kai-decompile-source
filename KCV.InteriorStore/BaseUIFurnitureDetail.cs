using local.models;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class BaseUIFurnitureDetail : MonoBehaviour
	{
		[Serializable]
		protected class Preview
		{
			protected Transform _traObj;

			protected UIPanel _uiMaskPanel;

			protected UISprite _uiWorker;

			protected UISprite[] _uiStars;

			protected UITexture _uiPrviewBg;

			protected UITexture _uiFurnitureTex;

			public Preview(Transform parent, string objName)
			{
			}

			public virtual void SetFurniture(FurnitureModel model)
			{
			}

			protected virtual void _setFurnitureTex(FurnitureModel model)
			{
				this._uiFurnitureTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(model.Type, model.MstId);
			}
		}

		protected static readonly int RARE_STAR_MAX = 5;

		protected UILabel _uiName;

		protected UILabel _uiDescription;

		protected virtual void Awake()
		{
			Util.FindParentToChild<UILabel>(ref this._uiName, base.get_transform(), "Name");
			Util.FindParentToChild<UILabel>(ref this._uiDescription, base.get_transform(), "Description");
		}

		public virtual void SetDetail(FurnitureModel model)
		{
			this._uiName.text = model.Name;
			this._uiDescription.text = model.Description;
		}
	}
}
