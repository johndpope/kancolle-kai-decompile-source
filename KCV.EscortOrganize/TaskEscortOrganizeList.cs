using KCV.Organize;
using System;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeList : TaskOrganizeList
	{
		protected override void Start()
		{
			this.IsCreate = false;
			this.IsShip = false;
			TaskOrganizeList.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			TaskOrganizeList.KeyController.useDoubleIndex(0, 3);
			this._mainObj = OrganizeTaskManager.GetMainObject().get_transform().FindChild("OrganizeScrollListParent").get_gameObject();
			this._shipListPanel = this._mainObj.get_transform().FindChild("List/ListFrame/ShipListScroll").get_gameObject();
			this._maskList = this._mainObj.get_transform().FindChild("Panel/ListBackMask").GetComponent<UITexture>();
			UIButtonMessage component = this._maskList.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "BackListEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

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

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			this.isShowFrame = false;
			if (this.isEnd)
			{
				if (this.changeState == "listDetail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.ListDetail);
				}
				else if (this.changeState == "detail")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Detail);
				}
				else if (this.changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				this.isEnd = false;
				Debug.Log("deff");
				return false;
			}
			TaskOrganizeList.KeyController.Update();
			if (!TaskOrganizeList.KeyController.IsShikakuDown())
			{
				if (TaskOrganizeList.KeyController.IsBatuDown())
				{
					this.changeState = "top";
					this.isEnd = true;
					base.BackListEL(null);
				}
				else if (TaskOrganizeList.KeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
				}
			}
			return true;
		}

		public bool isListOpen()
		{
			return TaskOrganizeList.ListScroll.isOpen;
		}
	}
}
