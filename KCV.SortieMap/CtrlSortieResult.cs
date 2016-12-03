using Common.Enum;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlSortieResult : InstantiateObject<CtrlSortieResult>
	{
		[Serializable]
		private class GetItemInfo : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UILabel _uiLabel;

			[SerializeField]
			private UIGrid _uiItemAnchor;

			[SerializeField]
			private UIAtlas _uiAtlas;

			private List<UISprite> _listItemIcon;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public bool Init(List<MapEventItemModel> items)
			{
				this.transform.localScaleY(0f);
				this.CreateGetItemIcon(items);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<UITexture>(ref this._uiBackground);
				Mem.Del<UILabel>(ref this._uiLabel);
				Mem.Del<UIAtlas>(ref this._uiAtlas);
			}

			private void CreateGetItemIcon(List<MapEventItemModel> items)
			{
				this._listItemIcon = new List<UISprite>();
				if (items != null && items.get_Count() > 0)
				{
					int cnt = 0;
					items.ForEach(delegate(MapEventItemModel x)
					{
						GameObject gameObject = new GameObject(string.Format("GetItem{0}", cnt));
						gameObject.get_transform().set_parent(this._uiItemAnchor.get_transform());
						gameObject.get_transform().localPositionZero();
						this._listItemIcon.Add(gameObject.AddComponent<UISprite>());
						this._listItemIcon.get_Item(cnt).atlas = this._uiAtlas;
						this._listItemIcon.get_Item(cnt).spriteName = string.Format("item_{0}", this.GetItemNum(x));
						this._listItemIcon.get_Item(cnt).MakePixelPerfect();
						this._listItemIcon.get_Item(cnt).depth = 1;
						this._listItemIcon.get_Item(cnt).alpha = 0f;
						this._listItemIcon.get_Item(cnt).get_transform().set_localScale(Vector3.get_one() * 0.8f);
						cnt++;
					});
					this._uiItemAnchor.Reposition();
				}
			}

			[DebuggerHidden]
			public IEnumerator Show(IObserver<bool> observer)
			{
				CtrlSortieResult.GetItemInfo.<Show>c__Iterator117 <Show>c__Iterator = new CtrlSortieResult.GetItemInfo.<Show>c__Iterator117();
				<Show>c__Iterator.observer = observer;
				<Show>c__Iterator.<$>observer = observer;
				<Show>c__Iterator.<>f__this = this;
				return <Show>c__Iterator;
			}

			[DebuggerHidden]
			public IEnumerator Hide(IObserver<bool> observer)
			{
				CtrlSortieResult.GetItemInfo.<Hide>c__Iterator118 <Hide>c__Iterator = new CtrlSortieResult.GetItemInfo.<Hide>c__Iterator118();
				<Hide>c__Iterator.observer = observer;
				<Hide>c__Iterator.<$>observer = observer;
				<Hide>c__Iterator.<>f__this = this;
				return <Hide>c__Iterator;
			}

			private string GetItemNum(MapEventItemModel model)
			{
				string result = string.Empty;
				if (model.IsMaterial())
				{
					switch (model.MaterialCategory)
					{
					case enumMaterialCategory.Fuel:
						result = "31";
						break;
					case enumMaterialCategory.Bull:
						result = "32";
						break;
					case enumMaterialCategory.Steel:
						result = "33";
						break;
					case enumMaterialCategory.Bauxite:
						result = "34";
						break;
					case enumMaterialCategory.Build_Kit:
						result = "2";
						break;
					case enumMaterialCategory.Repair_Kit:
						result = "1";
						break;
					case enumMaterialCategory.Dev_Kit:
						result = "3";
						break;
					case enumMaterialCategory.Revamp_Kit:
						result = "4";
						break;
					}
				}
				else if (model.IsUseItem())
				{
					result = model.ItemID.ToString();
				}
				return result;
			}
		}

		[SerializeField]
		private UIButton _uiGearButton;

		[SerializeField]
		private CtrlSortieResult.GetItemInfo _clsGetItemInfo;

		private UIPanel _uiPanel;

		private Action _actOnDecide;

		private bool _isInputPossible;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static CtrlSortieResult Instantiate(CtrlSortieResult prefab, Transform parent, List<MapEventItemModel> items, Action onDecide)
		{
			CtrlSortieResult ctrlSortieResult = InstantiateObject<CtrlSortieResult>.Instantiate(prefab, parent);
			ctrlSortieResult.Init(items, onDecide);
			return ctrlSortieResult;
		}

		private void OnDestroy()
		{
		}

		private bool Init(List<MapEventItemModel> items, Action onDecide)
		{
			this._actOnDecide = onDecide;
			this._isInputPossible = false;
			this._uiGearButton.GetComponent<BoxCollider2D>().set_enabled(false);
			this._uiGearButton.onClick = Util.CreateEventDelegateList(this, "OnDecide", null);
			this._clsGetItemInfo.Init(items);
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this._clsGetItemInfo.Show(observer)).Subscribe(delegate(bool _)
			{
				this._isInputPossible = true;
				this._uiGearButton.GetComponent<BoxCollider2D>().set_enabled(true);
			});
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (!this._isInputPossible)
			{
				return false;
			}
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this.OnDecide();
				return true;
			}
			return false;
		}

		private void OnDecide()
		{
			this._isInputPossible = false;
			this._uiGearButton.GetComponent<BoxCollider2D>().set_enabled(false);
			this._uiGearButton.get_transform().LTValue(1f, 0f, 0.3f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiGearButton.GetComponent<UISprite>().alpha = x;
			});
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this._clsGetItemInfo.Hide(observer)).Subscribe(delegate(bool _)
			{
				Dlg.Call(ref this._actOnDecide);
			});
		}
	}
}
