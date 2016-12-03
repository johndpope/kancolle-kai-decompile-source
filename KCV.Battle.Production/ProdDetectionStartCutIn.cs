using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(Animation))]
	public class ProdDetectionStartCutIn : MonoBehaviour
	{
		[SerializeField]
		private List<UISprite> _listCircles;

		[SerializeField]
		private List<UISprite> _listLabels;

		private UIPanel _uiPanel;

		private Animation _anim;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private Animation animation
		{
			get
			{
				return this.GetComponentThis(ref this._anim);
			}
		}

		public static ProdDetectionStartCutIn Instantiate(ProdDetectionStartCutIn prefab, Transform parent)
		{
			ProdDetectionStartCutIn prodDetectionStartCutIn = Object.Instantiate<ProdDetectionStartCutIn>(prefab);
			prodDetectionStartCutIn.get_transform().set_parent(parent);
			prodDetectionStartCutIn.get_transform().localScaleZero();
			prodDetectionStartCutIn.get_transform().localPositionZero();
			prodDetectionStartCutIn.Init();
			return prodDetectionStartCutIn;
		}

		private void OnDestroy()
		{
			this._listCircles.ForEach(delegate(UISprite x)
			{
				x.Clear();
			});
			this._listLabels.ForEach(delegate(UISprite x)
			{
				x.Clear();
			});
			Mem.DelListSafe<UISprite>(ref this._listCircles);
			Mem.DelListSafe<UISprite>(ref this._listLabels);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Animation>(ref this._anim);
		}

		private bool Init()
		{
			this.panel.widgetsAreStatic = true;
			return true;
		}

		public IObservable<bool> Play()
		{
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		[DebuggerHidden]
		private IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdDetectionStartCutIn.<AnimationObserver>c__IteratorE2 <AnimationObserver>c__IteratorE = new ProdDetectionStartCutIn.<AnimationObserver>c__IteratorE2();
			<AnimationObserver>c__IteratorE.observer = observer;
			<AnimationObserver>c__IteratorE.<$>observer = observer;
			<AnimationObserver>c__IteratorE.<>f__this = this;
			return <AnimationObserver>c__IteratorE;
		}

		private void PlayMessageSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.BattleNightMessage);
		}
	}
}
