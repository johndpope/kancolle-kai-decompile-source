using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferUnifiedFire : BaseProdBuffer
	{
		private Action _actOnPlayLookAnim;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<Action>(ref this._actOnPlayLookAnim);
		}

		public bool Init(Action onPlayLookAnim)
		{
			this._actOnPlayLookAnim = onPlayLookAnim;
			return true;
		}

		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferUnifiedFire.<AnimationObserver>c__IteratorDC <AnimationObserver>c__IteratorDC = new ProdBufferUnifiedFire.<AnimationObserver>c__IteratorDC();
			<AnimationObserver>c__IteratorDC.observer = observer;
			<AnimationObserver>c__IteratorDC.<$>observer = observer;
			<AnimationObserver>c__IteratorDC.<>f__this = this;
			return <AnimationObserver>c__IteratorDC;
		}

		private void PlayLookAnimation()
		{
			Dlg.Call(ref this._actOnPlayLookAnim);
		}
	}
}
