using KCV.Organize;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class TaskEscortOrganizeDetail : TaskOrganizeDetail
	{
		protected override bool Init()
		{
			TaskOrganizeDetail.KeyController = OrganizeTaskManager.GetKeyControl();
			this.detailManager.Open();
			this.isEnd = false;
			return true;
		}

		protected override bool Run()
		{
			if (this.isEnd)
			{
				if (this.changeState == "top")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				else if (this.changeState == "list")
				{
					EscortOrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				return false;
			}
			if (TaskOrganizeDetail.KeyController.IsLeftDown())
			{
				this.detailManager.buttons.UpdateButton(true, null);
			}
			else if (TaskOrganizeDetail.KeyController.IsRightDown())
			{
				this.detailManager.buttons.UpdateButton(false, null);
			}
			else if (TaskOrganizeDetail.KeyController.IsMaruDown())
			{
				this.detailManager.buttons.Decide();
			}
			else if (TaskOrganizeDetail.KeyController.IsBatuDown())
			{
				this.BackMaskEL(null);
			}
			else if (TaskOrganizeDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		public bool CheckBtnEnabled()
		{
			bool result = true;
			if (base.isEnabled || EscortOrganizeTaskManager._clsTop.isTenderAnimation())
			{
				result = false;
			}
			return result;
		}

		public void Show(ShipModel ship)
		{
			this.ship = ship;
			this.changeState = string.Empty;
			this.detailManager.SetDetailPanel(this.ship, true, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), EscortOrganizeTaskManager.GetEscortManager(), TaskOrganizeTop.BannerIndex - 1, null);
			this.detailManager.Open();
			this.isEnd = false;
		}

		private void BackMaskEL(GameObject obj)
		{
			if (!this.isEnd)
			{
				this.detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				this.changeState = "top";
				this.isEnd = true;
			}
		}

		public void SetBtnEL(GameObject obj)
		{
			if (!string.IsNullOrEmpty(this.changeState))
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this.changeState = "list";
			this.isEnd = true;
			EscortOrganizeTaskManager._clsList.setShipNumber(this.ship);
			EscortOrganizeTaskManager._clsList.Show(true);
			this.detailManager.Close();
		}

		public void ResetBtnEL(GameObject obj)
		{
			if (!string.IsNullOrEmpty(this.changeState))
			{
				return;
			}
			if (!this.isEnd)
			{
				this.detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				this.changeState = "top";
				this.isEnd = true;
				OrganizeTaskManager.Instance.GetLogicManager().UnsetOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), TaskOrganizeTop.BannerIndex - 1);
				EscortOrganizeTaskManager._clsTop.UpdateAllBannerByRemoveShip(false);
				EscortOrganizeTaskManager._clsTop.UpdateAllSelectBanner();
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
			}
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
