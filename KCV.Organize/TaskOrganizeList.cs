using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeList : SceneTaskMono
	{
		[SerializeField]
		protected GameObject _shipListPanel;

		[SerializeField]
		protected UITexture _maskList;

		[SerializeField]
		protected Camera Camera;

		protected bool isControl;

		protected bool IsCreate;

		protected bool IsShip;

		protected int shipNumber;

		protected bool IsCreated;

		protected bool isEnd;

		protected string changeState;

		protected GameObject _mainObj;

		protected ShipModel[] AllShips;

		protected ShipModel ship;

		public bool isEnabled;

		public static KeyControl KeyController;

		public static OrganizeScrollListParent ListScroll;

		[NonSerialized]
		public bool isInit;

		protected bool isShowFrame;

		protected override bool Init()
		{
			if (!TaskOrganizeList.ListScroll.isOpen)
			{
				TaskOrganizeList.ListScroll.MovePosition(205, true, null);
				this.setList(true);
			}
			else
			{
				this.setList(false);
			}
			this.isInit = true;
			return true;
		}

		public void FirstInit()
		{
			if (!this.IsCreated)
			{
				this.IsCreate = false;
				this.IsShip = false;
				this.AllShips = OrganizeTaskManager.Instance.GetLogicManager().GetShipList();
				TaskOrganizeList.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
				TaskOrganizeList.KeyController.useDoubleIndex(0, 3);
				this._mainObj = OrganizeTaskManager.GetMainObject().get_transform().FindChild("OrganizeScrollListParent").get_gameObject();
				this._shipListPanel = this._mainObj.get_transform().FindChild("List/ListFrame/ShipListScroll").get_gameObject();
				this._maskList = this._mainObj.get_transform().FindChild("Panel/ListBackMask").GetComponent<UITexture>();
				UIButtonMessage component = this._maskList.GetComponent<UIButtonMessage>();
				component.target = base.get_gameObject();
				component.functionName = "BackListEL";
				component.trigger = UIButtonMessage.Trigger.OnClick;
				TaskOrganizeList.ListScroll = this._mainObj.get_transform().FindChild("List").GetComponent<OrganizeScrollListParent>();
				TaskOrganizeList.ListScroll.Initialize(OrganizeTaskManager.Instance.GetLogicManager(), this.Camera);
				TaskOrganizeList.ListScroll.HeadFocus();
				TaskOrganizeList.ListScroll.SetOnSelect(new OrganizeScrollListParent.OnSelectCallBack(this.ListSelectEL));
				TaskOrganizeList.ListScroll.SetOnCancel(new Action(this.ListCancelEL));
				this.IsCreated = true;
			}
		}

		protected override bool UnInit()
		{
			this.isInit = false;
			return true;
		}

		private void OnDestroy()
		{
			this._shipListPanel = null;
			this._maskList = null;
			this._mainObj = null;
			this.AllShips = null;
			this.ship = null;
			TaskOrganizeList.KeyController = null;
			TaskOrganizeList.ListScroll = null;
		}

		protected override bool Run()
		{
			this.isShowFrame = false;
			if (this.isEnd)
			{
				if (this.changeState == "listDetail")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.DetailList;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.ListDetail);
				}
				else if (this.changeState == "detail")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (this.changeState == "top")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.Top;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				this.isEnd = false;
				return false;
			}
			if (Input.get_touchCount() == 0)
			{
				TaskOrganizeList.KeyController.Update();
			}
			if (TaskOrganizeList.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public void setShipNumber(ShipModel ship)
		{
			this.IsShip = true;
			this.ship = ship;
		}

		public virtual void setList(bool isHeadFocus)
		{
			if (!this.IsCreate)
			{
				TaskOrganizeList.KeyController.firstUpdate = true;
				TaskOrganizeList.KeyController.ClearKeyAll();
				this.updateList(true);
				this.IsCreate = true;
			}
			else
			{
				this.updateList(isHeadFocus);
			}
			TaskOrganizeList.ListScroll.SetKeyController(TaskOrganizeList.KeyController);
			TaskOrganizeList.ListScroll.StartControl();
		}

		public void ListSelectEL(ShipModel model)
		{
			if (!OrganizeTaskManager.Instance.GetListDetailTask().isEnabled)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				OrganizeTaskManager.Instance.GetListTask().setChangePhase("listDetail");
				OrganizeTaskManager.Instance.GetListDetailTask().Show(model);
			}
		}

		public void ListCancelEL()
		{
			this.BackListEL(null);
		}

		protected virtual void updateList(bool isHeadFocus)
		{
			TaskOrganizeList.KeyController.KeyInputInterval = 0.05f;
		}

		public void Show(bool isShip)
		{
			if (this.isEnabled)
			{
				return;
			}
			Debug.Log("Show");
			TaskOrganizeList.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			TaskOrganizeList.ListScroll.SetKeyController(TaskOrganizeList.KeyController);
			this._maskList.get_transform().set_localPosition(Vector3.get_zero());
			this._maskList.SafeGetTweenAlpha(0f, 0.5f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
			this.isEnabled = true;
			this.isEnd = false;
			this.isShowFrame = true;
		}

		protected void OnAnimationComp()
		{
		}

		public void BackListEL(GameObject obj)
		{
			this.BackListEL(obj, false);
		}

		public void BackListEL(GameObject obj, bool isForce)
		{
			if (this.isEnabled && !this.isShowFrame && (this.isInit || isForce))
			{
				Debug.Log("BackListEL");
				this.isEnabled = false;
				OrganizeTaskManager.Instance.GetTopTask().isControl = false;
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				TaskOrganizeList.ListScroll.MovePosition(1060, false, new Action(this.compBackList));
				this._maskList.SafeGetTweenAlpha(0.5f, 0f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
				this.setChangePhase("top");
				TaskOrganizeList.KeyController.ClearKeyAll();
			}
		}

		private void compBackList()
		{
			OrganizeTaskManager.Instance.GetTopTask().isControl = true;
			TaskOrganizeList.ListScroll.get_transform().localPositionX(1060f);
			this._maskList.alpha = 0f;
		}

		public void setChangePhase(string state)
		{
			this.changeState = state;
			this.isEnd = true;
		}

		internal void UpdateList()
		{
			TaskOrganizeList.ListScroll.RefreshViews();
		}
	}
}
