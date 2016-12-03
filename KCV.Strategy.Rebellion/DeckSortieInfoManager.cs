using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class DeckSortieInfoManager : MonoBehaviour
	{
		[SerializeField]
		private DeckSortieInfo[] deckSortieInfos;

		public int Init(List<DeckModel> areaDecks)
		{
			List<DeckModel> list = new List<DeckModel>();
			List<IsGoCondition> list2 = new List<IsGoCondition>();
			for (int i = 0; i < areaDecks.get_Count(); i++)
			{
				List<IsGoCondition> list3 = areaDecks.get_Item(i).IsValidSortie();
				if (0 < list3.get_Count())
				{
					list.Add(areaDecks.get_Item(i));
					list2.Add(list3.get_Item(0));
				}
			}
			this.deckSortieInfos = new DeckSortieInfo[9];
			for (int j = 1; j < this.deckSortieInfos.Length; j++)
			{
				this.deckSortieInfos[j] = base.get_transform().FindChild("DeckSortieInfo" + j).GetComponent<DeckSortieInfo>();
			}
			for (int k = 1; k < this.deckSortieInfos.Length; k++)
			{
				if (k < list.get_Count() + 1)
				{
					this.deckSortieInfos[k].SetActive(true);
					this.deckSortieInfos[k].SetDeckInfo(list.get_Item(k - 1), list2.get_Item(k - 1));
				}
				else
				{
					this.deckSortieInfos[k].SetActive(false);
				}
			}
			return list.get_Count();
		}

		public List<DeckModel> GetSortieEnableDeck(List<DeckModel> areaDecks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < areaDecks.get_Count(); i++)
			{
				List<IsGoCondition> list2 = areaDecks.get_Item(i).IsValidSortie();
				if (list2.get_Count() == 0)
				{
					list.Add(areaDecks.get_Item(i));
				}
			}
			return list;
		}
	}
}
