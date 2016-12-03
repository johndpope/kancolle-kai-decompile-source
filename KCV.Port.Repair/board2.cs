using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board2 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIScrollBar sb;

		private UIGrid uig;

		private ShipModel _clsShipModel;

		private repair rep;

		private RepairManager _clsRepair;

		private board1_top_mask bd1m;

		private ShipModel[] ships;

		private GameObject line;

		private board bd;

		private board3 bd3;

		private UIScrollBar scb;

		private KeyControl dockSelectController;

		private int cy;

		private ResourceManager _clsResource;

		private KeyScrollControl _clsScroll;

		private UIScrollListRepair rep_p;

		private SoundManager sm;

		[SerializeField]
		private UIShipSortButton SortButton;

		private bool _startup;

		private UISprite sprite;

		private bool _board2_anime;

		private int damage_flag;

		private bool _debug_shipping;

		public void CompleteHandler()
		{
			this.set_board2_anime(false);
		}

		public void CompleteHandlerStaticOn()
		{
			base.get_gameObject().set_isStatic(true);
			base.get_gameObject().GetComponent<UIPanel>().set_enabled(false);
			this.set_board2_anime(false);
		}

		public bool get_board2_anime()
		{
			return this._board2_anime;
		}

		public void set_board2_anime(bool value)
		{
			this._board2_anime = value;
		}

		private void Start()
		{
			this.set_board2_anime(false);
		}

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this.ele_l);
			Mem.Del<UITexture>(ref this.ele_t);
			Mem.Del<UIScrollBar>(ref this.sb);
			Mem.Del<UIGrid>(ref this.uig);
			Mem.Del<ShipModel>(ref this._clsShipModel);
			Mem.Del<repair>(ref this.rep);
			Mem.Del<RepairManager>(ref this._clsRepair);
			Mem.Del<board1_top_mask>(ref this.bd1m);
			Mem.Del<ShipModel[]>(ref this.ships);
			Mem.Del<GameObject>(ref this.line);
			Mem.Del<board>(ref this.bd);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<UIScrollBar>(ref this.scb);
			Mem.Del<KeyControl>(ref this.dockSelectController);
			Mem.Del<KeyScrollControl>(ref this._clsScroll);
			Mem.Del<UIScrollListRepair>(ref this.rep_p);
			Mem.Del<UIShipSortButton>(ref this.SortButton);
			Mem.Del(ref this.sprite);
		}

		public void _init_repair()
		{
			this._startup = false;
			this.StartUp();
		}

		public void StartUp()
		{
			if (this._startup)
			{
				return;
			}
			this._startup = true;
			this.bd = GameObject.Find("board1_top/board").GetComponent<board>();
			this.bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
			this.rep = base.get_gameObject().get_transform().get_parent().get_parent().GetComponent<repair>();
			this.rep_p = this.rep.get_transform().FindChild("board2_top/board2/UIScrollListRepair").GetComponent<UIScrollListRepair>();
			Camera component = this.rep.get_transform().FindChild("Camera").GetComponent<Camera>();
			this._clsScroll = new KeyScrollControl(6, 6, this.scb);
			this._clsRepair = this.rep.now_clsRepair();
			this.damage_flag = 0;
			this._debug_shipping = false;
			TweenPosition.Begin(base.get_gameObject(), 0.01f, new Vector3(840f, 123f, -1f));
			this.ships = this._clsRepair.GetShipList();
			this.dockSelectController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.dockSelectController.setChangeValue(0f, 0f, 0f, 0f);
			this.rep_p.SetCamera(component);
			this.rep_p.SetKeyController(this.dockSelectController);
			this.rep_p.ResumeControl();
			this.rep_p.SetOnSelectedListener(delegate(UIScrollListRepairChild child)
			{
				this.rep_p.keyController.ClearKeyAll();
				this.rep_p.LockControl();
				this.rep.set_mode(-2);
				if (child.GetModel() == null)
				{
					return;
				}
				this.bd3.UpdateInfo(child.GetModel());
				this.bd3.board3_appear(true);
				this.rep.setmask(2, true);
				this.rep.set_mode(3);
			});
			this.redraw();
		}

		public void redraw()
		{
			this.redraw(false);
		}

		public void redraw(bool val)
		{
			this.rep.set_mode(2);
			this.UpdateList();
		}

		public void UpdateList()
		{
			this.rep_p.Initialize(this.ships);
		}

		private void Update()
		{
			if (this.rep == null)
			{
				return;
			}
			if (this.rep.now_mode() != 2)
			{
				return;
			}
			if (this.get_board2_anime())
			{
				return;
			}
			if (this.bd3.get_board3_anime())
			{
				return;
			}
			if (this.rep.first_change())
			{
				return;
			}
			this.dockSelectController.Update();
			if (this.dockSelectController.keyState.get_Item(0).down)
			{
				this.dockSelectController.ClearKeyAll();
				this.dockSelectController.firstUpdate = true;
				this.Cancelled();
			}
		}

		public void set_touch_mode(bool value)
		{
			if (value)
			{
				this.rep_p.StartControl();
			}
			else
			{
				this.rep_p.LockControl();
			}
		}

		public void Cancelled()
		{
			this.Cancelled(false, false);
		}

		public void Cancelled(bool NotChangeMode)
		{
			this.Cancelled(NotChangeMode, false);
		}

		public void Cancelled(bool NotChangeMode, bool isSirent)
		{
			if (this.rep.now_mode() == 2 || NotChangeMode)
			{
				this.rep.set_mode(-10);
				this.board2_appear(false, isSirent);
			}
			if (!NotChangeMode)
			{
				this.rep.set_mode(1);
			}
		}

		public void board2_appear(bool boardStart)
		{
			this.board2_appear(boardStart, false);
		}

		public void board2_appear(bool boardStart, bool isSirent)
		{
			if (boardStart)
			{
				base.get_gameObject().set_isStatic(false);
				base.get_gameObject().GetComponent<UIPanel>().set_enabled(true);
				this.SortButton.SetActive(true);
				this.rep_p.Initialize(this.ships);
				for (int i = 0; i < 4; i++)
				{
					if (this.bd.get_HS_anime(i))
					{
						this.rep.now_clsRepair().ChangeRepairSpeed(i);
						this.bd.set_anime(i, false);
						this.bd.set_HS_anime(i, false);
					}
				}
				this.set_board2_anime(true);
				TweenPosition tweenPosition = TweenPosition.Begin(base.get_gameObject(), 0.35f, new Vector3(162f, 123f, -1f));
				tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition.SetOnFinished(new EventDelegate.Callback(this.CompleteHandler));
				this.rep.set_mode(-2);
				this.rep.setmask(1, true);
				this.rep.set_mode(2);
			}
			else
			{
				this.set_board2_anime(true);
				TweenPosition tweenPosition2 = TweenPosition.Begin(base.get_gameObject(), 0.3f, new Vector3(840f, 123f, -1f));
				tweenPosition2.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition2.SetOnFinished(new EventDelegate.Callback(this.CompleteHandlerStaticOn));
				this.rep.setmask(1, false);
				this.rep.set_mode(1);
			}
			if (!isSirent)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void DBG_damage()
		{
			if (this._debug_shipping)
			{
				return;
			}
			this._debug_shipping = true;
			UserInfoModel userInfo = this.rep.now_clsRepair().UserInfo;
			List<ShipModel> all_ships = userInfo.__GetShipList__();
			List<ShipModel> list = this.rep.now_clsRepair().GetAreaShips(this.rep.NowArea(), all_ships);
			if (this.rep.NowArea() == 1)
			{
				list.AddRange(this.rep.now_clsRepair().GetDepotShips(all_ships));
			}
			list = DeckUtil.GetSortedList(list, SortKey.DAMAGE);
			using (List<ShipModel>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel current = enumerator.get_Current();
					int subvalue;
					switch (this.damage_flag)
					{
					case 0:
						subvalue = (int)Math.Ceiling((double)((float)current.MaxHp * 0.25f));
						break;
					case 1:
						subvalue = (int)Math.Ceiling((double)((float)current.MaxHp * 0.25f));
						break;
					case 2:
						subvalue = current.NowHp - 1;
						break;
					case 3:
						goto IL_F9;
					default:
						goto IL_F9;
					}
					IL_115:
					Debug_Mod.SubHp(current.MemId, subvalue);
					continue;
					IL_F9:
					subvalue = current.NowHp - current.MaxHp;
					this.damage_flag = -1;
					goto IL_115;
				}
			}
			this.damage_flag++;
			Mem.DelList<ShipModel>(ref all_ships);
			Mem.DelList<ShipModel>(ref list);
			CommonPopupDialog.Instance.StartPopup("DBG: 母港へ行きます。");
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			this._debug_shipping = false;
		}

		internal void Resume()
		{
			this.rep_p.keyController.ClearKeyAll();
			this.rep_p.keyController.firstUpdate = true;
			this.rep_p.ResumeControl();
		}
	}
}
