using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdExclamationPoint : MonoBehaviour
	{
		private UISprite _uiSprite;

		private UISprite sprite
		{
			get
			{
				return this.GetComponentThis(ref this._uiSprite);
			}
		}

		public static ProdExclamationPoint Instantiate(ProdExclamationPoint prefab, Transform parent)
		{
			ProdExclamationPoint prodExclamationPoint = Object.Instantiate<ProdExclamationPoint>(prefab);
			prodExclamationPoint.get_transform().set_parent(parent);
			prodExclamationPoint.get_transform().localScaleZero();
			prodExclamationPoint.get_transform().localPositionZero();
			prodExclamationPoint.sprite.alpha = 0f;
			return prodExclamationPoint;
		}

		private void OnDestroy()
		{
			base.get_transform().LTCancel();
			Mem.Del(ref this._uiSprite);
		}

		public IObservable<bool> Play()
		{
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		[DebuggerHidden]
		private IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdExclamationPoint.<AnimationObserver>c__Iterator120 <AnimationObserver>c__Iterator = new ProdExclamationPoint.<AnimationObserver>c__Iterator120();
			<AnimationObserver>c__Iterator.observer = observer;
			<AnimationObserver>c__Iterator.<$>observer = observer;
			<AnimationObserver>c__Iterator.<>f__this = this;
			return <AnimationObserver>c__Iterator;
		}
	}
}
