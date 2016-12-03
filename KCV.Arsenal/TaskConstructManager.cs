using Common.Enum;
using KCV.PopupString;
using KCV.Production;
using KCV.Strategy;
using KCV.Utils;
using local.models;
using Server_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Arsenal
{
	public class TaskConstructManager : SceneTaskMono
	{
		private enum PanelType
		{
			Normal,
			Big,
			Tanker,
			SlotItem
		}

		private enum PanelFocus
		{
			Material,
			MaterialCountPanel,
			HighSpeed,
			Devkit,
			Deside,
			Tanker,
			BigShip
		}

		private enum BtnName
		{
			Fuel = 1,
			Steel,
			DevKit,
			Ammo,
			Baux,
			TankerButton,
			BigShipButton,
			HighSpeed,
			Deside
		}

		public BaseDialogPopup dialogPopUp;

		private ProdReceiveSlotItem _prodReceiveItem;

		private UiSpeedIconManager _speedIconManager;

		private BuildDockModel[] dock;

		private ShipModel[] ships;

		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private GameObject[] _uiMaterialsObj;

		[SerializeField]
		private GameObject[] _uiMaterialFrameObj;

		[SerializeField]
		private UITexture[] _uiMaterialOnFrame;

		[SerializeField]
		public UISprite[] _uiMaterialIcon;

		[SerializeField]
		private UiArsenalMaterialDialog _uiDialog;

		[SerializeField]
		private Arsenal_DevKit _devKitManager;

		[SerializeField]
		private Arsenal_SPoint _sPointManager;

		[SerializeField]
		private UISprite _uiBtnTanker;

		[SerializeField]
		private UISprite _uiBtnBig;

		[SerializeField]
		private GameObject _uiBtnBigObj;

		[SerializeField]
		private UISprite _uiBtnStart;

		private bool isConstructStart;

		private KeyControl KeyController;

		private bool isControl;

		private bool isCreate;

		private bool _isCreateView;

		private int _controllIndex;

		private bool _changeOnce;

		private ButtonLightTexture _uiBtnLightTexture;

		private TaskConstructManager.PanelType nowPanelType;

		private TaskConstructManager.PanelFocus nowPanelState;

		private int dockIndex;

		private readonly int[] changeIndexMaterial = new int[]
		{
			-1,
			0,
			2,
			-1,
			1,
			3,
			-1,
			-1,
			-1,
			-1
		};

		private int[] materialCnt;

		public int[] materialMax;

		public int[] materialMin;

		public int[,] materialPreset;

		[SerializeField]
		private UiArsenalConstBG _BG_touch1;

		[SerializeField]
		private UiArsenalParamBG _BG_touch2;

		private bool isEnd;

		private Color _Orange = new Color(1f, 0.6f, 0f);

		private int[] _material = new int[5];

		private Vector3 KaihatsuPanelPosition = new Vector3(-70f, 0f, 0f);

		[SerializeField]
		private StrategyShipCharacter Live2DRender;

		[SerializeField]
		private GameObject _uiBtnLight;

		private bool isFirst;

		private readonly int[,] normalIndexMap;

		private readonly int[,] normalIndexMapDisableBig;

		private readonly int[,] BigIndexMap;

		private readonly int[,] KaihatsuIndexMap;

		private readonly int[,] TankerIndexMap;

		private bool isBigConstruct
		{
			get
			{
				return this.nowPanelType == TaskConstructManager.PanelType.Big;
			}
		}

		private bool isMaterialCountMode
		{
			get
			{
				return this.nowPanelState == TaskConstructManager.PanelFocus.MaterialCountPanel;
			}
		}

		private bool isSlotItem
		{
			get
			{
				return this.nowPanelType == TaskConstructManager.PanelType.SlotItem;
			}
		}

		public int MaterialIndex
		{
			get
			{
				return this.changeIndexMaterial[this.KeyController.Index];
			}
		}

		public TaskConstructManager()
		{
			int[,] expr_61 = new int[10, 8];
			expr_61[1, 2] = 2;
			expr_61[1, 4] = 4;
			expr_61[2, 2] = 6;
			expr_61[2, 4] = 5;
			expr_61[2, 6] = 1;
			expr_61[4, 0] = 1;
			expr_61[4, 2] = 5;
			expr_61[5, 0] = 2;
			expr_61[5, 2] = 7;
			expr_61[5, 6] = 4;
			expr_61[6, 4] = 7;
			expr_61[6, 6] = 2;
			expr_61[7, 0] = 6;
			expr_61[7, 4] = 8;
			expr_61[7, 6] = 5;
			expr_61[8, 0] = 7;
			expr_61[8, 4] = 9;
			expr_61[8, 6] = 5;
			expr_61[9, 0] = 8;
			expr_61[9, 6] = 5;
			this.normalIndexMap = expr_61;
			int[,] expr_126 = new int[10, 8];
			expr_126[1, 2] = 2;
			expr_126[1, 4] = 4;
			expr_126[2, 2] = 6;
			expr_126[2, 4] = 5;
			expr_126[2, 6] = 1;
			expr_126[4, 0] = 1;
			expr_126[4, 2] = 5;
			expr_126[5, 0] = 2;
			expr_126[5, 2] = 8;
			expr_126[5, 6] = 4;
			expr_126[6, 4] = 8;
			expr_126[6, 6] = 2;
			expr_126[7, 0] = 6;
			expr_126[7, 4] = 8;
			expr_126[7, 6] = 5;
			expr_126[8, 0] = 6;
			expr_126[8, 4] = 9;
			expr_126[8, 6] = 5;
			expr_126[9, 0] = 8;
			expr_126[9, 6] = 5;
			this.normalIndexMapDisableBig = expr_126;
			int[,] expr_1EB = new int[10, 8];
			expr_1EB[1, 2] = 2;
			expr_1EB[1, 4] = 4;
			expr_1EB[2, 2] = 3;
			expr_1EB[2, 4] = 5;
			expr_1EB[2, 6] = 1;
			expr_1EB[3, 4] = 8;
			expr_1EB[3, 6] = 2;
			expr_1EB[4, 0] = 1;
			expr_1EB[4, 2] = 5;
			expr_1EB[5, 0] = 2;
			expr_1EB[5, 2] = 8;
			expr_1EB[5, 6] = 4;
			expr_1EB[8, 0] = 3;
			expr_1EB[8, 4] = 9;
			expr_1EB[8, 6] = 5;
			expr_1EB[9, 0] = 8;
			expr_1EB[9, 6] = 5;
			this.BigIndexMap = expr_1EB;
			int[,] expr_295 = new int[10, 8];
			expr_295[1, 2] = 2;
			expr_295[1, 4] = 4;
			expr_295[2, 2] = 9;
			expr_295[2, 4] = 5;
			expr_295[2, 6] = 1;
			expr_295[4, 0] = 1;
			expr_295[4, 2] = 5;
			expr_295[5, 0] = 2;
			expr_295[5, 2] = 9;
			expr_295[5, 6] = 4;
			expr_295[9, 6] = 5;
			this.KaihatsuIndexMap = expr_295;
			int[,] expr_309 = new int[10, 8];
			expr_309[1, 2] = 2;
			expr_309[1, 4] = 4;
			expr_309[2, 2] = 3;
			expr_309[2, 4] = 5;
			expr_309[2, 6] = 1;
			expr_309[3, 4] = 8;
			expr_309[3, 6] = 2;
			expr_309[4, 0] = 1;
			expr_309[4, 2] = 5;
			expr_309[5, 0] = 2;
			expr_309[5, 2] = 8;
			expr_309[5, 6] = 4;
			expr_309[8, 0] = 3;
			expr_309[8, 4] = 9;
			expr_309[8, 6] = 5;
			expr_309[9, 0] = 8;
			expr_309[9, 6] = 5;
			this.TankerIndexMap = expr_309;
			base..ctor();
		}

		private void Start()
		{
			this.nowPanelState = TaskConstructManager.PanelFocus.Material;
			this.nowPanelType = TaskConstructManager.PanelType.Normal;
		}

		protected override bool Init()
		{
			this.Show(TaskMainArsenalManager.StateType, ArsenalTaskManager._clsArsenal.DockIndex);
			if (TaskMainArsenalManager.StateType == TaskMainArsenalManager.State.KAIHATSU)
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
			}
			SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial, TutorialGuideManager.TutorialID.BuildShip, null);
			return true;
		}

		public void firstInit()
		{
			this.KeyController = new KeyControl(1, 9, 0.4f, 0.1f);
			this.KeyController.Index = 1;
			this.KeyController.HoldJudgeTime = 0.2f;
			this.isConstructStart = false;
			this._controllIndex = 1;
			this.dialogPopUp = ArsenalTaskManager.GetDialogPopUp();
			this._uiPanel = GameObject.Find("ConstructPanel").GetComponent<UIPanel>();
			this._uiBtnTanker = this._uiPanel.get_transform().FindChild("RightPane/BtnTanker").GetComponent<UISprite>();
			this._uiBtnBig = this._uiPanel.get_transform().FindChild("RightPane/BtnBig").GetComponent<UISprite>();
			this._uiBtnBigObj = this._uiBtnBig.get_transform().FindChild("FlameObj").get_gameObject();
			this._uiBtnStart = this._uiPanel.get_transform().FindChild("RightPane/BtnStart").GetComponent<UISprite>();
			this._uiDialog = this._uiPanel.get_transform().FindChild("Dialog").GetComponent<UiArsenalMaterialDialog>();
			this._uiDialog.init(0);
			this._speedIconManager = this._uiPanel.get_transform().FindChild("RightPane/HighSpeed").SafeGetComponent<UiSpeedIconManager>();
			this._speedIconManager.init();
			this._speedIconManager.SetOff();
			this._sPointManager = this._uiPanel.get_transform().FindChild("RightPane/SPoint").SafeGetComponent<Arsenal_SPoint>();
			this._devKitManager = this._uiPanel.get_transform().FindChild("RightPane/Devkit").SafeGetComponent<Arsenal_DevKit>();
			this._uiMaterialsObj = new GameObject[5];
			this.materialCnt = new int[5];
			this.materialMin = new int[5];
			this.materialMax = new int[5];
			this.materialPreset = new int[5, 5];
			for (int i = 0; i < 5; i++)
			{
				this.materialCnt[i] = 0;
			}
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					this.materialPreset[k, j] = -1;
				}
			}
			this._uiMaterialsObj = new GameObject[4];
			this._uiMaterialFrameObj = new GameObject[4];
			this._uiMaterialOnFrame = new UITexture[4];
			this._uiMaterialIcon = new UISprite[4];
			for (int l = 0; l < 4; l++)
			{
				this._uiMaterialsObj[l] = this._uiPanel.get_transform().FindChild("Material" + (l + 1)).get_gameObject();
				this._uiMaterialFrameObj[l] = this._uiMaterialsObj[l].get_transform().FindChild("MaterialFrame1").get_gameObject();
				this._uiMaterialOnFrame[l] = this._uiMaterialsObj[l].get_transform().FindChild("FrameOn").GetComponent<UITexture>();
				this._uiMaterialIcon[l] = this._uiMaterialsObj[l].get_transform().FindChild("Icon").GetComponent<UISprite>();
			}
			this._uiBtnLightTexture = this._uiBtnLight.GetComponent<ButtonLightTexture>();
			this._uiBtnLightTexture.StopAnim();
			this.isFirst = true;
			this._BG_touch1.set_touch(false);
			this._BG_touch2.set_touch(false);
			this._BG_touch2.isEnable = false;
			UIButtonMessage component = this._uiBtnBig.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "_bigButtonEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._uiBtnTanker.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "_tankerButtonEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			this._speedIconManager.SetOff();
			this.Live2DRender.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip());
			this.Live2DRender.get_transform().localPositionX(this.Live2DRender.getEnterPosition().x);
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			this.isCreate = true;
		}

		protected override bool UnInit()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			return true;
		}

		private void OnDestroy()
		{
			this.dialogPopUp = null;
			this._prodReceiveItem = null;
			this._speedIconManager = null;
			Mem.DelAry<BuildDockModel>(ref this.dock);
			Mem.DelAry<ShipModel>(ref this.ships);
			this._uiPanel = null;
			Mem.DelAry<GameObject>(ref this._uiMaterialsObj);
			Mem.DelAry<GameObject>(ref this._uiMaterialFrameObj);
			Mem.DelAry<UITexture>(ref this._uiMaterialOnFrame);
			Mem.DelAry<UISprite>(ref this._uiMaterialIcon);
			this._uiDialog = null;
			this._devKitManager = null;
			this._sPointManager = null;
			this._uiBtnTanker = null;
			this._uiBtnBig = null;
			this._uiBtnBigObj = null;
			this._uiBtnStart = null;
			this.KeyController = null;
			Mem.DelAry<int>(ref this.materialCnt);
			Mem.DelAry<int>(ref this.materialMax);
			Mem.DelAry<int>(ref this.materialMin);
			Mem.DelAry<int>(ref this.materialPreset);
			this._BG_touch1 = null;
			this._BG_touch2 = null;
			Mem.DelAry<int>(ref this._material);
		}

		protected override bool Run()
		{
			if (this.isEnd)
			{
				this.isEnd = false;
				return false;
			}
			if (!this.isControl)
			{
				return true;
			}
			this.KeyController.Update();
			if (this._isCreateView)
			{
				return true;
			}
			if (this.isFirst)
			{
				this.LightButton();
				this.isFirst = false;
			}
			if (this.KeyController.IsChangeIndex)
			{
				this.UpdateSelectBtn();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else
			{
				if (this._BG_touch1.get_touch())
				{
					this.isFirst = true;
					this._closeConstructPanel();
					return false;
				}
				if (this.KeyController.keyState.get_Item(5).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
				else if (!this.isSlotItem && this.KeyController.IsShikakuDown())
				{
					this._speedIconManager.SpeedIconEL(null);
				}
			}
			switch (this.nowPanelState)
			{
			case TaskConstructManager.PanelFocus.Material:
				return this.KeyControlMaterialFocus();
			case TaskConstructManager.PanelFocus.MaterialCountPanel:
				return this.KeyControlMaterialCount();
			case TaskConstructManager.PanelFocus.HighSpeed:
				return this.KeyControlHighSpeed();
			case TaskConstructManager.PanelFocus.Devkit:
				return this.KeyControlDevkit();
			case TaskConstructManager.PanelFocus.Deside:
				return this.KeyControlDeside();
			case TaskConstructManager.PanelFocus.Tanker:
				return this.KeyControlTanker();
			case TaskConstructManager.PanelFocus.BigShip:
				return this.KeyControlBig();
			default:
				return true;
			}
		}

		private void LightButton()
		{
			if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
			{
				if (!this._sPointManager.SPointStarted())
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateTanker(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this._sPointManager.GetUseSpointNum()))
				{
					if (!this._uiBtnLightTexture.NowPlay())
					{
						this._uiBtnLightTexture.PlayAnim();
					}
				}
				else if (this._uiBtnLightTexture.NowPlay())
				{
					this._uiBtnLightTexture.StopAnim();
				}
			}
			else if (this.nowPanelType == TaskConstructManager.PanelType.SlotItem)
			{
				if (this.materialCnt == null)
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateItem(this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3]))
				{
					if (!this._uiBtnLightTexture.NowPlay())
					{
						this._uiBtnLightTexture.PlayAnim();
					}
				}
				else if (this._uiBtnLightTexture.NowPlay())
				{
					this._uiBtnLightTexture.StopAnim();
				}
			}
			else
			{
				if (this.materialCnt == null || false || this._speedIconManager == null)
				{
					return;
				}
				if (TaskMainArsenalManager.arsenalManager.IsValidCreateShip(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this.materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID))
				{
					if (!this._uiBtnLightTexture.NowPlay())
					{
						this._uiBtnLightTexture.PlayAnim();
					}
				}
				else if (this._uiBtnLightTexture.NowPlay())
				{
					this._uiBtnLightTexture.StopAnim();
				}
			}
		}

		private bool KeyControlMaterialFocus()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown() && this.isFocusMaterial())
			{
				this._selectMaterialFrame();
			}
			return true;
		}

		private bool KeyControlMaterialCount()
		{
			if (this.KeyController.IsDownDown())
			{
				this.changeMaterialCnt(true);
			}
			else if (this.KeyController.IsUpDown())
			{
				this.changeMaterialCnt(false);
			}
			else if (this.KeyController.IsLDown())
			{
				this.changeMaterialCntMinMax(false);
			}
			else if (this.KeyController.IsRDown())
			{
				this.changeMaterialCntMinMax(true);
			}
			else if (this.KeyController.IsRightDown())
			{
				this._uiDialog.SetFrameIndex(true, this.isBigConstruct);
			}
			else if (this.KeyController.IsLeftDown())
			{
				this._uiDialog.SetFrameIndex(false, this.isBigConstruct);
			}
			else if (this.KeyController.IsBatuDown() || this.KeyController.IsMaruDown() || this._BG_touch2.get_touch())
			{
				if (this.nowPanelState == TaskConstructManager.PanelFocus.Material)
				{
					return true;
				}
				this._BG_touch2.set_touch(false);
				this.UpdateMaterialCount(this.MaterialIndex);
				this.nowPanelState = TaskConstructManager.PanelFocus.Material;
				this._uiDialog._frameIndex = 0;
				this._uiDialog.HidelDialog();
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this.KeyController.isStopIndex = false;
				this.KeyController.firstUpdate = true;
				this.isFirst = true;
				this.LightButton();
			}
			return true;
		}

		private bool KeyControlHighSpeed()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown())
			{
				this._speedIconManager.SpeedIconEL(null);
			}
			return true;
		}

		private bool KeyControlDevkit()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown())
			{
				if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
				{
					this._sPointManager.NextSwitch();
				}
				else
				{
					this.moveDevkitSwitch();
				}
				this.LightButton();
				this.UpdateSelectBtn();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this.LightButton();
			return true;
		}

		private bool KeyControlDeside()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown())
			{
				bool flag = this.startConstructEL(null);
			}
			return true;
		}

		private bool KeyControlTanker()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown())
			{
				this._tankerButtonEL(null);
			}
			return true;
		}

		private bool KeyControlBig()
		{
			if (this.KeyController.IsBatuDown())
			{
				this._closeConstructPanel();
				return false;
			}
			if (this.KeyController.IsMaruDown())
			{
				this._bigButtonEL(null);
			}
			return true;
		}

		private void _closeConstructPanel()
		{
			this._BG_touch1.set_touch(false);
			this.isFirst = true;
			this.Hide();
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			ArsenalTaskManager._clsArsenal.hideDialog();
		}

		private void _changeConstructPanel(TaskConstructManager.PanelType type)
		{
			this.nowPanelType = type;
			this._changeHidePanel();
			this.LightButton();
		}

		private void _bigButtonEL(GameObject obj)
		{
			if (!TaskMainArsenalManager.arsenalManager.LargeEnabled)
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this._changeConstructPanel(TaskConstructManager.PanelType.Big);
		}

		private void _tankerButtonEL(GameObject obj)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this._changeConstructPanel(TaskConstructManager.PanelType.Tanker);
		}

		public void Show(TaskMainArsenalManager.State state, int num)
		{
			this._uiPanel.alpha = 1f;
			this._BG_touch1.get_transform().set_localScale(Vector3.get_one());
			this._BG_touch2.get_transform().set_localScale(Vector3.get_one());
			this._speedIconManager.SetOff();
			this._speedIconManager.SetBuildKitValue();
			this._uiDialog._frameIndex = 0;
			this.dockIndex = num;
			this.isControl = true;
			this._isCreateView = false;
			this.setUpConstructPanelbyType(state);
			this.setIndexMap();
			this.KeyController.firstUpdate = true;
			this.KeyController.Index = 1;
			this._controllIndex = 1;
			for (int i = 0; i < 4; i++)
			{
				this._uiMaterialFrameObj[i].SetActive(this.isBigConstruct);
			}
			UITexture component = this._uiPanel.get_transform().FindChild("BgBig2_1").GetComponent<UITexture>();
			UITexture component2 = this._uiPanel.get_transform().FindChild("BgBig2_2").GetComponent<UITexture>();
			component.SetActive(this.nowPanelType == TaskConstructManager.PanelType.Big);
			component2.SetActive(this.nowPanelType == TaskConstructManager.PanelType.Big);
			this.UpdateSelectBtn();
			this.UpdateMaterialFrame();
			UISelectedObject.SelectedOneButtonZoomUpDown(this._uiBtnStart.get_gameObject(), false);
			for (int j = 0; j < 4; j++)
			{
				this.UpdateMaterialCount(j);
			}
			this.UpdateSelectBtn();
			Vector3 pos;
			if (state == TaskMainArsenalManager.State.KAIHATSU)
			{
				pos = this.KaihatsuPanelPosition;
				this._uiBtnStart.get_transform().localPositionX(414f);
				this.Live2DRender.SetActive(true);
			}
			else
			{
				pos = Vector3.get_zero();
				this._uiBtnStart.get_transform().localPositionX(380f);
				this.Live2DRender.SetActive(false);
			}
			TweenPosition tweenPosition = TweenPosition.Begin(this._uiPanel.get_gameObject(), 0.3f, pos);
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.LightButton();
		}

		public void Hide()
		{
			this.isControl = false;
			this._BG_touch1.get_transform().set_localScale(Vector3.get_zero());
			this._BG_touch2.get_transform().set_localScale(Vector3.get_zero());
			this._speedIconManager.StopSleepParticle();
			for (int i = 0; i < 4; i++)
			{
				TaskMainArsenalManager.dockMamager[i].EnableParticles();
			}
			TweenPosition tweenPosition = TweenPosition.Begin(this._uiPanel.get_gameObject(), 0.3f, Vector3.get_right() * 877f);
			tweenPosition.eventReceiver = base.get_gameObject();
			tweenPosition.callWhenFinished = "compHide";
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			if (this._speedIconManager.IsHigh)
			{
				base.StartCoroutine(this.SetOff());
			}
		}

		private void compHide()
		{
			this._uiPanel.alpha = 0f;
		}

		private void setUpConstructPanelbyType(TaskMainArsenalManager.State state)
		{
			this.initMaterialCount(state);
			UILabel component = this._uiPanel.get_transform().FindChild("Header/NameLabel").GetComponent<UILabel>();
			this._devKitManager.SetActive(false);
			this._sPointManager.SetActive(false);
			this._speedIconManager.SetActive(false);
			if (state == TaskMainArsenalManager.State.KENZOU)
			{
				component.text = "建造";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				this._uiBtnStart.spriteName = "btn_start";
				this.nowPanelType = TaskConstructManager.PanelType.Normal;
				this._speedIconManager.SetActive(true);
			}
			else if (state == TaskMainArsenalManager.State.KAIHATSU)
			{
				component.text = "開発";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				this.nowPanelType = TaskConstructManager.PanelType.SlotItem;
				this._uiBtnStart.spriteName = "btn_start2";
			}
			else if (state == TaskMainArsenalManager.State.KENZOU_BIG)
			{
				component.text = "大型建造";
				TaskMainArsenalManager.arsenalManager.LargeState = true;
				this._uiBtnStart.spriteName = "btn_start";
				this._speedIconManager.SetActive(true);
				this._devKitManager.SetActive(true);
				this._devKitManager.Init();
				this.nowPanelType = TaskConstructManager.PanelType.Big;
			}
			else if (state == TaskMainArsenalManager.State.YUSOUSEN)
			{
				component.text = "輸送船建造";
				TaskMainArsenalManager.arsenalManager.LargeState = false;
				this._uiBtnStart.spriteName = "btn_start";
				this._speedIconManager.SetActive(true);
				this._sPointManager.SetActive(true);
				this._sPointManager.Init();
				this.nowPanelType = TaskConstructManager.PanelType.Tanker;
			}
		}

		private void _changeShow()
		{
			if (this._changeOnce)
			{
				return;
			}
			TaskMainArsenalManager.State state = TaskMainArsenalManager.State.NONE;
			switch (this.nowPanelType)
			{
			case TaskConstructManager.PanelType.Normal:
				state = TaskMainArsenalManager.State.KENZOU;
				break;
			case TaskConstructManager.PanelType.Big:
				state = TaskMainArsenalManager.State.KENZOU_BIG;
				break;
			case TaskConstructManager.PanelType.Tanker:
				state = TaskMainArsenalManager.State.YUSOUSEN;
				break;
			case TaskConstructManager.PanelType.SlotItem:
				state = TaskMainArsenalManager.State.KAIHATSU;
				break;
			}
			this._changeOnce = true;
			this.Show(state, this.dockIndex);
		}

		[DebuggerHidden]
		private IEnumerator SetOff()
		{
			TaskConstructManager.<SetOff>c__Iterator74 <SetOff>c__Iterator = new TaskConstructManager.<SetOff>c__Iterator74();
			<SetOff>c__Iterator.<>f__this = this;
			return <SetOff>c__Iterator;
		}

		private void _changeHidePanel()
		{
			this.isControl = false;
			this._changeOnce = false;
			this._speedIconManager.StopSleepParticle();
			TweenPosition tweenPosition = TweenPosition.Begin(this._uiPanel.get_gameObject(), 0.3f, Vector3.get_right() * 877f);
			tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
			tweenPosition.SetOnFinished(new EventDelegate.Callback(this._changeShow));
		}

		private bool isFocusMaterial()
		{
			return this.MaterialIndex != -1;
		}

		public void UpdateSelectBtn()
		{
			this.UnsetSelectBtn();
			if (this.isFocusMaterial())
			{
				this.FocusMaterialButton();
			}
			this.updateStartBtn();
			this._controllIndex = this.KeyController.Index;
			if (this.KeyController.Index == 6)
			{
				this._uiBtnTanker.spriteName = "btn_yuso_on";
				this.nowPanelState = TaskConstructManager.PanelFocus.Tanker;
			}
			else if (this.KeyController.Index == 7)
			{
				this._uiBtnBig.spriteName = "btn_big_on";
				this.nowPanelState = TaskConstructManager.PanelFocus.BigShip;
			}
			else if (this.KeyController.Index == 3)
			{
				if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
				{
					this._sPointManager.SetSelecter(true);
				}
				else
				{
					this._devKitManager.SetSelecter(true);
				}
				this.nowPanelState = TaskConstructManager.PanelFocus.Devkit;
			}
			else if (this.KeyController.Index == 8)
			{
				this._speedIconManager.SetSelect(true);
				this.nowPanelState = TaskConstructManager.PanelFocus.HighSpeed;
			}
			else if (this.KeyController.Index == 9)
			{
				this._uiBtnStart.spriteName = ((!this.isSlotItem) ? "btn_start_on" : "btn_start2_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(this._uiBtnStart.get_gameObject(), true);
				this.nowPanelState = TaskConstructManager.PanelFocus.Deside;
			}
			else
			{
				this.nowPanelState = TaskConstructManager.PanelFocus.Material;
			}
		}

		private void FocusMaterialButton()
		{
			this._uiMaterialOnFrame[this.MaterialIndex].alpha = 1f;
		}

		public void UnsetSelectBtn()
		{
			for (int i = 0; i < 4; i++)
			{
				this._uiMaterialOnFrame[i].alpha = 0f;
			}
			this._speedIconManager.SetSelect(false);
			if (this.nowPanelType == TaskConstructManager.PanelType.Normal)
			{
				this._uiBtnTanker.SetActive(true);
				this._uiBtnBig.SetActive(true);
				this._uiBtnBigObj.SetActive(!TaskMainArsenalManager.arsenalManager.LargeEnabled);
			}
			else
			{
				this._uiBtnTanker.SetActive(false);
				this._uiBtnBig.SetActive(false);
			}
			if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
			{
				this._sPointManager.SetSelecter(false);
			}
			if (this.nowPanelType == TaskConstructManager.PanelType.Big)
			{
				this._devKitManager.SetSelecter(false);
			}
			this._uiBtnTanker.spriteName = "btn_yuso";
			this._uiBtnBig.spriteName = "btn_big";
			this._uiBtnStart.spriteName = ((!this.isSlotItem) ? "btn_start" : "btn_start2");
			UISelectedObject.SelectedOneButtonZoomUpDown(this._uiBtnStart.get_gameObject(), false);
		}

		public void updateStartBtn()
		{
			this.isConstructStart = true;
			if (this.isSlotItem)
			{
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateItem(this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3]))
				{
					this.isConstructStart = false;
				}
			}
			else if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
			{
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateTanker(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this._sPointManager.GetUseSpointNum()))
				{
					this.isConstructStart = false;
				}
			}
			else
			{
				this.materialCnt[4] = ((this.nowPanelType != TaskConstructManager.PanelType.Normal) ? this._devKitManager.getUseDevKitNum() : 1);
				if (!TaskMainArsenalManager.arsenalManager.IsValidCreateShip(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this.materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID))
				{
					this.isConstructStart = false;
				}
			}
			if (this.isConstructStart)
			{
				this._uiBtnStart.spriteName = ((!this.isSlotItem) ? "btn_start" : "btn_start2");
				UISelectedObject.SelectedOneButtonZoomUpDown(this._uiBtnStart.get_gameObject(), false);
			}
			this.LightButton();
		}

		public void UpdateMaterialFrame()
		{
			this._uiDialog.ActiveMaterialFrame(this.isBigConstruct);
			this.UpdateDialogMaterialCount();
		}

		public void changeMaterialCntEL(GameObject obj)
		{
			bool isDown = false;
			if (!this.isControl)
			{
				return;
			}
			string name = obj.get_name();
			if (name != null)
			{
				if (TaskConstructManager.<>f__switch$map13 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
					dictionary.Add("ArrowDown1", 0);
					dictionary.Add("ArrowDown2", 1);
					dictionary.Add("ArrowDown3", 2);
					dictionary.Add("ArrowDown4", 3);
					dictionary.Add("ArrowUp1", 4);
					dictionary.Add("ArrowUp2", 5);
					dictionary.Add("ArrowUp3", 6);
					dictionary.Add("ArrowUp4", 7);
					TaskConstructManager.<>f__switch$map13 = dictionary;
				}
				int num;
				if (TaskConstructManager.<>f__switch$map13.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						isDown = true;
						this._uiDialog._frameIndex = 0;
						break;
					case 1:
						isDown = true;
						this._uiDialog._frameIndex = 1;
						break;
					case 2:
						isDown = true;
						this._uiDialog._frameIndex = 2;
						break;
					case 3:
						isDown = true;
						this._uiDialog._frameIndex = 3;
						break;
					case 4:
						this._uiDialog._frameIndex = 0;
						break;
					case 5:
						this._uiDialog._frameIndex = 1;
						break;
					case 6:
						this._uiDialog._frameIndex = 2;
						break;
					case 7:
						this._uiDialog._frameIndex = 3;
						break;
					}
				}
			}
			this._uiDialog.UpdateFrameSelect();
			this.changeMaterialCnt(isDown);
		}

		public void changeMaterialCnt(bool isDown)
		{
			UIPanel component = this._uiDialog._uiMaterialFrame[this._uiDialog._frameIndex].get_transform().FindChild("Panel").GetComponent<UIPanel>();
			UILabel component2 = component.get_transform().FindChild("LabelGrp/Label3").GetComponent<UILabel>();
			if (isDown)
			{
				if (component2.textInt == 0)
				{
					this.materialCnt[this.MaterialIndex] += this._uiDialog.SetMaterialCount() * 9;
				}
				else
				{
					this.materialCnt[this.MaterialIndex] -= this._uiDialog.SetMaterialCount();
				}
			}
			else if (component2.textInt == 9)
			{
				this.materialCnt[this.MaterialIndex] -= this._uiDialog.SetMaterialCount() * 9;
			}
			else
			{
				this.materialCnt[this.MaterialIndex] += this._uiDialog.SetMaterialCount();
			}
			this.isControl = false;
			this._uiDialog.MoveMaterialSlot(!isDown);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this.LightButton();
		}

		public void changeMaterialCntMinMax(bool isMinButton)
		{
			UIPanel component = this._uiDialog._uiMaterialFrame[this._uiDialog._frameIndex].get_transform().FindChild("Panel").GetComponent<UIPanel>();
			UILabel component2 = component.get_transform().FindChild("LabelGrp/Label3").GetComponent<UILabel>();
			if (isMinButton)
			{
				if (this.materialCnt[this.MaterialIndex] == this.materialMin[this.MaterialIndex])
				{
					return;
				}
				this.materialCnt[this.MaterialIndex] = this.materialMin[this.MaterialIndex];
			}
			else
			{
				if (this.materialCnt[this.MaterialIndex] == this.materialMax[this.MaterialIndex])
				{
					return;
				}
				this.materialCnt[this.MaterialIndex] = ((this.materialMax[this.MaterialIndex] >= this._material[this.MaterialIndex]) ? ((this._material[this.MaterialIndex] >= this.materialMin[this.MaterialIndex]) ? this._material[this.MaterialIndex] : this.materialMin[this.MaterialIndex]) : this.materialMax[this.MaterialIndex]);
			}
			this.isControl = false;
			this._uiDialog.MoveMaterialSlot(!isMinButton, true);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		private void setIndexMap()
		{
			switch (this.nowPanelType)
			{
			case TaskConstructManager.PanelType.Normal:
				if (ArsenalTaskManager.GetLogicManager().LargeEnabled)
				{
					this.KeyController.setUseIndexMap(this.normalIndexMap);
				}
				else
				{
					this.KeyController.setUseIndexMap(this.normalIndexMapDisableBig);
				}
				break;
			case TaskConstructManager.PanelType.Big:
				this.KeyController.setUseIndexMap(this.BigIndexMap);
				break;
			case TaskConstructManager.PanelType.Tanker:
				this.KeyController.setUseIndexMap(this.TankerIndexMap);
				break;
			case TaskConstructManager.PanelType.SlotItem:
				this.KeyController.setUseIndexMap(this.KaihatsuIndexMap);
				break;
			}
		}

		public void selectMaterialFrameEL(GameObject obj)
		{
			if (!this.isControl || this.isMaterialCountMode)
			{
				return;
			}
			string name = obj.get_name();
			if (name != null)
			{
				if (TaskConstructManager.<>f__switch$map14 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Material1", 0);
					dictionary.Add("Material2", 1);
					dictionary.Add("Material3", 2);
					dictionary.Add("Material4", 3);
					TaskConstructManager.<>f__switch$map14 = dictionary;
				}
				int num;
				if (TaskConstructManager.<>f__switch$map14.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.KeyController.Index = 1;
						break;
					case 1:
						this.KeyController.Index = 4;
						break;
					case 2:
						this.KeyController.Index = 2;
						break;
					case 3:
						this.KeyController.Index = 5;
						break;
					}
				}
			}
			this.UpdateDialogMaterialCount();
			this.UpdateSelectBtn();
			this._selectMaterialFrame();
		}

		private void _selectMaterialFrame()
		{
			if (this.isMaterialCountMode)
			{
				return;
			}
			this.nowPanelState = TaskConstructManager.PanelFocus.MaterialCountPanel;
			if (this._uiDialog._frameIndex == 0 && !this.isBigConstruct)
			{
				this._uiDialog._frameIndex = 1;
			}
			else
			{
				this._uiDialog._frameIndex = 0;
			}
			this._BG_touch2.get_transform().set_localScale(Vector3.get_one());
			this._uiDialog.ShowDialog(this.MaterialIndex);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.KeyController.isStopIndex = true;
		}

		public void initMaterialCount(TaskMainArsenalManager.State state)
		{
			if (state == TaskMainArsenalManager.State.KENZOU)
			{
				for (int i = 0; i < 4; i++)
				{
					this.materialMin[i] = 30;
					this.materialMax[i] = 999;
					this.materialCnt[i] = ((this.materialPreset[i, 0] != -1) ? this.materialPreset[i, 0] : this.materialMin[i]);
					this.materialPreset[i, 0] = this.materialCnt[i];
				}
			}
			else if (state == TaskMainArsenalManager.State.KAIHATSU)
			{
				for (int j = 0; j < 4; j++)
				{
					this.materialMin[j] = 10;
					this.materialMax[j] = 300;
					this.materialCnt[j] = ((this.materialPreset[j, 1] != -1) ? this.materialPreset[j, 1] : 20);
					this.materialPreset[j, 1] = this.materialCnt[j];
				}
			}
			else if (state == TaskMainArsenalManager.State.KENZOU_BIG)
			{
				this.materialMin[0] = 1500;
				this.materialMin[1] = 1500;
				this.materialMin[2] = 2000;
				this.materialMin[3] = 1000;
				for (int k = 0; k < 4; k++)
				{
					this.materialMax[k] = 7000;
					this.materialCnt[k] = ((this.materialPreset[k, 2] != -1) ? this.materialPreset[k, 2] : this.materialMin[k]);
					this.materialPreset[k, 2] = this.materialCnt[k];
				}
			}
			else if (state == TaskMainArsenalManager.State.YUSOUSEN)
			{
				this.materialMin[0] = 40;
				this.materialMin[1] = 10;
				this.materialMin[2] = 40;
				this.materialMin[3] = 10;
				for (int l = 0; l < 4; l++)
				{
					this.materialMax[l] = 999;
					this.materialCnt[l] = ((this.materialPreset[l, 3] != -1) ? this.materialPreset[l, 3] : this.materialMin[l]);
					this.materialPreset[l, 3] = this.materialCnt[l];
				}
			}
			this.materialMin[4] = 1;
			this.materialMax[4] = 1000;
			this.materialCnt[4] = ((this.materialPreset[4, 4] != -1) ? this.materialPreset[4, 4] : this.materialMin[4]);
			this.materialPreset[4, 4] = this.materialCnt[4];
		}

		public bool CheckDialogMaterialCount(int setCount)
		{
			int num = 0;
			int num2 = 0;
			if (this.KeyController.Index == 1)
			{
				num2 = 0;
				num = TaskMainArsenalManager.arsenalManager.Material.Fuel;
			}
			else if (this.KeyController.Index == 4)
			{
				num2 = 1;
				num = TaskMainArsenalManager.arsenalManager.Material.Ammo;
			}
			else if (this.KeyController.Index == 2)
			{
				num2 = 2;
				num = TaskMainArsenalManager.arsenalManager.Material.Steel;
			}
			else if (this.KeyController.Index == 5)
			{
				num2 = 3;
				num = TaskMainArsenalManager.arsenalManager.Material.Baux;
			}
			else if (this.KeyController.Index == 3 && this.isBigConstruct)
			{
				num2 = 4;
				num = TaskMainArsenalManager.arsenalManager.Material.Devkit;
			}
			return setCount <= num && setCount <= this.materialMax[num2] && setCount >= this.materialMin[num2];
		}

		public void UpdateDialogMaterialCount()
		{
			int nowMaterial;
			switch (this.KeyController.Index)
			{
			case 1:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Fuel;
				break;
			case 2:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Steel;
				break;
			case 3:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Devkit;
				break;
			case 4:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Ammo;
				break;
			case 5:
				nowMaterial = TaskMainArsenalManager.arsenalManager.Material.Baux;
				break;
			default:
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this._uiDialog.MoveMaterialCount(this.materialCnt[this.MaterialIndex], i, nowMaterial);
			}
			this.setMaterialLabel2(this.MaterialIndex, nowMaterial);
		}

		public void UpdateMaterialCount(int num)
		{
			int num2 = this.materialCnt[num];
			this._material[0] = TaskMainArsenalManager.arsenalManager.Material.Fuel;
			this._material[1] = TaskMainArsenalManager.arsenalManager.Material.Ammo;
			this._material[2] = TaskMainArsenalManager.arsenalManager.Material.Steel;
			this._material[3] = TaskMainArsenalManager.arsenalManager.Material.Baux;
			this._material[4] = TaskMainArsenalManager.arsenalManager.Material.Devkit;
			GameObject[] array = new GameObject[4];
			UILabel[] array2 = new UILabel[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = this._uiMaterialsObj[num].get_transform().FindChild("MaterialFrame" + (i + 1)).get_gameObject();
				array2[i] = array[i].get_transform().FindChild("Label").GetComponent<UILabel>();
				array2[i].color = this.setMaterialLabel(num, this._material[num]);
			}
			array2[3].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[2].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[1].text = string.Empty + num2 % 10;
			num2 /= 10;
			array2[0].text = string.Empty + num2 % 10;
			if (this.nowPanelType == TaskConstructManager.PanelType.Normal)
			{
				for (int j = 0; j < 4; j++)
				{
					this.materialPreset[j, 0] = this.materialCnt[j];
				}
			}
			else if (this.nowPanelType == TaskConstructManager.PanelType.SlotItem)
			{
				for (int k = 0; k < 4; k++)
				{
					this.materialPreset[k, 1] = this.materialCnt[k];
				}
			}
			else if (this.nowPanelType == TaskConstructManager.PanelType.Big)
			{
				for (int l = 0; l < 4; l++)
				{
					this.materialPreset[l, 2] = this.materialCnt[l];
				}
			}
			else if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
			{
				for (int m = 0; m < 4; m++)
				{
					this.materialPreset[m, 3] = this.materialCnt[m];
				}
			}
		}

		public void CompMoveMaterialSlot()
		{
			this.UpdateDialogMaterialCount();
			this.isControl = true;
		}

		public void setMaterialCount(int num, int nowMaterial)
		{
			if (this.materialCnt[num] >= nowMaterial)
			{
				this.materialCnt[num] = ((nowMaterial >= this.materialMin[num]) ? nowMaterial : this.materialMin[num]);
			}
			else
			{
				if (this.materialCnt[num] >= this.materialMax[num])
				{
					this.materialCnt[num] = this.materialMax[num];
				}
				if (this.materialCnt[num] <= this.materialMin[num])
				{
					this.materialCnt[num] = this.materialMin[num];
				}
			}
		}

		public Color setMaterialLabel(int num, int nowMaterial)
		{
			if (this.materialCnt[num] > nowMaterial)
			{
				return Color.get_red();
			}
			if (this.materialCnt[num] > this.materialMax[num] || this.materialCnt[num] < this.materialMin[num])
			{
				return this._Orange;
			}
			return Color.get_white();
		}

		public void setMaterialLabel2(int num, int nowMaterial)
		{
			Color color = Color.get_white();
			if (this.materialCnt[num] > nowMaterial)
			{
				color = Color.get_red();
			}
			else if (this.materialCnt[num] > this.materialMax[num] || this.materialCnt[num] < this.materialMin[num])
			{
				color = this._Orange;
			}
			else
			{
				color = Color.get_white();
			}
			for (int i = 0; i < 4; i++)
			{
				UIPanel component = this._uiDialog._uiMaterialFrame[i].get_transform().FindChild("Panel").GetComponent<UIPanel>();
				for (int j = 0; j < 5; j++)
				{
					UILabel component2 = component.get_transform().FindChild("LabelGrp/Label" + (j + 1)).GetComponent<UILabel>();
					component2.color = color;
				}
			}
		}

		public void moveDevkitSwitch()
		{
			this._devKitManager.nextSwitch();
		}

		public bool startConstructEL(GameObject obj)
		{
			if (!this.isControl)
			{
				return false;
			}
			if (this._isCreateView)
			{
				return false;
			}
			if (!this.isSlotItem)
			{
				return this.startKenzouEL();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, this.materialCnt[0]);
			dictionary.Add(enumMaterialCategory.Bull, this.materialCnt[1]);
			dictionary.Add(enumMaterialCategory.Steel, this.materialCnt[2]);
			dictionary.Add(enumMaterialCategory.Bauxite, this.materialCnt[3]);
			if (!TaskMainArsenalManager.arsenalManager.IsValidCreateItem(this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3]))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				bool flag = Enumerable.Any<int>(this.materialCnt, (int x) => x > 300);
				if (flag)
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.MaterialUpperLimit));
				}
				else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitItem));
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
				}
				return true;
			}
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			IReward_Slotitem reward_Slotitem = TaskMainArsenalManager.arsenalManager.CreateItem(this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(TaskMainArsenalManager.arsenalManager);
			this.KeyController.ClearKeyAll();
			this._isCreateView = true;
			TaskMainArsenalManager.isTouchEnable = false;
			TaskMainArsenalManager.IsControl = false;
			bool enabled = reward_Slotitem != null;
			this._prodReceiveItem = ProdReceiveSlotItem.Instantiate(PrefabFile.Load<ProdReceiveSlotItem>(PrefabFileInfos.CommonProdReceiveItem), GameObject.Find("ProdArea").get_transform(), reward_Slotitem, 20, this.KeyController, enabled, true);
			this._prodReceiveItem.SetLayer(13);
			this._prodReceiveItem.Play(new Action(this._onKaihatsuFinished));
			ArsenalTaskManager._clsArsenal.SetNeedRefreshForSlotItemKaitaiList(true);
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			return false;
		}

		public bool startKenzouEL()
		{
			bool flag = false;
			bool flag2;
			if (this.nowPanelType == TaskConstructManager.PanelType.Tanker)
			{
				this.materialCnt[4] = this._sPointManager.GetUseSpointNum();
				BuildDockModel buildDockModel = TaskMainArsenalManager.arsenalManager.CreateTanker(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this.materialCnt[4]);
				flag2 = (buildDockModel != null);
			}
			else
			{
				this.materialCnt[4] = ((this.nowPanelType != TaskConstructManager.PanelType.Normal) ? this._devKitManager.getUseDevKitNum() : 1);
				flag2 = TaskMainArsenalManager.arsenalManager.CreateShip(this.dockIndex + 1, this._speedIconManager.IsHigh, this.materialCnt[0], this.materialCnt[1], this.materialCnt[2], this.materialCnt[3], this.materialCnt[4], SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
				flag = true;
			}
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(TaskMainArsenalManager.arsenalManager);
			if (flag2)
			{
				this.Hide();
				ArsenalTaskManager.ReqPhase(ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
				if (this._speedIconManager.IsHigh)
				{
					for (int i = 0; i < 4; i++)
					{
						if (this.dockIndex == i)
						{
							TaskMainArsenalManager.dockMamager[i].SetFirstHight();
						}
					}
					TutorialModel tutorial = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
					if (tutorial.GetStep() == 1 && !tutorial.GetStepTutorialFlg(2))
					{
						tutorial.SetStepTutorialFlg(2);
						tutorial.SetStepTutorialFlg(3);
						CommonPopupDialog.Instance.StartPopup("「はじめての建造！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
					else if (tutorial.GetStep() == 2 && !tutorial.GetStepTutorialFlg(3))
					{
						tutorial.SetStepTutorialFlg(3);
						CommonPopupDialog.Instance.StartPopup("「高速建造！」 達成");
						ArsenalTaskManager._clsArsenal.DestroyTutorial();
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				ArsenalTaskManager._clsArsenal.hideDialog();
				ArsenalTaskManager._clsArsenal.SetDock();
				SoundUtils.PlaySE(SEFIleInfos.SE_015);
				this.isEnd = true;
				this.isControl = false;
				for (int j = 0; j < 4; j++)
				{
					TaskMainArsenalManager.dockMamager[j].updateSpeedUpIcon();
					TaskMainArsenalManager.dockMamager[j].setSelect(this.dockIndex == j);
				}
				if (flag)
				{
					TutorialModel tutorial2 = ArsenalTaskManager.GetLogicManager().UserInfo.Tutorial;
					if (tutorial2.GetStep() == 1 && !tutorial2.GetStepTutorialFlg(2))
					{
						tutorial2.SetStepTutorialFlg(2);
						CommonPopupDialog.Instance.StartPopup("「はじめての建造！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				return true;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			if (this.nowPanelType != TaskConstructManager.PanelType.Tanker)
			{
				if (Comm_UserDatas.Instance.User_basic.IsMaxChara())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitShip));
				}
				else if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.CannotArsenalByLimitItem));
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
				}
			}
			else if (ArsenalTaskManager.GetLogicManager().UserInfo.SPoint < this._sPointManager.GetUseSpointNum())
			{
				CommonPopupDialog.Instance.StartPopup("戦略ポイントが不足しています");
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughMaterial));
			}
			return false;
		}

		private void _onKaihatsuFinished()
		{
			if (this._prodReceiveItem != null)
			{
				this._prodReceiveItem.ReleaseTextures();
			}
			this._prodReceiveItem = null;
			this._isCreateView = false;
			TaskMainArsenalManager.isTouchEnable = true;
			TaskMainArsenalManager.IsControl = true;
			ArsenalTaskManager._clsArsenal.hideDialog();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
		}
	}
}
