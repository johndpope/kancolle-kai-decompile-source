using System;

namespace KCV.Battle.Utils
{
	public enum BattleLayer
	{
		Background = -10,
		TorpedoLayer = -5,
		Banner = 0,
		Explosion = 10,
		Damage = 20,
		Radar = 30,
		Aircraft = 40,
		Shutter = 60,
		InfoLayer = 70,
		PhaseTelop = 80,
		HPGauge = 100,
		CutIn = 110,
		MustTop = 120
	}
}
