using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class DeckShipInfoPanel : MonoBehaviour
	{
		private UIPanel panel;

		[SerializeField]
		private CommonShipBanner[] ShipBanners;

		[SerializeField]
		private UILabel DeckName;

		[SerializeField]
		private UILabel[] HPLabels;

		[SerializeField]
		private UILabel[] NameLabels;

		[SerializeField]
		private CommonShipSupplyState[] shipSupplyStates;

		[SerializeField]
		private GameObject BlackPanel;

		public bool isOpen;

		private float Cliping;

		private void Start()
		{
			this.panel = base.GetComponent<UIPanel>();
			this.panel.SetRect(0f, 0f, 0f, 1f);
			this.panel.alpha = 0f;
			this.Cliping = 0f;
			this.isOpen = false;
			this.DeckName.supportEncoding = false;
		}

		private void Update()
		{
		}

		public void OpenPanel()
		{
			this.isOpen = true;
			this.BlackPanel.SafeGetTweenAlpha(0f, 1f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			this.ChangeDeck();
			this.panel.alpha = 1f;
			this.panel.SetRect(0f, 0f, 0f, 1f);
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				this.Cliping,
				"to",
				1,
				"time",
				0.2f,
				"onupdate",
				"UpdateHandler"
			}));
			SoundUtils.PlaySE(SEFIleInfos.SE_002);
		}

		public void ClosePanel()
		{
			this.isOpen = false;
			this.BlackPanel.SafeGetTweenAlpha(1f, 0f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.2f, 0f);
			this.Cliping = 0f;
			for (int i = 0; i < this.ShipBanners.Length; i++)
			{
				if (this.ShipBanners[i].get_isActiveAndEnabled())
				{
					this.ShipBanners[i].StopParticle();
				}
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_003);
		}

		public void ChangeDeck()
		{
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			ShipModel[] ships = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(id).GetShips();
			this.DeckName.text = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Name;
			for (int i = 0; i < 6; i++)
			{
				if (i < ships.Length)
				{
					this.ShipBanners[i].get_transform().get_parent().SetActive(true);
					this.ShipBanners[i].SetShipData(ships[i]);
					this.HPLabels[i].text = ships[i].NowHp + " / " + ships[i].MaxHp;
					this.NameLabels[i].text = ships[i].Name;
					this.shipSupplyStates[i].setSupplyState(ships[i]);
				}
				else
				{
					this.ShipBanners[i].get_transform().get_parent().SetActive(false);
				}
			}
		}

		private void UpdateHandler(float value)
		{
			this.panel.SetRect(0f, 0f, 960f * value, 544f * value);
			this.Cliping = value;
		}
	}
}
