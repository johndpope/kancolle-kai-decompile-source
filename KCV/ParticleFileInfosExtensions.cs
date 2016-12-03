using System;

namespace KCV
{
	public static class ParticleFileInfosExtensions
	{
		public static string ParticleFilePath(this ParticleFileInfos info)
		{
			switch (info)
			{
			case ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST:
				return "Battle/Torpedo";
			case ParticleFileInfos.BattlePSTorpedowakeD:
				return "Battle/TorpedoD";
			case ParticleFileInfos.BattlePSExprosionS:
				return "Battle/PSTorpedoWake";
			case ParticleFileInfos.BattleAdventFleetCloud:
				return "Battle/Cloud";
			case ParticleFileInfos.BattlePSSplashTorpedo:
				return "Battle/Splash/PSSplashT";
			case ParticleFileInfos.BattlePSSplashTorpedoD:
				return "Battle/Splash/PSSplashTTes";
			case ParticleFileInfos.BattlePSSplashSmoke:
				return "Battle/Splash/SplashSmoke";
			case ParticleFileInfos.BattlePSSinkSpray:
				return "Battle/SinkSpray";
			case ParticleFileInfos.BattlePSDetectionRipple:
				return "Battle/Detection/PSDetectionRipple";
			case ParticleFileInfos.BattlePSTorpedoAircraft:
				return "Battle/Aerial/AircraftTrupedo";
			case ParticleFileInfos.BattleExplosionAircraft:
				return "Battle/Aerial/AerialExplosion1";
			case ParticleFileInfos.BattleExplosion2Aircraft:
				return "Battle/Aerial/AerialExplosion2";
			case ParticleFileInfos.BattlePSAircraftLost2D:
				return "Battle/Aerial/PSAerialExplosion";
			case ParticleFileInfos.BattlePSAircraftLost3D:
				return "Battle/Aerial/AircraftExplosion3D";
			case ParticleFileInfos.BattlePSAircraftSmoke:
				return "Battle/Aerial/AircraftSmoke";
			case ParticleFileInfos.BattlePSAircraftStrafe:
				return "Battle/Aerial/AircraftStrafe";
			case ParticleFileInfos.BattlePSExprosionB1:
				return "Battle/Explosion/PSExplosionB";
			case ParticleFileInfos.BattlePSExprosionB2:
				return "Battle/Explosion/PSExplosionB2";
			case ParticleFileInfos.BattlePSExprosionB3:
				return "Battle/Explosion/PSExplosionB3";
			case ParticleFileInfos.BattlePSExprosionB4:
				return "Battle/Explosion/PSExplosionB4";
			case ParticleFileInfos.BattlePSExprosionEx1:
				return "Battle/Explosion/PSExplosionEX_A";
			case ParticleFileInfos.BattlePSExprosionEx2:
				return "Battle/Explosion/PSExplosionEX_B";
			case ParticleFileInfos.BattlePSDepthChargeShot:
				return "Battle/Shelling/DepthChargeShot";
			case ParticleFileInfos.BattlePSDepthCharge:
				return "Battle/Shelling/DepthCharge";
			case ParticleFileInfos.BattleFlareLight:
				return "Battle/FlareLight";
			case ParticleFileInfos.BattleFlareLight2:
				return "Battle/FlareLight2";
			case ParticleFileInfos.BattleFlareStart:
				return "Battle/FlareLightStart";
			case ParticleFileInfos.BattleSplashMissColumn:
				return "Battle/Splash/PSSplashMiss2";
			}
			return string.Empty;
		}
	}
}
