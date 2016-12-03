using Common.Enum;
using KCV.EscortOrganize;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeListDetail : SceneTaskMono
	{
		[SerializeField]
		protected OrganizeDetail_Manager detailManager;

		protected bool isCreate;

		protected bool isControl;

		protected bool isEnd;

		protected string changeState;

		protected ShipModel[] ships;

		public int index;

		public ShipModel ship;

		public static KeyControl KeyController;

		public bool isEnabled
		{
			get
			{
				return this.detailManager.isShow;
			}
		}

		protected override bool Init()
		{
			this.isEnd = false;
			this.detailManager.buttons.LockSwitch.setChangeListViewIcon(new Action(TaskOrganizeList.ListScroll.ChangeLockBtnState));
			return true;
		}

		public virtual void FirstInit()
		{
			if (!this.isCreate)
			{
				TaskOrganizeListDetail.KeyController = OrganizeTaskManager.GetKeyControl();
				this.detailManager.Init();
				this.index = 0;
				this.isControl = false;
				this.isEnd = false;
				this.isCreate = true;
			}
		}

		protected override bool Run()
		{
			if (this.isEnd)
			{
				if (this.changeState == "list")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
				}
				else if (this.changeState == "top")
				{
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
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
			if (TaskOrganizeListDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public virtual void Show(ShipModel ship)
		{
			if (this.isEnabled)
			{
				return;
			}
			this.ship = ship;
			this.index = 0;
			this.isControl = true;
			this.detailManager.SetDetailPanel(this.ship, false, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), OrganizeTaskManager.Instance.GetLogicManager(), TaskOrganizeTop.BannerIndex - 1, null);
			this.detailManager.Open();
			OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.DetailList;
		}

		public void ChangeButtonEL(GameObject obj)
		{
			if (!this.isEnd && this.isControl)
			{
				int bannerIndex = TaskOrganizeTop.BannerIndex;
				int memId = this.ship.MemId;
				bool flag = OrganizeTaskManager.Instance.GetLogicManager().ChangeOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), bannerIndex - 1, memId);
				OrganizeTaskManager.Instance.GetListTask().UpdateList();
				if (!flag)
				{
					Debug.Log("EROOR: ChangeOrganize");
					return;
				}
				DifficultKind difficulty = OrganizeTaskManager.logicManager.UserInfo.Difficulty;
				this.isControl = false;
				OrganizeTaskManager.Instance.GetTopTask().isControl = false;
				this.detailManager.Close();
				SoundUtils.PlaySE(SEFIleInfos.SE_003);
				OrganizeTaskManager.Instance.GetListTask().BackListEL(null, true);
				OrganizeTaskManager.Instance.GetTopTask().UpdateAllBannerByChangeShip();
				ShipUtils.PlayShipVoice(this.ship, 13);
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.setDisable();
				this.setChangePhase("top");
				if (OrganizeTaskManager.Instance.GetType() != typeof(EscortOrganizeTaskManager))
				{
					TutorialModel tutorial = OrganizeTaskManager.logicManager.UserInfo.Tutorial;
					if (tutorial.GetStep() == 3 && !tutorial.GetStepTutorialFlg(4))
					{
						tutorial.SetStepTutorialFlg(4);
						CommonPopupDialog.Instance.StartPopup("「艦隊を編成！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
			}
		}

		public void BackDataEL(GameObject obj)
		{
			if (!this.isEnd && this.isControl)
			{
				this.isEnd = true;
				this.detailManager.Close();
				this.setChangePhase("list");
				OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
				TaskOrganizeList.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
				TaskOrganizeList.KeyController.useDoubleIndex(0, 3);
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
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

		private void OnDestroy()
		{
			this.detailManager = null;
			this.ships = null;
			this.ship = null;
			TaskOrganizeListDetail.KeyController = null;
		}
	}
}
