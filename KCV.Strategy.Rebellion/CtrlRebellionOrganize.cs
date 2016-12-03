using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class CtrlRebellionOrganize : MonoBehaviour
	{
		public enum RebellionOrganizeMode
		{
			Main,
			Detail
		}

		public const float STATE_CHANGE_TIME = 0.2f;

		public static readonly LeanTweenType STATE_CHANGE_EASING = LeanTweenType.easeInSine;

		[SerializeField]
		private int _nBaseDepth;

		[SerializeField]
		private Transform _prefabUINavigation;

		[SerializeField]
		private Transform _prefabFleetSelector;

		[SerializeField]
		private Transform _prefabParticipatingFleetSelector;

		[SerializeField]
		private Transform _prefabHeader;

		[SerializeField]
		private UIRebellionFleetShipsList _uiFleetShipsList;

		private bool _isDecide;

		private CtrlRebellionOrganize.RebellionOrganizeMode _iMode;

		private UIRebellionNavigation _uiNavigation;

		private UIRebellionFleetSelector _uiFleetSelector;

		private UIRebellionParticipatingFleetSelector _uiParticipatingFleetSelector;

		private UIRebellionHeader _uiHeader;

		private StatementMachine _clsState;

		private Action _actSortieStartCallback;

		private Action _actCalcelCallback;

		public CtrlRebellionOrganize.RebellionOrganizeMode mode
		{
			get
			{
				return this._iMode;
			}
		}

		public int baseDepth
		{
			get
			{
				return this._nBaseDepth;
			}
			set
			{
				if (this._nBaseDepth != value)
				{
					this._nBaseDepth = value;
					this.SortPanelDepth(this._nBaseDepth);
				}
			}
		}

		public UIRebellionParticipatingFleetSelector participatingFleetSelector
		{
			get
			{
				return this._uiParticipatingFleetSelector;
			}
		}

		public static CtrlRebellionOrganize Instantiate(CtrlRebellionOrganize prefab, Transform parent, Action sortieStartAction, Action cancelAction)
		{
			CtrlRebellionOrganize ctrlRebellionOrganize = Object.Instantiate<CtrlRebellionOrganize>(prefab);
			ctrlRebellionOrganize.get_transform().set_parent(parent);
			ctrlRebellionOrganize.get_transform().localPositionZero();
			ctrlRebellionOrganize.get_transform().localScaleZero();
			ctrlRebellionOrganize._actSortieStartCallback = sortieStartAction;
			ctrlRebellionOrganize._actCalcelCallback = cancelAction;
			ctrlRebellionOrganize.Setup();
			return ctrlRebellionOrganize;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabUINavigation);
			Mem.Del<Transform>(ref this._prefabFleetSelector);
			Mem.Del<Transform>(ref this._prefabParticipatingFleetSelector);
			Mem.Del<UIRebellionNavigation>(ref this._uiNavigation);
			Mem.Del<UIRebellionFleetSelector>(ref this._uiFleetSelector);
			Mem.Del<UIRebellionParticipatingFleetSelector>(ref this._uiParticipatingFleetSelector);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.Del<Action>(ref this._actSortieStartCallback);
			Mem.Del<Action>(ref this._actCalcelCallback);
		}

		private bool Setup()
		{
			this._isDecide = false;
			this._iMode = CtrlRebellionOrganize.RebellionOrganizeMode.Main;
			this._clsState = new StatementMachine();
			Observable.FromCoroutine(new Func<IEnumerator>(this.InstantiateObjects), false).Subscribe(delegate(Unit _)
			{
				this.Init();
			}).AddTo(base.get_gameObject());
			return true;
		}

		[DebuggerHidden]
		private IEnumerator InstantiateObjects()
		{
			CtrlRebellionOrganize.<InstantiateObjects>c__Iterator15D <InstantiateObjects>c__Iterator15D = new CtrlRebellionOrganize.<InstantiateObjects>c__Iterator15D();
			<InstantiateObjects>c__Iterator15D.<>f__this = this;
			return <InstantiateObjects>c__Iterator15D;
		}

		public bool Init()
		{
			this.SortPanelDepth(this._nBaseDepth);
			this._uiParticipatingFleetSelector.Init(new DelDicideRebellionOrganizeSelectBtn(this.DecideParticipatingFleetInfo), new Action(this.DecideSortieStart));
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < rebellionManager.Decks.get_Count(); i++)
			{
				if (rebellionManager.Decks.get_Item(i).IsValidSortie().get_Count() == 0)
				{
					list.Add(rebellionManager.Decks.get_Item(i));
				}
			}
			this._uiFleetSelector.Init(list, 0, new DelDecideRebellionOrganizeFleetSelector(this.DecideFleetSelector));
			this._uiFleetSelector.rouletteSelector.controllable = false;
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			keycontrol.reset(0, 0, 0.4f, 0.1f);
			keycontrol.setMinMaxIndex(0, Enum.GetValues(typeof(RebellionFleetType)).get_Length());
			keycontrol.useDoubleIndex(0, this._uiFleetSelector.fleetCnt - 1);
			base.get_transform().localScaleOne();
			this.Show();
			return true;
		}

		private void SortPanelDepth(int baseDepth)
		{
			this._uiFleetSelector.panel.depth = baseDepth;
			this._uiParticipatingFleetSelector.panel.depth = this._uiFleetSelector.panel.depth + 1;
			this._uiFleetShipsList.panel.depth = this._uiParticipatingFleetSelector.panel.depth + 1;
			this._uiNavigation.panel.depth = this._uiFleetShipsList.panel.depth + 1;
			this._uiHeader.panel.depth = this._uiNavigation.panel.depth + 1;
		}

		private void Show()
		{
			this._uiHeader.Show(null);
			this._uiNavigation.Show(null);
			this._uiParticipatingFleetSelector.Show(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMainProcess), new StatementMachine.StatementMachineUpdate(this.UpdateMainProcess));
			});
		}

		public bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return true;
		}

		private bool InitMainProcess(object data)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			this._uiNavigation.SetNavigation(CtrlRebellionOrganize.RebellionOrganizeMode.Main);
			this._uiFleetSelector.isColliderEnabled = true;
			this._uiParticipatingFleetSelector.isColliderEnabled = true;
			return false;
		}

		private bool UpdateMainProcess(object data)
		{
			if (this._isDecide)
			{
				return true;
			}
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			if (keycontrol.GetDown(KeyControl.KeyName.UP))
			{
				this._uiParticipatingFleetSelector.MovePrev();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.DOWN))
			{
				this._uiParticipatingFleetSelector.MoveNext();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.LEFT))
			{
				this._uiFleetSelector.rouletteSelector.MovePrev();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.RIGHT))
			{
				this._uiFleetSelector.rouletteSelector.MoveNext();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.MARU))
			{
				this._uiFleetSelector.rouletteSelector.Determine();
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.BATU))
			{
				if (this._actCalcelCallback != null)
				{
					this._actCalcelCallback.Invoke();
				}
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.SHIKAKU))
			{
				if (!this._uiParticipatingFleetSelector.isSortieStartFocus)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					this._uiParticipatingFleetSelector.SetFleetInfo((RebellionFleetType)this._uiParticipatingFleetSelector.nowIndex, null);
					this._uiParticipatingFleetSelector.ChkSortieStartState();
				}
			}
			else if (keycontrol.GetDown(KeyControl.KeyName.SANKAKU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
				DebugUtils.Log("RebellionOrganizeCtrl", string.Format("[{0}({1})]{2}", this._uiFleetSelector.nowSelectedDeck.Id, this._uiFleetSelector.nowSelectedDeck.Name, (this._uiFleetSelector.nowSelectedDeck.GetFlagShip() == null) ? string.Empty : this._uiFleetSelector.nowSelectedDeck.GetFlagShip().Name));
				if (this._uiFleetSelector.nowSelectedDeck.GetFlagShip() == null)
				{
					return false;
				}
				this._uiParticipatingFleetSelector.isColliderEnabled = false;
				this._uiFleetSelector.isColliderEnabled = false;
				this._uiFleetSelector.rouletteSelector.controllable = false;
				this._uiFleetShipsList.Init(this._uiFleetSelector.nowSelectedDeck);
				this.ChangeState(CtrlRebellionOrganize.RebellionOrganizeMode.Detail);
				return true;
			}
			return false;
		}

		private bool InitDetailProcess(object data)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			this._uiNavigation.SetNavigation(CtrlRebellionOrganize.RebellionOrganizeMode.Detail);
			return false;
		}

		private bool UpdateDetailProcess(object data)
		{
			KeyControl keycontrol = StrategyTaskManager.GetStrategyRebellion().keycontrol;
			if (keycontrol.GetDown(KeyControl.KeyName.MARU))
			{
				this.SetRebellionParticipatingFleet((RebellionFleetType)this._uiParticipatingFleetSelector.nowIndex, this._uiFleetSelector.nowSelectedDeck);
				this.ChangeState(CtrlRebellionOrganize.RebellionOrganizeMode.Main);
				return true;
			}
			if (keycontrol.GetDown(KeyControl.KeyName.BATU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				this.ChangeState(CtrlRebellionOrganize.RebellionOrganizeMode.Main);
				return true;
			}
			if (keycontrol.GetDown(KeyControl.KeyName.SANKAKU))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
				this.ChangeState(CtrlRebellionOrganize.RebellionOrganizeMode.Main);
				return true;
			}
			return false;
		}

		private bool SetRebellionParticipatingFleet(RebellionFleetType iType, DeckModel model)
		{
			if (this.isValidSetDeck(iType, model))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this._uiParticipatingFleetSelector.SetFleetInfo(iType, this._uiFleetSelector.nowSelectedDeck);
				this._uiParticipatingFleetSelector.ChkSortieStartState();
				return true;
			}
			return false;
		}

		private bool isValidSetDeck(RebellionFleetType iType, DeckModel model)
		{
			DebugUtils.Log("isValidSetDeck::" + iType);
			bool flag = !this._uiParticipatingFleetSelector.IsAlreadySetFleet(this._uiFleetSelector.nowSelectedDeck) && this._uiFleetSelector.nowSelectedDeck.GetFlagShip() != null;
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			bool flag2 = false;
			List<IsGoCondition> list = null;
			if (iType != RebellionFleetType.VanguardFleet && iType != RebellionFleetType.DecisiveBattlePrimaryFleet)
			{
				if (iType == RebellionFleetType.VanguardSupportFleet)
				{
					list = rebellionManager.IsValidMissionSub(model.Id);
				}
				else if (iType == RebellionFleetType.DecisiveBattleSupportFleet)
				{
					list = rebellionManager.IsValid_MissionMain(model.Id);
				}
			}
			if (list == null || list.get_Count() == 0)
			{
				flag2 = true;
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list.get_Item(0)));
			}
			return flag && flag2;
		}

		private void ChangeState(CtrlRebellionOrganize.RebellionOrganizeMode iMode)
		{
			this._iMode = iMode;
			if (iMode != CtrlRebellionOrganize.RebellionOrganizeMode.Main)
			{
				if (iMode == CtrlRebellionOrganize.RebellionOrganizeMode.Detail)
				{
					this._uiFleetSelector.ReqMode(CtrlRebellionOrganize.RebellionOrganizeMode.Detail, 0.2f, CtrlRebellionOrganize.STATE_CHANGE_EASING);
					this._uiHeader.Hide(null);
					this._uiParticipatingFleetSelector.Hide(null);
					this._uiFleetShipsList.Show(delegate
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDetailProcess), new StatementMachine.StatementMachineUpdate(this.UpdateDetailProcess));
					});
				}
			}
			else
			{
				this._uiFleetSelector.ReqMode(CtrlRebellionOrganize.RebellionOrganizeMode.Main, 0.2f, CtrlRebellionOrganize.STATE_CHANGE_EASING);
				this._uiHeader.Show(null);
				this._uiParticipatingFleetSelector.Show(null);
				this._uiFleetShipsList.Hide(delegate
				{
					this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMainProcess), new StatementMachine.StatementMachineUpdate(this.UpdateMainProcess));
				});
			}
		}

		private void DecideFleetSelector(DeckModel model)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Format("[{0}({1})]{2}", model.Id, model.Name, (model.GetFlagShip() == null) ? string.Empty : model.GetFlagShip().Name));
			if (this._uiParticipatingFleetSelector.isSortieStartFocus)
			{
				this.DecideSortieStart();
				return;
			}
			if (this.SetRebellionParticipatingFleet((RebellionFleetType)this._uiParticipatingFleetSelector.nowIndex, this._uiFleetSelector.nowSelectedDeck))
			{
			}
		}

		private void DecideParticipatingFleetInfo(IRebellionOrganizeSelectObject selectObj)
		{
			DebugUtils.Log("RebellionOrganizeCtrl", selectObj.button.get_name());
		}

		private void DecideSortieStart()
		{
			DebugUtils.Log("RebellionOrganizeCtrl", string.Empty);
			if (this._isDecide)
			{
				return;
			}
			this._isDecide = true;
			if (this._actSortieStartCallback != null)
			{
				this._actSortieStartCallback.Invoke();
			}
		}
	}
}
