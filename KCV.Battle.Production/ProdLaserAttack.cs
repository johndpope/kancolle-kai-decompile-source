using local.models.battle;
using System;

namespace KCV.Battle.Production
{
	public class ProdLaserAttack : BaseProdAttackShelling
	{
		public override void PlayAttack(HougekiModel model, int nCurrentShellingCnt, bool isNextAttack, bool isSkipAttack, Action callback)
		{
			base.PlayAttack(model, nCurrentShellingCnt, isNextAttack, isSkipAttack, callback);
			this.OnFinished();
		}

		protected override bool InitAttackerFocus(object data)
		{
			return base.InitAttackerFocus(data);
		}

		protected override bool InitRotateFocus(object data)
		{
			return base.InitRotateFocus(data);
		}

		protected override bool InitDefenderFocus(object data)
		{
			return base.InitDefenderFocus(data);
		}
	}
}
