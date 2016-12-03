using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class UIShortCutMenu : SingletonMonoBehaviour<UIShortCutMenu>
	{
		private class ButtonInfos
		{
			private UIButton _uiBtn;

			private Generics.Scene _iScene;

			public UIButton Button
			{
				get
				{
					return this._uiBtn;
				}
			}

			public Generics.Scene Scene
			{
				get
				{
					return this._iScene;
				}
			}

			public ButtonInfos(UIButton btn, Generics.Scene iScene)
			{
				this._uiBtn = btn;
				this._iScene = iScene;
			}

			public void SetDisable()
			{
				this._uiBtn.SetState(UIButtonColor.State.Disabled, false);
			}
		}

		[Serializable]
		private class PECamera : Generics.InnerCamera
		{
			private UIPanel _uiPanel;

			private UIButton _uiOverlay;

			private Blur _peBlur;

			public bool IsEffects
			{
				get;
				private set;
			}

			public PECamera(Transform parent, string objName) : base(parent, objName)
			{
				this.IsEffects = false;
				Util.FindParentToChild<UIPanel>(ref this._uiPanel, this._camCamera.get_transform(), "Panel");
				Util.FindParentToChild<UIButton>(ref this._uiOverlay, this._uiPanel.get_transform(), "Overlay");
				this._uiOverlay.isEnabled = false;
				this._camCamera.set_cullingMask(-1);
				this._peBlur = this._camCamera.GetComponent<Blur>();
				this._peBlur.downsample = 1;
				this._peBlur.blurSize = 0f;
				this._peBlur.blurIterations = 1;
				this._peBlur.blurType = 0;
				this._peBlur.set_enabled(false);
			}

			public void EnabledEffects(bool isEnabled)
			{
				this.IsEffects = isEnabled;
				this._peBlur.set_enabled(isEnabled);
			}

			public void EnabledOverlay(bool isEnabled)
			{
				this._uiOverlay.isEnabled = isEnabled;
			}

			public void SetBlurSize(float blurSize)
			{
				this._peBlur.blurSize = blurSize;
			}

			public void LockTouchControl(bool isEnable)
			{
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsFocus && !isEnable)
				{
					return;
				}
				this._camCamera.SetActive(isEnable);
				this._uiPanel.SetActive(isEnable);
				this._uiOverlay.SetActive(isEnable);
				this._uiOverlay.GetComponent<BoxCollider2D>().set_enabled(isEnable);
			}
		}

		private Generics.InnerCamera _camERCamera;

		private UIShortCutMenu.PECamera _camPECamera;

		private UIPanel _uiBtnsPanel;

		private Dictionary<int, UIShortCutMenu.ButtonInfos> _dicBtns;

		[SerializeField]
		private UIShortCutGears gears;

		[SerializeField]
		private UIShortCutTruss truss;

		private bool _isFocus;

		private bool _isInputEnable;

		private bool _isTweenPos;

		private float _fPanelPosXfmCenter;

		private Action _actOperationEnabled;

		private Action _actoperationDisabled;

		private KeyControl _clsInput;

		public bool IsOpen;

		[SerializeField]
		private UIShortCutButtonManager ShortCutBtnManager;

		public bool isCloseAnimNow;

		public bool IsInputEnable
		{
			get
			{
				return this._isInputEnable;
			}
			set
			{
				this._isInputEnable = value;
			}
		}

		public bool IsFocus
		{
			get
			{
				return this._isFocus;
			}
		}

		public List<int> disableButtonList
		{
			get;
			private set;
		}

		protected override void Awake()
		{
			this._isFocus = false;
			this._isInputEnable = true;
			this._isTweenPos = false;
			this._camERCamera = new Generics.InnerCamera(base.get_transform(), "ERCamera");
			this._camPECamera = new UIShortCutMenu.PECamera(base.get_transform(), "PECamera");
			this._uiBtnsPanel = base.get_transform().FindChild("ERCamera/SidePanel").GetComponent<UIPanel>();
			Transform transform = base.get_transform().FindChild("ERCamera/SidePanel/Btns").get_transform();
			this._dicBtns = new Dictionary<int, UIShortCutMenu.ButtonInfos>();
			this._dicBtns.Add(0, new UIShortCutMenu.ButtonInfos(transform.FindChild("StrategyBtn").GetComponent<UIButton>(), Generics.Scene.Strategy));
			this._dicBtns.Add(1, new UIShortCutMenu.ButtonInfos(transform.FindChild("PortTopBtn").GetComponent<UIButton>(), Generics.Scene.PortTop));
			this._dicBtns.Add(2, new UIShortCutMenu.ButtonInfos(transform.FindChild("OrganizeBtn").GetComponent<UIButton>(), Generics.Scene.Organize));
			this._dicBtns.Add(3, new UIShortCutMenu.ButtonInfos(transform.FindChild("SupplyBtn").GetComponent<UIButton>(), Generics.Scene.Supply));
			this._dicBtns.Add(4, new UIShortCutMenu.ButtonInfos(transform.FindChild("RepairBtn").GetComponent<UIButton>(), Generics.Scene.Repair));
			this._dicBtns.Add(5, new UIShortCutMenu.ButtonInfos(transform.FindChild("RemodelBtn").GetComponent<UIButton>(), Generics.Scene.Remodel));
			this._dicBtns.Add(6, new UIShortCutMenu.ButtonInfos(transform.FindChild("ArsenalBtn").GetComponent<UIButton>(), Generics.Scene.Arsenal));
			this._dicBtns.Add(7, new UIShortCutMenu.ButtonInfos(transform.FindChild("RevampBtn").GetComponent<UIButton>(), Generics.Scene.ImprovementArsenal));
			this._dicBtns.Add(8, new UIShortCutMenu.ButtonInfos(transform.FindChild("MissionBtn").GetComponent<UIButton>(), Generics.Scene.Duty));
			this._dicBtns.Add(9, new UIShortCutMenu.ButtonInfos(transform.FindChild("ItemBtn").GetComponent<UIButton>(), Generics.Scene.Item));
			this._dicBtns.Add(10, new UIShortCutMenu.ButtonInfos(transform.FindChild("SaveBtn").GetComponent<UIButton>(), Generics.Scene.SaveLoad));
			int num = 0;
			using (Dictionary<int, UIShortCutMenu.ButtonInfos>.Enumerator enumerator = this._dicBtns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, UIShortCutMenu.ButtonInfos> current = enumerator.get_Current();
					current.get_Value().Button.onClick = Util.CreateEventDelegateList(this, "_decideButton", num);
					num++;
				}
			}
			this._fPanelPosXfmCenter = this._uiBtnsPanel.get_transform().get_localPosition().x;
			this._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
			this._clsInput.setChangeValue(0f, 0f, 0f, 0f);
			this.disableButtonList = new List<int>();
		}

		private void Start()
		{
			this.SetActiveChildren(false);
		}

		private void Update()
		{
			if (this._isInputEnable && this._clsInput != null)
			{
				this._clsInput.Update();
				if (this._clsInput.keyState.get_Item(4).down)
				{
					if (this.IsFocus)
					{
						this.CloseMenu();
						this.LockOffControl();
					}
					else if (!this.isCloseAnimNow)
					{
						this.OpenMenu();
					}
				}
				else if (this._isFocus && this._camPECamera.IsEffects)
				{
					if (this._clsInput.IsDownDown())
					{
						this.ShortCutBtnManager.setSelectedBtn(true);
					}
					else if (this._clsInput.IsUpDown())
					{
						this.ShortCutBtnManager.setSelectedBtn(false);
					}
					else if (this._clsInput.keyState.get_Item(0).down)
					{
						this.OnCancel();
					}
					else if (this._clsInput.keyState.get_Item(1).down)
					{
						this._decideButton(this.ShortCutBtnManager.ButtonManager.nowForcusIndex);
						this._clsInput.ClearKeyAll();
						this._clsInput.firstUpdate = true;
					}
					else if (this._clsInput.keyState.get_Item(5).down)
					{
						this.LockOffControl();
						this.CloseMenu();
					}
				}
			}
		}

		public void OpenMenu()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != MissionStates.NONE)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				CommonPopupDialog.Instance.StartPopup("艦隊は遠征中です");
				return;
			}
			if (!this.IsInputEnable)
			{
				return;
			}
			this.SetActiveChildren(true);
			if (this._actOperationEnabled != null)
			{
				this._actOperationEnabled.Invoke();
			}
			this._clsInput.Index = 0;
			this._camPECamera.EnabledEffects(true);
			this._camPECamera.EnabledOverlay(true);
			this._setTweenPos(true);
			this._isFocus = true;
			App.OnlyController = this._clsInput;
			App.OnlyController.firstUpdate = true;
			this.ShortCutBtnManager.ButtonManager.setAllButtonActive();
			this.setDisable();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.getButton(Generics.Scene.PortTop.ToString()).SetActive(false);
			this.ShortCutBtnManager.HideNowScene();
			this.ShortCutBtnManager.ButtonManager.isDisableFocus = false;
			this.ShortCutBtnManager.ButtonManager.setFocus(0);
			this.ShortCutBtnManager.ButtonManager.isDisableFocus = true;
			this.ShortCutBtnManager.ChangeCursolPos();
			this.IsOpen = true;
			if (SingletonMonoBehaviour<PortObjectManager>.exist() && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_037);
		}

		public void CloseMenu()
		{
			if (!this.IsInputEnable)
			{
				return;
			}
			if (this._actoperationDisabled != null)
			{
				this._actoperationDisabled.Invoke();
			}
			this._isFocus = false;
			this._camPECamera.EnabledEffects(false);
			this._setTweenPos(false);
			if (SingletonMonoBehaviour<PortObjectManager>.exist() && SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
			}
			this.IsOpen = false;
			SoundUtils.PlaySE(SEFIleInfos.SE_037);
		}

		public void OnCancel()
		{
			this.LockOffControl();
			this.CloseMenu();
		}

		public void LockOffControl()
		{
			if (!this.IsFocus)
			{
				this._camPECamera.EnabledOverlay(false);
			}
			App.OnlyController = null;
			App.isFirstUpdate = true;
		}

		public void LockTouchControl(bool isEnable)
		{
			this._camPECamera.LockTouchControl(isEnable);
		}

		private void _setTweenPos(bool isOpen)
		{
			float x;
			if (isOpen)
			{
				x = this._fPanelPosXfmCenter;
				TweenAlpha.Begin(this._uiBtnsPanel.get_gameObject(), 0.4f, 1f);
				this.gears.Enter();
				this.truss.Enter();
			}
			else
			{
				x = this._uiBtnsPanel.get_transform().get_localPosition().x;
				float fPanelPosXfmCenter = this._fPanelPosXfmCenter;
				TweenAlpha.Begin(this._uiBtnsPanel.get_gameObject(), 0.4f, 0f);
				this.gears.Exit();
				this.truss.Exit();
			}
			this._uiBtnsPanel.get_transform().localPositionX(x);
			if (isOpen)
			{
				this._uiBtnsPanel.GetComponent<TweenPosition>().PlayForward();
			}
			else
			{
				this._uiBtnsPanel.GetComponent<TweenPosition>().PlayReverse();
			}
			Hashtable hashtable = new Hashtable();
			float num = (!isOpen) ? 2f : 0f;
			float num2 = (!isOpen) ? 0f : 2f;
			hashtable.Clear();
			hashtable.Add("from", num);
			hashtable.Add("to", num2);
			hashtable.Add("time", 0.4f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			hashtable.Add("onupdate", "_setTweenBlurSize");
			iTween.ValueTo(base.get_gameObject(), hashtable);
			hashtable.Clear();
			this._isTweenPos = true;
		}

		private void setNowSceneButtonDisable()
		{
			for (int i = 0; i < this._dicBtns.get_Values().get_Count(); i++)
			{
				if (this._dicBtns.get_Item(i).Scene.ToString() == SingletonMonoBehaviour<PortObjectManager>.instance.NowScene && !this.disableButtonList.Contains(i))
				{
					this.disableButtonList.Add(i);
				}
			}
		}

		public void setDisable()
		{
			this.disableButtonList.Clear();
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			if (currentDeck == null)
			{
				return;
			}
			bool flag = currentDeck.Count <= 0;
			if (!SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(Generics.Scene.Arsenal))
			{
				this.addDisableButton(6);
				this.addDisableButton(7);
			}
			if (flag || currentDeck.HasBling())
			{
				this.setDisableExceptStrategy();
			}
			this.setDisableWithDock();
			if (SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel != null && SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel.ShipType != 19)
			{
				this.getButton(Generics.Scene.ImprovementArsenal.ToString()).SetActive(false);
			}
			else
			{
				this.getButton(Generics.Scene.Arsenal.ToString()).SetActive(false);
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene.ToLower() == Generics.Scene.Arsenal.ToString().ToLower())
				{
					this.getButton(Generics.Scene.ImprovementArsenal.ToString()).SetActive(false);
				}
			}
			this.setNowSceneButtonDisable();
			this.ShortCutBtnManager.setDisableButton(this.disableButtonList);
		}

		private void setDisableExceptStrategy()
		{
			for (int i = 1; i < this._dicBtns.get_Count(); i++)
			{
				if (i != 2 && i != 10)
				{
					this.addDisableButton(i);
				}
			}
		}

		private void setDisableWithDock()
		{
			if (!SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(Generics.Scene.Repair))
			{
				this.addDisableButton(4);
			}
		}

		private void addDisableButton(int index)
		{
			if (!this.disableButtonList.Contains(index))
			{
				this.disableButtonList.Add(index);
			}
		}

		public UIButton getButton(string sceneName)
		{
			KeyValuePair<int, UIShortCutMenu.ButtonInfos> keyValuePair = Enumerable.FirstOrDefault<KeyValuePair<int, UIShortCutMenu.ButtonInfos>>(this._dicBtns, (KeyValuePair<int, UIShortCutMenu.ButtonInfos> x) => x.get_Value().Scene.ToString().ToLower() == sceneName.ToLower());
			if (keyValuePair.get_Value() != null)
			{
				return keyValuePair.get_Value().Button;
			}
			return null;
		}

		private void _decideButton(int nIndex)
		{
			if (!(this.ShortCutBtnManager.ButtonManager.nowForcusButton.disabledSprite == this.ShortCutBtnManager.ButtonManager.nowForcusButton.hoverSprite))
			{
				this.isCloseAnimNow = true;
				if (PortObjectManager.isPrefabSecene(this._dicBtns.get_Item(nIndex).Scene) && !SingletonMonoBehaviour<PortObjectManager>.Instance.isLoadSecene())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(this._dicBtns.get_Item(nIndex).Scene, false);
				}
				else
				{
					if (this._dicBtns.get_Item(nIndex).Scene == Generics.Scene.SaveLoad)
					{
						Hashtable hashtable = new Hashtable();
						hashtable.Add("rootType", Generics.Scene.Strategy);
						RetentionData.SetData(hashtable);
					}
					SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(this._dicBtns.get_Item(nIndex).Scene);
					this.CloseMenu();
				}
				return;
			}
			string mes = string.Empty;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
			{
				CommonPopupDialog.Instance.StartPopup("艦隊を編成する必要があります");
				return;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				CommonPopupDialog.Instance.StartPopup("撤退中の艦が含まれています");
				return;
			}
			switch (this._dicBtns.get_Item(nIndex).Scene)
			{
			case Generics.Scene.Repair:
				mes = "この海域には入渠ドックがありません";
				break;
			case Generics.Scene.Arsenal:
				mes = "鎮守府海域でのみ選択可能です";
				break;
			case Generics.Scene.ImprovementArsenal:
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID != 1)
				{
					mes = "鎮守府海域でのみ選択可能です";
				}
				else
				{
					mes = "旗艦が工作艦である必要があります";
				}
				break;
			}
			CommonPopupDialog.Instance.StartPopup(mes);
		}

		public void SetJoyStickOperation(bool isEnabled, Action operationEnabled, Action operationDisabled)
		{
			this._isInputEnable = isEnabled;
			if (isEnabled)
			{
				if (this._camPECamera.IsEffects)
				{
					this._isFocus = false;
					this._camPECamera.EnabledEffects(false);
					this._camPECamera.EnabledOverlay(false);
				}
				this._actOperationEnabled = operationEnabled;
				this._actoperationDisabled = operationDisabled;
			}
			else
			{
				this._isFocus = false;
				this._camPECamera.EnabledEffects(false);
				this._camPECamera.EnabledOverlay(false);
				this._actOperationEnabled = null;
				this._actoperationDisabled = null;
			}
		}

		public void OperationDelegateUpdate(Action operationEnabled, Action operationDisabled)
		{
			if (this._actOperationEnabled != operationEnabled)
			{
				this._actOperationEnabled = operationEnabled;
			}
			if (this._actoperationDisabled != operationDisabled)
			{
				this._actoperationDisabled = operationDisabled;
			}
		}

		public void SetDepth(int nDepth)
		{
			this._camPECamera.depth = (float)nDepth;
			this._camERCamera.depth = (float)(nDepth + 1);
		}

		private void _onTweenPosFinished()
		{
			this._isTweenPos = false;
			this._isFocus = true;
		}

		private void _setTweenBlurSize(float fVal)
		{
			this._camPECamera.SetBlurSize(fVal);
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance = null;
			this._dicBtns.Clear();
			this._dicBtns = null;
		}
	}
}
