using Common.Enum;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeShipListChild : UIScrollListChild<ShipModel>
	{
		[SerializeField]
		protected UITexture _uiDeckFlag;

		[SerializeField]
		protected UISprite mSprite_TypeIcon;

		[SerializeField]
		protected UISprite mSprite_StateIcon;

		[SerializeField]
		protected UILabel mLabel_Name;

		[SerializeField]
		protected UILabel mLabel_Level;

		[SerializeField]
		protected UILabel mLabel_Taikyu;

		[SerializeField]
		protected UILabel mLabel_Karyoku;

		[SerializeField]
		protected UILabel mLabel_Taiku;

		[SerializeField]
		protected UILabel mLabel_Speed;

		[SerializeField]
		protected UISprite mSprite_LockIcon;

		[SerializeField]
		protected UISprite mSprite_ActiveState;

		[SerializeField]
		protected UISprite mSprite_ShipState;

		[SerializeField]
		protected Animation mAnimation_MarriagedRing;

		public override void OnTouchScrollListChild()
		{
			if (base.Model != null)
			{
				base.OnTouchScrollListChild();
			}
		}

		protected override void InitializeChildContents(ShipModel shipModel, bool clickable)
		{
			this.InitializeDeckFlag(shipModel);
			this.InitializeActiveStateIcon(shipModel);
			this.InitializeShipTypeIcon(shipModel);
			this.InitializeShipStateIcon(shipModel);
			this.mLabel_Name.text = shipModel.Name;
			this.mLabel_Level.text = shipModel.Level.ToString();
			this.mLabel_Taikyu.text = shipModel.MaxHp.ToString();
			this.mLabel_Karyoku.text = shipModel.Karyoku.ToString();
			this.mLabel_Taiku.text = shipModel.Taiku.ToString();
			this.InitializeShipSpeed(shipModel);
			this.InitializeLockIcon(shipModel);
			this.InitializeRing(shipModel);
			if (base.Model.IsMarriage())
			{
				this.StartRingAnimation();
			}
			else
			{
				this.StopRingAnimation();
			}
		}

		private void InitializeShipStateIcon(ShipModel shipModel)
		{
			string spriteName = string.Empty;
			bool flag = shipModel.IsInRepair();
			if (flag)
			{
				spriteName = "icon-ss_syufuku";
			}
			else
			{
				switch (shipModel.DamageStatus)
				{
				case DamageState.Shouha:
					spriteName = "icon-ss_shoha";
					break;
				case DamageState.Tyuuha:
					spriteName = "icon-ss_chuha";
					break;
				case DamageState.Taiha:
					spriteName = "icon-ss_taiha";
					break;
				}
			}
			this.mSprite_ShipState.spriteName = spriteName;
		}

		private void InitializeLockIcon(ShipModel shipModel)
		{
			bool flag = shipModel.IsLocked();
			if (flag)
			{
				this.mSprite_LockIcon.spriteName = "lock_ship";
			}
			else
			{
				this.mSprite_LockIcon.spriteName = "lock_ship_open";
			}
		}

		private void InitializeRing(ShipModel shipModel)
		{
			bool flag = shipModel.IsMarriage();
			if (flag)
			{
				this.mAnimation_MarriagedRing.get_gameObject().SetActive(true);
				using (IEnumerator enumerator = this.mAnimation_MarriagedRing.get_transform().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Transform component = (Transform)enumerator.get_Current();
						component.SetActive(true);
					}
				}
			}
			else
			{
				this.StopRingAnimation();
				using (IEnumerator enumerator2 = this.mAnimation_MarriagedRing.get_transform().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Transform component2 = (Transform)enumerator2.get_Current();
						component2.SetActive(false);
					}
				}
			}
		}

		public void SwitchShipLockState()
		{
			OrganizeTaskManager.Instance.GetTopTask().UpdateShipLock(base.Model.MemId);
			if (base.Model.IsLocked())
			{
				this.mSprite_LockIcon.spriteName = "lock_ship";
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				this.mSprite_LockIcon.spriteName = "lock_ship_open";
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
		}

		public void StartRingAnimation()
		{
			bool flag = base.Model.IsMarriage();
			if (flag)
			{
				bool flag2 = !this.mAnimation_MarriagedRing.get_isPlaying();
				if (flag2)
				{
					this.mAnimation_MarriagedRing.Play();
				}
			}
		}

		public void StopRingAnimation()
		{
			bool isPlaying = this.mAnimation_MarriagedRing.get_isPlaying();
			if (isPlaying)
			{
				this.mAnimation_MarriagedRing.Stop();
			}
		}

		private void InitializeShipSpeed(ShipModel shipModel)
		{
			bool flag = shipModel.Soku == 10;
			if (flag)
			{
				this.mLabel_Speed.text = "高速";
			}
			else
			{
				this.mLabel_Speed.text = "低速";
			}
		}

		private void InitializeShipTypeIcon(ShipModel shipIcon)
		{
			this.mSprite_TypeIcon.spriteName = "ship" + shipIcon.ShipType;
		}

		private void InitializeActiveStateIcon(ShipModel shipModel)
		{
			bool flag = shipModel.IsBling();
			if (flag)
			{
				this.mSprite_ActiveState.spriteName = "icon_kaikou";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag2 = shipModel.IsInMission();
			if (flag2)
			{
				this.mSprite_ActiveState.spriteName = "shipicon_ensei";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag3 = shipModel.IsEscaped();
			if (flag3)
			{
				this.mSprite_ActiveState.spriteName = "shipicon_withdraw";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag4 = shipModel.IsInActionEndDeck();
			if (flag4)
			{
				this.mSprite_ActiveState.spriteName = "icon-s_done";
				this.mSprite_ActiveState.SetDimensions(49, 41);
				return;
			}
			this.mSprite_ActiveState.spriteName = string.Empty;
		}

		private void InitializeDeckFlag(ShipModel shipModel)
		{
			bool flag = this.IsDeckInShip(shipModel);
			if (flag)
			{
				DeckModelBase deckFromShip = this.GetDeckFromShip(shipModel);
				bool isFlagShip = deckFromShip.GetFlagShip().MemId == shipModel.MemId;
				int id = deckFromShip.Id;
				bool flag2 = deckFromShip.IsEscortDeckMyself();
				if (flag2)
				{
					this.InitializeEscortDeckFlag(id, isFlagShip);
				}
				else
				{
					this.InitializeNormalDeckFlag(id, isFlagShip);
				}
			}
			else
			{
				this.RemoveDeckFlag();
			}
		}

		protected virtual bool IsDeckInShip(ShipModel shipModel)
		{
			DeckModelBase deckFromShip = this.GetDeckFromShip(shipModel);
			return deckFromShip != null;
		}

		protected virtual DeckModelBase GetDeckFromShip(ShipModel shipModel)
		{
			return shipModel.getDeck();
		}

		private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
		{
			string text = string.Empty;
			if (isFlagShip)
			{
				text = string.Format("icon_deck{0}_fs", deckId);
			}
			else
			{
				text = string.Format("icon_deck{0}", deckId);
			}
			this._uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
			this._uiDeckFlag.SetDimensions(56, 42);
		}

		private void RemoveDeckFlag()
		{
			this._uiDeckFlag.SetDimensions(0, 0);
			this._uiDeckFlag.mainTexture = null;
		}

		private void InitializeEscortDeckFlag(int deckId, bool isFlagShip)
		{
			string text = string.Empty;
			int w = 56;
			int h = 64;
			if (isFlagShip)
			{
				text = "icon_guard_fs";
			}
			else
			{
				text = "icon_guard";
			}
			this._uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
			this._uiDeckFlag.SetDimensions(w, h);
		}
	}
}
