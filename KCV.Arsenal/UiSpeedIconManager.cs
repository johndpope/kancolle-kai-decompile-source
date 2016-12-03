using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiSpeedIconManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite _highSpeedIcon;

		[SerializeField]
		private UISprite _highSpeedBar;

		[SerializeField]
		private UISprite _selectFrame;

		[SerializeField]
		private ParticleSystem _par;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private UILabel _nowItemNum;

		[SerializeField]
		private UILabel _nextItemNum;

		private bool _isAnimate;

		private bool _isLarge;

		public bool IsHigh;

		public bool init()
		{
			Util.FindParentToChild<UISprite>(ref this._highSpeedIcon, base.get_transform(), "HighBase/IconPanel/HighIcon");
			Util.FindParentToChild<UISprite>(ref this._highSpeedBar, base.get_transform(), "HighBase/IconPanel/HighBar");
			Util.FindParentToChild<UISprite>(ref this._selectFrame, base.get_transform(), "HighBase/IconPanel/FrameHigh");
			Util.FindParentToChild<ParticleSystem>(ref this._par, base.get_transform(), "HighBase/MiniChara/SleepPar");
			Util.FindParentToChild<UILabel>(ref this._nowItemNum, base.get_transform(), "HighBase/nowItemNum");
			Util.FindParentToChild<UILabel>(ref this._nextItemNum, base.get_transform(), "HighBase/nextItemNum");
			if (this._anim == null)
			{
				this._anim = base.get_gameObject().GetComponent<Animation>();
			}
			this.IsHigh = false;
			this._isAnimate = false;
			this.StartSleepAnimate();
			this.SetOff();
			this.SetBuildKitValue();
			UIButtonMessage component = base.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "SpeedIconEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._highSpeedIcon);
			Mem.Del(ref this._highSpeedBar);
			Mem.Del(ref this._selectFrame);
			Mem.Del(ref this._par);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<UILabel>(ref this._nowItemNum);
			Mem.Del<UILabel>(ref this._nextItemNum);
		}

		public void SetBuildKitValue()
		{
			this._nowItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			this._nextItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
		}

		public void SetOff()
		{
			this._highSpeedIcon.get_transform().localPositionX(-60f);
			this._highSpeedBar.spriteName = "switch_kenzo_off";
			this._highSpeedIcon.spriteName = "switch_pin_off";
			this.StartSleepAnimate();
			this.IsHigh = false;
		}

		public void SpeedIconEL(GameObject obj)
		{
			if (!base.GetComponent<Collider2D>().get_enabled())
			{
				return;
			}
			bool flag = !this.IsHigh;
			int buildKit = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			int buildKit2 = ((!flag) ? ArsenalTaskManager.GetLogicManager().GetMinForCreateShip() : ArsenalTaskManager.GetLogicManager().GetMaxForCreateShip()).BuildKit;
			int num = buildKit - buildKit2;
			if (num < 0)
			{
				return;
			}
			this.IsHigh = flag;
			float num2 = (!this.IsHigh) ? -60f : 70f;
			TweenPosition tweenPosition = TweenPosition.Begin(this._highSpeedIcon.get_gameObject(), 0.2f, new Vector3(num2, -20.1f, 0f));
			tweenPosition.animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this._highSpeedBar.spriteName = ((!this.IsHigh) ? "switch_kenzo_off" : "switch_kenzo_on");
			this._highSpeedIcon.spriteName = ((!this.IsHigh) ? "switch_pin_off" : "switch_pin_on");
			if (this.IsHigh)
			{
				this.StartUpAnimate();
			}
			else
			{
				this.StartSleepAnimate();
			}
			this.setNextItemNum(ArsenalTaskManager.GetLogicManager().Material.BuildKit - buildKit2);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		public void setNextItemNum(int nextNum)
		{
			this._nextItemNum.textInt = nextNum;
			this._nowItemNum.textInt = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
		}

		public void SetSelect(bool isSet)
		{
			this._selectFrame.alpha = ((!isSet) ? 0f : 1f);
		}

		public void StartUpAnimate()
		{
			this._anim.Stop();
			this._anim.Play("SpeedMiniUp");
			this._par.set_time(0f);
			this._par.Stop();
		}

		public void StartSleepAnimate()
		{
			this._anim.Stop();
			this._anim.Play("SpeedMiniSleepStart");
			this.StartSleepParticle();
		}

		public void StartSleepParticle()
		{
			this._par.set_time(0f);
			this._par.Stop();
			this._par.Play();
		}

		public void StopSleepParticle()
		{
			this._par.set_time(0f);
			this._par.Stop();
		}

		public void CompSleepAnimetion()
		{
			this._anim.Stop();
			this._anim.Play("SpeedMiniSleep");
		}

		public void CompSleepStartAnimetion()
		{
			this._anim.Stop();
			this._anim.Play("SpeedMiniSleep");
		}
	}
}
