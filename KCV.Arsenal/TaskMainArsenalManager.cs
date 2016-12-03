using Common.Enum;
using KCV.PopupString;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskMainArsenalManager : SceneTaskMono
	{
		public enum State
		{
			NONE,
			KENZOU,
			KENZOU_BIG,
			KAIHATSU,
			KAITAI,
			HAIKI,
			YUSOUSEN
		}

		public enum Mode
		{
			MENU_FOCUS,
			DOCK_FOCUS,
			KENZOU_DIALOG,
			KAIHATSU_DIALOG,
			KAITAI_HAIKI_DIALOG,
			HIGHSPEED_DIALOG,
			DOCKOPEN_DIALOG,
			TANKER_DIALOG
		}

		private enum DialogType
		{
			Tanker
		}

		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		public static TaskMainArsenalManager.State StateType;

		public static ArsenalManager arsenalManager;

		public static UiArsenalDock[] dockMamager;

		public static GameObject _MainObj;

		[SerializeField]
		public UiArsenalDockOpenDialog _dockOpenDialogManager;

		[SerializeField]
		public ArsenalTankerDialog _tankerDialog;

		public int DockIndex;

		public bool _isEnd;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private ArsenalHexMenu _hexMenu;

		[SerializeField]
		private UiArsenalSpeedDialog _speedDialogManager;

		[SerializeField]
		private GameObject _bgObj;

		[SerializeField]
		private GameObject _ConstructObj;

		[SerializeField]
		private GameObject _DismantleObj;

		[SerializeField]
		private UILabel mLabel_ListHeaderCategory;

		private GameObject Tutorial;

		private int dockSelectIndex;

		private bool isDockSelect;

		private bool _isCreate;

		private BuildDockModel[] _dock;

		private KeyControl KeyController;

		private static UICamera uiCamera;

		private static bool isControl;

		public static TaskMainArsenalManager.Mode NowMode;

		private bool mNeedRefreshShipKaitaiList = true;

		private bool mNeedRefreshSlotItemKaitaiList = true;

		private Transform ArrowAchor;

		public static bool isTouchEnable
		{
			set
			{
				TaskMainArsenalManager.uiCamera.set_enabled(value);
			}
		}

		public static bool IsControl
		{
			get
			{
				return TaskMainArsenalManager.isControl;
			}
			set
			{
				TaskMainArsenalManager.isControl = value;
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = value;
			}
		}

		public TaskMainArsenalManager.Mode CurrentMode
		{
			get;
			private set;
		}

		protected override bool Init()
		{
			if (!this._isCreate)
			{
				TaskMainArsenalManager.IsControl = true;
				this._isEnd = false;
				this.isDockSelect = false;
				this.CurrentMode = TaskMainArsenalManager.Mode.MENU_FOCUS;
				this.KeyController = ArsenalTaskManager.GetKeyControl();
				TaskMainArsenalManager.arsenalManager = ArsenalTaskManager.GetLogicManager();
				TaskMainArsenalManager.dockMamager = new UiArsenalDock[4];
				this._dock = TaskMainArsenalManager.arsenalManager.GetDocks();
				TaskMainArsenalManager._MainObj = base.scenePrefab.get_gameObject();
				this._bgObj = base.get_transform().get_parent().get_parent().get_transform().FindChild("BackGroundPanel").get_gameObject();
				if (this._hexMenu == null)
				{
					this._hexMenu = this._bgObj.get_transform().FindChild("HexMenu").GetComponent<ArsenalHexMenu>();
				}
				if (this._speedDialogManager == null)
				{
					this._speedDialogManager = GameObject.Find("TaskArsenalMain/HighSpeedPanel").GetComponent<UiArsenalSpeedDialog>();
				}
				if (this._dockOpenDialogManager == null)
				{
					this._dockOpenDialogManager = GameObject.Find("TaskArsenalMain/DockOpenDialog").GetComponent<UiArsenalDockOpenDialog>();
				}
				if (this._tankerDialog == null)
				{
					this._tankerDialog = this.commonDialog.dialogMessages[0].GetComponent<ArsenalTankerDialog>();
				}
				TaskMainArsenalManager.uiCamera = GameObject.Find("Arsenal Root/Camera").GetComponent<UICamera>();
				this._hexMenu.Init();
				this._speedDialogManager.init();
				this._dockOpenDialogManager.init();
				int numOfKeyPossessions = TaskMainArsenalManager.arsenalManager.NumOfKeyPossessions;
				for (int i = 0; i < 4; i++)
				{
					TaskMainArsenalManager.dockMamager[i] = this._bgObj.get_transform().FindChild("Dock" + (i + 1)).SafeGetComponent<UiArsenalDock>();
					TaskMainArsenalManager.dockMamager[i].init(this, i);
					TaskMainArsenalManager.dockMamager[i].EnableParticles();
					if (this._dock.Length > i)
					{
						TaskMainArsenalManager.dockMamager[i]._setShow();
						TaskMainArsenalManager.dockMamager[i].HideKeyLock();
					}
				}
				for (int j = 0; j < 4; j++)
				{
					if (!TaskMainArsenalManager.dockMamager[j].SelectDockMode())
					{
						TaskMainArsenalManager.dockMamager[j].ShowKeyLock();
						break;
					}
				}
				Animation component = this._bgObj.GetComponent<Animation>();
				component.Stop();
				component.Play();
				if (SingletonMonoBehaviour<PortObjectManager>.exist())
				{
					SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
					SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
					{
						this.isCreated();
					}, true, true);
				}
				else
				{
					this.isCreated();
				}
			}
			else if (!this.isDockSelect)
			{
				this._hexMenu.UpdateButtonForcus();
			}
			if (Enumerable.Any<BuildDockModel>(this._dock, (BuildDockModel x) => x.Ship != null && x.CompleteTurn != 0))
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial, TutorialGuideManager.TutorialID.SpeedBuild, null);
			}
			TutorialModel tutorial = TaskMainArsenalManager.arsenalManager.UserInfo.Tutorial;
			if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3) && this.Tutorial == null)
			{
				this.Tutorial = Util.InstantiatePrefab("TutorialGuide/TutorialGuide3_3", base.get_gameObject(), false);
				this.DelayActionFrame(2, delegate
				{
					this.setTutorialCat();
				});
			}
			this.setTutorialVisible(true);
			TaskMainArsenalManager.IsControl = true;
			return true;
		}

		private void OnDestroy()
		{
			TaskMainArsenalManager.arsenalManager = null;
			for (int i = 0; i < TaskMainArsenalManager.dockMamager.Length; i++)
			{
				TaskMainArsenalManager.dockMamager[i] = null;
			}
			Mem.DelAry<BuildDockModel>(ref this._dock);
			this.commonDialog = null;
			Mem.DelAry<UiArsenalDock>(ref TaskMainArsenalManager.dockMamager);
			this._hexMenu = null;
			this._speedDialogManager = null;
			this._dockOpenDialogManager = null;
			this._tankerDialog = null;
			TaskMainArsenalManager._MainObj = null;
			this._bgObj = null;
			this._ConstructObj = null;
			this._DismantleObj = null;
			this.mLabel_ListHeaderCategory = null;
		}

		public void setTutorialVisible(bool isVisible)
		{
			if (this.Tutorial != null)
			{
				if (isVisible)
				{
					TweenAlpha.Begin(this.Tutorial, 0.5f, 1f);
				}
				else
				{
					TweenAlpha.Begin(this.Tutorial, 0.2f, 0f);
				}
			}
		}

		public void DestroyTutorial()
		{
			if (this.Tutorial != null)
			{
				Object.Destroy(this.Tutorial);
				Object.Destroy(this.ArrowAchor.get_gameObject());
				this.ArrowAchor = null;
				this.Tutorial = null;
			}
		}

		private void isCreated()
		{
			if (TaskMainArsenalManager.dockMamager == null)
			{
				return;
			}
			this._isCreate = true;
			UiArsenalDock[] array = TaskMainArsenalManager.dockMamager;
			for (int i = 0; i < array.Length; i++)
			{
				UiArsenalDock uiArsenalDock = array[i];
				uiArsenalDock.isCompleteVoicePlayable = true;
			}
		}

		protected override bool UnInit()
		{
			this.setTutorialVisible(false);
			return true;
		}

		public void SetNeedRefreshForShipKaitaiList(bool needRefreshKaitaiList)
		{
			this.mNeedRefreshShipKaitaiList = needRefreshKaitaiList;
		}

		internal void SetNeedRefreshForSlotItemKaitaiList(bool needRefreshKaitaiList)
		{
			this.mNeedRefreshSlotItemKaitaiList = needRefreshKaitaiList;
		}

		protected override bool Run()
		{
			if (this._isEnd)
			{
				if (this.CurrentMode == TaskMainArsenalManager.Mode.KAITAI_HAIKI_DIALOG)
				{
					ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.List);
					if (TaskMainArsenalManager.StateType == TaskMainArsenalManager.State.KAITAI)
					{
						if (this.mNeedRefreshShipKaitaiList)
						{
							ArsenalTaskManager._clsList.StartStateKaitaiAtFirst();
						}
						else
						{
							ArsenalTaskManager._clsList.StartStateKaitai();
						}
						this.mNeedRefreshShipKaitaiList = false;
					}
					else if (TaskMainArsenalManager.StateType == TaskMainArsenalManager.State.HAIKI)
					{
						if (this.mNeedRefreshSlotItemKaitaiList)
						{
							ArsenalTaskManager._clsList.StartStateHaikiAtFirst();
						}
						else
						{
							ArsenalTaskManager._clsList.StartStateHaiki();
						}
						this.mNeedRefreshSlotItemKaitaiList = false;
					}
				}
				this._isEnd = false;
				return false;
			}
			return !TaskMainArsenalManager.IsControl || this.keyControllerHandler();
		}

		private bool keyControllerHandler()
		{
			TaskMainArsenalManager.NowMode = this.CurrentMode;
			switch (this.CurrentMode)
			{
			case TaskMainArsenalManager.Mode.MENU_FOCUS:
				return this.keyControllerMenuFocus();
			case TaskMainArsenalManager.Mode.DOCK_FOCUS:
				return this.keyControllerDockFocus();
			case TaskMainArsenalManager.Mode.KENZOU_DIALOG:
				return this.keyControllerKenzouDialog();
			case TaskMainArsenalManager.Mode.KAIHATSU_DIALOG:
				return this.keyControllerKaihatsuDialog();
			case TaskMainArsenalManager.Mode.KAITAI_HAIKI_DIALOG:
				return this.keyControllerKaitaiHaikiDialog();
			case TaskMainArsenalManager.Mode.HIGHSPEED_DIALOG:
				return this.keyControllerHighspeedDialog();
			case TaskMainArsenalManager.Mode.DOCKOPEN_DIALOG:
				return this.keyControllerDockOpenDialog();
			default:
				return false;
			}
		}

		private bool keyControllerMenuFocus()
		{
			this.unFocusDock();
			if (this.KeyController.keyState.get_Item(12).down)
			{
				this._hexMenu.NextButtonForcus();
			}
			else if (this.KeyController.keyState.get_Item(8).down)
			{
				this._hexMenu.BackButtonForcus();
			}
			else
			{
				if (this.KeyController.keyState.get_Item(10).down)
				{
					this.unsetHexFocus();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return this.focusDock();
				}
				if (this.KeyController.keyState.get_Item(5).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (this.KeyController.keyState.get_Item(1).down)
				{
					this.showDialogForMenu();
				}
				else if (this.KeyController.keyState.get_Item(0).down)
				{
					return this.goPortTop();
				}
			}
			return true;
		}

		private bool keyControllerDockFocus()
		{
			if (this.KeyController.keyState.get_Item(12).down)
			{
				int num = TaskMainArsenalManager.dockMamager.Length - 1;
				if (num < 3)
				{
					num++;
				}
				if (this.dockSelectIndex < num && TaskMainArsenalManager.dockMamager[this.dockSelectIndex + 1].SelectDockMode())
				{
					this.dockSelectIndex++;
					this.updateDockSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (this.KeyController.keyState.get_Item(8).down)
			{
				if (this.dockSelectIndex > 0)
				{
					this.dockSelectIndex--;
					this.updateDockSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else
			{
				if (this.KeyController.keyState.get_Item(14).down)
				{
					this._hexMenu.UpdateButtonForcus();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return this.unFocusDock();
				}
				if (this.KeyController.keyState.get_Item(5).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else
				{
					if (this.KeyController.keyState.get_Item(0).down)
					{
						return this.goPortTop();
					}
					if (this.KeyController.keyState.get_Item(1).down)
					{
						if (TaskMainArsenalManager.dockMamager[this.dockSelectIndex].GetLockDockMode())
						{
							TaskMainArsenalManager.dockMamager[this.dockSelectIndex].DockOpenBtnEL();
						}
						else
						{
							KdockStates dockState = TaskMainArsenalManager.dockMamager[this.dockSelectIndex].GetDockState();
							if (dockState == KdockStates.EMPTY)
							{
								TaskMainArsenalManager.dockMamager[this.dockSelectIndex].DockFrameEL();
							}
							else if (dockState == KdockStates.CREATE)
							{
								TaskMainArsenalManager.dockMamager[this.dockSelectIndex].HighSpeedIconEL();
							}
							else if (dockState == KdockStates.COMPLETE)
							{
								TaskMainArsenalManager.dockMamager[this.dockSelectIndex].GetShipBtnEL();
							}
						}
						return true;
					}
				}
			}
			return true;
		}

		private bool keyControllerKenzouDialog()
		{
			if (this.KeyController.keyState.get_Item(0).down)
			{
				this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					TaskMainArsenalManager.dockMamager[i].EnableParticles();
				}
			}
			return true;
		}

		private bool keyControllerKaihatsuDialog()
		{
			if (this.KeyController.keyState.get_Item(0).down)
			{
				this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					TaskMainArsenalManager.dockMamager[i].EnableParticles();
				}
			}
			return false;
		}

		private bool keyControllerKaitaiHaikiDialog()
		{
			if (this.KeyController.keyState.get_Item(0).down)
			{
				this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
				for (int i = 0; i < 4; i++)
				{
					TaskMainArsenalManager.dockMamager[i].EnableParticles();
				}
			}
			return true;
		}

		private bool keyControllerHighspeedDialog()
		{
			if (this.KeyController.keyState.get_Item(0).down)
			{
				this.hideHighSpeedDialog();
			}
			else if (this.KeyController.keyState.get_Item(14).down)
			{
				this._speedDialogManager.updateSpeedDialogBtn(0);
			}
			else if (this.KeyController.keyState.get_Item(10).down)
			{
				this._speedDialogManager.updateSpeedDialogBtn(1);
			}
			else if (this.KeyController.keyState.get_Item(1).down)
			{
				this.StartHighSpeedProcess();
			}
			return true;
		}

		private bool keyControllerDockOpenDialog()
		{
			if (this.KeyController.keyState.get_Item(0).down)
			{
				this._dockOpenDialogManager.OnNoButtonEL(null);
			}
			else if (this.KeyController.keyState.get_Item(14).down)
			{
				this._dockOpenDialogManager.updateDialogBtn(0);
			}
			else if (this.KeyController.keyState.get_Item(10).down)
			{
				this._dockOpenDialogManager.updateDialogBtn(1);
			}
			else if (this.KeyController.keyState.get_Item(1).down)
			{
				if (this._dockOpenDialogManager.Index == 0)
				{
					this._dockOpenDialogManager.OnYesButtonEL(null);
				}
				else
				{
					this._dockOpenDialogManager.OnNoButtonEL(null);
				}
				this.hideDockOpenDialog();
			}
			return true;
		}

		public void StartHighSpeedProcess()
		{
			if (this._speedDialogManager.Index == 0)
			{
				this.highSpeedProcess();
			}
			this.hideHighSpeedDialog();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void highSpeedProcess()
		{
			for (int i = 0; i < 4; i++)
			{
				if (TaskMainArsenalManager.dockMamager[i].IsShowHigh)
				{
					TaskMainArsenalManager.arsenalManager.ChangeHighSpeed(i + 1);
					this.DockIndex = i;
					TaskMainArsenalManager.dockMamager[i].StartSpeedUpAnimate();
				}
				TaskMainArsenalManager.dockMamager[i].updateSpeedUpIcon();
				TaskMainArsenalManager.dockMamager[i].setSelect(this.DockIndex == i);
			}
			TutorialModel tutorial = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3))
			{
				tutorial.SetStepTutorialFlg(3);
				CommonPopupDialog.Instance.StartPopup("「高速建造！」 達成");
				this.DestroyTutorial();
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
		}

		private void setTutorialCat()
		{
			this.ArrowAchor = this.Tutorial.get_transform().FindChild("TutorialGuide/ArrowAchor");
			this.ArrowAchor.SetActive(true);
			int num = -1;
			for (int i = 0; i < this._dock.Length; i++)
			{
				if (this._dock[i].State == KdockStates.CREATE)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				this.ArrowAchor.SetActive(false);
				return;
			}
			TweenAlpha.Begin(this.ArrowAchor.get_gameObject(), 0.2f, 1f);
			UiArsenalDock uiArsenalDock = TaskMainArsenalManager.dockMamager[num];
			this.ArrowAchor.set_parent(uiArsenalDock.get_transform().FindChild("ButtonHight"));
			this.ArrowAchor.set_localPosition(new Vector3(-126f, 0f, 0f));
			this.ArrowAchor.localEulerAnglesY(180f);
		}

		private bool goPortTop()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			return true;
		}

		public void showDialogForMenu()
		{
			if (!TaskMainArsenalManager.IsControl)
			{
				return;
			}
			if (this.CurrentMode == TaskMainArsenalManager.Mode.DOCK_FOCUS)
			{
				this.unFocusDock();
			}
			this._hexMenu.AllButtonEnable(false);
			switch (TaskMainArsenalManager.StateType)
			{
			case TaskMainArsenalManager.State.KENZOU:
				if (!this.selectedKenzou())
				{
					this._isEnd = true;
				}
				break;
			case TaskMainArsenalManager.State.KENZOU_BIG:
				if (!this.selectedKenzou())
				{
					this._isEnd = true;
				}
				break;
			case TaskMainArsenalManager.State.KAIHATSU:
				if (!this.selectedKaihatsu())
				{
					this._isEnd = true;
				}
				break;
			case TaskMainArsenalManager.State.KAITAI:
				this.mLabel_ListHeaderCategory.text = "解体  艦種  艦名\u3000\u3000\u3000\u3000\u3000      Lv";
				if (!this.selectedKaitaiHaiki())
				{
					this._isEnd = true;
				}
				break;
			case TaskMainArsenalManager.State.HAIKI:
				this.mLabel_ListHeaderCategory.text = "廃棄      装備名\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000 レア";
				if (!this.selectedKaitaiHaiki())
				{
					this._isEnd = true;
				}
				break;
			case TaskMainArsenalManager.State.YUSOUSEN:
				if (!this.selectedKenzou())
				{
					this._isEnd = true;
				}
				break;
			}
			if (!this._isEnd)
			{
				this._hexMenu.AllButtonEnable(true);
			}
			else
			{
				TaskMainArsenalManager.IsControl = false;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void unsetHexFocus()
		{
			this._hexMenu.unsetFocus();
		}

		public bool focusDock()
		{
			this.CurrentMode = TaskMainArsenalManager.Mode.DOCK_FOCUS;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			this.isDockSelect = true;
			this.dockSelectIndex = 0;
			this.updateDockSelect();
			return true;
		}

		private bool unFocusDock()
		{
			this.CurrentMode = TaskMainArsenalManager.Mode.MENU_FOCUS;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			this.isDockSelect = false;
			this.dockSelectIndex = -1;
			this.updateDockSelect();
			return true;
		}

		public void selectDock(int index)
		{
			this.dockSelectIndex = index;
			this.updateDockSelect();
		}

		public bool hideDialog()
		{
			this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
			this._hexMenu.AllButtonEnable(true);
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			return true;
		}

		public bool selectedKenzou()
		{
			bool flag = false;
			for (int i = 0; i < 4; i++)
			{
				if (TaskMainArsenalManager.dockMamager[i].CheckStateEmpty())
				{
					this.DockIndex = i;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.CurrentMode = TaskMainArsenalManager.Mode.KENZOU_DIALOG;
				for (int j = 0; j < 4; j++)
				{
					TaskMainArsenalManager.dockMamager[j].DisableParticles();
				}
				TaskMainArsenalManager.dockMamager[this.DockIndex].setConstruct();
				return false;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByFullDeck));
			return true;
		}

		public bool selectedKaihatsu()
		{
			this.CurrentMode = TaskMainArsenalManager.Mode.KAIHATSU_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
			TaskMainArsenalManager.dockMamager[this.DockIndex].setConstruct();
			return false;
		}

		public bool selectedKaitaiHaiki()
		{
			this.CurrentMode = TaskMainArsenalManager.Mode.KAITAI_HAIKI_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
			return false;
		}

		public void showHighSpeedDialog(int dockNum)
		{
			this.CurrentMode = TaskMainArsenalManager.Mode.HIGHSPEED_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
			this._speedDialogManager.showHighSpeedDialog(dockNum);
			ArsenalTaskManager._clsArsenal.setTutorialVisible(false);
		}

		public void hideHighSpeedDialog()
		{
			this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			this._speedDialogManager.hideHighSpeedDialog();
			for (int j = 0; j < 4; j++)
			{
				if (TaskMainArsenalManager.dockMamager[j].IsShowHigh)
				{
					TaskMainArsenalManager.dockMamager[j].updateSpeedUpIcon();
					TaskMainArsenalManager.dockMamager[j].IsShowHigh = false;
					break;
				}
				TaskMainArsenalManager.dockMamager[j].setSelect(this.DockIndex == j);
			}
			ArsenalTaskManager._clsArsenal.setTutorialVisible(true);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public void showTankerDialog(int CreateNum, int beforeNum, int afterNum)
		{
			this._tankerDialog.setMessage(CreateNum, beforeNum, afterNum);
			this.commonDialog.OpenDialog(0, DialogAnimation.AnimType.POPUP);
			this.commonDialog.setCloseAction(delegate
			{
				ArsenalTaskManager._clsArsenal.setTutorialVisible(true);
			});
		}

		public void hideTankerDialog()
		{
			this.commonDialog.CloseDialog();
		}

		public void showDockOpenDialog()
		{
			ArsenalTaskManager._clsArsenal.setTutorialVisible(false);
			this.CurrentMode = TaskMainArsenalManager.Mode.DOCKOPEN_DIALOG;
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].DisableParticles();
			}
		}

		public void hideDockOpenDialog()
		{
			ArsenalTaskManager._clsArsenal.setTutorialVisible(true);
			this.CurrentMode = ((!this.isDockSelect) ? TaskMainArsenalManager.Mode.MENU_FOCUS : TaskMainArsenalManager.Mode.DOCK_FOCUS);
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
		}

		public void SetDock()
		{
			TaskMainArsenalManager.dockMamager[this.DockIndex]._setShow();
		}

		public bool IsShipGetViewAllDock()
		{
			for (int i = 0; i < 4; i++)
			{
				if (TaskMainArsenalManager.dockMamager[i] != null && TaskMainArsenalManager.dockMamager[i].IsShipGetView())
				{
					return true;
				}
			}
			return false;
		}

		private void updateDockSelect()
		{
			for (int i = 0; i < TaskMainArsenalManager.dockMamager.Length; i++)
			{
				if (TaskMainArsenalManager.dockMamager[i].SelectDockMode())
				{
					if (this.dockSelectIndex == i)
					{
						TaskMainArsenalManager.dockMamager[i].setSelect(true);
					}
					else
					{
						TaskMainArsenalManager.dockMamager[i].setSelect(false);
					}
				}
			}
		}

		public void OnClickMenuKenzou()
		{
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KENZOU;
			this.showDialogForMenu();
		}

		public void OnClickMenuKenzouBig()
		{
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KENZOU_BIG;
			this.selectedKenzou();
		}

		public void OnClickMenuKaihatsu()
		{
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KAIHATSU;
			this.showDialogForMenu();
		}

		public void OnClickMenuKaitai()
		{
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.KAITAI;
			this.selectedKaitaiHaiki();
		}

		public void OnClickMenuHaiki()
		{
			TaskMainArsenalManager.StateType = TaskMainArsenalManager.State.HAIKI;
			this.selectedKaitaiHaiki();
		}

		public bool checkDialogOpen()
		{
			bool result = this.CurrentMode != TaskMainArsenalManager.Mode.MENU_FOCUS && this.CurrentMode != TaskMainArsenalManager.Mode.DOCK_FOCUS;
			if (!TaskMainArsenalManager.IsControl)
			{
				result = false;
			}
			return result;
		}

		public bool isInConstructDialog()
		{
			return !this.checkDialogOpen() && (this._ConstructObj.get_transform().get_localPosition().x < -60f || this._ConstructObj.get_transform().get_localPosition().x > 0f);
		}
	}
}
