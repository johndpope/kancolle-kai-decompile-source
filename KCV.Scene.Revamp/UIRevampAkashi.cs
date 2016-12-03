using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRevampAkashi : MonoBehaviour
	{
		public enum CharacterType
		{
			NONE,
			Akashi,
			AkashiKai
		}

		public enum BodyType
		{
			Normal,
			Making
		}

		[SerializeField]
		private UITexture mTexture_Body;

		[SerializeField]
		private UITexture mTexture_Eye;

		[SerializeField]
		private Texture mTexture2d_Eye_00;

		[SerializeField]
		private Texture mTexture2d_Eye_01;

		[SerializeField]
		private Texture mTexture2d_Eye_02;

		[SerializeField]
		private Texture mTexture2d_Eye_03;

		[SerializeField]
		private Texture mTexture2d_Eye_04;

		private Texture mTexture2d_Body_Normal;

		private Texture mTexture2d_Body_Making;

		private Coroutine mAnimationCoroutine;

		private UIWidget mWidgetThis;

		private bool isAnimation;

		private UIRevampAkashi.CharacterType mCharacterType;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0f;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Body, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Eye, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Eye_00, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Eye_01, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Eye_02, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Eye_03, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Eye_04, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Body_Normal, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Body_Making, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			this.mAnimationCoroutine = null;
		}

		private void Update()
		{
			if (Input.GetKeyUp(113))
			{
				this.ChangeBodyTo(UIRevampAkashi.BodyType.Making);
			}
			else if (Input.GetKeyUp(119))
			{
				this.ChangeBodyTo(UIRevampAkashi.BodyType.Normal);
			}
			if (this.mCharacterType != UIRevampAkashi.CharacterType.NONE)
			{
				this.RandomWait(delegate
				{
					bool flag = Random.Range(0, 2) != 1;
					bool flag2 = 20 > Random.Range(0, 100);
					if (flag)
					{
						if (flag2)
						{
							this.BlinkWithMove(delegate
							{
								this.isAnimation = false;
							});
						}
						else
						{
							this.Blink(delegate
							{
								this.isAnimation = false;
							});
						}
					}
					else if (flag2)
					{
						this.Blink(delegate
						{
							this.BlinkWithMove(delegate
							{
								this.isAnimation = false;
							});
						});
					}
					else
					{
						this.DoubleBlink(delegate
						{
							this.isAnimation = false;
						});
					}
				});
			}
		}

		public void Initialize(UIRevampAkashi.CharacterType type)
		{
			this.mCharacterType = type;
			UIRevampAkashi.CharacterType characterType = this.mCharacterType;
			if (characterType != UIRevampAkashi.CharacterType.Akashi)
			{
				if (characterType == UIRevampAkashi.CharacterType.AkashiKai)
				{
					this.mTexture2d_Body_Normal = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashikai");
					this.mTexture2d_Body_Making = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashikai_making");
					this.mTexture_Body.mainTexture = this.mTexture2d_Body_Normal;
					this.mTexture_Eye.mainTexture = this.mTexture2d_Eye_00;
				}
			}
			else
			{
				this.mTexture2d_Body_Normal = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashi");
				this.mTexture2d_Body_Making = Resources.Load<Texture>("Textures/ImprovementArsenal/RepairShip/akashi_making");
				this.mTexture_Body.mainTexture = this.mTexture2d_Body_Normal;
				this.mTexture_Eye.mainTexture = this.mTexture2d_Eye_00;
			}
			this.mWidgetThis.alpha = 1f;
		}

		public void RandomWait(Action callBack)
		{
			if (this.mAnimationCoroutine == null)
			{
				this.mAnimationCoroutine = base.StartCoroutine(this.RandomWaitCoroutine(delegate
				{
					this.mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack.Invoke();
					}
				}));
			}
		}

		private void Blink(Action callBack)
		{
			if (this.mAnimationCoroutine == null)
			{
				this.mAnimationCoroutine = base.StartCoroutine(this.BlinkCoroutine(delegate
				{
					this.mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack.Invoke();
					}
				}));
			}
		}

		private void DoubleBlink(Action callBack)
		{
			if (this.mAnimationCoroutine == null)
			{
				this.mAnimationCoroutine = base.StartCoroutine(this.DoubleBlinkCoroutine(delegate
				{
					this.mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack.Invoke();
					}
				}));
			}
		}

		private void BlinkWithMove(Action callBack)
		{
			if (this.mAnimationCoroutine == null)
			{
				this.mAnimationCoroutine = base.StartCoroutine(this.BlinkWithMoveCoroutine(delegate
				{
					this.mAnimationCoroutine = null;
					if (callBack != null)
					{
						callBack.Invoke();
					}
				}));
			}
		}

		public void ChangeBodyTo(UIRevampAkashi.BodyType bodyType)
		{
			if (bodyType != UIRevampAkashi.BodyType.Normal)
			{
				if (bodyType == UIRevampAkashi.BodyType.Making)
				{
					this.mTexture_Body.mainTexture = this.mTexture2d_Body_Making;
				}
			}
			else
			{
				this.mTexture_Body.mainTexture = this.mTexture2d_Body_Normal;
			}
		}

		[DebuggerHidden]
		private IEnumerator BlinkWithMoveCoroutine(Action finished)
		{
			UIRevampAkashi.<BlinkWithMoveCoroutine>c__IteratorC0 <BlinkWithMoveCoroutine>c__IteratorC = new UIRevampAkashi.<BlinkWithMoveCoroutine>c__IteratorC0();
			<BlinkWithMoveCoroutine>c__IteratorC.finished = finished;
			<BlinkWithMoveCoroutine>c__IteratorC.<$>finished = finished;
			<BlinkWithMoveCoroutine>c__IteratorC.<>f__this = this;
			return <BlinkWithMoveCoroutine>c__IteratorC;
		}

		[DebuggerHidden]
		private IEnumerator BlinkCoroutine(Action finished)
		{
			UIRevampAkashi.<BlinkCoroutine>c__IteratorC1 <BlinkCoroutine>c__IteratorC = new UIRevampAkashi.<BlinkCoroutine>c__IteratorC1();
			<BlinkCoroutine>c__IteratorC.finished = finished;
			<BlinkCoroutine>c__IteratorC.<$>finished = finished;
			<BlinkCoroutine>c__IteratorC.<>f__this = this;
			return <BlinkCoroutine>c__IteratorC;
		}

		[DebuggerHidden]
		private IEnumerator DoubleBlinkCoroutine(Action finished)
		{
			UIRevampAkashi.<DoubleBlinkCoroutine>c__IteratorC2 <DoubleBlinkCoroutine>c__IteratorC = new UIRevampAkashi.<DoubleBlinkCoroutine>c__IteratorC2();
			<DoubleBlinkCoroutine>c__IteratorC.finished = finished;
			<DoubleBlinkCoroutine>c__IteratorC.<$>finished = finished;
			<DoubleBlinkCoroutine>c__IteratorC.<>f__this = this;
			return <DoubleBlinkCoroutine>c__IteratorC;
		}

		[DebuggerHidden]
		private IEnumerator RandomWaitCoroutine(Action finished)
		{
			UIRevampAkashi.<RandomWaitCoroutine>c__IteratorC3 <RandomWaitCoroutine>c__IteratorC = new UIRevampAkashi.<RandomWaitCoroutine>c__IteratorC3();
			<RandomWaitCoroutine>c__IteratorC.finished = finished;
			<RandomWaitCoroutine>c__IteratorC.<$>finished = finished;
			return <RandomWaitCoroutine>c__IteratorC;
		}
	}
}
