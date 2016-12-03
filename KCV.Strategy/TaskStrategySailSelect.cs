using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategySailSelect : SceneTaskMono
	{
		private StrategyShipManager shipIconManager;

		private StrategyInfoManager infoManager;

		private StrategyAreaManager areaManager;

		private GameObject topCamera;

		private StrategyTopTaskManager sttm;

		public KeyControl DeckSelectController;

		private int touchedButtonID;

		private Action FirstPlayVoice;

		public bool isEnableCharacterEnter;

		private int prevDeckID;

		public UILabel turnLabel;

		public UILabel DateLabel;

		private GameObject selectTile;

		private AsyncOperation asyncOpe;

		public Live2DModel Live2DModel;

		private Color TileFocusColor;

		[SerializeField]
		private UIGoSortieConfirm uiGoSortieConfirm;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private PSVitaMovie movie;

		private StrategyMapManager LogicMng
		{
			get
			{
				return StrategyTopTaskManager.GetLogicManager();
			}
		}

		public int PrevDeckID
		{
			get
			{
				return this.prevDeckID;
			}
		}

		public void sailSelectFirstInit()
		{
			this.shipIconManager = StrategyTopTaskManager.Instance.ShipIconManager;
			this.infoManager = StrategyTopTaskManager.Instance.GetInfoMng();
			this.areaManager = StrategyTopTaskManager.Instance.GetAreaMng();
			this.TileFocusColor = new Color(25f, 227f, 143f, 1f);
			this.sttm = StrategyTaskManager.GetStrategyTop();
			this.DeckSelectController = new KeyControl(0, StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount - 1, 0.4f, 0.1f);
			this.DeckSelectController.setChangeValue(0f, 1f, 0f, -1f);
			this.DeckSelectController.KeyInputInterval = 0.2f;
			this.DeckSelectController.SilentChangeIndex(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			this.Live2DModel = SingletonMonoBehaviour<Live2DModel>.Instance;
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			KeyControlManager.Instance.KeyController.Index = currentAreaID;
			this.shipIconManager.setShipIcons(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks(), true);
			SingletonMonoBehaviour<AppInformation>.Instance.prevStrategyDecks = null;
			this.FirstPlayVoice = delegate
			{
				StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			};
			this.isEnableCharacterEnter = true;
			this.prevDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID;
		}

		protected override bool Init()
		{
			this.shipIconManager.changeFocus();
			KeyControlManager.Instance.KeyController = this.DeckSelectController;
			this.DeckSelectController.SilentChangeIndex(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id - 1);
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != MissionStates.NONE)
			{
				this.isEnableCharacterEnter = false;
			}
			if (this.isEnableCharacterEnter)
			{
				this.moveCharacterScreen(true, this.FirstPlayVoice);
			}
			this.FirstPlayVoice = null;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
			}
			if (StrategyTopTaskManager.Instance.TutorialGuide6_2 != null)
			{
				StrategyTopTaskManager.Instance.TutorialGuide6_2.Hide();
			}
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.SetKeyController(this.DeckSelectController, StrategyAreaManager.sailKeyController);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			return true;
		}

		protected override bool UnInit()
		{
			if (StrategyTopTaskManager.Instance != null)
			{
				StrategyTopTaskManager.Instance.UIModel.HowToStrategy.SetKeyController(null, null);
			}
			return true;
		}

		protected override bool Run()
		{
			if (this.LogicMng == null)
			{
				return true;
			}
			this.DeckSelectController.Update();
			StrategyAreaManager.sailKeyController.SilentChangeIndex(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID);
			StrategyAreaManager.sailKeyController.RightStickUpdate();
			return this.KeyAction();
		}

		private bool KeyAction()
		{
			if (this.DeckSelectController.IsChangeIndex)
			{
				bool isNext = this.DeckSelectController.prevIndexChangeValue == 1;
				this.SearchAndChangeDeck(isNext, false);
				if (this.prevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
				{
					this.changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
					StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					if (StrategyTopTaskManager.Instance.UIModel.Character.shipModel != null)
					{
						StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, null);
					}
				}
				return true;
			}
			if (StrategyAreaManager.sailKeyController.IsChangeIndex)
			{
				this.areaManager.UpdateSelectArea(StrategyAreaManager.sailKeyController.Index, false);
			}
			else if (this.DeckSelectController.keyState.get_Item(1).down)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() == null)
				{
					this.GotoOrganize();
				}
				else
				{
					this.OpenCommandMenu();
				}
			}
			else if (this.DeckSelectController.keyState.get_Item(3).down)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShipCount() != 0)
				{
					if (this.prevDeckID != SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID)
					{
						this.changeDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
						StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
					}
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
					}
					this.uiGoSortieConfirm.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f));
					this.commonDialog.OpenDialog(2, DialogAnimation.AnimType.FEAD);
					this.uiGoSortieConfirm.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, false);
					this.commonDialog.setCloseAction(delegate
					{
						KeyControlManager.Instance.KeyController = this.DeckSelectController;
						if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
						{
							TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
							SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
						}
					});
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				}
			}
			else if (this.DeckSelectController.keyState.get_Item(5).down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			else if (this.DeckSelectController.keyState.get_Item(0).down)
			{
				this.areaManager.UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID, false);
			}
			else if (this.DeckSelectController.keyState.get_Item(2).down)
			{
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				this.commonDialog.OpenDialog(4, DialogAnimation.AnimType.POPUP);
				this.commonDialog.keyController.IsRun = false;
				this.commonDialog.setOpenAction(delegate
				{
					this.commonDialog.keyController.IsRun = true;
				});
				this.commonDialog.ShikakuButtonAction = delegate
				{
					base.Close();
					StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.TurnEnd);
					StrategyTopTaskManager.GetTurnEnd().TurnEnd();
					if (StrategyTopTaskManager.Instance.TutorialGuide8_1 != null)
					{
						if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
						{
							SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().HideAndDestroy();
						}
						StrategyTopTaskManager.Instance.TutorialGuide8_1.HideAndDestroy();
					}
				};
				this.commonDialog.BatuButtonAction = delegate
				{
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
					}
				};
			}
			return true;
		}

		[DebuggerHidden]
		public IEnumerator SceneChange()
		{
			TaskStrategySailSelect.<SceneChange>c__Iterator193 <SceneChange>c__Iterator = new TaskStrategySailSelect.<SceneChange>c__Iterator193();
			<SceneChange>c__Iterator.<>f__this = this;
			return <SceneChange>c__Iterator;
		}

		public void CloseCommonDialog()
		{
			this.commonDialog.CloseDialog();
		}

		public void OpenCommandMenu()
		{
			if (StrategyTopTaskManager.GetCommandMenu().CommandMenu.isOpen)
			{
				return;
			}
			this.changeDeckAreaSelect(StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID);
			StrategyTopTaskManager.Instance.GetInfoMng().ExitInfoPanel();
			if (Random.Range(0, 3) == 0)
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() != null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
				{
					ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 3);
				}
			}
			else
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			base.Close();
		}

		public void moveCharacterScreen(bool isEnter, Action Onfinished)
		{
			UIShipCharacter character = StrategyTopTaskManager.Instance.UIModel.Character;
			if (isEnter)
			{
				character.Enter(Onfinished);
			}
			else
			{
				character.Exit(Onfinished, false);
			}
		}

		public DeckModel changeDeck(int DeckID)
		{
			if (this.prevDeckID == DeckID)
			{
				return null;
			}
			this.prevDeckID = DeckID;
			DeckModel deck = this.LogicMng.UserInfo.GetDeck(DeckID);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel = StrategyTopTaskManager.GetLogicManager().Area.get_Item(deck.AreaId);
			bool arg_62_0 = deck.GetFlagShip() != null && deck.GetFlagShip().IsDamaged();
			StrategyTopTaskManager.Instance.GetInfoMng().changeCharacter(deck);
			StrategyTopTaskManager.Instance.UIModel.Character.setState(deck);
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(false);
			return deck;
		}

		private void changeDeckAreaSelect(int areaID)
		{
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			DeckModel[] decks = this.LogicMng.Area.get_Item(areaID).GetDecks();
			if (decks.Length == 0)
			{
				return;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID == areaID)
			{
				return;
			}
			if (Enumerable.All<DeckModel>(decks, (DeckModel x) => x.GetFlagShip() == null))
			{
				return;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != decks[0])
			{
				this.prevDeckID = id;
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = decks[0];
				this.changeDeck(decks[0].Id);
				this.shipIconManager.changeFocus();
			}
		}

		public void SearchAndChangeDeck(bool isNext, bool isSeachLocalArea)
		{
			if (StrategyTopTaskManager.GetLogicManager().UserInfo.DeckCount > 1)
			{
				int id;
				if (isNext)
				{
					id = StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.getNextDeck(this.DeckSelectController.prevIndex + 1, isSeachLocalArea).Id;
					this.DeckSelectController.SilentChangeIndex(id - 1);
				}
				else
				{
					id = StrategyTopTaskManager.Instance.UIModel.UIMapManager.ShipIconManager.getPrevDeck(this.DeckSelectController.prevIndex + 1, isSeachLocalArea).Id;
					this.DeckSelectController.SilentChangeIndex(id - 1);
				}
				this.prevDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID;
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == this.LogicMng.UserInfo.GetDeck(id))
				{
					return;
				}
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = this.LogicMng.UserInfo.GetDeck(id);
				this.shipIconManager.changeFocus();
			}
			this.areaManager.UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID, false);
		}

		public void GotoOrganize()
		{
			if (base.isRun)
			{
				DeckModel deckModel = Enumerable.FirstOrDefault<DeckModel>(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks(), (DeckModel x) => x.Count == 0);
				if (deckModel != null)
				{
					SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deckModel;
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Organize, false);
				}
			}
		}

		private void OnDestroy()
		{
		}
	}
}
