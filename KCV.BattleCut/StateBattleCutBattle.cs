using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.BattleCut
{
	public class StateBattleCutBattle : BaseBattleCutState
	{
		private ProdBCBattle _prodBCBattle;

		private bool _isNightCombat;

		public ProdBCBattle prodBCBattle
		{
			get
			{
				return this._prodBCBattle;
			}
		}

		public bool isNightCombat
		{
			get
			{
				return this._isNightCombat;
			}
			set
			{
				this._isNightCombat = value;
			}
		}

		public override bool Init(object data)
		{
			Observable.FromCoroutine<ProdBCBattle>((IObserver<ProdBCBattle> observer) => this.CreateBCBattle(observer)).Subscribe(delegate(ProdBCBattle x)
			{
				Action arg_4A_0;
				if (this.isNightCombat)
				{
					arg_4A_0 = delegate
					{
						BattleCutManager.ReqPhase(BattleCutPhase.Judge);
					};
				}
				else
				{
					arg_4A_0 = delegate
					{
						BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
					};
				}
				Action callback = arg_4A_0;
				x.Play(this._isNightCombat, callback);
			});
			return false;
		}

		public override bool Run(object data)
		{
			return base.IsCheckPhase(BattleCutPhase.DayBattle);
		}

		public override bool Terminate(object data)
		{
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateBCBattle(IObserver<ProdBCBattle> observer)
		{
			StateBattleCutBattle.<CreateBCBattle>c__Iterator111 <CreateBCBattle>c__Iterator = new StateBattleCutBattle.<CreateBCBattle>c__Iterator111();
			<CreateBCBattle>c__Iterator.observer = observer;
			<CreateBCBattle>c__Iterator.<$>observer = observer;
			<CreateBCBattle>c__Iterator.<>f__this = this;
			return <CreateBCBattle>c__Iterator;
		}
	}
}
