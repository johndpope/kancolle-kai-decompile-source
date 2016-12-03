using Common.Enum;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget))]
	public class UICompassGirl : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiCompassGirl;

		private UIWidget _uiWidget;

		private CompassType _iCompassType;

		private Action<string> _actOnCompassGirlMessage;

		private Action<UICompass.Power> _actOnStopRollCompass;

		private UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiCompassGirl);
			Mem.Del<UIWidget>(ref this._uiWidget);
			Mem.Del<CompassType>(ref this._iCompassType);
			Mem.Del<Action<string>>(ref this._actOnCompassGirlMessage);
			Mem.Del<Action<UICompass.Power>>(ref this._actOnStopRollCompass);
		}

		public bool Init(Action<string> onCompassGirlMessage, Action<UICompass.Power> onStopRollCompass, CompassType iRashinType)
		{
			this._actOnCompassGirlMessage = onCompassGirlMessage;
			this._actOnStopRollCompass = onStopRollCompass;
			this.widget.alpha = 0f;
			base.get_transform().LTCancel();
			this._uiCompassGirl.get_transform().LTCancel();
			this._iCompassType = iRashinType;
			switch (this._iCompassType)
			{
			case CompassType.Stupid:
				this.InitStupid();
				break;
			case CompassType.Normal:
				this.InitNormal();
				break;
			case CompassType.Super:
				this.InitSuper();
				break;
			case CompassType.Wizard:
				this.InitWizard();
				break;
			}
			return true;
		}

		private bool InitStupid()
		{
			this._uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_01-1");
			this._uiCompassGirl.SetDimensions(112, 146);
			this._uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitNormal()
		{
			this._uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_02-1");
			this._uiCompassGirl.SetDimensions(115, 164);
			this._uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitSuper()
		{
			this._uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_03-2");
			this._uiCompassGirl.SetDimensions(115, 164);
			this._uiCompassGirl.alpha = 0f;
			return true;
		}

		private bool InitWizard()
		{
			this._uiCompassGirl.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/CompassGirls/CompassGirl_04-1");
			this._uiCompassGirl.SetDimensions(300, 190);
			this._uiCompassGirl.alpha = 0f;
			return true;
		}

		public void Play()
		{
			this.widget.alpha = 1f;
			IObservable<Unit> source = Observable.FromCoroutine(new Func<IEnumerator>(this.InDisplay), false);
			IObservable<Unit> source2 = Observable.FromCoroutine(new Func<IEnumerator>(this.WaitRoll), false);
			IObservable<Unit> observable = Observable.FromCoroutine(new Func<IEnumerator>(this.StartRoll), false);
			source.SelectMany(source2.SelectMany(new Func<IEnumerator>(this.StartRoll), false)).Subscribe<Unit>();
		}

		public void Hide()
		{
			this.OutDisplay();
		}

		[DebuggerHidden]
		private IEnumerator InDisplay()
		{
			UICompassGirl.<InDisplay>c__Iterator127 <InDisplay>c__Iterator = new UICompassGirl.<InDisplay>c__Iterator127();
			<InDisplay>c__Iterator.<>f__this = this;
			return <InDisplay>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator InDisplayFadeInFromTop()
		{
			UICompassGirl.<InDisplayFadeInFromTop>c__Iterator128 <InDisplayFadeInFromTop>c__Iterator = new UICompassGirl.<InDisplayFadeInFromTop>c__Iterator128();
			<InDisplayFadeInFromTop>c__Iterator.<>f__this = this;
			return <InDisplayFadeInFromTop>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator InDisplayFadeInFromBottom()
		{
			UICompassGirl.<InDisplayFadeInFromBottom>c__Iterator129 <InDisplayFadeInFromBottom>c__Iterator = new UICompassGirl.<InDisplayFadeInFromBottom>c__Iterator129();
			<InDisplayFadeInFromBottom>c__Iterator.<>f__this = this;
			return <InDisplayFadeInFromBottom>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator InDisplayFadeInFromTopRight()
		{
			UICompassGirl.<InDisplayFadeInFromTopRight>c__Iterator12A <InDisplayFadeInFromTopRight>c__Iterator12A = new UICompassGirl.<InDisplayFadeInFromTopRight>c__Iterator12A();
			<InDisplayFadeInFromTopRight>c__Iterator12A.<>f__this = this;
			return <InDisplayFadeInFromTopRight>c__Iterator12A;
		}

		[DebuggerHidden]
		private IEnumerator WaitRoll()
		{
			UICompassGirl.<WaitRoll>c__Iterator12B <WaitRoll>c__Iterator12B = new UICompassGirl.<WaitRoll>c__Iterator12B();
			<WaitRoll>c__Iterator12B.<>f__this = this;
			return <WaitRoll>c__Iterator12B;
		}

		[DebuggerHidden]
		private IEnumerator StartRoll()
		{
			UICompassGirl.<StartRoll>c__Iterator12C <StartRoll>c__Iterator12C = new UICompassGirl.<StartRoll>c__Iterator12C();
			<StartRoll>c__Iterator12C.<>f__this = this;
			return <StartRoll>c__Iterator12C;
		}

		[DebuggerHidden]
		private IEnumerator StartRollMajo()
		{
			UICompassGirl.<StartRollMajo>c__Iterator12D <StartRollMajo>c__Iterator12D = new UICompassGirl.<StartRollMajo>c__Iterator12D();
			<StartRollMajo>c__Iterator12D.<>f__this = this;
			return <StartRollMajo>c__Iterator12D;
		}

		[DebuggerHidden]
		private IEnumerator StartRollBob()
		{
			UICompassGirl.<StartRollBob>c__Iterator12E <StartRollBob>c__Iterator12E = new UICompassGirl.<StartRollBob>c__Iterator12E();
			<StartRollBob>c__Iterator12E.<>f__this = this;
			return <StartRollBob>c__Iterator12E;
		}

		[DebuggerHidden]
		private IEnumerator StartRollDal()
		{
			UICompassGirl.<StartRollDal>c__Iterator12F <StartRollDal>c__Iterator12F = new UICompassGirl.<StartRollDal>c__Iterator12F();
			<StartRollDal>c__Iterator12F.<>f__this = this;
			return <StartRollDal>c__Iterator12F;
		}

		[DebuggerHidden]
		private IEnumerator StartRollPony()
		{
			UICompassGirl.<StartRollPony>c__Iterator130 <StartRollPony>c__Iterator = new UICompassGirl.<StartRollPony>c__Iterator130();
			<StartRollPony>c__Iterator.<>f__this = this;
			return <StartRollPony>c__Iterator;
		}

		private void Shake()
		{
			int count = 15;
			Vector3 originPos = this._uiCompassGirl.get_transform().get_localPosition();
			Observable.IntervalFrame(1, FrameCountType.Update).Take(count).Subscribe(delegate(long x)
			{
				float num = Mathe.Rate(0f, 15f, (float)x);
				this._uiCompassGirl.get_transform().set_localPosition(new Vector3(originPos.x + XorRandom.GetF11() * 5f * num, originPos.y + XorRandom.GetF11() * 5f * num, 0f));
			}, delegate
			{
				this._uiCompassGirl.get_transform().set_localPosition(originPos);
			}).AddTo(base.get_gameObject());
		}

		private void OutDisplay()
		{
			switch (this._iCompassType)
			{
			case CompassType.Stupid:
				this.OutDisplayFadeOutToBottom();
				break;
			case CompassType.Normal:
				this.OutDisplayFadeOut();
				break;
			case CompassType.Super:
				this.OutDisplayFadeOutToTop();
				break;
			case CompassType.Wizard:
				this.OutDisplayFadeOutToTopRight();
				break;
			}
		}

		private void OutDisplayFadeOutToTop()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			this._uiCompassGirl.get_transform().LTValue(this._uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				this._uiCompassGirl.alpha = x;
			});
			this._uiCompassGirl.get_transform().LTMoveLocalY(30f, time).setEase(ease);
		}

		private void OutDisplayFadeOut()
		{
			this._uiCompassGirl.get_transform().LTValue(this._uiCompassGirl.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this._uiCompassGirl.alpha = x;
			});
		}

		private void OutDisplayFadeOutToBottom()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			this._uiCompassGirl.get_transform().LTValue(this._uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				this._uiCompassGirl.alpha = x;
			});
			this._uiCompassGirl.get_transform().LTMoveLocalY(-30f, time).setEase(ease);
		}

		private void OutDisplayFadeOutToTopRight()
		{
			float time = 0.5f;
			LeanTweenType ease = LeanTweenType.easeOutQuad;
			this._uiCompassGirl.get_transform().LTValue(this._uiCompassGirl.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				this._uiCompassGirl.alpha = x;
			});
			this._uiCompassGirl.get_transform().LTMoveLocal(new Vector3(this._uiCompassGirl.get_transform().get_localPosition().x + 30f, this._uiCompassGirl.get_transform().get_localPosition().y + 30f, 0f), time).setEase(ease);
		}

		private void OnCompassGirlMessage(string message)
		{
			Dlg.Call<string>(ref this._actOnCompassGirlMessage, message);
		}

		private void OnStopRollCompass(UICompass.Power power)
		{
			Dlg.Call<UICompass.Power>(ref this._actOnStopRollCompass, power);
		}
	}
}
