using Common.Enum;
using DG.Tweening;
using KCV.Organize;
using KCV.PopupString;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using ModeProc;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class DeckStateViews : MonoBehaviour
	{
		private enum Mode
		{
			Top,
			SupplyConfirm,
			RepairConfirm,
			RepairKitConfirm,
			OrganizeDetail,
			OrganizeList,
			OrganizeListDetail
		}

		[SerializeField]
		private UIRebellionOrgaizeShipBanner[] ShipStates;

		private KeyControl key;

		private UIRebellionOrgaizeShipBanner FocusBanner;

		private SupplyManager SupplyMng;

		private RepairManager RepairMng;

		private OrganizeManager OrganizeMng;

		[SerializeField]
		private UILabel DeckNoLabel;

		[SerializeField]
		private UITexture DeckNoIcon;

		private ModeProcessor ModeProcessor;

		private KeyControl dialogKeyController;

		private ShipModel ListSelectShipModel;

		private DeckModel CurrentDeck;

		[SerializeField]
		private Camera DialogCamera;

		[SerializeField]
		private BoxCollider2D BackButtonCol;

		[SerializeField]
		private Transform DeckActionEnd;

		[SerializeField]
		private OrganizeScrollListParent ListParent;

		[SerializeField]
		private Animation BauxiSuccess;

		[SerializeField]
		private Animation BauxiField;

		[SerializeField]
		private GameObject Prefab_SupplyConfim;

		private StrategySupplyConfirm supplyConfirm;

		private RepairDockModel repairDockModel;

		[SerializeField]
		private GameObject Prefab_RepairConfim;

		private StrategyRepairConfirm repairConfim;

		[SerializeField]
		private GameObject Prefab_RepairKitConfim;

		private StrategyRepairKitConfirm repairKitConfim;

		[SerializeField]
		private Transform Prefab_OrganizeDetailMng;

		[SerializeField]
		private Transform Prefab_OrganizeList;

		private OrganizeDetail_Manager OrganizeDetailMng;

		private bool isInitialize;

		public void Start()
		{
			this.ListParent.SetActive(false);
		}

		public void Initialize(DeckModel deckModel, bool isCustomMode = false)
		{
			this.UpdateView(deckModel);
			this.CurrentDeck = deckModel;
			if (isCustomMode)
			{
				this.SetBannerColliderEnable(true);
				this.SetCustomMode();
			}
			else
			{
				this.ListParent.SetActive(false);
				this.SetBannerColliderEnable(false);
				this.BauxiSuccess.get_transform().get_parent().SetActive(false);
			}
		}

		private void SetBannerColliderEnable(bool isEnable)
		{
			UIRebellionOrgaizeShipBanner[] shipStates = this.ShipStates;
			for (int i = 0; i < shipStates.Length; i++)
			{
				UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = shipStates[i];
				uIRebellionOrgaizeShipBanner.GetComponent<BoxCollider2D>().set_enabled(isEnable);
			}
		}

		private void Update()
		{
			if (this.key == null)
			{
				return;
			}
			this.key.Update();
			this.ModeProcessor.ModeRun();
		}

		private void UpdateView(DeckModel deckModel)
		{
			int num = 0;
			int count = deckModel.Count;
			UIRebellionOrgaizeShipBanner[] shipStates = this.ShipStates;
			for (int i = 0; i < shipStates.Length; i++)
			{
				UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = shipStates[i];
				int nIndex = num + 1;
				uIRebellionOrgaizeShipBanner.SetShipData(deckModel.GetShip(num), nIndex);
				uIRebellionOrgaizeShipBanner.SetShipIndex(num);
				num++;
			}
			if (this.FocusBanner != null)
			{
				this.BannerFocusAnim(false);
				this.FocusBanner = this.ShipStates[this.key.Index];
				this.BannerFocusAnim(true);
			}
			this.DeckNoLabel.text = deckModel.Name;
			this.DeckNoLabel.supportEncoding = false;
			this.DeckNoIcon.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckModel.Id) as Texture2D);
			if (deckModel.IsActionEnd())
			{
				this.DeckActionEnd.SetActive(true);
				ShortcutExtensions.DOKill(this.DeckActionEnd, false);
				ShortcutExtensions.DOLocalRotate(this.DeckActionEnd, new Vector3(0f, 0f, 300f), 0f, 1);
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this.DeckActionEnd, new Vector3(0f, 0f, 360f), 0.8f, 1), 30);
			}
			else
			{
				this.DeckActionEnd.SetActive(false);
			}
		}

		public void SetCustomMode()
		{
			this.dialogKeyController = App.OnlyController;
			this.dialogKeyController.IsRun = false;
			this.BauxiSuccess.get_transform().localPositionX(1000f);
			this.BauxiField.get_transform().localPositionX(1000f);
			this.key = new KeyControl(0, 5, 0.4f, 0.1f);
			this.key.isLoopIndex = false;
			this.key.setChangeValue(-2f, 1f, 2f, -1f);
			App.OnlyController = this.key;
			this.BauxiSuccess.get_transform().get_parent().SetActive(true);
			this.ModeProcessor = base.GetComponent<ModeProcessor>();
			this.ModeProcessor.addMode("Top", new ModeProc.Mode.ModeRun(this.TopModeRun), new ModeProc.Mode.ModeChange(this.TopModeEnter), new ModeProc.Mode.ModeChange(this.TopModeExit));
			this.ModeProcessor.addMode("SupplyConfirm", new ModeProc.Mode.ModeRun(this.SupplyConfirmModeRun), new ModeProc.Mode.ModeChange(this.SupplyConfirmModeEnter), new ModeProc.Mode.ModeChange(this.SupplyConfirmModeExit));
			this.ModeProcessor.addMode("RepairConfirm", new ModeProc.Mode.ModeRun(this.RepairConfirmModeRun), new ModeProc.Mode.ModeChange(this.RepairConfirmModeEnter), new ModeProc.Mode.ModeChange(this.RepairConfirmModeExit));
			this.ModeProcessor.addMode("RepairKitConfirm", new ModeProc.Mode.ModeRun(this.RepairKitConfirmModeRun), new ModeProc.Mode.ModeChange(this.RepairKitConfirmModeEnter), new ModeProc.Mode.ModeChange(this.RepairKitConfirmModeExit));
			this.ModeProcessor.addMode("OrganizeDetail", new ModeProc.Mode.ModeRun(this.OrganizeDetailModeRun), new ModeProc.Mode.ModeChange(this.OrganizeDetailModeEnter), new ModeProc.Mode.ModeChange(this.OrganizeDetailModeExit));
			this.ModeProcessor.addMode("OrganizeList", new ModeProc.Mode.ModeRun(this.OrganizeListModeRun), new ModeProc.Mode.ModeChange(this.OrganizeListModeEnter), new ModeProc.Mode.ModeChange(this.OrganizeListModeExit));
			this.ModeProcessor.addMode("OrganizeListDetail", new ModeProc.Mode.ModeRun(this.OrganizeListDetailModeRun), new ModeProc.Mode.ModeChange(this.OrganizeListDetailModeEnter), new ModeProc.Mode.ModeChange(this.OrganizeListDetailModeExit));
			this.FocusBanner = this.ShipStates[0];
			UIRebellionOrgaizeShipBanner[] shipStates = this.ShipStates;
			for (int i = 0; i < shipStates.Length; i++)
			{
				UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = shipStates[i];
				uIRebellionOrgaizeShipBanner.SetOnClick(new Action<int>(this.OnClickBanner));
			}
			this.BannerFocusAnim(true);
			this.OrganizeMng = new OrganizeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.Instance.setActiveStrategy(false);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			this.ListParent.SetBackButtonEnable(false);
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceShow();
			this.isInitialize = false;
		}

		private void BannerFocusAnim(bool isEnable)
		{
			this.FocusBanner.SetFocus(isEnable);
		}

		private void TopModeRun()
		{
			if (this.key.IsChangeIndex)
			{
				if (this.key.prevIndex == 4 && this.key.IsDownDown())
				{
					this.key.Index = 4;
					return;
				}
				if (this.key.prevIndex == 1 && this.key.IsUpDown())
				{
					this.key.Index = 1;
					return;
				}
				this.BannerFocusAnim(false);
				this.FocusBanner = this.ShipStates[this.key.Index];
				this.BannerFocusAnim(true);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove, null);
			}
			else if (this.key.IsMaruDown())
			{
				this.GotoOrganize();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1, null);
			}
			else if (this.key.IsShikakuDown())
			{
				this.GotoSupplyConfirm();
			}
			else if (this.key.IsSankakuDown() && this.FocusBanner.ShipModel != null)
			{
				this.GotoRepairConfirm();
			}
			else if (this.key.IsBatuDown())
			{
				this.BackToSailSelect();
			}
			else if (this.key.IsRSRightDown())
			{
				this.ChangeDeck(true);
			}
			else if (this.key.IsRSLeftDown())
			{
				this.ChangeDeck(false);
			}
		}

		[DebuggerHidden]
		private IEnumerator TopModeEnter()
		{
			DeckStateViews.<TopModeEnter>c__Iterator136 <TopModeEnter>c__Iterator = new DeckStateViews.<TopModeEnter>c__Iterator136();
			<TopModeEnter>c__Iterator.<>f__this = this;
			return <TopModeEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator TopModeExit()
		{
			DeckStateViews.<TopModeExit>c__Iterator137 <TopModeExit>c__Iterator = new DeckStateViews.<TopModeExit>c__Iterator137();
			<TopModeExit>c__Iterator.<>f__this = this;
			return <TopModeExit>c__Iterator;
		}

		private void ChangeDeck(bool isNext)
		{
			int num = (!isNext) ? -1 : 1;
			int num2 = this.CurrentDeck.Id;
			num2 = (int)Util.LoopValue(num2 + num, 1f, (float)StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks().Length);
			StrategyTopTaskManager.GetSailSelect().DeckSelectController.SilentChangeIndex(num2 - 1);
			DeckModel deckModel = StrategyTopTaskManager.GetSailSelect().changeDeck(num2);
			if (deckModel == null)
			{
				return;
			}
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deckModel;
			this.CurrentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			this.UpdateView(this.CurrentDeck);
			this.UpdateFlagShip();
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID, false);
			this.OrganizeMng = new OrganizeManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			this.ListParent.Initialize(this.OrganizeMng, this.DialogCamera);
		}

		private void BackToSailSelect()
		{
			StrategyTopTaskManager.Instance.UIModel.HowToStrategy.isForceHide();
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(true);
			this.dialogKeyController.IsRun = true;
			StrategyTopTaskManager.GetSailSelect().CloseCommonDialog();
			StrategyTopTaskManager.Instance.setActiveStrategy(true);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			this.ListParent.SetActive(false);
			if (this.CurrentDeck.GetFlagShip() == null)
			{
				StrategyTopTaskManager.GetSailSelect().SearchAndChangeDeck(false, false);
				StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			}
			UIRebellionOrgaizeShipBanner[] shipStates = this.ShipStates;
			for (int i = 0; i < shipStates.Length; i++)
			{
				UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = shipStates[i];
				uIRebellionOrgaizeShipBanner.UnsetFocus();
			}
			this.FocusBanner = null;
		}

		private void GotoSupplyConfirm()
		{
			if (!this.CheckDeckState())
			{
				return;
			}
			ShipModel[] ships = this.CurrentDeck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i].IsTettaiBling())
				{
					CommonPopupDialog.Instance.StartPopup("撤退中の艦を含んでいます");
					return;
				}
			}
			this.SupplyMng = new SupplyManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			this.SupplyMng.InitForDeck(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id);
			if (this.SupplyMng.CheckBoxALLState != CheckBoxStatus.ON)
			{
				this.SupplyMng.ClickCheckBoxAll();
			}
			if (this.SupplyMng.CheckedShipIndices.Length == 0)
			{
				CommonPopupDialog.Instance.StartPopup("補給対象の艦が居ません");
				return;
			}
			if (!this.SupplyMng.IsValidSupply(SupplyType.All))
			{
				CommonPopupDialog.Instance.StartPopup("資源が不足しています");
				return;
			}
			this.ModeProcessor.ChangeMode(1);
		}

		private void GotoRepairConfirm()
		{
			if (!this.CheckDeckState())
			{
				return;
			}
			if (this.FocusBanner.ShipModel.IsTettaiBling())
			{
				CommonPopupDialog.Instance.StartPopup("撤退中の艦は入渠出来ません");
				return;
			}
			if (this.FocusBanner.ShipModel.IsInRepair())
			{
				if (StrategyTopTaskManager.GetLogicManager().Material.RepairKit <= 0)
				{
					CommonPopupDialog.Instance.StartPopup("高速修復材を持っていません");
					return;
				}
				this.ModeProcessor.ChangeMode(3);
			}
			else if (this.IsValidRepair())
			{
				this.ModeProcessor.ChangeMode(2);
			}
		}

		private void GotoOrganize()
		{
			if (this.FocusBanner.ShipModel != null)
			{
				this.ModeProcessor.ChangeMode(4);
			}
			else
			{
				this.ModeProcessor.ChangeMode(5);
			}
		}

		private void OnClickBanner(int index)
		{
			this.BannerFocusAnim(false);
			this.FocusBanner = this.ShipStates[index];
			this.BannerFocusAnim(true);
			this.GotoOrganize();
			this.key.SilentChangeIndex(index);
		}

		private void SupplyConfirmModeRun()
		{
		}

		[DebuggerHidden]
		private IEnumerator SupplyConfirmModeEnter()
		{
			DeckStateViews.<SupplyConfirmModeEnter>c__Iterator138 <SupplyConfirmModeEnter>c__Iterator = new DeckStateViews.<SupplyConfirmModeEnter>c__Iterator138();
			<SupplyConfirmModeEnter>c__Iterator.<>f__this = this;
			return <SupplyConfirmModeEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator SupplyConfirmModeExit()
		{
			DeckStateViews.<SupplyConfirmModeExit>c__Iterator139 <SupplyConfirmModeExit>c__Iterator = new DeckStateViews.<SupplyConfirmModeExit>c__Iterator139();
			<SupplyConfirmModeExit>c__Iterator.<>f__this = this;
			return <SupplyConfirmModeExit>c__Iterator;
		}

		private void OnDesideSupply()
		{
			App.OnlyController = this.key;
			bool flag2;
			bool flag = this.SupplyMng.Supply(SupplyType.All, out flag2);
			this.UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			if (flag && flag2)
			{
				this.BauxiSuccess.Play();
			}
			else if (!flag && flag2)
			{
				this.BauxiField.Play();
			}
			ShipModel model = (this.FocusBanner.ShipModel == null) ? this.CurrentDeck.GetFlagShip() : this.FocusBanner.ShipModel;
			ShipUtils.PlayShipVoice(model, 27);
			this.ModeProcessor.ChangeMode(0);
		}

		private void OnCancelSupply()
		{
			App.OnlyController = this.key;
			this.ModeProcessor.ChangeMode(0);
		}

		private void RepairConfirmModeRun()
		{
		}

		[DebuggerHidden]
		private IEnumerator RepairConfirmModeEnter()
		{
			DeckStateViews.<RepairConfirmModeEnter>c__Iterator13A <RepairConfirmModeEnter>c__Iterator13A = new DeckStateViews.<RepairConfirmModeEnter>c__Iterator13A();
			<RepairConfirmModeEnter>c__Iterator13A.<>f__this = this;
			return <RepairConfirmModeEnter>c__Iterator13A;
		}

		[DebuggerHidden]
		private IEnumerator RepairConfirmModeExit()
		{
			DeckStateViews.<RepairConfirmModeExit>c__Iterator13B <RepairConfirmModeExit>c__Iterator13B = new DeckStateViews.<RepairConfirmModeExit>c__Iterator13B();
			<RepairConfirmModeExit>c__Iterator13B.<>f__this = this;
			return <RepairConfirmModeExit>c__Iterator13B;
		}

		private void OnDesideRepair()
		{
			App.OnlyController = this.key;
			this.RepairMng.StartRepair(this.RepairMng.GetDockIndexFromDock(this.repairDockModel), this.FocusBanner.ShipModel.MemId, false);
			this.UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			StrategyTopTaskManager.Instance.TileManager.UpdateAllAreaDockIcons();
			if (this.FocusBanner.ShipModel.TaikyuRate > 50.0)
			{
				ShipUtils.PlayShipVoice(this.FocusBanner.ShipModel, 11);
			}
			else
			{
				ShipUtils.PlayShipVoice(this.FocusBanner.ShipModel, 12);
			}
			this.ModeProcessor.ChangeMode(0);
		}

		private void OnCancelRepair()
		{
			App.OnlyController = this.key;
			this.ModeProcessor.ChangeMode(0);
		}

		private bool IsValidRepair()
		{
			this.RepairMng = new RepairManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			this.repairDockModel = null;
			RepairDockModel[] docks = this.RepairMng.GetDocks();
			if (this.FocusBanner.ShipModel.NowHp >= this.FocusBanner.ShipModel.MaxHp)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDamage));
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel.NDockMax == 0)
			{
				CommonPopupDialog.Instance.StartPopup("この海域には入渠ドックがありません");
				return false;
			}
			for (int i = 0; i < this.RepairMng.GetDocks().Length; i++)
			{
				if (docks[i].State == NdockStates.EMPTY)
				{
					this.repairDockModel = docks[i];
					break;
				}
			}
			if (this.repairDockModel == null)
			{
				CommonPopupDialog.Instance.StartPopup("入渠ドックに空きがありません");
				return false;
			}
			if (!this.RepairMng.IsValidStartRepair(this.FocusBanner.ShipModel.MemId))
			{
				CommonPopupDialog.Instance.StartPopup("資源が不足しています");
				return false;
			}
			return true;
		}

		private void RepairKitConfirmModeRun()
		{
		}

		[DebuggerHidden]
		private IEnumerator RepairKitConfirmModeEnter()
		{
			DeckStateViews.<RepairKitConfirmModeEnter>c__Iterator13C <RepairKitConfirmModeEnter>c__Iterator13C = new DeckStateViews.<RepairKitConfirmModeEnter>c__Iterator13C();
			<RepairKitConfirmModeEnter>c__Iterator13C.<>f__this = this;
			return <RepairKitConfirmModeEnter>c__Iterator13C;
		}

		[DebuggerHidden]
		private IEnumerator RepairKitConfirmModeExit()
		{
			DeckStateViews.<RepairKitConfirmModeExit>c__Iterator13D <RepairKitConfirmModeExit>c__Iterator13D = new DeckStateViews.<RepairKitConfirmModeExit>c__Iterator13D();
			<RepairKitConfirmModeExit>c__Iterator13D.<>f__this = this;
			return <RepairKitConfirmModeExit>c__Iterator13D;
		}

		private void OnDesideRepairKit()
		{
			App.OnlyController = this.key;
			this.RepairMng.ChangeRepairSpeed(this.RepairMng.GetDockIndexFromDock(this.repairDockModel));
			this.UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			ShipUtils.PlayShipVoice(this.FocusBanner.ShipModel, 26);
			this.ModeProcessor.ChangeMode(0);
			this.UpdateFlagShip();
			StrategyTopTaskManager.Instance.TileManager.UpdateAllAreaDockIcons();
		}

		private void OnCancelRepairKit()
		{
			App.OnlyController = this.key;
			this.ModeProcessor.ChangeMode(0);
		}

		private void OrganizeDetailModeRun()
		{
			if (this.key.IsBatuDown())
			{
				this.BackToTop();
			}
			else if (this.key.IsRightDown())
			{
				this.OrganizeDetailMng.buttons.UpdateButton(false, this.OrganizeMng);
			}
			else if (this.key.IsLeftDown())
			{
				this.OrganizeDetailMng.buttons.UpdateButton(true, this.OrganizeMng);
			}
			else if (this.key.IsMaruDown())
			{
				this.OrganizeDetailMng.buttons.Decide();
			}
		}

		[DebuggerHidden]
		private IEnumerator OrganizeDetailModeEnter()
		{
			DeckStateViews.<OrganizeDetailModeEnter>c__Iterator13E <OrganizeDetailModeEnter>c__Iterator13E = new DeckStateViews.<OrganizeDetailModeEnter>c__Iterator13E();
			<OrganizeDetailModeEnter>c__Iterator13E.<>f__this = this;
			return <OrganizeDetailModeEnter>c__Iterator13E;
		}

		[DebuggerHidden]
		private IEnumerator OrganizeDetailModeExit()
		{
			DeckStateViews.<OrganizeDetailModeExit>c__Iterator13F <OrganizeDetailModeExit>c__Iterator13F = new DeckStateViews.<OrganizeDetailModeExit>c__Iterator13F();
			<OrganizeDetailModeExit>c__Iterator13F.<>f__this = this;
			return <OrganizeDetailModeExit>c__Iterator13F;
		}

		public void SetBtnEL()
		{
			this.ModeProcessor.ChangeMode(5);
		}

		public void ResetBtnEL()
		{
			this.OrganizeMng.UnsetOrganize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID, this.key.Index);
			this.ModeProcessor.ChangeMode(0);
			this.UpdateView(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
			this.UpdateFlagShip();
		}

		public void BackToTop()
		{
			this.ModeProcessor.ChangeMode(0);
		}

		private void CreateDetailPanel(bool isFirstDetail, ShipModel Ship)
		{
			GameObject gameObject = Util.Instantiate(this.Prefab_OrganizeDetailMng.get_gameObject(), base.get_gameObject(), false, false);
			this.OrganizeDetailMng = gameObject.GetComponent<OrganizeDetail_Manager>();
			this.OrganizeDetailMng.SetDetailPanel(Ship, isFirstDetail, SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id, this.OrganizeMng, this.FocusBanner.index, this);
		}

		private void OrganizeListModeRun()
		{
		}

		[DebuggerHidden]
		private IEnumerator OrganizeListModeEnter()
		{
			DeckStateViews.<OrganizeListModeEnter>c__Iterator140 <OrganizeListModeEnter>c__Iterator = new DeckStateViews.<OrganizeListModeEnter>c__Iterator140();
			<OrganizeListModeEnter>c__Iterator.<>f__this = this;
			return <OrganizeListModeEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OrganizeListModeExit()
		{
			DeckStateViews.<OrganizeListModeExit>c__Iterator141 <OrganizeListModeExit>c__Iterator = new DeckStateViews.<OrganizeListModeExit>c__Iterator141();
			<OrganizeListModeExit>c__Iterator.<>f__this = this;
			return <OrganizeListModeExit>c__Iterator;
		}

		private void OnShipSelect(ShipModel ship)
		{
			this.ModeProcessor.ChangeMode(6);
			this.ListSelectShipModel = ship;
		}

		private void OnCancel()
		{
			this.ModeProcessor.ChangeMode(0);
		}

		private void OrganizeListDetailModeRun()
		{
			if (this.key.IsBatuDown())
			{
				this.BackToList();
			}
			else if (this.key.IsMaruDown())
			{
				this.OrganizeDetailMng.buttons.Decide();
			}
			else if (this.key.IsShikakuDown())
			{
				this.ChangeLockButton();
			}
			else if (this.key.IsRightDown() && this.ListParent.GetFocusModel().IsLocked())
			{
				this.ChangeLockButton();
			}
			else if (this.key.IsLeftDown() && !this.ListParent.GetFocusModel().IsLocked())
			{
				this.ChangeLockButton();
			}
		}

		[DebuggerHidden]
		private IEnumerator OrganizeListDetailModeEnter()
		{
			DeckStateViews.<OrganizeListDetailModeEnter>c__Iterator142 <OrganizeListDetailModeEnter>c__Iterator = new DeckStateViews.<OrganizeListDetailModeEnter>c__Iterator142();
			<OrganizeListDetailModeEnter>c__Iterator.<>f__this = this;
			return <OrganizeListDetailModeEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OrganizeListDetailModeExit()
		{
			DeckStateViews.<OrganizeListDetailModeExit>c__Iterator143 <OrganizeListDetailModeExit>c__Iterator = new DeckStateViews.<OrganizeListDetailModeExit>c__Iterator143();
			<OrganizeListDetailModeExit>c__Iterator.<>f__this = this;
			return <OrganizeListDetailModeExit>c__Iterator;
		}

		public void ChangeLockButton()
		{
			this.OrganizeDetailMng.buttons.LockSwitch.MoveLockBtn();
		}

		public void ChangeButtonEL()
		{
			if (this.OrganizeMng.IsValidChange(this.CurrentDeck.Id, this.key.Index, this.ListSelectShipModel.MemId))
			{
				this.OrganizeMng.ChangeOrganize(this.CurrentDeck.Id, this.key.Index, this.ListSelectShipModel.MemId);
				this.UpdateView(this.CurrentDeck);
				this.UpdateFlagShip();
				this.ListParent.RefreshViews();
				this.ListParent.MovePosition(1030, false, null);
				ShipUtils.PlayShipVoice(this.ListSelectShipModel, 13);
				this.ModeProcessor.ChangeMode(0);
				StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			}
		}

		public void BackToList()
		{
			this.ModeProcessor.ChangeMode(5);
		}

		private void OnDestroy()
		{
			this.ShipStates = null;
			this.key = null;
			this.FocusBanner = null;
			this.SupplyMng = null;
			this.RepairMng = null;
			this.OrganizeMng = null;
			this.ModeProcessor = null;
			this.dialogKeyController = null;
			this.repairDockModel = null;
			this.Prefab_RepairConfim = null;
			this.repairConfim = null;
			this.Prefab_RepairKitConfim = null;
			this.repairKitConfim = null;
			this.Prefab_OrganizeDetailMng = null;
			this.Prefab_OrganizeList = null;
			this.OrganizeDetailMng = null;
			this.DeckNoIcon = null;
			this.DeckNoLabel = null;
			this.ListSelectShipModel = null;
			this.CurrentDeck = null;
		}

		private void UpdateFlagShip()
		{
			StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(this.CurrentDeck);
			StrategyTopTaskManager.Instance.UIModel.Character.ResetPosition();
			this.DelayActionFrame(1, delegate
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			});
			this.UpdateShipIcons();
		}

		private void UpdateShipIcons()
		{
			StrategyTopTaskManager.Instance.ShipIconManager.setShipIcons(this.OrganizeMng.UserInfo.GetDecks(), false);
			StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			StrategyTopTaskManager.Instance.ShipIconManager.SetVisible(true);
		}

		private bool CheckDeckState()
		{
			if (this.CurrentDeck.MissionState != MissionStates.NONE)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.Mission));
				return false;
			}
			return true;
		}
	}
}
