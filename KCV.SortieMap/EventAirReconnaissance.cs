using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.SortieMap
{
	public class EventAirReconnaissance : BaseEvent
	{
		private MapEventAirReconnaissanceModel _clsEventModel;

		public EventAirReconnaissance(MapEventAirReconnaissanceModel eventModel)
		{
			this._clsEventModel = eventModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<MapEventAirReconnaissanceModel>(ref this._clsEventModel);
			base.Dispose(disposing);
		}

		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			EventAirReconnaissance.<AnimationObserver>c__Iterator11A <AnimationObserver>c__Iterator11A = new EventAirReconnaissance.<AnimationObserver>c__Iterator11A();
			<AnimationObserver>c__Iterator11A.observer = observer;
			<AnimationObserver>c__Iterator11A.<$>observer = observer;
			<AnimationObserver>c__Iterator11A.<>f__this = this;
			return <AnimationObserver>c__Iterator11A;
		}
	}
}
