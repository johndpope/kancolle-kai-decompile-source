using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySidePanel : MonoBehaviour
	{
		[SerializeField]
		private StrategySideAreaInfo sideAreaInfo;

		[SerializeField]
		private UITexture blackBG;

		private UIWidget AreaInfoWidget;

		private void Awake()
		{
			this.AreaInfoWidget = this.sideAreaInfo.GetComponent<UIWidget>();
			this.AreaInfoWidget.alpha = 0f;
		}

		public void Init(StrategyInfoManager.Mode nowMode)
		{
		}

		public void UpdateSidePanel(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode != StrategyInfoManager.Mode.DeckInfo)
				{
				}
			}
			else
			{
				this.sideAreaInfo.UpdateSideAreaPanel();
			}
		}

		public void ChangeMode(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode == StrategyInfoManager.Mode.DeckInfo)
				{
					this.sideAreaInfo.ExitAreaInfoPanel();
				}
			}
		}

		public void SetMode(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode == StrategyInfoManager.Mode.DeckInfo)
				{
					this.sideAreaInfo.setVisible(false);
				}
			}
		}

		public void Enter(StrategyInfoManager.Mode nowMode, float delay)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode != StrategyInfoManager.Mode.DeckInfo)
				{
				}
			}
			else
			{
				this.sideAreaInfo.EnterAreaInfoPanel(delay);
			}
		}

		public void Exit(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode != StrategyInfoManager.Mode.DeckInfo)
				{
				}
			}
			else
			{
				this.sideAreaInfo.ExitAreaInfoPanel();
			}
		}

		public void EnterBG()
		{
			if (!this.blackBG.get_enabled())
			{
				this.blackBG.set_enabled(true);
			}
			TweenAlpha.Begin(this.blackBG.get_gameObject(), 0.2f, 0.5f);
		}

		public void ExitBG(bool isPanelOff = false)
		{
			TweenAlpha.Begin(this.blackBG.get_gameObject(), 0.2f, 0f);
			if (isPanelOff)
			{
				this.blackBG.set_enabled(false);
			}
		}
	}
}
