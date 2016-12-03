using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog2 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private board3_top_mask bd3m;

		private board bd1;

		private board2 bd2;

		private RepairManager _clsRepair;

		private KeyControl dockSelectController;

		private board2 b2;

		private board3 b3;

		private CommonShipBanner csb;

		private ShipModel sm;

		private UIButton _Button_Yes2;

		private UIButton _Button_No2;

		private int shipid;

		private string _shipname;

		private int selected_dock;

		private bool _isBtnMaruUp;

		private GameObject[] btn_obj = new GameObject[2];

		private UIButton _uiOverlayButton2;

		private bool _dialog2_anime;

		private Animation _ani;

		private repair_clickmask _clickmask;

		private Vector3 _bVector = Vector3.get_one();

		public bool get_dialog2_anime()
		{
			return this._dialog2_anime;
		}

		public void set_dialog2_anime(bool value)
		{
			this._dialog2_anime = value;
		}

		public void CompleteHandler()
		{
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
			this.set_dialog2_anime(false);
		}

		public void CompleteHandler_onClose()
		{
			this.set_dialog2_anime(false);
			base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().alpha = 1f;
			this.Set_Button_Sprite(true);
			this.dockSelectController.Index = 0;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (value)
			{
				UIButton component = GameObject.Find("dialog2_top/dialog2/btn_yes").GetComponent<UIButton>();
				component.normalSprite = "btn_yes";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes";
				component = GameObject.Find("dialog2_top/dialog2/btn_no").GetComponent<UIButton>();
				component.normalSprite = "btn_no_on";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no_on";
			}
			else
			{
				UIButton component2 = GameObject.Find("dialog2_top/dialog2/btn_yes").GetComponent<UIButton>();
				component2.normalSprite = "btn_yes_on";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes_on";
				component2 = GameObject.Find("dialog2_top/dialog2/btn_no").GetComponent<UIButton>();
				component2.normalSprite = "btn_no";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no";
			}
		}

		public void SetShipName(string name)
		{
			this._shipname = name;
		}

		public int GetShipID()
		{
			return this.shipid;
		}

		public ShipModel GetShipModel()
		{
			return this.sm;
		}

		public void SetDock(int dock)
		{
			this.selected_dock = dock;
		}

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this.ele_l);
			Mem.Del<UITexture>(ref this.ele_t);
			Mem.Del<UIPanel>(ref this.panel);
			Mem.Del<repair>(ref this.rep);
			Mem.Del(ref this.ele_d);
			Mem.Del<board3_top_mask>(ref this.bd3m);
			Mem.Del<board>(ref this.bd1);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<board2>(ref this.b2);
			Mem.Del<board3>(ref this.b3);
			Mem.Del<CommonShipBanner>(ref this.csb);
			Mem.Del<ShipModel>(ref this.sm);
			Mem.Del<UIButton>(ref this._Button_Yes2);
			Mem.Del<UIButton>(ref this._Button_No2);
			Mem.Del<string>(ref this._shipname);
			Mem.Del<GameObject[]>(ref this.btn_obj);
			Mem.Del<UIButton>(ref this._uiOverlayButton2);
			Mem.Del<Animation>(ref this._ani);
			Mem.Del<repair_clickmask>(ref this._clickmask);
			Mem.Del<Vector3>(ref this._bVector);
		}

		public void _init_repair()
		{
			this._isBtnMaruUp = false;
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
			this.csb = GameObject.Find("dialog2_top/dialog2/Banner/CommonShipBanner2").GetComponent<CommonShipBanner>();
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.isLoopIndex = false;
			this.dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
			this._Button_Yes2 = GameObject.Find("dialog2_top/dialog2/btn_yes").GetComponent<UIButton>();
			this._Button_No2 = GameObject.Find("dialog2_top/dialog2/btn_no").GetComponent<UIButton>();
			this.btn_obj[0] = this._Button_No2.get_gameObject();
			this.btn_obj[1] = this._Button_Yes2.get_gameObject();
			this._uiOverlayButton2 = GameObject.Find("dialog2_top/dialog2/OverlayBtn2").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton2.onClick, new EventDelegate.Callback(this._onClickOverlayButton2));
			UIButtonMessage component = this._Button_Yes2.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "Pressed_Button_Yes2";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._Button_No2.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "Pressed_Button_No2";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			this.Set_Button_Sprite(true);
			this._clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
			this._bVector = Vector3.get_one();
		}

		private void Update()
		{
			if (this.rep.now_mode() != 5)
			{
				return;
			}
			this.dockSelectController.Update();
			if (this.dockSelectController.keyState.get_Item(1).up || !this.dockSelectController.keyState.get_Item(1).down)
			{
				this._isBtnMaruUp = true;
			}
			if (this.rep.first_change())
			{
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
				return;
			}
			if (this.dockSelectController.IsChangeIndex)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
			}
			if (this.dockSelectController.Index == 1)
			{
				this.Set_Button_Sprite(true);
			}
			else
			{
				this.Set_Button_Sprite(false);
			}
			if (base.get_gameObject().get_transform().get_localScale() != Vector3.get_one())
			{
				if (base.get_gameObject().get_transform().get_localScale() == this._bVector)
				{
					base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
					this.set_dialog2_anime(false);
				}
				this._bVector = base.get_gameObject().get_transform().get_localScale();
			}
			if (this.get_dialog2_anime())
			{
				return;
			}
			if (this.dockSelectController.keyState.get_Item(0).up)
			{
				this.dockSelectController.Index = 0;
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
				this.Pressed_Button_No2(null);
			}
			else if (this.dockSelectController.keyState.get_Item(1).down && this._isBtnMaruUp)
			{
				this._isBtnMaruUp = false;
				if (this.dockSelectController.Index == 1)
				{
					this.Pressed_Button_No2(null);
				}
				else
				{
					this.Pressed_Button_Yes2(null);
				}
			}
		}

		private void _onClickOverlayButton2()
		{
			if (this.get_dialog2_anime() || this.bd2.get_board2_anime())
			{
				return;
			}
			this.Pressed_Button_No2(null);
		}

		private void OnClick()
		{
		}

		public void UpdateInfo(int ret)
		{
			this._isBtnMaruUp = false;
			this._clsRepair = this.rep.now_clsRepair();
			this.sm = this._clsRepair.GetDockData(ret).GetShip();
			this.csb.SetShipData(this.sm);
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
			this.ele_l = GameObject.Find("dialog2_top/dialog2/label_shipname").GetComponent<UILabel>();
			this.ele_l.text = this.sm.Name;
			this.ele_l = GameObject.Find("dialog2_top/dialog2/label_lv").GetComponent<UILabel>();
			this.ele_l.text = string.Empty + this.sm.Level;
			GameObject.Find("dialog2/label_param_1").GetComponent<UILabel>().text = this._clsRepair.GetDockData(ret).RemainingTurns.ToString();
			GameObject.Find("dialog2/label_param_2").GetComponent<UILabel>().text = this.rep.now_repairkit().ToString();
			GameObject.Find("dialog2/label_param_3").GetComponent<UILabel>().text = (this.rep.now_repairkit() - 1).ToString();
			this.shipid = this.sm.MstId;
		}

		public void dialog2_appear(bool bstat)
		{
			this._isBtnMaruUp = false;
			this._ani = base.get_gameObject().get_transform().get_parent().GetComponent<Animation>();
			if (bstat)
			{
				this._clickmask.unclickable_sec(0.45f);
				base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().set_enabled(true);
				base.get_gameObject().MoveTo(new Vector3(0f, -39f, -2f), 0f, true);
				this.set_dialog2_anime(true);
				TweenScale tweenScale = TweenScale.Begin(base.get_gameObject(), 0.4f, Vector3.get_one());
				tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
				tweenScale.SetOnFinished(new EventDelegate.Callback(this.CompleteHandler));
				this.rep.setmask(3, true);
			}
			else
			{
				this.set_dialog2_anime(true);
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
				this._ani.Play("dialog2_off");
				this.rep.setmask(3, false);
			}
		}

		public void Pressed_Button_Yes2(GameObject obj)
		{
			this.rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this._clickmask.unclickable_onesec();
			this._isBtnMaruUp = false;
			this.Set_Button_Sprite(false);
			this.dockSelectController.Index = 0;
			this.shipid = this.GetShipID();
			this.dialog2_appear(false);
			this.b3 = GameObject.Find("board3").GetComponent<board3>();
			this.b3.board3_appear(false, true);
			this.b2 = GameObject.Find("board2").GetComponent<board2>();
			this.b2.board2_appear(false, true);
			this.rep.all_rid_mask();
			this.bd1.set_HS_anime(this.selected_dock, true);
			this.rep.now_clsRepair().ChangeRepairSpeed(this.selected_dock);
			this.rep.tochu_go(this.selected_dock, this.sm);
			this.rep.set_mode(1);
		}

		public void Pressed_Button_No2(GameObject obj)
		{
			this.rep.set_mode(-1);
			this._clickmask.unclickable_sec(0.3f);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this._isBtnMaruUp = false;
			this.Set_Button_Sprite(true);
			this.dialog2_appear(false);
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.setmask(3, false);
			this.rep.set_mode(1);
		}
	}
}
