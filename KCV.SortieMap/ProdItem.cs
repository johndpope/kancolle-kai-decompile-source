using Common.Enum;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget)), RequireComponent(typeof(iTweenEvent))]
	public class ProdItem : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiItemIcon;

		[SerializeField]
		private UILabel _uiCount;

		[SerializeField]
		private Vector3 _vEndPos = new Vector3(-15f, 40f, 0f);

		private UIWidget _uiWidget;

		private iTweenEvent _iteLostItemAnim;

		private UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		private iTweenEvent lostItemAnim
		{
			get
			{
				return this.GetComponentThis(ref this._iteLostItemAnim);
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiItemIcon);
			Mem.Del<UILabel>(ref this._uiCount);
			Mem.Del<Vector3>(ref this._vEndPos);
			Mem.Del<iTweenEvent>(ref this._iteLostItemAnim);
			Mem.Del<UIWidget>(ref this._uiWidget);
		}

		public static ProdItem Instantiate(ProdItem prefab, Transform parent, MapEventItemModel model)
		{
			ProdItem prodItem = Object.Instantiate<ProdItem>(prefab);
			prodItem.get_transform().set_parent(parent);
			prodItem.get_transform().set_localScale(Vector3.get_one() * 1.3f);
			prodItem.get_transform().localPositionZero();
			prodItem.Init(model);
			return prodItem;
		}

		private bool Init(MapEventItemModel model)
		{
			this.widget.alpha = 0f;
			this._uiItemIcon.spriteName = string.Empty;
			this._uiCount.text = string.Empty;
			if (model.IsMaterial())
			{
				switch (model.ItemID)
				{
				case 1:
					this._uiItemIcon.spriteName = "icon-m1";
					break;
				case 2:
					this._uiItemIcon.spriteName = "icon-m2";
					break;
				case 3:
					this._uiItemIcon.spriteName = "icon-m3";
					break;
				case 4:
					this._uiItemIcon.spriteName = "icon-m4";
					break;
				case 5:
					this._uiItemIcon.spriteName = "icon-m10";
					break;
				case 6:
					this._uiItemIcon.spriteName = "icon-m5";
					break;
				case 7:
					this._uiItemIcon.spriteName = "icon-m8";
					break;
				case 8:
					this._uiItemIcon.spriteName = "icon-m16";
					break;
				default:
					this._uiItemIcon.spriteName = "icon_find";
					break;
				}
			}
			else if (model.IsUseItem())
			{
				switch (model.ItemID)
				{
				case 10:
					this._uiItemIcon.spriteName = "icon-m12";
					break;
				case 11:
					this._uiItemIcon.spriteName = "icon-m13";
					break;
				case 12:
					this._uiItemIcon.spriteName = "icon-m14";
					break;
				default:
					this._uiItemIcon.spriteName = "icon_find";
					break;
				}
			}
			this._uiCount.text = model.Count.ToString();
			return true;
		}

		public void PlayGetAnim(Action onFinished)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_032);
			Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate(long _)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_032);
			});
			float num = 1f;
			this.widget.alpha = 1f;
			this._uiWidget.get_transform().LTValue(1f, 0f, num * 0.1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			}).setDelay(num * 0.9f);
			base.get_transform().LTMoveLocal(this._vEndPos, num).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(this.get_transform().get_gameObject());
			});
		}

		public static ProdItem Instantiate(ProdItem prefab, Transform parent, MapEventHappeningModel model)
		{
			ProdItem prodItem = Object.Instantiate<ProdItem>(prefab);
			prodItem.get_transform().set_parent(parent);
			prodItem.get_transform().set_localScale(Vector3.get_one() * 1.3f);
			prodItem.get_transform().localPositionZero();
			prodItem.Init(model);
			return prodItem;
		}

		private bool Init(MapEventHappeningModel model)
		{
			this.widget.alpha = 0f;
			this._uiItemIcon.spriteName = string.Empty;
			this._uiCount.text = string.Empty;
			switch (model.Material)
			{
			case enumMaterialCategory.Fuel:
				this._uiItemIcon.spriteName = "icon-m1";
				break;
			case enumMaterialCategory.Bull:
				this._uiItemIcon.spriteName = "icon-m2";
				break;
			case enumMaterialCategory.Steel:
				this._uiItemIcon.spriteName = "icon-m3";
				break;
			case enumMaterialCategory.Bauxite:
				this._uiItemIcon.spriteName = "icon-m4";
				break;
			case enumMaterialCategory.Build_Kit:
				this._uiItemIcon.spriteName = "icon-m10";
				break;
			case enumMaterialCategory.Repair_Kit:
				this._uiItemIcon.spriteName = "icon-m5";
				break;
			case enumMaterialCategory.Dev_Kit:
				this._uiItemIcon.spriteName = "icon-m8";
				break;
			case enumMaterialCategory.Revamp_Kit:
				this._uiItemIcon.spriteName = "icon-m16";
				break;
			default:
				this._uiItemIcon.spriteName = "icon_find";
				break;
			}
			this._uiCount.text = model.Count.ToString();
			return true;
		}

		public void PlayLostAnim(Action onFinished)
		{
			this.widget.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			});
			Bezier bezier = new Bezier(Bezier.BezierType.Quadratic, Vector3.get_zero(), new Vector3(-75f, -60f, 0f), new Vector3(-45f, 100f, 0f), Vector3.get_zero());
			base.get_transform().LTValue(0f, 1f, 2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.get_transform().set_localPosition(bezier.Interpolate(x));
			});
			Observable.Timer(TimeSpan.FromSeconds(1.2000000476837158)).Subscribe(delegate(long _)
			{
				this.widget.get_transform().LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this.widget.alpha = x;
				}).setOnComplete(delegate
				{
					Object.Destroy(this.get_gameObject());
				});
				Dlg.Call(ref onFinished);
			});
		}
	}
}
