using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class SoundUtils
	{
		public static AudioSource PlayShellingSE(ShipModel_Battle model)
		{
			if (model.ShipType == 8 || model.ShipType == 9 || model.ShipType == 10)
			{
				return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_903);
			}
			if (model.ShipType == 5)
			{
				return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_902);
			}
			return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_901);
		}

		public static AudioSource PlayTorpedoSE(ShipModel_Attacker model)
		{
			if (model.ShipType == 4)
			{
				return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleAdmission);
			}
			return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleAdmission);
		}

		public static AudioSource PlayDamageSE(HitState iState, bool isTorpedo)
		{
			switch (iState)
			{
			case HitState.Miss:
				return KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_908);
			case HitState.Gard:
			case HitState.NomalDamage:
				return (!isTorpedo) ? KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_906) : KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_909);
			case HitState.CriticalDamage:
				return (!isTorpedo) ? KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_907) : KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_910);
			default:
				return null;
			}
		}
	}
}
