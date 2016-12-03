using local.utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBattleEnd : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiEndLabel;

		[SerializeField]
		private TweenAlpha _taAlpha;

		public static ProdBattleEnd Instantiate(ProdBattleEnd prefab, Transform parent)
		{
			ProdBattleEnd prodBattleEnd = Object.Instantiate<ProdBattleEnd>(prefab);
			prodBattleEnd.get_transform().set_parent(parent);
			prodBattleEnd.get_transform().localPositionZero();
			prodBattleEnd.get_transform().localScaleOne();
			return prodBattleEnd;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiEndLabel);
			Mem.Del<TweenAlpha>(ref this._taAlpha);
		}

		public void Play(Action callback)
		{
			TrophyUtil.Unlock_At_SCutBattle();
			this._taAlpha.PlayForward();
			this._taAlpha.SetOnFinished(delegate
			{
				if (callback != null)
				{
					callback.Invoke();
				}
				Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
				{
					Object.Destroy(this.get_gameObject());
				});
			});
		}
	}
}
