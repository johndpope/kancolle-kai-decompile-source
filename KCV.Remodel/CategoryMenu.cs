using Common.Enum;
using Common.Struct;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class CategoryMenu : MonoBehaviour, UIRemodelView
	{
		private const int REMOVE_TOGGLE_IDX = -1;

		[SerializeField]
		private UIButton backAreaButton;

		[SerializeField]
		private GameObject itemContainer;

		[SerializeField]
		private CategoryMenuItem removeItem;

		private int currentIdx;

		private KeyControl keyController;

		private List<CategoryMenuItem> items;

		private SlotitemCategory[] Categories = new SlotitemCategory[]
		{
			SlotitemCategory.Syuhou,
			SlotitemCategory.Kanjouki,
			SlotitemCategory.Fukuhou,
			SlotitemCategory.Suijouki,
			SlotitemCategory.Kiju,
			SlotitemCategory.Dentan,
			SlotitemCategory.Gyorai,
			SlotitemCategory.Other
		};

		private ShipModel ship;

		private UIRemodelEquipSlotItem slotItem;

		private Vector3 showPos = new Vector3(235f, 0f, 0f);

		private Vector3 hidePos = new Vector3(1050f, 0f, 0f);

		private int _BeforeIdx;

		private bool isShown;

		private List<SlotitemCategory> selectableCategories
		{
			get
			{
				if (!this.slotItem.isExSlot)
				{
					return this.ship.GetEquipCategory();
				}
				List<SlotitemCategory> list = new List<SlotitemCategory>();
				list.Add(SlotitemCategory.Other);
				return list;
			}
		}

		public int group
		{
			get;
			private set;
		}

		private int currentRow
		{
			get
			{
				return (int)Math.Ceiling((double)((float)(this.currentIdx + 1) / 2f));
			}
		}

		private int maxRow
		{
			get
			{
				return (int)Math.Ceiling((double)((float)this.Categories.Length / 2f));
			}
		}

		private void Awake()
		{
			this.backAreaButton.SetActive(false);
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Init(KeyControl keyController, ShipModel ship, UIRemodelEquipSlotItem slotItem)
		{
			this.ship = ship;
			this.slotItem = slotItem;
			this.group = this.GetHashCode();
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.keyController = keyController;
			if (this.items == null)
			{
				this.items = new List<CategoryMenuItem>();
			}
			this.items.Clear();
			int index = 0;
			CategoryMenuItem[] componentsInChildren = this.itemContainer.get_transform().GetComponentsInChildren<CategoryMenuItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				CategoryMenuItem categoryMenuItem = componentsInChildren[i];
				categoryMenuItem.Init(this, index, this.selectableCategories.IndexOf(this.Categories[index++]) != -1);
				this.items.Add(categoryMenuItem);
			}
			for (int j = 0; j < this.items.get_Count(); j++)
			{
				if (this.changeCurrentItem(j))
				{
					this._BeforeIdx = j;
					break;
				}
			}
			bool flag = slotItem.GetModel() != null;
			if (flag)
			{
				this.removeItem.Init(this, -1, true);
			}
			this.removeItem.SetActive(flag);
		}

		private bool changeCurrentItem(int targetIdx)
		{
			if (targetIdx == -1)
			{
				this.currentIdx = -1;
				this.removeItem.Set(true);
				if (targetIdx != this._BeforeIdx)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this._BeforeIdx = targetIdx;
				}
				return true;
			}
			if (this.IsSelectable(targetIdx))
			{
				this.currentIdx = targetIdx;
				this.items.get_Item(this.currentIdx).Set(false);
				this.items.get_Item(this.currentIdx).Set(true);
				if (targetIdx != this._BeforeIdx)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this._BeforeIdx = targetIdx;
				}
				return true;
			}
			return false;
		}

		private void Update()
		{
			if (this.keyController != null && base.get_enabled() && this.isShown)
			{
				if (this.keyController.IsLeftDown())
				{
					int targetIdx = this.currentIdx - 1;
					if (this.currentIdx % 2 == 1)
					{
						this.changeCurrentItem(targetIdx);
					}
				}
				else if (this.keyController.IsRightDown())
				{
					int targetIdx2 = this.currentIdx + 1;
					if (this.currentIdx % 2 == 0)
					{
						this.changeCurrentItem(targetIdx2);
					}
				}
				else if (this.keyController.IsUpDown())
				{
					if (this.currentIdx == -1)
					{
						for (int i = this.Categories.Length - 1; i > 0; i--)
						{
							if (this.changeCurrentItem(i))
							{
								break;
							}
						}
					}
					else
					{
						for (int j = this.currentIdx - 2; j >= 0; j -= 2)
						{
							if (this.changeCurrentItem(j))
							{
								break;
							}
						}
					}
				}
				else if (this.keyController.IsDownDown())
				{
					if (this.currentIdx != -1)
					{
						bool flag = false;
						for (int k = this.currentIdx + 2; k < this.Categories.Length; k += 2)
						{
							if (flag = this.changeCurrentItem(k))
							{
								break;
							}
						}
						if (!flag && this.removeItem.get_gameObject().get_activeSelf())
						{
							this.changeCurrentItem(-1);
						}
					}
				}
				else if (this.keyController.IsMaruDown())
				{
					if (this.currentIdx == -1)
					{
						this.ProcessRemove();
					}
					else
					{
						this.Forward();
					}
				}
				else if (this.keyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		public void OnTouchBack()
		{
			this.Back();
		}

		private void Forward()
		{
			if (this.isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				this.Hide();
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouItemSelect(this.ship, this.Categories[this.currentIdx]);
			}
		}

		private void Back()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this.Hide();
				UserInterfaceRemodelManager.instance.Back2SoubiHenkou();
			}
		}

		public void Show()
		{
			base.get_gameObject().SetActive(true);
			this.backAreaButton.SetActive(true);
			base.set_enabled(true);
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
			{
				this.isShown = true;
			});
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			this.backAreaButton.SetActive(false);
			base.set_enabled(false);
			this.isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.3f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}

		public bool IsSelectable(int index)
		{
			return this.selectableCategories.IndexOf(this.Categories[index]) != -1;
		}

		public void OnItemClick(CategoryMenuItem child)
		{
			this.currentIdx = child.index;
			if (this.currentIdx == -1)
			{
				this.ProcessRemove();
			}
			else
			{
				this.Forward();
			}
		}

		public void ProcessRemove()
		{
			bool flag;
			if (!this.slotItem.isExSlot)
			{
				flag = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetSlotitem(this.ship.MemId, this.slotItem.index);
			}
			else
			{
				flag = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetSlotitemEx(this.ship.MemId);
			}
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_009);
				int memId = this.ship.MemId;
				int index = this.slotItem.index;
				SlotSetInfo slotitemInfoToUnset = UserInterfaceRemodelManager.instance.mRemodelManager.GetSlotitemInfoToUnset(memId, index);
				if (!this.slotItem.isExSlot)
				{
					UserInterfaceRemodelManager.instance.mRemodelManager.UnsetSlotitem(memId, index);
				}
				else
				{
					UserInterfaceRemodelManager.instance.mRemodelManager.UnsetSlotitemEx(memId);
				}
				UserInterfaceRemodelManager.instance.Back2SoubiHenkou();
				this.Hide();
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.backAreaButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.backAreaButton);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.backAreaButton);
			if (this.items != null)
			{
				this.items.Clear();
			}
			this.items = null;
			this.itemContainer = null;
			this.removeItem = null;
			this.keyController = null;
			this.Categories = null;
			this.ship = null;
			this.slotItem = null;
		}
	}
}
