using Common.Enum;
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
	public class repair : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		private UISprite sprite;

		private UISprite cursor;

		private UISprite board;

		private UISprite board2;

		private UITexture tex;

		private UILabel lab;

		private UIButton btn;

		private RepairManager _clsRepair;

		private RepairDockModel _clsRepairDockModel;

		private crane_anime _crane;

		private board2 bd2;

		private dialog dg;

		private board1_top_mask bd1m;

		private board2_top_mask bd2m;

		private board3_top_mask bd3m;

		private sw sw;

		private bool first_change_mode;

		private int _MapArea;

		private board bd1;

		private SoundManager sm;

		private UISprite go;

		private int _nTimem;

		private bool _isStartUP;

		private bool _isAsFinished;

		private KeyControl dockSelectController;

		private int _now_texgirl;

		private bool _FadeDone;

		private int nowmode;

		private Mst_DataManager _mstManager;

		private Mst_ship _shipdata;

		private void Awake()
		{
		}

		private void _all_init()
		{
			board component = base.get_transform().FindChild("board1_top/board").GetComponent<board>();
			board2 component2 = base.get_transform().FindChild("board2_top/board2").GetComponent<board2>();
			board3 component3 = base.get_transform().FindChild("board3_top/board3").GetComponent<board3>();
			board3_btn component4 = base.get_transform().FindChild("board3_top/board3/Button").GetComponent<board3_btn>();
			board1_top_mask component5 = base.get_transform().FindChild("board1_top_mask").GetComponent<board1_top_mask>();
			board2_top_mask component6 = base.get_transform().FindChild("board2_top_mask").GetComponent<board2_top_mask>();
			board3_top_mask component7 = base.get_transform().FindChild("board3_top_mask").GetComponent<board3_top_mask>();
			crane_anime component8 = base.get_transform().FindChild("board1_top/board/Grid/01/Anime").GetComponent<crane_anime>();
			dialog component9 = base.get_transform().FindChild("dialog_top/dialog").GetComponent<dialog>();
			dialog2 component10 = base.get_transform().FindChild("dialog2_top/dialog2").GetComponent<dialog2>();
			high_repair component11 = base.get_transform().FindChild("board1_top/board/Grid/00/repair_now/btn_high_repair").GetComponent<high_repair>();
			sentaku component12 = base.get_transform().FindChild("board1_top/board/Grid/00/btn_Sentaku").GetComponent<sentaku>();
			sw component13 = base.get_transform().FindChild("board3_top/board3/sw01").GetComponent<sw>();
			component._init_repair();
			component2._init_repair();
			component3._init_repair();
			component5._init_repair();
			component6._init_repair();
			component7._init_repair();
			component4._init_repair();
			component8._init_repair();
			component9._init_repair();
			component10._init_repair();
			component11._init_repair();
			component12._init_repair();
			component.ResetMaruKey();
			GameObject.Find("Repair Root/debug").get_transform().localPositionY(-120f);
		}

		public Mst_DataManager GetMstManager()
		{
			return this._mstManager;
		}

		public Mst_ship GetMst_ship()
		{
			return this._shipdata;
		}

		public void delete_MstManager()
		{
			this._mstManager = null;
		}

		public void delete_Mst_ship()
		{
			this._shipdata = null;
		}

		private void Start()
		{
			this._MapArea = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			this._clsRepair = new RepairManager(this._MapArea);
			this._mstManager = Mst_DataManager.Instance;
			this._shipdata = new Mst_ship();
			this._FadeDone = false;
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
				{
					SoundUtils.SwitchBGM(BGMFileInfos.PortTools);
					this.OnTransitionFinished();
				}, true, true);
			}
			else
			{
				this.OnTransitionFinished();
			}
			this._isStartUP = false;
			this._now_texgirl = 0;
			this.bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			this.StartUp();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this.sprite);
			Mem.Del(ref this.cursor);
			Mem.Del(ref this.board);
			Mem.Del(ref this.board2);
			Mem.Del<UITexture>(ref this.tex);
			Mem.Del<UILabel>(ref this.lab);
			Mem.Del<UIButton>(ref this.btn);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<RepairDockModel>(ref this._clsRepairDockModel);
			Mem.Del<crane_anime>(ref this._crane);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<dialog>(ref this.dg);
			Mem.Del<board1_top_mask>(ref this.bd1m);
			Mem.Del<board2_top_mask>(ref this.bd2m);
			Mem.Del<board3_top_mask>(ref this.bd3m);
			Mem.Del<sw>(ref this.sw);
			Mem.Del<board>(ref this.bd1);
			Mem.Del<SoundManager>(ref this.sm);
			Mem.Del(ref this.go);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<Mst_DataManager>(ref this._mstManager);
			Mem.Del<Mst_ship>(ref this._shipdata);
		}

		private void OnTransitionFinished()
		{
			this.bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			GameObject.Find("board1_top").GetComponent<UIPanel>().set_enabled(true);
			for (int i = 0; i < 4; i++)
			{
				string arg_3F_0 = "board1_top/board/Grid/0";
				int num = i;
				GameObject.Find(arg_3F_0 + num.ToString()).GetComponent<UIPanel>().set_enabled(true);
				string arg_67_0 = "board1_top/board/Grid/0";
				int num2 = i;
				GameObject.Find(arg_67_0 + num2.ToString() + "/btn_Sentaku").GetComponent<UIPanel>().set_enabled(true);
				string arg_90_0 = "board1_top/board/Grid/0";
				int num3 = i;
				GameObject.Find(arg_90_0 + num3.ToString() + "/bg").GetComponent<UIPanel>().set_enabled(true);
				string arg_B9_0 = "board1_top/board/Grid/0";
				int num4 = i;
				GameObject.Find(arg_B9_0 + num4.ToString() + "/repair_now").GetComponent<UIPanel>().set_enabled(true);
			}
			this._all_init();
			Animation component = GameObject.Find("board1_top/board/Grid").GetComponent<Animation>();
			component.Play();
			this._FadeDone = true;
		}

		private void StartUp()
		{
			if (this._isStartUP)
			{
				return;
			}
			this._isStartUP = true;
			this.bd1m = base.get_transform().FindChild("board1_top_mask").GetComponent<board1_top_mask>();
			this.bd2m = base.get_transform().FindChild("board2_top_mask").GetComponent<board2_top_mask>();
			this.bd3m = base.get_transform().FindChild("board3_top_mask").GetComponent<board3_top_mask>();
			this.bd2 = base.get_transform().FindChild("board2_top/board2").GetComponent<board2>();
			this.sm = SingletonMonoBehaviour<SoundManager>.Instance;
		}

		public bool isFadeDone()
		{
			return this._FadeDone;
		}

		public RepairManager now_clsRepair()
		{
			return this._clsRepair;
		}

		public void delete_clsRepair()
		{
			this._clsRepair = null;
		}

		public RepairDockModel now_clsRepairDockModel()
		{
			return this._clsRepairDockModel;
		}

		public int NowArea()
		{
			return this._MapArea;
		}

		public int now_mode()
		{
			return this.nowmode;
		}

		public void set_mode(int mode)
		{
			if (!this._FadeDone)
			{
				return;
			}
			this.nowmode = mode;
			UILabel component = GameObject.Find("debug/_damegeButton/lbl_setmode").GetComponent<UILabel>();
			component.text = string.Empty + this.nowmode;
			if (mode == 1)
			{
				this.bd1.set_kira(true);
			}
			else
			{
				this.bd1.set_kira(false);
			}
			if (mode == 2)
			{
				this.bd1m.GetComponent<Collider2D>().set_enabled(true);
				this.bd1m.GetComponent<UIPanel>().alpha = 0.3f;
				this.bd2m.GetComponent<Collider2D>().set_enabled(false);
				this.bd2m.GetComponent<UIPanel>().alpha = 0f;
				this.bd3m.GetComponent<Collider2D>().set_enabled(false);
				this.bd3m.GetComponent<UIPanel>().alpha = 0f;
			}
			else if (mode == 3)
			{
				this.bd1m.GetComponent<Collider2D>().set_enabled(true);
				this.bd1m.GetComponent<UIPanel>().alpha = 0.3f;
				this.bd2m.GetComponent<Collider2D>().set_enabled(true);
				this.bd2m.GetComponent<UIPanel>().alpha = 0.3f;
				this.bd3m.GetComponent<Collider2D>().set_enabled(false);
				this.bd3m.GetComponent<UIPanel>().alpha = 0f;
			}
			else if (mode == 4)
			{
				this.bd1m.GetComponent<Collider2D>().set_enabled(true);
				this.bd1m.GetComponent<UIPanel>().alpha = 0.3f;
				this.bd2m.GetComponent<Collider2D>().set_enabled(true);
				this.bd2m.GetComponent<UIPanel>().alpha = 0.3f;
				this.bd3m.GetComponent<Collider2D>().set_enabled(true);
				this.bd3m.GetComponent<UIPanel>().alpha = 0.3f;
			}
			else
			{
				this.bd1m.GetComponent<Collider2D>().set_enabled(false);
				this.bd1m.GetComponent<UIPanel>().alpha = 0f;
				this.bd2m.GetComponent<Collider2D>().set_enabled(false);
				this.bd2m.GetComponent<UIPanel>().alpha = 0f;
				this.bd3m.GetComponent<Collider2D>().set_enabled(false);
				this.bd3m.GetComponent<UIPanel>().alpha = 0f;
			}
			this.first_change_mode = true;
		}

		public bool first_change()
		{
			if (this.first_change_mode)
			{
				this.first_change_mode = false;
				return true;
			}
			return false;
		}

		private void set_bg_color(int timezone)
		{
			switch (timezone)
			{
			case 1:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(1f, 1f, 1f);
				break;
			case 2:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.94f, 0.75f, 0.56f);
				break;
			case 3:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0f, 0.13f, 0.38f);
				break;
			case 4:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.09f, 0.13f, 0.19f);
				break;
			case 5:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(0.63f, 0.78f, 1f);
				break;
			default:
				GameObject.Find("BG Panel/bg_landscape").GetComponent<UITexture>().color = new Color(1f, 1f, 1f);
				break;
			}
		}

		public void setmask(int no, bool value)
		{
			GameObject gameObject = GameObject.Find("Repair Root/board" + no + "_top_mask");
			if (value)
			{
				gameObject.GetComponent<Collider2D>().set_enabled(true);
				gameObject.GetComponent<Animation>().Play("bd" + no + "m_on");
			}
			else
			{
				gameObject.GetComponent<Collider2D>().set_enabled(false);
				gameObject.GetComponent<Animation>().Play("bd" + no + "m_off");
			}
		}

		public void all_rid_mask()
		{
			Animation component = this.bd1m.GetComponent<Animation>();
			this.bd1m.GetComponent<Collider2D>().set_enabled(false);
			component.Play("bd1m_off");
			component = this.bd2m.GetComponent<Animation>();
			this.bd2m.GetComponent<Collider2D>().set_enabled(false);
			component.Play("bd2m_off");
			component = this.bd3m.GetComponent<Animation>();
			this.bd3m.GetComponent<Collider2D>().set_enabled(false);
			component.Play("bd3m_off");
		}

		private void InitDock()
		{
			this.set_mode(1);
			this.all_rid_mask();
			this.dockSelectController = new KeyControl(0, this._clsRepair.MapArea.NDockCount - 1, 0.4f, 0.1f);
		}

		public int now_repairkit()
		{
			return this._clsRepair.Material.RepairKit;
		}

		[DebuggerHidden]
		private IEnumerator WaitAndSpeak(ShipModel ship, int VoiceNo, float WaitSec)
		{
			repair.<WaitAndSpeak>c__IteratorBD <WaitAndSpeak>c__IteratorBD = new repair.<WaitAndSpeak>c__IteratorBD();
			<WaitAndSpeak>c__IteratorBD.WaitSec = WaitSec;
			<WaitAndSpeak>c__IteratorBD.ship = ship;
			<WaitAndSpeak>c__IteratorBD.VoiceNo = VoiceNo;
			<WaitAndSpeak>c__IteratorBD.<$>WaitSec = WaitSec;
			<WaitAndSpeak>c__IteratorBD.<$>ship = ship;
			<WaitAndSpeak>c__IteratorBD.<$>VoiceNo = VoiceNo;
			return <WaitAndSpeak>c__IteratorBD;
		}

		public void nyukyogo(int dock, ShipModel ship, bool _isRepairKit)
		{
			Debug.Log(string.Concat(new object[]
			{
				"入渠します Dock:",
				dock,
				" MemId:",
				ship.MemId,
				" 高速:",
				_isRepairKit,
				" 耐久度率：",
				ship.TaikyuRate
			}));
			if (_isRepairKit)
			{
				base.StartCoroutine(this.WaitAndSpeak(ship, 26, 1.5f));
			}
			else if (ship.TaikyuRate >= 50.0)
			{
				base.StartCoroutine(this.WaitAndSpeak(ship, 11, 1.5f));
			}
			else
			{
				base.StartCoroutine(this.WaitAndSpeak(ship, 12, 1.5f));
			}
			this._clsRepair.StartRepair(dock, ship.MemId, _isRepairKit);
			GameObject gameObject = GameObject.Find("board1_top/board/Grid/0" + dock.ToString());
			if (_isRepairKit)
			{
				this.bd1.set_HS_anime(dock, true);
				GameObject.Find("board1_top/board").GetComponent<board>().set_rnow_enable(dock, true);
				GameObject.Find("board1_top/board").GetComponent<board>().set_stk_enable(dock, false);
				this.dg = GameObject.Find("dialog").GetComponent<dialog>();
				string arg_154_0 = "board/Grid/0";
				int num = dock;
				this.tex = GameObject.Find(arg_154_0 + num.ToString() + "/repair_now/ship_banner").GetComponent<UITexture>();
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
				string arg_201_0 = "board/Grid/0";
				int num2 = dock;
				this.lab = GameObject.Find(arg_201_0 + num2.ToString() + "/repair_now/text_ship_name").GetComponent<UILabel>();
				this.lab.text = ship.Name;
				string arg_23B_0 = "board/Grid/0";
				int num3 = dock;
				this.lab = GameObject.Find(arg_23B_0 + num3.ToString() + "/repair_now/text_level").GetComponent<UILabel>();
				this.lab.text = string.Empty + ship.Level;
				string arg_284_0 = "board/Grid/0";
				int num4 = dock;
				this.lab = GameObject.Find(arg_284_0 + num4.ToString() + "/repair_now/text_hp").GetComponent<UILabel>();
				this.lab.text = this.dg.GetBeforeHp() + "/" + ship.MaxHp;
				this.bd1.set_dock_MaxHP(dock, ship.MaxHp);
				string arg_2EF_0 = "board/Grid/0";
				int num5 = dock;
				this.sprite = GameObject.Find(arg_2EF_0 + num5.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter").GetComponent<UISprite>();
				this.sprite.width = (int)((float)this.dg.GetBeforeHp() * 210f / (float)ship.MaxHp);
				this.sprite.color = Util.HpGaugeColor2(ship.MaxHp, this.dg.GetBeforeHp());
				string arg_35F_0 = "board/Grid/0";
				int num6 = dock;
				this.sprite = GameObject.Find(arg_35F_0 + num6.ToString() + "/repair_now/HP_Gauge/panel/HP_bar_meter2").GetComponent<UISprite>();
				this.sprite.width = (int)((float)this.dg.GetBeforeHp() * 210f / (float)ship.MaxHp);
				this.sprite.color = Util.HpGaugeColor2(ship.MaxHp, this.dg.GetBeforeHp());
				string arg_3CF_0 = "board/Grid/0";
				int num7 = dock;
				this.lab = GameObject.Find(arg_3CF_0 + num7.ToString() + "/repair_now/text_least_time").GetComponent<UILabel>();
				this.lab.text = string.Empty + ship.RepairTime;
				string arg_417_0 = "board/Grid/0";
				int num8 = dock;
				GameObject.Find(arg_417_0 + num8.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>().isEnabled = false;
				crane_anime component = GameObject.Find("board/Grid/0" + dock.ToString() + "/Anime").GetComponent<crane_anime>();
				component.high_repair_anime(dock, false);
			}
			else
			{
				iTween.MoveTo(gameObject.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					1000f,
					"time",
					0.1f
				}));
			}
			this.bd2.UpdateList();
			this.update_portframe();
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this._clsRepair);
		}

		public void tochu_go(int dock, ShipModel shipid)
		{
			string arg_14_0 = "board/Grid/0";
			int num = dock;
			this._crane = GameObject.Find(arg_14_0 + num.ToString() + "/Anime").GetComponent<crane_anime>();
			this._crane.high_repair_anime(dock);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this._clsRepair);
			base.StartCoroutine(this.WaitAndSpeak(shipid, 26, 1.5f));
		}

		public void update_portframe()
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(this._clsRepair);
		}
	}
}
