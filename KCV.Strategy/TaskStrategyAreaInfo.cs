using Common.Enum;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyAreaInfo : SceneTaskMono
	{
		private KeyControl CommandKeyController;

		private KeyControl keyController;

		private bool isEnter;

		private Vector3 defaultPos;

		public GameObject AreaInfo;

		public AsyncObjects asyncObj;

		private Action ExitAction;

		protected override void Start()
		{
			this.AreaInfo.SetActive(false);
			this.AreaInfo.GetComponent<UIWidget>().alpha = 0f;
		}

		protected override bool Init()
		{
			this.isEnter = false;
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState == MissionStates.NONE)
			{
				UIShipCharacter character = StrategyTopTaskManager.Instance.UIModel.Character;
				character.moveCharacterX(StrategyTopTaskManager.Instance.UIModel.Character.getModelDefaultPosX() - 600f, 0.4f, delegate
				{
					this.afterInit();
				});
			}
			else
			{
				this.afterInit();
			}
			return true;
		}

		private void afterInit()
		{
			this.keyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.AreaInfo.SetActive(true);
			this.AreaInfo.GetComponent<UIWidget>().alpha = 0f;
			this.DelayActionFrame(3, delegate
			{
				TweenAlpha.Begin(this.AreaInfo, 0.3f, 1f);
			});
			this.isEnter = true;
		}

		protected override bool Run()
		{
			if (!this.isEnter)
			{
				return true;
			}
			this.keyController.Update();
			return this.KeyAction();
		}

		protected override bool UnInit()
		{
			this.AreaInfo.GetComponent<UIWidget>().alpha = 0f;
			return true;
		}

		private bool KeyAction()
		{
			if (this.keyController.keyState.get_Item(0).down)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this.backToCommandMenu();
				return false;
			}
			if (this.keyController.keyState.get_Item(5).down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		private void backToCommandMenu()
		{
			this.AreaInfo.SetActive(false);
			this.ExitAction.Invoke();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
		}

		public void OnBackTouch()
		{
			this.backToCommandMenu();
			base.Close();
		}

		public void setExitAction(Action act)
		{
			this.ExitAction = act;
		}
	}
}
