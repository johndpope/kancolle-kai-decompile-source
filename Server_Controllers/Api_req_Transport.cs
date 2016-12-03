using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Transport
	{
		private Dictionary<int, int> mst_escort_group;

		public Dictionary<int, int> Mst_escort_group
		{
			get
			{
				return this.mst_escort_group;
			}
			private set
			{
				this.mst_escort_group = value;
			}
		}

		public void initEscortGroup()
		{
			if (this.mst_escort_group != null)
			{
				return;
			}
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
			this.mst_escort_group = Enumerable.ToDictionary<XElement, int, int>(enumerable, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => int.Parse(value.Element("Escort").get_Value()));
		}

		public Dictionary<enumMaterialCategory, int> GetMaterialNum(int area_id, int tankerNum, DeckShips deckShip)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(enumMaterialCategory)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					dictionary.Add((enumMaterialCategory)((int)current), 0);
				}
			}
			Mst_maparea mst_maparea = null;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, ref mst_maparea))
			{
				return dictionary;
			}
			DeckShips deckShip2 = (deckShip != null) ? deckShip : Comm_UserDatas.Instance.User_EscortDeck.get_Item(area_id).Ship;
			mst_maparea.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, tankerNum, ref dictionary, true, deckShip2);
			return dictionary;
		}

		public Api_Result<List<Mem_tanker>> GoTanker(int area_id, int num)
		{
			Api_Result<List<Mem_tanker>> api_Result = new Api_Result<List<Mem_tanker>>();
			List<Mem_tanker> freeTanker = Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker);
			if (Enumerable.Count<Mem_tanker>(freeTanker) < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			IEnumerable<Mem_tanker> enumerable = Enumerable.Take<Mem_tanker>(freeTanker, num);
			using (IEnumerator<Mem_tanker> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_tanker current = enumerator.get_Current();
					current.GoArea(area_id);
				}
			}
			api_Result.data = Enumerable.ToList<Mem_tanker>(enumerable);
			return api_Result;
		}

		public List<Mem_tanker> GetEnableBackTanker(int area_id)
		{
			IEnumerable<Mem_tanker> enumerable = Enumerable.Where<Mem_tanker>(Comm_UserDatas.Instance.User_tanker.get_Values(), (Mem_tanker x) => x.Maparea_id == area_id && !x.IsBlingShip() && x.Disposition_status == DispositionStatus.ARRIVAL);
			return Enumerable.ToList<Mem_tanker>(enumerable);
		}

		public Api_Result<List<Mem_tanker>> BackTanker(int area_id, int num)
		{
			Api_Result<List<Mem_tanker>> api_Result = new Api_Result<List<Mem_tanker>>();
			List<Mem_tanker> enableBackTanker = this.GetEnableBackTanker(area_id);
			if (Enumerable.Count<Mem_tanker>(enableBackTanker) < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			IEnumerable<Mem_tanker> enumerable = Enumerable.Take<Mem_tanker>(enableBackTanker, num);
			using (IEnumerator<Mem_tanker> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_tanker current = enumerator.get_Current();
					current.BackTanker();
				}
			}
			api_Result.data = Enumerable.ToList<Mem_tanker>(enumerable);
			return api_Result;
		}

		public Api_Result<Mem_esccort_deck> Update_DeckName(int deck_rid, string name)
		{
			Api_Result<Mem_esccort_deck> api_Result = new Api_Result<Mem_esccort_deck>();
			api_Result.data = null;
			Mem_esccort_deck mem_esccort_deck = null;
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(deck_rid, ref mem_esccort_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			mem_esccort_deck.SetDeckName(name);
			api_Result.data = mem_esccort_deck;
			return api_Result;
		}

		public bool IsValidChange(int deck_rid, int deck_targetIdx, int ship_rid, DeckShips deckShip)
		{
			Api_req_Transport.<IsValidChange>c__AnonStorey4BC <IsValidChange>c__AnonStorey4BC = new Api_req_Transport.<IsValidChange>c__AnonStorey4BC();
			<IsValidChange>c__AnonStorey4BC.ship_rid = ship_rid;
			<IsValidChange>c__AnonStorey4BC.deck_rid = deck_rid;
			this.initEscortGroup();
			Mem_esccort_deck mem_esccort_deck = null;
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(<IsValidChange>c__AnonStorey4BC.deck_rid, ref mem_esccort_deck))
			{
				return false;
			}
			<IsValidChange>c__AnonStorey4BC.unsetShips = null;
			if (<IsValidChange>c__AnonStorey4BC.ship_rid == -2)
			{
				if (deckShip.Count() <= 1)
				{
					return false;
				}
				deckShip.Clone(out <IsValidChange>c__AnonStorey4BC.unsetShips);
				<IsValidChange>c__AnonStorey4BC.unsetShips.RemoveAt(0);
			}
			else if (<IsValidChange>c__AnonStorey4BC.ship_rid == -1)
			{
				Api_req_Transport.<IsValidChange>c__AnonStorey4BC arg_AF_0 = <IsValidChange>c__AnonStorey4BC;
				List<int> list = new List<int>();
				list.Add(deckShip[deck_targetIdx]);
				arg_AF_0.unsetShips = list;
			}
			else
			{
				if (<IsValidChange>c__AnonStorey4BC.ship_rid == deckShip[deck_targetIdx])
				{
					return false;
				}
				int stype = Comm_UserDatas.Instance.User_ship.get_Item(<IsValidChange>c__AnonStorey4BC.ship_rid).Stype;
				if (Mst_DataManager.Instance.Mst_stype.get_Item(stype).IsSubmarine())
				{
					return false;
				}
				Api_req_Transport.<IsValidChange>c__AnonStorey4BC arg_11F_0 = <IsValidChange>c__AnonStorey4BC;
				List<int> list = new List<int>();
				list.Add(<IsValidChange>c__AnonStorey4BC.ship_rid);
				arg_11F_0.unsetShips = list;
			}
			DeckShips deckShips = null;
			deckShip.Clone(out deckShips);
			this.Change_TempDeck(deck_targetIdx, <IsValidChange>c__AnonStorey4BC.ship_rid, ref deckShips);
			if (!this.validEscortStypeCheck(deckShips, -1))
			{
				return false;
			}
			Mem_ndock mem_ndock = Enumerable.FirstOrDefault<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => <IsValidChange>c__AnonStorey4BC.unsetShips.Contains(x.Ship_id));
			if (mem_ndock != null)
			{
				return false;
			}
			bool flag = Enumerable.Any<int>(<IsValidChange>c__AnonStorey4BC.unsetShips, delegate(int bling_rid)
			{
				Mem_ship mem_ship2 = null;
				return Comm_UserDatas.Instance.User_ship.TryGetValue(bling_rid, ref mem_ship2) && mem_ship2.IsBlingShip();
			});
			if (flag)
			{
				return false;
			}
			if (<IsValidChange>c__AnonStorey4BC.ship_rid < 0)
			{
				return true;
			}
			bool flag2 = Enumerable.Any<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship.Find(<IsValidChange>c__AnonStorey4BC.ship_rid) != -1);
			if (flag2)
			{
				return false;
			}
			bool flag3 = Enumerable.Any<Mem_esccort_deck>(Comm_UserDatas.Instance.User_EscortDeck.get_Values(), (Mem_esccort_deck x) => x.Rid != <IsValidChange>c__AnonStorey4BC.deck_rid && x.Ship.Find(<IsValidChange>c__AnonStorey4BC.ship_rid) != -1);
			if (flag3)
			{
				return false;
			}
			Func<DeckShips, bool> func = delegate(DeckShips x)
			{
				int num = x.Count();
				List<string> list2 = new List<string>();
				for (int i = 0; i < num; i++)
				{
					int ship_id = Comm_UserDatas.Instance.User_ship.get_Item(x[i]).Ship_id;
					list2.Add(Mst_DataManager.Instance.Mst_ship.get_Item(ship_id).Yomi);
				}
				IEnumerable<IGrouping<string, string>> enumerable = Enumerable.GroupBy<string, string>(list2, (string y) => y);
				return Enumerable.Count<IGrouping<string, string>>(enumerable) == Enumerable.Count<string>(list2);
			};
			Mem_ship mem_ship = null;
			return Comm_UserDatas.Instance.User_ship.TryGetValue(<IsValidChange>c__AnonStorey4BC.ship_rid, ref mem_ship) && !mem_ship.IsBlingShip() && mem_ship.BlingType != Mem_ship.BlingKind.WaitDeck && (mem_ship.BlingType != Mem_ship.BlingKind.WaitEscort || mem_ship.BlingWaitArea == mem_esccort_deck.Maparea_id) && !deckShip.Equals(deckShips) && func.Invoke(deckShips);
		}

		private bool validEscortStypeCheck(DeckShips deckship, int deck_targetIdx)
		{
			if (deckship.Count() == 0)
			{
				return true;
			}
			EscortCheckKinds escortCheckKinds = DeckUtils.IsValidChangeEscort(deckship.getMemShip(), this.mst_escort_group, deck_targetIdx);
			return escortCheckKinds == EscortCheckKinds.OK;
		}

		public bool Change_TempDeck(int deck_targetIdx, int ship_rid, ref DeckShips deckShip)
		{
			DeckShips deckShips = deckShip;
			if (ship_rid == -2)
			{
				deckShips.RemoveRange(1, deckShips.Count() - 1);
				return true;
			}
			int num = deckShips.Find(ship_rid);
			int num2 = deckShips[deck_targetIdx];
			deckShips[deck_targetIdx] = ship_rid;
			if (ship_rid == -1 || num == -1)
			{
				return true;
			}
			if (num2 != ship_rid)
			{
				deckShips[num] = num2;
			}
			else
			{
				deckShips[num] = -1;
			}
			return true;
		}

		public Api_Result<Mem_esccort_deck> Change(int deck_rid, DeckShips deckShip)
		{
			Api_Result<Mem_esccort_deck> api_Result = new Api_Result<Mem_esccort_deck>();
			Mem_esccort_deck deck = null;
			List<int> lastTempResult = null;
			deckShip.Clone(out lastTempResult);
			if (!Comm_UserDatas.Instance.User_EscortDeck.TryGetValue(deck_rid, ref deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<int> list = null;
			deck.Ship.Clone(out list);
			Dictionary<int, Mem_ship> cloneShips = Enumerable.ToDictionary<Mem_ship, int, Mem_ship>(deck.Ship.getMemShip(), (Mem_ship rid) => rid.Rid, (Mem_ship obj) => obj);
			deck.Ship.Clear();
			int count = lastTempResult.get_Count();
			for (int i = 0; i < count; i++)
			{
				deck.Ship[i] = lastTempResult.get_Item(i);
				Comm_UserDatas.Instance.User_ship.get_Item(deck.Ship[i]).BlingWaitToStop();
			}
			list.ForEach(delegate(int ship_rid)
			{
				if (!lastTempResult.Contains(ship_rid) && Comm_UserDatas.Instance.Temp_escortship.Contains(ship_rid))
				{
					cloneShips.get_Item(ship_rid).BlingWait(deck.Maparea_id, Mem_ship.BlingKind.WaitEscort);
				}
			});
			if (deck.Ship.Count() > 0 && deck.Disposition_status == DispositionStatus.NONE)
			{
				deck.GoArea(deck.Maparea_id);
			}
			else if (deck.Ship.Count() == 0)
			{
				deck.EscortStop();
			}
			else if (deck.Disposition_status == DispositionStatus.ARRIVAL && deck.GetBlingTurn() == 0)
			{
				if (Enumerable.All<Mem_ship>(deck.Ship.getMemShip(), (Mem_ship x) => x.GetBlingTurn() > 0))
				{
					deck.EscortStop();
					deck.GoArea(deck.Maparea_id);
				}
			}
			api_Result.data = deck;
			return api_Result;
		}
	}
}
