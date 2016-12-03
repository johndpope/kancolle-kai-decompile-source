using Common.Enum;
using local.models;
using Server_Controllers;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexTile : MonoBehaviour
	{
		[SerializeField]
		private UISprite HexTile;

		[SerializeField]
		private UIButton TileButton;

		[SerializeField]
		private StrategyTileDockIcons DockIcons;

		private UISprite ShadowTile;

		private UISprite GrowTile;

		private TweenScale ScaleAnimation;

		private MapAreaModel areaModel;

		public int DebugRP = 100;

		public bool isFocus;

		public bool isMovable;

		public bool isColorChanged;

		private static readonly Color32 AttentionColor = new Color32(255, 185, 11, 110);

		private static readonly Color32 CautionColor = new Color32(255, 167, 107, 200);

		private static readonly Color32 WarningColor = new Color32(255, 107, 107, 200);

		private static readonly Color32 AlertColor = new Color32(255, 40, 40, 200);

		private static readonly Color32 InvationColor = new Color32(255, 0, 0, 200);

		[Button("TileColorRefreshdebug", "TileColorRefreshdebug", new object[]
		{

		})]
		public int button1;

		[Button("SetRebellionPoint", "SetRebellionPoint", new object[]
		{

		})]
		public int button2;

		public int areaID
		{
			get
			{
				return this.areaModel.Id;
			}
		}

		public bool isOpen
		{
			get
			{
				return this.areaModel.IsOpen();
			}
		}

		public bool isRebellionTile
		{
			get;
			private set;
		}

		public bool isNatural
		{
			get
			{
				return !this.isFocus && !this.isMovable;
			}
		}

		public UIButton getTileButton()
		{
			return this.TileButton;
		}

		public MapAreaModel GetAreaModel()
		{
			return this.areaModel;
		}

		private void Awake()
		{
			this.ScaleAnimation = base.GetComponent<TweenScale>();
			base.get_transform().localScaleX(0f);
			base.get_transform().localScaleY(0f);
			this.GrowTile = base.get_transform().FindChild("Grow").GetComponent<UISprite>();
			this.ShadowTile = this.GrowTile.get_transform().FindChild("Shadow").GetComponent<UISprite>();
			this.TileButton.tweenTarget = this.GrowTile.get_gameObject();
			this.ShadowTile.set_enabled(true);
			this.GrowTile.set_enabled(true);
			this.TileButton.duration = 0.5f;
			EventDelegate.Add(this.TileButton.onClick, new EventDelegate.Callback(this.PlayBoundAnimation));
			EventDelegate.Add(this.TileButton.onClick, new EventDelegate.Callback(this.OnPushFocusArea));
			EventDelegate.Add(this.TileButton.onClick, new EventDelegate.Callback(this.OnPushFocusChange));
		}

		public void PlayPopUpAnimation(Action<bool> callBack, float delay)
		{
			this.ScaleAnimation.delay = delay;
			this.ScaleAnimation.PlayForward();
			if (callBack != null)
			{
				this.ScaleAnimation.SetOnFinished(delegate
				{
					callBack.Invoke(true);
				});
			}
		}

		public void setAreaModel(MapAreaModel model)
		{
			this.areaModel = model;
		}

		public UISprite getSprite()
		{
			return this.HexTile;
		}

		public void PlayBoundAnimation()
		{
			if (!StrategyTopTaskManager.GetSailSelect().isRun && !StrategyTopTaskManager.GetShipMove().isRun)
			{
				return;
			}
			this.ScaleAnimation.delay = 0f;
			this.ScaleAnimation.from = new Vector3(0.9f, 0.9f, 0.9f);
			this.ScaleAnimation.ResetToBeginning();
			this.ScaleAnimation.PlayForward();
		}

		private void OnPushFocusChange()
		{
			if (!StrategyTopTaskManager.GetSailSelect().isRun && !StrategyTopTaskManager.GetShipMove().isRun)
			{
				return;
			}
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(this.areaID, false);
		}

		private void OnPushFocusArea()
		{
			if (!StrategyTopTaskManager.GetSailSelect().isRun && !StrategyTopTaskManager.GetShipMove().isRun)
			{
				return;
			}
			if (StrategyTopTaskManager.Instance.TileManager.FocusTile == this)
			{
				if (StrategyTopTaskManager.GetSailSelect().isRun)
				{
					StrategyTopTaskManager.GetSailSelect().OpenCommandMenu();
				}
				else if (this.areaID == SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID)
				{
					StrategyTopTaskManager.GetShipMove().OnMoveCancel();
				}
				else
				{
					StrategyTopTaskManager.GetShipMove().OnMoveDeside();
				}
			}
		}

		public void setTileColor()
		{
			if (this.isNatural || this.isFocus)
			{
				Color color = this.getRebellionColor();
				this.TileButton.hover = color;
				this.TileButton.pressed = color;
				this.TileButton.defaultColor = color;
				this.isColorChanged = (color != Color.get_clear());
			}
			else if (this.isMovable)
			{
				this.TileButton.hover = Color.get_blue();
				this.TileButton.pressed = Color.get_blue();
				this.TileButton.defaultColor = Color.get_blue();
			}
			else
			{
				Debug.Log("Tile" + this.areaID);
			}
		}

		public void setRebellionTile()
		{
			this.isRebellionTile = true;
			Color color = StrategyHexTile.InvationColor;
			this.TileButton.hover = color;
			this.TileButton.pressed = color;
			this.TileButton.defaultColor = color;
		}

		private Color32 getRebellionColor()
		{
			switch (this.areaModel.RState)
			{
			case RebellionState.Safety:
				return Color.get_clear();
			case RebellionState.Attention:
				return StrategyHexTile.AttentionColor;
			case RebellionState.Caution:
				return StrategyHexTile.CautionColor;
			case RebellionState.Warning:
				return StrategyHexTile.WarningColor;
			case RebellionState.Alert:
				return StrategyHexTile.AlertColor;
			case RebellionState.Invation:
				return StrategyHexTile.InvationColor;
			default:
				return Color.get_white();
			}
		}

		public void TileColorRefreshdebug()
		{
			this.TileColorRefresh(null);
		}

		public void TileColorRefresh(Action Onfinished)
		{
			this.TileButton.set_enabled(false);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.GrowTile.get_gameObject(), 3f, 0f);
			tweenAlpha.onFinished.Clear();
			tweenAlpha.SetOnFinished(delegate
			{
				this.TileButton.set_enabled(true);
				this.TileButton.defaultColor = Color.get_clear();
				this.TileButton.hover = Color.get_clear();
				this.TileButton.pressed = Color.get_clear();
				if (Onfinished != null)
				{
					Onfinished.Invoke();
				}
			});
		}

		public void ChangeMoveTileColor()
		{
			TweenColor.Begin(this.HexTile.get_gameObject(), 0.2f, Color.get_cyan());
			this.ShadowTile.depth = 1;
			this.GrowTile.depth = 2;
		}

		public void ClearTileColor()
		{
			TweenColor.Begin(this.HexTile.get_gameObject(), 0.2f, Color.get_white());
			this.ShadowTile.depth = 4;
			this.GrowTile.depth = 5;
		}

		public Vector3 getDefaultPosition()
		{
			return base.get_transform().get_localPosition() + base.get_transform().get_parent().get_localPosition();
		}

		public void PlayAreaOpenAnimation(GameObject AnimationPrefab, Action Onfinished)
		{
			this.SetActive(true);
			base.get_transform().set_localScale(Vector3.get_zero());
			this.ScaleAnimation.from = Vector3.get_zero();
			this.ScaleAnimation.delay = 0f;
			this.ScaleAnimation.ResetToBeginning();
			this.ScaleAnimation.PlayForward();
			this.ScaleAnimation.duration = 2f;
			this.SafeGetTweenRotation(Vector3.get_zero(), new Vector3(0f, 0f, 720f), 1.5f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			Keyframe[] array = new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			};
			this.ScaleAnimation.animationCurve = new AnimationCurve(array);
			GameObject gameObject = Util.Instantiate(AnimationPrefab, base.get_transform().get_parent().get_gameObject(), false, false);
			gameObject.get_transform().get_transform().set_position(this.HexTile.get_transform().get_position());
			this.GrowTile.color = Color.get_clear();
			base.StartCoroutine(this.waitAnimationFinish(gameObject, Onfinished, 2f));
		}

		public void PlayAreaCloseAnimation(Action Onfinished)
		{
			this.StartTileBreakAnimation(Onfinished);
		}

		[DebuggerHidden]
		private IEnumerator waitAnimationFinish(GameObject Animation, Action Onfinished, float waitTime)
		{
			StrategyHexTile.<waitAnimationFinish>c__Iterator177 <waitAnimationFinish>c__Iterator = new StrategyHexTile.<waitAnimationFinish>c__Iterator177();
			<waitAnimationFinish>c__Iterator.waitTime = waitTime;
			<waitAnimationFinish>c__Iterator.Onfinished = Onfinished;
			<waitAnimationFinish>c__Iterator.Animation = Animation;
			<waitAnimationFinish>c__Iterator.<$>waitTime = waitTime;
			<waitAnimationFinish>c__Iterator.<$>Onfinished = Onfinished;
			<waitAnimationFinish>c__Iterator.<$>Animation = Animation;
			return <waitAnimationFinish>c__Iterator;
		}

		public void setActivePositionAnimation(bool isActive)
		{
		}

		public void StartTileBreakAnimation(Action Onfinished)
		{
			StrategyHexBreak strategyHexBreak = StrategyHexBreak.Instantiate(Resources.Load<StrategyHexBreak>("Prefabs/StrategyPrefab/StrategyTop/TileBreak"), base.get_transform());
			this.DelayActionFrame(3, delegate
			{
				strategyHexBreak.Play(delegate
				{
					Onfinished.Invoke();
					this.get_transform().set_localScale(Vector3.get_zero());
					Object.Destroy(strategyHexBreak.get_gameObject());
				});
				this.HexTile.alpha = 0f;
				this.GrowTile.alpha = 0f;
			});
		}

		public void StartTileLightAnimation(Action Onfinished)
		{
			StrategyHexLight strategyHexLight = StrategyHexLight.Instantiate(Resources.Load<StrategyHexLight>("Prefabs/StrategyPrefab/StrategyTop/TileLight"), base.get_transform());
			this.DelayActionFrame(3, delegate
			{
				strategyHexLight.Play(delegate
				{
					if (Onfinished != null)
					{
						Onfinished.Invoke();
					}
					Object.Destroy(strategyHexLight.get_gameObject());
				});
			});
			this.TileButton.set_enabled(true);
			this.TileButton.defaultColor = Color.get_clear();
			this.TileButton.hover = Color.get_clear();
			this.TileButton.pressed = Color.get_clear();
		}

		private void SetRebellionPoint()
		{
			Debug_Mod.SetRebellionPoint(this.areaID, this.DebugRP);
			StrategyTopTaskManager.CreateLogicManager();
			this.setTileColor();
		}

		public void UpdateDockIcons()
		{
			if (this.DockIcons != null)
			{
				this.DockIcons.SetDockIcon(this.areaModel);
			}
		}

		public void SetVisibleDockIcons(bool isVisible)
		{
			if (this.DockIcons != null)
			{
				this.DockIcons.SetActive(isVisible);
			}
		}
	}
}
