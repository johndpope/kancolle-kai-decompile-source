using Common.Enum;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class BattleUtils
	{
		public static HitState ConvertBattleHitState2HitState(DamageModelBase model)
		{
			HitState result = HitState.Miss;
			if (model == null)
			{
				return result;
			}
			BattleHitStatus hitState = model.GetHitState();
			if (hitState != BattleHitStatus.Normal)
			{
				if (hitState != BattleHitStatus.Clitical)
				{
					result = HitState.Miss;
				}
				else
				{
					if (model.GetGurdEffect())
					{
					}
					result = HitState.CriticalDamage;
				}
			}
			else
			{
				if (model.GetGurdEffect())
				{
				}
				result = HitState.NomalDamage;
			}
			return result;
		}

		public static BattlePhase NextPhase(BattlePhase iPhase)
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager == null)
			{
				return BattlePhase.BattlePhase_BEF;
			}
			BattlePhase result = BattlePhase.BattlePhase_BEF;
			switch (iPhase)
			{
			case BattlePhase.BattlePhase_ST:
				result = BattlePhase.FleetAdvent;
				break;
			case BattlePhase.FleetAdvent:
				if (battleManager is SortieBattleManager || battleManager is RebellionBattleManager)
				{
					BattleManager battleManager2 = BattleTaskManager.GetBattleManager();
					if (BattleUtils.GetStarBattleFleetAdventNextPhase(battleManager2.WarType) == BattlePhase.NightCombat)
					{
						result = BattlePhase.NightCombat;
						break;
					}
				}
				if (battleManager.IsExistSakutekiData())
				{
					result = BattlePhase.Detection;
				}
				else if (battleManager.IsExistCommandPhase())
				{
					result = BattlePhase.Command;
				}
				else if (battleManager.IsExistKoukuuData())
				{
					result = BattlePhase.AerialCombat;
				}
				else if (battleManager.IsExistShienData())
				{
					result = BattlePhase.SupportingFire;
				}
				else if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.Detection:
				if (battleManager.IsExistCommandPhase())
				{
					result = BattlePhase.Command;
				}
				else if (battleManager.IsExistKoukuuData())
				{
					result = BattlePhase.AerialCombat;
				}
				else if (battleManager.IsExistShienData())
				{
					result = BattlePhase.SupportingFire;
				}
				else if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.Command:
				if (battleManager.IsExistKoukuuData())
				{
					result = BattlePhase.AerialCombat;
				}
				else if (battleManager.IsExistShienData())
				{
					result = BattlePhase.SupportingFire;
				}
				else if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.AerialCombat:
				if (battleManager.IsExistShienData())
				{
					result = BattlePhase.SupportingFire;
				}
				else if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.AerialCombatSecond:
				if (battleManager.IsExistShienData())
				{
					result = BattlePhase.SupportingFire;
				}
				else if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.SupportingFire:
				if (battleManager.IsExistKaimakuData())
				{
					result = BattlePhase.OpeningTorpedoSalvo;
				}
				else if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.OpeningTorpedoSalvo:
				if (battleManager.IsExistHougekiPhase_Day())
				{
					result = BattlePhase.Shelling;
				}
				else if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.Shelling:
				if (battleManager.IsExistRaigekiData())
				{
					result = BattlePhase.TorpedoSalvo;
				}
				else
				{
					result = BattlePhase.WithdrawalDecision;
				}
				break;
			case BattlePhase.TorpedoSalvo:
				result = BattlePhase.WithdrawalDecision;
				break;
			case BattlePhase.WithdrawalDecision:
				if (battleManager.HasNightBattle())
				{
					result = BattlePhase.NightCombat;
				}
				else
				{
					result = BattlePhase.Result;
				}
				break;
			case BattlePhase.NightCombat:
				result = BattlePhase.Result;
				break;
			case BattlePhase.Result:
				result = BattlePhase.ClearReward;
				break;
			case BattlePhase.FlagshipWreck:
				result = BattlePhase.MapOpen;
				break;
			case BattlePhase.EscortShipEvacuation:
				result = BattlePhase.AdvancingWithdrawal;
				break;
			case BattlePhase.ClearReward:
				if (battleManager.GetEscapeCandidate() != null)
				{
				}
				result = BattlePhase.MapOpen;
				break;
			}
			return result;
		}

		public static List<BattlePhase> GetBattlePhaseList()
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager == null)
			{
				return null;
			}
			List<BattlePhase> list = new List<BattlePhase>();
			list.Add(BattlePhase.BattlePhase_ST);
			list.Add(BattlePhase.FleetAdvent);
			list.Add(BattlePhase.Detection);
			if (battleManager.IsExistKoukuuData())
			{
				list.Add(BattlePhase.AerialCombat);
			}
			if (battleManager.IsExistShienData())
			{
				list.Add(BattlePhase.SupportingFire);
			}
			if (battleManager.IsExistKaimakuData())
			{
				list.Add(BattlePhase.OpeningTorpedoSalvo);
			}
			if (battleManager.IsExistHougekiPhase_Day())
			{
				list.Add(BattlePhase.Shelling);
			}
			if (battleManager.IsExistRaigekiData())
			{
				list.Add(BattlePhase.TorpedoSalvo);
			}
			if (battleManager.HasNightBattle())
			{
				list.Add(BattlePhase.NightCombat);
			}
			return list;
		}

		public static BattlePhase GetStarBattleFleetAdventNextPhase(enumMapWarType iType)
		{
			BattlePhase result = BattlePhase.BattlePhase_BEF;
			switch (iType)
			{
			case enumMapWarType.Normal:
				result = BattlePhase.Detection;
				break;
			case enumMapWarType.Midnight:
			case enumMapWarType.Night_To_Day:
				result = BattlePhase.NightCombat;
				break;
			}
			return result;
		}

		public static bool IsPlayMVPVoice(BattleWinRankKinds iKind)
		{
			bool result;
			switch (iKind)
			{
			case BattleWinRankKinds.B:
			case BattleWinRankKinds.A:
			case BattleWinRankKinds.S:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		public static Hashtable GetRetentionDataAdvancingWithdrawal(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager sortieMapManager = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", sortieMapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager rebellionMapManager = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataAdvancingWithdrawalDC(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager sortieMapManager = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", sortieMapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager rebellionMapManager = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataFlagshipWreck(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager sortieMapManager = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", sortieMapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager rebellionMapManager = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataMapOpen(MapManager manager, BattleResultModel model)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager sortieMapManager = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", sortieMapManager);
				hashtable.Add("newOpenAreaIDs", model.NewOpenAreaIDs);
				hashtable.Add("newOpenMapIDs", model.NewOpenMapIDs);
				hashtable.Add("rootType", 1);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager rebellionMapManager = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("newOpenAreaIDs", model.NewOpenAreaIDs);
				hashtable.Add("newOpenMapIDs", model.NewOpenMapIDs);
				hashtable.Add("rootType", 2);
			}
			return hashtable;
		}

		public static ShipRecoveryType GetShipRecoveryType(AdvancingWithdrawalDCType iType)
		{
			if (iType == AdvancingWithdrawalDCType.Youin)
			{
				return ShipRecoveryType.Personnel;
			}
			if (iType != AdvancingWithdrawalDCType.Megami)
			{
				return ShipRecoveryType.None;
			}
			return ShipRecoveryType.Goddes;
		}

		public static AsyncOperation GetNextLoadLevelAsync()
		{
			return Application.LoadLevelAsync(Generics.Scene.Strategy.ToString());
		}

		public static AsyncOperation GetNextLoadLevelAsync(BattlePhase iPhase)
		{
			if (iPhase != BattlePhase.FlagshipWreck)
			{
				return null;
			}
			return Application.LoadLevelAsync(Generics.Scene.Strategy.ToString());
		}

		public static Generics.Scene GetNextLoadLevelAsync(UIHexButtonEx btn)
		{
			switch (btn.index)
			{
			case 0:
				return Generics.Scene.Strategy;
			case 1:
				return Generics.Scene.SortieAreaMap;
			case 2:
				return Generics.Scene.SortieAreaMap;
			default:
				return Generics.Scene.Scene_BEF;
			}
		}

		public static AsyncOperation GetNextLoadLevelAsync(UIHexButton btn)
		{
			return (btn.index != 1) ? Application.LoadLevelAsync(Generics.Scene.Strategy.ToString()) : Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
		}

		[DebuggerHidden]
		public static IEnumerator ClearMemory()
		{
			return new BattleUtils.<ClearMemory>c__IteratorCF();
		}

		public static string log(ShipModel_Battle[] ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel_Battle shipModel_Battle = ships[i];
				if (shipModel_Battle == null)
				{
					text += string.Format("[{0}] - \n", i);
				}
				else
				{
					text = text + shipModel_Battle.ToString() + "\n";
				}
			}
			return text;
		}
	}
}
