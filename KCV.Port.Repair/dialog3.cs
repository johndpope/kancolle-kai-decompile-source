using KCV.Utils;
using local.managers;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog3 : MonoBehaviour
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

		private UIButton _Button_Yes3;

		private UIButton _Button_No3;

		private int selected_dock;

		private bool _isBtnMaruUp;

		private GameObject[] btn_obj = new GameObject[2];

		private UIButton _uiOverlayButton3;

		private Animation _ani;

		private repair_clickmask _clickmask;

		private Vector3 _bVector = Vector3.get_one();

		private bool _dialog3_anime;

		public bool get_dialog3_anime()
		{
			return this._dialog3_anime;
		}

		public void set_dialog3_anime(bool value)
		{
			this._dialog3_anime = value;
		}

		public void CompleteHandler()
		{
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
			this.set_dialog3_anime(false);
		}

		public void CompleteHandler_onClose()
		{
			this.set_dialog3_anime(false);
			base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().alpha = 1f;
			this.Set_Button_Sprite(true);
			this.dockSelectController.Index = 0;
		}

		public void Set_Button_Sprite(bool value)
		{
			if (value)
			{
				UIButton component = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
				component.normalSprite = "btn_yes";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes";
				component = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
				component.normalSprite = "btn_no_on";
				component.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no_on";
			}
			else
			{
				UIButton component2 = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
				component2.normalSprite = "btn_yes_on";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_yes_on";
				component2 = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
				component2.normalSprite = "btn_no";
				component2.get_transform().FindChild("Background").GetComponent<UISprite>().spriteName = "btn_no";
			}
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
			Mem.Del<UIButton>(ref this._Button_Yes3);
			Mem.Del<UIButton>(ref this._Button_No3);
			Mem.Del<GameObject[]>(ref this.btn_obj);
			Mem.Del<UIButton>(ref this._uiOverlayButton3);
			Mem.Del<Animation>(ref this._ani);
			Mem.Del<repair_clickmask>(ref this._clickmask);
			Mem.Del<Vector3>(ref this._bVector);
		}

		public void _init_repair()
		{
			this._isBtnMaruUp = false;
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd1 = GameObject.Find("board").GetComponent<board>();
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.isLoopIndex = false;
			this.dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
			this._Button_Yes3 = GameObject.Find("dialog3_top/dialog3/btn_yes").GetComponent<UIButton>();
			this._Button_No3 = GameObject.Find("dialog3_top/dialog3/btn_no").GetComponent<UIButton>();
			this.btn_obj[0] = this._Button_No3.get_gameObject();
			this.btn_obj[1] = this._Button_Yes3.get_gameObject();
			this._uiOverlayButton3 = GameObject.Find("dialog3_top/dialog3/OverlayBtn3").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton3.onClick, new EventDelegate.Callback(this._onClickOverlayButton3));
			UIButtonMessage component = this._Button_Yes3.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "Pressed_Button_Yes3";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._Button_No3.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "Pressed_Button_No3";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			this.Set_Button_Sprite(true);
			this._clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
			this._bVector = Vector3.get_one();
		}

		private void Update()
		{
			if (this.rep.now_mode() != 6)
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
					this.set_dialog3_anime(false);
				}
				this._bVector = base.get_gameObject().get_transform().get_localScale();
			}
			if (this.get_dialog3_anime())
			{
				return;
			}
			if (this.dockSelectController.keyState.get_Item(0).up)
			{
				this.dockSelectController.Index = 0;
				UISelectedObject.SelectedButtonsZoomUpDown(this.btn_obj, this.dockSelectController.Index);
				this.Pressed_Button_No3(null);
			}
			else if (this.dockSelectController.keyState.get_Item(1).down && this._isBtnMaruUp)
			{
				this._isBtnMaruUp = false;
				if (this.dockSelectController.Index == 1)
				{
					this.Pressed_Button_No3(null);
				}
				else
				{
					this.Pressed_Button_Yes3(null);
				}
			}
		}

		private void _onClickOverlayButton3()
		{
			this.dockSelectController.Index = 0;
			if (this.get_dialog3_anime() || this.bd2.get_board2_anime())
			{
				return;
			}
			this.Pressed_Button_No3(null);
		}

		private void OnClick()
		{
		}

		public void UpdateInfo(int dockNo)
		{
			this.selected_dock = dockNo;
			this._isBtnMaruUp = false;
			this._clsRepair = this.rep.now_clsRepair();
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
			int numOfKeyPossessions = this._clsRepair.NumOfKeyPossessions;
			GameObject.Find("dialog3_top/dialog3/Text_b").GetComponent<UILabel>().text = numOfKeyPossessions.ToString();
			GameObject.Find("dialog3_top/dialog3/Text_a").GetComponent<UILabel>().text = (numOfKeyPossessions - 1).ToString();
		}

		public void dialog3_appear(bool bstat)
		{
			this._isBtnMaruUp = false;
			this._ani = base.get_gameObject().get_transform().get_parent().GetComponent<Animation>();
			if (bstat)
			{
				this._clickmask.unclickable_sec(0.5f);
				base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().set_enabled(true);
				base.get_gameObject().MoveTo(new Vector3(0f, 0f, -2f), 0f, true);
				this.set_dialog3_anime(true);
				TweenScale tweenScale = TweenScale.Begin(base.get_gameObject(), 0.4f, Vector3.get_one());
				tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
				tweenScale.SetOnFinished(new EventDelegate.Callback(this.CompleteHandler));
				this.rep.setmask(3, true);
			}
			else
			{
				this.set_dialog3_anime(true);
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0f,
					"y",
					2200,
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
				this._ani.Play("dialog3_off");
				this.rep.setmask(3, false);
			}
		}

		public void Pressed_Button_Yes3(GameObject obj)
		{
			this.rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this._clickmask.unclickable_onesec();
			this._isBtnMaruUp = false;
			this.Set_Button_Sprite(false);
			this.dockSelectController.Index = 0;
			this.dialog3_appear(false);
			this.b3 = GameObject.Find("board3").GetComponent<board3>();
			this.b3.board3_appear(false, true);
			this.b2 = GameObject.Find("board2").GetComponent<board2>();
			this.b2.board2_appear(false, true);
			this.rep.all_rid_mask();
			this.bd1.OpenDock(this.selected_dock);
			this.rep.set_mode(1);
			TrophyUtil.Unlock_At_DockOpen();
		}

		public void Pressed_Button_No3(GameObject obj)
		{
			this.rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this._clickmask.unclickable_sec(0.3f);
			this._isBtnMaruUp = false;
			this.Set_Button_Sprite(true);
			this.dialog3_appear(false);
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.setmask(3, false);
			this.rep.set_mode(1);
		}
	}
}
