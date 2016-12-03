using Common.Enum;
using KCV.PopupString;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board : MonoBehaviour
	{
		private repair rep;

		private UIGrid grid;

		private UISprite bds;

		private UISprite bds2;

		private UISprite ele_d;

		private UILabel lab;

		private UIButton uibutton;

		private sw swsw;

		private GameObject[] btn_Stk = new GameObject[4];

		private GameObject[] r_now = new GameObject[4];

		private GameObject[] dock = new GameObject[4];

		private GameObject[] dock_cursor = new GameObject[4];

		private GameObject[] repair_btn = new GameObject[4];

		private GameObject[] kira_obj = new GameObject[4];

		private GameObject[] label1_obj = new GameObject[4];

		private GameObject[] label2_obj = new GameObject[4];

		private GameObject[] label3_obj = new GameObject[4];

		private GameObject[] shutter = new GameObject[4];

		private int[] dock_flag = new int[4];

		private int[] MaxHP = new int[4];

		private bool[] dock_anime = new bool[4];

		private bool[] dock_HS_anime = new bool[4];

		private RepairManager _clsRepair;

		private UITexture tex;

		private ShipModel _clsShipModel;

		private board2 bd2;

		private board3 bd3;

		private dialog dia;

		private dialog2 dia2;

		private dialog3 dia3;

		private KeyControl dockSelectController;

		private GameObject cursor;

		private int _go_kosoku;

		private int _select_dock;

		private int _go_kosoku_m;

		private int _select_dock_m;

		private int dock_count;

		private bool _dock_exist;

		private bool _HOLT;

		private bool _HOLT_DIALOG;

		private bool _isStartUpDone;

		private Animation _ANI;

		private bool _isBtnMaruUp;

		private bool _first_key;

		private int now_kit;

		private bool _already_des;

		private int now_dock;

		private bool now_high;

		[SerializeField]
		private ButtonLightTexture[] btnLight = new ButtonLightTexture[4];

		private Color TextShadowLight = new Color(0.63f, 0.91f, 1f);

		private int _imfocus;

		private void btnLights(bool Play)
		{
			this.btnLights(Play, false);
		}

		private void btnLights(bool Play, bool force)
		{
			if (this.now_high == Play && !force)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (Play)
				{
					this.btnLight[i].PlayAnim();
				}
				else
				{
					this.btnLight[i].StopAnim();
				}
			}
			this.now_high = Play;
		}

		public void dock_flag_init()
		{
			for (int i = 0; i < this._clsRepair.GetDocks().Length; i++)
			{
				if (this._clsRepair.GetDockData(i).ShipId != 0 && this._clsRepair.GetDockData(i).RemainingTurns != 0)
				{
					this.dock_flag[i] = 2;
				}
				else
				{
					this.dock_flag[i] = 1;
				}
			}
			for (int j = this._clsRepair.GetDocks().Length; j < this._clsRepair.MapArea.NDockMax; j++)
			{
				this.dock_flag[j] = 0;
			}
			for (int k = this._clsRepair.MapArea.NDockMax; k < 4; k++)
			{
				this.dock_flag[k] = -1;
			}
		}

		public void set_anime(int no, bool stat)
		{
			this.dock_anime[no] = stat;
			if (stat)
			{
				string arg_22_0 = "board1_top/board/Grid/0";
				int num = no;
				GameObject.Find(arg_22_0 + num.ToString() + "/repair_now/btn_high_repair").get_transform().set_localScale(Vector3.get_one());
			}
			else
			{
				string arg_53_0 = "board1_top/board/Grid/0";
				int num2 = no;
				GameObject.Find(arg_53_0 + num2.ToString() + "/repair_now/btn_high_repair").get_transform().set_localScale(Vector3.get_zero());
			}
		}

		public bool get_anime(int no)
		{
			return this.dock_anime[no];
		}

		public void set_HS_anime(int no, bool stat)
		{
			if (this.dock_HS_anime[no] == stat)
			{
				return;
			}
			this.dock_HS_anime[no] = stat;
			if (stat)
			{
				this.now_kit--;
			}
		}

		public bool get_HS_anime(int no)
		{
			return this.dock_HS_anime[no];
		}

		public GameObject get_dock_obj(int i)
		{
			return this.dock[i];
		}

		public void ResetMaruKey()
		{
			this._isBtnMaruUp = false;
		}

		public static int SortByNameDesc(Transform a, Transform b)
		{
			return string.Compare(b.get_name(), a.get_name());
		}

		public void set_cx(int cx)
		{
			this._go_kosoku = cx;
		}

		public void set_rnow_enable(int dock, bool a)
		{
			this.r_now[dock].SetActive(a);
		}

		public void set_stk_enable(int dock, bool a)
		{
			this.btn_Stk[dock].SetActive(a);
		}

		public int get_dock_MaxHP(int dock)
		{
			return (dock >= 0 && dock <= 3) ? this.MaxHP[dock] : -1;
		}

		public void set_dock_MaxHP(int dock, int value)
		{
			if (dock < 0 || dock > 3)
			{
				return;
			}
			this.MaxHP[dock] = value;
		}

		public int get_dock_count()
		{
			return this.dock_count;
		}

		public int get_dock_touchable_count()
		{
			int num = this.dock_count;
			if (num == this._clsRepair.MapArea.NDockMax)
			{
				return num;
			}
			return num + 1;
		}

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
			Mem.Del<UIGrid>(ref this.grid);
			Mem.Del(ref this.bds);
			Mem.Del(ref this.bds2);
			Mem.Del(ref this.ele_d);
			Mem.Del<UILabel>(ref this.lab);
			Mem.Del<UIButton>(ref this.uibutton);
			Mem.Del<sw>(ref this.swsw);
			Mem.Del<GameObject[]>(ref this.btn_Stk);
			Mem.Del<GameObject[]>(ref this.r_now);
			Mem.Del<GameObject[]>(ref this.dock);
			Mem.Del<GameObject[]>(ref this.dock_cursor);
			Mem.Del<GameObject[]>(ref this.repair_btn);
			Mem.Del<GameObject[]>(ref this.kira_obj);
			Mem.Del<GameObject[]>(ref this.label1_obj);
			Mem.Del<GameObject[]>(ref this.label2_obj);
			Mem.Del<GameObject[]>(ref this.label3_obj);
			Mem.Del<GameObject[]>(ref this.shutter);
			Mem.Del<int[]>(ref this.dock_flag);
			Mem.Del<int[]>(ref this.MaxHP);
			Mem.Del<bool[]>(ref this.dock_anime);
			Mem.Del<bool[]>(ref this.dock_HS_anime);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<UITexture>(ref this.tex);
			Mem.Del<ShipModel>(ref this._clsShipModel);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<dialog>(ref this.dia);
			Mem.Del<dialog2>(ref this.dia2);
			Mem.Del<dialog3>(ref this.dia3);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<GameObject>(ref this.cursor);
			Mem.Del<Animation>(ref this._ANI);
			Mem.Del<ButtonLightTexture[]>(ref this.btnLight);
			Mem.Del<Color>(ref this.TextShadowLight);
		}

		public void _init_repair()
		{
			this._already_des = false;
			this._isBtnMaruUp = false;
			this._HOLT = false;
			this._HOLT_DIALOG = false;
			this._isStartUpDone = false;
			this._isBtnMaruUp = false;
			this.rep = base.get_gameObject().get_transform().get_parent().get_parent().GetComponent<repair>();
		}

		public void StartUP()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.set_mode(-1);
			if (this._isStartUpDone)
			{
				return;
			}
			this._isStartUpDone = true;
			this.bd2 = this.rep.get_transform().FindChild("board2_top/board2").GetComponent<board2>();
			this.bd3 = this.rep.get_transform().FindChild("board3_top/board3").GetComponent<board3>();
			this.dia = GameObject.Find("dialog").GetComponent<dialog>();
			this.dia2 = GameObject.Find("dialog2").GetComponent<dialog2>();
			this.dia3 = GameObject.Find("dialog3").GetComponent<dialog3>();
			for (int i = 0; i < 4; i++)
			{
				GameObject[] arg_DE_0 = this.shutter;
				int arg_DE_1 = i;
				string arg_D4_0 = "board1_top/board/Grid/0";
				int num = i;
				arg_DE_0[arg_DE_1] = GameObject.Find(arg_D4_0 + num.ToString() + "/Shutter");
			}
			this._clsRepair = this.rep.now_clsRepair();
			this.now_kit = this._clsRepair.Material.RepairKit;
			this._ANI = this.rep.get_transform().FindChild("info").GetComponent<Animation>();
			int num2 = this._clsRepair.MapArea.NDockCount;
			if (num2 == this._clsRepair.MapArea.NDockMax)
			{
				num2--;
			}
			this.dockSelectController = new KeyControl(0, num2, 0.4f, 0.1f);
			this.dock_flag_init();
			this.dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			this.dockSelectController.isLoopIndex = false;
			this._first_key = false;
			this._go_kosoku = 0;
			this._go_kosoku_m = -1;
			this._select_dock = 0;
			this._select_dock_m = -1;
			this._dock_exist = false;
			this.redraw();
			this.bd2.StartUp();
			this.bd3.StartUp();
		}

		public void redraw()
		{
			this.redraw(true);
		}

		public void redraw(bool anime)
		{
			this.redraw(true, -1);
		}

		public void redraw(bool anime, int mode)
		{
			this.rep.set_mode(1);
			if (!this._dock_exist)
			{
				this.board_dock_clear();
			}
			if (anime)
			{
				this.DockMake(this.rep.NowArea());
			}
			else if (!this._dock_exist)
			{
				this.DockMake(this.rep.NowArea(), false, mode);
			}
			else
			{
				this.DockMake(this.rep.NowArea(), false);
			}
			this.DockStatus(this.rep.NowArea());
		}

		public void DockMake()
		{
			this.DockMake(this.rep.NowArea(), true);
		}

		public void DockMake(int MapArea)
		{
			this.DockMake(MapArea, true);
		}

		public void DockMake(int MapArea, bool anime)
		{
			this.DockMake(MapArea, anime, -1);
		}

		public void DockMake(int MapArea, bool anime, int mode)
		{
			int num = 4;
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			UIGrid component = GameObject.Find("board1_top/board/Grid").GetComponent<UIGrid>();
			if (!this._dock_exist)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject[] arg_59_0 = this.dock;
					int arg_59_1 = i;
					string arg_4F_0 = "board1_top/board/Grid/0";
					int num2 = i;
					arg_59_0[arg_59_1] = GameObject.Find(arg_4F_0 + num2.ToString());
					this.dock[i].set_name(string.Empty + string.Format("{0:00}", i));
					this.dock[i].get_gameObject().get_transform().set_localScale(Vector3.get_one());
					this.shutter[i].get_transform().set_localScale(Vector3.get_one());
					this.dock_cursor[i] = GameObject.Find("board1_top/board/Grid/" + this.dock[i].get_name() + "/bg/BackGround");
					GameObject[] arg_107_0 = this.kira_obj;
					int arg_107_1 = i;
					string arg_F8_0 = "board1_top/board/Grid/0";
					int num3 = i;
					arg_107_0[arg_107_1] = GameObject.Find(arg_F8_0 + num3.ToString() + "/Anime_H/crane1/wire/item/Light").get_gameObject();
					GameObject[] arg_12D_0 = this.btn_Stk;
					int arg_12D_1 = i;
					string arg_123_0 = "board/Grid/0";
					int num4 = i;
					arg_12D_0[arg_12D_1] = GameObject.Find(arg_123_0 + num4.ToString() + "/btn_Sentaku");
					GameObject[] arg_153_0 = this.r_now;
					int arg_153_1 = i;
					string arg_149_0 = "board/Grid/0";
					int num5 = i;
					arg_153_0[arg_153_1] = GameObject.Find(arg_149_0 + num5.ToString() + "/repair_now");
					this.repair_btn[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/btn_high_repair/Background");
					this.label1_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_lv");
					this.label2_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_day");
					this.label3_obj[i] = GameObject.Find("board/Grid/0" + i + "/repair_now/text_ato");
					this.dock[i].get_gameObject().get_transform().localPosition(new Vector3(0f, (float)i * -100f, 0f));
					GameObject.Find("board1_top/board/Grid/" + this.dock[i].get_name() + "/Anime").GetComponent<UIPanel>().depth = i + 14;
					GameObject.Find("board1_top/board/Grid/" + this.dock[i].get_name() + "/Anime_H").GetComponent<UIPanel>().depth = i + 14;
				}
			}
			for (int j = 0; j < num; j++)
			{
				if (!anime)
				{
					if (mode == -1)
					{
						iTween.MoveTo(this.dock[j].get_gameObject(), iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							(float)j * -100f + 530f,
							"time",
							0f
						}));
					}
					else if (mode == j)
					{
						iTween.MoveTo(this.dock[j].get_gameObject(), iTween.Hash(new object[]
						{
							"islocal",
							true,
							"y",
							(float)j * -100f + 530f,
							"time",
							0f
						}));
					}
				}
			}
			component.Reposition();
			this._dock_exist = true;
		}

		public void DockStatus()
		{
			this.DockStatus(this.rep.NowArea(), -1);
		}

		public void DockStatus(int MapArea)
		{
			this.DockStatus(MapArea, -1);
		}

		public void DockStatus(int MapArea, int TargetDock)
		{
			this._first_key = true;
			Mst_DataManager mstManager = this.rep.GetMstManager();
			Mst_ship mst_ship = this.rep.GetMst_ship();
			this.rep.update_portframe();
			this._clsRepair = this.rep.now_clsRepair();
			this.dock_count = this._clsRepair.GetDocks().Length;
			for (int i = 0; i < this.dock_count; i++)
			{
				string arg_6A_0 = "board1_top/board/Grid/0";
				int num = i;
				this.bds2 = GameObject.Find(arg_6A_0 + num.ToString() + "/bg/BackGround").GetComponent<UISprite>();
				this.bds2.spriteName = "list_bg";
				string arg_A3_0 = "board1_top/board/Grid/0";
				int num2 = i;
				this.bds2 = GameObject.Find(arg_A3_0 + num2.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
				this.bds2.spriteName = "cardArea_YB";
				string arg_DC_0 = "board1_top/board/Grid/0";
				int num3 = i;
				this.bds2 = GameObject.Find(arg_DC_0 + num3.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
				this.bds2.spriteName = this.AreaIdToSeaSpriteName(this.rep.NowArea());
				string arg_120_0 = "board1_top/board/Grid/0";
				int num4 = i;
				GameObject.Find(arg_120_0 + num4.ToString() + "/Shutter/BGKey").get_transform().localScaleZero();
				string arg_148_0 = "board1_top/board/Grid/0";
				int num5 = i;
				GameObject.Find(arg_148_0 + num5.ToString() + "/Shutter/BGMes").get_transform().localScaleZero();
				if (TargetDock == -1)
				{
					this.shutter[i].get_transform().set_localScale(Vector3.get_zero());
				}
				this.dock_cursor[i] = GameObject.Find("board1_top/board/Grid/" + this.dock[i].get_name() + "/bg/BackGround");
				this.btn_Stk[i].SetActive(true);
			}
			int nDockMax = this._clsRepair.MapArea.NDockMax;
			for (int j = this.dock_count; j < nDockMax; j++)
			{
				if (TargetDock == -1 || TargetDock == j)
				{
					string arg_204_0 = "board1_top/board/Grid/0";
					int num6 = j;
					this.bds2 = GameObject.Find(arg_204_0 + num6.ToString() + "/bg/BackGround").GetComponent<UISprite>();
					this.bds2.spriteName = "list_bg_bar_closed";
					string arg_23E_0 = "board1_top/board/Grid/0";
					int num7 = j;
					this.bds2 = GameObject.Find(arg_23E_0 + num7.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
					this.bds2.spriteName = null;
					string arg_274_0 = "board1_top/board/Grid/0";
					int num8 = j;
					this.bds2 = GameObject.Find(arg_274_0 + num8.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
					this.bds2.spriteName = null;
					if (this._first_key)
					{
						if (this._clsRepair.IsValidOpenNewDock())
						{
							string arg_2C4_0 = "board1_top/board/Grid/0";
							int num9 = j;
							GameObject.Find(arg_2C4_0 + num9.ToString() + "/Shutter/BGMes").GetComponent<UISprite>().spriteName = "huki_r_02";
							string arg_2F2_0 = "board1_top/board/Grid/0";
							int num10 = j;
							GameObject.Find(arg_2F2_0 + num10.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
						}
						else
						{
							string arg_325_0 = "board1_top/board/Grid/0";
							int num11 = j;
							GameObject.Find(arg_325_0 + num11.ToString() + "/Shutter/BGMes").GetComponent<UISprite>().spriteName = "huki_r_01";
							string arg_353_0 = "board1_top/board/Grid/0";
							int num12 = j;
							GameObject.Find(arg_353_0 + num12.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
						}
						string arg_381_0 = "board1_top/board/Grid/0";
						int num13 = j;
						GameObject.Find(arg_381_0 + num13.ToString() + "/Shutter/BGKey").get_transform().localScaleOne();
						this._first_key = false;
					}
					else
					{
						string arg_3B6_0 = "board1_top/board/Grid/0";
						int num14 = j;
						GameObject.Find(arg_3B6_0 + num14.ToString() + "/Shutter/BGKey").get_transform().localScaleZero();
						string arg_3DF_0 = "board1_top/board/Grid/0";
						int num15 = j;
						GameObject.Find(arg_3DF_0 + num15.ToString() + "/Shutter/BGMes").get_transform().localScaleZero();
					}
					this.shutter[j].get_transform().set_localScale(Vector3.get_one());
					this.dock_cursor[j] = GameObject.Find("board1_top/board/Grid/" + this.dock[j].get_name() + "/Shutter/ShutterALL");
					this.dock_cursor[j].get_transform().set_localScale(Vector3.get_one());
					this.btn_Stk[j].SetActive(j <= this.dock_count);
					this.r_now[j].SetActive(false);
				}
			}
			for (int k = nDockMax; k < 4; k++)
			{
				if (TargetDock == -1 || TargetDock == k)
				{
					string arg_4BA_0 = "board1_top/board/Grid/0";
					int num16 = k;
					this.bds2 = GameObject.Find(arg_4BA_0 + num16.ToString() + "/bg/BackGround").GetComponent<UISprite>();
					this.bds2.spriteName = null;
					string arg_4F0_0 = "board1_top/board/Grid/0";
					int num17 = k;
					this.bds2 = GameObject.Find(arg_4F0_0 + num17.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
					this.bds2.spriteName = null;
					string arg_526_0 = "board1_top/board/Grid/0";
					int num18 = k;
					this.bds2 = GameObject.Find(arg_526_0 + num18.ToString() + "/bg/BackGround2").GetComponent<UISprite>();
					this.bds2.spriteName = null;
					string arg_55B_0 = "board1_top/board/Grid/0";
					int num19 = k;
					GameObject.Find(arg_55B_0 + num19.ToString() + "/Shutter/BGKey").get_transform().localScaleZero();
					string arg_584_0 = "board1_top/board/Grid/0";
					int num20 = k;
					GameObject.Find(arg_584_0 + num20.ToString() + "/Shutter/BGMes").get_transform().localScaleZero();
					this.shutter[k].get_transform().set_localScale(Vector3.get_zero());
					this.btn_Stk[k].SetActive(false);
					this.r_now[k].SetActive(false);
				}
			}
			for (int l = 0; l < this.dock_count; l++)
			{
				crane_anime component = GameObject.Find("board/Grid/0" + l.ToString() + "/Anime").GetComponent<crane_anime>();
				ShipModel ship = this._clsRepair.GetDockData(l).GetShip();
				if (TargetDock == -1 || TargetDock == l)
				{
					if (this._clsRepair.GetDockData(l).ShipId != 0 && this._clsRepair.GetDockData(l).RemainingTurns != 0)
					{
						this.r_now[l].SetActive(true);
						this.btn_Stk[l].SetActive(false);
						string arg_68B_0 = "board/Grid/0";
						int num21 = l;
						this.tex = GameObject.Find(arg_68B_0 + num21.ToString() + "/repair_now/ship_banner").GetComponent<UITexture>();
						if (ship.DamageStatus == DamageState.Taiha)
						{
							this.tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 2);
						}
						else if (ship.DamageStatus == DamageState.Tyuuha)
						{
							this.tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 2);
						}
						else
						{
							this.tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 1);
						}
						string arg_73F_0 = "board/Grid/0";
						int num22 = l;
						this.lab = GameObject.Find(arg_73F_0 + num22.ToString() + "/repair_now/text_ship_name").GetComponent<UILabel>();
						this.lab.text = ship.Name;
						string arg_77B_0 = "board/Grid/0";
						int num23 = l;
						this.lab = GameObject.Find(arg_77B_0 + num23.ToString() + "/repair_now/text_level").GetComponent<UILabel>();
						this.lab.text = string.Empty + ship.Level;
						string arg_7C6_0 = "board/Grid/0";
						int num24 = l;
						this.lab = GameObject.Find(arg_7C6_0 + num24.ToString() + "/repair_now/text_hp").GetComponent<UILabel>();
						this.lab.text = ship.NowHp + "/" + ship.MaxHp;
						this.MaxHP[l] = ship.MaxHp;
						string arg_82D_0 = "board/Grid/0";
						int num25 = l;
						this.ele_d = GameObject.Find(arg_82D_0 + num25.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter2").GetComponent<UISprite>();
						this.ele_d.width = (int)((float)ship.NowHp * 210f / (float)ship.MaxHp);
						this.ele_d.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
						string arg_898_0 = "board/Grid/0";
						int num26 = l;
						this.ele_d = GameObject.Find(arg_898_0 + num26.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter").GetComponent<UISprite>();
						this.ele_d.width = (int)((float)ship.NowHp * 210f / (float)ship.MaxHp);
						this.ele_d.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
						string arg_903_0 = "board/Grid/0";
						int num27 = l;
						this.lab = GameObject.Find(arg_903_0 + num27.ToString() + "/repair_now/text_least_time").GetComponent<UILabel>();
						this.lab.text = string.Empty + this._clsRepair.GetDockData(l).RemainingTurns;
						string arg_959_0 = "board/Grid/0";
						int num28 = l;
						this.uibutton = GameObject.Find(arg_959_0 + num28.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>();
						if (this._clsRepair.Material.RepairKit > 0)
						{
							this.uibutton.isEnabled = true;
						}
						else
						{
							this.uibutton.isEnabled = false;
						}
						this.set_anime(l, false);
						component.start_anime(l);
					}
					else
					{
						this.r_now[l].SetActive(false);
						this.btn_Stk[l].SetActive(true);
						component.stop_anime(l);
						this.set_anime(l, false);
					}
				}
			}
			this.swsw = GameObject.Find("board3_top/board3/sw01").GetComponent<sw>();
			this.swsw.set_sw_stat(this._clsRepair.Material.RepairKit > 0);
			this._dock_exist = true;
		}

		public void set_kira(bool value)
		{
			if (this.kira_obj[0] == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.kira_obj[i].SetActive(value);
			}
		}

		public void board_dock_clear()
		{
			if (!this._dock_exist)
			{
				return;
			}
			this.grid = GameObject.Find("board1_top/board/Grid").GetComponent<UIGrid>();
			GameObject[] children = this.grid.get_gameObject().GetChildren(true);
			for (int i = 0; i < children.Length; i++)
			{
				Object.Destroy(children[i]);
			}
			this.grid.get_transform().DetachChildren();
			this._dock_exist = false;
		}

		public void DockCursorBlink(int index)
		{
			UISelectedObject.SelectedObjectBlink(this.dock_cursor, index);
		}

		private void Update()
		{
			if (!this._isStartUpDone || !this.rep.isFadeDone())
			{
				this.StartUP();
			}
			if (!this._isStartUpDone)
			{
				return;
			}
			if (this._HOLT)
			{
				return;
			}
			int index = this.dockSelectController.Index;
			this.dockSelectController.Update();
			if (this.dockSelectController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			if (this.rep.now_mode() != 1)
			{
				this.dockSelectController.Index = index;
				return;
			}
			if (this.dockSelectController.keyState.get_Item(8).down)
			{
			}
			if (this.dockSelectController.keyState.get_Item(12).down)
			{
			}
			if (this.bd2.get_board2_anime() || this.dia2.get_dialog2_anime() || this.dia.get_dialog_anime())
			{
				return;
			}
			if (!this._isBtnMaruUp && (this.dockSelectController.keyState.get_Item(1).up || !this.dockSelectController.keyState.get_Item(1).down))
			{
				this._isBtnMaruUp = true;
				return;
			}
			if (this.rep.first_change())
			{
				UISelectedObject.SelectedObjectBlink(this.dock_cursor, this.dockSelectController.Index);
				this.btnLights(false, true);
				return;
			}
			if (this.dockSelectController.IsChangeIndex)
			{
				UISelectedObject.SelectedObjectBlink(this.dock_cursor, this.dockSelectController.Index);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			for (int i = 0; i < this.dock_cursor.Length; i++)
			{
				if (this._clsRepair.Material.RepairKit <= 0 || this.now_kit <= 0)
				{
					this.repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick_off";
					this.btnLights(false);
				}
				else
				{
					this.btnLights(true);
					if (this.dockSelectController.Index == i)
					{
						if (!this.get_HS_anime(i))
						{
							this.repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick_on";
						}
					}
					else if (!this.get_HS_anime(i))
					{
						this.repair_btn[i].GetComponent<UISprite>().spriteName = "btn_quick";
					}
				}
				if (this.dockSelectController.Index == i)
				{
					this.label1_obj[i].GetComponent<UILabel>().effectColor = this.TextShadowLight;
					this.label2_obj[i].GetComponent<UILabel>().effectColor = this.TextShadowLight;
					this.label3_obj[i].GetComponent<UILabel>().effectColor = this.TextShadowLight;
				}
				else
				{
					this.label1_obj[i].GetComponent<UILabel>().effectColor = Color.get_white();
					this.label2_obj[i].GetComponent<UILabel>().effectColor = Color.get_white();
					this.label3_obj[i].GetComponent<UILabel>().effectColor = Color.get_white();
				}
			}
			if (this.dockSelectController.IsChangeIndex)
			{
				UISelectedObject.SelectedObjectBlink(this.dock_cursor, this.dockSelectController.Index);
				for (int j = 0; j < this.dock_cursor.Length; j++)
				{
					if (this._clsRepair.Material.RepairKit <= 0 || this.now_kit <= 0)
					{
						this.repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick_off";
						this.btnLights(false);
					}
					else
					{
						this.btnLights(true);
						if (this.dockSelectController.Index == j)
						{
							if (!this.get_HS_anime(j))
							{
								this.repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick_on";
							}
						}
						else if (!this.get_HS_anime(j))
						{
							this.repair_btn[j].GetComponent<UISprite>().spriteName = "btn_quick";
						}
					}
					if (this.dockSelectController.Index == j)
					{
						this.label1_obj[j].GetComponent<UILabel>().effectColor = new Color(0.63f, 0.91f, 1f);
						this.label2_obj[j].GetComponent<UILabel>().effectColor = new Color(0.63f, 0.91f, 1f);
					}
					else
					{
						this.label1_obj[j].GetComponent<UILabel>().effectColor = Color.get_white();
						this.label2_obj[j].GetComponent<UILabel>().effectColor = Color.get_white();
					}
				}
				for (int k = 0; k < this._clsRepair.MapArea.NDockMax; k++)
				{
					if (this._clsRepair.MapArea.NDockCount == this.dockSelectController.Index)
					{
						string arg_4B0_0 = "board1_top/board/Grid/0";
						int num = k;
						GameObject.Find(arg_4B0_0 + num.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock_on";
					}
					else
					{
						string arg_4E2_0 = "board1_top/board/Grid/0";
						int num2 = k;
						GameObject.Find(arg_4E2_0 + num2.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
					}
				}
			}
			if (this.dockSelectController.Index < this._clsRepair.GetDocks().Length)
			{
				if (this._clsRepair.GetDockData(this.dockSelectController.Index).ShipId != 0)
				{
					this._go_kosoku = 1;
				}
				else
				{
					this._go_kosoku = 0;
				}
			}
			if (this.dockSelectController.keyState.get_Item(0).down)
			{
				this.back_to_port();
			}
			else if (this.dockSelectController.keyState.get_Item(1).down && this._isBtnMaruUp)
			{
				this._isBtnMaruUp = false;
				this.now_dock = this.dockSelectController.Index;
				if (this._clsRepair.Material.RepairKit <= 0)
				{
					this.now_kit = -1;
				}
				this.dock_selected(this.dockSelectController.Index);
			}
			else if (this.dockSelectController.keyState.get_Item(5).down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}

		public void dock_selected(int dockNo)
		{
			this.DockCursorBlink(dockNo);
			this.dockSelectController.Index = dockNo;
			for (int i = this.dock_count; i < this._clsRepair.MapArea.NDockMax; i++)
			{
				if (this._clsRepair.IsValidOpenNewDock())
				{
					string arg_42_0 = "board1_top/board/Grid/0";
					int num = i;
					GameObject.Find(arg_42_0 + num.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
				}
				else
				{
					string arg_73_0 = "board1_top/board/Grid/0";
					int num2 = i;
					GameObject.Find(arg_73_0 + num2.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock";
				}
			}
			if (dockNo < this._clsRepair.GetDocks().Length)
			{
				if (this._clsRepair.GetDockData(this.dockSelectController.Index).ShipId != 0)
				{
					this._go_kosoku = 1;
				}
				else
				{
					this._go_kosoku = 0;
				}
				if (this._go_kosoku == 0)
				{
					if (this._clsRepair.GetDockData(dockNo).ShipId != 0 || this.get_HS_anime(dockNo))
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
					}
					else
					{
						this.rep.set_mode(-2);
						GameObject.Find("dialog_top/dialog").GetComponent<dialog>().SetDock(dockNo);
						this.bd2 = GameObject.Find("board2").GetComponent<board2>();
						this.bd2.board2_appear(true);
						this.bd2.set_touch_mode(true);
						this.rep.setmask(1, true);
						this.rep.set_mode(2);
					}
				}
				else if (this.get_HS_anime(dockNo) || this.now_kit <= 0)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
				else if (this._clsRepair.IsValidChangeRepairSpeed(dockNo))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					this.rep.set_mode(-5);
					this.dia2.UpdateInfo(dockNo);
					this.dia2.SetDock(dockNo);
					this.rep.setmask(3, true);
					this.dia2.dialog2_appear(true);
					this.rep.set_mode(5);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
			}
			else
			{
				string arg_24C_0 = "board1_top/board/Grid/0";
				int num3 = dockNo;
				GameObject.Find(arg_24C_0 + num3.ToString() + "/Shutter/BGKey").GetComponent<UISprite>().spriteName = "btn_addDock_on";
				if (this._clsRepair.IsValidOpenNewDock())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					this.dia3.UpdateInfo(dockNo);
					this.dia3.dialog3_appear(true);
					this.rep.set_mode(6);
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
					CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NoDockKey));
				}
			}
		}

		public void OpenDock(int dockNo)
		{
			this._clsRepair.OpenNewDock();
			GameObject.Find("info/text_open").GetComponent<Animation>().PlayQueued("go_open");
			int num = this.get_dock_touchable_count();
			if (num == this._clsRepair.MapArea.NDockMax)
			{
				num--;
			}
			this.dockSelectController.setMaxIndex(num);
			if (this._clsRepair.MapArea.NDockCount != this._clsRepair.MapArea.NDockMax)
			{
				this.DockStatus(this.rep.NowArea(), dockNo + 1);
			}
			this.DockStatus(this.rep.NowArea(), dockNo);
			GameObject.Find("board1_top/board/Grid/0" + (this._clsRepair.MapArea.NDockCount - 1).ToString() + "/Shutter/ShutterALL").get_transform().set_localScale(Vector3.get_zero());
			GameObject.Find("board1_top/board/Grid/0" + (this._clsRepair.MapArea.NDockCount - 1).ToString() + "/Shutter").GetComponent<Animation>().Play();
			UISelectedObject.SelectedObjectBlink(this.dock_cursor, dockNo);
		}

		private void Pop_Repairs()
		{
			GameObject.Find("board/Grid/00/Anime").GetComponent<crane_anime>().HPgrowStop();
			GameObject.Find("Repair Root/info/text_open").GetComponent<Animation>().Stop();
			GameObject.Find("Repair Root/info/text_open/text_open").get_transform().localPositionX(784f);
			GameObject.Find("Repair Root/info/text_open/text_bg").get_transform().localPositionX(784f);
			this.bd2.Cancelled(true, true);
			this.bd3.Cancelled(true, true);
			for (int i = 0; i < 4; i++)
			{
				this.dock[i].get_gameObject().get_transform().set_localScale(Vector3.get_zero());
				this.shutter[i].get_transform().set_localScale(Vector3.get_one());
				string arg_C3_0 = "board1_top/board/Grid/0";
				int num = i;
				GameObject.Find(arg_C3_0 + num.ToString() + "/Shutter/ShutterALL").get_transform().set_localScale(Vector3.get_one());
				string arg_F0_0 = "board1_top/board/Grid/0";
				int num2 = i;
				GameObject.Find(arg_F0_0 + num2.ToString() + "/Shutter/ShutterL").get_transform().localPositionX(0f);
				string arg_11E_0 = "board1_top/board/Grid/0";
				int num3 = i;
				GameObject.Find(arg_11E_0 + num3.ToString() + "/Shutter/ShutterR").get_transform().localPositionX(-1f);
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime_H").GetComponent<Animation>().Stop();
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime_H/crane1").get_gameObject().get_transform().localPositionX(520f);
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime").GetComponent<Animation>().Stop();
				GameObject.Find("board/Grid/0" + i.ToString() + "/Anime/crane1").get_gameObject().get_transform().localPositionX(520f);
				string arg_1F7_0 = "board1_top/board/Grid/0";
				int num4 = i;
				this.bds2 = GameObject.Find(arg_1F7_0 + num4.ToString() + "/bg/BackGround3").GetComponent<UISprite>();
				this.bds2.spriteName = "cardArea_YB";
				if (this.get_HS_anime(i))
				{
					this.rep.now_clsRepair().ChangeRepairSpeed(i);
					this.set_anime(i, false);
					this.set_HS_anime(i, false);
				}
			}
			for (int j = 0; j < 3; j++)
			{
				GameObject.Find(string.Concat(new string[]
				{
					"board",
					(j + 1).ToString(),
					"_top_mask/board",
					(j + 1).ToString(),
					"_guard"
				})).GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.007f);
			}
			GameObject.Find("Repair_BGS/trusses").get_transform().set_localScale(Vector3.get_one());
			GameObject.Find("Repair_BGS/BG Panel2/bg_cr03").get_transform().set_localScale(Vector3.get_one());
			GameObject.Find("Repair Root/board1_top/header").get_transform().set_localScale(Vector3.get_one());
			GameObject.Find("info/docknotfound").get_transform().localPositionX(-1500f);
			GameObject gameObject = GameObject.Find("ObjectPool/Repair_BGS/Root");
			GameObject.Find("Repair Root/board1_top").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/board1_top_mask").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/board2_top").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/board2_top_mask").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/board3_top").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/board3_top_mask").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/dialog_top").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/dialog2_top").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/debug").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/info").get_transform().set_parent(gameObject.get_transform());
			GameObject.Find("Repair Root/Guide").get_transform().set_parent(gameObject.get_transform());
			this.rep.delete_clsRepair();
			this.rep.delete_MstManager();
			this.rep.delete_Mst_ship();
		}

		private void back_to_port()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		private void Dock_not_found()
		{
			if (this._HOLT_DIALOG)
			{
				return;
			}
			GameObject.Find("Repair_BGS/trusses").get_transform().set_localScale(Vector3.get_zero());
			GameObject.Find("Repair_BGS/BG Panel2/bg_cr03").get_transform().set_localScale(Vector3.get_zero());
			GameObject.Find("board1_top/header").get_transform().set_localScale(Vector3.get_zero());
			UIButton component = GameObject.Find("info/Button").GetComponent<UIButton>();
			UIButtonMessage component2 = component.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "Pressed_Button_Back";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			iTween.MoveTo(GameObject.Find("info/docknotfound"), iTween.Hash(new object[]
			{
				"islocal",
				true,
				"x",
				0f,
				"time",
				2f,
				"easetype",
				iTween.EaseType.easeOutElastic
			}));
			this._HOLT_DIALOG = true;
		}

		private void Pressed_Button_Back(GameObject obj)
		{
			this.back_to_port();
		}

		private string AreaIdToSeaSpriteName(int areaId)
		{
			switch (areaId)
			{
			case 1:
			case 8:
			case 9:
			case 11:
			case 12:
				return "list_sea" + 1;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "list_sea" + 2;
			case 3:
			case 13:
				return "list_sea" + 3;
			case 15:
			case 16:
			case 17:
				return "list_sea" + 4;
			default:
				return "list_sea" + 5;
			}
		}

		[DebuggerHidden]
		private IEnumerator _wait(float time)
		{
			board.<_wait>c__IteratorB9 <_wait>c__IteratorB = new board.<_wait>c__IteratorB9();
			<_wait>c__IteratorB.time = time;
			<_wait>c__IteratorB.<$>time = time;
			return <_wait>c__IteratorB;
		}
	}
}
