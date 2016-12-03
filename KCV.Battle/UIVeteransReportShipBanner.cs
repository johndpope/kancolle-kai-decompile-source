using KCV.Battle.Utils;
using KCV.Generic;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class UIVeteransReportShipBanner : BaseUISortieBattleShipBanner<ShipModel_BattleResult>
	{
		public enum BannerMode
		{
			HP,
			EXP
		}

		[SerializeField]
		private UILabel _uiLv;

		[SerializeField]
		private UILabel _uiLvLabel;

		[SerializeField]
		private UISlider _uiEXPSlider;

		[SerializeField]
		private UIWidget _uiShipInfos;

		[SerializeField]
		private UILevelUpIcon _uiLevelUpIcon;

		[SerializeField]
		private UIMVPIcon _uiMVPIcon;

		[SerializeField]
		private List<Vector3> _listBannerMaskPos;

		[SerializeField]
		private List<Vector3> _listShipInfosPos;

		[SerializeField]
		private float _fStartVerticalOffs = 10f;

		[SerializeField]
		private float _fStartHorizontalOffs = 10f;

		private int _nDrawNowLv;

		private bool isFriend
		{
			get
			{
				return base.shipModel.IsFriend();
			}
		}

		private Vector3 bannerMaskPos
		{
			get
			{
				return this._listBannerMaskPos.get_Item((!this.isFriend) ? 1 : 0);
			}
		}

		private Vector3 shipInfosPos
		{
			get
			{
				return this._listShipInfosPos.get_Item((!this.isFriend) ? 1 : 0);
			}
		}

		private ShipExpModel expModel
		{
			get
			{
				return base.shipModel.ExpInfo;
			}
		}

		public static UIVeteransReportShipBanner Instantiate(UIVeteransReportShipBanner prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			UIVeteransReportShipBanner uIVeteransReportShipBanner = Object.Instantiate<UIVeteransReportShipBanner>(prefab);
			uIVeteransReportShipBanner.get_transform().set_parent(parent);
			uIVeteransReportShipBanner.get_transform().localScaleOne();
			uIVeteransReportShipBanner.get_transform().set_localPosition(pos);
			uIVeteransReportShipBanner.VirtualCtor(model);
			return uIVeteransReportShipBanner;
		}

		protected override void VirtualCtor(ShipModel_BattleResult model)
		{
			base.VirtualCtor(model);
			base.set_name(string.Format("VeteransReportShipBanner{0}", model.Index));
			this.SetShipInfos(model);
			this.SetUISettings(model.IsFriend());
		}

		protected override void OnUnInit()
		{
			Mem.Del<UILabel>(ref this._uiLv);
			Mem.Del<UILabel>(ref this._uiLvLabel);
			Mem.Del<UISlider>(ref this._uiEXPSlider);
			Mem.Del<UIWidget>(ref this._uiShipInfos);
			Mem.Del<UILevelUpIcon>(ref this._uiLevelUpIcon);
			Mem.Del<UILevelUpIcon>(ref this._uiLevelUpIcon);
			Mem.Del<UIMVPIcon>(ref this._uiMVPIcon);
			Mem.DelListSafe<Vector3>(ref this._listBannerMaskPos);
			Mem.DelListSafe<Vector3>(ref this._listShipInfosPos);
			Mem.Del<float>(ref this._fStartVerticalOffs);
			Mem.Del<float>(ref this._fStartHorizontalOffs);
			Mem.Del<int>(ref this._nDrawNowLv);
		}

		protected override void SetShipInfos(ShipModel_BattleResult model)
		{
			this.SetShipName(model.Name);
			this.SetShipBannerTexture(ShipUtils.LoadBannerTextureInVeteransReport(model), model.DamagedFlgEnd, model.DmgStateEnd, model.IsEscape());
			base.SetShipIcons(model.DmgStateEnd, model.IsGroundFacility(), model.IsEscape());
			this.SetHPSliderValue(model.HpStart, model.MaxHp);
			this._uiLv.textInt = (this._nDrawNowLv = model.Level);
			if (!this.isFriend)
			{
				this._uiLv.SetActive(false);
				this._uiLvLabel.SetActive(false);
				this._uiShipName.SetActive(true);
			}
			else
			{
				this._uiEXPSlider.value = Mathe.Rate(0f, 100f, (float)model.ExpInfo.ExpRateBefore);
				this._uiEXPSlider.foregroundWidget.color = KCVColor.WarVateransEXPGaugeGreen;
				this._uiShipName.SetActive(false);
			}
			this._uiEXPSlider.alpha = 0f;
			this._uiHPSlider.alpha = 1f;
		}

		protected override void SetUISettings(bool isFriend)
		{
			UIWidget arg_19_0 = this._uiShipTexture;
			float alpha = 0f;
			this._uiShipInfos.alpha = alpha;
			arg_19_0.alpha = alpha;
			this._uiMVPIcon.get_transform().set_localScale(Vector3.get_one() * 1.25f);
			this._uiMVPIcon.SetActive(false);
			this._uiShipInfos.get_transform().set_localPosition(this.shipInfosPos);
			this._uiShipTexture.get_transform().set_localPosition(this.bannerMaskPos);
			this._uiShipTexture.get_transform().set_localPosition(this._uiShipTexture.get_transform().get_localPosition() + Vector3.get_down() * this._fStartVerticalOffs);
			this._uiShipInfos.get_transform().set_localPosition(this._uiShipInfos.get_transform().get_localPosition() + ((!isFriend) ? (Vector3.get_left() * this._fStartHorizontalOffs) : (Vector3.get_right() * this._fStartHorizontalOffs)));
		}

		public void ChangeMode(UIVeteransReportShipBanner.BannerMode iMode)
		{
			if (iMode == UIVeteransReportShipBanner.BannerMode.EXP)
			{
				this._uiHPSlider.get_transform().LTCancel();
				this._uiHPSlider.get_transform().LTValue(this._uiHPSlider.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiHPSlider.alpha = x;
				});
				this._uiEXPSlider.get_transform().LTCancel();
				this._uiEXPSlider.get_transform().LTValue(this._uiEXPSlider.alpha, 1f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiEXPSlider.alpha = x;
				});
			}
			else
			{
				this._uiEXPSlider.get_transform().LTCancel();
				this._uiEXPSlider.get_transform().LTValue(this._uiEXPSlider.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiEXPSlider.alpha = x;
				});
				this._uiHPSlider.get_transform().LTCancel();
				this._uiHPSlider.get_transform().LTValue(this._uiHPSlider.alpha, 1f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiHPSlider.alpha = x;
				});
			}
		}

		public void PlayBannerSlotIn(Action callback)
		{
			this._uiShipTexture.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this._uiShipTexture.alpha = x;
			}).setOnComplete(callback);
			this._uiShipTexture.get_transform().LTMoveLocal(this.bannerMaskPos, 0.5f).setEase(LeanTweenType.easeOutSine);
		}

		public void PlayShipInfosIn(Action callback)
		{
			this._uiShipInfos.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this._uiShipInfos.alpha = x;
			});
			this._uiShipInfos.get_transform().LTMoveLocal(this.shipInfosPos, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(callback);
		}

		public void PlayHPUpdate(Action callback)
		{
			this._uiHPSlider.get_transform().LTValue((float)base.shipModel.HpStart, (float)base.shipModel.HpEnd, 0.85f).setDelay(0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				int num = Convert.ToInt32(x);
				this._uiHPSlider.value = Mathe.Rate(0f, (float)base.shipModel.MaxHp, (float)num);
				this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(base.shipModel.MaxHp, num);
			}).setOnComplete(callback);
		}

		public void PlayEXPUpdate(Action callback)
		{
			float intervalTime = 2f / (float)this.expModel.ExpRateAfter.get_Count();
			Queue<int> exp = new Queue<int>(this.expModel.ExpRateAfter);
			this.UpdateEXP(this.expModel.ExpRateBefore, exp, intervalTime, callback);
		}

		private void UpdateEXP(int nowEXPRate, Queue<int> exp, float intervalTime, Action callback)
		{
			int num = exp.Dequeue();
			this._uiEXPSlider.get_transform().LTValue((float)nowEXPRate, (float)num, intervalTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiEXPSlider.value = x / 100f;
				if (this._uiEXPSlider.value == 1f)
				{
					this.PlayLevelUp();
				}
			}).setOnComplete(delegate
			{
				if (exp.get_Count() != 0)
				{
					this.UpdateEXP(0, exp, intervalTime, callback);
				}
				else
				{
					Dlg.Call(ref callback);
				}
			});
		}

		private void PlayLevelUp()
		{
			this._nDrawNowLv++;
			this._uiLv.textInt = this._nDrawNowLv;
			this._uiLevelUpIcon.Play();
		}

		public void PlayMVP()
		{
			this._uiMVPIcon.SetActive(true);
			this._uiMVPIcon.Play();
		}
	}
}
