using Common.Enum;
using Common.Struct;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog : MonoBehaviour
	{
		private UILabel ele_l;

		private UILabel ele_l2;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private int number;

		private bool _sw;

		private board3_top_mask bd3m;

		private RepairManager _clsRepair;

		private UITexture ele_t;

		private CommonShipBanner csb;

		private board3 bd3;

		private KeyControl dockSelectController;

		private int cx;

		private UIButton _Button_Yes;

		private UIButton _Button_No;

		private ShipModel ship;

		private bool _isSmoked;

		private bool _isKira;

		private GameObject csb_smokes;

		private GameObject csb_kira;

		private int selected_dock;

		private bool hsp;

		private int before_hp;

		private GameObject[] btn_obj = new GameObject[2];

		private UIButton _uiOverlayButton;

		private bool _dialog_anime;

		private Animation _ani;

		private sw sw;

		private repair_clickmask _clickmask;

		private Vector3 _bVector = Vector3.get_one();

		public void SetShip(ShipModel shi)
		{
			this.ship = shi;
			this.sw = GameObject.Find("sw01").GetComponent<sw>();
			this.sw.setSW(false);
		}

		public void SetDock(int dock)
		{
			this.selected_dock = dock;
		}

		public void SetSpeed(bool use)
		{
			this.hsp = use;
		}

		private void Start()
		{
			this._dialog_anime = false;
			this._init_repair();
			this.set_dialog_anime(false);
			this._clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
		}

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this.ele_l);
			Mem.Del<UILabel>(ref this.ele_l2);
			Mem.Del<UIPanel>(ref this.panel);
			Mem.Del<repair>(ref this.rep);
			Mem.Del(ref this.ele_d);
			Mem.Del<board3_top_mask>(ref this.bd3m);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<UITexture>(ref this.ele_t);
			Mem.Del<CommonShipBanner>(ref this.csb);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<UIButton>(ref this._Button_Yes);
			Mem.Del<UIButton>(ref this._Button_No);
			Mem.Del<ShipModel>(ref this.ship);
			Mem.Del<GameObject>(ref this.csb_smokes);
			Mem.Del<GameObject>(ref this.csb_kira);
			Mem.Del<GameObject[]>(ref this.btn_obj);
			Mem.Del<UIButton>(ref this._uiOverlayButton);
			Mem.Del<Animation>(ref this._ani);
			Mem.Del<sw>(ref this.sw);
			Mem.Del<repair_clickmask>(ref this._clickmask);
			Mem.Del<Vector3>(ref this._bVector);
		}

		public bool get_dialog_anime()
		{
			return this._dialog_anime;
		}

		public void set_dialog_anime(bool value)
		{
			this._dialog_anime = value;
		}

		public void CompleteHandler()
		{
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
			this.set_dialog_anime(false);
		}

		public void CompleteHandler_onClose()
		{
			this.Set_Button_Sprite(true);
			base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().alpha = 1f;
			this.set_dialog_anime(false);
			this.Set_Button_Sprite(true);
			this.dockSelectController.Index = 0;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (value)
			{
				UIButton component = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
				component.normalSprite = "btn_yes";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes";
				component = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
				component.normalSprite = "btn_no_on";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no_on";
			}
			else
			{
				UIButton component2 = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
				component2.normalSprite = "btn_yes_on";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes_on";
				component2 = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
				component2.normalSprite = "btn_no";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no";
			}
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.csb = GameObject.Find("dialog_top/dialog/Banner/CommonShipBanner2").GetComponent<CommonShipBanner>();
			this.bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
			this._Button_Yes = GameObject.Find("dialog_top/dialog/btn_yes").GetComponent<UIButton>();
			this._Button_No = GameObject.Find("dialog_top/dialog/btn_no").GetComponent<UIButton>();
			this.btn_obj[0] = this._Button_No.get_gameObject();
			this.btn_obj[1] = this._Button_Yes.get_gameObject();
			this._uiOverlayButton = GameObject.Find("dialog_top/dialog/OverlayBtn").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton.onClick, new EventDelegate.Callback(this._onClickOverlayButton));
			UIButtonMessage component = this._Button_Yes.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "Pressed_Button_Yes";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._Button_No.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "Pressed_Button_No";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			this._clsRepair = this.rep.now_clsRepair();
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.isLoopIndex = false;
			this.dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			this.sw = GameObject.Find("sw01").GetComponent<sw>();
			this.sw.setSW(false);
			this._bVector = Vector3.get_one();
		}

		private void Update()
		{
			if (this.rep.now_mode() != 4)
			{
				return;
			}
			this.dockSelectController.Update();
			if (this.rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
				return;
			}
			if (this.dockSelectController.Index == 1)
			{
				this.Set_Button_Sprite(true);
			}
			else
			{
				this.Set_Button_Sprite(false);
			}
			if (this.dockSelectController.IsChangeIndex)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
			}
			if (base.get_gameObject().get_transform().get_localScale() != Vector3.get_one())
			{
				if (base.get_gameObject().get_transform().get_localScale() == this._bVector)
				{
					base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
					this.set_dialog_anime(false);
				}
				this._bVector = base.get_gameObject().get_transform().get_localScale();
			}
			if (this.get_dialog_anime() || this.bd3.get_board3_anime())
			{
				return;
			}
			if (this.dockSelectController.keyState.get_Item(0).down)
			{
				this.dockSelectController.Index = 0;
				this.Pressed_Button_No(null);
			}
			else if (this.dockSelectController.keyState.get_Item(1).down)
			{
				if (this.dockSelectController.Index == 1)
				{
					this.Pressed_Button_No(null);
				}
				else
				{
					this.dockSelectController.Index = 0;
					this.rep.set_mode(1);
					this.Pressed_Button_Yes(null);
				}
			}
		}

		private void _onClickOverlayButton()
		{
			if (this.get_dialog_anime() || this.bd3.get_board3_anime())
			{
				return;
			}
			this.dockSelectController.Index = 0;
			this.Pressed_Button_No(null);
		}

		public void UpdateInfo(ShipModel value_ship)
		{
			this.ship = value_ship;
			this.csb.SetShipData(value_ship);
			if (base.get_gameObject().get_transform().get_localPosition().y < 100f)
			{
				return;
			}
			this.ele_l = GameObject.Find("dialog/label_shipname").GetComponent<UILabel>();
			this.ele_l.text = this.ship.Name;
			this.ele_l = GameObject.Find("dialog/label_lv").GetComponent<UILabel>();
			this.ele_l.text = this.ship.Level + string.Empty;
			if (this.ship.DamageStatus != DamageState.Normal)
			{
				this._isSmoked = true;
			}
			else
			{
				this._isSmoked = false;
				this.csb_smokes = null;
			}
			if (this.ship.ConditionState == FatigueState.Exaltation)
			{
				this._isKira = true;
			}
			else
			{
				this._isKira = false;
			}
			MaterialInfo resourcesForRepair = this.ship.GetResourcesForRepair();
			this.ele_l = GameObject.Find("dialog/label_param").GetComponent<UILabel>();
			this.ele_l.text = string.Concat(new object[]
			{
				resourcesForRepair.Steel,
				"\n",
				resourcesForRepair.Fuel,
				"\n"
			});
			UILabel expr_154 = this.ele_l;
			expr_154.text += ((!this._sw) ? (this.ship.RepairTime + string.Empty) : "0");
			UILabel expr_199 = this.ele_l;
			expr_199.text += "\u3000\n使用";
			if (this._sw)
			{
				UILabel expr_1BF = this.ele_l;
				expr_1BF.text += "する";
				this.before_hp = this.ship.NowHp;
			}
			else
			{
				UILabel expr_1F0 = this.ele_l;
				expr_1F0.text += "しない";
			}
		}

		public int GetBeforeHp()
		{
			return this.before_hp;
		}

		public void UpdateSW(bool sw)
		{
			this._sw = sw;
			this.UpdateInfo(this.ship);
		}

		private void OnClick()
		{
		}

		public void dialog_appear(bool bstat)
		{
			this.rep.set_mode(-4);
			this._ani = base.get_gameObject().get_transform().get_parent().GetComponent<Animation>();
			if (bstat)
			{
				this._clickmask.unclickable_sec(0.45f);
				base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().set_enabled(true);
				this.set_dialog_anime(true);
				this.csb.StopParticle();
				base.get_gameObject().get_transform().set_localPosition(new Vector3(0f, -27f, 0f));
				base.get_gameObject().get_transform().set_localScale(Vector3.get_one() * 0.6f);
				TweenScale tweenScale = TweenScale.Begin(base.get_gameObject(), 0.4f, Vector3.get_one());
				tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
				tweenScale.SetOnFinished(new EventDelegate.Callback(this.CompleteHandler));
				this.rep.setmask(3, true);
				this.rep.set_mode(4);
			}
			else
			{
				this.set_dialog_anime(true);
				this.csb.StartParticle();
				base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0f,
					"y",
					800,
					"z",
					-2,
					"delay",
					0.3f,
					"time",
					0f
				}));
				iTween.ScaleTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0.6f,
					"y",
					0.6f,
					"z",
					0.6f,
					"delay",
					0.3f,
					"time",
					0f,
					"easetype",
					iTween.EaseType.easeOutBack,
					"oncomplete",
					"CompleteHandler_onClose",
					"oncompletetarget",
					base.get_gameObject()
				}));
				this._ani.Play("dialog_off");
				this.rep.setmask(3, false);
				this.rep.set_mode(3);
			}
		}

		public void Pressed_Button_Yes(GameObject obj)
		{
			this.rep.set_mode(-1);
			this._clickmask.unclickable_onesec();
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter2);
			this.dialog_appear(false);
			GameObject.Find("board3").GetComponent<board3>().board3_appear(false, true);
			GameObject.Find("board2").GetComponent<board2>().board2_appear(false, true);
			this.rep.all_rid_mask();
			if (!this.hsp)
			{
				this.rep.nyukyogo(this.selected_dock, this.ship, false);
				GameObject.Find("board1_top/board").GetComponent<board>().redraw(false, this.selected_dock);
			}
			else
			{
				this.rep.nyukyogo(this.selected_dock, this.ship, true);
			}
			GameObject.Find("board2").GetComponent<board2>().UpdateList();
			GameObject.Find("board1_top/board").GetComponent<board>().set_cx(0);
			this.rep.set_mode(1);
		}

		public void Pressed_Button_No(GameObject obj)
		{
			Debug.Log("Pressed_Button_No");
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this._clickmask.unclickable_sec(0.3f);
			this.rep.set_mode(-1);
			this.rep.setmask(3, false);
			this.dialog_appear(false);
			this.rep.set_mode(3);
		}

		[DebuggerHidden]
		private IEnumerator _wait(float time)
		{
			dialog.<_wait>c__IteratorBC <_wait>c__IteratorBC = new dialog.<_wait>c__IteratorBC();
			<_wait>c__IteratorBC.time = time;
			<_wait>c__IteratorBC.<$>time = time;
			return <_wait>c__IteratorBC;
		}
	}
}
