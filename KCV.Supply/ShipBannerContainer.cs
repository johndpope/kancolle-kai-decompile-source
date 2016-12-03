using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Supply
{
	public class ShipBannerContainer : MonoBehaviour
	{
		private const int MAX_SHIP_PER_DECK = 6;

		[SerializeField]
		private UISupplyDeckShipBanner[] _shipBanner;

		private int currentIdx;

		private Vector3 showPos = new Vector3(0f, 0f);

		private Vector3 hidePos = new Vector3(-1000f, 0f);

		private DeckModel deck;

		private int shipCount
		{
			get
			{
				return this.deck.GetShipCount();
			}
		}

		public void Init()
		{
			for (int i = 0; i < 6; i++)
			{
				this._shipBanner[i].SafeGetTweenAlpha(0f, 1f, 0.2f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				this._shipBanner[i].Init(new Vector3(-162f, 106f - (float)i * 63f, 0f));
			}
		}

		public void SelectLengthwise(bool isUp)
		{
			bool flag = this.UpdateCurrentItem((!isUp) ? (this.currentIdx + 1) : (this.currentIdx - 1));
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void InitDeck(DeckModel deck)
		{
			this.deck = deck;
			for (int i = 0; i < 6; i++)
			{
				if (this._shipBanner[i] != null)
				{
					this._shipBanner[i].SetBanner((i >= this.shipCount) ? null : deck.GetShip(i), i);
				}
			}
			this.UpdateCurrentItem(0);
		}

		public bool UpdateCurrentItem(int newIdx)
		{
			if (newIdx < 0 || newIdx >= this.shipCount)
			{
				return false;
			}
			this.currentIdx = newIdx;
			for (int i = 0; i < 6; i++)
			{
				if (this._shipBanner[i] != null)
				{
					this._shipBanner[i].Hover(i == this.currentIdx);
				}
			}
			return true;
		}

		private void RemoveAllHover()
		{
			for (int i = 0; i < 6; i++)
			{
				this._shipBanner[i].Hover(false);
			}
		}

		public void SwitchCurrentSelected()
		{
			if (!this._shipBanner[this.currentIdx].IsSelectable())
			{
				return;
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this._shipBanner[this.currentIdx].SwitchSelected();
			SupplyMainManager.Instance.UpdateRightPain();
		}

		public void SwitchAllSelected()
		{
			bool flag = false;
			SupplyMainManager.Instance.SupplyManager.ClickCheckBoxAll();
			for (int i = 0; i < this.shipCount; i++)
			{
				if (this._shipBanner[i].get_enabled())
				{
					flag = true;
					this._shipBanner[i].Select(SupplyMainManager.Instance.SupplyManager.CheckBoxStates[i] == CheckBoxStatus.ON);
				}
			}
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			SupplyMainManager.Instance.UpdateRightPain();
		}

		public List<ShipModel> getSeletedModelList()
		{
			List<ShipModel> list = new List<ShipModel>();
			for (int i = 0; i < this.shipCount; i++)
			{
				if (this._shipBanner[i].selected)
				{
					list.Add(this._shipBanner[i].Ship);
				}
			}
			return list;
		}

		public void Show(bool animation)
		{
			base.get_transform().set_localPosition((!animation) ? this.showPos : this.showPos);
		}

		public void Hide(bool animation)
		{
			SupplyMainManager.Instance.SetControllDone(false);
			base.get_transform().set_localPosition((!animation) ? this.hidePos : this.hidePos);
		}

		public void SetFocus(bool focused)
		{
			SupplyMainManager.Instance.SetControllDone(focused);
			this.RemoveAllHover();
			if (focused)
			{
				this._shipBanner[this.currentIdx].Hover(true);
			}
		}

		public void ProcessRecoveryAnimation()
		{
			for (int i = 0; i < this.shipCount; i++)
			{
				if (this._shipBanner[i].selected)
				{
					this._shipBanner[i].ProcessRecoveryAnimation();
				}
			}
		}
	}
}
