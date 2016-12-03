using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIYouseiSwitch : MonoBehaviour
	{
		public enum ActionType
		{
			OFF,
			ON
		}

		public delegate void UIYouseiSwitchAction(UIYouseiSwitch.ActionType actionType);

		private UIYouseiSwitch.UIYouseiSwitchAction mYouseiSwitchActionCallBack;

		[SerializeField]
		private UISprite mSprite_Background;

		[SerializeField]
		private UISprite mSprite_Thumb;

		[SerializeField]
		private UISpriteAnimation mSpriteAnimation_Yousei;

		[SerializeField]
		private ParticleSystem mSleepParticle;

		[SerializeField]
		public bool Enabled = true;

		private bool mSwitchFlag;

		private int THUMB_POS_X_OFF;

		private int THUMB_POS_X_ON = 128;

		private Vector3 SWITCH_ON_YOUSEI_HIDE_POS = new Vector3(-50f, -45f, 0f);

		private Vector3 SWITCH_ON_YOUSEI_STAND_POS = new Vector3(-50f, -20f, 0f);

		private Vector3 SWITCH_OFF_YOUSEI_STAND_POS = new Vector3(46f, -59f, 0f);

		private Vector3 SWITCH_OFF_YOUSEI_HIDE_POS = new Vector3(46f, -85f, 0f);

		private IEnumerator mAnimationCoroutine;

		public void SetYouseiSwitchActionCallBack(UIYouseiSwitch.UIYouseiSwitchAction callBack)
		{
			this.mYouseiSwitchActionCallBack = callBack;
		}

		private void Start()
		{
			this.mSleepParticle.SetActive(true);
			this.mSleepParticle.Play();
		}

		private void Update()
		{
			if (Input.GetKeyUp(116))
			{
				this.ClickSwitch();
			}
		}

		public void ClickSwitch()
		{
			if (this.Enabled)
			{
				this.OnClickSwitch();
			}
		}

		private void OnClickSwitch()
		{
			if (this.mAnimationCoroutine == null)
			{
				if (this.mSwitchFlag)
				{
					this.mSleepParticle.SetActive(true);
					this.mSleepParticle.Play();
					this.ClickSwitchOff();
				}
				else
				{
					this.mSleepParticle.Stop();
					this.mSleepParticle.SetActive(false);
					this.ClickSwitchOn();
				}
				this.mSwitchFlag = !this.mSwitchFlag;
				if (this.mSwitchFlag)
				{
					this.OnCallBack(UIYouseiSwitch.ActionType.ON);
				}
				else
				{
					this.OnCallBack(UIYouseiSwitch.ActionType.OFF);
				}
			}
		}

		private void OnCallBack(UIYouseiSwitch.ActionType actionType)
		{
			if (this.mYouseiSwitchActionCallBack != null)
			{
				this.mYouseiSwitchActionCallBack(actionType);
			}
		}

		private void ClickSwitchOn()
		{
			this.mSpriteAnimation_Yousei.get_gameObject().get_transform().set_localPosition(this.SWITCH_ON_YOUSEI_HIDE_POS);
			this.mSpriteAnimation_Yousei.namePrefix = "mini_05_b_0";
			this.mSpriteAnimation_Yousei.framesPerSecond = 2;
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			this.mAnimationCoroutine = this.ClickSwitchOnCoroutine(delegate
			{
				this.mAnimationCoroutine = null;
			});
			base.StartCoroutine(this.mAnimationCoroutine);
		}

		private void ClickSwitchOff()
		{
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			this.mAnimationCoroutine = this.ClickSwitchOffCoroutine(delegate
			{
				this.mSpriteAnimation_Yousei.namePrefix = "mini_05_a_0";
				this.mSpriteAnimation_Yousei.framesPerSecond = 1;
				this.mAnimationCoroutine = null;
			});
			base.StartCoroutine(this.mAnimationCoroutine);
		}

		[DebuggerHidden]
		private IEnumerator ClickSwitchOnCoroutine(Action finished)
		{
			UIYouseiSwitch.<ClickSwitchOnCoroutine>c__Iterator1C3 <ClickSwitchOnCoroutine>c__Iterator1C = new UIYouseiSwitch.<ClickSwitchOnCoroutine>c__Iterator1C3();
			<ClickSwitchOnCoroutine>c__Iterator1C.finished = finished;
			<ClickSwitchOnCoroutine>c__Iterator1C.<$>finished = finished;
			<ClickSwitchOnCoroutine>c__Iterator1C.<>f__this = this;
			return <ClickSwitchOnCoroutine>c__Iterator1C;
		}

		[DebuggerHidden]
		private IEnumerator ClickSwitchOffCoroutine(Action finished)
		{
			UIYouseiSwitch.<ClickSwitchOffCoroutine>c__Iterator1C4 <ClickSwitchOffCoroutine>c__Iterator1C = new UIYouseiSwitch.<ClickSwitchOffCoroutine>c__Iterator1C4();
			<ClickSwitchOffCoroutine>c__Iterator1C.finished = finished;
			<ClickSwitchOffCoroutine>c__Iterator1C.<$>finished = finished;
			<ClickSwitchOffCoroutine>c__Iterator1C.<>f__this = this;
			return <ClickSwitchOffCoroutine>c__Iterator1C;
		}

		private void OnDestroy()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mYouseiSwitchActionCallBack = null;
			this.mSprite_Background = null;
			this.mSprite_Thumb = null;
			this.mSpriteAnimation_Yousei = null;
			this.mSleepParticle = null;
		}
	}
}
