using local.models.battle;
using System;

namespace KCV.Battle.Utils
{
	public delegate void DelProdShellingAttack(HougekiModel model, int nCurrentShellingCnt, bool isNextAttack, bool isSkipAttack, Action callback);
}
