using Common.Enum;
using Common.Struct;
using KCV.Display;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UserInterfaceRemodelManager : MonoBehaviour, CommonDeckSwitchHandler
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UISprite backGroundUpperSteelFrame;

		[SerializeField]
		private UISprite backGroundLowerSteelFrame;

		[SerializeField]
		private Camera mCamera_TouchEventCatch;

		[SerializeField]
		private PortUpgradesConvertShipManager mPrefab_PortUpgradesConvertShipManager_KaizouAnimation;

		[SerializeField]
		private PortUpgradesModernizeShipManager mPrefab_PortUpgradesModernizeShipManager_KindaikakaishuAnimation;

		[SerializeField]
		private UIRemodelHeader mUIRemodelHeader;

		[SerializeField]
		private UIRemodelShipSlider mUIRemodelShipSlider;

		[SerializeField]
		private UIRemodelShipStatus mUIRemodelShipStatus;

		[SerializeField]
		private UIRemodelLeftShipStatus mUIRemodelLeftShipStatus;

		[SerializeField]
		private UIRemodelStartRightInfo mUIRemodelStartRightInfo;

		[SerializeField]
		private UIRemodelEquipSlotItems mUIRemodelEquipSlotItems;

		[SerializeField]
		private UIRemodelModeSelectMenu mUIRemodelModeSelectMenu;

		[SerializeField]
		private UIRemodelDeckSwitchManager mUIRemodelDeckSwitchManager;

		[SerializeField]
		private UIRemodelModernization mUIRemodelModernization;

		[SerializeField]
		private UIRemodelKaizou mUIRemodelKaizou;

		[SerializeField]
		private UIRemodelEquipListParentNew mUIRemodelEquipListParent;

		[SerializeField]
		private UIRemodelSlotItemChangePreview mUIRemodelSlotItemChangePreview;

		[SerializeField]
		private CategoryMenu mCategoryMenu;

		[SerializeField]
		private UIRemodeModernizationShipTargetListParentNew mUIRemodeModernizationShipTargetListParentNew;

		[SerializeField]
		private UIRemodelModernizationStartConfirm mUIRemodelModernizationStartConfirm;

		[SerializeField]
		private UIRemodelOtherShipPickerParentNew mUIRemodelOtherShipPickerParent;

		[SerializeField]
		private UIRemodelDeckSwitchManager commonDeckSwitchManager;

		[SerializeField]
		private UIRemodelCurrentSlot mUIRemodelCurrentSlot;

		[SerializeField]
		private UIRemodelHowTo mUIHowTo;

		[SerializeField]
		private GameObject okBauxiteUseMessage;

		[SerializeField]
		private GameObject okBauxiteUseHighCostMessage;

		[SerializeField]
		private GameObject ngBauxiteShortMessage;

		[SerializeField]
		private GameObject ngBausiteShortHighCostMessage;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		private List<UIRemodelView> allSwitchableViews = new List<UIRemodelView>();

		private Dictionary<ScreenStatus, List<UIRemodelView>> viewList = new Dictionary<ScreenStatus, List<UIRemodelView>>();

		public ScreenStatus status;

		public bool guideoff;

		public RemodelManager mRemodelManager
		{
			get;
			private set;
		}

		public RemodelGradeUpManager mRemodelGradeUpManager
		{
			get;
			private set;
		}

		public DeckModel focusedDeckModel
		{
			get;
			private set;
		}

		public ShipModel focusedShipModel
		{
			get;
			private set;
		}

		private bool otherShip
		{
			get
			{
				return this.focusedDeckModel == null;
			}
		}

		public static UserInterfaceRemodelManager instance
		{
			get;
			private set;
		}

		private int CountBannersInTexture(CommonShipBanner[] banners, Texture countTargetTexture)
		{
			int num = 0;
			if (countTargetTexture == null)
			{
				return num;
			}
			for (int i = 0; i < banners.Length; i++)
			{
				CommonShipBanner commonShipBanner = banners[i];
				if (!(commonShipBanner == null))
				{
					Texture mainTexture = commonShipBanner.GetUITexture().mainTexture;
					if (!(mainTexture == null))
					{
						bool flag = countTargetTexture.GetNativeTexturePtr() == mainTexture.GetNativeTexturePtr();
						if (flag)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		public void ReleaseRequestBanner(ref Texture releaseRequestTexture)
		{
			if (releaseRequestTexture == null)
			{
				releaseRequestTexture = null;
				return;
			}
			int num = 0;
			IBannerResourceManage bannerResourceManage = this.mUIRemodelOtherShipPickerParent;
			CommonShipBanner[] banner = bannerResourceManage.GetBanner();
			int num2 = this.CountBannersInTexture(banner, releaseRequestTexture);
			num += num2;
			IBannerResourceManage bannerResourceManage2 = this.mUIRemodelModernization;
			CommonShipBanner[] banner2 = bannerResourceManage2.GetBanner();
			int num3 = this.CountBannersInTexture(banner2, releaseRequestTexture);
			num += num3;
			IBannerResourceManage bannerResourceManage3 = this.mUIRemodeModernizationShipTargetListParentNew;
			CommonShipBanner[] banner3 = bannerResourceManage3.GetBanner();
			int num4 = this.CountBannersInTexture(banner3, releaseRequestTexture);
			num += num4;
			IBannerResourceManage bannerResourceManage4 = this.mUIRemodelModernizationStartConfirm;
			CommonShipBanner[] banner4 = bannerResourceManage4.GetBanner();
			int num5 = this.CountBannersInTexture(banner4, releaseRequestTexture);
			num += num5;
			bool flag = num == 0;
			if (flag)
			{
				Resources.UnloadAsset(releaseRequestTexture);
			}
			releaseRequestTexture = null;
		}

		private void SetStatus(ScreenStatus status)
		{
			this.status = status;
			if (status != ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION && status != ScreenStatus.MODE_KAIZO_ANIMATION)
			{
				if (!SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.isTransitionNow)
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			}
			this.UpdateHeaderTitle();
			this.SwitchViews();
		}

		private void SwitchViews()
		{
			switch (this.status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
				this.guideoff = false;
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				this.guideoff = false;
				this.mUIRemodelShipStatus.ShowMove();
				this.mUIRemodelLeftShipStatus.Init(this.focusedShipModel);
				this.mUIRemodelLeftShipStatus.SetExpand(false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU:
				this.mUIRemodelLeftShipStatus.Init(this.focusedShipModel);
				this.mUIRemodelLeftShipStatus.SetExpand(false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
				this.mUIRemodelCurrentSlot.SetActive(true);
				this.mUIRemodelCurrentSlot.Init(this.mUIRemodelEquipSlotItems.currentFocusItem.GetModel());
				break;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
				this.mUIRemodelModernization.InitFocus();
				this.mUIRemodelLeftShipStatus.Init(this.focusedShipModel);
				this.mUIRemodelLeftShipStatus.SetExpand(true);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				break;
			case ScreenStatus.MODE_KAIZO:
				this.mUIRemodelKaizou.Show();
				break;
			case ScreenStatus.MODE_KAIZO_ANIMATION:
				this.guideoff = true;
				break;
			}
			this.allSwitchableViews.ForEach(delegate(UIRemodelView e)
			{
				if (this.viewList.ContainsKey(this.status) && this.viewList.get_Item(this.status).Contains(e))
				{
					((MonoBehaviour)e).SetActive(true);
					e.Show();
				}
				else if (((MonoBehaviour)e).get_gameObject().get_activeSelf())
				{
					e.Hide();
				}
			});
		}

		public void Awake()
		{
			UserInterfaceRemodelManager.instance = this;
			this.guideoff = false;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				this.HandleKeyController();
			}
		}

		private void HandleKeyController()
		{
			ScreenStatus screenStatus = this.status;
			if (screenStatus == ScreenStatus.SELECT_DECK_SHIP || screenStatus == ScreenStatus.SELECT_OTHER_SHIP)
			{
				if (this.mKeyController.IsBatuDown())
				{
					this.Back2PortTop();
				}
			}
			if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION && this.status != ScreenStatus.MODE_KAIZO_ANIMATION && this.mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		public void OnTouchShipStatus()
		{
			switch (this.status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
			case ScreenStatus.SELECT_OTHER_SHIP:
				this.Forward2ModeSelect();
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				this.Back2ShipSelect();
				break;
			}
		}

		private void Back2PortTop()
		{
			if (this.status == ScreenStatus.SELECT_DECK_SHIP || this.status == ScreenStatus.SELECT_OTHER_SHIP)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}

		[DebuggerHidden]
		private IEnumerator CloseProcess()
		{
			return new UserInterfaceRemodelManager.<CloseProcess>c__IteratorB4();
		}

		public void Back2ShipSelect()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.mUIRemodelStartRightInfo.Init(this.focusedShipModel);
			this.SetStatus((this.focusedDeckModel != null) ? ScreenStatus.SELECT_DECK_SHIP : ScreenStatus.SELECT_OTHER_SHIP);
		}

		public void Forward2ModeSelect()
		{
			if (this.status != ScreenStatus.SELECT_DECK_SHIP && this.status != ScreenStatus.SELECT_OTHER_SHIP && this.status != ScreenStatus.MODE_KAIZO_END_ANIMATION)
			{
				return;
			}
			this.DecideShip();
			this.SetStatus(ScreenStatus.SELECT_SETTING_MODE);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void Back2ModeSelect()
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU && this.status != ScreenStatus.MODE_KINDAIKA_KAISHU && this.status != ScreenStatus.MODE_KAIZO && this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION)
			{
				return;
			}
			this.SetStatus(ScreenStatus.SELECT_SETTING_MODE);
		}

		public void Forward2SoubiHenkou(SlotitemModel slotItemModel = null, bool requestChangeMode = false)
		{
			if (this.status != ScreenStatus.SELECT_DECK_SHIP && this.status != ScreenStatus.SELECT_OTHER_SHIP && this.status != ScreenStatus.SELECT_SETTING_MODE)
			{
				return;
			}
			if (this.status == ScreenStatus.SELECT_DECK_SHIP || this.status == ScreenStatus.SELECT_OTHER_SHIP)
			{
				this.DecideShip();
			}
			this.mUIRemodelEquipSlotItems.Initialize(this.mKeyController, this.focusedShipModel);
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU);
			if (requestChangeMode)
			{
				this.mUIRemodelEquipSlotItems.ChangeFocusItemFromModel(slotItemModel);
			}
		}

		public void Back2SoubiHenkou()
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT && this.status != ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW)
			{
				return;
			}
			this.mUIRemodelEquipSlotItems.Initialize(this.mKeyController, this.focusedShipModel);
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU);
		}

		public void Forward2SoubiHenkouTypeSelect()
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU)
			{
				return;
			}
			this.mCategoryMenu.SetActive(true);
			this.mCategoryMenu.Init(this.mKeyController, this.focusedShipModel, this.mUIRemodelEquipSlotItems.currentFocusItem);
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT);
		}

		public void Back2SoubiHenkouTypeSelect()
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT);
		}

		public void Forward2SoubiHenkouItemSelect(ShipModel shipModel, SlotitemCategory slotitemCategory)
		{
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT);
			this.mUIRemodelEquipListParent.SetActive(true);
			this.mUIRemodelEquipListParent.Initialize(this.mKeyController, this.mUIRemodelShipStatus, this.mUIRemodelEquipSlotItems, shipModel, slotitemCategory);
		}

		public void Back2SoubiHenkouItemSelect()
		{
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT);
		}

		public void Wait2AnimationFromKindaikaKakunin()
		{
			this.status = ScreenStatus.WAIT;
		}

		public void Resume2WaitKindaikaKakunin()
		{
			this.status = ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN;
		}

		public void Forward2SoubiHenkouPreview(ShipModel targetShipModel, int selectedSlotIndex, UIRemodelEquipListChildNew child)
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW);
			SlotitemModel srcSlotItemModel = null;
			if (selectedSlotIndex < targetShipModel.SlotitemList.get_Count())
			{
				srcSlotItemModel = targetShipModel.SlotitemList.get_Item(selectedSlotIndex);
			}
			else if (targetShipModel.HasExSlot())
			{
				srcSlotItemModel = targetShipModel.SlotitemEx;
			}
			this.mUIRemodelSlotItemChangePreview.Initialize(this.mKeyController, targetShipModel, srcSlotItemModel, child.GetModel(), selectedSlotIndex);
		}

		public void Forward2KindaikaKaishu()
		{
			if (this.status != ScreenStatus.SELECT_SETTING_MODE)
			{
				return;
			}
			this.mUIRemodelModernizationStartConfirm.DrawShip(this.focusedShipModel);
			this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU);
		}

		public void Back2KindaikaKaishu()
		{
			if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU && this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU);
		}

		public void Forward2KindaikaKaishuSozaiSentaku(List<ShipModel> selectedSozaiShipModels)
		{
			if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU && this.status != ScreenStatus.SELECT_SETTING_MODE)
			{
				return;
			}
			this.mUIRemodeModernizationShipTargetListParentNew.SetActive(true);
			this.mUIRemodeModernizationShipTargetListParentNew.Initialize(this.mKeyController, this.mUIRemodelModernization.GetFocusSlot().GetSlotInShip(), selectedSozaiShipModels);
			this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU);
		}

		public void Forward2KindaikaKaishuKakunin(ShipModel targetShipModel, List<ShipModel> sozaiShipModels)
		{
			if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN);
			PowUpInfo powUpInfo = this.mRemodelManager.getPowUpInfo(sozaiShipModels);
			this.mUIRemodelModernizationStartConfirm.Initialize(this.mKeyController, targetShipModel, sozaiShipModels, powUpInfo);
		}

		public void Forward2KindaikaKaishuAnimation(List<ShipModel> sozai, ShipModel baseShip)
		{
			if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION);
			this.HideHeader();
			this.mUIRemodelHeader.SetActive(false);
			this.commonDeckSwitchManager.SetActive(false);
			this.backGroundUpperSteelFrame.SetActive(false);
			this.backGroundLowerSteelFrame.SetActive(false);
			bool superSuccessed;
			ShipModel shipModel = this.mRemodelManager.PowerUp(sozai, out superSuccessed);
			bool isFail = shipModel == null;
			this.mUIRemodelShipStatus.SetActive(false);
			base.StartCoroutine(this.StartModernizeAnimation(baseShip, 1, this.mKeyController, isFail, superSuccessed, sozai.get_Count(), baseShip.IsDamaged(), delegate
			{
				if (this.status != ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION)
				{
					return;
				}
				this.SetStatus(ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION);
				this.ShowHeader();
				this.mUIRemodelHeader.SetActive(true);
				this.backGroundUpperSteelFrame.SetActive(true);
				this.backGroundLowerSteelFrame.SetActive(true);
				this.mUIRemodelShipStatus.SetActive(true);
				this.mUIRemodelModernization.UnSetAll();
				this.mUIRemodelModernization.RemoveFocus();
				this.mUIRemodelOtherShipPickerParent.Refresh((!this.otherShip) ? null : this.focusedShipModel);
				this.commonDeckSwitchManager.SetActive(true);
				this.mUIRemodelLeftShipStatus.SetExpand(false);
				this.mUIRemodelLeftShipStatus.Hide(false);
				this.Back2ModeSelect();
			}));
		}

		public void Forward2Kaizo()
		{
			if (this.status != ScreenStatus.SELECT_SETTING_MODE)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_KAIZO);
			this.mUIRemodelKaizou.Initialize(this.focusedShipModel, this.mRemodelGradeUpManager.DesignSpecificationsForGradeup);
			this.mUIRemodelKaizou.SetKeyController(this.mKeyController);
		}

		public void Forward2KaizoAnimation(ShipModel targetShipModel)
		{
			if (this.status != ScreenStatus.MODE_KAIZO)
			{
				return;
			}
			this.SetStatus(ScreenStatus.MODE_KAIZO_ANIMATION);
			if (UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.GradeUp())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.mRemodelManager.ClearUnsetSlotsCache();
			}
			this.backGroundUpperSteelFrame.SetActive(false);
			this.backGroundLowerSteelFrame.SetActive(false);
			this.mUIRemodelShipStatus.SetActive(false);
			this.HideHeader();
			this.mUIRemodelHeader.SetActive(false);
			this.UpdateHeaderMaterial();
			base.StartCoroutine(this.StartGradeUpProductionCoroutine(targetShipModel, this.mKeyController, delegate
			{
				if (this.status != ScreenStatus.MODE_KAIZO_ANIMATION)
				{
					return;
				}
				this.status = ScreenStatus.MODE_KAIZO_END_ANIMATION;
				if (this.otherShip)
				{
					this.mUIRemodelOtherShipPickerParent.Refresh(this.focusedShipModel);
				}
				this.mUIRemodelShipStatus.SetActive(true);
				this.ShowHeader();
				this.mUIRemodelHeader.SetActive(true);
				this.ChangeFocusShip(targetShipModel);
				this.backGroundUpperSteelFrame.SetActive(true);
				this.backGroundLowerSteelFrame.SetActive(true);
				this.mUIRemodelModeSelectMenu.Init(this.mKeyController, this.mRemodelGradeUpManager.GradeupBtnEnabled);
				this.Forward2ModeSelect();
			}));
		}

		private void ChangeFocusDeck(DeckModel deckModel)
		{
			this.focusedDeckModel = deckModel;
			if (this.focusedDeckModel != null)
			{
				this.mUIRemodelShipSlider.Initialize(this.focusedDeckModel);
				this.ChangeFocusShip(this.focusedDeckModel.GetFlagShip());
			}
		}

		public void ChangeFocusShip(ShipModel focusShipModel)
		{
			this.focusedShipModel = focusShipModel;
			this.mUIRemodelShipStatus.Init(this.focusedShipModel);
			this.mUIRemodelStartRightInfo.Init(this.focusedShipModel);
		}

		public void ProcessChangeSlotItem(int slotIndex, SlotitemModel slotItem, int voiceType, bool isExSlot)
		{
			if (this.status != ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW)
			{
				return;
			}
			SlotSetInfo slotitemInfoToChange = this.mRemodelManager.GetSlotitemInfoToChange(this.focusedShipModel.MemId, slotItem.MemId, slotIndex);
			SlotSetChkResult_Slot slotSetChkResult_Slot;
			if (!isExSlot)
			{
				slotSetChkResult_Slot = this.mRemodelManager.IsValidChangeSlotitem(this.focusedShipModel.MemId, slotItem.MemId, slotIndex);
			}
			else
			{
				slotSetChkResult_Slot = this.mRemodelManager.IsValidChangeSlotitemEx(this.focusedShipModel.MemId, slotItem.MemId);
			}
			bool flag = false;
			switch (slotSetChkResult_Slot)
			{
			case SlotSetChkResult_Slot.Ok:
			case SlotSetChkResult_Slot.OkBauxiteUse:
			case SlotSetChkResult_Slot.OkBauxiteUseHighCost:
				flag = true;
				break;
			case SlotSetChkResult_Slot.NgBauxiteShort:
				this.AnimateBauxite(this.ngBauxiteShortMessage);
				break;
			case SlotSetChkResult_Slot.NgBausiteShortHighCost:
				this.AnimateBauxite(this.ngBausiteShortHighCostMessage);
				break;
			}
			SlotSetChkResult_Slot slotSetChkResult_Slot2;
			if (!isExSlot)
			{
				slotSetChkResult_Slot2 = this.mRemodelManager.ChangeSlotitem(this.focusedShipModel.MemId, slotItem.MemId, slotIndex);
			}
			else
			{
				slotSetChkResult_Slot2 = this.mRemodelManager.ChangeSlotitemEx(this.focusedShipModel.MemId, slotItem.MemId);
			}
			if (flag)
			{
				bool flag2 = true;
				switch (slotSetChkResult_Slot2)
				{
				case SlotSetChkResult_Slot.Ok:
					break;
				case SlotSetChkResult_Slot.OkBauxiteUse:
					this.AnimateBauxite(this.okBauxiteUseMessage);
					break;
				case SlotSetChkResult_Slot.OkBauxiteUseHighCost:
					this.AnimateBauxite(this.okBauxiteUseHighCostMessage);
					break;
				default:
					flag2 = false;
					break;
				}
				if (flag2)
				{
					ShipUtils.PlayShipVoice(this.focusedShipModel, voiceType);
				}
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mRemodelManager);
				}
			}
			this.Back2SoubiHenkou();
		}

		private void AnimateBauxite(GameObject texture)
		{
			texture.GetComponent<Animation>().Play("RemodelBauxiteMessage");
		}

		public void SelectKindaikaKaishuSozai(ShipModel selectedShipModel)
		{
			if (selectedShipModel == null)
			{
				this.mUIRemodelModernization.GetFocusSlot().UnSet();
			}
			else
			{
				this.mUIRemodelModernization.SetCurrentFocusToShip(selectedShipModel);
			}
			this.mUIRemodelModernization.RefreshList();
			this.Back2KindaikaKaishu();
		}

		private void DecideShip()
		{
			this.mUIRemodelModernization.SetActive(true);
			this.mUIRemodelModernization.Initialize(this.mKeyController, this.focusedShipModel);
			this.mRemodelGradeUpManager = new RemodelGradeUpManager(this.focusedShipModel);
			this.mUIRemodelModeSelectMenu.Init(this.mKeyController, this.mRemodelGradeUpManager.GradeupBtnEnabled);
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfaceRemodelManager.<Start>c__IteratorB5 <Start>c__IteratorB = new UserInterfaceRemodelManager.<Start>c__IteratorB5();
			<Start>c__IteratorB.<>f__this = this;
			return <Start>c__IteratorB;
		}

		private void InitViews(DeckModel[] decks)
		{
			this.mUIRemodelShipSlider.Init(this.mKeyController);
			this.mUIRemodelOtherShipPickerParent.SetActive(true);
			this.mUIRemodelOtherShipPickerParent.Initialize(this.mKeyController);
			this.mUIRemodelDeckSwitchManager.Init(decks, this, this.mKeyController, true);
			this.allSwitchableViews.Add(this.mUIRemodelShipSlider);
			this.allSwitchableViews.Add(this.mUIRemodelLeftShipStatus);
			this.allSwitchableViews.Add(this.mUIRemodelStartRightInfo);
			this.allSwitchableViews.Add(this.mUIRemodelEquipSlotItems);
			this.allSwitchableViews.Add(this.mUIRemodelModeSelectMenu);
			this.allSwitchableViews.Add(this.mUIRemodelDeckSwitchManager);
			this.allSwitchableViews.Add(this.mUIRemodelModernization);
			this.allSwitchableViews.Add(this.mUIRemodelKaizou);
			this.allSwitchableViews.Add(this.mUIRemodelEquipListParent);
			this.allSwitchableViews.Add(this.mUIRemodelSlotItemChangePreview);
			this.allSwitchableViews.Add(this.mCategoryMenu);
			this.allSwitchableViews.Add(this.mUIRemodeModernizationShipTargetListParentNew);
			this.allSwitchableViews.Add(this.mUIRemodelModernizationStartConfirm);
			this.allSwitchableViews.Add(this.mUIRemodelOtherShipPickerParent);
			this.allSwitchableViews.Add(this.commonDeckSwitchManager);
			this.allSwitchableViews.Add(this.mUIRemodelCurrentSlot);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_185_0 = this.viewList;
			ScreenStatus arg_185_1 = ScreenStatus.SELECT_DECK_SHIP;
			List<UIRemodelView> list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelDeckSwitchManager);
			list.Add(this.mUIRemodelShipSlider);
			list.Add(this.mUIRemodelStartRightInfo);
			arg_185_0.Add(arg_185_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_1B0_0 = this.viewList;
			ScreenStatus arg_1B0_1 = ScreenStatus.SELECT_OTHER_SHIP;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelDeckSwitchManager);
			list.Add(this.mUIRemodelOtherShipPickerParent);
			arg_1B0_0.Add(arg_1B0_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_1DB_0 = this.viewList;
			ScreenStatus arg_1DB_1 = ScreenStatus.SELECT_SETTING_MODE;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelModeSelectMenu);
			list.Add(this.mUIRemodelLeftShipStatus);
			arg_1DB_0.Add(arg_1DB_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_206_0 = this.viewList;
			ScreenStatus arg_206_1 = ScreenStatus.MODE_SOUBI_HENKOU;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelEquipSlotItems);
			list.Add(this.mUIRemodelLeftShipStatus);
			arg_206_0.Add(arg_206_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_231_0 = this.viewList;
			ScreenStatus arg_231_1 = ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT;
			list = new List<UIRemodelView>();
			list.Add(this.mCategoryMenu);
			list.Add(this.mUIRemodelCurrentSlot);
			arg_231_0.Add(arg_231_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_250_0 = this.viewList;
			ScreenStatus arg_250_1 = ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelEquipListParent);
			arg_250_0.Add(arg_250_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_26F_0 = this.viewList;
			ScreenStatus arg_26F_1 = ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelSlotItemChangePreview);
			arg_26F_0.Add(arg_26F_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_29A_0 = this.viewList;
			ScreenStatus arg_29A_1 = ScreenStatus.MODE_KINDAIKA_KAISHU;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelLeftShipStatus);
			list.Add(this.mUIRemodelModernization);
			arg_29A_0.Add(arg_29A_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_2C5_0 = this.viewList;
			ScreenStatus arg_2C5_1 = ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelLeftShipStatus);
			list.Add(this.mUIRemodeModernizationShipTargetListParentNew);
			arg_2C5_0.Add(arg_2C5_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_2E5_0 = this.viewList;
			ScreenStatus arg_2E5_1 = ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelModernizationStartConfirm);
			arg_2E5_0.Add(arg_2E5_1, list);
			Dictionary<ScreenStatus, List<UIRemodelView>> arg_305_0 = this.viewList;
			ScreenStatus arg_305_1 = ScreenStatus.MODE_KAIZO;
			list = new List<UIRemodelView>();
			list.Add(this.mUIRemodelKaizou);
			arg_305_0.Add(arg_305_1, list);
		}

		[DebuggerHidden]
		private IEnumerator InitViewsCoroutine(DeckModel[] decks)
		{
			UserInterfaceRemodelManager.<InitViewsCoroutine>c__IteratorB6 <InitViewsCoroutine>c__IteratorB = new UserInterfaceRemodelManager.<InitViewsCoroutine>c__IteratorB6();
			<InitViewsCoroutine>c__IteratorB.decks = decks;
			<InitViewsCoroutine>c__IteratorB.<$>decks = decks;
			<InitViewsCoroutine>c__IteratorB.<>f__this = this;
			return <InitViewsCoroutine>c__IteratorB;
		}

		[DebuggerHidden]
		private IEnumerator StartModernizeAnimation(ShipModelMst targetShipModelMst, int bgID, KeyControl keyController, bool isFail, bool SuperSuccessed, int sozai_count, bool isDamaged, Action OnFinishedAnimation)
		{
			UserInterfaceRemodelManager.<StartModernizeAnimation>c__IteratorB7 <StartModernizeAnimation>c__IteratorB = new UserInterfaceRemodelManager.<StartModernizeAnimation>c__IteratorB7();
			<StartModernizeAnimation>c__IteratorB.keyController = keyController;
			<StartModernizeAnimation>c__IteratorB.targetShipModelMst = targetShipModelMst;
			<StartModernizeAnimation>c__IteratorB.isFail = isFail;
			<StartModernizeAnimation>c__IteratorB.SuperSuccessed = SuperSuccessed;
			<StartModernizeAnimation>c__IteratorB.sozai_count = sozai_count;
			<StartModernizeAnimation>c__IteratorB.isDamaged = isDamaged;
			<StartModernizeAnimation>c__IteratorB.OnFinishedAnimation = OnFinishedAnimation;
			<StartModernizeAnimation>c__IteratorB.<$>keyController = keyController;
			<StartModernizeAnimation>c__IteratorB.<$>targetShipModelMst = targetShipModelMst;
			<StartModernizeAnimation>c__IteratorB.<$>isFail = isFail;
			<StartModernizeAnimation>c__IteratorB.<$>SuperSuccessed = SuperSuccessed;
			<StartModernizeAnimation>c__IteratorB.<$>sozai_count = sozai_count;
			<StartModernizeAnimation>c__IteratorB.<$>isDamaged = isDamaged;
			<StartModernizeAnimation>c__IteratorB.<$>OnFinishedAnimation = OnFinishedAnimation;
			<StartModernizeAnimation>c__IteratorB.<>f__this = this;
			return <StartModernizeAnimation>c__IteratorB;
		}

		[DebuggerHidden]
		private IEnumerator StartGradeUpProductionCoroutine(ShipModel targetShipModel, KeyControl keyController, Action OnFinishedAnimation)
		{
			UserInterfaceRemodelManager.<StartGradeUpProductionCoroutine>c__IteratorB8 <StartGradeUpProductionCoroutine>c__IteratorB = new UserInterfaceRemodelManager.<StartGradeUpProductionCoroutine>c__IteratorB8();
			<StartGradeUpProductionCoroutine>c__IteratorB.keyController = keyController;
			<StartGradeUpProductionCoroutine>c__IteratorB.targetShipModel = targetShipModel;
			<StartGradeUpProductionCoroutine>c__IteratorB.OnFinishedAnimation = OnFinishedAnimation;
			<StartGradeUpProductionCoroutine>c__IteratorB.<$>keyController = keyController;
			<StartGradeUpProductionCoroutine>c__IteratorB.<$>targetShipModel = targetShipModel;
			<StartGradeUpProductionCoroutine>c__IteratorB.<$>OnFinishedAnimation = OnFinishedAnimation;
			<StartGradeUpProductionCoroutine>c__IteratorB.<>f__this = this;
			return <StartGradeUpProductionCoroutine>c__IteratorB;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.backGroundUpperSteelFrame);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.backGroundLowerSteelFrame);
			this.mCamera_TouchEventCatch = null;
			this.mPrefab_PortUpgradesConvertShipManager_KaizouAnimation = null;
			this.mPrefab_PortUpgradesModernizeShipManager_KindaikakaishuAnimation = null;
			this.mUIRemodelHeader = null;
			this.mUIRemodelShipSlider = null;
			this.mUIRemodelShipStatus = null;
			this.mUIRemodelLeftShipStatus = null;
			this.mUIRemodelStartRightInfo = null;
			this.mUIRemodelEquipSlotItems = null;
			this.mUIRemodelModeSelectMenu = null;
			this.mUIRemodelDeckSwitchManager = null;
			this.mUIRemodelModernization = null;
			this.mUIRemodelKaizou = null;
			this.mUIRemodelEquipListParent = null;
			this.mUIRemodelSlotItemChangePreview = null;
			this.mCategoryMenu = null;
			this.mUIRemodeModernizationShipTargetListParentNew = null;
			this.mUIRemodelModernizationStartConfirm = null;
			this.mUIRemodelOtherShipPickerParent = null;
			this.commonDeckSwitchManager = null;
			this.mUIRemodelCurrentSlot = null;
			this.mUIHowTo = null;
			this.okBauxiteUseMessage = null;
			this.okBauxiteUseHighCostMessage = null;
			this.ngBauxiteShortMessage = null;
			this.ngBausiteShortHighCostMessage = null;
			this.mUIDisplaySwipeEventRegion = null;
			UserInterfaceRemodelManager.instance = null;
		}

		public void UpdateHeaderMaterial()
		{
			this.mUIRemodelHeader.RefreshMaterial(this.mRemodelManager);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mRemodelManager);
		}

		private void UpdateHeaderTitle()
		{
			this.mUIRemodelHeader.RefreshTitle(this.status, this.focusedDeckModel);
		}

		private void ShowHeader()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.ReqMode(UIPortFrame.FrameMode.Show);
		}

		private void HideHeader()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.ReqMode(UIPortFrame.FrameMode.Hide);
		}

		public void OnDeckChange(DeckModel deck)
		{
			this.ChangeFocusDeck(deck);
			this.SetStatus((!this.otherShip) ? ScreenStatus.SELECT_DECK_SHIP : ScreenStatus.SELECT_OTHER_SHIP);
			this.UpdateHeaderTitle();
			if (!this.otherShip)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deck;
			}
		}

		public bool IsDeckSelectable(int index, DeckModel deck)
		{
			if (deck == null)
			{
				return this.mRemodelManager.GetOtherShipList().Length > 0;
			}
			return deck.GetShipCount() > 0;
		}

		public bool IsValidShip()
		{
			return this.mRemodelManager.IsValidShip(this.focusedShipModel);
		}

		private void OnSwipeDeckListener(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (this.status == ScreenStatus.SELECT_DECK_SHIP || this.status == ScreenStatus.SELECT_OTHER_SHIP)
				{
					float num = 0.1f;
					if (num < Math.Abs(movePercentageX))
					{
						if (0f < movePercentageX)
						{
							this.commonDeckSwitchManager.ChangePrevDeck();
						}
						else
						{
							this.commonDeckSwitchManager.ChangeNextDeck();
						}
					}
				}
			}
		}

		internal void Forward2SoubiHenkouShortCut(SlotitemModel slotitemModel)
		{
			UserInterfaceRemodelManager.instance.Forward2ModeSelect();
			if (this.mUIRemodelModeSelectMenu.IsValidSlotItemChange())
			{
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkou(slotitemModel, true);
				UserInterfaceRemodelManager.instance.Forward2SoubiHenkouTypeSelect();
			}
			else
			{
				this.mUIRemodelModeSelectMenu.PopUpFailOpenSummary();
			}
		}
	}
}
