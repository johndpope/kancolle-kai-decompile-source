using KCV.Battle.Utils;
using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdWithdrawalDecisionSelection : BaseAnimation
	{
		public enum Mode
		{
			Selection,
			TacticalSituation
		}

		[SerializeField]
		private Transform _prefabUITacticalSituation;

		[SerializeField]
		private List<UIWithdrawalButton> _listHexExBtns;

		private UITacticalSituation _uiTacticalSituation;

		private UIPanel _uiPanel;

		private ProdWithdrawalDecisionSelection.Mode _iMode;

		private WithdrawalDecisionType _iSelectType;

		private bool _isDecide;

		private bool _isInputPossible;

		private DelDecideHexButtonEx _delDecideWithdrawalButton;

		private StatementMachine _clsState;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public bool isDecide
		{
			get
			{
				return this._isDecide;
			}
		}

		public bool isInputPossible
		{
			get
			{
				return this._isInputPossible;
			}
		}

		public static ProdWithdrawalDecisionSelection Instantiate(ProdWithdrawalDecisionSelection prefab, Transform parent)
		{
			ProdWithdrawalDecisionSelection prodWithdrawalDecisionSelection = Object.Instantiate<ProdWithdrawalDecisionSelection>(prefab);
			prodWithdrawalDecisionSelection.get_transform().set_parent(parent);
			prodWithdrawalDecisionSelection.get_transform().localScaleZero();
			prodWithdrawalDecisionSelection.get_transform().localPositionZero();
			prodWithdrawalDecisionSelection.Init();
			return prodWithdrawalDecisionSelection;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<Transform>(ref this._prefabUITacticalSituation);
			Mem.DelListSafe<UIWithdrawalButton>(ref this._listHexExBtns);
			Mem.DelComponentSafe<UITacticalSituation>(ref this._uiTacticalSituation);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<ProdWithdrawalDecisionSelection.Mode>(ref this._iMode);
			Mem.Del<WithdrawalDecisionType>(ref this._iSelectType);
			Mem.Del<bool>(ref this._isDecide);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<DelDecideHexButtonEx>(ref this._delDecideWithdrawalButton);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
		}

		public override bool Init()
		{
			base.Init();
			this._iMode = ProdWithdrawalDecisionSelection.Mode.Selection;
			this._clsState = new StatementMachine();
			this._isDecide = false;
			this._isInputPossible = false;
			this._delDecideWithdrawalButton = null;
			this.InitObjects();
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInWithdrawalDecision(this._iMode);
			return true;
		}

		private bool InitObjects()
		{
			int cnt = 0;
			this._listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				x.Init(cnt, false, 20, delegate
				{
					this.OnDeside((WithdrawalDecisionType)x.index);
				});
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (WithdrawalDecisionType)x.index);
				cnt++;
			});
			return true;
		}

		public void Play(Action forceCallback, DelDecideHexButtonEx decideCallback)
		{
			base.Init();
			this._actForceCallback = forceCallback;
			this._delDecideWithdrawalButton = decideCallback;
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
			{
				BattleShutter shutter = BattleTaskManager.GetPrefabFile().battleShutter;
				shutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					this.get_transform().localScaleOne();
					Observable.FromCoroutine(new Func<IEnumerator>(this.PlayForceCallback), false).Subscribe(delegate(Unit __)
					{
						shutter.ReqMode(BaseShutter.ShutterMode.Open, delegate
						{
						});
					});
				});
			});
		}

		public bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return false;
		}

		[DebuggerHidden]
		private IEnumerator PlayForceCallback()
		{
			ProdWithdrawalDecisionSelection.<PlayForceCallback>c__IteratorF4 <PlayForceCallback>c__IteratorF = new ProdWithdrawalDecisionSelection.<PlayForceCallback>c__IteratorF4();
			<PlayForceCallback>c__IteratorF.<>f__this = this;
			return <PlayForceCallback>c__IteratorF;
		}

		private bool InitWithdrawalSelection(object data)
		{
			this._iMode = ProdWithdrawalDecisionSelection.Mode.Selection;
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.SetNavigationInWithdrawalDecision(this._iMode);
			return false;
		}

		private bool UpdateWithdrawalSelection(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (!this._isDecide && this._isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					this.PreparaNext(false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					this.PreparaNext(true);
				}
				else
				{
					if (keyControl.GetDown(KeyControl.KeyName.MARU))
					{
						this._listHexExBtns.get_Item((int)this._iSelectType).OnDecide();
						return true;
					}
					if (keyControl.GetDown(KeyControl.KeyName.SANKAKU))
					{
						return this.OnReqModeTacticalSituation();
					}
				}
			}
			return false;
		}

		private void PreparaNext(bool isFoward)
		{
			WithdrawalDecisionType iSelectType = this._iSelectType;
			this._iSelectType = (WithdrawalDecisionType)Mathe.NextElement((int)this._iSelectType, 0, this._listHexExBtns.get_Count() - 1, isFoward);
			if (iSelectType != this._iSelectType)
			{
				this.ChangeFocus(this._iSelectType);
			}
		}

		private void ChangeFocus(WithdrawalDecisionType iType)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this._listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				x.isFocus = (x.index == (int)iType);
			});
		}

		private bool OnReqModeTacticalSituation()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitTacticalSituation), new StatementMachine.StatementMachineUpdate(this.UpdateTacticalSituation));
			return true;
		}

		private bool InitTacticalSituation(object data)
		{
			this._iMode = ProdWithdrawalDecisionSelection.Mode.TacticalSituation;
			this._uiTacticalSituation.Show(delegate
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.SetNavigationInWithdrawalDecision(this._iMode);
			});
			return false;
		}

		private bool UpdateTacticalSituation(object data)
		{
			if (this._uiTacticalSituation != null)
			{
				this._uiTacticalSituation.Run();
			}
			return false;
		}

		private void OnTacticalSituationBack()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitWithdrawalSelection), new StatementMachine.StatementMachineUpdate(this.UpdateWithdrawalSelection));
		}

		private void OnActive(WithdrawalDecisionType iType)
		{
			if (this._iSelectType != iType)
			{
				this._iSelectType = iType;
				this.ChangeFocus(this._iSelectType);
			}
		}

		private void OnDeside(WithdrawalDecisionType iType)
		{
			if (this._isDecide)
			{
				return;
			}
			this._isDecide = true;
			this._isInputPossible = false;
			this._listHexExBtns.ForEach(delegate(UIWithdrawalButton x)
			{
				x.toggle.set_enabled(false);
			});
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_036);
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.Hide(0.3f, null);
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
			{
				if (this._delDecideWithdrawalButton != null)
				{
					this._delDecideWithdrawalButton(this._listHexExBtns.get_Item((int)iType));
				}
			});
		}
	}
}
