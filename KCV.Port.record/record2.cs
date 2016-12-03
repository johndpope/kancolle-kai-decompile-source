using Common.Enum;
using KCV.Display;
using KCV.Scene.Strategy;
using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Port.record
{
	public class record2 : MonoBehaviour
	{
		private bool _DEBUG_MODE_NOW_;

		private string _ANIM_filebase = "boards_mvud";

		private UIStageCover[] uiStageCovers;

		private UILabel label;

		private UILabel label2;

		private UISprite sprite;

		private UITexture texture;

		private UIButton _Button_L;

		private UIButton _Button_R;

		private UITexture _Button_L_B;

		private UITexture _Button_R_B;

		private UIButton _DBG_Button_L;

		private UIButton _DBG_Button_R;

		private Animation _AM;

		private Animation _AM_l;

		private Animation _AM_b;

		private SoundManager _SM;

		private KeyControl ItemSelectController;

		private int _now_page;

		private AsyncObjects _AS;

		private bool _StartUp;

		private int _flag_ship;

		private bool _damaged;

		private bool _isRecordScene;

		private GameObject _board1;

		private Color alphaZero_b = new Color(0f, 0f, 0f, 0f);

		private Color _Color_dock = new Color(0.51f, 0.953f, 0.357f);

		private int _dbg_class;

		private bool _Xpressed;

		private bool _firstUpdate;

		private bool _isAnime;

		private bool _isTouch;

		private bool _onYet;

		private bool _arrow_flag;

		private int _now_area;

		private CommonShipBanner[] csb = new CommonShipBanner[6];

		private StrategyMapManager strategyLogicManager;

		public bool getRecordDone()
		{
			return this._Xpressed;
		}

		private void CompleteHandler(GameObject value)
		{
			this._isAnime = false;
			this._isTouch = false;
		}

		private void PageAnimeDone()
		{
			this._isAnime = false;
		}

		private void Start()
		{
			this._now_area = -1;
			this._StartUp = false;
			this._Xpressed = false;
			this._firstUpdate = false;
			this._isAnime = false;
			this._isTouch = false;
			this._arrow_flag = false;
			this.StartUp();
		}

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this.label);
			Mem.Del<UILabel>(ref this.label2);
			Mem.Del(ref this.sprite);
			Mem.Del<UITexture>(ref this.texture);
			Mem.Del<UIButton>(ref this._Button_L);
			Mem.Del<UIButton>(ref this._Button_R);
			Mem.Del<UITexture>(ref this._Button_L_B);
			Mem.Del<UITexture>(ref this._Button_R_B);
			Mem.Del<UIButton>(ref this._DBG_Button_L);
			Mem.Del<UIButton>(ref this._DBG_Button_R);
			Mem.Del<Animation>(ref this._AM);
			Mem.Del<Animation>(ref this._AM_l);
			Mem.Del<Animation>(ref this._AM_b);
			this.uiStageCovers = null;
		}

		private void OnEnable()
		{
			this.strategyLogicManager = StrategyTopTaskManager.GetLogicManager();
			this.map_status();
			if (!this._onYet)
			{
				return;
			}
			this._Xpressed = false;
			this._firstUpdate = false;
			this._isAnime = false;
			this._isTouch = false;
			this._arrow_flag = false;
			this._draw_labels();
			this._board1 = GameObject.Find("board1");
			Object.Destroy(this._board1.GetComponent<iTween>());
			this._board1.get_transform().set_localPosition(Vector3.get_zero());
			this._now_page = 1;
			this._Button_L_B.get_transform().set_localScale(Vector3.get_zero());
			this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
		}

		private void OnDisable()
		{
			if (this.uiStageCovers != null)
			{
				UIStageCover[] array = this.uiStageCovers;
				for (int i = 0; i < array.Length; i++)
				{
					UIStageCover uIStageCover = array[i];
					uIStageCover.SelfRelease();
				}
			}
		}

		private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (this._isAnime)
			{
				this._isTouch = false;
				return;
			}
			if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY >= 0.05f && !this._isTouch)
			{
				this._isTouch = true;
				this.Pressed_Button_L(null);
			}
			else if (actionType == UIDisplaySwipeEventRegion.ActionType.Moving && movedPercentageY <= -0.05f && !this._isTouch)
			{
				this._isTouch = true;
				this.Pressed_Button_R(null);
			}
			else if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				this._isTouch = false;
			}
		}

		private void StartUp()
		{
			this._StartUp = true;
			if (this._DEBUG_MODE_NOW_)
			{
				this._dbg_class = 1;
				this._DBG_Button_L = base.get_transform().FindChild("Debug_ship/DBG_Button_L").GetComponent<UIButton>();
				this._DBG_Button_R = base.get_transform().FindChild("Debug_ship/DBG_Button_R").GetComponent<UIButton>();
				UIButtonMessage component = this._DBG_Button_L.GetComponent<UIButtonMessage>();
				component.target = base.get_gameObject();
				component.functionName = "Pressed_DBG_Button_L";
				component.trigger = UIButtonMessage.Trigger.OnClick;
				UIButtonMessage component2 = this._DBG_Button_R.GetComponent<UIButtonMessage>();
				component2.target = base.get_gameObject();
				component2.functionName = "Pressed_DBG_Button_R";
				component2.trigger = UIButtonMessage.Trigger.OnClick;
			}
			UIDisplaySwipeEventRegion component3 = GameObject.Find("TouchEventArea").GetComponent<UIDisplaySwipeEventRegion>();
			Camera component4;
			if (Application.get_loadedLevelName() == "Record")
			{
				this._isRecordScene = true;
				component4 = GameObject.Find("Camera").GetComponent<Camera>();
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), 8);
				}, true, true);
			}
			else
			{
				this._isRecordScene = false;
				component4 = GameObject.Find("OverViewCamera").GetComponent<Camera>();
			}
			component3.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.SwipeJudgeDelegate));
			component3.SetEventCatchCamera(component4);
			this._ANIM_filebase = "boards_mvud";
			this._AM = GameObject.Find("RecordScene").GetComponent<Animation>();
			if (this._isRecordScene)
			{
				this._AM_l = GameObject.Find("medalist").GetComponent<Animation>();
			}
			this._AM_b = GameObject.Find("btn").GetComponent<Animation>();
			this._SM = SingletonMonoBehaviour<SoundManager>.Instance;
			this._Button_L = base.get_transform().FindChild("btn/Button_L").GetComponent<UIButton>();
			this._Button_R = base.get_transform().FindChild("btn/Button_R").GetComponent<UIButton>();
			this._Button_L_B = base.get_transform().FindChild("btn/Button_L/Background").GetComponent<UITexture>();
			this._Button_R_B = base.get_transform().FindChild("btn/Button_R/Background").GetComponent<UITexture>();
			this._Button_L_B.get_transform().set_localScale(Vector3.get_zero());
			this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			UIButtonMessage component5 = this._Button_L.GetComponent<UIButtonMessage>();
			component5.target = base.get_gameObject();
			component5.functionName = "Pressed_Button_L";
			component5.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component6 = this._Button_R.GetComponent<UIButtonMessage>();
			component6.target = base.get_gameObject();
			component6.functionName = "Pressed_Button_R";
			component6.trigger = UIButtonMessage.Trigger.OnClick;
			this._board1 = GameObject.Find("board1");
			this.ItemSelectController = new KeyControl(0, 2, 0.4f, 0.1f);
			this.ItemSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			this._draw_labels();
			this._now_page = 1;
			ShipModel shipModel = (SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel == null) ? new ShipModel(1) : SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			this._flag_ship = shipModel.GetGraphicsMstId();
			DamageState damageStatus = shipModel.DamageStatus;
			if (damageStatus == DamageState.Normal || damageStatus == DamageState.Shouha)
			{
				this._damaged = false;
			}
			else
			{
				this._damaged = true;
			}
		}

		private void _set_arrow()
		{
			if (this._isAnime)
			{
				return;
			}
			if (!this._arrow_flag)
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
					-10f,
					"time",
					0.5f
				}));
				this._Button_L_B.get_transform().set_localScale(Vector3.get_zero());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			}
			if (this._now_page == 2)
			{
				this._AM_b.Play("btn_mvud_on_on");
				this._Button_L_B.get_transform().set_localScale(Vector3.get_one());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_one());
			}
			if (this._now_page == 3)
			{
				this._AM_b.Play("btn_mvud_on_off");
				iTween.MoveTo(this._Button_R_B.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					10f,
					"time",
					0.5f
				}));
				this._Button_L_B.get_transform().set_localScale(Vector3.get_one());
				this._Button_R_B.get_transform().set_localScale(Vector3.get_zero());
			}
		}

		private void _set_view_board(int page)
		{
			if (this._isAnime)
			{
				return;
			}
			this._onYet = true;
			this._isAnime = true;
			if (page == 1)
			{
				if (this._now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					TweenPosition tweenPosition = TweenPosition.Begin(this._board1, 0.5f, new Vector3(this._board1.get_transform().get_localPosition().x, 0f, this._board1.get_transform().get_localPosition().z));
					tweenPosition.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition.SetOnFinished(new EventDelegate.Callback(this.PageAnimeDone));
					this._AM.Play("boards_wait");
					this._arrow_flag = true;
				}
				this._now_page = 1;
			}
			else if (page == 2)
			{
				if (this._now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					TweenPosition tweenPosition2 = TweenPosition.Begin(this._board1, 0.5f, new Vector3(this._board1.get_transform().get_localPosition().x, 544f, this._board1.get_transform().get_localPosition().z));
					tweenPosition2.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition2.SetOnFinished(new EventDelegate.Callback(this.PageAnimeDone));
					this._AM.Play("boards_wait");
					this._arrow_flag = true;
				}
				this._now_page = 2;
			}
			else
			{
				if (this._now_page != page)
				{
					SoundUtils.PlaySE(SEFIleInfos.MainMenuOnMouse);
					TweenPosition tweenPosition3 = TweenPosition.Begin(this._board1, 0.5f, new Vector3(this._board1.get_transform().get_localPosition().x, 1088f, this._board1.get_transform().get_localPosition().z));
					tweenPosition3.animationCurve = UtilCurves.TweenEaseInOutQuad;
					tweenPosition3.SetOnFinished(new EventDelegate.Callback(this.PageAnimeDone));
					this._AM.Play("boards_wait");
					this._arrow_flag = true;
				}
				this._now_page = 3;
			}
		}

		public void map_status()
		{
			this.uiStageCovers = GameObject.Find("board2nd/board1/page1/UIStageCovers").GetComponentsInChildren<UIStageCover>();
			MapAreaModel areaModel = StrategyTopTaskManager.Instance.TileManager.FocusTile.GetAreaModel();
			MapModel[] maps = StrategyTopTaskManager.GetLogicManager().SelectArea(areaModel.Id).Maps;
			UILabel component = GameObject.Find("board2nd/board1/page1/Labels/Label_0-1").GetComponent<UILabel>();
			component.text = Util.getDifficultyString(this.strategyLogicManager.UserInfo.Difficulty);
			UILabel component2 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-2").GetComponent<UILabel>();
			component2.text = areaModel.Name;
			UILabel component3 = GameObject.Find("board2nd/board1/page2/Labels/Label_2-4").GetComponent<UILabel>();
			component3.supportEncoding = false;
			if (areaModel.Id < 15)
			{
				string name = areaModel.GetEscortDeck().Name;
				if (name.Replace(" ", string.Empty).Replace("\u3000", string.Empty).get_Length() != 0)
				{
					component3.text = name;
				}
				else
				{
					component3.text = areaModel.Name.Replace("海域", string.Empty) + "航路護衛隊";
				}
			}
			else
			{
				component3.text = "---";
			}
			GameObject.Find("board2nd/board1/page2/Decks").get_transform().set_localPosition(new Vector3(-17.536f * (float)this.strategyLogicManager.UserInfo.DeckCount + 94.286f, 0f));
			for (int i = 0; i < 8; i++)
			{
				UISprite component4 = GameObject.Find("board2nd/board1/page2/Decks/Deck" + (i + 1).ToString()).GetComponent<UISprite>();
				component4.color = Color.get_black();
				if (i < this.strategyLogicManager.UserInfo.DeckCount)
				{
					component4.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					component4.get_transform().set_localScale(Vector3.get_zero());
				}
			}
			for (int j = 0; j < areaModel.GetDecks().Length; j++)
			{
				UISprite component4 = GameObject.Find("board2nd/board1/page2/Decks/Deck" + areaModel.GetDecks()[j].Id).GetComponent<UISprite>();
				if (areaModel.GetDecks()[j].GetShipCount() != 0)
				{
					if (areaModel.GetDecks()[j].IsActionEnd())
					{
						component4.color = this._Color_dock * 0.75f;
					}
					else if (areaModel.GetDecks()[j].MissionState != MissionStates.NONE)
					{
						component4.color = Color.get_blue();
					}
					else
					{
						component4.color = this._Color_dock;
					}
				}
			}
			if (maps.Length < 5)
			{
				UILabel component5 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-2").GetComponent<UILabel>();
				component5.get_transform().set_localPosition(new Vector3(160f, 160f, 0f));
				component5.fontSize = 36;
				UILabel component6 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-1").GetComponent<UILabel>();
				component6.get_transform().set_localPosition(new Vector3(160f, 105f, 0f));
				component6.fontSize = 32;
				UILabel component7 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-0").GetComponent<UILabel>();
				component7.get_transform().set_localPosition(new Vector3(329f, 105f, 0f));
				component7.fontSize = 20;
				UILabel component8 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-1").GetComponent<UILabel>();
				component8.get_transform().set_localPosition(new Vector3(413f, 105f, 0f));
				component8.fontSize = 20;
				UITexture component9 = GameObject.Find("board2nd/board1/page1/lines/line_1").GetComponent<UITexture>();
				if (component9 != null)
				{
					component9.get_transform().set_localPosition(new Vector3(160f, 103f, 0f));
					component9.width = 556;
					component9.height = 2;
				}
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 2; l++)
					{
						int num = k * 2 + l + 1;
						GameObject gameObject = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + num.ToString());
						if (gameObject == null)
						{
							break;
						}
						gameObject.get_transform().set_localScale(Vector3.get_one() * 0.6f);
						if (num < 5)
						{
							gameObject.get_transform().set_localPosition(new Vector3(18f + 293f * (float)l, -17f - 158f * (float)k, 0f));
						}
						else
						{
							gameObject.get_transform().set_localPosition(new Vector3(18f + 293f * (float)l, 320f, 0f));
						}
					}
				}
				if (maps.Length == 3)
				{
					GameObject gameObject2 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover4");
					gameObject2.get_transform().set_localScale(Vector3.get_zero());
					GameObject gameObject3 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover3");
					gameObject3.get_transform().set_localPosition(new Vector3(160f, -175f));
				}
				else if (maps.Length == 4)
				{
					GameObject gameObject4 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover4");
					gameObject4.get_transform().set_localScale(Vector3.get_one() * 0.6f);
					GameObject gameObject5 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover3");
					gameObject5.get_transform().set_localPosition(new Vector3(18f, -175f));
				}
			}
			else
			{
				component2.get_transform().set_localPosition(new Vector3(160f, 171f, 0f));
				component2.fontSize = 28;
				UILabel component10 = GameObject.Find("board2nd/board1/page1/Labels/Label_1-1").GetComponent<UILabel>();
				component10.get_transform().set_localPosition(new Vector3(160f, 139f, 0f));
				component10.fontSize = 24;
				UILabel component11 = GameObject.Find("board2nd/board1/page1/Labels/Label_0-0").GetComponent<UILabel>();
				component11.get_transform().set_localPosition(new Vector3(329f, 149f, 0f));
				component11.fontSize = 20;
				component.get_transform().set_localPosition(new Vector3(413f, 149f, 0f));
				component.fontSize = 20;
				UITexture component12 = GameObject.Find("board2nd/board1/page1/lines/line_1").GetComponent<UITexture>();
				component12.get_transform().set_localPosition(new Vector3(160f, 143f, 0f));
				component12.width = 556;
				component12.height = 2;
				for (int m = 0; m < 3; m++)
				{
					for (int n = 0; n < 2; n++)
					{
						GameObject gameObject6 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + (m * 2 + n + 1).ToString());
						gameObject6.get_transform().set_localScale(Vector3.get_one() * 0.5f);
						gameObject6.get_transform().set_localPosition(new Vector3(12f + 299f * (float)n, 40f - 122f * (float)m, 0f));
					}
				}
				if (maps.Length == 5)
				{
					GameObject gameObject7 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover5");
					gameObject7.get_transform().set_localPosition(new Vector3(162f, -209f, 0f));
					GameObject gameObject8 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover6");
					gameObject8.get_transform().set_localPosition(new Vector3(162f, 320f, 0f));
				}
			}
			for (int num2 = 0; num2 < maps.Length; num2++)
			{
				UIStageCover component13 = GameObject.Find("board2nd/board1/page1/UIStageCovers/UIStageCover" + (num2 + 1)).GetComponent<UIStageCover>();
				MapModel mapModel = maps[num2];
				component13.Initialize(mapModel);
			}
		}

		public void _draw_labels()
		{
			MapAreaModel areaModel = StrategyTopTaskManager.Instance.TileManager.FocusTile.GetAreaModel();
			RecordManager recordManager = new RecordManager();
			GameObject.Find("VERSION").GetComponent<UILabel>().text = "Version 1.02";
			string text = Util.RankNameJ(recordManager.Rank);
			if (this._isRecordScene)
			{
				this.label = GameObject.Find("adm_name").GetComponent<UILabel>();
				this.label.text = recordManager.Name;
				this.label = GameObject.Find("adm_level").GetComponent<UILabel>();
				this.label.textInt = recordManager.Level;
				this.label = GameObject.Find("adm_status").GetComponent<UILabel>();
				this.label.text = text;
				this.label = GameObject.Find("adm_exp").GetComponent<UILabel>();
				this.label.text = recordManager.Experience + "/" + recordManager.NextExperience;
			}
			string text2 = string.Concat(new object[]
			{
				recordManager.DeckCount,
				"\n",
				recordManager.ShipCount,
				" / ",
				recordManager.ShipCountMax,
				"\n",
				recordManager.SlotitemCount,
				" / ",
				recordManager.SlotitemCountMax,
				"\n",
				recordManager.MaterialMax,
				"\n",
				recordManager.NDockCount,
				"\n"
			});
			string text3;
			if (areaModel.NDockMax != 0)
			{
				text3 = text2;
				text2 = string.Concat(new object[]
				{
					text3,
					areaModel.NDockCount,
					" / ",
					areaModel.NDockMax,
					"\n"
				});
			}
			else
			{
				text2 += "－ / －\n";
			}
			text3 = text2;
			text2 = string.Concat(new object[]
			{
				text3,
				recordManager.KDockCount,
				" / ",
				4
			});
			GameObject.Find("Label_3-2").GetComponent<UILabel>().text = text2;
			for (int i = 0; i < areaModel.GetEscortDeck().Count; i++)
			{
				this.csb[i] = GameObject.Find("board2nd/board1/page2/banners/banner" + (i + 1).ToString() + "/CommonShipBanner2").GetComponent<CommonShipBanner>();
				this.csb[i].SetShipData(areaModel.GetEscortDeck().GetShips()[i]);
				this.csb[i].get_transform().set_localScale(Vector3.get_one() * 0.703125f);
			}
			for (int j = areaModel.GetEscortDeck().Count; j < 6; j++)
			{
				this.csb[j] = GameObject.Find("board2nd/board1/page2/banners/banner" + (j + 1).ToString() + "/CommonShipBanner2").GetComponent<CommonShipBanner>();
				this.csb[j].get_transform().set_localScale(Vector3.get_zero());
				UITexture component = GameObject.Find("board2nd/board1/page2/banners/banner" + (j + 1).ToString() + "/BannerBG").GetComponent<UITexture>();
				component.color = Color.get_gray() / 2f;
			}
			UILabel component2 = GameObject.Find("board2nd/board1/page2/Labels/Label_2-2").GetComponent<UILabel>();
			int countNoMove = areaModel.GetTankerCount().GetCountNoMove();
			int maxCount = areaModel.GetTankerCount().GetMaxCount();
			if (areaModel.Id < 15)
			{
				component2.text = countNoMove.ToString() + "/" + maxCount.ToString();
			}
			else
			{
				component2.text = "---";
			}
			if (areaModel.Id < 15)
			{
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial1/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove).get_Item(enumMaterialCategory.Fuel));
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial3/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove).get_Item(enumMaterialCategory.Steel));
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial2/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove).get_Item(enumMaterialCategory.Bull));
				component2 = GameObject.Find("board2nd/board1/page2/material/GetMaterial4/num").GetComponent<UILabel>();
				component2.text = "× " + string.Format("{0, 3}", areaModel.GetResources(countNoMove).get_Item(enumMaterialCategory.Bauxite));
			}
			else
			{
				GameObject.Find("board2nd/board1/page2/material/GetMaterial1/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial3/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial2/num").GetComponent<UILabel>().text = "× ---";
				GameObject.Find("board2nd/board1/page2/material/GetMaterial4/num").GetComponent<UILabel>().text = "× ---";
			}
		}

		private void Update()
		{
			if (!this._StartUp)
			{
				return;
			}
			this._set_arrow();
			if (!this._firstUpdate)
			{
				this._firstUpdate = false;
				base.get_gameObject().get_transform().set_localScale(new Vector3(1f, 1f, 1f));
			}
			this.ItemSelectController.Update();
			if (this.ItemSelectController.keyState.get_Item(8).down)
			{
				this.Pressed_Button_L(null);
			}
			if (this.ItemSelectController.keyState.get_Item(12).down)
			{
				this.Pressed_Button_R(null);
			}
			if (this.ItemSelectController.keyState.get_Item(0).down)
			{
				this._Xpressed = true;
				if (this._isRecordScene)
				{
					this._StartUp = false;
					this.back_to_port();
				}
			}
			if (this._DEBUG_MODE_NOW_)
			{
				if (this.ItemSelectController.keyState.get_Item(4).down)
				{
					this.Pressed_DBG_Button_L(null);
				}
				if (this.ItemSelectController.keyState.get_Item(5).down)
				{
					this.Pressed_DBG_Button_R(null);
				}
				if (this.ItemSelectController.keyState.get_Item(8).down)
				{
					this.Pressed_DBG_Button_UP(null);
				}
				if (this.ItemSelectController.keyState.get_Item(12).down)
				{
					this.Pressed_DBG_Button_DOWN(null);
				}
			}
		}

		private void pop_record()
		{
			for (int i = 1; i <= 3; i++)
			{
				this.texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
				this.texture.mainTexture = null;
				Resources.UnloadAsset(this.texture.mainTexture);
			}
			this.texture = GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>();
			this.texture.mainTexture = null;
			Resources.UnloadAsset(this.texture.mainTexture);
		}

		private void back_to_port()
		{
			this._StartUp = false;
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				this.pop_record();
				AsyncLoadScene.LoadLevelAsyncScene(this, Generics.Scene.PortTop.ToString(), null);
			});
		}

		private void Medalist_Anime()
		{
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
		}

		public void Pressed_DBG_Button_L(GameObject obj)
		{
			if (this._flag_ship <= 1)
			{
				return;
			}
			this._flag_ship--;
			this.set_flagship_texture(this._flag_ship);
			if (GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture == null)
			{
				GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffddaa]" + this._flag_ship;
			}
			else
			{
				GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffffff]" + this._flag_ship;
			}
		}

		public void Pressed_DBG_Button_R(GameObject obj)
		{
			if (this._flag_ship > 600)
			{
				return;
			}
			this._flag_ship++;
			this.set_flagship_texture(this._flag_ship);
			if (GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture == null)
			{
				GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffddaa]" + this._flag_ship;
			}
			else
			{
				GameObject.Find("Debug_ship/Label_ship").GetComponent<UILabel>().text = "[ffffff]" + this._flag_ship;
			}
		}

		public void Pressed_DBG_Button_UP(GameObject obj)
		{
			if (this._dbg_class == 10)
			{
				return;
			}
			this._dbg_class++;
			for (int i = 1; i <= 3; i++)
			{
				this.texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
				this.texture.mainTexture = (Resources.Load("Textures/Record/RecordTextures/NewUI/class_bg/class_" + string.Format("{0:00}", this._dbg_class)) as Texture);
				this.texture.MakePixelPerfect();
			}
		}

		public void Pressed_DBG_Button_DOWN(GameObject obj)
		{
			if (this._dbg_class == 1)
			{
				return;
			}
			this._dbg_class--;
			for (int i = 1; i <= 3; i++)
			{
				this.texture = GameObject.Find("board1/page" + i + "/bg_class").GetComponent<UITexture>();
				this.texture.mainTexture = (Resources.Load("Textures/Record/RecordTextures/NewUI/class_bg/class_" + string.Format("{0:00}", this._dbg_class)) as Texture);
				this.texture.MakePixelPerfect();
			}
		}

		public void set_flagship_texture(int _flag_ship)
		{
			ShipOffset shipOffset = new ShipOffset(_flag_ship);
			RecordShipLocation recordShipLocation = Resources.Load<RecordShipLocation>("Data/RecordShipLocation");
			float num;
			float num2;
			int texNum;
			if (!this._damaged)
			{
				num = (float)recordShipLocation.param.get_Item(_flag_ship).Rec_X2 - 370f;
				num2 = (float)recordShipLocation.param.get_Item(_flag_ship).Rec_Y2 - 180f;
				texNum = 9;
			}
			else
			{
				num = (float)recordShipLocation.param.get_Item(_flag_ship).Rec_dam_X2 - 370f;
				num2 = (float)recordShipLocation.param.get_Item(_flag_ship).Rec_dam_Y2 - 180f;
				texNum = 10;
			}
			GameObject.Find("Secretary/shipgirl").get_transform().localPositionX(num);
			GameObject.Find("Secretary/shipgirl").get_transform().localPositionY(num2);
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_flag_ship, texNum);
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().MakePixelPerfect();
			Vector2 localSize = GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().localSize;
			GameObject.Find("Secretary/shipgirl").GetComponent<UITexture>().SetDimensions((int)localSize.x, (int)localSize.y);
			if (this._DEBUG_MODE_NOW_)
			{
				mst_shipgraphfaceanchor mst_shipgraphfaceanchor = Resources.Load<mst_shipgraphfaceanchor>("Data/Mst_ShipGraphFaceAnchor");
				float x = (float)mst_shipgraphfaceanchor.param.get_Item(_flag_ship).facea9_x + num + 108f;
				float y = (float)mst_shipgraphfaceanchor.param.get_Item(_flag_ship).facea9_y + num2 - 152f;
				GameObject.Find("Secretary/facea").get_transform().localPositionX(x);
				GameObject.Find("Secretary/facea").get_transform().localPositionY(y);
				x = (float)mst_shipgraphfaceanchor.param.get_Item(_flag_ship).faceb9_x + num + 108f;
				y = (float)mst_shipgraphfaceanchor.param.get_Item(_flag_ship).faceb9_y + num2 - 152f;
				GameObject.Find("Secretary/faceb").get_transform().localPositionX(x);
				GameObject.Find("Secretary/faceb").get_transform().localPositionY(y);
			}
		}

		public void SetCursorColor(Color col)
		{
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().defaultColor = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().defaultColor = col;
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().hover = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().hover = col;
			GameObject.Find("RecordScene/btn/Button_L").GetComponent<UIButton>().pressed = col;
			GameObject.Find("RecordScene/btn/Button_R").GetComponent<UIButton>().pressed = col;
		}
	}
}
