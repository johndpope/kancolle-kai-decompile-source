using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleShutter : BaseShutter
	{
		[SerializeField]
		private Animation _aninShutter;

		public static BattleShutter Instantiate(BattleShutter prefab, Transform parent, int nPanelDepth)
		{
			BattleShutter battleShutter = Object.Instantiate<BattleShutter>(prefab);
			battleShutter.get_transform().set_parent(parent);
			battleShutter.get_transform().set_localScale(Vector3.get_one());
			battleShutter.get_transform().set_localPosition(Vector3.get_zero());
			battleShutter._uiPanel.depth = nPanelDepth;
			return battleShutter;
		}

		[DebuggerHidden]
		public static IEnumerator Instantiate(IObserver<BattleShutter> observer, BattleShutter prefab, Transform parent, int nPanelDepth)
		{
			BattleShutter.<Instantiate>c__IteratorCE <Instantiate>c__IteratorCE = new BattleShutter.<Instantiate>c__IteratorCE();
			<Instantiate>c__IteratorCE.prefab = prefab;
			<Instantiate>c__IteratorCE.parent = parent;
			<Instantiate>c__IteratorCE.observer = observer;
			<Instantiate>c__IteratorCE.<$>prefab = prefab;
			<Instantiate>c__IteratorCE.<$>parent = parent;
			<Instantiate>c__IteratorCE.<$>observer = observer;
			return <Instantiate>c__IteratorCE;
		}

		public override void ReqMode(BaseShutter.ShutterMode iMode, Action callback)
		{
			if (iMode == BaseShutter.ShutterMode.None)
			{
				return;
			}
			if (this._iShutterMode == iMode)
			{
				return;
			}
			this._actCallback = callback;
			if (!this._isTween)
			{
				if (iMode == BaseShutter.ShutterMode.Close)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_034);
				}
				this._uiTop.get_transform().LTMoveLocal(this._vTopPos[(int)iMode], 0.25f).setEase(LeanTweenType.easeInQuad).setOnComplete(delegate
				{
					this.OnShutterActionComplate();
				});
				this._uiBtm.get_transform().LTMoveLocal(this._vBtnPos[(int)iMode], 0.25f).setEase(LeanTweenType.easeInQuad).setOnComplete(delegate
				{
				});
			}
			this._iShutterMode = iMode;
		}

		protected override void OnShutterActionComplate()
		{
			this._isTween = false;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
