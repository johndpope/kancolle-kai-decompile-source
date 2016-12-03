using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public interface IBattlePhase
	{
		List<ShipModel_Defender> GetDefenders(bool is_friend);

		List<ShipModel_Defender> GetDefenders(bool is_friend, DamagedStates damage_event);

		List<ShipModel_Defender> GetGekichinShips();

		List<ShipModel_Defender> GetGekichinShips(bool is_friend);

		bool HasChuhaEvent();

		bool HasChuhaEvent(bool is_friend);

		bool HasTaihaEvent();

		bool HasTaihaEvent(bool is_friend);

		bool HasGekichinEvent();

		bool HasGekichinEvent(bool is_friend);

		bool HasRecoveryEvent();

		bool HasRecoveryEvent(bool is_friend);
	}
}
