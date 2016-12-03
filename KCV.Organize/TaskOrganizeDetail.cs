using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class TaskOrganizeDetail : SceneTaskMono
	{
		[SerializeField]
		protected OrganizeDetail_Manager detailManager;

		protected bool isControl;

		protected bool isCreate;

		protected bool isEnd;

		protected string changeState;

		protected ShipModel ship;

		public int bannerIndex;

		public static KeyControl KeyController;

		public UiStarManager StarManager;

		public bool isEnabled
		{
			get
			{
				return this.detailManager.isShow;
			}
		}

		protected override bool Init()
		{
			this.detailManager.Init();
			this.detailManager.Open();
			this.isEnd = false;
			return true;
		}

		public void FirstInit()
		{
			if (!this.isCreate)
			{
				TaskOrganizeDetail.KeyController = OrganizeTaskManager.GetKeyControl();
				this.isCreate = true;
			}
		}

		protected override bool UnInit()
		{
			return true;
		}

		private void OnDestroy()
		{
			this.StarManager = null;
			this.ship = null;
			this.detailManager = null;
			TaskOrganizeDetail.KeyController = null;
		}

		protected override bool Run()
		{
			if (this.isEnd)
			{
				if (this.changeState == "top")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.Top;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.Phase_ST);
				}
				else if (this.changeState == "list")
				{
					OrganizeTaskManager.Instance.GetTopTask()._state2 = TaskOrganizeTop.OrganizeState.List;
					OrganizeTaskManager.ReqPhase(OrganizeTaskManager.OrganizePhase.List);
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
			if (TaskOrganizeDetail.KeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			return true;
		}

		public bool CheckBtnEnabled()
		{
			return !this.isEnabled && !OrganizeTaskManager.Instance.GetTopTask().isTenderAnimation();
		}

		public void Show(ShipModel ship)
		{
			if (this.isEnabled)
			{
				return;
			}
			this.ship = ship;
			this.changeState = string.Empty;
			this.detailManager.SetDetailPanel(this.ship, true, OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), OrganizeTaskManager.Instance.GetLogicManager(), TaskOrganizeTop.BannerIndex - 1, null);
			this.detailManager.Open();
			this.isEnd = false;
			this.isControl = true;
		}

		protected void BackMaskEL(GameObject obj)
		{
			if (!this.isEnd)
			{
				this.isControl = false;
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
			if (this.isEnd || !this.isControl)
			{
				return;
			}
			this.isControl = false;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this.changeState = "list";
			this.isEnd = true;
			OrganizeTaskManager.Instance.GetListTask().setShipNumber(this.ship);
			OrganizeTaskManager.Instance.GetListTask().Show(true);
			this.detailManager.Close();
		}

		public void ResetBtnEL(GameObject obj)
		{
			if (!string.IsNullOrEmpty(this.changeState))
			{
				return;
			}
			if (this.isEnd || !this.isControl)
			{
				return;
			}
			TutorialModel tutorial = OrganizeTaskManager.logicManager.UserInfo.Tutorial;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID != 1 && SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(tutorial, TutorialGuideManager.TutorialID.Bring))
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.Bring, null);
				return;
			}
			this.isEnd = true;
			this.isControl = false;
			this.detailManager.Close();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this.changeState = "top";
			OrganizeTaskManager.Instance.GetLogicManager().UnsetOrganize(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), TaskOrganizeTop.BannerIndex - 1);
			OrganizeTaskManager.Instance.GetTopTask().UpdateAllBannerByRemoveShip(false);
			OrganizeTaskManager.Instance.GetTopTask().UpdateAllSelectBanner();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.setDisable();
			OrganizeTaskManager.Instance.GetListTask().UpdateList();
		}
	}
}
