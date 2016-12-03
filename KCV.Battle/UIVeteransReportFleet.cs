using KCV.Battle.Utils;
using KCV.Generic;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportFleet : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabVeteransReportShipBanner;

		[SerializeField]
		private Transform _traShipBannerAnchor;

		[SerializeField]
		private float _fBannerVerticalOffs = -50f;

		[SerializeField]
		private float _fWarVateransSliderOffs = 20f;

		[SerializeField]
		private UILabel _uiFleetName;

		[SerializeField]
		private UISlider _uiWarVateransSlider;

		private UIPanel _uiPanel;

		private List<UIVeteransReportShipBanner> _listShipBanners;

		private List<int> _listWarVateransVal;

		private BattleResultModel _clsResultModel;

		private FleetType _iType;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		public FleetType fleetType
		{
			get
			{
				return this._iType;
			}
		}

		public List<UIVeteransReportShipBanner> veteransReportShipBanner
		{
			get
			{
				return this._listShipBanners;
			}
		}

		private string fleetName
		{
			get
			{
				return (this._iType != FleetType.Friend) ? this._clsResultModel.EnemyName : this._clsResultModel.DeckName;
			}
		}

		private List<int> warVateransVal
		{
			get
			{
				if (this._listWarVateransVal.get_Count() == 0)
				{
					this._listWarVateransVal = new List<int>();
					this._listWarVateransVal.Add((this._iType != FleetType.Friend) ? this._clsResultModel.HPStart_e : this._clsResultModel.HPStart_f);
					this._listWarVateransVal.Add((this._iType != FleetType.Friend) ? this._clsResultModel.HPEnd_e : this._clsResultModel.HPEnd_f);
				}
				return this._listWarVateransVal;
			}
		}

		private UIVeteransReportShipBanner lastShipBanner
		{
			get
			{
				return Enumerable.Last<UIVeteransReportShipBanner>(this._listShipBanners);
			}
		}

		public static UIVeteransReportFleet Instantiate(UIVeteransReportFleet prefab, Transform parent, Vector3 pos, BattleResultModel model, FleetType iType)
		{
			UIVeteransReportFleet uIVeteransReportFleet = Object.Instantiate<UIVeteransReportFleet>(prefab);
			uIVeteransReportFleet.get_transform().set_parent(parent);
			uIVeteransReportFleet.get_transform().localScaleOne();
			uIVeteransReportFleet.get_transform().set_localPosition(pos);
			uIVeteransReportFleet._iType = iType;
			uIVeteransReportFleet.set_name(string.Format("{0}Fleet", iType));
			uIVeteransReportFleet._clsResultModel = model;
			uIVeteransReportFleet.Init();
			return uIVeteransReportFleet;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabVeteransReportShipBanner);
			Mem.Del<Transform>(ref this._traShipBannerAnchor);
			Mem.Del<float>(ref this._fBannerVerticalOffs);
			Mem.Del<float>(ref this._fWarVateransSliderOffs);
			Mem.Del<UILabel>(ref this._uiFleetName);
			Mem.Del<UISlider>(ref this._uiWarVateransSlider);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelListSafe<UIVeteransReportShipBanner>(ref this._listShipBanners);
			Mem.DelListSafe<int>(ref this._listWarVateransVal);
			Mem.Del<BattleResultModel>(ref this._clsResultModel);
			Mem.Del<FleetType>(ref this._iType);
		}

		private bool Init()
		{
			this._listShipBanners = new List<UIVeteransReportShipBanner>();
			this._listWarVateransVal = new List<int>();
			this._uiFleetName.text = this.fleetName;
			this._uiFleetName.supportEncoding = false;
			this._uiWarVateransSlider.value = Mathe.Rate(0f, (float)this.warVateransVal.get_Item(0), 0f);
			this._uiWarVateransSlider.foregroundWidget.color = ((this.fleetType != FleetType.Friend) ? KCVColor.WarVateransGaugeRed : KCVColor.WarVateransGaugeGreen);
			this._uiWarVateransSlider.alpha = 0f;
			this._uiWarVateransSlider.get_transform().set_localPosition(this._uiWarVateransSlider.get_transform().get_localPosition() + ((this.fleetType != FleetType.Friend) ? Vector3.get_left() : Vector3.get_right()) * this._fWarVateransSliderOffs);
			return true;
		}

		public void CreateInstance()
		{
			List<ShipModel_BattleResult> list = new List<ShipModel_BattleResult>((this._iType != FleetType.Friend) ? this._clsResultModel.Ships_e : this._clsResultModel.Ships_f);
			int num = 0;
			using (List<ShipModel_BattleResult>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel_BattleResult current = enumerator.get_Current();
					if (current != null)
					{
						this._listShipBanners.Add(UIVeteransReportShipBanner.Instantiate(this._prefabVeteransReportShipBanner.GetComponent<UIVeteransReportShipBanner>(), this._traShipBannerAnchor, Vector3.get_down() * this._fBannerVerticalOffs * (float)num, current));
						num++;
					}
				}
			}
		}

		public void ChangeBannerMode(UIVeteransReportShipBanner.BannerMode iMode)
		{
			this._listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				x.ChangeMode(iMode);
			});
		}

		[DebuggerHidden]
		public IEnumerator PlayBannersSlotIn(Action callback)
		{
			UIVeteransReportFleet.<PlayBannersSlotIn>c__Iterator10B <PlayBannersSlotIn>c__Iterator10B = new UIVeteransReportFleet.<PlayBannersSlotIn>c__Iterator10B();
			<PlayBannersSlotIn>c__Iterator10B.callback = callback;
			<PlayBannersSlotIn>c__Iterator10B.<$>callback = callback;
			<PlayBannersSlotIn>c__Iterator10B.<>f__this = this;
			return <PlayBannersSlotIn>c__Iterator10B;
		}

		[DebuggerHidden]
		public IEnumerator PlayShipInfosIn(Action callback)
		{
			UIVeteransReportFleet.<PlayShipInfosIn>c__Iterator10C <PlayShipInfosIn>c__Iterator10C = new UIVeteransReportFleet.<PlayShipInfosIn>c__Iterator10C();
			<PlayShipInfosIn>c__Iterator10C.callback = callback;
			<PlayShipInfosIn>c__Iterator10C.<$>callback = callback;
			<PlayShipInfosIn>c__Iterator10C.<>f__this = this;
			return <PlayShipInfosIn>c__Iterator10C;
		}

		public void PlayBannerHPUpdate(Action callback)
		{
			this._listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				if (x == this.lastShipBanner)
				{
					x.PlayHPUpdate(callback);
				}
				else
				{
					x.PlayHPUpdate(null);
				}
			});
		}

		public void PlayWarVateransGauge(Action callback)
		{
			this._uiWarVateransSlider.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiWarVateransSlider.alpha = x;
			});
			Vector3 to = this._uiWarVateransSlider.get_transform().get_localPosition() + ((this.fleetType != FleetType.Friend) ? Vector3.get_right() : Vector3.get_left()) * this._fWarVateransSliderOffs;
			this._uiWarVateransSlider.get_transform().LTMoveLocal(to, 0.5f).setEase(LeanTweenType.linear);
			this._uiWarVateransSlider.get_transform().LTValue(0f, (float)this.warVateransVal.get_Item(1), 0.5f).setDelay(1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiWarVateransSlider.value = Mathe.Rate(0f, (float)this.warVateransVal.get_Item(0), x);
			}).setOnComplete(delegate
			{
				Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
				{
					Dlg.Call(ref callback);
				});
			});
		}

		public void PlayEXPUpdate(Action callback)
		{
			this._listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				x.PlayEXPUpdate((!(x == this.lastShipBanner)) ? null : callback);
			});
		}
	}
}
