using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyGetRewardOpenCreateLargeTanker : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSprite_Yousei_Spanner;

		[SerializeField]
		private UISprite mSprite_Yousei_Osage;

		[SerializeField]
		private UISprite mSprite_Yousei_Kinpatsu;

		[SerializeField]
		private UISprite mSprite_Yousei_Hammer;

		[SerializeField]
		private Transform mTransform_TankerBuilders;

		private void Start()
		{
			IEnumerator enumerator = this.YouseiAnimation();
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator YouseiAnimation()
		{
			UIDutyGetRewardOpenCreateLargeTanker.<YouseiAnimation>c__Iterator78 <YouseiAnimation>c__Iterator = new UIDutyGetRewardOpenCreateLargeTanker.<YouseiAnimation>c__Iterator78();
			<YouseiAnimation>c__Iterator.<>f__this = this;
			return <YouseiAnimation>c__Iterator;
		}

		private void OnDestroy()
		{
			base.StopCoroutine(this.YouseiAnimation());
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Yousei_Spanner);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Yousei_Osage);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Yousei_Kinpatsu);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Yousei_Hammer);
			this.mTransform_TankerBuilders = null;
		}
	}
}
