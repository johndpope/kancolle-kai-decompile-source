using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipCharacter : UIShipCharacter
	{
		[SerializeField]
		private UISprite DeckStateTag;

		[SerializeField]
		private StrategyCharacterCollision collision;

		public static ShipModel nowShipModel;

		[SerializeField]
		private UITexture HeadArea;

		[Button("DebugChange", "DebugChange", new object[]
		{
			1
		})]
		public int Button01;

		[Button("DebugChange", "DebugChangePrev", new object[]
		{
			-1
		})]
		public int Button02;

		public int DebugMstID = 1;

		public int GraphicID;

		private void Start()
		{
			if (StrategyShipCharacter.nowShipModel == null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != null)
			{
				StrategyShipCharacter.nowShipModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
			}
		}

		public void setState(DeckModel deck)
		{
			string text = string.Empty;
			if (this.isEnsei(deck))
			{
				text = "ensei";
			}
			if (this.isTettai(deck))
			{
				text = "withdraw";
			}
			this.DeckStateTag.spriteName = "shipicon_" + text;
			if (text != string.Empty)
			{
				base.get_transform().set_localPosition(base.getExitPosition());
			}
			else if (this.isEnter)
			{
				base.get_transform().set_localPosition(base.getEnterPosition());
			}
		}

		protected override bool CheckMstIDEnable(ShipModelMst model)
		{
			if (model == null)
			{
				this.Render.mainTexture = null;
				if (this.collision != null)
				{
					this.collision.SetActive(false);
				}
				return false;
			}
			if (this.collision != null)
			{
				this.collision.SetActive(true);
			}
			return true;
		}

		private bool isEnsei(DeckModel deck)
		{
			return deck.MissionState != MissionStates.NONE;
		}

		private bool isTettai(DeckModel deck)
		{
			return deck.GetFlagShip() != null && deck.GetFlagShip().IsEscaped();
		}

		public void PlayVoice(DeckModel deck)
		{
			if (this.isEnsei(deck) || this.isTettai(deck) || deck.GetFlagShip() == null)
			{
				return;
			}
			ShipUtils.PlayShipVoice(deck.GetFlagShip(), 2);
		}

		public void PlayVoice(EscortDeckModel deck)
		{
			if (deck.GetFlagShip() == null)
			{
				return;
			}
			ShipUtils.PlayShipVoice(deck.GetFlagShip(), 2);
		}

		public void ChangeCharacter(DeckModel deck)
		{
			ShipModel shipModel = deck.GetFlagShip();
			if (deck != null && deck.MissionState != MissionStates.NONE)
			{
				shipModel = null;
			}
			if (SingletonMonoBehaviour<Live2DModel>.Instance != null && !SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel && StrategyShipCharacter.nowShipModel != null && shipModel != null && (StrategyShipCharacter.nowShipModel.GetGraphicsMstId() != shipModel.GetGraphicsMstId() || !shipModel.IsDamaged()) && this.Render != null && this.Render.mainTexture != null)
			{
				Resources.UnloadAsset(this.Render.mainTexture);
				this.Render.mainTexture = null;
			}
			base.ChangeCharacter(shipModel, deck.Id);
			StrategyShipCharacter.nowShipModel = shipModel;
			if (this.collision != null)
			{
				this.collision.ResetTouchCount();
				if (this.Render != null)
				{
					this.collision.SetCollisionHight(this.Render.height);
				}
			}
		}

		public void ResetPosition()
		{
			base.get_transform().localPositionX(this.ShipIn.x + this.L2dBias.x);
		}

		private DeckModel getDeck(ShipModel model)
		{
			if (model == null)
			{
				return null;
			}
			int num = model.IsInDeck();
			if (num == -1)
			{
				return null;
			}
			if (num == 0)
			{
				num = 1;
			}
			return StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(num);
		}

		public void DebugChange(int direction)
		{
			ShipModelMst shipModelMst = null;
			int i = 0;
			while (i < 200)
			{
				this.DebugMstID = (int)Util.RangeValue(this.DebugMstID + direction, 1f, 500f);
				try
				{
					shipModelMst = new ShipModelMst(this.DebugMstID);
					int buildStep = shipModelMst.BuildStep;
				}
				catch (KeyNotFoundException var_3_44)
				{
					goto IL_65;
				}
				goto IL_4F;
				IL_65:
				i++;
				continue;
				IL_4F:
				if (shipModelMst != null && shipModelMst.MstId != 0)
				{
					break;
				}
				goto IL_65;
			}
			this.GraphicID = shipModelMst.GetGraphicsMstId();
			base.ChangeCharacter(shipModelMst, -1, false);
			base.get_transform().localPositionX(this.ShipIn.x + this.L2dBias.x);
			if (shipModelMst == null)
			{
				this.DebugMstID = 0;
			}
			if (this.HeadArea != null && this.shipModel != null)
			{
				this.HeadArea.get_transform().localPositionX((float)this.shipModel.Offsets.GetFace(false).x - this.L2dBias.x);
				this.HeadArea.get_transform().localPositionY((float)(-(float)this.shipModel.Offsets.GetFace(false).y) - this.L2dBias.y);
				this.HeadArea.depth = 15;
			}
		}

		private void OnDestroy()
		{
			StrategyShipCharacter.nowShipModel = null;
			this.collision = null;
			this.DeckStateTag = null;
		}

		public void ResetTouchCount()
		{
			this.collision.ResetTouchCount();
		}

		public void SetCollisionEnable(bool isEnable)
		{
			this.collision.SetActive(isEnable);
		}

		public void SetEnableBackTouch(bool isEnable)
		{
			this.collision.SetEnableBackTouch(isEnable);
		}
	}
}
