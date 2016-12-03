using KCV.Utils;
using local.managers;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class dialog4 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIPanel panel;

		private repair rep;

		private UISprite ele_d;

		private board3_top_mask bd3m;

		private board bd1;

		private board2 bd2;

		private board2 b2;

		private board3 b3;

		private KeyControl dockSelectController;

		private repair_clickmask _clickmask;

		private bool _isBtnMaruUp;

		private RepairManager _clsRepair;

		private UIButton _uiOverlayButton4;

		private Animation _ani;

		private bool _dialog4_anime;

		private Vector3 _bVector = Vector3.get_one();

		public bool get_dialog4_anime()
		{
			return this._dialog4_anime;
		}

		public void set_dialog4_anime(bool value)
		{
			this._dialog4_anime = value;
		}

		public void CompleteHandler()
		{
			base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
			this.set_dialog4_anime(false);
		}

		public void CompleteHandler_onClose()
		{
			this.set_dialog4_anime(false);
			base.get_gameObject().get_transform().get_parent().GetComponent<UIPanel>().alpha = 1f;
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
			Mem.Del<board2>(ref this.b2);
			Mem.Del<board3>(ref this.b3);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<repair_clickmask>(ref this._clickmask);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<UIButton>(ref this._uiOverlayButton4);
			Mem.Del<Animation>(ref this._ani);
			Mem.Del<Vector3>(ref this._bVector);
		}

		public void _init_repair()
		{
			this._isBtnMaruUp = false;
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
			this.dockSelectController = new KeyControl(0, 1, 0.4f, 0.1f);
			this.dockSelectController.setChangeValue(0f, 1f, 0f, -1f);
			this._uiOverlayButton4 = GameObject.Find("dialog4_top/dialog4/OverlayBtn4").GetComponent<UIButton>();
			EventDelegate.Add(this._uiOverlayButton4.onClick, new EventDelegate.Callback(this._onClickOverlayButton4));
			this._clickmask = GameObject.Find("click_mask").GetComponent<repair_clickmask>();
			this._bVector = Vector3.get_one();
		}

		private void Update()
		{
			if (this.rep.now_mode() != 7)
			{
				return;
			}
			this.dockSelectController.Update();
			if (this.dockSelectController.keyState.get_Item(1).up || !this.dockSelectController.keyState.get_Item(1).down)
			{
				this._isBtnMaruUp = true;
			}
			if (base.get_gameObject().get_transform().get_localScale() != Vector3.get_one())
			{
				if (base.get_gameObject().get_transform().get_localScale() == this._bVector)
				{
					base.get_gameObject().get_transform().set_localScale(Vector3.get_one());
					this.set_dialog4_anime(false);
				}
				this._bVector = base.get_gameObject().get_transform().get_localScale();
			}
			if (this.get_dialog4_anime())
			{
				return;
			}
			if (this.dockSelectController.keyState.get_Item(0).up || (this.dockSelectController.keyState.get_Item(1).down && this._isBtnMaruUp))
			{
				this.dockSelectController.Index = 0;
				this.Pressed_Button_No4(null);
			}
		}

		private void _onClickOverlayButton4()
		{
			if (this.get_dialog4_anime() || this.bd2.get_board2_anime())
			{
				return;
			}
			this.Pressed_Button_No4(null);
		}

		private void OnClick()
		{
		}

		public void UpdateInfo()
		{
			this._clsRepair = this.rep.now_clsRepair();
			iTween.ScaleTo(base.get_gameObject(), new Vector3(0.6f, 0.6f, 0.6f), 0f);
		}

		public void dialog4_appear(bool bstat)
		{
			this._isBtnMaruUp = false;
			this._ani = base.get_gameObject().get_transform().get_parent().GetComponent<Animation>();
			if (bstat)
			{
				this._clickmask.unclickable_sec(0.3f);
				GameObject.Find("dialog4_top").GetComponent<UIPanel>().set_enabled(true);
				base.get_gameObject().MoveTo(new Vector3(0f, -39f, -2f), 0f, true);
				this.set_dialog4_anime(true);
				iTween.ScaleTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					1f,
					"y",
					1f,
					"z",
					1f,
					"time",
					0.5f,
					"easetype",
					iTween.EaseType.easeOutBack,
					"oncomplete",
					"CompleteHandler",
					"oncompletetarget",
					base.get_gameObject()
				}));
				this.rep.setmask(3, true);
			}
			else
			{
				this.set_dialog4_anime(true);
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					0f,
					"y",
					-1100,
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
				this._ani.Play("dialog4_off");
				this.rep.setmask(3, false);
			}
		}

		public void Pressed_Button_No4(GameObject obj)
		{
			this.rep.set_mode(-1);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this._isBtnMaruUp = false;
			this.dialog4_appear(false);
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.setmask(3, false);
			this.rep.set_mode(1);
		}
	}
}
