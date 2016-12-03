using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyInfoManager : MonoBehaviour
	{
		public enum Mode
		{
			AreaInfo,
			DeckInfo,
			EnumEnd
		}

		private const int DECK_MAXNUM = 8;

		private StrategyMapManager LogicMng;

		private StrategyInfoManager.Mode nowInfoMode;

		[SerializeField]
		private StrategySidePanel SidePanel;

		[SerializeField]
		private StrategyUpperInfo UpperInfo;

		[SerializeField]
		private StrategyBottomInfo FooterInfo;

		[SerializeField]
		private RotateMenu_Strategy2 RotateMenu;

		[SerializeField]
		private StrategyAreaName AreaName;

		public StrategyInfoManager.Mode NowInfoMode
		{
			get
			{
				return this.nowInfoMode;
			}
			set
			{
				this.nowInfoMode = value;
			}
		}

		public StrategyBottomInfo GetFooterInfo()
		{
			return this.FooterInfo;
		}

		public void init()
		{
			this.nowInfoMode = StrategyInfoManager.Mode.AreaInfo;
			this.FooterInfo.UpdateBottomPanel(this.nowInfoMode, true);
			this.FooterInfo.ChangeMode(this.nowInfoMode);
			this.UpperInfo.UpdateUpperInfo();
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = this.LogicMng.Area.get_Item(1).GetDecks()[0];
			}
		}

		public void updateInfoPanel(int areaID)
		{
			this.SidePanel.UpdateSidePanel(this.nowInfoMode);
			this.UpperInfo.UpdateUpperInfo();
		}

		public void nextInfoPanel()
		{
			this.nowInfoMode++;
			if (this.nowInfoMode >= StrategyInfoManager.Mode.EnumEnd)
			{
				this.nowInfoMode = StrategyInfoManager.Mode.AreaInfo;
			}
			this.SidePanel.ChangeMode(this.nowInfoMode);
			float delay = (this.nowInfoMode != StrategyInfoManager.Mode.AreaInfo) ? 0f : 0.3f;
			this.SidePanel.Enter(this.nowInfoMode, delay);
		}

		public void changeMode(StrategyInfoManager.Mode InfoMode)
		{
			this.nowInfoMode = InfoMode;
			this.SidePanel.ChangeMode(InfoMode);
		}

		public void SetSidePanelMode(StrategyInfoManager.Mode InfoMode)
		{
			this.nowInfoMode = InfoMode;
			this.SidePanel.SetMode(InfoMode);
		}

		public void EnterInfoPanel(float delay = 0f)
		{
			this.SidePanel.Enter(this.nowInfoMode, delay);
			this.SidePanel.EnterBG();
		}

		public void ExitInfoPanel()
		{
			this.SidePanel.Exit(this.nowInfoMode);
		}

		public void updateFooterInfo(bool isUpdateMaterial)
		{
			this.FooterInfo.UpdateBottomPanel(this.nowInfoMode, isUpdateMaterial);
		}

		public void nextFooterInfo()
		{
			this.FooterInfo.ChangeMode(this.nowInfoMode);
		}

		public void EnterFooterInfo()
		{
			this.FooterInfo.Enter(this.nowInfoMode);
		}

		public void ExitFooterInfo()
		{
			this.FooterInfo.Exit();
		}

		public void updateUpperInfo()
		{
			this.UpperInfo.UpdateUpperInfo();
		}

		public void changeAreaName(int areaID)
		{
			this.AreaName.setAreaName(areaID);
			this.AreaName.StartAnimation();
		}

		public void MoveScreenOut(Action Onfinished, bool isCharacterExit = true, bool isPanelOff = false)
		{
			this.SidePanel.Exit(this.nowInfoMode);
			this.SidePanel.ExitBG(isPanelOff);
			this.UpperInfo.Exit();
			this.FooterInfo.Exit();
			if (isCharacterExit)
			{
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(false, Onfinished);
			}
		}

		public void MoveScreenIn(Action Onfinished, bool isCharacterEnter = true, bool isSidePanelEnter = true)
		{
			if (isSidePanelEnter && !this.RotateMenu.isOpen)
			{
				this.SidePanel.Enter(this.nowInfoMode, 0f);
			}
			this.UpperInfo.Enter();
			this.FooterInfo.Enter(this.nowInfoMode);
			this.SidePanel.EnterBG();
			if (isCharacterEnter)
			{
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, Onfinished);
			}
			else if (Onfinished != null)
			{
				this.DelayAction(0.2f, Onfinished);
			}
		}

		public void changeCharacter(DeckModel deck)
		{
			StrategyTopTaskManager.Instance.UIModel.Character.ChangeCharacter(deck);
		}
	}
}
