using Common.Enum;
using KCV.CommandMenu;
using KCV.Display;
using KCV.Scene.Practice;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyCommandMenu : SceneTaskMono
	{
		private enum Scene
		{
			NONE,
			EXPEDISION,
			PRACTICE,
			ORGANIZE
		}

		private enum MENU_NAME
		{
			SALLY,
			MOVE,
			DEPLOY,
			ENSEI,
			ENSYU,
			INFO,
			TURNEND,
			PORT,
			_NUM
		}

		private delegate bool CommandMenuMethod();

		public GameObject ENSEI;

		[SerializeField]
		public GameObject EscortOrganize;

		[SerializeField]
		private GameObject ENSEI_Cancel;

		[SerializeField]
		private UserInterfacePracticeManager mPrefab_UserInterfacePracticeManager;

		private UserInterfacePracticeManager mUserInterfacePracticeManager;

		private StrategyTopTaskManager sttm;

		private TaskStrategySailSelect sailSelect;

		private int areaID;

		private int IndexChange;

		private bool swipeWait;

		public RotateMenu_Strategy2 CommandMenu;

		private StrategyMapManager LogicMng;

		public UILabel ShipNumberLabel;

		private MapAreaModel areaModel;

		public GameObject warningPanel;

		private bool sceneChange;

		public GameObject InfoRoot;

		public GameObject MapRoot;

		public GameObject OverView;

		public GameObject OverSceneObject;

		private StopExpedition StopExpeditionPanel;

		public KeyControl keyController;

		private UIDisplaySwipeEventRegion SwipeEvent;

		private int Debug_MstID = 1;

		private TaskStrategyCommandMenu.Scene OverScene;

		private List<TaskStrategyCommandMenu.CommandMenuMethod> pushCommandMenuList;

		private bool isKeyControlDisable;

		private TaskStrategyCommandMenu.MENU_NAME currentMenu;

		private bool isChangeingDeck;

		private bool isInfoOpenEnable;

		private float prevMoveX;

		private float prevMoveY;

		private void Awake()
		{
			this.pushCommandMenuList = new List<TaskStrategyCommandMenu.CommandMenuMethod>();
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushSally));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushMove));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushDeploy));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushExpedition));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushPractice));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushInfo));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushTurnEnd));
			this.pushCommandMenuList.Add(new TaskStrategyCommandMenu.CommandMenuMethod(this.pushPort));
		}

		protected override void Start()
		{
			this.OverScene = TaskStrategyCommandMenu.Scene.NONE;
			this.keyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.keyController.setChangeValue(-1f, 0f, 1f, 0f);
			this.keyController.KeyInputInterval = 0.2f;
			this.CommandMenu.Init(this.keyController);
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.SALLY;
			this.sailSelect = StrategyTopTaskManager.GetSailSelect();
			this.isInfoOpenEnable = true;
		}

		protected override bool Init()
		{
			if (!this.isKeyControlDisable)
			{
				this.keyController.IsRun = true;
			}
			this.IndexChange = 0;
			this.swipeWait = false;
			this.SwipeEvent = this.CommandMenu.get_transform().FindChild("AreaInfoBG").GetComponent<UIDisplaySwipeEventRegion>();
			this.SwipeEvent.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.CheckSwipe));
			this.LogicMng = StrategyTopTaskManager.GetLogicManager();
			this.CommandMenu.SetActive(true);
			Util.FindParentToChild(ref this.CommandMenu.Menus, this.CommandMenu.get_transform(), "Menus");
			this.sttm = StrategyTaskManager.GetStrategyTop();
			this.areaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			this.areaModel = StrategyTopTaskManager.GetLogicManager().Area.get_Item(this.areaID);
			this.sceneChange = true;
			this.DeckEnableCheck();
			this.CommandMenu.MenuEnter((int)this.currentMenu);
			this.keyController.Index = (int)this.currentMenu;
			KeyControlManager.Instance.KeyController = this.keyController;
			this.isInfoOpenEnable = true;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
			return true;
		}

		protected override bool Run()
		{
			if (this.OverScene != TaskStrategyCommandMenu.Scene.NONE)
			{
				return true;
			}
			if (this.StopExpeditionPanel != null)
			{
				return true;
			}
			this.keyController.Update();
			if (this.IndexChange != 0 && !this.keyController.IsChangeIndex)
			{
				this.keyController.Index += this.IndexChange;
				this.IndexChange = 0;
			}
			if (this.keyController.IsRightDown())
			{
				StrategyTopTaskManager.GetSailSelect().DeckSelectController.Index++;
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(true, true);
				if (StrategyTopTaskManager.GetSailSelect().PrevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					StrategyTopTaskManager.GetSailSelect().changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					this.DeckEnableCheck();
					this.CommandMenu.setFocus();
				}
			}
			else if (this.keyController.IsLeftDown())
			{
				StrategyTopTaskManager.GetSailSelect().DeckSelectController.Index--;
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(false, true);
				if (StrategyTopTaskManager.GetSailSelect().PrevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					StrategyTopTaskManager.GetSailSelect().changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					this.DeckEnableCheck();
					this.CommandMenu.setFocus();
				}
			}
			if (this.keyController.IsChangeIndex && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
			{
				this.CommandMenu.moveCursol();
			}
			if (this.keyController.keyState.get_Item(14).press || this.keyController.keyState.get_Item(10).press)
			{
				this.isChangeingDeck = true;
			}
			else
			{
				this.isChangeingDeck = false;
			}
			if (this.keyController.keyState.get_Item(1).down)
			{
				this.pushMenuButton();
			}
			else if (this.keyController.keyState.get_Item(0).down)
			{
				this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.SALLY;
				this.ExitCommandMenu();
			}
			if (this.keyController.keyState.get_Item(5).down)
			{
				this.pushPort();
			}
			return this.sceneChange;
		}

		public void pushMenuButton()
		{
			if (this.isChangeingDeck)
			{
				return;
			}
			if (StrategyTopTaskManager.Instance.UIModel.MapCamera.isMoving)
			{
				return;
			}
			if (!StrategyTopTaskManager.GetCommandMenu().isRun)
			{
				Debug.Log("Not CommandMenuMode return");
				return;
			}
			if (!this.CommandMenu.isOpen)
			{
				Debug.Log("Not CommandMenu Open return");
				return;
			}
			int num = this.areaModel.GetDecks().Length;
			bool flag;
			if (this.isMenuActive((TaskStrategyCommandMenu.MENU_NAME)this.keyController.Index))
			{
				Debug.Log("PUSH");
				flag = this.pushCommandMenuList.get_Item(this.keyController.Index)();
				this.CommandMenu.menuParts[this.keyController.Index].setColider(false);
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			}
			else
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonCancel2);
			}
		}

		private bool isMenuActive(TaskStrategyCommandMenu.MENU_NAME MenuName)
		{
			return this.CommandMenu.menuParts[(int)MenuName].menuState != CommandMenuParts.MenuState.Disable;
		}

		public void ExitCommandMenu()
		{
			Debug.Log("ExitCommandMenu");
			if (this.isChangeingDeck)
			{
				return;
			}
			this.CommandMenu.MenuExit();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.4f);
			this.sceneChange = false;
			SoundUtils.PlayOneShotSE(SEFIleInfos.SE_037);
		}

		public void DeckEnableCheck()
		{
			for (int i = 0; i < this.CommandMenu.menuParts.Length; i++)
			{
				this.CommandMenu.menuParts[i].SetMenuState(CommandMenuParts.MenuState.Forcus);
				this.CommandMenu.menuParts[i].SetMenuState(CommandMenuParts.MenuState.NonForcus);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.IsActionEnd() || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != MissionStates.NONE || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				this.CommandMenu.menuParts[0].SetMenuState(CommandMenuParts.MenuState.Disable);
				this.CommandMenu.menuParts[1].SetMenuState(CommandMenuParts.MenuState.Disable);
				this.CommandMenu.menuParts[4].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.IsActionEnd() || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				this.CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.STOP)
			{
				this.CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			if (StrategyAreaManager.FocusAreaID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count <= 0)
			{
				if (this.currentMenu != TaskStrategyCommandMenu.MENU_NAME.INFO)
				{
					this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.DEPLOY;
				}
				this.CommandMenu.menuParts[0].SetMenuState(CommandMenuParts.MenuState.Disable);
				this.CommandMenu.menuParts[1].SetMenuState(CommandMenuParts.MenuState.Disable);
				this.CommandMenu.menuParts[4].SetMenuState(CommandMenuParts.MenuState.Disable);
				this.CommandMenu.menuParts[3].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			MissionStates missionState = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState;
			if (missionState != MissionStates.NONE)
			{
				if (this.currentMenu != TaskStrategyCommandMenu.MENU_NAME.INFO)
				{
					this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.DEPLOY;
				}
				this.CommandMenu.menuParts[7].SetMenuState(CommandMenuParts.MenuState.Disable);
			}
			this.CommandMenu.SetMissionState(missionState);
			if (!this.CheckActiveDeckExist())
			{
				this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.TURNEND;
			}
			else if (this.currentMenu == TaskStrategyCommandMenu.MENU_NAME.TURNEND || this.currentMenu == TaskStrategyCommandMenu.MENU_NAME.MOVE)
			{
				this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.SALLY;
			}
		}

		private bool CheckActiveDeckExist()
		{
			return Enumerable.Any<DeckModel>(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks(), (DeckModel x) => !x.IsActionEnd());
		}

		private bool pushSally()
		{
			if (!this.validCheck(TaskStrategyCommandMenu.MENU_NAME.SALLY))
			{
				return false;
			}
			if (StrategyTopTaskManager.Instance.UIModel.MapCamera.GetComponent<iTween>() != null)
			{
				return true;
			}
			this.CommandMenu.MenuExit();
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(false);
			StrategyTopTaskManager.Instance.GetAreaMng().tileRouteManager.HideRoute();
			StrategyTopTaskManager.Instance.TileManager.SetVisible(false);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.MapSelect);
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(false, null);
			this.sceneChange = false;
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.SALLY;
			if (StrategyTopTaskManager.Instance.TutorialGuide6_2 != null)
			{
				StrategyTopTaskManager.Instance.TutorialGuide6_2.Hide();
			}
			return true;
		}

		private bool pushMove()
		{
			if (!this.validCheck(TaskStrategyCommandMenu.MENU_NAME.MOVE))
			{
				return false;
			}
			this.CommandMenu.MenuExit();
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(false, null);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.ShipMove);
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 14);
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.MOVE;
			this.sceneChange = false;
			return true;
		}

		private bool pushDeploy()
		{
			if (!this.validCheck(TaskStrategyCommandMenu.MENU_NAME.DEPLOY))
			{
				return false;
			}
			this.keyController.IsRun = false;
			this.CommandMenu.MenuExit();
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.DEPLOY;
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Deploy);
			Transform transform = StrategyTopTaskManager.Instance.UIModel.OverView.FindChild("Deploy");
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, false, false);
			StrategyTaskManager.setCallBack(delegate
			{
				StrategyTopTaskManager.Instance.UIModel.Character.get_transform().set_localPosition(StrategyTopTaskManager.Instance.UIModel.Character.getExitPosition());
				StrategyTopTaskManager.Instance.UIModel.Character.isEnter = false;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, true, false);
				this.keyController.IsRun = true;
			});
			if (StrategyTopTaskManager.Instance.TutorialGuide9_1 != null)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
				}
				StrategyTopTaskManager.Instance.TutorialGuide9_1.HideAndDestroy();
			}
			this.sceneChange = false;
			return true;
		}

		private bool pushInfo()
		{
			if (!this.isInfoOpenEnable)
			{
				return true;
			}
			this.CommandMenu.MenuExit();
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.INFO;
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Info);
			this.keyController.IsRun = false;
			this.sceneChange = false;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 8);
			}
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, false, false);
			StrategyTopTaskManager.GetAreaInfoTask().setExitAction(delegate
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
				{
					this.isInfoOpenEnable = false;
					StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX(), 0.4f, delegate
					{
						this.isInfoOpenEnable = true;
					});
					StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, false, false);
				}
				else
				{
					StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(delegate
					{
						this.isInfoOpenEnable = true;
					}, false, false);
				}
			});
			return true;
		}

		private bool pushPractice()
		{
			if (!this.validCheck(TaskStrategyCommandMenu.MENU_NAME.ENSYU))
			{
				return false;
			}
			this.CommandMenu.MenuExit();
			this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.ENSYU;
			StrategyTaskManager.setCallBack(delegate
			{
				if (this.mUserInterfacePracticeManager != null)
				{
					Object.Destroy(this.mUserInterfacePracticeManager.get_gameObject());
					this.mUserInterfacePracticeManager = null;
				}
				StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(false);
				this.InfoRoot.SetActive(true);
				this.MapRoot.SetActive(true);
				this.OverView.SetActive(true);
				this.OverScene = TaskStrategyCommandMenu.Scene.NONE;
				KeyControlManager.Instance.KeyController = this.keyController;
				StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(false);
				this.CommandMenu.MenuEnter(4);
				this.keyController.Index = 4;
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenIn(null, false, true);
				StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX(), 0.4f, delegate
				{
					this.keyController.IsRun = true;
				});
				StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(false);
				StrategyTopTaskManager.Instance.TileManager.setActivePositionAnimations(true);
				this.keyController.IsRun = false;
				StrategyTopTaskManager.Instance.setActiveStrategy(true);
				StrategyTopTaskManager.Instance.UIModel.Character.SetCollisionEnable(true);
			});
			StrategyTopTaskManager.Instance.UIModel.MapCamera.setBlurEnable(true);
			this.DelayActionFrame(1, delegate
			{
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, false, false);
				StrategyTopTaskManager.Instance.UIModel.Character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX() - 600f, 0.4f, delegate
				{
					this.DelayAction(0.1f, delegate
					{
						this.mUserInterfacePracticeManager = Util.Instantiate(this.mPrefab_UserInterfacePracticeManager.get_gameObject(), this.OverView, false, false).GetComponent<UserInterfacePracticeManager>();
						this.OverSceneObject = GameObject.Find("UIRoot");
					});
				});
				StrategyTopTaskManager.Instance.UIModel.Character.SetCollisionEnable(false);
			});
			this.OverScene = TaskStrategyCommandMenu.Scene.PRACTICE;
			StrategyTopTaskManager.Instance.setActiveStrategy(false);
			return true;
		}

		private bool pushExpedition()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				if (!this.validCheck(TaskStrategyCommandMenu.MENU_NAME.ENSEI))
				{
					return false;
				}
				this.CommandMenu.MenuExit();
				this.currentMenu = TaskStrategyCommandMenu.MENU_NAME.ENSEI;
				StrategyTaskManager.setCallBack(delegate
				{
					StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(false);
					StrategyTopTaskManager.Instance.GetInfoMng().updateUpperInfo();
					this.InfoRoot.SetActive(true);
					this.MapRoot.SetActive(true);
					this.OverView.SetActive(true);
					this.OverScene = TaskStrategyCommandMenu.Scene.NONE;
					KeyControlManager.Instance.KeyController = this.keyController;
					this.CommandMenu.MenuEnter(3);
					StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.setShipIconsState();
					StrategyTopTaskManager.Instance.UIModel.Character.setState(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					StrategyTopTaskManager.Instance.setActiveStrategy(true);
					SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
				});
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					this.InfoRoot.SetActive(false);
					this.OverView.SetActive(false);
					GameObject gameObject = Object.Instantiate<GameObject>(this.ENSEI);
					gameObject.get_transform().positionX(999f);
				});
				this.OverSceneObject = GameObject.Find("UIRoot");
				this.OverScene = TaskStrategyCommandMenu.Scene.EXPEDISION;
				StrategyTopTaskManager.Instance.setActiveStrategy(false);
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			else
			{
				this.StopExpeditionPanel = Util.Instantiate(this.ENSEI_Cancel, this.OverView, false, false).GetComponent<StopExpedition>();
				MissionManager missionMng = new MissionManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
				this.StopExpeditionPanel.StartPanel(missionMng);
			}
			return true;
		}

		private bool pushTurnEnd()
		{
			Debug.Log("ターンエンド");
			StrategyTopTaskManager.GetTurnEnd().TurnEnd();
			this.CommandMenu.MenuExit();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.TurnEnd);
			this.sceneChange = false;
			if (StrategyTopTaskManager.Instance.TutorialGuide8_1 != null)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
				}
				StrategyTopTaskManager.Instance.TutorialGuide8_1.HideAndDestroy();
			}
			return true;
		}

		private bool pushPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			return true;
		}

		private bool validCheck(TaskStrategyCommandMenu.MENU_NAME menuName)
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			List<IsGoCondition> list = new List<IsGoCondition>();
			switch (menuName)
			{
			case TaskStrategyCommandMenu.MENU_NAME.SALLY:
				list = currentDeck.IsValidSortie();
				break;
			case TaskStrategyCommandMenu.MENU_NAME.MOVE:
				list = currentDeck.IsValidMove();
				break;
			case TaskStrategyCommandMenu.MENU_NAME.DEPLOY:
			{
				int num = StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID;
				if (num == 15 || num == 16 || num == 17)
				{
					CommonPopupDialog.Instance.StartPopup("この海域には配備出来ません");
					return false;
				}
				break;
			}
			case TaskStrategyCommandMenu.MENU_NAME.ENSEI:
				list = currentDeck.IsValidMission();
				break;
			case TaskStrategyCommandMenu.MENU_NAME.ENSYU:
				list = currentDeck.IsValidPractice();
				break;
			}
			bool flag = list.get_Count() == 0;
			if (!flag)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(list.get_Item(0)));
			}
			else
			{
				bool flag2 = TaskStrategyCommandMenu.MENU_NAME.ENSEI == menuName;
				bool flag3 = !StrategyTopTaskManager.GetLogicManager().GetMissionAreaId().Contains(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID);
				if (flag3 && flag2)
				{
					CommonPopupDialog.Instance.StartPopup("この海域の遠征任務は解放されていません");
					return false;
				}
			}
			return flag;
		}

		private void CheckSwipe(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp || SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
			{
				this.swipeWait = false;
				this.prevMoveY = 0f;
				this.keyController.ClearKeyAll();
				return;
			}
			if (!this.swipeWait)
			{
				if (movedPercentageY - this.prevMoveY > 0.15f && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
				{
					this.IndexChange = -1;
					this.prevMoveY = movedPercentageY;
				}
				else if (movedPercentageY - this.prevMoveY < -0.15f && !SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus)
				{
					this.IndexChange = 1;
					this.prevMoveY = movedPercentageY;
				}
			}
		}

		private void OnDestroy()
		{
			this.ENSEI = null;
			this.EscortOrganize = null;
			this.mPrefab_UserInterfacePracticeManager = null;
			this.mUserInterfacePracticeManager = null;
			this.sttm = null;
			this.sailSelect = null;
			this.CommandMenu = null;
			this.LogicMng = null;
			this.ShipNumberLabel = null;
			this.areaModel = null;
			this.warningPanel = null;
			this.InfoRoot = null;
			this.MapRoot = null;
			this.OverView = null;
			this.OverSceneObject = null;
			this.StopExpeditionPanel = null;
			this.keyController = null;
			this.SwipeEvent = null;
			this.StopExpeditionPanel = null;
		}
	}
}
