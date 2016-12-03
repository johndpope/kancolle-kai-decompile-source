using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdBalloon : AbsBalloon
	{
		[Serializable]
		private class Item
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private UISprite _uiItemIcon;

			[SerializeField]
			private UILabel _uiLabel;

			public Transform transform
			{
				get
				{
					return this._uiWidget.get_transform();
				}
			}

			public int depth
			{
				set
				{
					this._uiItemIcon.depth = value;
					this._uiLabel.depth = value + 1;
				}
			}

			public UISprite itemIcon
			{
				get
				{
					return this._uiItemIcon;
				}
			}

			public UIWidget widget
			{
				get
				{
					return this._uiWidget;
				}
			}

			public bool Init(MapEventItemModel itemModel)
			{
				string spriteName = string.Empty;
				if (itemModel.IsMaterial())
				{
					switch (itemModel.MaterialCategory)
					{
					case enumMaterialCategory.Fuel:
						spriteName = "icon_item1";
						break;
					case enumMaterialCategory.Bull:
						spriteName = "icon_item2";
						break;
					case enumMaterialCategory.Steel:
						spriteName = "icon_item3";
						break;
					case enumMaterialCategory.Bauxite:
						spriteName = "icon_item4";
						break;
					case enumMaterialCategory.Build_Kit:
						spriteName = "icon_item8";
						break;
					case enumMaterialCategory.Repair_Kit:
						spriteName = "icon_item6";
						break;
					case enumMaterialCategory.Dev_Kit:
						spriteName = "icon_item7";
						break;
					}
					this._uiItemIcon.spriteName = spriteName;
					this._uiItemIcon.MakePixelPerfect();
					this._uiLabel.text = string.Format("×{0}", itemModel.Count);
					return true;
				}
				this._uiItemIcon.spriteName = spriteName;
				return false;
			}

			public bool UnInit()
			{
				Mem.Del<UIWidget>(ref this._uiWidget);
				Mem.Del(ref this._uiItemIcon);
				Mem.Del<UILabel>(ref this._uiLabel);
				return true;
			}
		}

		[Serializable]
		private class AirRecResult
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private ProdBalloon.Item _clsItem;

			[SerializeField]
			private UISprite _uiGetLabel;

			[SerializeField]
			private UISprite _uiResultLabel;

			public Transform transform
			{
				get
				{
					return this._uiWidget.get_transform();
				}
			}

			public int depth
			{
				set
				{
					this._uiResultLabel.depth = value;
					this._uiGetLabel.depth = value;
					this._clsItem.depth = value;
				}
			}

			public bool Init(MissionResultKinds iKind, MapEventItemModel itemModel)
			{
				this.SetSprite(iKind);
				this.SetItem(iKind, itemModel);
				return true;
			}

			public bool UnInit()
			{
				Mem.Del<UIWidget>(ref this._uiWidget);
				this._clsItem.UnInit();
				Mem.Del<ProdBalloon.Item>(ref this._clsItem);
				Mem.Del(ref this._uiGetLabel);
				Mem.Del(ref this._uiResultLabel);
				return true;
			}

			private void SetItem(MissionResultKinds iKind, MapEventItemModel itemModel)
			{
				if (iKind == MissionResultKinds.FAILE)
				{
					this._clsItem.widget.alpha = 0f;
				}
				else
				{
					this._clsItem.Init(itemModel);
					this._clsItem.itemIcon.get_transform().set_localScale(Vector3.get_one() * 0.6f);
					this._clsItem.widget.alpha = 1f;
				}
			}

			private void SetSprite(MissionResultKinds iKind)
			{
				switch (iKind)
				{
				case MissionResultKinds.FAILE:
					this._uiResultLabel.spriteName = "txt4";
					this._uiResultLabel.MakePixelPerfect();
					break;
				case MissionResultKinds.SUCCESS:
					this._uiResultLabel.spriteName = "txt2";
					this._uiResultLabel.MakePixelPerfect();
					break;
				case MissionResultKinds.GREAT:
					this._uiResultLabel.spriteName = "txt1";
					this._uiResultLabel.MakePixelPerfect();
					break;
				}
				Vector3 localPosition = (iKind != MissionResultKinds.FAILE) ? (Vector3.get_up() * 26f) : Vector3.get_zero();
				this._uiResultLabel.get_transform().set_localPosition(localPosition);
				this._uiGetLabel.alpha = ((iKind != MissionResultKinds.FAILE) ? 1f : 0f);
			}
		}

		[SerializeField]
		private UILabel _uiText;

		[SerializeField]
		private ProdBalloon.Item _uiItem;

		[SerializeField]
		private ProdBalloon.AirRecResult _clsAirRecResult;

		public int depth
		{
			set
			{
				base.sprite.depth = value;
				this._uiText.depth = value + 1;
				this._uiItem.depth = value + 1;
				this._clsAirRecResult.depth = value + 1;
			}
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, enumMapEventType iEventType, enumMapWarType iWarType)
		{
			ProdBalloon prodBalloon = Object.Instantiate<ProdBalloon>(prefab);
			prodBalloon.get_transform().set_parent(parent);
			prodBalloon.get_transform().localPositionZero();
			prodBalloon.get_transform().localScaleZero();
			prodBalloon.InitText(iDirection, iEventType, iWarType);
			return prodBalloon;
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapEventItemModel itemModel)
		{
			ProdBalloon prodBalloon = Object.Instantiate<ProdBalloon>(prefab);
			prodBalloon.get_transform().set_parent(parent);
			prodBalloon.get_transform().localPositionZero();
			prodBalloon.get_transform().localScaleZero();
			prodBalloon.InitPortBackEo(iDirection, itemModel);
			return prodBalloon;
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel)
		{
			ProdBalloon prodBalloon = Object.Instantiate<ProdBalloon>(prefab);
			prodBalloon.get_transform().set_parent(parent);
			prodBalloon.get_transform().localPositionZero();
			prodBalloon.get_transform().localScaleZero();
			prodBalloon.InitAirRec(iDirection, eventAirRecModel, eventItemModel);
			return prodBalloon;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UILabel>(ref this._uiText);
			this._uiItem.UnInit();
			Mem.Del<ProdBalloon.Item>(ref this._uiItem);
			this._clsAirRecResult.UnInit();
			Mem.Del<ProdBalloon.AirRecResult>(ref this._clsAirRecResult);
		}

		private bool InitText(UISortieShip.Direction iDirection, enumMapEventType iEventType, enumMapWarType iWarType)
		{
			this._uiItem.transform.localScaleZero();
			this._clsAirRecResult.transform.localScaleZero();
			this._uiText.get_transform().localScaleOne();
			this.SetBalloonPos(iDirection);
			this.SetText(iEventType, iWarType);
			return true;
		}

		private void SetText(enumMapEventType iEventType, enumMapWarType iWarType)
		{
			string text = string.Empty;
			if (iEventType == enumMapEventType.Stupid)
			{
				if (iWarType == enumMapWarType.Midnight)
				{
					text = "艦隊針路\n選択可能!";
				}
			}
			this._uiText.text = text;
		}

		private bool InitPortBackEo(UISortieShip.Direction iDirection, MapEventItemModel itemModel)
		{
			this._uiItem.transform.localScaleOne();
			this._uiItem.widget.alpha = 1f;
			this._clsAirRecResult.transform.localScaleZero();
			this._uiText.get_transform().localScaleZero();
			this.SetBalloonPos(iDirection);
			this.SetGetItem(itemModel);
			return true;
		}

		private void SetGetItem(MapEventItemModel itemModel)
		{
			this._uiItem.Init(itemModel);
		}

		private bool InitAirRec(UISortieShip.Direction iDirection, MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel)
		{
			this._uiItem.transform.localScaleZero();
			this._uiText.get_transform().localScaleZero();
			this.SetBalloonPos(iDirection);
			this._clsAirRecResult.Init(eventAirRecModel.SearchResult, eventItemModel);
			this._clsAirRecResult.transform.localScaleOne();
			return true;
		}

		protected override void SetBalloonPos(UISortieShip.Direction iDirection)
		{
			if (iDirection != UISortieShip.Direction.Left)
			{
				if (iDirection == UISortieShip.Direction.Right)
				{
					base.get_transform().set_localPosition(new Vector3(-71f, 17f, 0f));
					base.sprite.flip = UIBasicSprite.Flip.Horizontally;
				}
			}
			else
			{
				base.get_transform().set_localPosition(new Vector3(71f, 17f, 0f));
				base.sprite.flip = UIBasicSprite.Flip.Horizontally;
			}
		}
	}
}
