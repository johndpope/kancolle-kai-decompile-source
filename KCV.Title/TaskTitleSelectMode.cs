using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleSelectMode : SceneTaskMono
	{
		private UIPressAnyKey _uiPressAnyKey;

		private CtrlTitleSelectMode _ctrlTitleSelectMode;

		private StatementMachine _clsState;

		private IDisposable _disLeaveSubscription;

		protected override bool Init()
		{
			App.InitLoadMasterDataManager();
			App.InitSystems();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			maskPanel.get_transform().LTCancel();
			maskPanel.get_transform().LTValue(maskPanel.alpha, 0f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				maskPanel.alpha = x;
			});
			SoundUtils.PlaySceneBGM(BGMFileInfos.Strategy);
			this._clsState = new StatementMachine();
			UITitleLogo logo = TitleTaskManager.GetUITitleLogo();
			if (logo.panel.alpha == 0f)
			{
				logo.Show().setOnComplete(delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
					{
						logo.StartLogoAnim();
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitPressAnyKey), new StatementMachine.StatementMachineUpdate(this.UpdatePressAnyKey));
						this.SetupLeaveTimer();
					});
				});
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitPressAnyKey), new StatementMachine.StatementMachineUpdate(this.UpdatePressAnyKey));
				this.SetupLeaveTimer();
			}
			Observable.FromCoroutine(new Func<IEnumerator>(this.NoticeMasterInitComplete), false).Subscribe<Unit>().AddTo(base.get_gameObject());
			return true;
		}

		protected override bool UnInit()
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			this._disLeaveSubscription.Dispose();
			if (this._uiPressAnyKey != null && this._uiPressAnyKey.get_gameObject() != null)
			{
				Object.Destroy(this._uiPressAnyKey.get_gameObject());
			}
			Mem.Del<UIPressAnyKey>(ref this._uiPressAnyKey);
			if (this._ctrlTitleSelectMode != null && this._ctrlTitleSelectMode.get_gameObject() != null)
			{
				Object.Destroy(this._ctrlTitleSelectMode.get_gameObject());
			}
			Mem.Del<CtrlTitleSelectMode>(ref this._ctrlTitleSelectMode);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			if (keyControl.IsAnyKey)
			{
				this.SetupLeaveTimer();
			}
			this._clsState.OnUpdate(Time.get_deltaTime());
			return TitleTaskManager.GetMode() == TitleTaskManagerMode.TitleTaskManagerMode_BEF || TitleTaskManager.GetMode() == TitleTaskManagerMode.SelectMode;
		}

		private bool InitPressAnyKey(object data)
		{
			this._uiPressAnyKey = UIPressAnyKey.Instantiate(TitleTaskManager.GetPrefabFile().prefabUIPressAnyKey.GetComponent<UIPressAnyKey>(), TitleTaskManager.GetSharedPlace(), new Action(this.OnPressAnyKeyFinished));
			return false;
		}

		private bool UpdatePressAnyKey(object data)
		{
			if (!(this._uiPressAnyKey != null))
			{
				return false;
			}
			if (this._uiPressAnyKey.Run())
			{
				this.SetupLeaveTimer();
				return true;
			}
			return false;
		}

		private void OnPressAnyKeyFinished()
		{
			if (this._uiPressAnyKey != null)
			{
				Object.Destroy(this._uiPressAnyKey.get_gameObject());
			}
			Mem.Del<UIPressAnyKey>(ref this._uiPressAnyKey);
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSelectMode), new StatementMachine.StatementMachineUpdate(this.UpdateSelectMode));
		}

		private bool InitSelectMode(object data)
		{
			this._ctrlTitleSelectMode = CtrlTitleSelectMode.Instantiate(TitleTaskManager.GetPrefabFile().prefabCtrlTitleSelectMode.GetComponent<CtrlTitleSelectMode>(), TitleTaskManager.GetSharedPlace(), new Action(this.SetupLeaveTimer));
			this._ctrlTitleSelectMode.Play(new Action<SelectMode>(this.OnDecideMode));
			return false;
		}

		private bool UpdateSelectMode(object data)
		{
			if (this._ctrlTitleSelectMode != null)
			{
				this._ctrlTitleSelectMode.Run();
				return false;
			}
			this._disLeaveSubscription.Dispose();
			return true;
		}

		private void SetupLeaveTimer()
		{
			if (this._disLeaveSubscription != null)
			{
				this._disLeaveSubscription.Dispose();
			}
			this._disLeaveSubscription = Observable.Timer(TimeSpan.FromSeconds(30.0)).Subscribe(delegate(long _)
			{
				this._clsState.Clear();
				UITitleLogo uITitleLogo = TitleTaskManager.GetUITitleLogo();
				uITitleLogo.Hide();
				SoundUtils.StopFadeBGM(0.3f, delegate
				{
					TitleTaskManager.ReqMode(TitleTaskManagerMode.TitleTaskManagerMode_ST);
				});
			}).AddTo(base.get_gameObject());
		}

		[DebuggerHidden]
		private IEnumerator NoticeMasterInitComplete()
		{
			return new TaskTitleSelectMode.<NoticeMasterInitComplete>c__Iterator1A2();
		}

		private void OnDecideMode(SelectMode iMode)
		{
			Object.Destroy(this._ctrlTitleSelectMode.get_gameObject());
			Mem.Del<CtrlTitleSelectMode>(ref this._ctrlTitleSelectMode);
			if (iMode != SelectMode.Appointed)
			{
				if (iMode == SelectMode.Inheriting)
				{
					Observable.FromCoroutine<AsyncOperation>((IObserver<AsyncOperation> observer) => TitleTaskManager.GotoLoadScene(observer)).Subscribe(delegate(AsyncOperation x)
					{
						x.set_allowSceneActivation(true);
					}).AddTo(base.get_gameObject());
				}
			}
			else
			{
				TitleTaskManager.ReqMode(TitleTaskManagerMode.NewGame);
			}
		}
	}
}
