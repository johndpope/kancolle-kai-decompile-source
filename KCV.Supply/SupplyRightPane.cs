using Common.Enum;
using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Supply
{
	public class SupplyRightPane : MonoBehaviour
	{
		[SerializeField]
		private UILabel _fuelLabel;

		[SerializeField]
		private UILabel _ammoLabel;

		[SerializeField]
		private UIButton _fuelBtn;

		[SerializeField]
		private UIButton _ammoBtn;

		[SerializeField]
		private UIButton _allBtn;

		[SerializeField]
		private UITexture _window;

		private UIButton _currentBtn;

		[SerializeField]
		private UISupplyFuelIconManager _fuelSupplyIconManager;

		[SerializeField]
		private UISupplyAmmoIconManager _ammoSupplyIconManager;

		[SerializeField]
		private ButtonLightTexture[] btnLight = new ButtonLightTexture[3];

		public void Init()
		{
			this._fuelBtn.set_enabled(false);
			this._ammoBtn.set_enabled(false);
			this._allBtn.set_enabled(false);
			this._currentBtn = null;
			this.UpdateBtns();
		}

		public void SelectButtonLengthwise(bool isUp)
		{
			if (isUp)
			{
				if (this._fuelBtn.get_enabled() && this._currentBtn == this._allBtn)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetCurrentBtn(this._fuelBtn);
				}
				else if (this._ammoBtn.get_enabled() && this._currentBtn == this._allBtn)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.SetCurrentBtn(this._ammoBtn);
				}
			}
			else if (this._currentBtn != this._allBtn)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (this._allBtn.get_enabled())
				{
					this.SetCurrentBtn(this._allBtn);
				}
			}
		}

		public bool SelectButtonHorizontal(bool isLeft)
		{
			if (isLeft)
			{
				if (this._currentBtn == this._ammoBtn && this._fuelBtn.get_enabled())
				{
					this.SetCurrentBtn(this._fuelBtn);
				}
				else
				{
					SupplyMainManager.Instance.change_2_SHIP_SELECT(true);
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else
			{
				if (!(this._currentBtn == this._fuelBtn) || !this._ammoBtn.get_enabled())
				{
					return true;
				}
				this.SetCurrentBtn(this._ammoBtn);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			return false;
		}

		public void DisideButton()
		{
			if (this._currentBtn == this._fuelBtn)
			{
				this.DoSupplyFuel();
			}
			else if (this._currentBtn == this._ammoBtn)
			{
				this.DoSupplyAmmo();
			}
			else if (this._currentBtn == this._allBtn)
			{
				this.DoSupplyAll();
			}
		}

		public void Refresh()
		{
			bool flag = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Fuel);
			bool flag2 = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Ammo);
			this._fuelBtn.set_enabled(flag);
			this._ammoBtn.set_enabled(flag2);
			this._allBtn.set_enabled(SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.All));
			this.UpdateBtns();
			int fuelForSupply = SupplyMainManager.Instance.SupplyManager.FuelForSupply;
			int ammoForSupply = SupplyMainManager.Instance.SupplyManager.AmmoForSupply;
			this._fuelLabel.textInt = fuelForSupply;
			this._ammoLabel.textInt = ammoForSupply;
			Color color = new Color(1f, 1f, 1f);
			Color color2 = new Color(1f, 0f, 0f);
			this._fuelLabel.color = ((fuelForSupply != 0 && !flag) ? color2 : color);
			this._ammoLabel.color = ((ammoForSupply != 0 && !flag2) ? color2 : color);
			this._fuelSupplyIconManager.init(fuelForSupply);
			this._ammoSupplyIconManager.init(ammoForSupply);
		}

		public void OnFuelBtnClick()
		{
			this.UpdateBtns();
			this.SetCurrentBtn(this._fuelBtn);
			this.DoSupplyFuel();
		}

		public void OnAmmoBtnClick()
		{
			this.UpdateBtns();
			this.SetCurrentBtn(this._ammoBtn);
			this.DoSupplyAmmo();
		}

		public void OnAllBtnClick()
		{
			this.UpdateBtns();
			this.SetCurrentBtn(this._allBtn);
			this.DoSupplyAll();
		}

		private void DoSupplyFuel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this._ammoLabel.text = "0";
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.Fuel);
		}

		private void DoSupplyAmmo()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this._fuelLabel.text = "0";
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.Ammo);
		}

		private void DoSupplyAll()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			Debug.Log("まとめて補給ボタン押下");
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.All);
		}

		public void setFocus()
		{
			if (this._allBtn.get_enabled())
			{
				this.SetCurrentBtn(this._allBtn);
			}
			else if (this._fuelBtn.get_enabled())
			{
				this.SetCurrentBtn(this._fuelBtn);
			}
			else if (this._ammoBtn.get_enabled())
			{
				this.SetCurrentBtn(this._ammoBtn);
			}
		}

		public void LostFocus()
		{
			SupplyMainManager.Instance.SetControllDone(false);
			this._currentBtn = null;
			this.UpdateBtns();
		}

		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this._fuelBtn.set_enabled(SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Fuel));
				this._ammoBtn.set_enabled(SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Ammo));
				this._allBtn.set_enabled(SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.All));
			}
			else
			{
				this._fuelBtn.set_enabled(false);
				this._ammoBtn.set_enabled(false);
				this._allBtn.set_enabled(false);
			}
			this.UpdateBtns();
		}

		public bool IsFocusable()
		{
			return this._allBtn.get_enabled() || this._fuelBtn.get_enabled() || this._ammoBtn.get_enabled();
		}

		private void SetCurrentBtn(UIButton btn)
		{
			this.UpdateBtns();
			this._currentBtn = btn;
			this._currentBtn.SetState(UIButtonColor.State.Hover, true);
		}

		private void UpdateBtns()
		{
			this._allBtn.SetState((!this._allBtn.get_enabled()) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
			this._fuelBtn.SetState((!this._fuelBtn.get_enabled()) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
			this._ammoBtn.SetState((!this._ammoBtn.get_enabled()) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
			if (this._allBtn.get_enabled())
			{
				this.btnLight[0].PlayAnim();
			}
			else
			{
				this.btnLight[0].StopAnim();
			}
			if (this._fuelBtn.get_enabled())
			{
				this.btnLight[1].PlayAnim();
			}
			else
			{
				this.btnLight[1].StopAnim();
			}
			if (this._ammoBtn.get_enabled())
			{
				this.btnLight[2].PlayAnim();
			}
			else
			{
				this.btnLight[2].StopAnim();
			}
		}

		public void DoWindowOpenAnimation(SupplyType supplyType)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.3f);
			hashtable.Add("y", 199f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "OnCompleteOfWindowOpenAnimation");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			hashtable.Add("oncompleteparams", supplyType);
			this._window.get_transform().MoveTo(hashtable);
		}

		private void OnCompleteOfWindowOpenAnimation(SupplyType supplyType)
		{
			switch (supplyType)
			{
			case SupplyType.All:
				this._ammoSupplyIconManager.ProcessConsumingAnimation();
				this._fuelSupplyIconManager.ProcessConsumingAnimation();
				break;
			case SupplyType.Fuel:
				this._fuelSupplyIconManager.ProcessConsumingAnimation();
				this._ammoSupplyIconManager.ProcessCancelAnimation();
				break;
			case SupplyType.Ammo:
				this._fuelSupplyIconManager.ProcessCancelAnimation();
				this._ammoSupplyIconManager.ProcessConsumingAnimation();
				break;
			}
		}

		public void DoWindowCloseAnimation()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.6f);
			hashtable.Add("y", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "OnCompleteOfRecoveryAnimation");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._window.get_transform().MoveTo(hashtable);
		}

		public void OnCompleteOfRecoveryAnimation()
		{
			SupplyMainManager.Instance.ProcessSupplyFinished();
		}
	}
}
