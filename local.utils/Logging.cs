using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;

namespace local.utils
{
	public static class Logging
	{
		public static void log(params object[] messages)
		{
			string text = string.Empty;
			for (int i = 0; i < messages.Length; i++)
			{
				text = text + messages[i].ToString() + "  ";
			}
			NGUIDebug.Log(new object[]
			{
				text
			});
		}

		public static void log(DeckModel[] decks)
		{
			string empty = string.Empty;
			for (int i = 0; i < decks.Length; i++)
			{
				Logging.log(new object[]
				{
					string.Format("{0}", decks[i])
				});
			}
			Logging.log(new object[]
			{
				empty
			});
		}

		public static void log(ShipModel[] ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				if (shipModel == null)
				{
					text += string.Format("[{0}] - \n", i);
				}
				else
				{
					text += string.Format("[{0}]{1} {2} Lv:{3}", new object[]
					{
						i,
						shipModel.ShipTypeName,
						shipModel.ShortName,
						shipModel.Level
					});
					text += string.Format(" {0}/{1}({2:F}% - {3})", new object[]
					{
						shipModel.NowHp,
						shipModel.MaxHp,
						shipModel.TaikyuRate,
						shipModel.DamageStatus
					});
					text += string.Format(" 疲労度:{0}", shipModel.Condition);
					DeckModelBase deck = shipModel.getDeck();
					if (deck != null && deck is DeckModel)
					{
						text += string.Format(" [艦隊ID:{0}{1}に編成中]", ((DeckModel)deck).Id, (!shipModel.IsInActionEndDeck()) ? string.Empty : "(行動終了済)");
					}
					else if (deck != null && deck is EscortDeckModel)
					{
						text += string.Format(" [護衛艦隊{0}に編成中]", ((EscortDeckModel)deck).Id);
					}
					text += string.Format(" {0}", (!shipModel.IsLocked()) ? string.Empty : "[ロック]");
					text += string.Format(" {0}", (!shipModel.IsInRepair()) ? string.Empty : "[入渠中]");
					text += string.Format(" {0}", (!shipModel.IsInMission()) ? string.Empty : "[遠征中]");
					text += string.Format(" {0}", (!shipModel.IsBling()) ? string.Empty : "[回航中]");
					if (shipModel.IsBlingWait())
					{
						text += string.Format("[回航待ち中(Area:{0})]", shipModel.AreaIdBeforeBlingWait);
					}
					text += string.Format(" {0}", (!shipModel.IsTettaiBling()) ? string.Empty : "[撤退中]");
					text += string.Format(" mstID:{0} memID:{1}", shipModel.MstId, shipModel.MemId);
					text += string.Format("\n", new object[0]);
				}
			}
			Logging.log(new object[]
			{
				text
			});
		}

		public static void log(SlotitemModel[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				SlotitemModel slotitemModel = items[i];
				if (slotitemModel == null)
				{
					text += string.Format("[{0}] - \n", i);
				}
				else
				{
					text += string.Format("[{0}]{1}\n", i, slotitemModel.ShortName);
				}
			}
			Logging.log(new object[]
			{
				text
			});
		}

		public static void log(ItemlistModel[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				ItemlistModel itemlistModel = items[i];
				if (itemlistModel != null)
				{
					text += string.Format("{0}{1}\n", itemlistModel, (!itemlistModel.IsUsable()) ? string.Empty : "[使用可能]");
				}
				else
				{
					text += " - \n";
				}
			}
			Logging.log(new object[]
			{
				text
			});
		}

		public static void log(ItemStoreModel[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				ItemStoreModel itemStoreModel = items[i];
				if (itemStoreModel != null)
				{
					text += string.Format("{0}\n", itemStoreModel);
				}
				else
				{
					text += " - \n";
				}
			}
			Logging.log(new object[]
			{
				text
			});
		}

		public static void log(IAlbumModel[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] != null)
				{
					Logging.log(new object[]
					{
						string.Format("[{0}]{1}", i, data[i])
					});
				}
			}
		}

		public static void log(List<IReward> rewards)
		{
			using (List<IReward>.Enumerator enumerator = rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IReward current = enumerator.get_Current();
					Logging.log(new object[]
					{
						current
					});
				}
			}
		}

		public static string ToString(List<IsGoCondition> conds)
		{
			string text = string.Empty;
			for (int i = 0; i < conds.get_Count(); i++)
			{
				text += conds.get_Item(i);
				if (i < conds.get_Count())
				{
					text += ", ";
				}
			}
			return text;
		}
	}
}
