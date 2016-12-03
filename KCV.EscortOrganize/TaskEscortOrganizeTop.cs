using KCV.Organize;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using Sony.Vita.Dialog;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeTop : TaskOrganizeTop
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.Strategy;

		private IOrganizeManager logicManager;

		public override int GetDeckID()
		{
			return StrategyAreaManager.FocusAreaID;
		}

		private void Start()
		{
			this.StateControllerDic = new Dictionary<string, TaskOrganizeTop.StateController>();
			this.StateControllerDic.Add("banner", new TaskOrganizeTop.StateController(this.StateKeyControl_Banner));
			this.StateControllerDic.Add("system", new TaskOrganizeTop.StateController(this.StateKeyControl_System));
			this.StateControllerDic.Add("tender", new TaskOrganizeTop.StateController(base.StateKeyControl_Tender));
			base.currentDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
			this.StartRefGet();
			this._fleetNameLabel.text = ((EscortDeckModel)base.currentDeck).Name;
			this._fleetNameLabel.supportEncoding = false;
			this.mEditName = ((EscortDeckModel)base.currentDeck).Name;
			Ime.add_OnGotIMEDialogResult(new Messages.EventHandler(base.OnGotIMEDialogResult));
			Main.Initialise();
		}

		private void StartRefGet()
		{
			GameObject gameObject = GameObject.Find("DeployOrganizePanel").get_gameObject();
			Util.FindParentToChild<UIPanel>(ref this._bgPanel, gameObject.get_transform(), "DeployOrganizeButtons");
			Util.FindParentToChild<UIPanel>(ref this._bannerPanel, gameObject.get_transform(), "Banner");
			GameObject gameObject2 = GameObject.Find("DeckManager");
			UIPanel component = this._bgPanel.get_transform().FindChild("MiscContainer").GetComponent<UIPanel>();
			Util.FindParentToChild<UILabel>(ref this._fleetNameLabel, component.get_transform(), "DeckNameLabel");
			Transform parent = this._bgPanel.get_transform().FindChild("SideButtons");
			Util.FindParentToChild<UIButton>(ref this._allUnsetBtn, parent, "AllUnsetBtn");
			Util.FindParentToChild<UIButton>(ref this._tenderBtn, parent, "TenderBtn");
			Util.FindParentToChild<UIButton>(ref this._fleetNameBtn, parent, "DeckNameBtn");
		}

		public bool FirstInit()
		{
			if (!this.IsCreate)
			{
				TaskOrganizeTop.KeyController = OrganizeTaskManager.GetKeyControl();
				TaskOrganizeTop.KeyController.IsRun = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(true);
				this.logicManager = OrganizeTaskManager.Instance.GetLogicManager();
				TaskOrganizeTop.decks = new EscortDeckModel[]
				{
					OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetEscortDeck()
				};
				ShipModel[] ships = TaskOrganizeTop.decks[0].GetShips();
				TaskOrganizeTop.allShip = this.logicManager.GetShipList();
				TaskOrganizeTop.BannerIndex = 1;
				TaskOrganizeTop.SystemIndex = 0;
				base.isControl = true;
				EscortOrganizeTaskManager._clsTop.setControlState();
				this._bannerManager = new OrganizeBannerManager[6];
				for (int i = 0; i < 6; i++)
				{
					Util.FindParentToChild<OrganizeBannerManager>(ref this._bannerManager[i], this._bannerPanel.get_transform(), "ShipSlot" + (i + 1));
					this._bannerManager[i].init(i + 1, new Predicate<OrganizeBannerManager>(this.OnCheckDragDropTarget), new Action<OrganizeBannerManager>(this.OnDragDropStart), new Predicate<OrganizeBannerManager>(this.OnDragDropRelease), new Action(this.OnDragDropEnd), false);
					if (ships.Length > i)
					{
						this._bannerManager[i].setBanner(ships[i], true, null, true);
					}
					this._bannerManager[i].UpdateBanner(false);
				}
				this._bannerManager[0].UpdateBanner(true);
				this.DelayAction(0.3f, delegate
				{
					TaskOrganizeTop.KeyController.IsRun = true;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(false);
				});
				Transform parent = this._bgPanel.get_transform().FindChild("SideButtons");
				Util.FindParentToChild<UIButton>(ref this._allUnsetBtn, parent, "AllUnsetBtn");
				Util.FindParentToChild<UIButton>(ref this._tenderBtn, parent, "TenderBtn");
				Util.FindParentToChild<UIButton>(ref this._fleetNameBtn, parent, "DeckNameBtn");
				base.currentDeck = EscortOrganizeTaskManager.GetEscortManager().EditDeck;
				base.UpdateSystemButtons();
				base.isControl = false;
				TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
				if (!SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.EscortOrganize, null, delegate
				{
					this.IsCreate = true;
					base.isControl = true;
				}))
				{
					base.isControl = true;
					this.IsCreate = true;
				}
			}
			return true;
		}

		protected override bool Init()
		{
			if (TaskOrganizeList.ListScroll.isOpen)
			{
				TaskOrganizeList.ListScroll.OnCancel();
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			if (!this.isInit)
			{
				this.Init();
				this.isInit = true;
			}
			Main.Update();
			if (this.isEnd)
			{
				if (TaskOrganizeTop.changeState == "detail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (TaskOrganizeTop.changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				this.isEnd = false;
				return false;
			}
			return !base.isControl || TaskOrganizeTop.controlState == null || !this.StateControllerDic.ContainsKey(TaskOrganizeTop.controlState) || this.StateControllerDic.get_Item(TaskOrganizeTop.controlState)();
		}

		private bool StateKeyControl_Banner()
		{
			if (TaskOrganizeTop.KeyController.IsMaruDown())
			{
				this._bannerManager[TaskOrganizeTop.BannerIndex - 1].DetailEL(null);
				return true;
			}
			if (base.isTenderAnimation())
			{
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				((EscortOrganizeTaskManager)OrganizeTaskManager.Instance).backToDeployTop();
				return false;
			}
			if (TaskOrganizeTop.KeyController.IsShikakuDown())
			{
				SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
				base.AllUnsetBtnEL();
			}
			else if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				TaskOrganizeTop.BannerIndex += 2;
				if (TaskOrganizeTop.BannerIndex >= 7)
				{
					TaskOrganizeTop.BannerIndex -= 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				base.UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				TaskOrganizeTop.BannerIndex -= 2;
				if (TaskOrganizeTop.BannerIndex <= 0)
				{
					TaskOrganizeTop.BannerIndex += 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				base.UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsLeftDown())
			{
				TaskOrganizeTop.BannerIndex--;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (TaskOrganizeTop.BannerIndex == 0)
				{
					if (base.IsTenderBtn() || base.IsAllUnsetBtn())
					{
						bool flag = false;
						if (base.IsTenderBtn())
						{
							TaskOrganizeTop.SystemIndex = 0;
							flag = true;
						}
						if (!flag)
						{
							TaskOrganizeTop.SystemIndex = 1;
						}
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					base.UpdateSystemButtons();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				base.UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				if (TaskOrganizeTop.BannerIndex >= 6)
				{
					return true;
				}
				TaskOrganizeTop.BannerIndex++;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				for (int i = 0; i < 6; i++)
				{
					if (TaskOrganizeTop.BannerIndex - 1 == i)
					{
						this._bannerManager[i].UpdateBanner(true);
					}
					else
					{
						this._bannerManager[i].UpdateBanner(false);
					}
				}
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private bool StateKeyControl_System()
		{
			if (TaskOrganizeTop.KeyController.IsMaruDown() && TaskOrganizeTop.controlState == "system")
			{
				if (TaskOrganizeTop.SystemIndex == 0)
				{
					this.TenderManager.ShowSelectTender();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else if (TaskOrganizeTop.SystemIndex == 1)
				{
					base.AllUnsetBtnEL();
					TaskOrganizeTop.SystemIndex = 0;
					TaskOrganizeTop.BannerIndex = 1;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					base.UpdateChangeBanner();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else
				{
					Debug.Log("openDeckNameInput");
					base.openDeckNameInput();
				}
				return true;
			}
			if (base.isTenderAnimation())
			{
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				((EscortOrganizeTaskManager)OrganizeTaskManager.Instance).backToDeployTop();
				return false;
			}
			if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				if (TaskOrganizeTop.SystemIndex >= 2)
				{
					TaskOrganizeTop.SystemIndex = 2;
					return true;
				}
				int systemIndex = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex++;
				if (TaskOrganizeTop.SystemIndex == 1 && !base.IsAllUnsetBtn())
				{
					TaskOrganizeTop.SystemIndex = 2;
				}
				base.UpdateSystemButtons();
				if (systemIndex != TaskOrganizeTop.SystemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				if (TaskOrganizeTop.SystemIndex <= 0)
				{
					Debug.Log("KEY:" + TaskOrganizeTop.KeyController.Index);
					TaskOrganizeTop.SystemIndex = 0;
					return true;
				}
				int systemIndex2 = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex--;
				if (TaskOrganizeTop.SystemIndex == 1 && !base.IsAllUnsetBtn())
				{
					if (!base.IsTenderBtn())
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 0;
					}
				}
				if (TaskOrganizeTop.SystemIndex == 0 && !base.IsTenderBtn())
				{
					if (!base.IsAllUnsetBtn())
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 1;
					}
				}
				Debug.Log(string.Concat(new object[]
				{
					"KEY:",
					TaskOrganizeTop.KeyController.Index,
					" KEE:",
					TaskOrganizeTop.BannerIndex
				}));
				base.UpdateSystemButtons();
				if (systemIndex2 != TaskOrganizeTop.SystemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				TaskOrganizeTop.BannerIndex++;
				base.UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				base.UpdateChangeBanner();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public void UnVisibleEmptyFrame()
		{
			for (int i = 0; i < 6; i++)
			{
				if (!this._bannerManager[i].IsSet)
				{
					this._bannerManager[i].SetShipFrameActive(false);
				}
			}
		}

		public override void UpdateDeckSwitchManager()
		{
		}

		private bool OnCheckDragDropTarget(OrganizeBannerManager target)
		{
			return false;
		}

		private void OnDragDropStart(OrganizeBannerManager target)
		{
		}

		private bool OnDragDropRelease(OrganizeBannerManager target)
		{
			OrganizeManager organizeManager = OrganizeTaskManager.logicManager;
			DeckModel currentDeck = this.deckSwitchManager.currentDeck;
			ShipModel ship = target.ship;
			ShipModel ship2 = this._uiDragDropItem.ship;
			if (organizeManager.ChangeOrganize(currentDeck.Id, target.number - 1, ship2.MemId))
			{
				target.setBanner(ship2, true, null, false);
				this._uiDragDropItem.setBanner(ship, true, delegate
				{
					this.OnDragDropEnd();
				}, false);
			}
			return true;
		}

		private void OnDragDropEnd()
		{
		}

		private void OnDestroy()
		{
			this._bgPanel = null;
			this._bannerPanel = null;
			this._allUnsetBtn = null;
			this._tenderBtn = null;
			this._fleetNameBtn = null;
			this._fleetNameLabel = null;
			this.mTransform_TurnEndStamp = null;
			this.deckIcon = null;
			Mem.DelDictionarySafe<string, TaskOrganizeTop.StateController>(ref this.StateControllerDic);
			this._bannerManager = null;
			TaskOrganizeTop.SystemIndex = 0;
			TaskOrganizeTop.prevControlState = string.Empty;
			TaskOrganizeTop.changeState = string.Empty;
			Mem.Del<int>(ref TaskOrganizeTop.BannerIndex);
			Mem.Del<string>(ref TaskOrganizeTop.controlState);
			this.uiCamera = null;
			this.TenderManager = null;
			base.currentDeck = null;
			this.deckSwitchManager = null;
			TaskOrganizeTop.decks = null;
			TaskOrganizeTop.allShip = null;
			TaskOrganizeTop.KeyController = null;
		}
	}
}
