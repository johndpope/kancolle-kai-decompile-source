using KCV.Utils;
using local.models;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupFirstShipSelect : SceneTaskMono
	{
		private CtrlStarterSelect _ctrlStarterSelect;

		private CtrlPartnerSelect _ctrlPartnerSelect;

		private StatementMachine _clsState;

		private bool _shipCancelled;

		private bool isCached;

		public CtrlPartnerSelect ctrlPartnerSelect
		{
			get
			{
				return this._ctrlPartnerSelect;
			}
		}

		protected override bool Init()
		{
			if (this._ctrlPartnerSelect == null)
			{
				this._ctrlPartnerSelect = GameObject.Find("PartnerShip").GetComponent<CtrlPartnerSelect>();
			}
			if (this._ctrlStarterSelect == null)
			{
				this._ctrlStarterSelect = GameObject.Find("CtrlStarterSelect").GetComponent<CtrlStarterSelect>();
			}
			this._shipCancelled = false;
			this._clsState = new StatementMachine();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitStarterSelect), new StatementMachine.StatementMachineUpdate(this.UpdateStarterSelect));
			return true;
		}

		protected override bool UnInit()
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			return true;
		}

		protected override bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF || StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.FirstShipSelect;
		}

		private bool InitStarterSelect(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			startupHeader.SetMessage("ゲーム開始");
			this._ctrlStarterSelect.Init(new Action<CtrlStarterSelect.StarterType>(this.OnStarterSelected), new Action(this.OnStarterSelectCancel));
			this._ctrlPartnerSelect.SetActive(false);
			this.StartCache(null);
			return false;
		}

		private bool UpdateStarterSelect(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this._ctrlStarterSelect.OnClickStarter(this._ctrlStarterSelect.selectType);
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				this._ctrlStarterSelect.OnCancel();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				this._ctrlStarterSelect.PreparaNext(false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				this._ctrlStarterSelect.PreparaNext(true);
			}
			return false;
		}

		private void OnStarterSelected(CtrlStarterSelect.StarterType iType)
		{
			this._ctrlPartnerSelect.SetStarter(iType);
			this._clsState.Clear();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitPartnerSelect), new StatementMachine.StatementMachineUpdate(this.UpdatePartnerSelect));
		}

		private void OnStarterSelectCancel()
		{
			StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST);
		}

		private bool InitPartnerSelect(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			startupHeader.SetMessage("初期艦選択");
			this._ctrlPartnerSelect.SetActive(true);
			this._ctrlPartnerSelect.Init(new Action<ShipModelMst>(this.OnPartnerShipSelectFinished), new Action(this.OnPartnerShipCancel));
			return false;
		}

		private bool UpdatePartnerSelect(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				this._ctrlPartnerSelect.press_Button(CtrlPartnerSelect.ButtonIndex.R);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				this._ctrlPartnerSelect.press_Button(CtrlPartnerSelect.ButtonIndex.L);
			}
			else
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return this._ctrlPartnerSelect.OnDecidePartnerShip();
				}
				if (keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					return this._ctrlPartnerSelect.OnCancel();
				}
			}
			return false;
		}

		private void OnPartnerShipSelectFinished(ShipModelMst partnerShip)
		{
			XorRandom.Init(0u);
			StartupTaskManager.GetData().PartnerShipID = partnerShip.MstId;
			Observable.TimerFrame(10, FrameCountType.EndOfFrame).Subscribe(delegate(long _)
			{
				this._ctrlPartnerSelect.Hide();
				StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.PictureStoryShow);
			});
		}

		private void OnPartnerShipCancel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this._ctrlPartnerSelect.SetActive(false);
			this._shipCancelled = false;
			this._clsState.Clear();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitStarterSelect), new StatementMachine.StatementMachineUpdate(this.UpdateStarterSelect));
		}

		public void StartCache(Action Onfinished)
		{
			this.isCached = false;
			this._ctrlPartnerSelect.cachePreLoad();
		}
	}
}
