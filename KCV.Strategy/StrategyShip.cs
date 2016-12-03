using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShip : MonoBehaviour
	{
		[SerializeField]
		private UISprite shipSprite;

		[SerializeField]
		private UITexture shipStateIcon;

		[SerializeField]
		private UISprite shipNoIcon;

		[SerializeField]
		private Texture2D EnseiIcon;

		[SerializeField]
		private Texture2D TettaiIcon;

		[SerializeField]
		private TweenScale popupAnimation;

		private UIButtonMessage ButtonMes;

		[SerializeField]
		private AnimationCurve curve;

		public DeckModel deck
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.ButtonMes = this.shipSprite.GetComponent<UIButtonMessage>();
			this.ButtonMes.target = base.get_gameObject();
			this.ButtonMes.functionName = "OnIconTouch";
			this.setColliderEnable(false);
		}

		public void setDeckModel(DeckModel deck)
		{
			this.deck = deck;
		}

		public void setShipGraph()
		{
			int num = 2;
			if (this.deck.GetFlagShip() != null)
			{
				num = this.deck.GetFlagShip().ShipType;
			}
			this.shipSprite.spriteName = "shipicon_" + num;
			this.shipSprite.MakePixelPerfect();
			this.shipSprite.get_transform().localScaleX(0.8f);
			this.shipNoIcon = base.get_transform().FindChild("Number").GetComponent<UISprite>();
			this.shipStateIcon = base.get_transform().FindChild("stateIcon").GetComponent<UITexture>();
		}

		public void setShipState()
		{
			if (this.isActionEndOrEnseiOrTettai())
			{
				this.setShipColor(Color.get_gray());
			}
			else
			{
				this.setShipColor(Color.get_white());
			}
			if (this.deck.HasBling())
			{
				this.shipStateIcon.mainTexture = this.TettaiIcon;
				this.shipStateIcon.width = 60;
				this.shipStateIcon.height = 80;
			}
			else if (this.deck.MissionState == MissionStates.RUNNING || this.deck.MissionState == MissionStates.STOP)
			{
				this.shipStateIcon.mainTexture = this.EnseiIcon;
				this.shipStateIcon.width = 60;
				this.shipStateIcon.height = 80;
			}
			else
			{
				this.shipStateIcon.mainTexture = null;
			}
		}

		public void unsetShipStateIcon()
		{
			this.shipStateIcon.mainTexture = null;
		}

		public void setShipAreaPosition(Vector3 pos)
		{
			base.get_transform().set_position(pos);
		}

		public void popUpShipIcon()
		{
			this.popupAnimation.ResetToBeginning();
			this.popupAnimation.PlayForward();
			if (this.popupAnimation.animationCurve != this.curve)
			{
				this.popupAnimation.SetOnFinished(delegate
				{
					this.ChangePopUpAnimation();
					this.setColliderEnable(true);
				});
			}
		}

		private void ChangePopUpAnimation()
		{
			this.popupAnimation.animationCurve = this.curve;
			this.popupAnimation.duration = 0.3f;
			this.popupAnimation.onFinished.Clear();
		}

		private void setShipColor(Color color)
		{
			this.shipSprite.color = color;
			this.shipNoIcon.color = color;
		}

		private bool isActionEndOrEnseiOrTettai()
		{
			return this.deck.IsActionEnd() || this.deck.MissionState == MissionStates.RUNNING || this.deck.MissionState == MissionStates.STOP;
		}

		private void OnIconTouch()
		{
			if (StrategyTopTaskManager.Instance != null && !StrategyTopTaskManager.GetSailSelect().isRun)
			{
				return;
			}
			this.popUpShipIcon();
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == this.deck && StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID == this.deck.AreaId)
			{
				StrategyTopTaskManager.GetSailSelect().OpenCommandMenu();
			}
			else
			{
				this.ChangeCurrentDeck();
			}
		}

		private void ChangeCurrentDeck()
		{
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = this.deck;
			StrategyTopTaskManager.GetSailSelect().changeDeck(this.deck.Id);
			StrategyTopTaskManager.Instance.GetAreaMng().ChangeFocusTile(this.deck.AreaId, false);
			StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
		}

		public void setColliderEnable(bool isEnable)
		{
			if (this.ButtonMes != null)
			{
				this.ButtonMes.set_enabled(isEnable);
			}
		}
	}
}
