using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Supply
{
	public abstract class UISupplyCommonShipBanner : MonoBehaviour
	{
		[SerializeField]
		private UILabel _shipName;

		[SerializeField]
		private UILabel _shipLevel;

		[SerializeField]
		private UILabel _shipTaikyu;

		[SerializeField]
		private UISprite _shipGauge;

		[SerializeField]
		private UISprite _shipGaugeFrame;

		[SerializeField]
		private UISprite _checkBox;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UITexture _backGround;

		[SerializeField]
		private SupplyStorage _ammoStorage;

		[SerializeField]
		private SupplyStorage _fuelStorage;

		[SerializeField]
		private UITexture _waveAnime;

		public int idx;

		public bool selected;

		public ShipModel Ship;

		[Button("ProcessRecoveryAnimation", "ProcessRecoveryAnimation", new object[]
		{

		})]
		public int button1;

		public iTween.EaseType type;

		public bool Init()
		{
			this.selected = false;
			return true;
		}

		public virtual void SetBanner(ShipModel ship, int idx)
		{
			this.SetEnabled(ship != null);
			this.Ship = ship;
			this.idx = idx;
			this.selected = false;
			if (base.get_enabled())
			{
				this.UpdateCheckBoxBackground();
				this.UpdateCheckBox();
				this._checkBox.SetActive(true);
				this._check.SetActive(false);
				this._shipName.text = this.Ship.Name;
				this._shipLevel.text = "Lv" + this.Ship.Level;
				this._shipTaikyu.text = this.Ship.NowHp + "/" + this.Ship.MaxHp;
				float num = (float)this.Ship.NowHp / (float)this.Ship.MaxHp;
				float num2 = (float)this._shipGaugeFrame.width * num;
				this._shipGauge.width = (int)num2;
				this._shipGauge.alpha = 1f;
				this._shipGaugeFrame.alpha = 1f;
				this._shipGauge.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
				int num3 = 0;
				for (int i = 0; i < 10; i++)
				{
					if (ship.Ammo <= ship.AmmoMax / 5 * i)
					{
						break;
					}
					num3++;
				}
				this._ammoStorage.init(num3, 0.2f);
				int num4 = 0;
				for (int j = 0; j < 5; j++)
				{
					if (ship.Fuel <= ship.FuelMax / 5 * j)
					{
						break;
					}
					num4++;
				}
				this._fuelStorage.init(num4, 0.2f);
				this.UpdateCheckBoxBackground();
			}
		}

		public void BannerAnimation(bool isShow)
		{
			Animation component = base.GetComponent<Animation>();
			component.Stop();
			if (isShow)
			{
				component.get_Item("SupplyBanner").set_time(0f);
				component.Play("SupplyBanner");
			}
			else
			{
				component.get_Item("SupplyShutterIn").set_time(0f);
				component.Play("SupplyShutterIn");
			}
		}

		public virtual void SetEnabled(bool enabled)
		{
			base.set_enabled(enabled);
			this._checkBox.SetActive(enabled);
			this._check.SetActive(enabled);
			this._shipName.SetActive(enabled);
			this._shipLevel.SetActive(enabled);
			this._shipTaikyu.SetActive(enabled);
			this._shipGauge.SetActive(enabled);
			this._shipGaugeFrame.SetActive(enabled);
			this._fuelStorage.SetActive(enabled);
			this._ammoStorage.SetActive(enabled);
			if (!enabled)
			{
				this._fuelStorage.init(0, 0.5f);
				this._ammoStorage.init(0, 0.5f);
			}
		}

		public void Hover(bool enabled)
		{
			UISelectedObject.SelectedOneObjectBlink(this._backGround.get_gameObject(), enabled);
		}

		private void UpdateCheckBoxBackground()
		{
			this._checkBox.spriteName = ((!this.IsSelectable()) ? "check_bg" : "check_bg_on");
		}

		private void UpdateCheckBox()
		{
			this._check.get_transform().SetActive(this.selected);
		}

		public void Select(bool selected)
		{
			this.selected = selected;
			this.UpdateCheckBox();
		}

		public bool SwitchSelected()
		{
			this.selected = !this.selected;
			this.UpdateCheckBox();
			SupplyMainManager.Instance.SupplyManager.ClickCheckBox(this.Ship.MemId);
			return this.selected;
		}

		public void ProcessRecoveryAnimation()
		{
			this._waveAnime.get_transform().set_localPosition(new Vector3(-360f, 4f, 0f));
			this._waveAnime.alpha = 1f;
			iTween.MoveTo(this._waveAnime.get_gameObject(), iTween.Hash(new object[]
			{
				"position",
				new Vector3(250f, 0f, 0f),
				"islocal",
				true,
				"time",
				0.39f,
				"easetype",
				this.type,
				"looptype",
				iTween.LoopType.none,
				"oncomplete",
				"OnCompleteOfRecoveryAnimation",
				"oncompletetarget",
				base.get_gameObject()
			}));
			TweenAlpha.Begin(this._waveAnime.get_gameObject(), 0.39f, 0.5f);
		}

		public void OnCompleteOfRecoveryAnimation()
		{
			iTween.Stop(this._waveAnime.get_gameObject());
			this._waveAnime.alpha = 0f;
		}

		public bool IsSelectable()
		{
			if (SupplyMainManager.Instance.SupplyManager.CheckBoxStates == null)
			{
				DebugUtils.SLog("SupplyManager.CheckBoxStates == null occured!!!!");
				return false;
			}
			return base.get_enabled() && SupplyMainManager.Instance.SupplyManager.CheckBoxStates[this.idx] != CheckBoxStatus.DISABLE;
		}

		private bool IsAmmoNotFull()
		{
			return this.Ship.Ammo != this.Ship.AmmoMax;
		}

		private bool IsFuelNotFull()
		{
			return this.Ship.Fuel != this.Ship.FuelMax;
		}
	}
}
