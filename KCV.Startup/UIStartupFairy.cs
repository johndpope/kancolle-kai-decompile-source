using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(UISprite))]
	public class UIStartupFairy : MonoBehaviour
	{
		public enum FairyType
		{
			None,
			Tails,
			Ahoge,
			Braid
		}

		public enum FairyState
		{
			Idle,
			Move,
			Jump
		}

		[Serializable]
		private struct Params
		{
			public float animationTime;
		}

		[Serializable]
		private struct StateParam
		{
			public int spriteNum;

			public int drawIntervalFrame;

			public int[] convertSpriteNumber;
		}

		[Serializable]
		private struct FairyAnimParameter
		{
			public UIStartupFairy.FairyType fairyType;

			public Vector2 baseSpriteSize;

			public UIStartupFairy.StateParam idleStateParam;

			public UIStartupFairy.StateParam moveStateParam;

			public UIStartupFairy.StateParam jumpStateParam;

			public Vector3 startPos;

			public Vector3 endPos;

			public float tripTime;

			public int jumpFrame;

			public float rotTime;

			public UIStartupFairy.StateParam GetStateParam(UIStartupFairy.FairyState iState)
			{
				switch (iState)
				{
				case UIStartupFairy.FairyState.Idle:
					return this.idleStateParam;
				case UIStartupFairy.FairyState.Move:
					return this.moveStateParam;
				case UIStartupFairy.FairyState.Jump:
					return this.jumpStateParam;
				default:
					return default(UIStartupFairy.StateParam);
				}
			}
		}

		private const string FAIRY_SPRITE_NAME = "mini{0}_{1}_{2:D2}";

		[SerializeField]
		private UITexture _uiBalloon;

		[Header("[Animation Parameter]"), SerializeField]
		private UIStartupFairy.Params _strParams;

		[Header("[Fairy Animation Parameters]"), SerializeField]
		private List<UIStartupFairy.FairyAnimParameter> _listFairyAnimationParameter;

		private UIStartupFairy.FairyType _iType;

		private UIStartupFairy.FairyState _iState;

		private UIStartupFairy.FairyState _iStatePrev;

		private UISprite _uiFairySprite;

		private BoxCollider _colBox;

		private List<IDisposable> _listObserverStream;

		private int _nSpriteIndex;

		private AudioSource _asSource;

		private UISprite fairySprite
		{
			get
			{
				return this.GetComponentThis(ref this._uiFairySprite);
			}
		}

		private BoxCollider collider
		{
			get
			{
				return this.GetComponentThis(ref this._colBox);
			}
		}

		public void Startup()
		{
			this._nSpriteIndex = 0;
			this._listObserverStream = new List<IDisposable>(Enum.GetValues(typeof(UIStartupFairy.FairyState)).get_Length());
			for (int i = 0; i < this._listObserverStream.get_Capacity(); i++)
			{
				this._listObserverStream.Add(null);
			}
			this.fairySprite.alpha = 0f;
			this._uiBalloon.alpha = 0f;
			this.collider.set_enabled(false);
		}

		private void Awake()
		{
			this.Startup();
		}

		private void ReqFairyType(UIStartupFairy.FairyType iType)
		{
			if (this._iType == iType)
			{
				return;
			}
			this._iType = iType;
			this._iState = (this._iStatePrev = UIStartupFairy.FairyState.Idle);
			this._nSpriteIndex = 0;
			this.SetSprite(this._iType, this._iState, this._nSpriteIndex);
			this._listObserverStream.ForEach(delegate(IDisposable x)
			{
				if (x != null)
				{
					x.Dispose();
				}
			});
		}

		private void SetSprite(UIStartupFairy.FairyType iType, UIStartupFairy.FairyState iState, int nIndex)
		{
			this.fairySprite.spriteName = ((iType != UIStartupFairy.FairyType.None) ? string.Format("mini{0}_{1}_{2:D2}", (int)iType, iState.ToString(), nIndex) : string.Empty);
			this.fairySprite.localSize = this._listFairyAnimationParameter.get_Item((int)iType).baseSpriteSize;
		}

		private void ReqFairyState(UIStartupFairy.FairyState iState)
		{
			if (this._iType == UIStartupFairy.FairyType.None)
			{
				return;
			}
			if (this._listFairyAnimationParameter.get_Item((int)this._iType).GetStateParam(iState).spriteNum <= 0)
			{
				return;
			}
			this._nSpriteIndex = 0;
			this._listObserverStream.ForEach(delegate(IDisposable x)
			{
				if (x != null)
				{
					x.Dispose();
				}
			});
			this._listObserverStream.set_Item((int)iState, (from x in Observable.IntervalFrame(this._listFairyAnimationParameter.get_Item((int)this._iType).GetStateParam(iState).drawIntervalFrame, FrameCountType.Update)
			select this._nSpriteIndex++).Subscribe(delegate(int index)
			{
				int num = index % this._listFairyAnimationParameter.get_Item((int)this._iType).GetStateParam(iState).spriteNum;
				int nIndex = this._listFairyAnimationParameter.get_Item((int)this._iType).GetStateParam(iState).convertSpriteNumber[num];
				this.SetSprite(this._iType, iState, nIndex);
			}).AddTo(base.get_gameObject()));
			this._iStatePrev = this._iState;
			this._iState = iState;
			switch (iState)
			{
			case UIStartupFairy.FairyState.Move:
				if (this._iStatePrev != UIStartupFairy.FairyState.Jump)
				{
					float tripTime = this._listFairyAnimationParameter.get_Item((int)this._iType).tripTime;
					float rotTime = this._listFairyAnimationParameter.get_Item((int)this._iType).rotTime;
					float delayTime = tripTime * 2f;
					base.get_transform().LTCancel();
					base.get_transform().LTDelayedCall(delayTime, delegate
					{
						this.get_transform().LTRotateLocal(Vector3.get_zero(), rotTime).setEase(LeanTweenType.easeOutSine);
						this.get_transform().LTMoveLocalX(this._listFairyAnimationParameter.get_Item((int)this._iType).endPos.x, tripTime).setEase(LeanTweenType.linear);
						this.get_transform().LTRotateLocal(Vector3.get_up() * 180f, rotTime).setDelay(tripTime).setEase(LeanTweenType.easeOutSine);
						this.get_transform().LTMoveLocalX(this._listFairyAnimationParameter.get_Item((int)this._iType).startPos.x, tripTime).setDelay(tripTime).setEase(LeanTweenType.linear);
					}).setOnCompleteOnStart(true).setLoopClamp();
				}
				break;
			case UIStartupFairy.FairyState.Jump:
				Observable.TimerFrame(this._listFairyAnimationParameter.get_Item((int)this._iType).jumpFrame, FrameCountType.Update).Subscribe(delegate(long _)
				{
					this.ReqFairyState(this._iStatePrev);
				});
				break;
			}
		}

		public void Show(UIStartupFairy.FairyType iType, UIStartupFairy.FairyState iState, Action onFinished)
		{
			this.ReqFairyType(iType);
			base.get_transform().set_localPosition(this._listFairyAnimationParameter.get_Item((int)iType).startPos);
			this.PreparaAnimation(true, delegate
			{
				this.collider.set_enabled(true);
				this.ReqFairyState(iState);
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			this.PreparaAnimation(false, delegate
			{
				this._uiBalloon.alpha = 0f;
				Dlg.Call(ref onFinished);
			});
		}

		private void PreparaAnimation(bool isFoward, Action onFinished)
		{
			float to = (!isFoward) ? 0f : 1f;
			base.get_transform().LTValue(this.fairySprite.alpha, to, this._strParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.fairySprite.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void ShowBaloon(int nPage, Tuple<Vector3, Vector3> vBalloon, Action onFinished)
		{
			this._uiBalloon.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Startup/PictureStoryShow/info{0}_fuki", nPage + 1));
			this._uiBalloon.localSize = vBalloon.get_Item1();
			this._uiBalloon.get_transform().set_localPosition(vBalloon.get_Item2());
			this._uiBalloon.alpha = 0f;
			this._uiBalloon.get_transform().LTValue(this._uiBalloon.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiBalloon.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public AudioSource PlayVoice(int nPage, Action onFinished)
		{
			switch (nPage)
			{
			case 0:
				return this._asSource = Utils.PlayDescriptionVoice(25, onFinished);
			case 1:
				return this._asSource = Utils.PlayDescriptionVoice(28, onFinished);
			case 2:
				return this._asSource = Utils.PlayDescriptionVoice(31, onFinished);
			default:
				return null;
			}
		}

		public void StopVoice()
		{
			ShipUtils.StopShipVoice(this._asSource, false, 0.25f);
		}

		public void ImmediateIdle()
		{
			this.collider.set_enabled(false);
			base.get_transform().LTCancel();
			base.get_transform().LTMoveLocal(this._listFairyAnimationParameter.get_Item((int)this._iType).startPos, 0.15f).setEase(LeanTweenType.easeOutSine);
			base.get_transform().LTRotateLocal(Vector3.get_zero(), 0.15f).setEase(LeanTweenType.easeOutSine);
			this.ReqFairyState(UIStartupFairy.FairyState.Idle);
		}

		private void OnPress(bool isPress)
		{
			if (isPress)
			{
				if (this._iType == UIStartupFairy.FairyType.Ahoge || this._iType == UIStartupFairy.FairyType.Braid)
				{
					return;
				}
				if (this._iState == UIStartupFairy.FairyState.Jump)
				{
					return;
				}
				this.ReqFairyState(UIStartupFairy.FairyState.Jump);
			}
		}
	}
}
