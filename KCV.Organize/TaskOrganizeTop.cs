using Common.Enum;
using DG.Tweening;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using Sony.Vita.Dialog;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeTop : SceneTaskMono, CommonDeckSwitchHandler
	{
		public enum OrganizeState
		{
			None = -1,
			Top,
			Detail,
			DetailList,
			List,
			System,
			Tender
		}

		protected delegate bool StateController();

		[SerializeField]
		protected UIPanel _bgPanel;

		[SerializeField]
		protected UIPanel _bannerPanel;

		[SerializeField]
		protected UIButton _allUnsetBtn;

		[SerializeField]
		protected UIButton _tenderBtn;

		[SerializeField]
		protected UIButton _fleetNameBtn;

		[SerializeField]
		protected UILabel _fleetNameLabel;

		[SerializeField]
		protected Transform mTransform_TurnEndStamp;

		[SerializeField]
		private UIDisplaySwipeEventRegion _displaySwipeEventRegion;

		[SerializeField]
		private OrganizeDeckChangeArrows deckChangeArrows;

		protected bool isInit;

		protected bool isInitTender;

		protected bool isInitDetail;

		protected bool isInitChangeList;

		protected bool IsCreate;

		protected bool isEnd;

		protected string mEditName = string.Empty;

		protected UISprite deckIcon;

		protected Dictionary<string, TaskOrganizeTop.StateController> StateControllerDic;

		protected OrganizeBannerManager[] _bannerManager;

		protected static int SystemIndex;

		protected static string prevControlState;

		protected static string changeState;

		private TaskOrganizeTop.OrganizeState _state;

		public static int BannerIndex;

		public static string controlState;

		public UICamera uiCamera;

		public OrganizeTender TenderManager;

		[SerializeField]
		public CommonDeckSwitchManager deckSwitchManager;

		public static DeckModelBase[] decks;

		public static ShipModel[] allShip;

		public static KeyControl KeyController;

		[SerializeField]
		private UIButtonManager BtnManager;

		public TaskOrganizeTop.OrganizeState _state2;

		protected OrganizeBannerManager _uiDragDropItem;

		private bool _isDragDrop;

		public bool isControl
		{
			get;
			set;
		}

		public DeckModelBase currentDeck
		{
			get;
			protected set;
		}

		public virtual int GetDeckID()
		{
			return ((DeckModel)this.currentDeck).Id;
		}

		private void Start()
		{
			this._displaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnSwipeEvent));
			this.StateControllerDic = new Dictionary<string, TaskOrganizeTop.StateController>();
			this.StateControllerDic.Add("banner", new TaskOrganizeTop.StateController(this.StateKeyControl_Banner));
			this.StateControllerDic.Add("system", new TaskOrganizeTop.StateController(this.StateKeyControl_System));
			this.StateControllerDic.Add("tender", new TaskOrganizeTop.StateController(this.StateKeyControl_Tender));
			Main.Initialise();
		}

		public bool FirstInit()
		{
			if (!this.IsCreate)
			{
				TaskOrganizeTop.KeyController = OrganizeTaskManager.GetKeyControl();
				GameObject gameObject = GameObject.Find("OrganizeRoot").get_gameObject();
				Util.FindParentToChild<UIPanel>(ref this._bgPanel, gameObject.get_transform(), "BackGround");
				Util.FindParentToChild<UIPanel>(ref this._bannerPanel, gameObject.get_transform(), "Banner");
				TaskOrganizeTop.decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
				TaskOrganizeTop.allShip = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
				TaskOrganizeTop.BannerIndex = 1;
				TaskOrganizeTop.SystemIndex = 0;
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				this._bannerManager = new OrganizeBannerManager[6];
				for (int i = 0; i < 6; i++)
				{
					Util.FindParentToChild<OrganizeBannerManager>(ref this._bannerManager[i], this._bannerPanel.get_transform(), "ShipSlot" + (i + 1));
					this._bannerManager[i].init(i + 1, new Predicate<OrganizeBannerManager>(this.OnCheckDragDropTarget), new Action<OrganizeBannerManager>(this.OnDragDropStart), new Predicate<OrganizeBannerManager>(this.OnDragDropRelease), new Action(this.OnDragDropEnd), true);
				}
				Transform parent = this._bgPanel.get_transform().FindChild("SideButtons");
				Util.FindParentToChild<UIButton>(ref this._allUnsetBtn, parent, "AllUnsetBtn");
				Util.FindParentToChild<UIButton>(ref this._tenderBtn, parent, "TenderBtn");
				Util.FindParentToChild<UIButton>(ref this._fleetNameBtn, parent, "DeckNameBtn");
				UIPanel component = this._bgPanel.get_transform().FindChild("MiscContainer").GetComponent<UIPanel>();
				Util.FindParentToChild<UILabel>(ref this._fleetNameLabel, component.get_transform(), "DeckNameLabel");
				this._fleetNameLabel.supportEncoding = false;
				Util.FindParentToChild<UISprite>(ref this.deckIcon, component.get_transform(), "DeckIcon");
				DeckModel[] array = new DeckModel[TaskOrganizeTop.decks.Length];
				TaskOrganizeTop.decks.CopyTo(array, 0);
				DeckModel deckModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				if (deckModel == null)
				{
					deckModel = array[0];
				}
				this.deckSwitchManager.Init((OrganizeManager)OrganizeTaskManager.Instance.GetLogicManager(), array, this, TaskOrganizeTop.KeyController, false, deckModel, 50);
				this.deckChangeArrows.UpdateView();
				this._bannerManager.ForEach(delegate(OrganizeBannerManager e)
				{
					e.updateBannerWhenShipExist(true, false);
				});
				this.UpdateSystemButtons();
				this.UpdeteDeckIcon();
				this.IsCreate = true;
				this.CreateTender();
				this._isDragDrop = false;
			}
			return true;
		}

		protected override bool UnInit()
		{
			this._isDragDrop = false;
			return true;
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
			this._displaySwipeEventRegion = null;
			this.deckChangeArrows = null;
			this.deckIcon = null;
			Mem.DelDictionarySafe<string, TaskOrganizeTop.StateController>(ref this.StateControllerDic);
			this._bannerManager = null;
			TaskOrganizeTop.SystemIndex = 0;
			TaskOrganizeTop.prevControlState = string.Empty;
			TaskOrganizeTop.changeState = string.Empty;
			this._state = TaskOrganizeTop.OrganizeState.Top;
			Mem.Del<int>(ref TaskOrganizeTop.BannerIndex);
			Mem.Del<string>(ref TaskOrganizeTop.controlState);
			this.uiCamera = null;
			this.TenderManager = null;
			this.currentDeck = null;
			this.deckSwitchManager = null;
			TaskOrganizeTop.decks = null;
			TaskOrganizeTop.allShip = null;
			TaskOrganizeTop.KeyController = null;
		}

		protected override bool Run()
		{
			Main.Update();
			if (this.isEnd)
			{
				if (TaskOrganizeTop.changeState == "detail")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
					this._state2 = TaskOrganizeTop.OrganizeState.Detail;
				}
				else if (TaskOrganizeTop.changeState == "list")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
					this._state2 = TaskOrganizeTop.OrganizeState.List;
				}
				this.isEnd = false;
				return false;
			}
			if (TaskOrganizeTop.controlState != null)
			{
				if (this.isTenderAnimation())
				{
					return true;
				}
				switch (this._state)
				{
				case TaskOrganizeTop.OrganizeState.Top:
					this._state2 = TaskOrganizeTop.OrganizeState.Top;
					return this.StateKeyControl_Banner();
				case TaskOrganizeTop.OrganizeState.System:
					this._state2 = TaskOrganizeTop.OrganizeState.System;
					return this.StateKeyControl_System();
				case TaskOrganizeTop.OrganizeState.Tender:
					this._state2 = TaskOrganizeTop.OrganizeState.Tender;
					return this.StateKeyControl_Tender();
				}
			}
			return true;
		}

		protected bool StateKeyControl_Banner()
		{
			if (this._isDragDrop)
			{
				return true;
			}
			this.deckSwitchManager.keyControlEnable = true;
			if (TaskOrganizeTop.KeyController.IsMaruDown())
			{
				this._bannerManager[TaskOrganizeTop.BannerIndex - 1].DetailEL(null);
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				this.BackToPort();
			}
			else if (TaskOrganizeTop.KeyController.IsShikakuDown())
			{
				this.AllUnsetBtnEL();
			}
			else if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				TaskOrganizeTop.BannerIndex += 2;
				if (TaskOrganizeTop.BannerIndex >= 7)
				{
					TaskOrganizeTop.BannerIndex -= 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				TaskOrganizeTop.BannerIndex -= 2;
				if (TaskOrganizeTop.BannerIndex <= 0)
				{
					TaskOrganizeTop.BannerIndex += 6;
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.UpdateChangeBanner();
			}
			else if (TaskOrganizeTop.KeyController.IsLeftDown())
			{
				TaskOrganizeTop.BannerIndex--;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (TaskOrganizeTop.BannerIndex == 0)
				{
					if (this.IsTenderBtn() || this.IsAllUnsetBtn())
					{
						TaskOrganizeTop.SystemIndex = ((!this.IsTenderBtn()) ? 1 : 0);
					}
					else
					{
						TaskOrganizeTop.SystemIndex = 2;
					}
					this.UpdateSystemButtons();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				this.UpdateChangeBanner();
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
					bool enabled = TaskOrganizeTop.BannerIndex - 1 == i;
					this._bannerManager[i].UpdateBanner(enabled);
				}
			}
			return true;
		}

		protected bool StateKeyControl_System()
		{
			if (this._isDragDrop)
			{
				return true;
			}
			this.deckSwitchManager.keyControlEnable = true;
			if (TaskOrganizeTop.KeyController.IsMaruDown())
			{
				if (TaskOrganizeTop.SystemIndex == 0)
				{
					this.TenderManager.ShowSelectTender();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else if (TaskOrganizeTop.SystemIndex == 1)
				{
					this.AllUnsetBtnEL();
					TaskOrganizeTop.SystemIndex = 0;
					TaskOrganizeTop.BannerIndex = 1;
					this.UpdateChangeBanner();
					OrganizeTaskManager.Instance.GetTopTask().setControlState();
				}
				else
				{
					this.openDeckNameInput();
				}
				return true;
			}
			if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				this.BackToPort();
			}
			else if (TaskOrganizeTop.KeyController.IsDownDown())
			{
				if (TaskOrganizeTop.SystemIndex >= 2)
				{
					return true;
				}
				int systemIndex = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex++;
				if (TaskOrganizeTop.SystemIndex == 1 && !this.IsAllUnsetBtn())
				{
					TaskOrganizeTop.SystemIndex = 2;
				}
				this.UpdateSystemButtons();
				if (TaskOrganizeTop.SystemIndex != systemIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsUpDown())
			{
				if (TaskOrganizeTop.SystemIndex <= 0)
				{
					return true;
				}
				int systemIndex2 = TaskOrganizeTop.SystemIndex;
				TaskOrganizeTop.SystemIndex--;
				if (TaskOrganizeTop.SystemIndex == 1 && !this.IsAllUnsetBtn())
				{
					TaskOrganizeTop.SystemIndex = (this.IsTenderBtn() ? 0 : 2);
				}
				if (TaskOrganizeTop.SystemIndex == 0 && !this.IsTenderBtn())
				{
					TaskOrganizeTop.SystemIndex = (this.IsAllUnsetBtn() ? 1 : 2);
				}
				this.UpdateSystemButtons();
				if (TaskOrganizeTop.SystemIndex != systemIndex2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				TaskOrganizeTop.BannerIndex++;
				this.UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.UpdateChangeBanner();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
			return true;
		}

		protected bool StateKeyControl_Tender()
		{
			if (TaskOrganizeTop.KeyController.IsMaruDown())
			{
				if (this.TenderManager.State == OrganizeTender.TenderState.Select)
				{
					this.TenderManager.ShowUseDialog();
				}
				else if (this.TenderManager.setIndex2 == 1)
				{
					this.TenderManager.OtherBackEL(null);
				}
				else
				{
					this.TenderManager.BtnYesEL(null);
					TaskOrganizeTop.SystemIndex = 0;
					this.SetTenderBtn();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.UpdateChangeBanner();
				}
			}
			else if (TaskOrganizeTop.KeyController.IsBatuDown())
			{
				if (this.TenderManager.State == OrganizeTender.TenderState.Select)
				{
					this.TenderManager.MainBackEL(null);
				}
				else
				{
					this.TenderManager.OtherBackEL(null);
				}
			}
			else if (TaskOrganizeTop.KeyController.IsLeftDown())
			{
				this.FlickLeftSweet();
			}
			else if (TaskOrganizeTop.KeyController.IsRightDown())
			{
				this.FlickRightSweet();
			}
			else if (TaskOrganizeTop.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public void ShowBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				this._bannerManager[i].Show(i);
			}
		}

		public void UpdateAllBannerByChangeShip()
		{
			TaskOrganizeTop.decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = this.currentDeck.GetShips();
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				if (ships.Length > i)
				{
					bool flag2 = !this._bannerManager[i].IsSetShip();
					if (flag2)
					{
						flag = true;
					}
					this._bannerManager[i].setBanner(ships[i], flag2, new Action(this.compBannerChange), false);
				}
				else
				{
					this._bannerManager[i].InitBanner(false);
				}
			}
			if (ships.Length == 0 || !flag)
			{
				this.compBannerChange();
			}
			this.UpdateSystemButtons();
		}

		private void compBannerChange()
		{
			this.isControl = true;
		}

		public void UpdateBanner(int bannerNum)
		{
			TaskOrganizeTop.decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = this.currentDeck.GetShips();
			if (ships.Length > bannerNum - 1)
			{
				this._bannerManager[bannerNum - 1].setBanner(ships[bannerNum - 1], true, null, false);
			}
			else
			{
				this._bannerManager[bannerNum - 1].InitBanner(false);
			}
			this.UpdateSystemButtons();
		}

		public void UpdateChangeBanner(int bannerNum)
		{
			TaskOrganizeTop.decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = this.currentDeck.GetShips();
			if (ships.Length > bannerNum - 1)
			{
				this._bannerManager[bannerNum - 1].ChangeBanner(ships[bannerNum - 1]);
			}
			else
			{
				this._bannerManager[bannerNum - 1].InitChangeBanner(false);
			}
			this.UpdateSystemButtons();
		}

		public void UpdateAllBannerByRemoveShip(bool allReset)
		{
			TaskOrganizeTop.decks = OrganizeTaskManager.Instance.GetLogicManager().MapArea.GetDecks();
			ShipModel[] ships = this.currentDeck.GetShips();
			for (int i = 0; i < 6; i++)
			{
				if (ships.Length > i)
				{
					this._bannerManager[i].setBanner(ships[i], false, null, false);
				}
				else
				{
					this._bannerManager[i].InitBanner(allReset || i == ships.Length);
				}
			}
			this.UpdateSystemButtons();
		}

		public void UpdateAllSelectBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				this._bannerManager[i].UpdateBanner(TaskOrganizeTop.BannerIndex - 1 == i);
			}
			this.UpdateSystemButtons();
		}

		public void SetAllUnsetBtn()
		{
			if (this.IsAllUnsetBtn())
			{
				this._allUnsetBtn.isEnabled = true;
				if (TaskOrganizeTop.SystemIndex == 1 && TaskOrganizeTop.BannerIndex == 0)
				{
					this._allUnsetBtn.SetState(UIButtonColor.State.Hover, true);
					UISelectedObject.SelectedOneButtonZoomUpDown(this._allUnsetBtn.get_gameObject(), true);
				}
				else
				{
					this._allUnsetBtn.SetState(UIButtonColor.State.Normal, true);
					UISelectedObject.SelectedOneButtonZoomUpDown(this._allUnsetBtn.get_gameObject(), false);
				}
			}
			else
			{
				this._allUnsetBtn.isEnabled = false;
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allUnsetBtn.get_gameObject(), false);
			}
		}

		public bool IsAllUnsetBtn()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUnsetAll(this.GetDeckID());
		}

		public void SetTenderBtn()
		{
			if (this.IsTenderBtn())
			{
				this._tenderBtn.isEnabled = true;
				if (TaskOrganizeTop.SystemIndex == 0 && TaskOrganizeTop.BannerIndex == 0)
				{
					this._tenderBtn.SetState(UIButtonColor.State.Hover, false);
					UISelectedObject.SelectedOneButtonZoomUpDown(this._tenderBtn.get_gameObject(), true);
				}
				else
				{
					this._tenderBtn.SetState(UIButtonColor.State.Normal, false);
					UISelectedObject.SelectedOneButtonZoomUpDown(this._tenderBtn.get_gameObject(), false);
				}
			}
			else
			{
				this._tenderBtn.isEnabled = false;
				UISelectedObject.SelectedOneButtonZoomUpDown(this._tenderBtn.get_gameObject(), false);
				if (TaskOrganizeTop.SystemIndex == 0 && TaskOrganizeTop.BannerIndex == 0)
				{
					TaskOrganizeTop.BannerIndex = 1;
					this.setControlState();
				}
			}
		}

		public void SetFleetNameBtn()
		{
			if (TaskOrganizeTop.SystemIndex == 2 && TaskOrganizeTop.BannerIndex == 0)
			{
				this._fleetNameBtn.SetState(UIButtonColor.State.Hover, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._fleetNameBtn.get_gameObject(), true);
			}
			else
			{
				this._fleetNameBtn.SetState(UIButtonColor.State.Normal, true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._fleetNameBtn.get_gameObject(), false);
			}
		}

		public bool IsTenderBtn()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUseSweets(this.GetDeckID());
		}

		public void UpdateSort(SortKey nowSortKey)
		{
			switch (OrganizeTaskManager.Instance.GetLogicManager().NowSortKey)
			{
			case SortKey.LEVEL:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.LEVEL);
				break;
			case SortKey.SHIPTYPE:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.SHIPTYPE);
				break;
			case SortKey.DAMAGE:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.DAMAGE);
				break;
			case SortKey.NEW:
				OrganizeTaskManager.Instance.GetLogicManager().ChangeSortKey(SortKey.NEW);
				break;
			}
			TaskOrganizeTop.allShip = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
		}

		public void UpdateShipLock(int shipNumber)
		{
			OrganizeTaskManager.Instance.GetLogicManager().Lock(shipNumber);
		}

		public void UpdateChangeBanner()
		{
			for (int i = 0; i < 6; i++)
			{
				this._bannerManager[i].UpdateBanner(TaskOrganizeTop.BannerIndex - 1 == i);
			}
		}

		public void UpdateChangeFatigue()
		{
			this.UpdateChangeBanner();
			for (int i = 0; i < 6; i++)
			{
				if (this.currentDeck.GetShips().Length > i)
				{
					this._bannerManager[i].UpdateBannerFatigue();
				}
			}
		}

		public void AllUnsetBtnEL()
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && this.IsAllUnsetBtn())
			{
				OrganizeTaskManager.Instance.GetLogicManager().UnsetAllOrganize(this.GetDeckID());
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
				this.UpdateAllBannerByRemoveShip(true);
				this.UpdateSystemButtons();
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
			}
		}

		public void TenderBtnEL()
		{
			this.CreateTender();
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && this.IsTenderBtn())
			{
				TaskOrganizeTop.BannerIndex = 0;
				TaskOrganizeTop.SystemIndex = 0;
				this.UpdateSystemButtons();
				this.UpdateChangeBanner();
				this.TenderManager.ShowSelectTender();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				this._state = TaskOrganizeTop.OrganizeState.Tender;
			}
		}

		public void ChangeDeckAnimate(int num)
		{
			this.UpdateChangeBanner(num);
			this._bannerManager[num - 1].UpdateBanner(num - 1 == 0);
		}

		public void BannerPanlAlpha(float from, float to)
		{
			this._bannerPanel.SafeGetTweenAlpha(from, to, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._bannerPanel.get_gameObject(), string.Empty);
		}

		public void CreateTender()
		{
			if (!this.isInitTender)
			{
				this.TenderManager = GameObject.Find("Tender").SafeGetComponent<OrganizeTender>();
				this.TenderManager.init();
				this.isInitTender = true;
			}
		}

		public void UpdeteDeckIcon()
		{
			this.deckIcon.spriteName = "icon_deck" + this.GetDeckID();
		}

		public void openDeckNameInput()
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && base.isRun)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_001);
				this._state = TaskOrganizeTop.OrganizeState.System;
				TaskOrganizeTop.controlState = "system";
				TaskOrganizeTop.BannerIndex = 0;
				TaskOrganizeTop.SystemIndex = 2;
				this.UpdateSystemButtons();
				this.UpdateChangeBanner();
				App.OnlyController = TaskOrganizeTop.KeyController;
				TaskOrganizeTop.KeyController.IsRun = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(true);
				this.DelayActionFrame(1, delegate
				{
					Ime.add_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
					Ime.ImeDialogParams imeDialogParams = new Ime.ImeDialogParams();
					imeDialogParams.supportedLanguages = 270336;
					imeDialogParams.languagesForced = true;
					imeDialogParams.type = 0;
					imeDialogParams.option = 0;
					imeDialogParams.canCancel = true;
					imeDialogParams.textBoxMode = 2;
					imeDialogParams.enterLabel = 0;
					imeDialogParams.maxTextLength = 12;
					imeDialogParams.set_title("艦隊名を入力してください。（12文字まで）");
					imeDialogParams.set_initialText(this.mEditName);
					Ime.Open(imeDialogParams);
				});
			}
		}

		protected void OnGotIMEDialogResult(Messages.PluginMessage msg)
		{
			Ime.remove_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
			Ime.ImeDialogResult result = Ime.GetResult();
			DebugUtils.SLog("OnGotIMEDialogResult2");
			DebugUtils.SLog("OnGotIMEDialogResult3");
			if (result.result == null)
			{
				DebugUtils.SLog("OnGotIMEDialogResult4");
				DebugUtils.SLog("★ IME から得た名前： result.text: " + result.get_text());
				DebugUtils.SLog("   名前の長さ：result.text.Length: " + result.get_text().get_Length());
				DebugUtils.SLog("OnGotIMEDialogResult5");
				this.mEditName = result.get_text();
				this._fleetNameLabel.text = this.mEditName;
				DebugUtils.SLog("OnGotIMEDialogResult6");
				OrganizeTaskManager.Instance.GetLogicManager().ChangeDeckName(this.GetDeckID(), result.get_text());
				DebugUtils.SLog("OnGotIMEDialogResult7");
			}
			DebugUtils.SLog("OnGotIMEDialogResult8");
			App.OnlyController = null;
			TaskOrganizeTop.KeyController.IsRun = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(false);
		}

		protected void UpdateDeckName()
		{
			this._fleetNameLabel.text = this.currentDeck.Name;
			this.mEditName = this.currentDeck.Name;
		}

		public void BackToPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void setChangePhase(string state)
		{
			TaskOrganizeTop.changeState = state;
			this.isEnd = true;
		}

		public void compFadeAnimation()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.SetActive(true);
			AsyncLoadScene.LoadLevelAsyncScene(this, Generics.Scene.PortTop, null);
		}

		protected string han2zen(string value)
		{
			string text = value;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(" ", "\u3000");
			dictionary.Add("!", "！");
			dictionary.Add("\"", "”");
			dictionary.Add("#", "＃");
			dictionary.Add("$", "＄");
			dictionary.Add("%", "％");
			dictionary.Add("&", "＆");
			dictionary.Add("'", "’");
			dictionary.Add("(", "（");
			dictionary.Add(")", "）");
			dictionary.Add("*", "＊");
			dictionary.Add("+", "＋");
			dictionary.Add(",", "，");
			dictionary.Add("-", "－");
			dictionary.Add(".", "．");
			dictionary.Add("/", "／");
			dictionary.Add("0", "０");
			dictionary.Add("1", "１");
			dictionary.Add("2", "２");
			dictionary.Add("3", "３");
			dictionary.Add("4", "４");
			dictionary.Add("5", "５");
			dictionary.Add("6", "６");
			dictionary.Add("7", "７");
			dictionary.Add("8", "８");
			dictionary.Add("9", "９");
			dictionary.Add(":", "：");
			dictionary.Add(";", "；");
			dictionary.Add("<", "＜");
			dictionary.Add("=", "＝");
			dictionary.Add(">", "＞");
			dictionary.Add("?", "？");
			dictionary.Add("@", "＠");
			dictionary.Add("A", "Ａ");
			dictionary.Add("B", "Ｂ");
			dictionary.Add("C", "Ｃ");
			dictionary.Add("D", "Ｄ");
			dictionary.Add("E", "Ｅ");
			dictionary.Add("F", "Ｆ");
			dictionary.Add("G", "Ｇ");
			dictionary.Add("H", "Ｈ");
			dictionary.Add("I", "Ｉ");
			dictionary.Add("J", "Ｊ");
			dictionary.Add("K", "Ｋ");
			dictionary.Add("L", "Ｌ");
			dictionary.Add("M", "Ｍ");
			dictionary.Add("N", "Ｎ");
			dictionary.Add("O", "Ｏ");
			dictionary.Add("P", "Ｐ");
			dictionary.Add("Q", "Ｑ");
			dictionary.Add("R", "Ｒ");
			dictionary.Add("S", "Ｓ");
			dictionary.Add("T", "Ｔ");
			dictionary.Add("U", "Ｕ");
			dictionary.Add("V", "Ｖ");
			dictionary.Add("W", "Ｗ");
			dictionary.Add("X", "Ｘ");
			dictionary.Add("Y", "Ｙ");
			dictionary.Add("Z", "Ｚ");
			dictionary.Add("[", "［");
			dictionary.Add("\\", "￥");
			dictionary.Add("]", "］");
			dictionary.Add("^", "＾");
			dictionary.Add("_", "＿");
			dictionary.Add("`", "‘");
			dictionary.Add("a", "ａ");
			dictionary.Add("b", "ｂ");
			dictionary.Add("c", "ｃ");
			dictionary.Add("d", "ｄ");
			dictionary.Add("e", "ｅ");
			dictionary.Add("f", "ｆ");
			dictionary.Add("g", "ｇ");
			dictionary.Add("h", "ｈ");
			dictionary.Add("i", "ｉ");
			dictionary.Add("j", "ｊ");
			dictionary.Add("k", "ｋ");
			dictionary.Add("l", "ｌ");
			dictionary.Add("m", "ｍ");
			dictionary.Add("n", "ｎ");
			dictionary.Add("o", "ｏ");
			dictionary.Add("p", "ｐ");
			dictionary.Add("q", "ｑ");
			dictionary.Add("r", "ｒ");
			dictionary.Add("s", "ｓ");
			dictionary.Add("t", "ｔ");
			dictionary.Add("u", "ｕ");
			dictionary.Add("v", "ｖ");
			dictionary.Add("w", "ｗ");
			dictionary.Add("x", "ｘ");
			dictionary.Add("y", "ｙ");
			dictionary.Add("z", "ｚ");
			dictionary.Add("{", "｛");
			dictionary.Add("|", "｜");
			dictionary.Add("}", "｝");
			dictionary.Add("~", "～");
			dictionary.Add("｡", "。");
			dictionary.Add("｢", "「");
			dictionary.Add("｣", "」");
			dictionary.Add("､", "、");
			dictionary.Add("･", "・");
			dictionary.Add("ｶﾞ", "ガ");
			dictionary.Add("ｷﾞ", "ギ");
			dictionary.Add("ｸﾞ", "グ");
			dictionary.Add("ｹﾞ", "ゲ");
			dictionary.Add("ｺﾞ", "ゴ");
			dictionary.Add("ｻﾞ", "ザ");
			dictionary.Add("ｼﾞ", "ジ");
			dictionary.Add("ｽﾞ", "ズ");
			dictionary.Add("ｾﾞ", "ゼ");
			dictionary.Add("ｿﾞ", "ゾ");
			dictionary.Add("ﾀﾞ", "ダ");
			dictionary.Add("ﾁﾞ", "ヂ");
			dictionary.Add("ﾂﾞ", "ヅ");
			dictionary.Add("ﾃﾞ", "デ");
			dictionary.Add("ﾄﾞ", "ド");
			dictionary.Add("ﾊﾞ", "バ");
			dictionary.Add("ﾋﾞ", "ビ");
			dictionary.Add("ﾌﾞ", "ブ");
			dictionary.Add("ﾍﾞ", "ベ");
			dictionary.Add("ﾎﾞ", "ボ");
			dictionary.Add("ｳﾞ", "ヴ");
			dictionary.Add("ﾜﾞ", "?");
			dictionary.Add("ｲﾞ", "?");
			dictionary.Add("ｴﾞ", "?");
			dictionary.Add("ｦﾞ", "?");
			dictionary.Add("ﾊﾟ", "パ");
			dictionary.Add("ﾋﾟ", "ピ");
			dictionary.Add("ﾌﾟ", "プ");
			dictionary.Add("ﾍﾟ", "ペ");
			dictionary.Add("ﾎﾟ", "ポ");
			dictionary.Add("ｦ", "ヲ");
			dictionary.Add("ｧ", "ァ");
			dictionary.Add("ｨ", "ィ");
			dictionary.Add("ｩ", "ゥ");
			dictionary.Add("ｪ", "ェ");
			dictionary.Add("ｫ", "ォ");
			dictionary.Add("ｬ", "ャ");
			dictionary.Add("ｭ", "ュ");
			dictionary.Add("ｮ", "ョ");
			dictionary.Add("ｯ", "ッ");
			dictionary.Add("ｰ", "ー");
			dictionary.Add("ｱ", "ア");
			dictionary.Add("ｲ", "イ");
			dictionary.Add("ｳ", "ウ");
			dictionary.Add("ｴ", "エ");
			dictionary.Add("ｵ", "オ");
			dictionary.Add("ｶ", "カ");
			dictionary.Add("ｷ", "キ");
			dictionary.Add("ｸ", "ク");
			dictionary.Add("ｹ", "ケ");
			dictionary.Add("ｺ", "コ");
			dictionary.Add("ｻ", "サ");
			dictionary.Add("ｼ", "シ");
			dictionary.Add("ｽ", "ス");
			dictionary.Add("ｾ", "セ");
			dictionary.Add("ｿ", "ソ");
			dictionary.Add("ﾀ", "タ");
			dictionary.Add("ﾁ", "チ");
			dictionary.Add("ﾂ", "ツ");
			dictionary.Add("ﾃ", "テ");
			dictionary.Add("ﾄ", "ト");
			dictionary.Add("ﾅ", "ナ");
			dictionary.Add("ﾆ", "ニ");
			dictionary.Add("ﾇ", "ヌ");
			dictionary.Add("ﾈ", "ネ");
			dictionary.Add("ﾉ", "ノ");
			dictionary.Add("ﾊ", "ハ");
			dictionary.Add("ﾋ", "ヒ");
			dictionary.Add("ﾌ", "フ");
			dictionary.Add("ﾍ", "ヘ");
			dictionary.Add("ﾎ", "ホ");
			dictionary.Add("ﾏ", "マ");
			dictionary.Add("ﾐ", "ミ");
			dictionary.Add("ﾑ", "ム");
			dictionary.Add("ﾒ", "メ");
			dictionary.Add("ﾓ", "モ");
			dictionary.Add("ﾔ", "ヤ");
			dictionary.Add("ﾕ", "ユ");
			dictionary.Add("ﾖ", "ヨ");
			dictionary.Add("ﾗ", "ラ");
			dictionary.Add("ﾘ", "リ");
			dictionary.Add("ﾙ", "ル");
			dictionary.Add("ﾚ", "レ");
			dictionary.Add("ﾛ", "ロ");
			dictionary.Add("ﾜ", "ワ");
			dictionary.Add("ﾝ", "ン");
			dictionary.Add("ﾞ", "゛");
			dictionary.Add("ﾟ", "゜");
			Dictionary<string, string> dictionary2 = dictionary;
			using (Dictionary<string, string>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					string text2 = text.Replace(current.get_Key(), current.get_Value());
					text = text2;
				}
			}
			return text;
		}

		public void setControlState()
		{
			if (this.TenderManager != null && this.TenderManager.State != OrganizeTender.TenderState.None)
			{
				TaskOrganizeTop.controlState = "tender";
				this._state = TaskOrganizeTop.OrganizeState.Tender;
			}
			else if (TaskOrganizeTop.BannerIndex == 0)
			{
				TaskOrganizeTop.controlState = "system";
				this._state = TaskOrganizeTop.OrganizeState.System;
			}
			else
			{
				TaskOrganizeTop.controlState = "banner";
				this._state = TaskOrganizeTop.OrganizeState.Top;
			}
			this.UpdateDeckSwitchManager();
		}

		public void UpdateSystemButtons()
		{
			this.SetAllUnsetBtn();
			this.SetTenderBtn();
			this.SetFleetNameBtn();
		}

		public void OnDeckChange(DeckModel deck)
		{
			if (this.currentDeck != null)
			{
				bool flag = this.GetDeckID() < deck.Id;
				for (int i = 0; i < 6; i++)
				{
					this._bannerManager[i].UpdateChangeBanner(false);
					if (flag)
					{
						this._bannerManager[i].DeckChangeAnimetion(true);
					}
					else
					{
						this._bannerManager[i].DeckChangeAnimetion(false);
					}
				}
			}
			this.currentDeck = deck;
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = (DeckModel)this.currentDeck;
			if (this.currentDeck != null && deck.IsActionEnd())
			{
				this.mTransform_TurnEndStamp.SetActive(true);
				ShortcutExtensions.DOKill(this.mTransform_TurnEndStamp, false);
				ShortcutExtensions.DOLocalRotate(this.mTransform_TurnEndStamp, new Vector3(0f, 0f, 300f), 0f, 1);
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this.mTransform_TurnEndStamp, new Vector3(0f, 0f, 360f), 0.8f, 1), 30);
			}
			else
			{
				this.mTransform_TurnEndStamp.SetActive(false);
			}
			ShipModel[] ships = deck.GetShips();
			for (int j = 0; j < 6; j++)
			{
				if (j < ships.Length)
				{
					this._bannerManager[j].setShip(ships[j]);
					this._bannerManager[j].IsSet = true;
				}
				else
				{
					this._bannerManager[j].IsSet = false;
				}
			}
			TaskOrganizeTop.BannerIndex = 1;
			this.setControlState();
			this.UpdateDeckName();
			this.UpdeteDeckIcon();
			this.UpdateSystemButtons();
			this.deckChangeArrows.UpdateView();
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			return true;
		}

		public void UpdateByModeChanging()
		{
			this.UpdateDeckSwitchManager();
		}

		public virtual void UpdateDeckSwitchManager()
		{
			OrganizeTaskManager.OrganizePhase phase = OrganizeTaskManager.GetPhase();
			this.deckSwitchManager.keyControlEnable = (phase == OrganizeTaskManager.OrganizePhase.Phase_ST && TaskOrganizeTop.controlState != "tender" && !this.isTenderAnimation());
		}

		private void OnSwipeEvent(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (0.3f < movedPercentageX)
				{
					this.FlickLeftSweet();
				}
				else if (movedPercentageX < -0.3f)
				{
					this.FlickRightSweet();
				}
			}
		}

		private void FlickRightSweet()
		{
			if (this.TenderManager.State == OrganizeTender.TenderState.Select)
			{
				if (OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() > 0)
				{
					if (this.TenderManager.setIndex < 2)
					{
						this.TenderManager.setIndex++;
					}
					else if (this.TenderManager.tenderDic.get_Item(SweetsType.Mamiya))
					{
						this.TenderManager.setIndex = 0;
					}
					else if (this.TenderManager.tenderDic.get_Item(SweetsType.Both))
					{
						this.TenderManager.setIndex = 1;
					}
					this.TenderManager.SetMainDialog();
				}
			}
			else if (this.TenderManager.State == OrganizeTender.TenderState.Maimiya || this.TenderManager.State == OrganizeTender.TenderState.Irako)
			{
				if (this.TenderManager.setIndex2 == 1)
				{
					return;
				}
				this.TenderManager.setIndex2++;
				this.TenderManager.updateSubBtn();
			}
			else if (this.TenderManager.State == OrganizeTender.TenderState.Twin)
			{
				if (this.TenderManager.setIndex2 == 1)
				{
					return;
				}
				this.TenderManager.setIndex2++;
				this.TenderManager.updateTwinBtn();
			}
		}

		private void FlickLeftSweet()
		{
			if (this.TenderManager.State == OrganizeTender.TenderState.Select)
			{
				if (TaskOrganizeTop.KeyController.IsRun)
				{
					if (0 < this.TenderManager.setIndex)
					{
						if (this.TenderManager.setIndex == 2)
						{
							if (OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() >= 1)
							{
								this.TenderManager.setIndex--;
								this.TenderManager.SetMainDialog();
							}
						}
						else if (this.TenderManager.setIndex == 1 && this.TenderManager.tenderDic.get_Item(SweetsType.Mamiya))
						{
							this.TenderManager.setIndex--;
							this.TenderManager.SetMainDialog();
						}
						else if (this.TenderManager.tenderDic.get_Item(SweetsType.Irako))
						{
							this.TenderManager.setIndex = 2;
							this.TenderManager.SetMainDialog();
						}
					}
					else if (0 < OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount())
					{
						this.TenderManager.setIndex = 2;
						this.TenderManager.SetMainDialog();
					}
				}
			}
			else if (this.TenderManager.State == OrganizeTender.TenderState.Maimiya || this.TenderManager.State == OrganizeTender.TenderState.Irako)
			{
				if (this.TenderManager.setIndex2 == 0)
				{
					return;
				}
				this.TenderManager.setIndex2--;
				this.TenderManager.updateSubBtn();
			}
			else if (this.TenderManager.State == OrganizeTender.TenderState.Twin)
			{
				if (this.TenderManager.setIndex2 == 0)
				{
					return;
				}
				this.TenderManager.setIndex2--;
				this.TenderManager.updateTwinBtn();
			}
		}

		public bool isTenderAnimation()
		{
			return this.TenderManager != null && this.TenderManager.isAnimation;
		}

		private bool OnCheckDragDropTarget(OrganizeBannerManager target)
		{
			return !this._isDragDrop && (this._uiDragDropItem == null || target.Equals(this._uiDragDropItem));
		}

		private void OnDragDropStart(OrganizeBannerManager target)
		{
			this._isDragDrop = true;
			this._uiDragDropItem = target;
			this.deckSwitchManager.keyControlEnable = false;
			if (TaskOrganizeTop.BannerIndex != target.number)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			TaskOrganizeTop.BannerIndex = target.number;
			this.UpdateChangeBanner();
		}

		private bool OnDragDropRelease(OrganizeBannerManager target)
		{
			if (target == null || this._uiDragDropItem == null)
			{
				Debug.LogWarning(" DragDrop NULL");
				return false;
			}
			OrganizeManager logicManager = OrganizeTaskManager.logicManager;
			DeckModel currentDeck = this.deckSwitchManager.currentDeck;
			ShipModel ship = target.ship;
			ShipModel ship2 = this._uiDragDropItem.ship;
			if (logicManager.ChangeOrganize(currentDeck.Id, target.number - 1, ship2.MemId))
			{
				target.setBanner(ship2, true, null, false);
				this._uiDragDropItem.setBanner(ship, true, delegate
				{
					this.OnDragDropEnd();
				}, false);
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
			}
			return true;
		}

		private void OnDragDropEnd()
		{
			this._isDragDrop = false;
			this._uiDragDropItem = null;
		}
	}
}
