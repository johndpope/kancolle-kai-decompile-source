using Common.Enum;
using Common.Struct;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskArsenalListManager : SceneTaskMono
	{
		public enum State
		{
			NONE,
			ShipSelect,
			ShipDestroyConfirm,
			ShipDestroying,
			SlotItemSelect,
			SlotItemDestroyConfirm,
			SlotItemDestroying
		}

		[SerializeField]
		private UIPanel _bgPanel;

		[SerializeField]
		private GameObject[] _materialObj;

		[SerializeField]
		private UIButton _dismantleBtn;

		[SerializeField]
		private ButtonLightTexture _dismantleBtnLight;

		[SerializeField]
		private UITexture _shipBanner;

		[SerializeField]
		private UITexture _slotItemTex;

		[SerializeField]
		private ArsenalScrollListNew ShipScroll;

		[SerializeField]
		private ArsenalScrollItemListNew ItemScroll;

		[SerializeField]
		private Camera mCamera_TouchEventCatch;

		[SerializeField]
		private Transform mTransform_OverlayButtonForConfirm;

		[SerializeField]
		private UiBreakAnimation _breakMaterialManager;

		[SerializeField]
		private UIShipSortButton mUIShipSortButton;

		private StateManager<TaskArsenalListManager.State> mStateManager;

		private ArsenalManager mArsenalManager;

		private SortKey mSortKey;

		private KeyControl KeyController;

		[SerializeField]
		private Transform _uiOverlay3;

		[SerializeField]
		private Transform _uiOverlay4;

		public bool _ShikakuON;

		protected override void Awake()
		{
			base.Awake();
			this.mStateManager = new StateManager<TaskArsenalListManager.State>(TaskArsenalListManager.State.NONE);
			this.mStateManager.OnPush = new Action<TaskArsenalListManager.State>(this.OnPushState);
			this.mStateManager.OnPop = new Action<TaskArsenalListManager.State>(this.OnPopState);
			this.mStateManager.OnResume = new Action<TaskArsenalListManager.State>(this.OnResumeState);
			this.ShipScroll.SetOnSelectedListener(new Action<ArsenalScrollListChildNew>(this.OnSelectedShipListener));
			this.ItemScroll.SetOnSelectedListener(new Action<ArsenalScrollItemListChildNew>(this.OnSelectedSlotItemListener));
			this.mUIShipSortButton.SetCheckClicableDelegate(new UIShipSortButton.CheckClickable(this.CheckClicableDelegate));
			this.ShipScroll.SetCamera(this.mCamera_TouchEventCatch);
			this._ShikakuON = false;
		}

		private bool CheckClicableDelegate()
		{
			return this.ShipScroll.GetCurrentState() == UIScrollList<ShipModel, ArsenalScrollListChildNew>.ListState.Waiting;
		}

		private void OnPushState(TaskArsenalListManager.State state)
		{
			switch (state)
			{
			case TaskArsenalListManager.State.ShipSelect:
				this.OnPushStateShipSelect();
				break;
			case TaskArsenalListManager.State.ShipDestroyConfirm:
				this.OnPushStateShipDestroyConfirm();
				break;
			case TaskArsenalListManager.State.ShipDestroying:
				this.OnPushStateShipDestroying();
				break;
			case TaskArsenalListManager.State.SlotItemSelect:
				this.OnPushStateSlotItemSelect();
				break;
			case TaskArsenalListManager.State.SlotItemDestroyConfirm:
				this.OnPushStateSlotItemDestroyConfirm();
				break;
			case TaskArsenalListManager.State.SlotItemDestroying:
				this.OnPushStateSlotItemDestroying();
				break;
			}
		}

		private void OnPushStateSlotItemDestroyConfirm()
		{
			this._uiOverlay4.localPositionX(-344f);
			this._dismantleBtn.normalSprite = "btn_haiki_on";
			this._dismantleBtnLight.PlayAnim();
			this._ShikakuON = true;
		}

		private void OnPushStateShipDestroying()
		{
			this._breakMaterialManager.startAnimation();
		}

		private void OnPushStateSlotItemDestroying()
		{
			this._breakMaterialManager.startItemAnimation();
		}

		private void OnPushStateShipDestroyConfirm()
		{
			this._uiOverlay4.localPositionX(-344f);
			this._dismantleBtn.normalSprite = "btn_kaitai_on";
			this._dismantleBtnLight.PlayAnim();
			this._ShikakuON = true;
		}

		private void OnResumeStateShipSelect()
		{
			this._uiOverlay4.localPositionX(614f);
			this._dismantleBtn.normalSprite = "btn_kaitai";
			this.ShipScroll.ResumeControl();
		}

		private void OnResumeStateSlotItemSelect()
		{
			this._uiOverlay4.localPositionX(614f);
			this._dismantleBtn.normalSprite = "btn_haiki";
			this.ItemScroll.ResumeControl();
		}

		private void OnPushStateShipSelect()
		{
			this._uiOverlay4.localPositionX(614f);
			this._uiOverlay3.get_transform().set_localScale(Vector3.get_one());
			this.ShipScroll.SetActive(true);
			this.ItemScroll.SetActive(false);
			this._dismantleBtn.normalSprite = "btn_kaitai";
			this.KeyController.ClearKeyAll();
			this.ShipScroll.SetKeyController(this.KeyController);
			this.ShipScroll.StartControl();
			TweenPosition tweenPosition = TweenPosition.Begin(this._bgPanel.get_gameObject(), 0.3f, Vector3.get_zero());
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}

		private void OnPushStateSlotItemSelect()
		{
			this._uiOverlay4.localPositionX(614f);
			this._uiOverlay3.get_transform().set_localScale(Vector3.get_one());
			this.ShipScroll.SetActive(false);
			this.ItemScroll.SetActive(true);
			this._dismantleBtn.normalSprite = "btn_haiki";
			this.KeyController.ClearKeyAll();
			this.ItemScroll.SetKeyController(this.KeyController);
			this.ItemScroll.StartControl();
			TweenPosition tweenPosition = TweenPosition.Begin(this._bgPanel.get_gameObject(), 0.3f, Vector3.get_zero());
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}

		private void OnResumeState(TaskArsenalListManager.State state)
		{
			switch (state)
			{
			case TaskArsenalListManager.State.ShipSelect:
				this.OnResumeStateShipSelect();
				break;
			case TaskArsenalListManager.State.SlotItemSelect:
				this.OnResumeStateSlotItemSelect();
				break;
			}
		}

		private void OnPopState(TaskArsenalListManager.State state)
		{
			switch (state)
			{
			case TaskArsenalListManager.State.ShipSelect:
				this.ShipScroll.ClearSelected();
				break;
			case TaskArsenalListManager.State.ShipDestroyConfirm:
				this.OnPopStateShipDestroyConfirm();
				break;
			case TaskArsenalListManager.State.SlotItemDestroyConfirm:
				this.OnPopStateSlotItemDestroyConfirm();
				break;
			}
		}

		private void OnPopStateShipDestroyConfirm()
		{
			this._ShikakuON = false;
			this._dismantleBtnLight.StopAnim();
		}

		private void OnPopStateSlotItemDestroyConfirm()
		{
			this._ShikakuON = false;
			this._dismantleBtnLight.StopAnim();
		}

		private void OnSelectedShipListener(ArsenalScrollListChildNew selectedView)
		{
			ShipModel model = selectedView.GetModel();
			this.UpdateKaitaiInfo(model);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void UpdateKaitaiInfo(ShipModel shipModel)
		{
			this.UpdateKaitaiShipInfo(shipModel);
			this.UpdateKaitaiMaterialsInfo(shipModel);
		}

		private void UpdateKaitaiShipInfo(ShipModel shipModel)
		{
			if (shipModel == null)
			{
				this._shipBanner.alpha = 0f;
			}
			else
			{
				int texNum = (!shipModel.IsDamaged()) ? 1 : 2;
				this._shipBanner.alpha = 1f;
				this._shipBanner.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.MstId, texNum);
				this._shipBanner.MakePixelPerfect();
				float num = (float)this._shipBanner.mainTexture.get_width();
				float num2 = 256f / num;
				this._shipBanner.get_transform().localScaleX(num2);
				this._shipBanner.get_transform().localScaleY(num2);
			}
		}

		private void UpdateKaitaiMaterialsInfo(ShipModel setShip)
		{
			if (setShip == null)
			{
				for (int i = 0; i < 4; i++)
				{
					this._materialObj[i].SetActive(false);
				}
			}
			else
			{
				int[] array = new int[4];
				for (int j = 0; j < 4; j++)
				{
					array[j] = 0;
				}
				MaterialInfo resourcesForDestroy = setShip.GetResourcesForDestroy();
				array[0] = resourcesForDestroy.Fuel;
				array[1] = resourcesForDestroy.Ammo;
				array[2] = resourcesForDestroy.Steel;
				array[3] = resourcesForDestroy.Baux;
				for (int k = 0; k < 4; k++)
				{
					if (array[k] > 0)
					{
						this._materialObj[k].SetActive(true);
						UILabel component = this._materialObj[k].get_transform().FindChild("LabelMaterial").GetComponent<UILabel>();
						component.textInt = array[k];
					}
					else
					{
						this._materialObj[k].SetActive(false);
					}
				}
			}
		}

		public void firstInit()
		{
			this.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this._shipBanner.alpha = 0f;
			this._slotItemTex.alpha = 0f;
			this.mArsenalManager = ArsenalTaskManager.GetLogicManager();
			this._breakMaterialManager.init();
			base.Close();
		}

		protected override bool Run()
		{
			if (this.KeyController != null)
			{
				this.KeyController.Update();
				switch (this.mStateManager.CurrentState)
				{
				case TaskArsenalListManager.State.ShipSelect:
					this.OnUpdateShipSelect();
					break;
				case TaskArsenalListManager.State.ShipDestroyConfirm:
					this.OnUpdateShipDestroyConfirm();
					break;
				case TaskArsenalListManager.State.SlotItemSelect:
					this.OnUpdateSlotItemSelect();
					break;
				case TaskArsenalListManager.State.SlotItemDestroyConfirm:
					this.OnUpdateSlotItemDestroyConfirm();
					break;
				}
			}
			return true;
		}

		private void OnUpdateSlotItemDestroyConfirm()
		{
			if (this.KeyController.IsLeftDown())
			{
				this.RequestBackTransitionFromSlotItemDestroyConfirm();
			}
			else if (this.KeyController.IsMaruDown())
			{
				CommonPopupDialog.Instance.StartPopup("廃棄は □ボタンで行います");
			}
			else if (this.KeyController.IsBatuDown())
			{
				this.RequestBackTransitionFromShipDestroyConfirm();
			}
			else if (this.KeyController.IsShikakuDown())
			{
				bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroyConfirm;
				if (flag)
				{
					this.StartHaiki(this.mArsenalManager);
				}
			}
		}

		private void OnUpdateSlotItemSelect()
		{
			if (this.KeyController.IsRightDown())
			{
				this.RequestTransitionForSlotItemDestroyConfirm();
			}
			else if (this.KeyController.IsBatuDown())
			{
				this.RequestBackTransitionFromSlotItemSelect();
			}
		}

		private void RequestTransitionForSlotItemDestroyConfirm()
		{
			bool flag = 0 < this.mArsenalManager.GetSelectedItemsForDetroy().get_Count();
			if (flag)
			{
				this.ItemScroll.LockControl();
				this.mStateManager.PushState(TaskArsenalListManager.State.SlotItemDestroyConfirm);
			}
		}

		private void OnUpdateShipSelect()
		{
			if (this.KeyController.IsRightDown())
			{
				this.RequestTransitionForShipDestroyConfirm();
			}
			else if (this.KeyController.IsBatuDown())
			{
				this.RequestBackTransitionFromShipSelect();
			}
		}

		private void RequestBackTransitionFromShipSelect()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipSelect;
			if (flag)
			{
				this.UpdateKaitaiInfo(null);
				this.mStateManager.PopState();
				this.ShipScroll.LockControl();
				this.Hide();
				base.Close();
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			}
		}

		private void RequestBackTransitionFromSlotItemSelect()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemSelect;
			if (flag)
			{
				this.mArsenalManager.ClearSelectedState();
				this.UpdateHaikiInfo();
				this.mStateManager.PopState();
				this.ItemScroll.LockControl();
				this.Hide();
				base.Close();
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			}
		}

		private void RequestTransitionForShipDestroyConfirm()
		{
			bool flag = this.ShipScroll.SelectedShip != null;
			if (flag)
			{
				this.ShipScroll.LockControl();
				this.mStateManager.PushState(TaskArsenalListManager.State.ShipDestroyConfirm);
			}
		}

		private void OnUpdateShipDestroyConfirm()
		{
			if (this.KeyController.IsLeftDown())
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
			else if (!this.KeyController.IsRightDown())
			{
				if (this.KeyController.IsMaruDown())
				{
					CommonPopupDialog.Instance.StartPopup("解体は □ボタンで行います");
				}
				else if (this.KeyController.IsBatuDown())
				{
					this.RequestBackTransitionFromShipDestroyConfirm();
				}
				else if (this.KeyController.IsShikakuDown())
				{
					bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroyConfirm;
					if (flag)
					{
						ShipModel selectedShip = this.ShipScroll.SelectedShip;
						this.StartKaitai(selectedShip);
					}
				}
			}
		}

		private void OnTouchStartKaitai()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroyConfirm;
			if (flag)
			{
				ShipModel selectedShip = this.ShipScroll.SelectedShip;
				this.StartKaitai(selectedShip);
			}
		}

		[Obsolete("Inspector上で設定して使用したい....")]
		public void OnTouchStartHaiki()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroyConfirm;
			if (flag && 0 < this.mArsenalManager.GetSelectedItemsForDetroy().get_Count())
			{
				this.StartHaiki(this.mArsenalManager);
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchDestroyButton()
		{
			if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroyConfirm)
			{
				this.OnTouchStartKaitai();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroyConfirm)
			{
				this.OnTouchStartHaiki();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchOverlay4()
		{
			if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipSelect)
			{
				this.RequestTransitionForShipDestroyConfirm();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemSelect)
			{
				this.RequestTransitionForSlotItemDestroyConfirm();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroyConfirm)
			{
				this.RequestBackTransitionFromShipDestroyConfirm();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroyConfirm)
			{
				this.RequestBackTransitionFromSlotItemDestroyConfirm();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchOverlay3()
		{
			if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipSelect)
			{
				this.RequestBackTransitionFromShipSelect();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemSelect)
			{
				this.RequestBackTransitionFromSlotItemSelect();
			}
		}

		private void RequestBackTransitionFromShipDestroyConfirm()
		{
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void RequestBackTransitionFromSlotItemDestroyConfirm()
		{
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void StartKaitai(ShipModel shipModel)
		{
			bool flag = this.mArsenalManager.IsValidBreakShip(shipModel);
			if (flag)
			{
				bool flag2 = this.mArsenalManager.BreakShip(shipModel.MemId);
				if (flag2)
				{
					this.ShipScroll.LockControl();
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.mStateManager.PushState(TaskArsenalListManager.State.ShipDestroying);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("艦がロックされています");
				}
			}
		}

		private void StartHaiki(ArsenalManager arsenalManager)
		{
			bool flag = this.mArsenalManager.IsValidBreakItem();
			if (flag)
			{
				bool flag2 = this.mArsenalManager.BreakItem();
				if (flag2)
				{
					this.ItemScroll.LockControl();
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.mStateManager.PushState(TaskArsenalListManager.State.SlotItemDestroying);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("ロックされている装備があります");
				}
			}
		}

		private void OnSelectedSlotItemListener(ArsenalScrollItemListChildNew selectedView)
		{
			this.mArsenalManager.GetSelectedItemsForDetroy();
			int memId = selectedView.GetModel().GetSlotItemModel().MemId;
			this.mArsenalManager.ToggleSelectedState(memId);
			this.ItemScroll.UpdateChoiceModelAndView(selectedView.GetRealIndex(), selectedView.GetModel().GetSlotItemModel());
			this.UpdateHaikiInfo();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void UpdateHaikiInfo()
		{
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = 0;
			}
			MaterialInfo materialsForBreakItem = this.mArsenalManager.GetMaterialsForBreakItem();
			array[0] = materialsForBreakItem.Fuel;
			array[1] = materialsForBreakItem.Ammo;
			array[2] = materialsForBreakItem.Steel;
			array[3] = materialsForBreakItem.Baux;
			for (int j = 0; j < 4; j++)
			{
				if (array[j] > 0)
				{
					this._materialObj[j].SetActive(true);
					UILabel component = this._materialObj[j].get_transform().FindChild("LabelMaterial").GetComponent<UILabel>();
					component.SetActive(true);
					component.textInt = array[j];
				}
				else
				{
					this._materialObj[j].SetActive(false);
				}
			}
			List<SlotitemModel> selectedItemsForDetroy = this.mArsenalManager.GetSelectedItemsForDetroy();
			int count = selectedItemsForDetroy.get_Count();
			if (count != 0)
			{
				if (count != 1)
				{
					this._slotItemTex.mainTexture = (Resources.Load("Textures/Arsenal/kaitai_haiki/icon_haiki") as Texture2D);
					this._slotItemTex.width = 162;
					this._slotItemTex.height = 102;
					this._slotItemTex.alpha = 1f;
				}
				else
				{
					SlotitemModel slotitemModel = selectedItemsForDetroy.get_Item(0);
					this.UnloadSlotItemTex();
					this._slotItemTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotitemModel.MstId, 2);
					this._slotItemTex.width = 144;
					this._slotItemTex.height = 215;
					this._slotItemTex.alpha = 1f;
				}
			}
			else
			{
				this._slotItemTex.alpha = 0f;
			}
		}

		private void UnloadSlotItemTex()
		{
			if (this._slotItemTex != null)
			{
				if (this._slotItemTex.mainTexture != null)
				{
					Resources.UnloadAsset(this._slotItemTex.mainTexture);
				}
				this._slotItemTex.mainTexture = null;
			}
		}

		public void CloseShipDialog()
		{
			this._shipBanner.alpha = 0f;
			this._dismantleBtn.normalSprite = "btn_kaitai";
		}

		private void CloseHaikiDialog()
		{
			this._slotItemTex.alpha = 0f;
			this._dismantleBtn.normalSprite = "btn_haiki";
		}

		private void HideMaterialsInfo()
		{
			for (int i = 0; i < 4; i++)
			{
				this._materialObj[i].SetActive(false);
			}
		}

		public void StartStateKaitai()
		{
			this.mUIShipSortButton.SetActive(true);
			this.mUIShipSortButton.SetClickable(true);
			this.KeyController.ClearKeyAll();
			this.KeyController.firstUpdate = true;
			this.mStateManager.ClearStates();
			this.mArsenalManager.ClearSelectedState();
			this.mStateManager.PushState(TaskArsenalListManager.State.ShipSelect);
		}

		public void StartStateKaitaiAtFirst()
		{
			ShipModel[] shipList = this.mArsenalManager.GetShipList();
			this.ShipScroll.Initialize(shipList);
			this.StartStateKaitai();
		}

		public void StartStateHaiki()
		{
			this.ItemScroll.ClearChecked();
			this.mUIShipSortButton.SetActive(false);
			this.mUIShipSortButton.SetClickable(false);
			this.KeyController.ClearKeyAll();
			this.KeyController.firstUpdate = true;
			this.mStateManager.ClearStates();
			this.mArsenalManager.ClearSelectedState();
			this.mStateManager.PushState(TaskArsenalListManager.State.SlotItemSelect);
		}

		internal void StartStateHaikiAtFirst()
		{
			SlotitemModel[] unsetSlotitems = this.mArsenalManager.GetUnsetSlotitems();
			this.ItemScroll.Initialize(this.mArsenalManager, unsetSlotitems, this.mCamera_TouchEventCatch);
			this.mUIShipSortButton.SetActive(false);
			this.mUIShipSortButton.SetClickable(false);
			this.KeyController.ClearKeyAll();
			this.KeyController.firstUpdate = true;
			this.mStateManager.ClearStates();
			this.mArsenalManager.ClearSelectedState();
			this.mStateManager.PushState(TaskArsenalListManager.State.SlotItemSelect);
		}

		public void Hide()
		{
			this._uiOverlay3.get_transform().set_localScale(Vector3.get_zero());
			TweenPosition tweenPosition = TweenPosition.Begin(this._bgPanel.get_gameObject(), 0.3f, Vector3.get_right() * 877f);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this._dismantleBtnLight.StopAnim();
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		}

		public void compBreakAnimation()
		{
			if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroying)
			{
				this.OnCompleteShipBreakAnimation();
			}
			else if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroying)
			{
				this.OnCompleteSlotItemBreakAnimation();
			}
		}

		private void OnCompleteSlotItemBreakAnimation()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroying;
			if (flag)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mArsenalManager);
				SlotitemModel[] unsetSlotitems = this.mArsenalManager.GetUnsetSlotitems();
				this.UpdateHaikiInfo();
				this.ItemScroll.Refresh(unsetSlotitems);
				this.ItemScroll.SetKeyController(this.KeyController);
				this.ItemScroll.StartControl();
				this.CloseHaikiDialog();
				this.mStateManager.PopState();
				if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemDestroyConfirm)
				{
					this.mStateManager.PopState();
				}
				if (this.mStateManager.CurrentState == TaskArsenalListManager.State.SlotItemSelect)
				{
					this.mStateManager.ResumeState();
				}
				TrophyUtil.Unlock_Material();
			}
		}

		private void OnCompleteShipBreakAnimation()
		{
			bool flag = this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroying;
			if (flag)
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mArsenalManager);
				ShipModel[] shipList = this.mArsenalManager.GetShipList();
				this.UpdateKaitaiInfo(null);
				int focusModelIndex = this.ShipScroll.GetFocusModelIndex();
				this.ShipScroll.Refresh(shipList);
				this.ShipScroll.SetKeyController(this.KeyController);
				this.ShipScroll.StartControl();
				this.CloseShipDialog();
				this.mStateManager.PopState();
				if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipDestroyConfirm)
				{
					this.mStateManager.PopState();
				}
				if (this.mStateManager.CurrentState == TaskArsenalListManager.State.ShipSelect)
				{
					this.mStateManager.ResumeState();
				}
				TrophyUtil.Unlock_Material();
			}
		}

		private void OnDestroy()
		{
			this._bgPanel = null;
			this._materialObj = null;
			this._dismantleBtn = null;
			this._dismantleBtnLight = null;
			this._shipBanner = null;
			this._slotItemTex = null;
			this.ShipScroll = null;
			this.ItemScroll = null;
			this.mCamera_TouchEventCatch = null;
			this.mTransform_OverlayButtonForConfirm = null;
			this._breakMaterialManager = null;
			this.mUIShipSortButton = null;
			this.mStateManager = null;
			this.mArsenalManager = null;
			this.KeyController = null;
			this._uiOverlay3 = null;
			this._uiOverlay4 = null;
		}
	}
}
