using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelEquipSlotItems : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UIButton touchBackArea;

		[SerializeField]
		private UIRemodelEquipSlotItem[] slots;

		[SerializeField]
		private GameObject parameters;

		[SerializeField]
		private CommonDialog ExSlotDialog;

		[SerializeField]
		private YesNoButton YesNoButton;

		[SerializeField]
		private UILabel ExSlotItemNum;

		private Vector3 showPos = new Vector3(300f, 0f);

		private Vector3 hidePos = new Vector3(780f, 0f);

		private ShipModel ship;

		private KeyControl keyController;

		private bool validShip;

		private bool validUnsetAll;

		private UIRemodelEquipSlotItem _BeforeItem;

		private bool isShown;

		public UIRemodelEquipSlotItem currentFocusItem
		{
			get;
			private set;
		}

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
			this.touchBackArea.SetActive(false);
			this.ExSlotDialog.SetActive(false);
		}

		public void Initialize(KeyControl keyController, ShipModel shipModel)
		{
			keyController.ClearKeyAll();
			keyController.firstUpdate = true;
			this.keyController = keyController;
			base.set_enabled(true);
			this.ship = shipModel;
			this.validShip = UserInterfaceRemodelManager.instance.IsValidShip();
			this.validUnsetAll = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidUnsetAll(UserInterfaceRemodelManager.instance.focusedShipModel.MemId);
			UIRemodelEquipSlotItem[] array = this.slots;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodelEquipSlotItem uIRemodelEquipSlotItem = array[i];
				if (this.validShip)
				{
					uIRemodelEquipSlotItem.SetOnUIRemodelEquipSlotItemActionListener(new UIRemodelEquipSlotItem.UIRemodelEquipSlotItemAction(this.UIRemodelEquipSlotItemAction));
				}
				else
				{
					uIRemodelEquipSlotItem.SetOnUIRemodelEquipSlotItemActionListener(null);
				}
				uIRemodelEquipSlotItem.Hide();
			}
			this.InitSlotItems(this.ship);
			ParameterType[] array2 = new ParameterType[]
			{
				ParameterType.Taikyu,
				ParameterType.Karyoku,
				ParameterType.Soukou,
				ParameterType.Raisou,
				ParameterType.Kaihi,
				ParameterType.Taiku,
				ParameterType.Tous,
				ParameterType.Taisen,
				ParameterType.Soku,
				ParameterType.Sakuteki,
				ParameterType.Leng,
				ParameterType.Lucky
			};
			int[] array3 = new int[]
			{
				shipModel.MaxHp,
				shipModel.Karyoku,
				shipModel.Soukou,
				shipModel.Raisou,
				shipModel.Kaihi,
				shipModel.Taiku,
				shipModel.TousaiMaxAll,
				shipModel.Taisen,
				shipModel.Soku,
				shipModel.Sakuteki,
				shipModel.Leng,
				shipModel.Lucky
			};
			for (int j = 0; j < array2.Length; j++)
			{
				UIRemodelParameter component = this.parameters.get_transform().GetChild(j).GetComponent<UIRemodelParameter>();
				component.Initialize(array2[j], array3[j]);
			}
			this.currentFocusItem = null;
			this._BeforeItem = this.slots[0];
			this.Focus();
		}

		private void InitSlotItems(ShipModel shipModel)
		{
			int i;
			for (i = 0; i < this.ship.SlotitemList.get_Count(); i++)
			{
				this.slots[i].Initialize(i, shipModel);
				this.slots[i].Show();
			}
			if (this.ship.HasExSlot())
			{
				this.slots[i].Initialize(i, this.ship.SlotitemEx, shipModel);
				this.slots[i].Show();
			}
			else if (this.ship.Level >= 30)
			{
				bool isEnable = UserInterfaceRemodelManager.instance.mRemodelManager.HokyoZousetsuNum > 0;
				this.slots[i].InitExSlotButton(isEnable);
				this.slots[i].Show();
			}
		}

		private void UIRemodelEquipSlotItemAction(UIRemodelEquipSlotItem.ActionType actionType, UIRemodelEquipSlotItem actionObject)
		{
			if (actionType == UIRemodelEquipSlotItem.ActionType.OnTouch)
			{
				if (actionObject == null)
				{
					Debug.Log("actionObject is null");
				}
				this.ChangeFocusItem(actionObject);
				this.forward();
			}
		}

		private void Focus()
		{
			if (!this.validShip)
			{
				return;
			}
			if (this.currentFocusItem == null)
			{
				this.FocusFirstItem();
			}
			else
			{
				this.currentFocusItem.Hover();
			}
		}

		public void OnTouchBackArea()
		{
			this.back();
		}

		private void forward()
		{
			if (this.isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				if (this.currentFocusItem.isExSlot && !this.ship.HasExSlot())
				{
					if (this.currentFocusItem.ExSlotButton.state != UIButtonColor.State.Disabled)
					{
						this.OpenExSlotDialog();
						return;
					}
					CommonPopupDialog.Instance.StartPopup("アイテム「補強増設」を持っていません");
					return;
				}
				else
				{
					UserInterfaceRemodelManager.instance.Forward2SoubiHenkouTypeSelect();
				}
			}
		}

		private void back()
		{
			if (this.isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Back2ModeSelect();
			}
		}

		private void UnsetAll()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.UnsetAll(this.ship.MemId);
			this.Initialize(this.keyController, this.ship);
		}

		private void ChangeFocusItem(UIRemodelEquipSlotItem target)
		{
			if (this.currentFocusItem != null)
			{
				this.currentFocusItem.RemoveHover();
			}
			this.currentFocusItem = target;
			if (this.currentFocusItem != null)
			{
				this.currentFocusItem.Hover();
			}
			if (target != this._BeforeItem)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCursolMove);
				this._BeforeItem = target;
			}
		}

		public void ChangeFocusItemFromModel(SlotitemModel slotItemModel)
		{
			bool flag = slotItemModel != null;
			if (flag)
			{
				for (int i = 0; i < this.slots.Length; i++)
				{
					if (this.slots[i].GetModel() != null && this.slots[i].GetModel().MemId == slotItemModel.MemId)
					{
						this.ChangeFocusItem(this.slots[i]);
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.slots.Length; j++)
				{
					if (this.slots[j].GetModel() == null)
					{
						this.ChangeFocusItem(this.slots[j]);
						break;
					}
				}
			}
		}

		private void FocusFirstItem()
		{
			if (this.slots.Length > 0)
			{
				this.ChangeFocusItem(this.slots[0]);
			}
		}

		public UIRemodelEquipSlotItem GetCurrentItem()
		{
			return this.currentFocusItem;
		}

		public int GetCurrentSlotIndex()
		{
			return Array.IndexOf<UIRemodelEquipSlotItem>(this.slots, this.currentFocusItem);
		}

		private void Update()
		{
			if (this.keyController != null && base.get_enabled() && this.isShown)
			{
				if (this.validShip && this.keyController.IsUpDown())
				{
					int num = Array.IndexOf<UIRemodelEquipSlotItem>(this.slots, this.currentFocusItem);
					int num2 = num - 1;
					if (0 <= num2)
					{
						Debug.Log("PrevIndex::" + num2);
						this.ChangeFocusItem(this.slots[num2]);
					}
				}
				else if (this.validShip && this.keyController.IsDownDown())
				{
					int num3 = Array.IndexOf<UIRemodelEquipSlotItem>(this.slots, this.currentFocusItem);
					int num4 = num3 + 1;
					int num5 = (this.ship.Level < 30) ? 0 : 1;
					if (num4 < this.ship.SlotitemList.get_Count() + num5)
					{
						Debug.Log("NextIndex::" + num4);
						this.ChangeFocusItem(this.slots[num4]);
					}
				}
				else if (this.validShip && this.keyController.IsMaruDown())
				{
					this.forward();
				}
				else if (this.keyController.IsBatuDown())
				{
					this.back();
				}
				else if (this.validUnsetAll && this.keyController.IsShikakuDown())
				{
					this.UnsetAll();
					SoundUtils.PlayOneShotSE(SEFIleInfos.SE_009);
				}
			}
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			base.get_gameObject().SetActive(true);
			base.set_enabled(true);
			this.Focus();
			this.touchBackArea.SetActive(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
				{
					this.isShown = true;
				});
			}
			else
			{
				this.isShown = true;
				base.get_transform().set_localPosition(this.showPos);
			}
			UserInterfaceRemodelManager.instance.UpdateHeaderMaterial();
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			this.isShown = false;
			base.set_enabled(false);
			this.touchBackArea.SetActive(false);
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

		private void OpenExSlotDialog()
		{
			this.ExSlotDialog.SetActive(true);
			this.keyController.IsRun = false;
			this.keyController.ClearKeyAll();
			int hokyoZousetsuNum = UserInterfaceRemodelManager.instance.mRemodelManager.HokyoZousetsuNum;
			this.ExSlotDialog.isUseDefaultKeyController = false;
			this.ExSlotItemNum.text = hokyoZousetsuNum + "\u3000→\u3000" + (hokyoZousetsuNum - 1);
			this.ExSlotDialog.OpenDialog(0, DialogAnimation.AnimType.POPUP);
			this.ExSlotDialog.setCloseAction(delegate
			{
				this.keyController.IsRun = true;
			});
			this.YesNoButton.SetOnSelectPositiveListener(new Action(this.OpenExSlot));
			this.YesNoButton.SetOnSelectNegativeListener(new Action(this.CloseExSlotDialog));
			this.YesNoButton.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f), true);
		}

		private void CloseExSlotDialog()
		{
			this.ExSlotDialog.CloseDialog();
		}

		private void OpenExSlot()
		{
			UserInterfaceRemodelManager.instance.mRemodelManager.OpenSlotEx(this.ship.MemId);
			this.InitSlotItems(this.ship);
			this.currentFocusItem.Hover();
			this.CloseExSlotDialog();
			base.StartCoroutine(this.PlayOpenAnimation());
		}

		[DebuggerHidden]
		private IEnumerator PlayOpenAnimation()
		{
			UIRemodelEquipSlotItems.<PlayOpenAnimation>c__IteratorB2 <PlayOpenAnimation>c__IteratorB = new UIRemodelEquipSlotItems.<PlayOpenAnimation>c__IteratorB2();
			<PlayOpenAnimation>c__IteratorB.<>f__this = this;
			return <PlayOpenAnimation>c__IteratorB;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.touchBackArea);
			if (this.slots != null)
			{
				for (int i = 0; i < this.slots.Length; i++)
				{
					this.slots[i] = null;
				}
			}
			this.slots = null;
			this.parameters = null;
			this.currentFocusItem = null;
			this.ship = null;
			this.keyController = null;
			this._BeforeItem = null;
			this.ExSlotDialog = null;
		}
	}
}
