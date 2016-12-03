using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIPanel)), SelectionBase]
	public class UIRemodelModernization : MonoBehaviour, UIRemodelView, IBannerResourceManage
	{
		private const float ANIMATION_DURATION = 0.2f;

		private UIPanel mPanelThis;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UIRemodeModernzationTargetShip[] mUIRemodeModernzationTargetShip_TargetShips;

		[SerializeField]
		private UIButton mButton_TouchBack;

		[SerializeField]
		private ButtonLightTexture btnLight;

		private Vector3 showPos = new Vector3(315f, 0f, 0f);

		private Vector3 hidePos = new Vector3(900f, 0f, 0f);

		private UIRemodeModernzationTargetShip mCurrentFocusTargetShipSlot;

		private UIRemodeModernzationTargetShip _BeforeTargetSlot;

		private KeyControl mKeyController;

		private ShipModel mModernzationShipModel;

		private int _CursorDown;

		private bool isShown;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			base.get_transform().set_localPosition(this.hidePos);
			this._BeforeTargetSlot = this.mUIRemodeModernzationTargetShip_TargetShips[0];
			this.mButton_TouchBack.SetActive(false);
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				uIRemodeModernzationTargetShip.SetOnUIRemodeModernzationTargetShipActionListener(new UIRemodeModernzationTargetShip.UIRemodeModernzationTargetShipAction(this.UIRemodeModernzationTargetShipAction));
			}
			this._CursorDown = 0;
		}

		public void Initialize(KeyControl keyController, ShipModel shipModel)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.mKeyController = keyController;
			this.mModernzationShipModel = shipModel;
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				uIRemodeModernzationTargetShip.UnSet();
			}
			this.UpdateStartButtonEnabled();
		}

		private void Update()
		{
			if (this.mKeyController != null && base.get_enabled() && this.isShown)
			{
				if (this.mKeyController.IsUpDown())
				{
					if (this.mCurrentFocusTargetShipSlot == null)
					{
						this.ChangeFocusSlot(this.mUIRemodeModernzationTargetShip_TargetShips[this.mUIRemodeModernzationTargetShip_TargetShips.Length - 1]);
						this.mButton_Start.SetState(UIButtonColor.State.Normal, true);
					}
					else
					{
						int num = Array.IndexOf<UIRemodeModernzationTargetShip>(this.mUIRemodeModernzationTargetShip_TargetShips, this.mCurrentFocusTargetShipSlot);
						int num2 = num - 1;
						if (0 <= num2)
						{
							this.ChangeFocusSlot(this.mUIRemodeModernzationTargetShip_TargetShips[num2]);
						}
					}
				}
				else if (this.mKeyController.IsDownDown() || this._CursorDown != 0)
				{
					do
					{
						if (!(this.mCurrentFocusTargetShipSlot == null))
						{
							int num3 = Array.IndexOf<UIRemodeModernzationTargetShip>(this.mUIRemodeModernzationTargetShip_TargetShips, this.mCurrentFocusTargetShipSlot);
							int num4 = num3 + 1;
							if (num4 < this.mUIRemodeModernzationTargetShip_TargetShips.Length)
							{
								this.ChangeFocusSlot(this.mUIRemodeModernzationTargetShip_TargetShips[num4], this._CursorDown != 0);
							}
							else if (num4 == this.mUIRemodeModernzationTargetShip_TargetShips.Length && this.CanModernize())
							{
								this.ChangeFocusSlot(null, this._CursorDown != 0);
								this.mButton_Start.SetState(UIButtonColor.State.Hover, true);
							}
						}
					}
					while (--this._CursorDown > 0);
					this._CursorDown = 0;
				}
				else if (this.mKeyController.IsMaruDown())
				{
					if (this.mCurrentFocusTargetShipSlot != null)
					{
						this.Forward4Select();
					}
					else
					{
						this.Forward4Confirm();
					}
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		private void Forward4Select()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishuSozaiSentaku(this.GetSetShipModels());
			}
		}

		private void Forward4Confirm()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Forward2KindaikaKaishuKakunin(this.GetModernizationShipModel(), this.GetSetShipModels());
			}
		}

		private void Back()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this.UnSetAll();
				this.RemoveFocus();
				UserInterfaceRemodelManager.instance.Back2ModeSelect();
			}
		}

		public void SwitchChildEnabled()
		{
			this.mButton_TouchBack.SetActive(base.get_enabled());
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				uIRemodeModernzationTargetShip.SetEnabled(base.get_enabled());
			}
		}

		public ShipModel GetModernizationShipModel()
		{
			return this.mModernzationShipModel;
		}

		public List<ShipModel> GetSetShipModels()
		{
			List<ShipModel> list = new List<ShipModel>();
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				if (uIRemodeModernzationTargetShip.GetSlotInShip() != null)
				{
					list.Add(uIRemodeModernzationTargetShip.GetSlotInShip());
				}
			}
			return list;
		}

		public void UnSetAll()
		{
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				uIRemodeModernzationTargetShip.UnSet();
			}
			this.mButton_Start.SetState(UIButtonColor.State.Normal, true);
			this.UpdateStartButtonEnabled();
		}

		private void UIRemodeModernzationTargetShipAction(UIRemodeModernzationTargetShip.ActionType actionType, UIRemodeModernzationTargetShip calledObject)
		{
			if (this.mKeyController == null)
			{
				return;
			}
			if (actionType == UIRemodeModernzationTargetShip.ActionType.OnTouch)
			{
				this.ChangeFocusSlot(calledObject);
				this.Forward4Select();
			}
		}

		private void ChangeFocusSlot(UIRemodeModernzationTargetShip target)
		{
			this.ChangeFocusSlot(target, false);
		}

		private void ChangeFocusSlot(UIRemodeModernzationTargetShip target, bool mute)
		{
			if (this.mCurrentFocusTargetShipSlot != null)
			{
				this.mCurrentFocusTargetShipSlot.RemoveHover();
			}
			this.mCurrentFocusTargetShipSlot = target;
			if (this.mCurrentFocusTargetShipSlot != null)
			{
				this.mCurrentFocusTargetShipSlot.Hover();
			}
			if (target != this._BeforeTargetSlot)
			{
				if (!mute)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this._BeforeTargetSlot = target;
			}
		}

		public UIRemodeModernzationTargetShip GetFocusSlot()
		{
			return this.mCurrentFocusTargetShipSlot;
		}

		public void Hide(bool animation)
		{
			base.set_enabled(false);
			this.SwitchChildEnabled();
			this._BeforeTargetSlot = this.mUIRemodeModernzationTargetShip_TargetShips[0];
			this.isShown = false;
			if (animation)
			{
				if (UserInterfaceRemodelManager.instance.status != ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
				{
					RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
					{
						if (!this.isShown)
						{
							base.get_gameObject().SetActive(false);
						}
					});
				}
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				uIRemodeModernzationTargetShip.Refresh();
			}
			base.get_gameObject().SetActive(true);
			base.set_enabled(true);
			this.SwitchChildEnabled();
			this.isShown = true;
			if (animation)
			{
				this.mPanelThis.widgetsAreStatic = true;
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, delegate
				{
					this.mPanelThis.widgetsAreStatic = false;
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.showPos);
			}
			if (this.mCurrentFocusTargetShipSlot != null)
			{
				this.mCurrentFocusTargetShipSlot.Hover();
			}
			this.UpdateStartButtonEnabled();
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void OnTouchStart()
		{
			if (UserInterfaceRemodelManager.instance.status == ScreenStatus.MODE_KINDAIKA_KAISHU)
			{
				this.Forward4Confirm();
			}
		}

		public void SetCurrentFocusToShip(ShipModel shipModel)
		{
			this.mCurrentFocusTargetShipSlot.Initialize(shipModel);
			this.UpdateStartButtonEnabled();
		}

		public void RefreshList()
		{
			List<ShipModel> setShipModels = this.GetSetShipModels();
			this.UnSetAll();
			int cursorDown = 0;
			using (List<ShipModel>.Enumerator enumerator = setShipModels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel current = enumerator.get_Current();
					this.mUIRemodeModernzationTargetShip_TargetShips[cursorDown++].Initialize(current);
				}
			}
			this._CursorDown = cursorDown;
			this.UpdateStartButtonEnabled();
		}

		private void UpdateStartButtonEnabled()
		{
			this.mButton_Start.set_enabled(this.GetSetShipModels().get_Count() > 0 && this.CanModernize());
			if (this.mButton_Start.get_enabled())
			{
				this.btnLight.PlayAnim();
			}
			else
			{
				this.btnLight.StopAnim();
			}
			this.mButton_Start.SetState((!this.mButton_Start.get_enabled()) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}

		public void InitFocus()
		{
			if (this.mUIRemodeModernzationTargetShip_TargetShips.Length > 0)
			{
				this.ChangeFocusSlot(this.mUIRemodeModernzationTargetShip_TargetShips[0]);
			}
		}

		public void RemoveFocus()
		{
			this.ChangeFocusSlot(null, true);
		}

		public void OnTouchBackArea()
		{
			this.Back();
		}

		private bool CanModernize()
		{
			return UserInterfaceRemodelManager.instance.mRemodelManager.IsValidPowerUp(this.GetSetShipModels());
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Start);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_TouchBack);
			if (this.mUIRemodeModernzationTargetShip_TargetShips != null)
			{
				for (int i = 0; i < this.mUIRemodeModernzationTargetShip_TargetShips.Length; i++)
				{
					this.mUIRemodeModernzationTargetShip_TargetShips[i] = null;
				}
			}
			this.mUIRemodeModernzationTargetShip_TargetShips = null;
			this.btnLight = null;
			this.mCurrentFocusTargetShipSlot = null;
			this._BeforeTargetSlot = null;
			this.mKeyController = null;
			this.mModernzationShipModel = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodeModernzationTargetShip[] array = this.mUIRemodeModernzationTargetShip_TargetShips;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodeModernzationTargetShip uIRemodeModernzationTargetShip = array[i];
				CommonShipBanner banner = uIRemodeModernzationTargetShip.GetBanner();
				list.Add(banner);
			}
			return list.ToArray();
		}
	}
}
