using Server_Common.Formats;
using System;

namespace local.models
{
	public class EnemyActionPhaseResultModel : PhaseResultModel
	{
		public EnemyActionPhaseResultModel(TurnWorkResult data) : base(data)
		{
		}

		public override string ToString()
		{
			return string.Format("[敵行動フェーズ]: ", new object[0]);
		}
	}
}
