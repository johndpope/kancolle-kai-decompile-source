using Common.Enum;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace logic.debug.utils
{
	public class SearchValueUtil
	{
		private static Dictionary<MapBranchResult.enumScoutingKind, Dictionary<int, double>> _TYPE3_COEFFICIENT;

		private static Dictionary<MapBranchResult.enumScoutingKind, double> _OTHER_COEFFICIENT;

		private static Dictionary<int, double> _LEVEL_COEFFICIENT;

		private static Dictionary<DifficultKind, double> _DIFFICULTY_COEFFICIENT;

		public static double Get_K2(DeckModel deck, DifficultKind difficulty)
		{
			return SearchValueUtil.GetSearchValue(deck, MapBranchResult.enumScoutingKind.K2, difficulty);
		}

		public static int Get_K2J(int user_level, double K2)
		{
			return (int)((double)((int)K2) - (Math.Sqrt((double)user_level) * 3.0 + (double)user_level * 0.2));
		}

		public static double GetSearchValue(DeckModel deck, MapBranchResult.enumScoutingKind type, DifficultKind difficulty)
		{
			double num = 0.0;
			for (int i = 0; i < deck.GetShips().Length; i++)
			{
				ShipModel ship = deck.GetShip(i);
				double shipValue = SearchValueUtil.GetShipValue(ship);
				double num2 = 0.0;
				List<SlotitemModel> slotitemList = ship.SlotitemList;
				for (int j = 0; j < slotitemList.get_Count(); j++)
				{
					num2 += SearchValueUtil.GetSlotValue(type, slotitemList.get_Item(j));
				}
				double difficultyCorrectionValue = SearchValueUtil.GetDifficultyCorrectionValue(difficulty, type);
				num += shipValue + num2 + difficultyCorrectionValue;
			}
			return num;
		}

		public static double GetShipValue(ShipModel ship)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(ship.MstId);
			return Math.Sqrt((double)mst_ship.Saku) - 2.0;
		}

		public static double GetSlotValue(MapBranchResult.enumScoutingKind type, SlotitemModel slot)
		{
			if (slot == null)
			{
				return 0.0;
			}
			double type3Coefficient = SearchValueUtil.GetType3Coefficient(type, slot);
			double otherCoefficient = SearchValueUtil.GetOtherCoefficient(type);
			double levelCoefficient = SearchValueUtil.GetLevelCoefficient(slot);
			double num = Math.Sqrt((double)slot.Level) * levelCoefficient;
			double num2 = (type3Coefficient <= 0.0) ? otherCoefficient : type3Coefficient;
			return ((double)slot.Sakuteki + num) * num2;
		}

		public static double GetDifficultyCorrectionValue(DifficultKind difficulty, MapBranchResult.enumScoutingKind type)
		{
			if (type == MapBranchResult.enumScoutingKind.K2)
			{
				double difficultyCoefficient = SearchValueUtil.GetDifficultyCoefficient(difficulty);
				return -(difficultyCoefficient * 2.0);
			}
			return 0.0;
		}

		public static double GetType3Coefficient(MapBranchResult.enumScoutingKind type, SlotitemModel slot)
		{
			if (SearchValueUtil._TYPE3_COEFFICIENT == null)
			{
				SearchValueUtil._CreateType3Coefficient();
			}
			Dictionary<int, double> dictionary;
			if (!SearchValueUtil._TYPE3_COEFFICIENT.TryGetValue(type, ref dictionary))
			{
				return 0.0;
			}
			double result;
			dictionary.TryGetValue(slot.Type3, ref result);
			return result;
		}

		public static double GetOtherCoefficient(MapBranchResult.enumScoutingKind type)
		{
			if (SearchValueUtil._OTHER_COEFFICIENT == null)
			{
				SearchValueUtil._CreateOtherCoefficient();
			}
			return SearchValueUtil._OTHER_COEFFICIENT.get_Item(type);
		}

		public static double GetLevelCoefficient(SlotitemModel slot)
		{
			if (SearchValueUtil._LEVEL_COEFFICIENT == null)
			{
				SearchValueUtil._CreateLevelCoefficient();
			}
			double result;
			SearchValueUtil._LEVEL_COEFFICIENT.TryGetValue(slot.Type3, ref result);
			return result;
		}

		public static double GetDifficultyCoefficient(DifficultKind difficulty)
		{
			if (SearchValueUtil._DIFFICULTY_COEFFICIENT == null)
			{
				SearchValueUtil._CreateDifficultyCoefficient();
			}
			return SearchValueUtil._DIFFICULTY_COEFFICIENT.get_Item(difficulty);
		}

		public static void _CreateType3Coefficient()
		{
			SearchValueUtil._TYPE3_COEFFICIENT = new Dictionary<MapBranchResult.enumScoutingKind, Dictionary<int, double>>();
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 1.2);
			dictionary.Add(11, 1.1);
			dictionary.Add(9, 1.0);
			dictionary.Add(8, 0.8);
			Dictionary<int, double> dictionary2 = dictionary;
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.C, dictionary2);
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.D, dictionary2);
			dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 4.8);
			dictionary.Add(11, 4.4);
			dictionary.Add(9, 4.0);
			dictionary.Add(8, 3.2);
			dictionary2 = dictionary;
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.E, dictionary2);
			dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 3.5999999999999996);
			dictionary.Add(11, 3.3000000000000003);
			dictionary.Add(9, 3.0);
			dictionary.Add(8, 2.4000000000000004);
			dictionary2 = dictionary;
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.E2, dictionary2);
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.K1, dictionary2);
			SearchValueUtil._TYPE3_COEFFICIENT.set_Item(MapBranchResult.enumScoutingKind.K2, dictionary2);
		}

		public static void _CreateOtherCoefficient()
		{
			Dictionary<MapBranchResult.enumScoutingKind, double> dictionary = new Dictionary<MapBranchResult.enumScoutingKind, double>();
			dictionary.Add(MapBranchResult.enumScoutingKind.C, 0.6);
			dictionary.Add(MapBranchResult.enumScoutingKind.D, 0.6);
			dictionary.Add(MapBranchResult.enumScoutingKind.E, 2.4);
			dictionary.Add(MapBranchResult.enumScoutingKind.E2, 1.7999999999999998);
			dictionary.Add(MapBranchResult.enumScoutingKind.K1, 1.7999999999999998);
			dictionary.Add(MapBranchResult.enumScoutingKind.K2, 1.7999999999999998);
			SearchValueUtil._OTHER_COEFFICIENT = dictionary;
		}

		public static void _CreateLevelCoefficient()
		{
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(9, 1.2);
			dictionary.Add(10, 1.2);
			dictionary.Add(11, 1.15);
			dictionary.Add(12, 1.25);
			dictionary.Add(13, 1.4);
			dictionary.Add(26, 1.0);
			dictionary.Add(41, 1.2);
			SearchValueUtil._LEVEL_COEFFICIENT = dictionary;
		}

		public static void _CreateDifficultyCoefficient()
		{
			Dictionary<DifficultKind, double> dictionary = new Dictionary<DifficultKind, double>();
			dictionary.Add(DifficultKind.TEI, 2.0);
			dictionary.Add(DifficultKind.HEI, 4.0);
			dictionary.Add(DifficultKind.OTU, 7.0);
			dictionary.Add(DifficultKind.KOU, 10.0);
			dictionary.Add(DifficultKind.SHI, 15.0);
			SearchValueUtil._DIFFICULTY_COEFFICIENT = dictionary;
		}
	}
}
