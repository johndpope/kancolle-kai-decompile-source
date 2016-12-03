using KCV.Organize;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeListDetail : TaskOrganizeListDetail
	{
		protected override void Start()
		{
			TaskOrganizeListDetail.KeyController = OrganizeTaskManager.GetKeyControl();
		}

		protected override bool Init()
		{
			this.isEnd = false;
			this.detailManager.buttons.LockSwitch.setChangeListViewIcon(new Action(TaskOrganizeList.ListScroll.ChangeLockBtnState));
			return true;
		}

		protected override bool Run()
		{
			if (this.isEnd)
			{
				if (this.changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				else if (this.changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				return false;
			}
			if (TaskOrganizeListDetail.KeyController.IsLeftDown())
			{
				if (!this.ship.IsLocked())
				{
					this.detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsRightDown())
			{
				if (this.ship.IsLocked())
				{
					this.detailManager.buttons.LockSwitch.MoveLockBtn();
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsShikakuDown())
			{
				this.detailManager.buttons.LockSwitch.MoveLockBtn();
			}
			else if (TaskOrganizeListDetail.KeyController.IsMaruDown())
			{
				if (!this.isEnd)
				{
					this.ChangeButtonEL(null);
				}
			}
			else if (TaskOrganizeListDetail.KeyController.IsBatuDown())
			{
				this.BackDataEL(null);
			}
			else if (TaskOrganizeListDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public override void Show(ShipModel ship)
		{
			this.ship = ship;
			this.index = 0;
			this.isControl = true;
			this.detailManager.SetDetailPanel(ship, false, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), EscortOrganizeTaskManager.GetEscortManager(), TaskOrganizeTop.BannerIndex - 1, null);
			this.detailManager.Open();
		}

		public void ChangeButtonEL(GameObject obj)
		{
			if (!this.isEnd)
			{
				int bannerIndex = TaskOrganizeTop.BannerIndex;
				int memId = this.ship.MemId;
				List<int> list = new List<int>();
				list.Add(memId);
				List<int> list2 = list;
				ShipModel[] ships = OrganizeTaskManager.Instance.GetTopTask().currentDeck.GetShips();
				if (bannerIndex - 1 < ships.Length)
				{
					list2.Add(ships[bannerIndex - 1].MemId);
				}
				if (!EscortOrganizeTaskManager.GetEscortManager().ChangeOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), bannerIndex - 1, memId))
				{
					Debug.Log("EROOR: ChangeOrganize");
					return;
				}
				this.detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.SE_003);
				EscortOrganizeTaskManager._clsList.BackListEL(null, true);
				EscortOrganizeTaskManager._clsTop.UpdateAllBannerByChangeShip();
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
				ShipUtils.PlayShipVoice(this.ship, 13);
				this.setChangePhase("top");
			}
		}

		public void BackDataEL(GameObject obj)
		{
			if (!this.isEnd)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				this.detailManager.Close();
				this.setChangePhase("list");
				TaskOrganizeList.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
		}

		public void setChangePhase(string state)
		{
			this.changeState = state;
			this.isEnd = true;
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
