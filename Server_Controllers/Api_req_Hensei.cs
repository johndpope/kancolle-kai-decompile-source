using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Hensei
	{
		public bool IsValidChange(int deck_rid, int deck_targetIdx, int ship_rid)
		{
			Mem_deck deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref deck))
			{
				return false;
			}
			if (deck.MissionState != MissionStates.NONE || deck.IsActionEnd)
			{
				return false;
			}
			List<int> list = new List<int>();
			if (ship_rid == -2)
			{
				if (deck.Ship.Count() == 0)
				{
					return false;
				}
				deck.Ship.Clone(out list);
				list.RemoveAt(0);
			}
			else if (ship_rid == -1)
			{
				list.Add(deck.Ship[deck_targetIdx]);
			}
			else
			{
				list.Add(ship_rid);
				list.Add(deck.Ship[deck_targetIdx]);
				bool flag = Enumerable.Any<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Ship_id == ship_rid && x.Area_id != deck.Area_id);
				if (flag)
				{
					return false;
				}
			}
			if (Enumerable.Any<Mem_esccort_deck>(Comm_UserDatas.Instance.User_EscortDeck.get_Values(), (Mem_esccort_deck escort) => escort.Ship.Find(ship_rid) != -1))
			{
				return false;
			}
			if (ship_rid == -2 || (ship_rid == -1 && deck_targetIdx == 0 && deck.Rid == 1))
			{
				return deck.Ship.Count() > 1;
			}
			if (ship_rid == -1)
			{
				return deck.Area_id == 1 || deck.Ship.Count() != 1;
			}
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				return false;
			}
			if (mem_ship.IsBlingShip())
			{
				return false;
			}
			if (mem_ship.BlingType == Mem_ship.BlingKind.WaitEscort)
			{
				return false;
			}
			if (mem_ship.BlingType == Mem_ship.BlingKind.WaitDeck && mem_ship.BlingWaitArea != deck.Area_id)
			{
				return false;
			}
			DeckShips deckShips = null;
			deck.Ship.Clone(out deckShips);
			int[] array = deck.Search_ShipIdx(ship_rid);
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
			if (array[0] == deck_rid)
			{
				deckShips[array[1]] = deckShips[deck_targetIdx];
			}
			else if (array[0] != -1)
			{
				Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.get_Item(array[0]);
				if (mem_deck.MissionState != MissionStates.NONE || mem_deck.IsActionEnd)
				{
					return false;
				}
				if (mem_deck.Area_id != deck.Area_id)
				{
					return false;
				}
				DeckShips deckShips2 = null;
				mem_deck.Ship.Clone(out deckShips2);
				deckShips2[array[1]] = deckShips[deck_targetIdx];
				if (mem_deck.Rid == 1 && deckShips2.Count() == 0)
				{
					return false;
				}
				if (deckShips2.Count() == 0 && mem_deck.Area_id != 1)
				{
					return false;
				}
				if (!func.Invoke(deckShips2))
				{
					return false;
				}
			}
			deckShips[deck_targetIdx] = ship_rid;
			return func.Invoke(deckShips) && !deckShips.Equals(deck.Ship);
		}

		public Api_Result<Hashtable> Change(int deck_rid, int deck_targetIdx, int ship_rid)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			if (!this.IsValidChange(deck_rid, deck_targetIdx, ship_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_deck mem_deck = null;
			List<int> list = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_deck.Ship.Clone(out list);
			HashSet<int> inNdockShips = new HashSet<int>(Enumerable.Select<Mem_ndock, int>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Ship_id));
			Func<int, int, bool> func = (int area_id, int target_ship) => area_id != 1 && Comm_UserDatas.Instance.Temp_deckship.Contains(target_ship) && !inNdockShips.Contains(target_ship);
			if (ship_rid == -1)
			{
				mem_deck.Ship[deck_targetIdx] = ship_rid;
				bool flag = Comm_UserDatas.Instance.Temp_deckship.Contains(ship_rid);
				if (func.Invoke(mem_deck.Area_id, list.get_Item(deck_targetIdx)))
				{
					this.setBlingShip(list.get_Item(deck_targetIdx), mem_deck.Area_id, mem_deck.Rid);
				}
				new QuestHensei().ExecuteCheck();
				return api_Result;
			}
			api_Result.state = Api_Result_State.Success;
			if (ship_rid != -2)
			{
				int[] array = mem_deck.Search_ShipIdx(Comm_UserDatas.Instance.User_deck, ship_rid);
				Mem_deck mem_deck2 = null;
				List<int> list2 = null;
				if (array[0] > 0)
				{
					if (!Comm_UserDatas.Instance.User_deck.TryGetValue(array[0], ref mem_deck2))
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						return api_Result;
					}
					mem_deck2.Ship.Clone(out list2);
					mem_deck2.Ship[array[1]] = mem_deck.Ship[deck_targetIdx];
				}
				else if (!Comm_UserDatas.Instance.Temp_deckship.Contains(ship_rid) && mem_deck.Ship[deck_targetIdx] != -1)
				{
					if (func.Invoke(mem_deck.Area_id, mem_deck.Ship[deck_targetIdx]))
					{
						this.setBlingShip(list.get_Item(deck_targetIdx), mem_deck.Area_id, mem_deck.Rid);
					}
				}
				else if (mem_deck.Ship[deck_targetIdx] != -1 && Comm_UserDatas.Instance.User_ship.get_Item(ship_rid).BlingWaitArea == mem_deck.Area_id && func.Invoke(mem_deck.Area_id, mem_deck.Ship[deck_targetIdx]))
				{
					this.setBlingShip(list.get_Item(deck_targetIdx), mem_deck.Area_id, mem_deck.Rid);
				}
				Comm_UserDatas.Instance.User_ship.get_Item(ship_rid).BlingWaitToStop();
				mem_deck.Ship[deck_targetIdx] = ship_rid;
				new QuestHensei().ExecuteCheck();
				return api_Result;
			}
			if (mem_deck.Ship.Count() < 2)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_deck.Area_id != 1)
			{
				int num = Enumerable.Count<int>(list) - 1;
				for (int i = 1; i <= num; i++)
				{
					if (func.Invoke(mem_deck.Area_id, list.get_Item(i)))
					{
						this.setBlingShip(list.get_Item(i), mem_deck.Area_id, mem_deck.Rid);
					}
				}
			}
			int num2 = mem_deck.Ship.Count();
			mem_deck.Ship.RemoveRange(1, mem_deck.Ship.Count() - 1);
			api_Result.data = new Hashtable();
			api_Result.data.set_Item("change_count", num2 - mem_deck.Ship.Count());
			return api_Result;
		}

		public Api_Result<Mem_ship> Lock(int ship_rid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_ship.ChangeLockSts();
			api_Result.data = mem_ship;
			return api_Result;
		}

		private bool setBlingShip(int ship_rid, int area_id, int deck_rid)
		{
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				return false;
			}
			mem_ship.BlingWait(area_id, Mem_ship.BlingKind.WaitDeck);
			return true;
		}
	}
}
