using Common.Enum;
using KCV.Display;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Port.record
{
	public class record_cam : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Transform mDifficulty;

		[SerializeField]
		private UIDisplaySwipeEventRegion mtouchEventArea;

		[SerializeField]
		private Transform mMedalist;

		[SerializeField]
		private Transform mboardud1;

		[SerializeField]
		private Transform mWindowParam;

		[SerializeField]
		private Transform mShipTexture;

		[SerializeField]
		private Transform[] mbgWalls;

		[SerializeField]
		private Transform[] mLabels;

		[SerializeField]
		private Transform Medals;

		private int _now_page;

		private int _dbg_class;

		private bool _Xpressed;

		private bool _firstUpdate;

		private bool needAnimation;

		private bool _isTouch;

		private bool _isScene;

		private bool needUpdate;

		private string _ANIM_filebase = "boards_mvud";

		private ShipModel mSecurityShipModel;

		private RecordManager _clsRecord;

		private UILabel label;

		private UILabel label2;

		private UISprite sprite;

		private UITexture mTexture_SecurityShip;

		private UITexture texture;

		private UIButton _Button_L;

		private UIButton _Button_R;

		private UISprite _Button_L_B;

		private UISprite _Button_R_B;

		private Animation _AM;

		private Animation _AM_l;

		private Animation _AM_b;

		private SoundManager _SM;

		private KeyControl ItemSelectController;

		private AsyncObjects _AS;

		private GameObject _board1;

		private Color alphaZero_b = Color.get_black() * 0f;

		private int __USEITEM_DOCKKEY__ = 49;

		private UITexture _diff1;

		private UITexture _diff2;

		private bool _gotMedal;

		private int _SelectableDiff;

		private void Start()
		{
			this.needUpdate = false;
			this._Xpressed = false;
			this._firstUpdate = false;
			this.needAnimation = false;
			this._isTouch = false;
			this._gotMedal = false;
			this._AS = base.GetComponent<AsyncObjects>();
			Util.FindParentToChild<UITexture>(ref this._diff1, this.mDifficulty, "mojiW");
			Util.FindParentToChild<UITexture>(ref this._diff2, this.mDifficulty, "mojiB");
			Util.FindParentToChild<UIButton>(ref this._Button_L, base.get_transform(), "MaskCamera/btn/Button_L");
			Util.FindParentToChild<UIButton>(ref this._Button_R, base.get_transform(), "MaskCamera/btn/Button_R");
			Util.FindParentToChild<Animation>(ref this._AM_b, base.get_transform(), "MaskCamera/btn");
			Util.FindParentToChild<UISprite>(ref this._Button_L_B, this._Button_L.get_transform(), "Background");
			Util.FindParentToChild<UISprite>(ref this._Button_R_B, this._Button_R.get_transform(), "Background");
			Util.FindParentToChild(ref this._board1, this.mboardud1, "board1");
			this._Button_L_B.get_transform().set_localScale(Vector3.get_zero());
			this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			this.StartUp();
		}

		private void Update()
		{
			if (this.needUpdate)
			{
				this._set_arrow();
				if (!this._firstUpdate)
				{
					this._firstUpdate = false;
					base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
				}
				if (this.ItemSelectController != null)
				{
					this.ItemSelectController.Update();
				}
				if (this.ItemSelectController.keyState.get_Item(8).down)
				{
					this.Pressed_Button_L(null);
				}
				if (this.ItemSelectController.keyState.get_Item(12).down)
				{
					this.Pressed_Button_R(null);
				}
				if (this.ItemSelectController.IsRDown())
				{
					this._Xpressed = true;
					this.needUpdate = false;
					if (this._isScene)
					{
						this.GoToStrategy();
					}
				}
				else if (this.ItemSelectController.IsBatuDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		public bool getRecordDone()
		{
			return this._Xpressed;
		}

		private void CompleteHandler(GameObject value)
		{
			this.needAnimation = false;
		}

		private void PageAnimeDone()
		{
			this.needAnimation = false;
		}

		private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (this.needAnimation)
			{
				return;
			}
			if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY >= 0.2f && !this._isTouch)
			{
				this._isTouch = true;
				this.Pressed_Button_L(null);
				return;
			}
			if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY <= -0.2f && !this._isTouch)
			{
				this._isTouch = true;
				this.Pressed_Button_R(null);
				return;
			}
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				this._isTouch = false;
			}
		}

		private void StartUp()
		{
			this.needUpdate = true;
			Camera component;
			if (Application.get_loadedLevelName() == "Record" || Application.get_loadedLevelName() == "Record_cam")
			{
				this._isScene = true;
				component = GameObject.Find("MaskCamera").GetComponent<Camera>();
				component.set_rect(new Rect(0.353f, 0.05f, 0.625f, 0.699f));
			}
			else
			{
				this._isScene = false;
				component = GameObject.Find("OverViewCamera").GetComponent<Camera>();
				GameObject.Find("MaskCamera").GetComponent<Camera>().set_rect(new Rect(0.353f, 0.16f, 0.625f, 0.699f));
			}
			this.mtouchEventArea.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.SwipeJudgeDelegate));
			this.mtouchEventArea.SetEventCatchCamera(component);
			this._ANIM_filebase = "boards_mvud";
			this._clsRecord = new RecordManager();
			this._gotMedal = this._clsRecord.IsCleardOnce();
			if (this._gotMedal)
			{
				this.ItemSelectController = new KeyControl(0, 3, 0.4f, 0.1f);
			}
			else
			{
				this.ItemSelectController = new KeyControl(0, 2, 0.4f, 0.1f);
			}
			this.ItemSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			this._SelectableDiff = 3;
			if (this._clsRecord.GetClearCount(DifficultKind.OTU) > 0)
			{
				this._SelectableDiff = 4;
			}
			if (this._clsRecord.GetClearCount(DifficultKind.KOU) > 0)
			{
				this._SelectableDiff = 5;
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(this._clsRecord);
			}
			this._AM = base.GetComponent<Animation>();
			if (this._isScene)
			{
				this._AM_l = this.mMedalist.GetComponent<Animation>();
			}
			this._SM = SingletonMonoBehaviour<SoundManager>.Instance;
			this._Button_L_B.get_transform().set_localPosition(Vector3.get_zero());
			this._Button_R_B.get_transform().set_localPosition(Vector3.get_zero());
			UIButtonMessage component2 = this._Button_L.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "Pressed_Button_L";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component3 = this._Button_R.GetComponent<UIButtonMessage>();
			component3.target = base.get_gameObject();
			component3.functionName = "Pressed_Button_R";
			component3.trigger = UIButtonMessage.Trigger.OnClick;
			this._draw_labels();
			this._now_page = 1;
			this.mSecurityShipModel = ((SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel == null) ? new ShipModel(1) : SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel);
			DamageState damageStatus = this.mSecurityShipModel.DamageStatus;
			if (SingletonMonoBehaviour<PortObjectManager>.Instance != null)
			{
				SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					ShipUtils.PlayShipVoice(this.mSecurityShipModel, 8);
				}, true, true);
			}
			if (this._isScene)
			{
				this.set_flagship_texture(this.mSecurityShipModel);
				iTween.MoveTo(this.mWindowParam.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					-307f,
					"y",
					-199f,
					"time",
					1f,
					"delay",
					0.5f
				}));
				iTween.MoveTo(this.mMedalist.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					426f,
					"y",
					-203f,
					"time",
					1f,
					"delay",
					1.5f
				}));
			}
			else
			{
				this.SetColorBG(Color.get_white() * 0.9f + Color.get_black());
				this.SetColorText(Color.get_white() * 0.5f + Color.get_black());
				this.SetColorLine(Color.get_white() * 0.5f + Color.get_black());
				this.SetColorIcon(Color.get_white() + Color.get_black());
				this.SetTextShadow(false);
				this.SetCursorColor(Color.get_red() * 0.75f + Color.get_black());
			}
		}

		private void _set_arrow()
		{
			if (this.needAnimation)
			{
				return;
			}
			if (this._now_page == 1)
			{
				this._AM_b.Play("btn_mvud_off_on");
				iTween.MoveTo(this._Button_L_B.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0f,
					"time",
					0.5f
				}));
				this._Button_L_B.get_transform().set_localScale(Vector3.get_zero());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			}
			else if (this._now_page == 2 || (this._now_page == 3 && this._gotMedal))
			{
				this._AM_b.Play("btn_mvud_on_on");
				this._Button_L_B.get_transform().set_localScale(Vector3.get_one());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			}
			else
			{
				this._AM_b.Play("btn_mvud_on_off");
				iTween.MoveTo(this._Button_R_B.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0f,
					"time",
					0.5f
				}));
				this._Button_L_B.get_transform().set_localScale(Vector3.get_one());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_zero());
			}
		}

		private void _set_view_board(int page)
		{
			if (!this.needAnimation)
			{
				this.needAnimation = true;
				if (page == 1)
				{
					if (this._now_page != page)
					{
						SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
						iTween.MoveTo(this._board1, iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							0f,
							"time",
							0.5f,
							"easeType",
							"easeInOutQuad",
							"oncomplete",
							"OnComplete",
							"oncompletetarget",
							base.get_gameObject()
						}));
						this._AM.Play("boards_wait");
					}
					this._now_page = 1;
				}
				else if (page == 2)
				{
					if (this._now_page != page)
					{
						SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
						iTween.MoveTo(this._board1, iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							380f,
							"time",
							0.5f,
							"easeType",
							"easeInOutQuad",
							"oncomplete",
							"OnComplete",
							"oncompletetarget",
							base.get_gameObject()
						}));
						this._AM.Play("boards_wait");
					}
					this._now_page = 2;
				}
				else if (page == 3)
				{
					if (this._now_page != page)
					{
						SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
						iTween.MoveTo(this._board1, iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							760f,
							"time",
							0.5f,
							"easeType",
							"easeInOutQuad",
							"oncomplete",
							"OnComplete",
							"oncompletetarget",
							base.get_gameObject()
						}));
						this._AM.Play("boards_wait");
					}
					this._now_page = 3;
				}
				else if (page == 4 && this._gotMedal)
				{
					if (this._now_page != page)
					{
						SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
						iTween.MoveTo(this._board1, iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							1140f,
							"time",
							0.5f,
							"easeType",
							"easeInOutQuad",
							"oncomplete",
							"OnComplete",
							"oncompletetarget",
							base.get_gameObject()
						}));
						this._AM.Play("boards_wait");
					}
					this._now_page = 4;
				}
			}
		}

		public DifficultKind int2DiffKind(int i)
		{
			switch (i)
			{
			case 1:
				return DifficultKind.TEI;
			case 2:
				return DifficultKind.HEI;
			case 3:
				return DifficultKind.OTU;
			case 4:
				return DifficultKind.KOU;
			case 5:
				return DifficultKind.SHI;
			default:
				return DifficultKind.TEI;
			}
		}

		public void _draw_labels()
		{
			Dictionary<int, string> mstBgm = Mst_DataManager.Instance.GetMstBgm();
			string text;
			mstBgm.TryGetValue(this._clsRecord.UserInfo.GetPortBGMId(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID), ref text);
			if (text == null)
			{
				text = "母港";
			}
			for (int i = 1; i <= 3; i++)
			{
				this.texture = this.mbgWalls[i - 1].GetComponent<UITexture>();
				this.texture.mainTexture = (Resources.Load("Textures/Record/logo") as Texture);
			}
			this.label = this.mWindowParam.FindChild("Labels/adm_name").GetComponent<UILabel>();
			this.label.text = this._clsRecord.Name;
			this.label.supportEncoding = false;
			this.label = this.mWindowParam.FindChild("Labels/adm_level").GetComponent<UILabel>();
			this.label.textInt = this._clsRecord.Level;
			this.label = this.mWindowParam.FindChild("Labels/adm_status").GetComponent<UILabel>();
			this.label.text = Util.RankNameJ(this._clsRecord.Rank);
			this.label = this.mWindowParam.FindChild("Labels/adm_exp").GetComponent<UILabel>();
			this.label.fontSize = 24;
			this.label.width = 300;
			if (this._clsRecord.NextExperience == 0u)
			{
				this.label.text = this._clsRecord.Experience.ToString();
			}
			else
			{
				this.label.text = this._clsRecord.Experience + "/" + this._clsRecord.NextExperience;
			}
			this.mLabels[0].FindChild("Label_1-2").GetComponent<UILabel>().text = string.Concat(new object[]
			{
				this._clsRecord.BattleCount,
				"  \n\n",
				this._clsRecord.SortieWin,
				"  \n\n",
				this._clsRecord.InterceptSuccess,
				"  \n\n",
				string.Format("{0:f1}", this._clsRecord.SortieRate),
				"%\n\n\n\n",
				this._clsRecord.DeckPractice,
				"  \n\n",
				(this._clsRecord.PracticeWin + this._clsRecord.PracticeLose).ToString(),
				"  "
			});
			int count = new UseitemUtil().GetCount(this.__USEITEM_DOCKKEY__);
			this.mLabels[1].FindChild("Label_2-2").GetComponent<UILabel>().text = string.Concat(new object[]
			{
				this._clsRecord.DeckCount,
				"\n",
				this._clsRecord.ShipCount,
				"\n",
				this._clsRecord.SlotitemCount,
				"\n",
				this._clsRecord.NDockCount,
				"\n",
				this._clsRecord.KDockCount,
				"\n",
				count,
				"\n",
				this._clsRecord.FurnitureCount
			});
			this.mLabels[2].FindChild("Label_3-2").GetComponent<UILabel>().text = string.Concat(new object[]
			{
				this._clsRecord.DeckCountMax,
				"\n",
				this._clsRecord.ShipCountMax,
				"\n",
				this._clsRecord.SlotitemCountMax,
				"\n",
				this._clsRecord.MaterialMax,
				"\n\n\n"
			});
			this.mLabels[2].FindChild("Label_3-3").GetComponent<UILabel>().text = "♪「" + text + "」";
			string text2 = "Textures/Record/difficulty/diff_" + (int)this._clsRecord.UserInfo.Difficulty;
			this._diff1.mainTexture = (Resources.Load(text2) as Texture);
			this._diff2.mainTexture = (Resources.Load(text2) as Texture);
			for (int j = 1; j <= 5; j++)
			{
				this.Medals.FindChild(string.Concat(new object[]
				{
					"medal_",
					j,
					"/Label_4-",
					j
				})).GetComponent<UILabel>().text = ((this._clsRecord.GetClearCount(this.int2DiffKind(j)) < 2) ? string.Empty : ("×" + this._clsRecord.GetClearCount(this.int2DiffKind(j))));
				Transform transform = this.Medals.FindChild("medal_" + j);
				Transform transform2 = transform.FindChild("medal");
				Transform transform3 = transform.FindChild("bg");
				if (this._clsRecord.GetClearCount(this.int2DiffKind(j)) > 0)
				{
					transform2.set_localScale(Vector3.get_one());
					transform3.set_localScale(Vector3.get_zero());
					transform.FindChild("Light").GetComponent<ParticleSystem>().Play();
				}
				else
				{
					transform2.set_localScale(Vector3.get_zero());
					transform3.set_localScale(Vector3.get_one());
					transform.FindChild("Light").GetComponent<ParticleSystem>().Stop();
				}
			}
			switch (this._SelectableDiff)
			{
			case 3:
				this.Medals.FindChild("medal_5").localPositionX(999f);
				this.Medals.FindChild("medal_4").localPositionX(999f);
				this.Medals.FindChild("medal_3").localPositionX(-166f);
				this.Medals.FindChild("medal_2").localPositionX(0f);
				this.Medals.FindChild("medal_1").localPositionX(166f);
				break;
			case 4:
				this.Medals.FindChild("medal_5").localPositionX(999f);
				this.Medals.FindChild("medal_4").localPositionX(-200f);
				this.Medals.FindChild("medal_3").localPositionX(-66.6f);
				this.Medals.FindChild("medal_2").localPositionX(66.6f);
				this.Medals.FindChild("medal_1").localPositionX(200f);
				break;
			case 5:
				this.Medals.FindChild("medal_5").localPositionX(-223f);
				this.Medals.FindChild("medal_4").localPositionX(-96f);
				this.Medals.FindChild("medal_3").localPositionX(16f);
				this.Medals.FindChild("medal_2").localPositionX(128f);
				this.Medals.FindChild("medal_1").localPositionX(240f);
				this.Medals.FindChild("medal_4/medal").localPositionX(-9f);
				this.Medals.FindChild("medal_4/Label_4-4").localPositionX(-9f);
				this.Medals.FindChild("medal_2/medal").localPositionX(3f);
				this.Medals.FindChild("medal_2/Label_4-2").localPositionX(3f);
				break;
			}
		}

		private void GoToStrategy()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}

		public void Pressed_Button_L(GameObject obj)
		{
			if (this._now_page == 2)
			{
				this._set_view_board(1);
			}
			else if (this._now_page == 3)
			{
				this._set_view_board(2);
			}
			else if (this._now_page == 4)
			{
				this._set_view_board(3);
			}
		}

		public void Pressed_Button_R(GameObject obj)
		{
			if (this._now_page == 1)
			{
				this._set_view_board(2);
			}
			else if (this._now_page == 2)
			{
				this._set_view_board(3);
			}
			else if (this._now_page == 3 && this._gotMedal)
			{
				this._set_view_board(4);
			}
		}

		public void set_flagship_texture(ShipModel shipModel)
		{
			this.mTexture_SecurityShip = this.mShipTexture.GetComponent<UITexture>();
			int texNum = (!shipModel.IsDamaged()) ? 9 : 10;
			this.mTexture_SecurityShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), texNum);
			this.mTexture_SecurityShip.MakePixelPerfect();
			this.mTexture_SecurityShip.get_transform().set_localPosition(Util.Poi2Vec(shipModel.Offsets.GetShipDisplayCenter(shipModel.IsDamaged())));
		}

		public void SetColorText(Color col)
		{
			GameObject gameObject = this.mboardud1.get_gameObject();
			UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
			UILabel[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UILabel uILabel = array[i];
				uILabel.color = col;
			}
		}

		public void SetTextShadow(bool shadow)
		{
			GameObject gameObject = this.mboardud1.get_gameObject();
			UILabel[] componentsInChildren = gameObject.GetComponentsInChildren<UILabel>();
			UILabel[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UILabel uILabel = array[i];
				if (shadow)
				{
					uILabel.effectStyle = UILabel.Effect.Shadow;
				}
				else
				{
					uILabel.effectStyle = UILabel.Effect.None;
				}
			}
		}

		public void SetColorBG(Color col)
		{
			GameObject gameObject = this.mboardud1.get_gameObject();
			UITexture[] componentsInChildren = gameObject.GetComponentsInChildren<UITexture>();
			UITexture[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UITexture uITexture = array[i];
				if (!uITexture.get_name().Contains("line"))
				{
					uITexture.color = col;
				}
			}
		}

		public void SetColorLine(Color col)
		{
			GameObject gameObject = this.mboardud1.get_gameObject();
			UITexture[] componentsInChildren = gameObject.GetComponentsInChildren<UITexture>();
			UITexture[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UITexture uITexture = array[i];
				if (uITexture.get_name().Contains("line"))
				{
					uITexture.color = col;
				}
			}
		}

		public void SetColorIcon(Color col)
		{
			GameObject gameObject = this.mboardud1.get_gameObject();
			UISprite[] componentsInChildren = gameObject.GetComponentsInChildren<UISprite>();
			UISprite[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UISprite uISprite = array[i];
				uISprite.color = col;
			}
		}

		public void SetCursorColor(Color col)
		{
			this._Button_L.defaultColor = col;
			this._Button_R.defaultColor = col;
			this._Button_L.hover = col;
			this._Button_R.hover = col;
			this._Button_L.pressed = col;
			this._Button_R.pressed = col;
		}

		private void OnDestroy()
		{
			if (this.mTexture_SecurityShip != null && this.mTexture_SecurityShip.mainTexture != null)
			{
				this.mTexture_SecurityShip = null;
			}
			if (this.mDifficulty != null)
			{
				this.mDifficulty = null;
			}
			if (this.mtouchEventArea != null)
			{
			}
			if (this.mMedalist != null)
			{
				this.mMedalist = null;
			}
			if (this.mboardud1 != null)
			{
				this.mboardud1 = null;
			}
			if (this.mWindowParam != null)
			{
				this.mWindowParam = null;
			}
			if (this.mShipTexture != null)
			{
				this.mShipTexture = null;
			}
			if (this.mbgWalls != null)
			{
				Mem.DelAry<Transform>(ref this.mbgWalls);
			}
			if (this.mLabels != null)
			{
				Mem.DelAry<Transform>(ref this.mLabels);
			}
			if (this.Medals != null)
			{
				this.Medals = null;
			}
			this._now_page = 0;
			this._dbg_class = 0;
			this._Xpressed = false;
			this._firstUpdate = false;
			this.needAnimation = false;
			this._isTouch = false;
			this._isScene = false;
			this.needUpdate = false;
			if (this._ANIM_filebase != null)
			{
				this._ANIM_filebase = null;
			}
			if (this.mSecurityShipModel != null)
			{
				this.mSecurityShipModel = null;
			}
			if (this._clsRecord != null)
			{
				this._clsRecord = null;
			}
			if (this.label != null)
			{
				this.label = null;
			}
			if (this.label2 != null)
			{
				this.label2 = null;
			}
			if (this.sprite != null)
			{
				this.sprite = null;
			}
			if (this.mTexture_SecurityShip != null)
			{
				this.mTexture_SecurityShip = null;
			}
			if (this.texture != null)
			{
				this.texture = null;
			}
			if (this._Button_L != null)
			{
				this._Button_L = null;
			}
			if (this._Button_R != null)
			{
				this._Button_R = null;
			}
			if (this._Button_L_B != null)
			{
				this._Button_L_B = null;
			}
			if (this._Button_R_B != null)
			{
				this._Button_R_B = null;
			}
			if (this._AM != null)
			{
				this._AM = null;
			}
			if (this._AM_l != null)
			{
				this._AM_l = null;
			}
			if (this._AM_b != null)
			{
				this._AM_b = null;
			}
			if (this._SM != null)
			{
				this._SM = null;
			}
			if (this.ItemSelectController != null)
			{
				this.ItemSelectController = null;
			}
			if (this._AS != null)
			{
				this._AS = null;
			}
			if (this._board1 != null)
			{
				this._board1 = null;
			}
			this.alphaZero_b = Color.get_black() * 0f;
			this.__USEITEM_DOCKKEY__ = 0;
			if (this._diff1 != null)
			{
				this._diff1 = null;
			}
			if (this._diff2 != null)
			{
				this._diff2 = null;
			}
			this._gotMedal = false;
			this._SelectableDiff = 0;
		}
	}
}
