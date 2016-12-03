using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdVeteransReport : MonoBehaviour
	{
		[Serializable]
		private class VeteransReportCommon : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiAreaName;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UITexture _uiCenterLine;

			[SerializeField]
			private UISprite _uiResultLabel;

			[SerializeField]
			private UITexture _uiOverlay;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public VeteransReportCommon(Transform obj)
			{
			}

			public bool Init(BattleResultModel model)
			{
				this._uiAreaName.text = model.MapName;
				this._uiOverlay.alpha = 0f;
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UILabel>(ref this._uiAreaName);
				Mem.Del<UITexture>(ref this._uiBackground);
				Mem.Del<UITexture>(ref this._uiCenterLine);
				Mem.Del(ref this._uiResultLabel);
				Mem.Del<UITexture>(ref this._uiOverlay);
			}

			public LTDescr ShowOverlay()
			{
				return this._uiOverlay.get_transform().LTValue(this._uiOverlay.alpha, 0.5f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiOverlay.alpha = x;
				});
			}

			public LTDescr HideOverlay()
			{
				return this._uiOverlay.get_transform().LTValue(this._uiOverlay.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiOverlay.alpha = x;
				});
			}
		}

		[SerializeField]
		private ProdVeteransReport.VeteransReportCommon _clsCommon;

		[SerializeField]
		private UIGearButton _uiGearBtn;

		[SerializeField]
		private Transform _prefabVeteransReportFleet;

		[SerializeField]
		private Transform _prefabVeteransReportBonus;

		[SerializeField]
		private Transform _prefabVeteransReportMVPShip;

		[SerializeField]
		private Transform _prefabProdWinRankJudge;

		[SerializeField]
		private List<Vector2> _listVeteransReportFleetsPos;

		[SerializeField]
		private Vector3 _vBounusPos = new Vector3(240f, 0f, 0f);

		[SerializeField]
		private Vector3 _vMVPShipAnchorPos = new Vector3(270f, -160f, 0f);

		private bool _isInputEnabled;

		private bool _isWinRunkFinished;

		private bool _isEXPUpdateFinished;

		private bool _isProdFinished;

		private BattleResultModel _clsResultModel;

		private List<UIVeteransReportFleet> _listVeteransReportFleets;

		private UIVeteransReportBonus _uiBonus;

		private UIVeteransReportMVPShip _uiMVPShip;

		private ProdWinRankJudge _prodWinRankJudge;

		private StatementMachine _clsState;

		public static ProdVeteransReport Instantiate(ProdVeteransReport prefab, Transform parent, BattleResultModel model)
		{
			ProdVeteransReport prodVeteransReport = Object.Instantiate<ProdVeteransReport>(prefab);
			prodVeteransReport.get_transform().set_parent(parent);
			prodVeteransReport.get_transform().localPositionZero();
			prodVeteransReport.get_transform().localScaleOne();
			prodVeteransReport._clsResultModel = model;
			return prodVeteransReport.VirtualCtor();
		}

		private void OnDestroy()
		{
			Mem.DelIDisposableSafe<ProdVeteransReport.VeteransReportCommon>(ref this._clsCommon);
			Mem.Del<UIGearButton>(ref this._uiGearBtn);
			Mem.Del<Transform>(ref this._prefabVeteransReportFleet);
			Mem.Del<Transform>(ref this._prefabVeteransReportBonus);
			Mem.Del<Transform>(ref this._prefabVeteransReportMVPShip);
			Mem.Del<Transform>(ref this._prefabProdWinRankJudge);
			Mem.DelListSafe<Vector2>(ref this._listVeteransReportFleetsPos);
			Mem.Del<Vector3>(ref this._vBounusPos);
			Mem.Del<Vector3>(ref this._vMVPShipAnchorPos);
			Mem.Del<bool>(ref this._isInputEnabled);
			Mem.Del<bool>(ref this._isWinRunkFinished);
			Mem.Del<bool>(ref this._isEXPUpdateFinished);
			Mem.Del<bool>(ref this._isProdFinished);
			Mem.Del<BattleResultModel>(ref this._clsResultModel);
			Mem.DelListSafe<UIVeteransReportFleet>(ref this._listVeteransReportFleets);
			Mem.Del<UIVeteransReportBonus>(ref this._uiBonus);
			Mem.Del<UIVeteransReportMVPShip>(ref this._uiMVPShip);
			Mem.Del<ProdWinRankJudge>(ref this._prodWinRankJudge);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
		}

		private ProdVeteransReport VirtualCtor()
		{
			this._clsState = new StatementMachine();
			this._isInputEnabled = false;
			this._isWinRunkFinished = false;
			this._isEXPUpdateFinished = false;
			this._isProdFinished = false;
			this._uiGearBtn.isColliderEnabled = false;
			this._uiGearBtn.widget.alpha = 0f;
			this._clsCommon.Init(this._clsResultModel);
			return this;
		}

		[DebuggerHidden]
		public IEnumerator CreateInstance(bool isPractice)
		{
			ProdVeteransReport.<CreateInstance>c__IteratorE8 <CreateInstance>c__IteratorE = new ProdVeteransReport.<CreateInstance>c__IteratorE8();
			<CreateInstance>c__IteratorE.isPractice = isPractice;
			<CreateInstance>c__IteratorE.<$>isPractice = isPractice;
			<CreateInstance>c__IteratorE.<>f__this = this;
			return <CreateInstance>c__IteratorE;
		}

		private void SortPanelDepth()
		{
		}

		public bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this._isProdFinished;
		}

		public void PlayVeteransReport()
		{
			this.PlayWarVateransSlotIn();
		}

		private void PlayWarVateransSlotIn()
		{
			FleetType callbackFleetType = FleetType.Friend;
			if (this._listVeteransReportFleets.get_Item(1).veteransReportShipBanner.get_Count() >= this._listVeteransReportFleets.get_Item(0).veteransReportShipBanner.get_Count())
			{
				callbackFleetType = FleetType.Enemy;
			}
			this._listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				Action act = (x.fleetType != callbackFleetType) ? null : new Action(this.PlayWarVateransShipInfosIn);
				Observable.FromCoroutine(() => x.PlayBannersSlotIn(act), false).Subscribe<Unit>().AddTo(this.get_gameObject());
			});
		}

		private void PlayWarVateransShipInfosIn()
		{
			this._listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				Action act = (x.fleetType != FleetType.Friend) ? null : new Action(this.PlayWarVeteransShipHPUpdate);
				Observable.FromCoroutine(() => x.PlayShipInfosIn(act), false).Subscribe<Unit>().AddTo(base.get_gameObject());
			});
		}

		private void PlayWarVeteransShipHPUpdate()
		{
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
			{
				this._listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
				{
					x.PlayBannerHPUpdate(null);
				});
			});
			Observable.Timer(TimeSpan.FromSeconds(0.34999999403953552)).Subscribe(delegate(long _)
			{
				this.PlayWarVateransGauge();
			});
		}

		private void PlayWarVateransGauge()
		{
			this._listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				if (x.fleetType == FleetType.Friend)
				{
					x.PlayWarVateransGauge(delegate
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.initWinRunkJudge), new StatementMachine.StatementMachineUpdate(this.updateWinRunkJudge));
					});
				}
				else
				{
					x.PlayWarVateransGauge(null);
				}
			});
		}

		private bool initWinRunkJudge(object data)
		{
			this._listVeteransReportFleets.get_Item(1).get_transform().LTMoveLocal(Vector3.get_right() * 1000f, 0.3f).setEase(LeanTweenType.easeInSine).setOnComplete(delegate
			{
				this._listVeteransReportFleets.get_Item(1).panel.widgetsAreStatic = true;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.blur.set_enabled(false);
				this._clsCommon.ShowOverlay();
			});
			Observable.FromCoroutine(new Func<IEnumerator>(this._prodWinRankJudge.StartBattleJudge), false).Subscribe(delegate(Unit _)
			{
				this.OnDecideWinRunkGearBtn();
				Mem.DelComponentSafe<ProdWinRankJudge>(ref this._prodWinRankJudge);
			});
			return false;
		}

		private bool updateWinRunkJudge(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (this._isInputEnabled && keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this._uiGearBtn.Decide();
				return true;
			}
			return this._isWinRunkFinished;
		}

		private void OnDecideWinRunkGearBtn()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.blur.set_enabled(false);
			this._clsCommon.HideOverlay();
			this._isWinRunkFinished = true;
			this._isInputEnabled = false;
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.initEXPReflection), new StatementMachine.StatementMachineUpdate(this.updateEXPReflection));
			this._listVeteransReportFleets.get_Item(0).ChangeBannerMode(UIVeteransReportShipBanner.BannerMode.EXP);
			if (this._uiMVPShip.shipModel != null)
			{
				this._listVeteransReportFleets.get_Item(0).veteransReportShipBanner.Find((UIVeteransReportShipBanner x) => x.shipModel == this._uiMVPShip.shipModel).PlayMVP();
			}
		}

		private bool initEXPReflection(object data)
		{
			TrophyUtil.Unlock_UserLevel();
			this._uiMVPShip.Show(BattleUtils.IsPlayMVPVoice(this._clsResultModel.WinRank), null);
			this._uiBonus.Show(null);
			Observable.Timer(TimeSpan.FromSeconds(0.25)).Subscribe(delegate(long _)
			{
				this._listVeteransReportFleets.get_Item(0).PlayEXPUpdate(delegate
				{
					UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
					battleNavigation.SetNavigationInResult();
					battleNavigation.Show(0.2f, null);
					this._uiGearBtn.Show(delegate
					{
						this._isInputEnabled = true;
					});
				});
				this._uiGearBtn.SetDecideAction(new Action(this.OnDecideEXPUodateGearBtn));
			});
			return false;
		}

		private bool updateEXPReflection(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (this._isInputEnabled && keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this._uiGearBtn.Decide();
				return true;
			}
			return this._isEXPUpdateFinished;
		}

		private void OnDecideEXPUodateGearBtn()
		{
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.Hide(0.2f, null);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this._isEXPUpdateFinished = true;
			this._isProdFinished = true;
			this._uiGearBtn.Hide(null);
		}
	}
}
