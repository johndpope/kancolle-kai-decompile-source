using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class MissionResultFmt
	{
		public Mem_deck Deck;

		public MissionResultKinds MissionResult;

		public int GetMemberExp;

		public int MemberLevel;

		public Dictionary<int, int> GetShipExp;

		public Dictionary<int, List<int>> LevelUpInfo;

		public string MissionName;

		public Dictionary<enumMaterialCategory, int> GetMaterials;

		public List<ItemGetFmt> GetItems;

		public int GetSpoint;

		public MissionResultFmt()
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			this.GetMaterials = dictionary;
			this.GetSpoint = 0;
		}
	}
}
