using KCV.PopupString;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelModeSelectMenu : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.3f;

		[SerializeField]
		private UIButton mButton_SlotItemChange;

		[SerializeField]
		private UIButton mButton_Modernization;

		[SerializeField]
		private UIButton mButton_Remodel;

		private UIButton[] mButtonsFocusable;

		private UIButton mCurrentButtonFocus;

		private Vector3 showPos = new Vector3(240f, 0f);

		private Vector3 hidePos = new Vector3(960f, 0f);

		private bool firstUpdateWhenShow = true;

		private KeyControl keyController;

		private bool validShip;

		private string validShipReason;

		private ShipModel ship;

		private UIButton _BeforeButton;

		private bool isShown;

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
			this.mButton_Modernization.OnEnableAndOnDisableChangeState = true;
			this.mButton_Remodel.OnEnableAndOnDisableChangeState = true;
			this.mButton_SlotItemChange.OnEnableAndOnDisableChangeState = true;
		}

		public void Init(KeyControl keyController, bool remodelable)
		{
			this.ChangeFocusButton(null);
			this.keyController = keyController;
			this.ship = UserInterfaceRemodelManager.instance.focusedShipModel;
			this.validShip = UserInterfaceRemodelManager.instance.IsValidShip();
			bool flag = UserInterfaceRemodelManager.instance.mRemodelManager.IsValidGradeUp(this.ship);
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_SlotItemChange);
			this.InitButton(list, this.mButton_SlotItemChange, true);
			this.InitButton(list, this.mButton_Modernization, this.validShip);
			this.InitButton(list, this.mButton_Remodel, this.validShip && flag);
			this._BeforeButton = this.mButton_SlotItemChange;
			this.mButtonsFocusable = list.ToArray();
			this.ChangeFocusButton(this.mButton_SlotItemChange);
		}

		private void InitButton(List<UIButton> list, UIButton button, bool enabled)
		{
			button.hover = Color.get_white();
			button.defaultColor = Color.get_white();
			button.pressed = Color.get_white();
			button.disabledColor = Color.get_white();
			button.set_enabled(enabled);
			if (enabled)
			{
				list.Add(button);
				button.SetState(UIButtonColor.State.Normal, true);
			}
			else
			{
				button.SetState(UIButtonColor.State.Disabled, true);
			}
		}

		public void Show()
		{
			base.get_gameObject().SetActive(true);
			if (this.keyController != null)
			{
				this.keyController.ClearKeyAll();
				this.keyController.firstUpdate = true;
			}
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
			{
				this.isShown = true;
			});
			if (this.mCurrentButtonFocus != null)
			{
				this.ChangeFocusButton(this.mCurrentButtonFocus);
			}
			else
			{
				this.ChangeFocusButton(this.mButtonsFocusable[0]);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
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

		private void SwitchButton(int changeIndex)
		{
			int num = Array.IndexOf<UIButton>(this.mButtonsFocusable, this.mCurrentButtonFocus);
			int num2 = num + changeIndex;
			if (0 <= num2 && num2 < this.mButtonsFocusable.Length)
			{
				this.ChangeFocusButton(this.mButtonsFocusable[num2]);
			}
		}

		private void Update()
		{
			if (this.keyController != null && this.isShown)
			{
				if (this.keyController.IsLeftDown())
				{
					this.ChangeFocusButton(this.mButton_SlotItemChange);
				}
				else if (this.keyController.IsRightDown())
				{
					if (this.mButton_Modernization.get_enabled())
					{
						this.ChangeFocusButton(this.mButton_Modernization);
					}
				}
				else if (this.keyController.IsUpDown() && this._BeforeButton != this.mButton_Modernization)
				{
					this.ChangeFocusButton(this.mButton_SlotItemChange);
				}
				else if (this.keyController.IsDownDown())
				{
					if (this.mButton_Remodel.get_enabled())
					{
						this.ChangeFocusButton(this.mButton_Remodel);
					}
				}
				else if (this.keyController.IsMaruDown())
				{
					if (this.mCurrentButtonFocus.Equals(this.mButton_SlotItemChange))
					{
						this.OnClickSlotItemChange();
					}
					else if (this.mCurrentButtonFocus.Equals(this.mButton_Modernization))
					{
						this.OnClickModernization();
					}
					else if (this.mCurrentButtonFocus.Equals(this.mButton_Remodel))
					{
						this.OnClickRemodel();
					}
				}
				else if (this.keyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchBack()
		{
			if (this.isShown)
			{
				this.Back();
			}
		}

		private void Back()
		{
			if (this.isShown)
			{
				UserInterfaceRemodelManager.instance.Back2ShipSelect();
			}
		}

		public bool IsValidSlotItemChange()
		{
			bool flag = !this.validShip || (this.ship.SlotitemList.get_Count() == 0 && !this.ship.HasExSlot() && this.ship.Level < 30);
			return !flag;
		}

		public void PopUpFailOpenSummary()
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCancel1);
			if (this.ship.IsInActionEndDeck())
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.ActionEndShip);
			}
			else if (this.ship.SlotitemList.get_Count() == 0)
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.NoSlot);
			}
			else if (this.ship.IsInRepair())
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.NowRepairing);
			}
			else if (this.ship.IsBling())
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.NowBlinging);
			}
			else if (this.ship.IsInEscortDeck() != -1)
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.InEscortShip);
			}
			else if (this.ship.IsInMission())
			{
				this.validShipReason = Util.getPopupMessage(PopupMess.InMissionShip);
			}
			else
			{
				this.validShipReason = string.Empty;
			}
			CommonPopupDialog.Instance.StartPopup(this.validShipReason);
		}

		public void OnClickSlotItemChange()
		{
			if (this.isShown)
			{
				if (!this.IsValidSlotItemChange())
				{
					this.PopUpFailOpenSummary();
					return;
				}
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				this.ChangeFocusButton(this.mButton_SlotItemChange);
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkou(null, false);
			}
		}

		public void OnClickModernization()
		{
			if (this.isShown)
			{
				this.ChangeFocusButton(this.mButton_Modernization);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishu();
			}
		}

		public void OnClickRemodel()
		{
			if (this.isShown)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				this.ChangeFocusButton(this.mButton_Remodel);
				UserInterfaceRemodelManager.instance.Forward2Kaizo();
			}
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (this.mCurrentButtonFocus != null)
			{
				this.mCurrentButtonFocus.SetState(UIButtonColor.State.Normal, true);
			}
			this.mCurrentButtonFocus = target;
			if (this.mCurrentButtonFocus != null)
			{
				this.mCurrentButtonFocus.SetState(UIButtonColor.State.Hover, true);
			}
			if (this.mCurrentButtonFocus != this._BeforeButton)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCursolMove);
				this._BeforeButton = this.mCurrentButtonFocus;
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mButtonsFocusable);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_SlotItemChange);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Modernization);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Remodel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mCurrentButtonFocus);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._BeforeButton);
			this.keyController = null;
			this.ship = null;
		}
	}
}
