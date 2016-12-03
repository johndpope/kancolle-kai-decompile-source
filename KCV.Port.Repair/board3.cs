using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board3 : MonoBehaviour
	{
		private UISprite ele_d;

		private UILabel ele_l;

		private UILabel ele_l2;

		private UITexture ele_t;

		private UITexture ele_t2;

		private UIScrollBar sb;

		private repair rep;

		private dialog dia;

		private sw sw_h;

		private board2_top_mask bd2m;

		private RepairManager _clsRepair;

		private UIButton uibutton;

		private dialog dig;

		private KeyControl dockSelectController;

		private board2 bd2;

		private board3 bd3;

		private sw sw;

		private int cy;

		private int mcy;

		private bool nyukyo;

		private GameObject cursor;

		private board3_btn bd3b;

		private CommonShipBanner csb;

		private bool bd3firstUpdate;

		private ShipModel _ShipModel;

		private int _sw;

		[SerializeField]
		private UILabel _uiShipName;

		[SerializeField]
		private ButtonLightTexture _btnLight;

		private GameObject[] btn_obj = new GameObject[2];

		private bool _board3_anime;

		public void CompleteHandler()
		{
			this.set_board3_anime(false);
		}

		public void CompleteHandlerStaticOn()
		{
			base.get_gameObject().set_isStatic(true);
			base.get_gameObject().GetComponent<UIPanel>().set_enabled(false);
			this.set_board3_anime(false);
			this.sw.setSW(false);
			this.cy = 1;
			this.dockSelectController.Index = 1;
		}

		public bool get_board3_anime()
		{
			return this._board3_anime;
		}

		public void set_board3_anime(bool value)
		{
			this._board3_anime = value;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (this.nyukyo)
			{
				this._btnLight.PlayAnim();
				if (value)
				{
					GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = Color.get_white();
					GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_off";
					GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_off";
				}
				else
				{
					GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.001f);
					GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_on";
					GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_on";
				}
			}
			else
			{
				this._btnLight.StopAnim();
				GameObject.Find("sw01/switch_cursor").GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 0.001f);
				GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>().normalSprite = "btn_start_disable";
				GameObject.Find("Repair Root/board3_top/board3/Button/Background").GetComponent<UISprite>().spriteName = "btn_start_disable";
				UISelectedObject.SelectedOneButtonZoomUpDown(this.btn_obj[0], false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this.btn_obj[1], false);
			}
		}

		private void Start()
		{
			this._init_repair();
			this.set_board3_anime(false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref this.ele_d);
			Mem.Del<UILabel>(ref this.ele_l);
			Mem.Del<UILabel>(ref this.ele_l2);
			Mem.Del<UITexture>(ref this.ele_t);
			Mem.Del<UITexture>(ref this.ele_t2);
			Mem.Del<UIScrollBar>(ref this.sb);
			Mem.Del<repair>(ref this.rep);
			Mem.Del<dialog>(ref this.dia);
			Mem.Del<sw>(ref this.sw_h);
			Mem.Del<board2_top_mask>(ref this.bd2m);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<UIButton>(ref this.uibutton);
			Mem.Del<dialog>(ref this.dig);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<sw>(ref this.sw);
			Mem.Del<GameObject>(ref this.cursor);
			Mem.Del<board3_btn>(ref this.bd3b);
			Mem.Del<CommonShipBanner>(ref this.csb);
			Mem.Del<ShipModel>(ref this._ShipModel);
			Mem.Del<UILabel>(ref this._uiShipName);
			Mem.Del<ButtonLightTexture>(ref this._btnLight);
			Mem.Del<GameObject[]>(ref this.btn_obj);
		}

		public void _init_repair()
		{
			this.rep = base.get_gameObject().get_transform().get_parent().get_parent().GetComponent<repair>();
			this.bd3b = this.rep.get_gameObject().get_transform().FindChild("board3_top/board3/Button").GetComponent<board3_btn>();
			this.bd3 = this.rep.get_gameObject().get_transform().FindChild("board3_top/board3").GetComponent<board3>();
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
			this.btn_obj[0] = GameObject.Find("switch_ball");
			this.btn_obj[1] = this.bd3b.get_gameObject();
			this.sw = GameObject.Find("sw01").GetComponent<sw>();
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			this.nyukyo = false;
			this.bd3firstUpdate = true;
			this.cy = 1;
			this._clsRepair = this.rep.now_clsRepair();
		}

		public void StartUp()
		{
			this.rep = base.get_gameObject().get_transform().get_parent().get_parent().GetComponent<repair>();
			this.bd3b = this.rep.get_gameObject().get_transform().FindChild("board3_top/board3/Button").GetComponent<board3_btn>();
			this.bd3 = this.rep.get_gameObject().get_transform().FindChild("board3_top/board3").GetComponent<board3>();
			this.dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			this.btn_obj[0] = GameObject.Find("switch_ball");
			this.btn_obj[1] = this.bd3b.get_gameObject();
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.setChangeValue(-1f, 0f, 1f, 0f);
			this.nyukyo = false;
			this._clsRepair = this.rep.now_clsRepair();
		}

		public void UpdateInfo(ShipModel shipz)
		{
			this._ShipModel = shipz;
			bool material = false;
			this.mcy = 0;
			this.dig = GameObject.Find("dialog_top/dialog/").GetComponent<dialog>();
			this.sw = GameObject.Find("board3/sw01").GetComponent<sw>();
			this.uibutton = GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>();
			bool flag;
			if (flag = this.rep.now_clsRepair().IsValidStartRepair(this._ShipModel.MemId))
			{
				this.uibutton.isEnabled = true;
				this.nyukyo = true;
				this.sw.set_sw_stat(true);
			}
			else
			{
				this.uibutton.isEnabled = false;
				this.nyukyo = false;
				this.sw.setSW(false);
				this.sw.set_sw_stat(false);
			}
			this.ele_l = GameObject.Find("board3/param/shipname").GetComponent<UILabel>();
			this.ele_l.text = this._ShipModel.Name;
			this.csb = GameObject.Find("board3/Banner/CommonShipBanner2").GetComponent<CommonShipBanner>();
			this.csb.SetShipData(this._ShipModel);
			this.ele_l = GameObject.Find("board3/param/Label_hp").GetComponent<UILabel>();
			this.ele_l.text = this._ShipModel.NowHp + "/" + this._ShipModel.MaxHp;
			this.ele_d = GameObject.Find("board3/param/HP_bar_grn").GetComponent<UISprite>();
			this.ele_d.width = (int)((float)this._ShipModel.NowHp * 210f / (float)this._ShipModel.MaxHp);
			this.ele_d.color = Util.HpGaugeColor2(this._ShipModel.MaxHp, this._ShipModel.NowHp);
			GameObject.Find("board3/param/icon_stars").GetComponent<UISprite>().SetDimensions((this._ShipModel.Srate + 1) * 25 - 2, 20);
			this.ele_l = GameObject.Find("board3/param/Label_lv").GetComponent<UILabel>();
			this.ele_l.text = string.Empty + this._ShipModel.Level;
			this.ele_l = GameObject.Find("board3/param/Label_param").GetComponent<UILabel>();
			this.sw = GameObject.Find("board3/sw01").GetComponent<sw>();
			string text = string.Empty;
			if (this._clsRepair.Material.Steel < this._ShipModel.GetResourcesForRepair().Steel)
			{
				text += "[e32c2c]";
				material = true;
			}
			else
			{
				text += "[404040]";
			}
			text = text + this._ShipModel.GetResourcesForRepair().Steel + "[-]\n";
			if (this._clsRepair.Material.Fuel < this._ShipModel.GetResourcesForRepair().Fuel)
			{
				text += "[e32c2c]";
				material = true;
			}
			else
			{
				text += "[404040]";
			}
			if (flag)
			{
				this.dia.UpdateInfo(this._ShipModel);
				this.dia.SetShip(this._ShipModel);
			}
			else
			{
				base.StartCoroutine(this.ReasonMessage(this._ShipModel, material));
			}
			this.dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			text = text + this._ShipModel.GetResourcesForRepair().Fuel + "[-]\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[404040]",
				this._ShipModel.RepairTime,
				"[-]  "
			});
			this.ele_l.text = text;
		}

		[DebuggerHidden]
		private IEnumerator ReasonMessage(ShipModel _ShipModel, bool material)
		{
			board3.<ReasonMessage>c__IteratorBA <ReasonMessage>c__IteratorBA = new board3.<ReasonMessage>c__IteratorBA();
			<ReasonMessage>c__IteratorBA._ShipModel = _ShipModel;
			<ReasonMessage>c__IteratorBA.material = material;
			<ReasonMessage>c__IteratorBA.<$>_ShipModel = _ShipModel;
			<ReasonMessage>c__IteratorBA.<$>material = material;
			<ReasonMessage>c__IteratorBA.<>f__this = this;
			return <ReasonMessage>c__IteratorBA;
		}

		private void Update()
		{
			if (this.rep.now_mode() != 3)
			{
				return;
			}
			if (this.dia.get_dialog_anime() || this.bd2.get_board2_anime())
			{
				return;
			}
			this.dockSelectController.Update();
			if (this.rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, 1);
				return;
			}
			if (this.mcy != this.cy || this.bd3firstUpdate)
			{
				if (this.mcy != this.cy && !this.bd3firstUpdate)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.bd3firstUpdate = false;
				if (this.nyukyo)
				{
					if (this.cy == 0)
					{
						this.Set_Button_Sprite(true);
					}
					else
					{
						this.Set_Button_Sprite(false);
					}
					UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.cy);
				}
				else
				{
					this.Set_Button_Sprite(true);
				}
			}
			this.mcy = this.cy;
			if (this.dockSelectController.keyState.get_Item(0).down)
			{
				this.bd3firstUpdate = true;
				this.dockSelectController.ClearKeyAll();
				this.dockSelectController.firstUpdate = true;
				this.Cancelled();
			}
			else if (this.dockSelectController.keyState.get_Item(2).down)
			{
				this.sw.OnClick();
			}
			else if (this.dockSelectController.IsLeftDown())
			{
				this.sw.setSW(false);
			}
			else if (this.dockSelectController.IsRightDown())
			{
				this.sw.setSW(true);
			}
			else if (this.dockSelectController.keyState.get_Item(1).down)
			{
				this.bd3firstUpdate = true;
				if (this.cy != 0)
				{
					this.uibutton = GameObject.Find("Repair Root/board3_top/board3/Button").GetComponent<UIButton>();
					if (!this.uibutton.isEnabled)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
					}
					if (this.nyukyo)
					{
						this.bd3b.OnClick();
					}
				}
				else if (this.nyukyo)
				{
					this.sw.OnClick();
				}
				else
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				}
			}
			if (!this.nyukyo)
			{
				return;
			}
			if (this.dockSelectController.keyState.get_Item(8).down)
			{
				this.cy = 0;
			}
			else if (this.dockSelectController.keyState.get_Item(12).down)
			{
				this.cy = 1;
			}
		}

		public void Cancelled()
		{
			this.Cancelled(false);
		}

		public void Cancelled(bool NotChangeMode)
		{
			this.Cancelled(NotChangeMode, false);
		}

		public void Cancelled(bool NotChangeMode, bool isSirent)
		{
			if (!NotChangeMode)
			{
				this.rep.set_mode(2);
			}
			this._board3_anime = true;
			this.board3_appear(false, isSirent);
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.setmask(2, false);
			this.bd2.Resume();
		}

		private void OnClick()
		{
		}

		public void board3_appear(bool bstat)
		{
			this.board3_appear(bstat, false);
		}

		public void board3_appear(bool bstat, bool isSirent)
		{
			if (bstat)
			{
				base.get_gameObject().set_isStatic(false);
				base.get_gameObject().GetComponent<UIPanel>().set_enabled(true);
				this.bd3firstUpdate = true;
				this.mcy = 1;
				this.cy = 1;
				this.set_board3_anime(true);
				TweenPosition tweenPosition = TweenPosition.Begin(base.get_gameObject(), 0.35f, new Vector3(261f, -51f, -2f));
				tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition.SetOnFinished(new EventDelegate.Callback(this.CompleteHandler));
				this.rep.setmask(2, true);
				this.rep.set_mode(3);
			}
			else
			{
				this.set_board3_anime(true);
				TweenPosition tweenPosition2 = TweenPosition.Begin(base.get_gameObject(), 0.3f, new Vector3(800f, -51f, -2f));
				tweenPosition2.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition2.SetOnFinished(new EventDelegate.Callback(this.CompleteHandlerStaticOn));
				this.rep.setmask(2, false);
				this.rep.set_mode(2);
			}
			if (!isSirent)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}
	}
}
