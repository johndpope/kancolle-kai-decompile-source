using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionStart : MonoBehaviour
	{
		[Button("StartAnimation", "StartAnimation", new object[]
		{

		})]
		public int button;

		[SerializeField]
		private UIPanel RedMask;

		[SerializeField]
		private UITexture Obi;

		[SerializeField]
		private Transform RaisyuText;

		[SerializeField]
		private TextureFlash texFlash;

		[SerializeField]
		private UILabel AreaName;

		private Action Onfinished;

		public static ProdRebellionStart Instantiate(ProdRebellionStart prefab, Transform parent)
		{
			ProdRebellionStart prodRebellionStart = Object.Instantiate<ProdRebellionStart>(prefab);
			prodRebellionStart.get_transform().set_parent(parent);
			prodRebellionStart.get_transform().localPositionZero();
			prodRebellionStart.get_transform().localScaleOne();
			return prodRebellionStart;
		}

		public void StartAnimation()
		{
			base.get_transform().GetComponent<Animation>().Play();
		}

		public void maskEffect()
		{
			this.texFlash.MaskFadeExpanding(2f, 0.5f, false);
		}

		public void AreaNameFadeIn()
		{
			TweenAlpha.Begin(this.AreaName.get_transform().get_parent().get_gameObject(), 0.2f, 1f);
		}

		public void AreaNameFadeOut()
		{
			TweenAlpha.Begin(this.AreaName.get_transform().get_parent().get_gameObject(), 0.2f, 0f);
		}

		public IObservable<bool> Play(Action Onfinished)
		{
			this.Onfinished = Onfinished;
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayAnimation(observer));
		}

		[DebuggerHidden]
		private IEnumerator PlayAnimation(IObserver<bool> observer)
		{
			ProdRebellionStart.<PlayAnimation>c__Iterator160 <PlayAnimation>c__Iterator = new ProdRebellionStart.<PlayAnimation>c__Iterator160();
			<PlayAnimation>c__Iterator.observer = observer;
			<PlayAnimation>c__Iterator.<$>observer = observer;
			<PlayAnimation>c__Iterator.<>f__this = this;
			return <PlayAnimation>c__Iterator;
		}

		public void PlaySE()
		{
			SoundUtils.PlaySE(SEFIleInfos.EnemyComming);
		}

		public void PlayBGM()
		{
			SoundUtils.PlayBGM((BGMFileInfos)4, true);
		}

		private void InitAreaName()
		{
			this.AreaName.text = StrategyTopTaskManager.GetLogicManager().Area.get_Item(StrategyRebellionTaskManager.RebellionArea).Name;
		}
	}
}
