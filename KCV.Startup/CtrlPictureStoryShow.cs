using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlPictureStoryShow : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			[Header("[Fairy Properties]")]
			public List<Vector3> fairyPos;

			public List<Vector3> fairySize;

			public List<Vector3> fairyBalloonPos;

			public List<Vector3> fairyBalloonSize;

			public float showBalloonTime;

			public float frameAnimationInterval;

			[Header("[Description Properties]")]
			public List<Vector3> descriptionPos;

			public List<Vector3> descriptionSize;

			[Header("[MessageWindow Properties]")]
			public List<Vector3> messageWindowPos;

			public float showHideMessageWindowTime;

			public void Dispose()
			{
				Mem.DelListSafe<Vector3>(ref this.fairyPos);
				Mem.DelListSafe<Vector3>(ref this.fairySize);
				Mem.DelListSafe<Vector3>(ref this.fairyBalloonPos);
				Mem.DelListSafe<Vector3>(ref this.fairyBalloonSize);
				Mem.Del<float>(ref this.showBalloonTime);
				Mem.Del<float>(ref this.frameAnimationInterval);
				Mem.DelListSafe<Vector3>(ref this.descriptionPos);
				Mem.DelListSafe<Vector3>(ref this.descriptionSize);
				Mem.DelListSafe<Vector3>(ref this.messageWindowPos);
				Mem.Del<float>(ref this.showHideMessageWindowTime);
			}
		}

		[Serializable]
		private class UIFairy : IDisposable
		{
			[Serializable]
			private struct FairyTexture : IDisposable
			{
				public Texture2D item1;

				public Texture2D item2;

				public void Dispose()
				{
					Mem.Del<Texture2D>(ref this.item1);
					Mem.Del<Texture2D>(ref this.item2);
				}
			}

			[SerializeField]
			private UITexture _uiFairy;

			[SerializeField]
			private UITexture _uiBalloon;

			[SerializeField]
			private List<CtrlPictureStoryShow.UIFairy.FairyTexture> _listFairyTexture;

			private IDisposable _disAnimation;

			public UITexture fairy
			{
				get
				{
					return this._uiFairy;
				}
			}

			public bool Init()
			{
				UIWidget arg_19_0 = this._uiFairy;
				float alpha = 0f;
				this._uiBalloon.alpha = alpha;
				arg_19_0.alpha = alpha;
				return true;
			}

			public void Dispose()
			{
				Mem.Del<UITexture>(ref this._uiFairy);
				Mem.Del<UITexture>(ref this._uiBalloon);
				if (this._listFairyTexture != null)
				{
					this._listFairyTexture.ForEach(delegate(CtrlPictureStoryShow.UIFairy.FairyTexture x)
					{
						x.Dispose();
					});
				}
				Mem.DelListSafe<CtrlPictureStoryShow.UIFairy.FairyTexture>(ref this._listFairyTexture);
				Mem.DelIDisposableSafe<IDisposable>(ref this._disAnimation);
			}

			public void SetFairy(int nPage, Tuple<Vector3, Vector3> vFairy, Tuple<Vector3, Vector3> vBalloon)
			{
				this._uiFairy.mainTexture = this._listFairyTexture.get_Item(nPage).item1;
				this._uiFairy.localSize = vFairy.get_Item1();
				this._uiFairy.get_transform().set_localPosition(vFairy.get_Item2());
				this._uiFairy.alpha = 1f;
				this._uiBalloon.mainTexture = Resources.Load<Texture2D>(string.Format("Textures/Startup/PictureStoryShow/info{0}_fuki", nPage + 1));
				this._uiBalloon.localSize = vBalloon.get_Item1();
				this._uiBalloon.get_transform().set_localPosition(vBalloon.get_Item2());
				this._uiBalloon.alpha = 0f;
			}

			public void PlayFairyAnimation(int nPage, float fFrameTime)
			{
				if (this._disAnimation != null)
				{
					this._disAnimation.Dispose();
				}
				bool isFoward = false;
				this._disAnimation = Observable.Interval(TimeSpan.FromSeconds((double)fFrameTime)).Subscribe(delegate(long _)
				{
					this._uiFairy.mainTexture = ((!isFoward) ? this._listFairyTexture.get_Item(nPage).item2 : this._listFairyTexture.get_Item(nPage).item1);
					isFoward = !isFoward;
				});
			}

			public LTDescr ShowBalloon(float fShowTime)
			{
				return this._uiBalloon.get_transform().LTValue(this._uiBalloon.alpha, 1f, fShowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiBalloon.alpha = x;
				});
			}

			public AudioSource PlayVoice(int nPage, Action onFinished)
			{
				switch (nPage)
				{
				case 0:
					return Utils.PlayDescriptionVoice(25, onFinished);
				case 1:
					return Utils.PlayDescriptionVoice(28, onFinished);
				case 2:
					return Utils.PlayDescriptionVoice(31, onFinished);
				default:
					return null;
				}
			}

			public void StopVoice()
			{
				ShipUtils.StopShipVoice();
			}
		}

		[Serializable]
		private class UISheet : IDisposable
		{
			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UITexture _uiDescription;

			[Header("[Animation Properties]"), SerializeField]
			private Vector3 _vStartPos = Vector3.get_zero();

			[SerializeField]
			private float _fShowTime = 1f;

			[SerializeField]
			private LeanTweenType _iShowTweenType = LeanTweenType.linear;

			public Transform transform
			{
				get
				{
					return this._uiBackground.get_transform();
				}
			}

			public bool Init()
			{
				this.transform.set_localPosition(this._vStartPos);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<UITexture>(ref this._uiBackground);
				Mem.Del<UITexture>(ref this._uiDescription);
			}

			public void Show(Action onFinished)
			{
				this.transform.LTMoveLocal(Vector3.get_zero(), this._fShowTime).setEase(this._iShowTweenType).setOnComplete(onFinished);
			}
		}

		private const int PAGE_MAX_NUM = 3;

		[SerializeField]
		private List<CtrlPictureStoryShow.UISheet> _listSheets;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiDescription;

		[SerializeField]
		private UIButton _uiGearButton;

		[SerializeField]
		private UIStartupFairy _uiStartupFairy;

		[Header("[Animation Parameters]"), SerializeField]
		private CtrlPictureStoryShow.Params _strParams;

		private int _nNowPage;

		private UIPanel _uiPanel;

		private Transform _traPartnerShip;

		private Action _actOnFinished;

		private List<Texture2D> _listDescriptionTex;

		private StatementMachine _clsState;

		public int nowPage
		{
			get
			{
				return this._nNowPage;
			}
			private set
			{
				this._nNowPage = value;
			}
		}

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static CtrlPictureStoryShow Instantiate(CtrlPictureStoryShow prefab, Transform parent, Action onFinished)
		{
			CtrlPictureStoryShow ctrlPictureStoryShow = Object.Instantiate<CtrlPictureStoryShow>(prefab);
			ctrlPictureStoryShow.get_transform().set_parent(parent);
			ctrlPictureStoryShow.get_transform().localScaleOne();
			ctrlPictureStoryShow.get_transform().localPositionZero();
			return ctrlPictureStoryShow.VitualCtor(onFinished);
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiDescription);
			Mem.Del<UIButton>(ref this._uiGearButton);
			Mem.Del<UIStartupFairy>(ref this._uiStartupFairy);
			Mem.DelIDisposable<CtrlPictureStoryShow.Params>(ref this._strParams);
			Mem.Del<int>(ref this._nNowPage);
			Mem.Del<Action>(ref this._actOnFinished);
			Mem.DelListSafe<Texture2D>(ref this._listDescriptionTex);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
		}

		private CtrlPictureStoryShow VitualCtor(Action onFinished)
		{
			this.nowPage = 0;
			this._actOnFinished = onFinished;
			this._uiBackground.alpha = 0f;
			this._uiDescription.alpha = 0f;
			this._uiGearButton.GetComponent<UISprite>().alpha = 0f;
			this._uiGearButton.set_enabled(false);
			this._traPartnerShip = StartupTaskManager.GetPartnerSelect().get_transform();
			this._uiStartupFairy.Startup();
			this._listSheets.ForEach(delegate(CtrlPictureStoryShow.UISheet x)
			{
				x.Init();
			});
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayPictureStoryShow(observer)).Subscribe(delegate(bool _)
			{
				this.HidePictureStoryShow(new Action(this.OnFinished));
			});
			return this;
		}

		[DebuggerHidden]
		private IEnumerator PlayPictureStoryShow(IObserver<bool> observer)
		{
			CtrlPictureStoryShow.<PlayPictureStoryShow>c__Iterator133 <PlayPictureStoryShow>c__Iterator = new CtrlPictureStoryShow.<PlayPictureStoryShow>c__Iterator133();
			<PlayPictureStoryShow>c__Iterator.observer = observer;
			<PlayPictureStoryShow>c__Iterator.<$>observer = observer;
			<PlayPictureStoryShow>c__Iterator.<>f__this = this;
			return <PlayPictureStoryShow>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator PlayDesctiption()
		{
			CtrlPictureStoryShow.<PlayDesctiption>c__Iterator134 <PlayDesctiption>c__Iterator = new CtrlPictureStoryShow.<PlayDesctiption>c__Iterator134();
			<PlayDesctiption>c__Iterator.<>f__this = this;
			return <PlayDesctiption>c__Iterator;
		}

		private void PlayDescriotionVoice(int nPage)
		{
			List<int> voiceNum = new List<int>();
			switch (nPage)
			{
			case 0:
				voiceNum.Add(23);
				voiceNum.Add(24);
				break;
			case 1:
				voiceNum.Add(26);
				voiceNum.Add(27);
				break;
			case 2:
				voiceNum.Add(29);
				voiceNum.Add(30);
				break;
			}
			Utils.PlayDescriptionVoice(voiceNum.get_Item(0), delegate
			{
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
				{
					Utils.PlayDescriptionVoice(voiceNum.get_Item(1), null);
				});
			});
		}

		private void ShowPictureStoryShow(Action onFinished)
		{
			UISprite gear = this._uiGearButton.GetComponent<UISprite>();
			this._uiBackground.get_transform().LTValue(0f, 1f, this._strParams.showHideMessageWindowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiBackground.alpha = x;
				this._uiDescription.alpha = x;
				gear.alpha = x;
			}).setOnComplete(delegate
			{
				this._uiGearButton.set_enabled(true);
				Dlg.Call(ref onFinished);
			});
		}

		private void HidePictureStoryShow(Action onFinished)
		{
			UISprite gear = this._uiGearButton.GetComponent<UISprite>();
			this._uiGearButton.set_enabled(false);
			this._uiBackground.get_transform().LTValue(1f, 0f, this._strParams.showHideMessageWindowTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				gear.alpha = x;
			}).setOnStart(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void OnFinished()
		{
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
