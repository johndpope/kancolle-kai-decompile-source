using System;
using System.Collections;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupPictureStoryShow : SceneTaskMono
	{
		private StatementMachine _clsState;

		private UITutorialConfirmDialog _uiTutorialConfirmDialog;

		private CtrlPictureStoryShow _ctrlPictureStoryShow;

		protected override bool Init()
		{
			this._clsState = new StatementMachine();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitPictureStoryShowConfirm), new StatementMachine.StatementMachineUpdate(this.UpdatePictureStoryShowConfirm));
			return true;
		}

		protected override bool UnInit()
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.DelComponentSafe<UITutorialConfirmDialog>(ref this._uiTutorialConfirmDialog);
			return true;
		}

		protected override bool Run()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF || StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.PictureStoryShow;
		}

		private bool InitPictureStoryShowConfirm(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			startupHeader.SetMessage("チュートリアル");
			navigation.Hide(null);
			this._uiTutorialConfirmDialog = UITutorialConfirmDialog.Instantiate(StartupTaskManager.GetPrefabFile().prefabUITutorialConfirmDialog.GetComponent<UITutorialConfirmDialog>(), StartupTaskManager.GetSharedPlace());
			this._uiTutorialConfirmDialog.Init(new Action(this.OnPictureStoryShowConfirmCancel), new Action<int>(this.OnPictureStoryShowConfirmDecide));
			this._uiTutorialConfirmDialog.Open(null);
			return false;
		}

		private bool UpdatePictureStoryShowConfirm(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this._uiTutorialConfirmDialog.OnDecide();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				this._uiTutorialConfirmDialog.OnCancel();
				return true;
			}
			return false;
		}

		private void OnPictureStoryShowConfirmCancel()
		{
			this._clsState.Clear();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("TutorialCancel", true);
			RetentionData.SetData(hashtable);
			this.OnPictureStoryShowFinished();
		}

		private void OnPictureStoryShowConfirmDecide(int nDecideIndex)
		{
			this._clsState.Clear();
			if (nDecideIndex == 0)
			{
				Mem.DelComponentSafe<UITutorialConfirmDialog>(ref this._uiTutorialConfirmDialog);
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitPictureStoryShowTutorial), new StatementMachine.StatementMachineUpdate(this.UpdatePictureStoryShowTutorial));
			}
			else
			{
				this.OnPictureStoryShowConfirmCancel();
			}
		}

		private bool InitPictureStoryShowTutorial(object data)
		{
			this._ctrlPictureStoryShow = CtrlPictureStoryShow.Instantiate(StartupTaskManager.GetPrefabFile().prefabCtrlPictureStoryShow.GetComponent<CtrlPictureStoryShow>(), StartupTaskManager.GetSharedPlace(), new Action(this.OnPictureStoryShowFinished));
			return false;
		}

		private bool UpdatePictureStoryShowTutorial(object data)
		{
			return true;
		}

		private void OnPictureStoryShowFinished()
		{
			this._clsState.Clear();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSecretaryShipMovie), new StatementMachine.StatementMachineUpdate(this.UpdateSecretaryShipMovie));
		}

		private bool InitSecretaryShipMovie(object data)
		{
			ProdSecretaryShipMovie prodSecretaryShipMovie = ProdSecretaryShipMovie.Instantiate(StartupTaskManager.GetPrefabFile().prefabProdSecretaryShipMovie.GetComponent<ProdSecretaryShipMovie>(), StartupTaskManager.GetSharedPlace(), StartupTaskManager.GetData().PartnerShipID);
			prodSecretaryShipMovie.Play(new Action(this.OnStartupAllFinished));
			return false;
		}

		private bool UpdateSecretaryShipMovie(object data)
		{
			return true;
		}

		private void OnStartupAllFinished()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(1f, delegate
			{
				StartupTaskManager.GetStartupHeader().get_transform().localScaleZero();
				StartupData data = StartupTaskManager.GetData();
				App.CreateSaveDataNInitialize(data.AdmiralName, data.PartnerShipID, data.Difficlty, data.isInherit);
				SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(Resources.Load("Sounds/Voice/kc9999/" + string.Format("{0:D2}", XorRandom.GetILim(206, 211))) as AudioClip, 0);
				GameObject.Find("BG Panel").get_transform().set_localScale(Vector3.get_zero());
				GameObject.Find("StartupTaskManager").get_transform().set_localScale(Vector3.get_zero());
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
			});
		}
	}
}
