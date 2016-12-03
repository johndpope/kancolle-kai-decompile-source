using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class BattleCommandParams : IDisposable
	{
		private int fspp;

		private int tspp;

		private int rspp;

		private bool isEscape;

		private BattleBaseData userBaseData;

		private int escapeNum;

		private HashSet<BattleCommand> enableCommand;

		private HashSet<BattleCommand> enableCommandOpening;

		private Dictionary<BattleCommand, Func<int, DayBattleProductionFmt>> commandFunc;

		private bool highSpeedFlag;

		public int Fspp
		{
			get
			{
				return this.fspp;
			}
			private set
			{
				this.fspp = value;
				if (this.fspp > 100)
				{
					this.fspp = 100;
				}
				else if (this.fspp < -100)
				{
					this.fspp = -100;
				}
			}
		}

		public int Tspp
		{
			get
			{
				return this.tspp;
			}
			private set
			{
				this.tspp = value;
				if (this.tspp > 100)
				{
					this.tspp = 100;
				}
				else if (this.tspp < -100)
				{
					this.tspp = -100;
				}
			}
		}

		public int Rspp
		{
			get
			{
				return this.rspp;
			}
			private set
			{
				this.rspp = value;
				if (this.rspp > 100)
				{
					this.rspp = 100;
				}
				else if (this.rspp < -100)
				{
					this.rspp = -100;
				}
			}
		}

		public bool IsEscape
		{
			get
			{
				return this.isEscape;
			}
			private set
			{
				this.isEscape = value;
			}
		}

		public BattleCommandParams(BattleBaseData userData)
		{
			this.isEscape = false;
			this.Fspp = 0;
			this.Tspp = 0;
			this.Rspp = 0;
			this.escapeNum = 0;
			this.userBaseData = userData;
			IEnumerable<int> enumerable = Enumerable.Select<Mem_ship, int>(this.userBaseData.ShipData, (Mem_ship x) => Mst_DataManager.Instance.Mst_ship.get_Item(x.Ship_id).Soku);
			this.highSpeedFlag = !Enumerable.Any<int>(enumerable, (int x) => x != 10);
			HashSet<BattleCommand> hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Sekkin);
			hashSet.Add(BattleCommand.Ridatu);
			hashSet.Add(BattleCommand.Kaihi);
			hashSet.Add(BattleCommand.Totugeki);
			hashSet.Add(BattleCommand.Tousha);
			hashSet.Add(BattleCommand.Raigeki);
			this.enableCommand = hashSet;
			hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Sekkin);
			hashSet.Add(BattleCommand.Ridatu);
			hashSet.Add(BattleCommand.Kaihi);
			this.enableCommandOpening = hashSet;
			Dictionary<BattleCommand, Func<int, DayBattleProductionFmt>> dictionary = new Dictionary<BattleCommand, Func<int, DayBattleProductionFmt>>();
			dictionary.Add(BattleCommand.Sekkin, new Func<int, DayBattleProductionFmt>(this.Sekkin));
			dictionary.Add(BattleCommand.Ridatu, new Func<int, DayBattleProductionFmt>(this.Ridatsu));
			dictionary.Add(BattleCommand.Kaihi, new Func<int, DayBattleProductionFmt>(this.Kaihi));
			dictionary.Add(BattleCommand.Totugeki, new Func<int, DayBattleProductionFmt>(this.Totsugeki));
			dictionary.Add(BattleCommand.Tousha, new Func<int, DayBattleProductionFmt>(this.Tousha));
			dictionary.Add(BattleCommand.Raigeki, new Func<int, DayBattleProductionFmt>(this.RaigPosture));
			this.commandFunc = dictionary;
		}

		public void Dispose()
		{
			this.enableCommand.Clear();
			this.enableCommandOpening.Clear();
			this.commandFunc.Clear();
		}

		public bool IsOpenengProductionCommand(BattleCommand command)
		{
			return this.enableCommandOpening.Contains(command);
		}

		public DayBattleProductionFmt GetProductionData(BattleCommand command)
		{
			if (!this.IsOpenengProductionCommand(command))
			{
				return null;
			}
			return this.commandFunc.get_Item(command).Invoke(0);
		}

		public DayBattleProductionFmt GetProductionData(int boxPos, BattleCommand command)
		{
			if (!this.enableCommand.Contains(command))
			{
				return null;
			}
			return this.commandFunc.get_Item(command).Invoke(boxPos);
		}

		private DayBattleProductionFmt RaigPosture(int index)
		{
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Raigeki;
			dayBattleProductionFmt.FSPP = this.Fspp;
			dayBattleProductionFmt.RSPP = this.Rspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			double max = Math.Sqrt((double)this.userBaseData.ShipData.get_Item(0).Level / 3.0);
			int num = 10 + (int)Utils.GetRandDouble(0.0, max, 1.0, 1);
			Mem_ship mem_ship = this.userBaseData.ShipData.get_Item(0);
			if (mem_ship.Lov >= 100)
			{
				double max2 = Math.Sqrt((double)mem_ship.Lov) / 3.0;
				double randDouble = Utils.GetRandDouble(0.0, max2, 0.1, 1);
				int num2 = (int)(randDouble + 0.5);
				num += num2;
			}
			if (this.highSpeedFlag)
			{
				num += 3;
			}
			this.Tspp += num;
			dayBattleProductionFmt.TSPP = this.Tspp;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Ridatsu(int index)
		{
			this.escapeNum++;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			bool flag = false;
			using (List<Mem_ship>.Enumerator enumerator = this.userBaseData.ShipData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					if (current.IsFight())
					{
						if (mst_ship.get_Item(current.Ship_id).Soku != 10)
						{
							flag = false;
							break;
						}
						flag = true;
					}
				}
			}
			DayBattleProductionFmt dayBattleProductionFmt = new DayBattleProductionFmt();
			dayBattleProductionFmt.productionKind = BattleCommand.Ridatu;
			dayBattleProductionFmt.FSPP = this.Fspp;
			dayBattleProductionFmt.RSPP = this.Rspp;
			dayBattleProductionFmt.TSPP = this.Tspp;
			dayBattleProductionFmt.BoxNo = index + 1;
			if (index == 0)
			{
				this.IsEscape = false;
				return dayBattleProductionFmt;
			}
			int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			if (flag && this.escapeNum >= 2 && num <= 80)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (flag && this.escapeNum == 1 && num <= 50)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (!flag && this.escapeNum >= 2 && num <= 60)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (!flag && this.escapeNum == 1 && num <= 35)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (index >= 3)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			if (index == 2 && num <= 65)
			{
				dayBattleProductionFmt.Withdrawal = true;
				this.IsEscape = true;
				return dayBattleProductionFmt;
			}
			this.IsEscape = false;
			return dayBattleProductionFmt;
		}

		private DayBattleProductionFmt Sekkin(int index)
		{
			Mem_ship mem_ship = this.userBaseData.ShipData.get_Item(0);
			double num = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 5)), 1.0, 1);
			double num2 = 7.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 3)), 1.0, 1);
			double num3 = -5.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 10)), 1.0, 1);
			if (this.highSpeedFlag)
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 8)), 1.0, 1);
				num2 = num2 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 8)), 1.0, 1);
			}
			else
			{
				num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 12)), 1.0, 1);
				num2 += Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 16)), 1.0, 1);
			}
			Mem_ship mem_ship2 = this.userBaseData.ShipData.get_Item(0);
			if (mem_ship2.Lov >= 100)
			{
				double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 3.0), 0.1, 1);
				int num4 = (int)(randDouble + 0.5);
				num += (double)num4;
				double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 5.0), 0.1, 1);
				int num5 = (int)(randDouble2 + 0.5);
				num2 += (double)num5;
			}
			this.Fspp += (int)num;
			this.Tspp += (int)num2;
			this.Rspp += (int)num3;
			return new DayBattleProductionFmt
			{
				productionKind = BattleCommand.Sekkin,
				FSPP = this.Fspp,
				RSPP = this.Rspp,
				TSPP = this.Tspp,
				BoxNo = index + 1
			};
		}

		private DayBattleProductionFmt Kaihi(int index)
		{
			Mem_ship mem_ship = this.userBaseData.ShipData.get_Item(0);
			double num = -4.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 20)), 1.0, 1);
			double num2 = -6.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 15)), 1.0, 1);
			double num3 = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 10)), 1.0, 1);
			if (this.highSpeedFlag)
			{
				num3 = num3 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 20)), 1.0, 1);
			}
			else
			{
				num3 += Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 25)), 1.0, 1);
			}
			this.Fspp += (int)num;
			this.Tspp += (int)num2;
			this.Rspp += (int)num3;
			return new DayBattleProductionFmt
			{
				productionKind = BattleCommand.Kaihi,
				FSPP = this.Fspp,
				RSPP = this.Rspp,
				TSPP = this.Tspp,
				BoxNo = index + 1
			};
		}

		private DayBattleProductionFmt Totsugeki(int index)
		{
			Mem_ship mem_ship = this.userBaseData.ShipData.get_Item(0);
			double num = 3.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 5)), 1.0, 1);
			double num2 = 5.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 3)), 1.0, 1);
			double num3 = -7.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 8)), 1.0, 1);
			if (this.highSpeedFlag)
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 8)), 1.0, 1);
				num2 = num2 + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 7)), 1.0, 1);
			}
			else
			{
				num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 12)), 1.0, 1);
				num2 += Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 15)), 1.0, 1);
			}
			Mem_ship mem_ship2 = this.userBaseData.ShipData.get_Item(0);
			if (mem_ship2.Lov >= 100)
			{
				double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 4.0), 0.1, 1);
				int num4 = (int)(randDouble + 0.5);
				num += (double)num4;
				double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt((double)mem_ship.Lov / 8.0), 0.1, 1);
				int num5 = (int)(randDouble2 + 0.5);
				num2 += (double)num5;
			}
			this.Fspp += (int)num;
			this.Tspp += (int)num2;
			this.Rspp += (int)num3;
			return new DayBattleProductionFmt
			{
				productionKind = BattleCommand.Totugeki,
				FSPP = this.Fspp,
				RSPP = this.Rspp,
				TSPP = this.Tspp,
				BoxNo = index + 1
			};
		}

		private DayBattleProductionFmt Tousha(int index)
		{
			List<Mst_slotitem> list = this.userBaseData.SlotData.get_Item(0);
			double num = 2.0;
			double num2 = 0.0;
			double num3 = -2.0;
			if (Enumerable.Any<Mst_slotitem>(list, (Mst_slotitem x) => (x.Api_mapbattle_type3 == 12 || x.Api_mapbattle_type3 == 13) && x.Tyku == 0))
			{
				num = num + 2.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(this.userBaseData.ShipData.get_Item(0).Level / 10)), 1.0, 1);
			}
			List<List<Mst_slotitem>> list2 = Enumerable.ToList<List<Mst_slotitem>>(Enumerable.Skip<List<Mst_slotitem>>(this.userBaseData.SlotData, 1));
			using (var enumerator = Enumerable.Select(list2, (List<Mst_slotitem> obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mem_ship mem_ship = this.userBaseData.ShipData.get_Item(current.ship_idx + 1);
					if (mem_ship.IsFight())
					{
						num = num + 1.0 + Utils.GetRandDouble(0.0, Math.Sqrt((double)(mem_ship.Level / 30)), 1.0, 1);
					}
				}
			}
			this.Fspp += (int)num;
			this.Tspp += (int)num2;
			this.Rspp += (int)num3;
			return new DayBattleProductionFmt
			{
				productionKind = BattleCommand.Tousha,
				FSPP = this.Fspp,
				RSPP = this.Rspp,
				TSPP = this.Tspp,
				BoxNo = index + 1
			};
		}
	}
}
