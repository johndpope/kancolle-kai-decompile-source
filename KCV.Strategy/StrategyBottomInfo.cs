using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyBottomInfo : MonoBehaviour
	{
		[Serializable]
		public class DeckInfo
		{
			public UILabel DeckName;

			public GameObject ParentObject;

			public void UpdateDeckInfo(bool isShort = false)
			{
				DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
				ShipModel shipModel = (currentDeck == null) ? null : currentDeck.GetFlagShip();
				this.DeckName.supportEncoding = false;
				if (shipModel == null)
				{
					this.DeckName.text = string.Empty;
					return;
				}
				if (isShort)
				{
					this.DeckName.text = "旗艦 " + shipModel.ShipTypeName + " " + shipModel.Name;
				}
				else
				{
					this.DeckName.text = string.Concat(new string[]
					{
						currentDeck.Name,
						"旗艦\n",
						shipModel.ShipTypeName,
						" ",
						shipModel.Name
					});
				}
			}

			public string getDeckStateString(DeckModel deck)
			{
				string result = string.Empty;
				switch (deck.MissionState)
				{
				case MissionStates.NONE:
					result = "行動可能";
					break;
				case MissionStates.RUNNING:
					result = "遠征中 残り" + deck.MissionRemainingTurns + "日";
					break;
				case MissionStates.STOP:
					result = "遠征帰還中";
					break;
				}
				if (deck.IsActionEnd())
				{
					result = "行動終了";
				}
				return result;
			}
		}

		[SerializeField]
		private StrategyBottomInfo.DeckInfo deckInfo;

		[SerializeField]
		private StrategyHaveMaterials haveMaterial;

		[SerializeField]
		private TweenPosition TweenPos;

		public void UpdateBottomPanel(StrategyInfoManager.Mode nowMode, bool isUpdateMaterial = true)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode == StrategyInfoManager.Mode.DeckInfo)
				{
					this.deckInfo.UpdateDeckInfo(false);
				}
			}
			else
			{
				if (isUpdateMaterial)
				{
					this.haveMaterial.UpdateFooterMaterials();
				}
				this.deckInfo.UpdateDeckInfo(false);
			}
		}

		public void UpdateDeckInfo(bool isShort)
		{
			this.deckInfo.UpdateDeckInfo(isShort);
		}

		public void Enter(StrategyInfoManager.Mode nowMode)
		{
			this.UpdateBottomPanel(nowMode, true);
			this.TweenPos.PlayForward();
		}

		public void Exit()
		{
			this.TweenPos.PlayReverse();
		}

		public void ChangeMode(StrategyInfoManager.Mode nowMode)
		{
			if (nowMode != StrategyInfoManager.Mode.AreaInfo)
			{
				if (nowMode == StrategyInfoManager.Mode.DeckInfo)
				{
					this.haveMaterial.ParentObject.SetActive(true);
					this.deckInfo.ParentObject.SetActive(false);
				}
			}
			else
			{
				this.haveMaterial.ParentObject.SetActive(true);
				this.deckInfo.ParentObject.SetActive(true);
			}
		}
	}
}
