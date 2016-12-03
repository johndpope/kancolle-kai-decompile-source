using Server_Common.Formats;
using System;

namespace local.models
{
	public class EnemyPreActionPhaseResultModel : PhaseResultModel
	{
		public EnemyPreActionPhaseResultModel(TurnWorkResult data) : base(data)
		{
		}

		public override string ToString()
		{
			return string.Format("[敵事前行動フェーズ]: ", new object[0]);
		}
	}
}
