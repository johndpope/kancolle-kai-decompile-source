using Common.Enum;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;

namespace KCV.SortieMap
{
	public class SortieUtils
	{
		public static string ConvertMatCategory2String(enumMaterialCategory iCategory)
		{
			switch (iCategory)
			{
			case enumMaterialCategory.Fuel:
				return "燃料";
			case enumMaterialCategory.Bull:
				return "弾薬";
			case enumMaterialCategory.Steel:
				return "鋼材";
			case enumMaterialCategory.Bauxite:
				return "ボーキサイト";
			case enumMaterialCategory.Build_Kit:
				return "高速建造材";
			case enumMaterialCategory.Repair_Kit:
				return "高速修復材";
			case enumMaterialCategory.Dev_Kit:
				return "開発資材";
			case enumMaterialCategory.Revamp_Kit:
				return "改修資材";
			default:
				return "未登録マテリアルID: " + iCategory.ToString();
			}
		}

		public static string ConvertItem2String(int nItemId)
		{
			switch (nItemId)
			{
			case 10:
				return "家具箱(小)";
			case 11:
				return "家具箱(中)";
			case 12:
				return "家具箱(大)";
			default:
				return "未登録ID: " + nItemId.ToString();
			}
		}

		public static BattleFormationKinds1[] GetFormationArray(DeckModel deck)
		{
			HashSet<BattleFormationKinds1> selectableFormations = DeckUtil.GetSelectableFormations(deck);
			List<BattleFormationKinds1> list = new List<BattleFormationKinds1>();
			if (selectableFormations.Contains(BattleFormationKinds1.TanJuu))
			{
				list.Add(BattleFormationKinds1.TanJuu);
			}
			if (selectableFormations.Contains(BattleFormationKinds1.FukuJuu))
			{
				list.Add(BattleFormationKinds1.FukuJuu);
			}
			if (selectableFormations.Contains(BattleFormationKinds1.Rinkei))
			{
				list.Add(BattleFormationKinds1.Rinkei);
			}
			if (selectableFormations.Contains(BattleFormationKinds1.Teikei))
			{
				list.Add(BattleFormationKinds1.Teikei);
			}
			if (selectableFormations.Contains(BattleFormationKinds1.TanOu))
			{
				list.Add(BattleFormationKinds1.TanOu);
			}
			return list.ToArray();
		}
	}
}
