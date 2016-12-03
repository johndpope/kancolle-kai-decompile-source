using live2d;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class UIShipCharacter : MonoBehaviour
	{
		public enum Live2DPosition
		{
			Strategy,
			Port,
			Natural
		}

		[SerializeField]
		protected UITexture Render;

		[SerializeField]
		protected TweenPosition tweenPosition;

		public AnimationCurve ForwardCurve;

		public AnimationCurve ReverseCurve;

		public bool isEnter;

		public Vector3 L2dBias;

		public Vector3 ShipIn;

		public Vector3 ShipIn2;

		public Vector3 ShipOut;

		public ShipModel shipModel;

		public UITexture render
		{
			get
			{
				return this.Render;
			}
		}

		private void Awake()
		{
			if (base.GetComponent<UITexture>() != null)
			{
				this.Render = base.GetComponent<UITexture>();
			}
			if (base.GetComponent<TweenPosition>() != null)
			{
				this.tweenPosition = base.GetComponent<TweenPosition>();
			}
		}

		public void moveCharacterX(float targetX, float time, Action act)
		{
			TweenPosition tweenPosition = this.PlayMove(base.get_transform().get_localPosition(), new Vector3(targetX, this.getEnterPosition().y, base.get_transform().get_localPosition().z), time, act, true, false);
		}

		public void moveAddCharacterX(float addX, float time, Action act)
		{
			TweenPosition tweenPosition = this.PlayMove(base.get_transform().get_localPosition(), base.get_transform().get_localPosition() + new Vector3(addX, 0f, 0f), time, act, true, false);
		}

		public void Enter(Action act)
		{
			if (!this.isEnter)
			{
				this.SetActive(true);
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
				this.PlayMove(this.getExitPosition(), this.getEnterPosition(), 0.4f, act, true, false);
				this.isEnter = true;
			}
			else if (act != null)
			{
				act.Invoke();
			}
		}

		public void Exit(Action act, bool isActive = false)
		{
			if (this.isEnter)
			{
				this.PlayMove(base.get_transform().get_localPosition(), this.getExitPosition(), 0.4f, act, isActive, true);
				this.isEnter = false;
			}
			else if (act != null)
			{
				act.Invoke();
			}
		}

		private TweenPosition PlayMove(Vector3 from, Vector3 to, float time, Action Onfinished, bool isActive, bool isReverse = false)
		{
			TweenPosition tweenPosition = this.tweenPosition;
			tweenPosition.set_enabled(true);
			tweenPosition.ResetToBeginning();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.duration = time;
			tweenPosition.onFinished.Clear();
			tweenPosition.SetOnFinished(delegate
			{
				if (Onfinished != null)
				{
					Onfinished.Invoke();
				}
				this.SetActive(isActive);
			});
			tweenPosition.animationCurve = ((!isReverse) ? this.ForwardCurve : this.ReverseCurve);
			tweenPosition.PlayForward();
			return tweenPosition;
		}

		public Vector3 getEnterPosition()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? this.ShipIn : (this.ShipIn + this.L2dBias);
		}

		public Vector3 getEnterPosition2()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? this.ShipIn2 : (this.ShipIn2 + this.L2dBias);
		}

		public Vector3 getExitPosition()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? this.ShipOut : (this.ShipOut + this.L2dBias);
		}

		public void ChangeCharacter()
		{
			this.ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip());
		}

		public void ChangeCharacter(ShipModelMst model)
		{
			this.ChangeCharacter(model, -1, false);
		}

		public virtual void ChangeCharacter(ShipModel model)
		{
			this.ChangeCharacter(model, -1);
		}

		public void ChangeCharacter(Live2DModelUnity Live2D, ShipModel model)
		{
			this.L2dBias = new Vector3((float)model.Offsets.GetLive2dBias().x, (float)model.Offsets.GetLive2dBias().y, 0f);
			this.ShipIn = new Vector3((float)model.Offsets.GetBoko(false).x, (float)model.Offsets.GetBoko(false).y, 0f);
			this.ShipIn2 = new Vector3((float)model.Offsets.GetCutinSp1_InBattle(false).x, (float)model.Offsets.GetCutinSp1_InBattle(false).y, 0f);
			this.ShipOut = new Vector3(1300f, (float)model.Offsets.GetBoko(false).y, 0f);
			this.Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(Live2D, model);
			this.Render.width = (int)((float)model.Offsets.GetLive2dSize().x * 1.25f);
			this.Render.height = model.Offsets.GetLive2dSize().y;
			base.get_transform().localPositionY((float)(model.Offsets.GetBoko(false).y + model.Offsets.GetLive2dBias().y));
		}

		public void ChangeCharacter(ShipModel model, int DeckID)
		{
			this.shipModel = model;
			ShipModelMst model2 = (model == null) ? null : model;
			bool isDamaged = model != null && model.IsDamaged();
			this.ChangeCharacter(model2, DeckID, isDamaged);
		}

		public void ChangeCharacter(ShipModelMst model, int DeckID, bool isDamaged)
		{
			if (!this.CheckMstIDEnable(model))
			{
				return;
			}
			if (this.tweenPosition != null)
			{
				this.tweenPosition.set_enabled(false);
			}
			int graphicsMstId = model.GetGraphicsMstId();
			if (DeckID == -1)
			{
				this.Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(graphicsMstId, isDamaged);
			}
			else
			{
				this.Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(graphicsMstId, isDamaged, DeckID);
			}
			this.L2dBias = new Vector3((float)model.Offsets.GetLive2dBias().x, (float)model.Offsets.GetLive2dBias().y, 0f);
			this.ShipIn = new Vector3((float)model.Offsets.GetBoko(isDamaged).x, (float)model.Offsets.GetBoko(isDamaged).y, 0f);
			this.ShipIn2 = new Vector3((float)model.Offsets.GetCutinSp1_InBattle(isDamaged).x, (float)model.Offsets.GetCutinSp1_InBattle(isDamaged).y, 0f);
			this.ShipOut = new Vector3(1300f, (float)model.Offsets.GetBoko(isDamaged).y, 0f);
			if (SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel)
			{
				this.Render.width = (int)((float)model.Offsets.GetLive2dSize().x * 1380f / 1024f);
				this.Render.height = model.Offsets.GetLive2dSize().y;
				base.get_transform().localPositionY((float)(model.Offsets.GetBoko(isDamaged).y + model.Offsets.GetLive2dBias().y));
			}
			else
			{
				this.L2dBias = Vector3.get_zero();
				base.get_transform().localPositionY((float)model.Offsets.GetBoko(isDamaged).y);
				this.Render.MakePixelPerfect();
			}
		}

		protected virtual bool CheckMstIDEnable(ShipModelMst model)
		{
			if (model == null)
			{
				this.Render.mainTexture = null;
				return false;
			}
			return true;
		}

		public float getModelDefaultPosX()
		{
			ShipModel flagShip = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
			if (flagShip == null)
			{
				return 0f;
			}
			return (float)(flagShip.Offsets.GetBoko(flagShip.IsDamaged()).x + flagShip.Offsets.GetLive2dBias().x);
		}

		public int GetWidth()
		{
			return this.Render.width;
		}

		public int GetHeight()
		{
			return this.Render.height;
		}
	}
}
