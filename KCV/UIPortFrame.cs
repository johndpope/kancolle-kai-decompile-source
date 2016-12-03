using Common.Struct;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UIPortFrame : SingletonMonoBehaviour<UIPortFrame>
	{
		public enum FrameMode
		{
			None,
			Hide,
			Show
		}

		[Serializable]
		private class AdmiralInfos
		{
			private GameObject _uiInfoObj;

			private UILabel _uiName;

			private UILabel _uiRank;

			private UILabel _uiSenryakuVal;

			private UILabel _uiUserLevel;

			public AdmiralInfos(Transform parent)
			{
				Util.FindParentToChild(ref this._uiInfoObj, parent, "AdmiralInfos");
				Util.FindParentToChild<UILabel>(ref this._uiName, this._uiInfoObj, "Name");
				Util.FindParentToChild<UILabel>(ref this._uiSenryakuVal, this._uiInfoObj, "SenryakuVal");
				Util.FindParentToChild<UILabel>(ref this._uiUserLevel, this._uiInfoObj, "UserLevel");
			}

			public bool UnInit()
			{
				this._uiName = null;
				this._uiRank = null;
				this._uiSenryakuVal = null;
				this._uiUserLevel = null;
				return true;
			}

			public void UpdateInfo(UserInfoModel userInfo)
			{
				if (userInfo == null)
				{
					return;
				}
				this._uiName.text = userInfo.Name;
				this._uiName.supportEncoding = false;
				this._uiUserLevel.text = string.Format("Lv. {0}", userInfo.Level);
				this._uiSenryakuVal.text = userInfo.SPoint.ToString();
			}
		}

		[Serializable]
		private class MaterialInfos
		{
			private GameObject _uiInfoObj;

			private UILabel _uiFuel;

			private UILabel _uiAmmo;

			private UILabel _uiSteel;

			private UILabel _uiBaux;

			private UILabel _uiRepairKit;

			private UILabel _uiDevKit;

			public MaterialInfos(Transform parent)
			{
				Util.FindParentToChild(ref this._uiInfoObj, parent, "MaterialInfos");
				Util.FindParentToChild<UILabel>(ref this._uiFuel, this._uiInfoObj, "FuelVal");
				Util.FindParentToChild<UILabel>(ref this._uiAmmo, this._uiInfoObj, "AmmoVal");
				Util.FindParentToChild<UILabel>(ref this._uiSteel, this._uiInfoObj, "SteelVal");
				Util.FindParentToChild<UILabel>(ref this._uiBaux, this._uiInfoObj, "BauxVal");
				Util.FindParentToChild<UILabel>(ref this._uiRepairKit, this._uiInfoObj, "RepairKitVal");
				Util.FindParentToChild<UILabel>(ref this._uiDevKit, this._uiInfoObj, "DebKitVal");
			}

			public bool UnInit()
			{
				this._uiAmmo = null;
				this._uiBaux = null;
				this._uiDevKit = null;
				this._uiFuel = null;
				this._uiInfoObj = null;
				this._uiRepairKit = null;
				this._uiSteel = null;
				return true;
			}

			public void UpdateInfo(int naturalRecoverMaterialMax, MaterialModel info)
			{
				if (info == null)
				{
					return;
				}
				if (naturalRecoverMaterialMax <= info.Fuel)
				{
					this._uiFuel.color = Color.get_yellow();
				}
				else
				{
					this._uiFuel.color = Color.get_white();
				}
				this._uiFuel.text = info.Fuel.ToString();
				if (naturalRecoverMaterialMax <= info.Ammo)
				{
					this._uiAmmo.color = Color.get_yellow();
				}
				else
				{
					this._uiAmmo.color = Color.get_white();
				}
				this._uiAmmo.text = info.Ammo.ToString();
				if (naturalRecoverMaterialMax <= info.Steel)
				{
					this._uiSteel.color = Color.get_yellow();
				}
				else
				{
					this._uiSteel.color = Color.get_white();
				}
				this._uiSteel.text = info.Steel.ToString();
				if (naturalRecoverMaterialMax <= info.Baux)
				{
					this._uiBaux.color = Color.get_yellow();
				}
				else
				{
					this._uiBaux.color = Color.get_white();
				}
				this._uiBaux.text = info.Baux.ToString();
				this._uiRepairKit.text = info.RepairKit.ToString();
				this._uiDevKit.text = info.Devkit.ToString();
			}
		}

		[Serializable]
		private class Circles
		{
			private UIButton _uiCircleBtn;

			private UITexture _uiModeSprite;

			private UITexture _uiModeSpriteBlur;

			public Transform transform
			{
				get
				{
					return this._uiCircleBtn.get_transform();
				}
			}

			public UIButton circleButton
			{
				get
				{
					return this._uiCircleBtn;
				}
			}

			public UITexture circleLabel
			{
				get
				{
					return this._uiModeSprite;
				}
			}

			public UITexture circleLabelBlur
			{
				get
				{
					return this._uiModeSpriteBlur;
				}
			}

			public Circles(Transform parent)
			{
				Util.FindParentToChild<UIButton>(ref this._uiCircleBtn, parent, "CirclesBtn");
				Util.FindParentToChild<UITexture>(ref this._uiModeSprite, this._uiCircleBtn.get_transform(), "Circle/Texture_Name");
				Util.FindParentToChild<UITexture>(ref this._uiModeSpriteBlur, this._uiCircleBtn.get_transform(), "Circle/Texture_Name_Blur");
			}

			public bool Init()
			{
				return true;
			}

			public bool UnInit()
			{
				this._uiCircleBtn = null;
				this._uiModeSprite = null;
				this._uiModeSpriteBlur = null;
				return true;
			}

			public void Update()
			{
				this._uiModeSprite.get_transform().get_parent().Rotate(new Vector3(0f, 0f, 10f) * -Time.get_deltaTime());
			}

			public void UpdateInfo(ManagerBase manager)
			{
				if (this._uiModeSprite.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiModeSprite.mainTexture);
					this._uiModeSprite.mainTexture = null;
				}
				if (this._uiModeSpriteBlur.mainTexture != null)
				{
					Resources.UnloadAsset(this._uiModeSpriteBlur.mainTexture);
					this._uiModeSpriteBlur.mainTexture = null;
				}
				this._uiModeSprite.mainTexture = this.RequestHeaderTitle(manager);
				this._uiModeSpriteBlur.mainTexture = this.RequestHeaderTitleBlur(manager);
			}

			private Texture RequestHeaderTitle(ManagerBase manager)
			{
				if (manager is OrganizeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_organize");
				}
				if (manager is RepairManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_repair");
				}
				if (manager is SupplyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_supply");
				}
				if (manager is ArsenalManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal");
				}
				if (manager is RevampManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal2");
				}
				if (manager is AlbumManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_album");
				}
				if (manager is DutyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_duty");
				}
				if (manager is PracticeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensyu");
				}
				if (manager is MissionManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensei");
				}
				if (manager is ItemlistManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item");
				}
				if (manager is ItemStoreManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item");
				}
				if (manager is RecordManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_record");
				}
				if (manager is InteriorManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_reform");
				}
				if (manager is RemodelManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_remodal");
				}
				return Resources.Load<Texture>("Textures/PortHeader/header_icon_bokou");
			}

			private Texture RequestHeaderTitleBlur(ManagerBase manager)
			{
				if (manager is OrganizeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_organize_b");
				}
				if (manager is RepairManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_repair_b");
				}
				if (manager is SupplyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_supply_b");
				}
				if (manager is ArsenalManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal_b");
				}
				if (manager is RevampManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_arsenal2_b");
				}
				if (manager is AlbumManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_album_b");
				}
				if (manager is DutyManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_duty_b");
				}
				if (manager is PracticeManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensyu_b");
				}
				if (manager is MissionManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_ensei_b");
				}
				if (manager is ItemlistManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item_b");
				}
				if (manager is ItemStoreManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_item_b");
				}
				if (manager is RecordManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_record_b");
				}
				if (manager is InteriorManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_reform_b");
				}
				if (manager is RemodelManager)
				{
					return Resources.Load<Texture>("Textures/PortHeader/header_icon_remodal_b");
				}
				return Resources.Load<Texture>("Textures/PortHeader/header_icon_bokou_b");
			}
		}

		[Serializable]
		private class DateTime
		{
			[SerializeField]
			private UILabel _uiDateTime;

			public DateTime(Transform parent)
			{
				Util.FindParentToChild<UILabel>(ref this._uiDateTime, parent, "Time");
			}

			public void UpdateInfo(TurnString time)
			{
				this._uiDateTime.fontSize = 20;
				this._uiDateTime.text = string.Format("[323232]{0}の年 {1}[b]{2}[/b]日 {3}[-]", new object[]
				{
					time.Year,
					time.Month,
					time.Day,
					UIPortFrame.getDayOfWeekJapanese(time.DayOfWeek)
				});
			}
		}

		private const float CIRCLEMODE_ROT_SPD = 10f;

		private const float CIRCLE2_ROT_SPD = 5f;

		private const float CIRCLE2_ROT_INTERVAL = 2f;

		private const float TRANSITION_BTN_OFFSET_X = 165f;

		private const float TOPFRAME_MOVE_TIME = 0.8f;

		private UIPanel _uiPanel;

		private GameObject _uiHeader;

		private GameObject _uiFrame;

		private UIPortFrame.Circles _clsCircles;

		private UIPortFrame.AdmiralInfos _clsAdmiralInfo;

		private UIPortFrame.MaterialInfos _clsMaterialInfo;

		private UIPortFrame.DateTime _clsDateTime;

		private Generics.InnerCamera _clsCam;

		private Vector3[] _vHeaderPos = new Vector3[]
		{
			new Vector3(-960f, 232f, 0f),
			new Vector3(-0f, 232f, 0f)
		};

		private Vector3[] _vFramePos = new Vector3[]
		{
			new Vector3(-960f, -273f, 0f),
			new Vector3(-480f, -273f, 0f)
		};

		private bool _isForcus;

		private bool isInitialized;

		private Action mOnClickCircleButtonListener;

		public bool IsForcus
		{
			get
			{
				return this._isForcus;
			}
			set
			{
				if (value)
				{
					if (!this._isForcus)
					{
						this._isForcus = true;
					}
				}
				else if (this._isForcus)
				{
					this._isForcus = false;
				}
			}
		}

		public float alpha
		{
			get
			{
				if (this._uiPanel != null)
				{
					return this._uiPanel.alpha;
				}
				return -1f;
			}
			set
			{
				if (this._uiPanel != null)
				{
					this._uiPanel.alpha = value;
				}
				else
				{
					Debug.LogWarning("Not Found _uiPanel");
				}
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return this._clsCircles.circleButton.isEnabled;
			}
			set
			{
				this._clsCircles.circleButton.isEnabled = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this._isForcus = false;
			Util.SetRootContentSize(base.GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			Util.FindParentToChild<UIPanel>(ref this._uiPanel, base.get_transform(), "Panel");
			Util.FindParentToChild(ref this._uiHeader, this._uiPanel.get_transform(), "Header");
			this._uiHeader.get_transform().set_localPosition(this._vHeaderPos[0]);
			Util.FindParentToChild(ref this._uiFrame, this._uiPanel.get_transform(), "Frame");
			this._uiFrame.get_transform().set_localPosition(this._vFramePos[0]);
			this._clsCircles = new UIPortFrame.Circles(this._uiPanel.get_transform());
			this._clsCircles.circleButton.onClick = Util.CreateEventDelegateList(this, "onCircleBtnClick", null);
			this._clsAdmiralInfo = new UIPortFrame.AdmiralInfos(this._uiHeader.get_transform());
			this._clsMaterialInfo = new UIPortFrame.MaterialInfos(this._uiHeader.get_transform());
			this._clsCam = new Generics.InnerCamera(base.get_transform().FindChild("FrameCamera"));
			this._clsDateTime = new UIPortFrame.DateTime(this._uiPanel.get_transform().FindChild("Frame"));
		}

		private void Start()
		{
			this.Init();
			this._portframeScreenIn();
			this.UpdateHeaderInfo();
		}

		private void OnDestroy()
		{
			this.UnInit();
		}

		private void Update()
		{
			this._clsCircles.Update();
		}

		public bool Init()
		{
			this._clsCircles.Init();
			return true;
		}

		public bool UnInit()
		{
			this._isForcus = false;
			this._clsCircles.UnInit();
			this._clsAdmiralInfo.UnInit();
			this._clsMaterialInfo.UnInit();
			this._uiPanel = null;
			this._uiHeader = null;
			return true;
		}

		public void Discard()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.instance != null)
			{
				Object.Destroy(base.get_gameObject());
			}
		}

		public void UpdateHeaderInfo(ManagerBase manager)
		{
			this._clsAdmiralInfo.UpdateInfo(manager.UserInfo);
			this._clsMaterialInfo.UpdateInfo(manager.UserInfo.GetMaterialMaxNum(), manager.Material);
			this._clsDateTime.UpdateInfo(manager.DatetimeString);
		}

		public void UpdateHeaderInfo()
		{
			this.UpdateHeaderInfo(new PortManager(SingletonMonoBehaviour<AppInformation>.instance.CurrentAreaID));
		}

		public void CircleUpdateInfo(ManagerBase manager)
		{
			this._clsCircles.UpdateInfo(manager);
			this.fadeInCircleButtonLabel();
		}

		private void onCircleBtnClick()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			if (SingletonMonoBehaviour<PortObjectManager>.Instance.NowScene == Generics.Scene.PortTop.ToString())
			{
				this.OnClickCircleButton();
			}
			else
			{
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
				{
					CommonPopupDialog.Instance.StartPopup("艦隊を編成する必要があります");
					return;
				}
				this.isColliderEnabled = false;
				if (PortObjectManager.SceneChangeAct != null)
				{
					PortObjectManager.SceneChangeAct.Invoke();
					PortObjectManager.SceneChangeAct = null;
				}
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.Strategy, false);
				}
				else if (SingletonMonoBehaviour<PortObjectManager>.Instance.IsLoadLevelScene)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.PortTop);
				}
				else
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.InstantiateScene(Generics.Scene.PortTop, false);
				}
			}
		}

		[Obsolete("UserInterfacePortManagerで使用します。")]
		public void SetOnClickCircleButtoListener(Action onClickCircleButtonListener)
		{
			this.mOnClickCircleButtonListener = onClickCircleButtonListener;
		}

		[Obsolete("UserInterfacePortManagerで使用します。")]
		private void OnClickCircleButton()
		{
			if (this.mOnClickCircleButtonListener != null)
			{
				this.mOnClickCircleButtonListener.Invoke();
			}
		}

		public void ReqMode(UIPortFrame.FrameMode iMode)
		{
			if (iMode == UIPortFrame.FrameMode.None)
			{
				return;
			}
			if (iMode != UIPortFrame.FrameMode.Hide)
			{
				if (iMode == UIPortFrame.FrameMode.Show)
				{
					this._clsCam.sameMask = Generics.Layers.UI2D;
				}
			}
			else
			{
				this._clsCam.sameMask = Generics.Layers.Nothing;
			}
		}

		public static string getDayOfWeekJapanese(string dow)
		{
			if (dow != null)
			{
				if (UIPortFrame.<>f__switch$map11 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
					dictionary.Add("Sunday", 0);
					dictionary.Add("Monday", 1);
					dictionary.Add("Tuesday", 2);
					dictionary.Add("Wednesday", 3);
					dictionary.Add("Thursday", 4);
					dictionary.Add("Friday", 5);
					dictionary.Add("Saturday", 6);
					UIPortFrame.<>f__switch$map11 = dictionary;
				}
				int num;
				if (UIPortFrame.<>f__switch$map11.TryGetValue(dow, ref num))
				{
					switch (num)
					{
					case 0:
						return "(日)";
					case 1:
						return "(月)";
					case 2:
						return "(火)";
					case 3:
						return "(水)";
					case 4:
						return "(木)";
					case 5:
						return "(金)";
					case 6:
						return "(土)";
					}
				}
			}
			return string.Empty;
		}

		public void setVisibleHeader(bool isVisible)
		{
			float alpha = (float)((!isVisible) ? 0 : 1);
			TweenAlpha.Begin(this._uiHeader, 0.2f, alpha);
		}

		public void fadeOutCircleButtonLabel()
		{
			TweenAlpha.Begin(this._clsCircles.circleLabel.get_gameObject(), 0.4f, 0f);
			TweenAlpha.Begin(this._clsCircles.circleLabelBlur.get_gameObject(), 0.4f, 0f);
		}

		public void fadeInCircleButtonLabel()
		{
			TweenAlpha.Begin(this._clsCircles.circleLabel.get_gameObject(), 0.4f, 1f);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this._clsCircles.circleLabelBlur.get_gameObject(), 2f, 0f);
			tweenAlpha.from = 0f;
			tweenAlpha.to = 1f;
			tweenAlpha.style = UITweener.Style.PingPong;
		}

		private void _portframeScreenIn()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", this._vHeaderPos[1]);
			hashtable.Add("time", 0.8f);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			iTween.MoveTo(this._uiHeader, hashtable);
		}

		private void _portframeScreenOut()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", this._vHeaderPos[0]);
			hashtable.Add("time", 0.8f);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			iTween.MoveTo(this._uiHeader, hashtable);
			hashtable.Remove("position");
			hashtable.Remove("delay");
			hashtable.Remove("time");
			hashtable.Add("position", this._vFramePos[0]);
			hashtable.Add("delay", 0f);
			hashtable.Add("time", 0.8f);
			iTween.MoveTo(this._uiFrame, hashtable);
		}

		public void ReqFrame(bool isScreenIn)
		{
			if (isScreenIn)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", this._vFramePos[1]);
				hashtable.Add("delay", 0.2f);
				hashtable.Add("time", 0.8f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
				iTween.MoveTo(this._uiFrame, hashtable);
			}
			else
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2.Add("isLocal", true);
				hashtable2.Add("easeType", iTween.EaseType.easeOutExpo);
				hashtable2.Add("position", this._vFramePos[0]);
				hashtable2.Add("delay", 0f);
				hashtable2.Add("time", 0.8f);
				iTween.MoveTo(this._uiFrame, hashtable2);
			}
		}
	}
}
