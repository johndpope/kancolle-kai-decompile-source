using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportAirBattle : Exec_AirBattle
	{
		public Exec_SupportAirBattle(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice) : base(myData, mySubInfo, enemyData, enemySubInfo, null, practice)
		{
			this.valance1 = 0.2;
			this.valance2 = 85.0;
			this.valance3 = 3.0;
		}

		public override AirBattle GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return base.GetResultData(formation, cParam);
		}

		protected override bool isAirBattleCommand()
		{
			return true;
		}

		protected override void setTargetingKind(FormationDatas formationDatas)
		{
			this.battleTargetKind = BattleTargetKind.Other;
		}

		protected override AirBattle1 getAirBattle1()
		{
			return base.getAirBattle1();
		}

		protected override int getDeckSeikuPow(Dictionary<Mem_ship, List<FighterInfo>> fighterData, ref int fighterCount)
		{
			return base.getDeckSeikuPow(fighterData, ref fighterCount);
		}

		protected override int getMerits(int f_seiku, int e_seiku, ref BattleSeikuKinds dispSeiku)
		{
			return base.getMerits(f_seiku, e_seiku, ref dispSeiku);
		}

		protected override void battleSeiku(int merits, LostPlaneInfo lostFmt, bool enemyFlag)
		{
			base.battleSeiku(merits, lostFmt, enemyFlag);
		}

		protected override int getSyokusetuInfo(int seiku, BattleBaseData baseData)
		{
			return 0;
		}

		protected override AirBattle2 getAirBattle2()
		{
			return base.getAirBattle2();
		}

		protected override AirFireInfo getAntiAirFireAttacker(BattleBaseData baseData)
		{
			return null;
		}

		protected override int getDeckTaikuPow(BattleBaseData battleBase)
		{
			return base.getDeckTaikuPow(battleBase);
		}

		protected override int battleTaiku(List<Mem_ship> taikuHaveShip, int deckTyku, AirFireInfo antifire)
		{
			return base.battleTaiku(taikuHaveShip, deckTyku, null);
		}

		protected override double getA1Plus(int type, int tyku, int slotLevel)
		{
			return 0.0;
		}

		protected override double getA2Plus(int type, int tyku, int slotLevel)
		{
			return 0.0;
		}

		protected override AirBattle3 getAirBattle3(AirBattle2 air2)
		{
			if (air2 == null)
			{
				return null;
			}
			int num = air2.F_LostInfo.Count - air2.F_LostInfo.LostCount;
			int num2 = air2.E_LostInfo.Count - air2.E_LostInfo.LostCount;
			if (num <= 0 && num2 <= 0)
			{
				return null;
			}
			AirBattle3 airBattle = new AirBattle3();
			if (num > 0)
			{
				this.setBakuraiPlane(this.f_FighterInfo, airBattle.F_BakugekiPlane, airBattle.F_RaigekiPlane);
				this.battleBakurai(this.F_Data, this.E_Data, this.f_FighterInfo, ref airBattle.E_Bakurai);
			}
			return airBattle;
		}

		protected override void setBakuraiPlane(Dictionary<Mem_ship, List<FighterInfo>> fighter, List<int> bakuPlane, List<int> raigPlane)
		{
			base.setBakuraiPlane(fighter, bakuPlane, raigPlane);
		}

		protected override void battleBakurai(BattleBaseData attacker, BattleBaseData target, Dictionary<Mem_ship, List<FighterInfo>> fighter, ref BakuRaiInfo bakurai)
		{
			base.battleBakurai(attacker, target, fighter, ref bakurai);
		}

		protected override int getBakuraiAtkPow(FighterInfo fighter, int fighterNum, Mem_ship target)
		{
			return base.getBakuraiAtkPow(fighter, fighterNum, target);
		}

		protected override double getTouchPlaneKeisu(Mst_slotitem slotitem)
		{
			return 1.0;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
