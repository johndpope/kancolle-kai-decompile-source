using Common.Enum;
using Common.Struct;
using local.managers;
using local.models;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace local.utils
{
	public static class Utils
	{
		public const int __DECK_NUM_MAX__ = 8;

		public static string GetText(TextType type, int target_id)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (type == TextType.SHIP_GET_TEXT)
			{
				text = "mst_shiptext";
				text2 = "Getmes";
			}
			else if (type == TextType.USEITEM_TEXT)
			{
				text = "mst_useitemtext";
				text2 = "Description";
			}
			else if (type == TextType.FURNITURE_TEXT)
			{
				text = "mst_furnituretext";
				text2 = "Description";
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Id", target_id.ToString());
			IEnumerable<XElement> enumerable = Server_Common.Utils.Xml_Result_Where(text, text, dictionary);
			if (enumerable == null)
			{
				return string.Empty;
			}
			return Enumerable.First<XElement>(enumerable).Element(text2).get_Value();
		}

		public static string enumMaterialCategoryToString(enumMaterialCategory material)
		{
			switch (material)
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
				return "高速建造";
			case enumMaterialCategory.Repair_Kit:
				return "高速修復剤";
			case enumMaterialCategory.Dev_Kit:
				return "開発資材";
			case enumMaterialCategory.Revamp_Kit:
				return "改修資材";
			default:
				return string.Empty;
			}
		}

		public static string GetSlotitemType3Name(int type3)
		{
			Dictionary<int, KeyValuePair<int, string>> slotItemEquipTypeName = Mst_DataManager.Instance.GetSlotItemEquipTypeName();
			KeyValuePair<int, string> keyValuePair;
			if (!slotItemEquipTypeName.TryGetValue(type3, ref keyValuePair))
			{
				return string.Empty;
			}
			return keyValuePair.get_Value();
		}

		public static List<KeyValuePair<enumMaterialCategory, int>> SortMaterial(List<enumMaterialCategory> sortkey, Dictionary<enumMaterialCategory, int> materials)
		{
			List<KeyValuePair<enumMaterialCategory, int>> list = new List<KeyValuePair<enumMaterialCategory, int>>();
			using (List<enumMaterialCategory>.Enumerator enumerator = sortkey.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumMaterialCategory current = enumerator.get_Current();
					int num = (!materials.ContainsKey(current)) ? 0 : materials.get_Item(current);
					KeyValuePair<enumMaterialCategory, int> keyValuePair = new KeyValuePair<enumMaterialCategory, int>(current, num);
					list.Add(keyValuePair);
				}
			}
			return list;
		}

		public static MemberMaxInfo ShipCountData()
		{
			int count = Comm_UserDatas.Instance.User_ship.get_Count();
			int max_chara = Comm_UserDatas.Instance.User_basic.Max_chara;
			MemberMaxInfo result = new MemberMaxInfo(count, max_chara);
			return result;
		}

		public static MemberMaxInfo SlotitemCountData()
		{
			int count = Comm_UserDatas.Instance.User_slot.get_Count();
			int max_slotitem = Comm_UserDatas.Instance.User_basic.Max_slotitem;
			MemberMaxInfo result = new MemberMaxInfo(count, max_slotitem);
			return result;
		}

		public static int GetResourceMstId(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship_resources.get_Item(mst_id).Standing_id;
		}

		public static int GetVoiceMstId(int mst_id, int voice_id)
		{
			int voiceId = Mst_DataManager.Instance.Mst_ship_resources.get_Item(mst_id).GetVoiceId(voice_id);
			return (voiceId != 0) ? voiceId : -1;
		}

		public static int GetSpecialVoiceId(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship_resources.get_Item(mst_id).GetDeckPracticeVoiceNo();
		}

		public static int GetSlotitemGraphicId(int mst_id)
		{
			bool flag;
			return Utils.GetSlotitemGraphicId(mst_id, out flag);
		}

		public static int GetSlotitemGraphicId(int mst_id, out bool master_loaded)
		{
			Dictionary<int, int> mst_slotitem_comvert = Mst_DataManager.Instance.UiBattleMaster.Mst_slotitem_comvert;
			master_loaded = (mst_slotitem_comvert != null);
			if (master_loaded)
			{
				int result;
				if (mst_slotitem_comvert.TryGetValue(mst_id, ref result))
				{
					return result;
				}
				if (mst_id > 500)
				{
					return mst_id - 500;
				}
			}
			return mst_id;
		}

		public static ShipRecoveryType __HasRecoveryItem__(List<SlotitemModel_Battle> slotitems)
		{
			return Utils.__HasRecoveryItem__(slotitems, null);
		}

		public static ShipRecoveryType __HasRecoveryItem__(List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitem_ex)
		{
			if (slotitem_ex != null)
			{
				if (slotitem_ex.MstId == 42)
				{
					return ShipRecoveryType.Personnel;
				}
				if (slotitem_ex.MstId == 43)
				{
					return ShipRecoveryType.Goddes;
				}
			}
			for (int i = 0; i < slotitems.get_Count(); i++)
			{
				SlotitemModel_Battle slotitemModel_Battle = slotitems.get_Item(i);
				if (slotitemModel_Battle != null)
				{
					if (slotitemModel_Battle.MstId == 42)
					{
						return ShipRecoveryType.Personnel;
					}
					if (slotitemModel_Battle.MstId == 43)
					{
						return ShipRecoveryType.Goddes;
					}
				}
			}
			return ShipRecoveryType.None;
		}

		public static Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count)
		{
			return Utils.GetAreaResource(area_id, tanker_count, null);
		}

		public static Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager)
		{
			DeckShips deckShip = null;
			if (eManager != null && eManager.EditDeck != null)
			{
				deckShip = ((TemporaryEscortDeckModel)eManager.EditDeck).DeckShips;
			}
			return new Api_req_Transport().GetMaterialNum(area_id, tanker_count, deckShip);
		}

		public static HashSet<SType> GetSortieLimit(int map_id, bool is_permitted)
		{
			if (is_permitted)
			{
				if (map_id == 71)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.Destroyter);
					hashSet.Add(SType.LightCruiser);
					hashSet.Add(SType.TrainingCruiser);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				if (map_id == 121)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.Submarine);
					hashSet.Add(SType.SubmarineAircraftCarrier);
					hashSet.Add(SType.SubmarineTender);
					hashSet.Add(SType.Destroyter);
					hashSet.Add(SType.LightCruiser);
					hashSet.Add(SType.TrainingCruiser);
					return hashSet;
				}
			}
			else
			{
				if (map_id == 22)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				if (map_id == 42)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					return hashSet;
				}
				if (map_id == 93)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.TorpedCruiser);
					hashSet.Add(SType.Submarine);
					hashSet.Add(SType.SubmarineAircraftCarrier);
					return hashSet;
				}
				if (map_id == 101)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				if (map_id == 122)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					return hashSet;
				}
				if (map_id == 131)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.HeavyCruiser);
					hashSet.Add(SType.AviationCruiser);
					return hashSet;
				}
				if (map_id == 132)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				if (map_id == 141)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				if (map_id == 142)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				if (map_id == 161)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					hashSet.Add(SType.LightAircraftCarrier);
					return hashSet;
				}
				if (map_id == 163)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
				if (map_id == 171)
				{
					HashSet<SType> hashSet = new HashSet<SType>();
					hashSet.Add(SType.BattleShip);
					hashSet.Add(SType.AviationBattleShip);
					hashSet.Add(SType.BattleCruiser);
					hashSet.Add(SType.AircraftCarrier);
					hashSet.Add(SType.ArmoredAircraftCarrier);
					return hashSet;
				}
			}
			return null;
		}

		public static int GetRadingStartTurn()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			Mst_radingtype mst_radingtype = Enumerable.Last<Mst_radingtype>(Mst_DataManager.Instance.Mst_RadingType.get_Item((int)difficult));
			return mst_radingtype.Turn_to;
		}
	}
}
