using Common.Enum;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutFormationSelect : BaseBattleCutState
	{
		private ProdFormationSelect _prodFormationSelect;

		public override bool Init(object data)
		{
			Observable.FromCoroutine<ProdFormationSelect>((IObserver<ProdFormationSelect> observer) => this.CreateFormationSelect(observer)).Subscribe(delegate(ProdFormationSelect x)
			{
				x.Play(delegate(BattleFormationKinds1 formation)
				{
					BattleCutManager.StartBattle(formation);
				});
			});
			return false;
		}

		public override bool Run(object data)
		{
			if (this._prodFormationSelect != null)
			{
				this._prodFormationSelect.Run();
			}
			return base.IsCheckPhase(BattleCutPhase.BattleCutPhase_ST);
		}

		public override bool Terminate(object data)
		{
			Object.Destroy(this._prodFormationSelect.get_gameObject());
			this._prodFormationSelect = null;
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateFormationSelect(IObserver<ProdFormationSelect> observer)
		{
			StateBattleCutFormationSelect.<CreateFormationSelect>c__Iterator114 <CreateFormationSelect>c__Iterator = new StateBattleCutFormationSelect.<CreateFormationSelect>c__Iterator114();
			<CreateFormationSelect>c__Iterator.observer = observer;
			<CreateFormationSelect>c__Iterator.<$>observer = observer;
			<CreateFormationSelect>c__Iterator.<>f__this = this;
			return <CreateFormationSelect>c__Iterator;
		}
	}
}
