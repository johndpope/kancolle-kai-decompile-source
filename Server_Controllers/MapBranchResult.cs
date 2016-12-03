using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class MapBranchResult
	{
		public enum enumScoutingKind
		{
			None,
			C,
			D,
			E,
			E2,
			K1,
			K2
		}

		public enum enumProductionKind
		{
			None,
			A,
			B,
			C1,
			C2
		}

		private Api_req_Map mapInstance;

		private Mst_mapcell2 mst_cell;

		private List<Mem_ship> mem_ship;

		private Dictionary<int, List<Mst_slotitem>> mst_slotitems;

		private Dictionary<int, int> slotitem_level;

		private Mem_mapclear mapClear;

		private MapCommentKind comment_kind;

		private MapProductionKind production_kind;

		private int user_level;

		private DifficultKind user_difficult;

		private Func<int> mapFunc;

		public MapBranchResult(Api_req_Map mapInstance)
		{
			this.mapInstance = mapInstance;
			this.mapFunc = null;
		}

		public bool getNextCellNo(out int cellNo, out MapCommentKind comment_kind, out MapProductionKind production_kind)
		{
			cellNo = 0;
			comment_kind = MapCommentKind.None;
			production_kind = MapProductionKind.None;
			this.init();
			if (this.mapFunc == null)
			{
				return false;
			}
			cellNo = this.mapFunc.Invoke();
			if (cellNo == 0)
			{
				return false;
			}
			comment_kind = this.comment_kind;
			production_kind = this.production_kind;
			return true;
		}

		private void init()
		{
			this.mst_cell = this.mapInstance.GetPrevCell();
			this.mapClear = this.mapInstance.GetMapClearState();
			this.mapInstance.GetSortieDeckInfo(this, out this.mem_ship, out this.mst_slotitems);
			this.slotitem_level = this.getSlotitemLevel();
			this.comment_kind = MapCommentKind.None;
			this.production_kind = MapProductionKind.None;
			this.user_level = Comm_UserDatas.Instance.User_record.Level;
			this.user_difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			this.selectFunc();
		}

		private void selectFunc()
		{
			if (this.mapFunc != null)
			{
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_11);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_12);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_13);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_14);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_15);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_16);
				return;
			}
			if (this.mst_cell.Maparea_id == 1 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_17);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_21);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_22);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_23);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_24);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_25);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_26);
				return;
			}
			if (this.mst_cell.Maparea_id == 2 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_27);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_31);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_32);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_33);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_34);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_35);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_36);
				return;
			}
			if (this.mst_cell.Maparea_id == 3 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_37);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_41);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_42);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_43);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_44);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_45);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_46);
				return;
			}
			if (this.mst_cell.Maparea_id == 4 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_47);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_51);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_52);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_53);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_54);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_55);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_56);
				return;
			}
			if (this.mst_cell.Maparea_id == 5 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_57);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_61);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_62);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_63);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_64);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_65);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_66);
				return;
			}
			if (this.mst_cell.Maparea_id == 6 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_67);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_71);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_72);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_73);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_74);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_75);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_76);
				return;
			}
			if (this.mst_cell.Maparea_id == 7 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_77);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_81);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_82);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_83);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_84);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_85);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_86);
				return;
			}
			if (this.mst_cell.Maparea_id == 8 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_87);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_91);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_92);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_93);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_94);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_95);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_96);
				return;
			}
			if (this.mst_cell.Maparea_id == 9 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_97);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_101);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_102);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_103);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_104);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_105);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_106);
				return;
			}
			if (this.mst_cell.Maparea_id == 10 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_107);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_111);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_112);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_113);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_114);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_115);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_116);
				return;
			}
			if (this.mst_cell.Maparea_id == 11 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_117);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_121);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_122);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_123);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_124);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_125);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_126);
				return;
			}
			if (this.mst_cell.Maparea_id == 12 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_127);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_131);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_132);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_133);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_134);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_135);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_136);
				return;
			}
			if (this.mst_cell.Maparea_id == 13 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_137);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_141);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_142);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_143);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_144);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_145);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_146);
				return;
			}
			if (this.mst_cell.Maparea_id == 14 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_147);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_151);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_152);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_153);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_154);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_155);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_156);
				return;
			}
			if (this.mst_cell.Maparea_id == 15 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_157);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_161);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_162);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_163);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_164);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_165);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_166);
				return;
			}
			if (this.mst_cell.Maparea_id == 16 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_167);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_171);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_172);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_173);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_174);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_175);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_176);
				return;
			}
			if (this.mst_cell.Maparea_id == 17 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_177);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_181);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_182);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_183);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_184);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_185);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_186);
				return;
			}
			if (this.mst_cell.Maparea_id == 18 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_187);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_191);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_192);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_193);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_194);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_195);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_196);
				return;
			}
			if (this.mst_cell.Maparea_id == 19 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_197);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 1)
			{
				this.mapFunc = new Func<int>(this.getMapCell_201);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 2)
			{
				this.mapFunc = new Func<int>(this.getMapCell_202);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 3)
			{
				this.mapFunc = new Func<int>(this.getMapCell_203);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 4)
			{
				this.mapFunc = new Func<int>(this.getMapCell_204);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 5)
			{
				this.mapFunc = new Func<int>(this.getMapCell_205);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 6)
			{
				this.mapFunc = new Func<int>(this.getMapCell_206);
				return;
			}
			if (this.mst_cell.Maparea_id == 20 && this.mst_cell.Mapinfo_no == 7)
			{
				this.mapFunc = new Func<int>(this.getMapCell_207);
				return;
			}
		}

		private int getMapCell_11()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No != 1)
			{
				return 0;
			}
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> expr_7A = dictionary2 = dictionary;
					int num;
					int expr_83 = num = current.Stype;
					num = dictionary2.get_Item(num);
					expr_7A.set_Item(expr_83, num + 1);
				}
			}
			if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
			{
				return this.mst_cell.Next_no_2;
			}
			int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			if (num2 <= 35)
			{
				return this.mst_cell.Next_no_1;
			}
			return this.mst_cell.Next_no_2;
		}

		private int getMapCell_12()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No != 0)
			{
				return 0;
			}
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> expr_79 = dictionary2 = dictionary;
					int num;
					int expr_82 = num = current.Stype;
					num = dictionary2.get_Item(num);
					expr_79.set_Item(expr_82, num + 1);
				}
			}
			if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
			{
				return this.mst_cell.Next_no_1;
			}
			int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			if (num2 <= 40)
			{
				return this.mst_cell.Next_no_2;
			}
			return this.mst_cell.Next_no_1;
		}

		private int getMapCell_13()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 65)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 70)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 3)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1AC = dictionary3 = dictionary;
						int num2;
						int expr_1B6 = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_1AC.set_Item(expr_1B6, num2 + 1);
					}
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 70)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_14()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				List<double> list = new List<double>();
				list.Add(30.0);
				list.Add(40.0);
				list.Add(30.0);
				int randomRateIndex = Utils.GetRandomRateIndex(list);
				if (randomRateIndex == 0)
				{
					return this.mst_cell.Next_no_3;
				}
				if (randomRateIndex == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 11)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_161 = dictionary2 = dictionary;
						int num2;
						int expr_16A = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_161.set_Item(expr_16A, num2 + 1);
					}
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 55)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_15()
		{
			return 0;
		}

		private int getMapCell_16()
		{
			return 0;
		}

		private int getMapCell_17()
		{
			return 0;
		}

		private int getMapCell_21()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 2 && num5 <= 35)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 12)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_267 = dictionary3 = dictionary;
						int num;
						int expr_271 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_267.set_Item(expr_271, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num7 >= 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_378 = dictionary4 = dictionary;
						int num;
						int expr_382 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_378.set_Item(expr_382, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num8 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_469 = dictionary5 = dictionary;
						int num;
						int expr_473 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_469.set_Item(expr_473, num + 1);
					}
				}
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num10 = dictionary.get_Item(11) + dictionary.get_Item(18);
				if (num10 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8 && this.mst_cell.No != 13)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_554 = dictionary6 = dictionary;
						int num;
						int expr_55E = num = current5.Stype;
						num = dictionary6.get_Item(num);
						expr_554.set_Item(expr_55E, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num11 += shipSakuParam2;
					}
				}
				int num12 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num12 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num11 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 12))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_22()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(6) + dictionary.get_Item(7) + dictionary.get_Item(10) + dictionary.get_Item(11) + dictionary.get_Item(16) + dictionary.get_Item(18);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3 && dictionary.get_Item(5) <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1AD = dictionary3 = dictionary;
						int num;
						int expr_1B7 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1AD.set_Item(expr_1B7, num + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 6 && this.mst_cell.No != 9)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2DE = dictionary4 = dictionary;
						int num;
						int expr_2E8 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2DE.set_Item(expr_2E8, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num5 += shipSakuParam;
					}
				}
				int num6 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num6 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 25))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_23()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1B4 = dictionary3 = dictionary;
						int num;
						int expr_1BE = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1B4.set_Item(expr_1BE, num + 1);
					}
				}
				int num5 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(10) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 <= 35)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2D7 = dictionary4 = dictionary;
						int num;
						int expr_2E1 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2D7.set_Item(expr_2E1, num + 1);
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num8 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num9 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 12)
			{
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_414 = dictionary5 = dictionary;
						int num;
						int expr_41E = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_414.set_Item(expr_41E, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num10 += shipSakuParam;
					}
				}
				int num11 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num11 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 7)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_522 = dictionary6 = dictionary;
						int num;
						int expr_52C = num = current5.Stype;
						num = dictionary6.get_Item(num);
						expr_522.set_Item(expr_52C, num + 1);
					}
				}
				int num12 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num13 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num14 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num13 >= 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num13 == 5 && num14 <= 75)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num13 == 4 && num14 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num12 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_24()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3 && num5 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_21C = dictionary3 = dictionary;
						int num;
						int expr_226 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_21C.set_Item(expr_226, num + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num7 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 <= 2 && dictionary.get_Item(3) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 4 && dictionary.get_Item(3) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3 && dictionary.get_Item(2) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 13)
			{
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num10 += shipSakuParam;
					}
				}
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 33))
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num11 += shipSakuParam2;
					}
				}
				if (num11 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_25()
		{
			return 0;
		}

		private int getMapCell_26()
		{
			return 0;
		}

		private int getMapCell_27()
		{
			return 0;
		}

		private int getMapCell_31()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 70)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_14C = dictionary3 = dictionary;
						int num;
						int expr_156 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_14C.set_Item(expr_156, num + 1);
					}
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(16) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 3)
				{
					return 0;
				}
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_21A = dictionary4 = dictionary;
						int num;
						int expr_224 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_21A.set_Item(expr_224, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 == 0 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 <= 60)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_32()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(2) <= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 4)
				{
					return this.mst_cell.Next_no_3;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4 && num2 <= 25)
				{
					return this.mst_cell.Next_no_3;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) == 6)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1C5 = dictionary3 = dictionary;
						int num;
						int expr_1CF = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1C5.set_Item(expr_1CF, num + 1);
					}
				}
				if (dictionary.get_Item(2) <= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 5 && this.mst_cell.No != 10)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_298 = dictionary4 = dictionary;
						int num;
						int expr_2A2 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_298.set_Item(expr_2A2, num + 1);
					}
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(2) == 6)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 5 && num4 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4 && num3 <= 90)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_33()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 3)
			{
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8 && this.mst_cell.No != 12)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1BE = dictionary3 = dictionary;
						int num;
						int expr_1C8 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1BE.set_Item(expr_1C8, num + 1);
					}
				}
				int num5 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_3;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num6 <= 80)
				{
					return this.mst_cell.Next_no_3;
				}
				num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 == 0 && dictionary.get_Item(3) >= 1 && num6 <= 70)
				{
					return this.mst_cell.Next_no_3;
				}
				num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_3;
			}
		}

		private int getMapCell_34()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_167 = dictionary3 = dictionary;
						int num;
						int expr_171 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_167.set_Item(expr_171, num + 1);
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 2 && num6 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 3)
				{
					return 0;
				}
				bool flag = true;
				bool flag2 = false;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2D8 = dictionary4 = dictionary;
						int num;
						int expr_2E2 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2D8.set_Item(expr_2E2, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Yomi == "あきつしま")
						{
							flag2 = true;
						}
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num8 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				if (flag2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_35()
		{
			return 0;
		}

		private int getMapCell_36()
		{
			return 0;
		}

		private int getMapCell_37()
		{
			return 0;
		}

		private int getMapCell_41()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num4 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 7 && this.mst_cell.No != 14)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_187 = dictionary3 = dictionary;
						int num;
						int expr_191 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_187.set_Item(expr_191, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num5 += shipSakuParam;
					}
				}
				if (dictionary.get_Item(2) == 0 && this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 46)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 1 && this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 37)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 2 && this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 28)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_42()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				if (num2 + dictionary.get_Item(7) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 + dictionary.get_Item(7) >= 3 && dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 3 && dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5)
			{
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num3 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num3, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num4 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 22)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_43()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num6 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num6 == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 <= 2 && num7 <= 75)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 15)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_288 = dictionary3 = dictionary;
						int num;
						int expr_292 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_288.set_Item(expr_292, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num10 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num11 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num12 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag && num8 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag && num8 >= 2 && num12 <= 75)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag && num8 >= 1 && num12 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num11 == 0 && num9 >= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num8 == 0)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_4E9 = dictionary4 = dictionary;
						int num;
						int expr_4F3 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_4E9.set_Item(expr_4F3, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num13 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num14 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num15 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num13 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num14 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num15 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 16)
			{
				double num16 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_666 = dictionary5 = dictionary;
						int num;
						int expr_670 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_666.set_Item(expr_670, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num16 += shipSakuParam;
					}
				}
				int num17 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num17 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.getMapSakuParam(num16, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 55)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 10 && this.mst_cell.No != 18)
				{
					return 0;
				}
				double num18 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num18 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num18, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_44()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 1)
			{
				bool flag2 = false;
				bool flag3 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_193 = dictionary3 = dictionary;
						int num;
						int expr_19D = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_193.set_Item(expr_19D, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Yomi == "あきつしま")
						{
							flag2 = true;
						}
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag3 = false;
						}
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (flag2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 17)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_305 = dictionary4 = dictionary;
						int num;
						int expr_30F = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_305.set_Item(expr_30F, num + 1);
					}
				}
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num7 == 5)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num6 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 == 5)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 9 || this.mst_cell.No == 19)
			{
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num8 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num8, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 50)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 12)
				{
					return 0;
				}
				double num9 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_549 = dictionary5 = dictionary;
						int num;
						int expr_553 = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_549.set_Item(expr_553, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num9 += shipSakuParam2;
					}
				}
				int num10 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				int mapSakuParam = this.getMapSakuParam(num9, MapBranchResult.enumScoutingKind.K2);
				if (num10 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (mapSakuParam < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 32 && num11 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				if (mapSakuParam < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_3;
			}
		}

		private int getMapCell_45()
		{
			return 0;
		}

		private int getMapCell_46()
		{
			return 0;
		}

		private int getMapCell_47()
		{
			return 0;
		}

		private int getMapCell_51()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 3 && dictionary.get_Item(3) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1FB = dictionary3 = dictionary;
						int num;
						int expr_205 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1FB.set_Item(expr_205, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_31A = dictionary4 = dictionary;
						int num;
						int expr_324 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_31A.set_Item(expr_324, num + 1);
					}
				}
				int num5 = 0;
				using (Dictionary<int, List<Mst_slotitem>>.Enumerator enumerator4 = this.mst_slotitems.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						KeyValuePair<int, List<Mst_slotitem>> current4 = enumerator4.get_Current();
						num5 += this.getDrumCount(current4.get_Value());
					}
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(10) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 6)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_4CF = dictionary5 = dictionary;
						int num;
						int expr_4D9 = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_4CF.set_Item(expr_4D9, num + 1);
					}
				}
				int num7 = 0;
				using (Dictionary<int, List<Mst_slotitem>>.Enumerator enumerator6 = this.mst_slotitems.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						KeyValuePair<int, List<Mst_slotitem>> current6 = enumerator6.get_Current();
						num7 += this.getDrumCount(current6.get_Value());
					}
				}
				if (dictionary.get_Item(10) >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_52()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18);
				if (num2 == 2 && dictionary.get_Item(3) == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_122 = dictionary3 = dictionary;
						int num;
						int expr_12C = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_122.set_Item(expr_12C, num + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num4 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 == 2 && dictionary.get_Item(7) == 1 && num4 <= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_1F4 = dictionary4 = dictionary;
						int num;
						int expr_1FE = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_1F4.set_Item(expr_1FE, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 11)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_2F7 = dictionary5 = dictionary;
						int num;
						int expr_301 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_2F7.set_Item(expr_301, num + 1);
					}
				}
				int num6 = 0;
				using (Dictionary<int, List<Mst_slotitem>>.Enumerator enumerator5 = this.mst_slotitems.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						KeyValuePair<int, List<Mst_slotitem>> current5 = enumerator5.get_Current();
						num6 += this.getDrumCount(current5.get_Value());
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num8 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 2 && dictionary.get_Item(7) >= 1 && num8 <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 7)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_461 = dictionary6 = dictionary;
						int num;
						int expr_46B = num = current6.Stype;
						num = dictionary6.get_Item(num);
						expr_461.set_Item(expr_46B, num + 1);
					}
				}
				int num10 = 0;
				using (Dictionary<int, List<Mst_slotitem>>.Enumerator enumerator7 = this.mst_slotitems.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						KeyValuePair<int, List<Mst_slotitem>> current7 = enumerator7.get_Current();
						num10 += this.getDrumCount(current7.get_Value());
					}
				}
				if (num10 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_53()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 2)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7C = dictionary2 = dictionary;
						int num;
						int expr_85 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7C.set_Item(expr_85, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (!flag && num2 <= 75)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 12)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_166 = dictionary3 = dictionary;
						int num;
						int expr_170 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_166.set_Item(expr_170, num + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(5) + dictionary.get_Item(6) >= 2 && dictionary.get_Item(3) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				List<double> list = new List<double>();
				list.Add(10.0);
				list.Add(30.0);
				list.Add(60.0);
				int randomRateIndex = Utils.GetRandomRateIndex(list);
				if (randomRateIndex == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				if (randomRateIndex == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 8)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_30D = dictionary4 = dictionary;
						int num;
						int expr_317 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_30D.set_Item(expr_317, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num6 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (!flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 4 && dictionary.get_Item(3) == 1 && dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 3 && this.mst_cell.No != 13 && this.mst_cell.No != 14)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_4A0 = dictionary5 = dictionary;
						int num;
						int expr_4AA = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_4A0.set_Item(expr_4AA, num + 1);
					}
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 <= 25)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_54()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1F3 = dictionary3 = dictionary;
						int num;
						int expr_1FD = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1F3.set_Item(expr_1FD, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num5 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num6 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 9)
			{
				bool flag3 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_317 = dictionary4 = dictionary;
						int num;
						int expr_321 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_317.set_Item(expr_321, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag3 = false;
						}
					}
				}
				int num7 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (!flag3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 12 && this.mst_cell.No != 18)
				{
					return 0;
				}
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_453 = dictionary5 = dictionary;
						int num;
						int expr_45D = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_453.set_Item(expr_45D, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num8 += shipSakuParam;
					}
				}
				int num9 = 0;
				using (Dictionary<int, List<Mst_slotitem>>.Enumerator enumerator5 = this.mst_slotitems.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						KeyValuePair<int, List<Mst_slotitem>> current5 = enumerator5.get_Current();
						num9 += this.getDrumCount(current5.get_Value());
					}
				}
				if (num8 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2 && num9 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num8 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 50))
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_55()
		{
			return 0;
		}

		private int getMapCell_56()
		{
			return 0;
		}

		private int getMapCell_57()
		{
			return 0;
		}

		private int getMapCell_61()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num6 = dictionary.get_Item(2) + dictionary.get_Item(3);
				if (num2 + num3 + num4 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 == 5 && this.mem_ship.get_Count() == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 == 4 && this.mem_ship.get_Count() == 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 == 3 && this.mem_ship.get_Count() == 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1 && num5 == 3 && dictionary.get_Item(2) == 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1 && num5 == 4 && num6 == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1 && num5 == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1 && num5 == 4 && this.mem_ship.get_Count() == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1 && num5 == 3 && this.mem_ship.get_Count() == 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_2FC = dictionary3 = dictionary;
						int num;
						int expr_306 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_2FC.set_Item(expr_306, num + 1);
					}
				}
				if (dictionary.get_Item(20) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				double num7 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_399 = dictionary4 = dictionary;
						int num;
						int expr_3A3 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_399.set_Item(expr_3A3, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num7 += shipSakuParam;
					}
				}
				int num8 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num10 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num11 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num12 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 + num9 + num10 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num11 <= 2 && num12 <= 35)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num13 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_507 = dictionary5 = dictionary;
						int num;
						int expr_511 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_507.set_Item(expr_511, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num13 += shipSakuParam2;
					}
				}
				int num14 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num15 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.getMapSakuParam(num13, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 7)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(20) == 1)
				{
					this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
					return this.mst_cell.Next_no_3;
				}
				if (this.getMapSakuParam(num13, MapBranchResult.enumScoutingKind.K2) > (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 10)
				{
					this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
					return this.mst_cell.Next_no_3;
				}
				if (num14 <= 3 && num15 <= 35 && dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num14 <= 2 && num15 <= 70 && dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_3;
			}
		}

		private int getMapCell_62()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 <= 20)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 <= 35)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_202 = dictionary3 = dictionary;
						int num;
						int expr_20C = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_202.set_Item(expr_20C, num + 1);
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num6 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num4 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2FA = dictionary4 = dictionary;
						int num;
						int expr_304 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2FA.set_Item(expr_304, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num10 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				if (num7 == 0 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num8 <= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3 && num9 == 1 && num10 == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3 && num9 == 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2 && flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3 && num8 <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_524 = dictionary5 = dictionary;
						int num;
						int expr_52E = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_524.set_Item(expr_52E, num + 1);
					}
				}
				int num11 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num12 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num11 >= 4 && num12 <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num11 >= 3 && num12 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 5 && this.mst_cell.No != 10)
				{
					return 0;
				}
				double num13 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current5.Ship_id, slotSakuParam);
						num13 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num13, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 12)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_63()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 5 || this.mst_cell.No == 11)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_8E = dictionary2 = dictionary;
						int num;
						int expr_97 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_8E.set_Item(expr_97, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(16) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) <= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8 && this.mst_cell.No != 12)
				{
					return 0;
				}
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num3 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num3, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_64()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(7) + dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(17);
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 <= 35)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1FF = dictionary3 = dictionary;
						int num;
						int expr_209 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1FF.set_Item(expr_209, num + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num7 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 == 0 && num8 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3 || this.mst_cell.No == 12)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_339 = dictionary4 = dictionary;
						int num;
						int expr_343 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_339.set_Item(expr_343, num + 1);
					}
				}
				int num9 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num10 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num11 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num12 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num13 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num9 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 + num11 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 + num9 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num12 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num12 + num13 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 13)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_4CB = dictionary5 = dictionary;
						int num;
						int expr_4D5 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_4CB.set_Item(expr_4D5, num + 1);
					}
				}
				int num14 = dictionary.get_Item(2) + dictionary.get_Item(3);
				int num15 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num16 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (dictionary.get_Item(2) <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num14 == 6)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num15 + num16 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num14 == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 5)
			{
				double num17 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_5ED = dictionary6 = dictionary;
						int num;
						int expr_5F7 = num = current5.Stype;
						num = dictionary6.get_Item(num);
						expr_5ED.set_Item(expr_5F7, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current5.Ship_id, slotSakuParam);
						num17 += shipSakuParam;
					}
				}
				int num18 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num19 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num20 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num20 >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num19 >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num19 + num20 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num18 >= 5)
				{
					return this.mst_cell.Next_no_3;
				}
				if (this.getMapSakuParam(num17, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8)
			{
				double num21 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						Dictionary<int, int> dictionary7;
						Dictionary<int, int> expr_79A = dictionary7 = dictionary;
						int num;
						int expr_7A4 = num = current6.Stype;
						num = dictionary7.get_Item(num);
						expr_79A.set_Item(expr_7A4, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current6.Ship_id, slotSakuParam2);
						num21 += shipSakuParam2;
					}
				}
				int num22 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (this.getMapSakuParam(num21, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 13)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num22 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9 && this.mst_cell.No != 16)
				{
					return 0;
				}
				double num23 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator7 = this.mem_ship.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						Mem_ship current7 = enumerator7.get_Current();
						Dictionary<int, int> dictionary8;
						Dictionary<int, int> expr_8CC = dictionary8 = dictionary;
						int num;
						int expr_8D6 = num = current7.Stype;
						num = dictionary8.get_Item(num);
						expr_8CC.set_Item(expr_8D6, num + 1);
						double slotSakuParam3 = this.getSlotSakuParam(current7, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current7.Ship_id, slotSakuParam3);
						num23 += shipSakuParam3;
					}
				}
				int num24 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (this.getMapSakuParam(num23, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 36)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num24 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_65()
		{
			return 0;
		}

		private int getMapCell_66()
		{
			return 0;
		}

		private int getMapCell_67()
		{
			return 0;
		}

		private int getMapCell_71()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5)
			{
				double num2 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current.Ship_id, slotSakuParam);
						num2 += shipSakuParam;
					}
				}
				if (num2 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 5))
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8)
			{
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_18F = dictionary2 = dictionary;
						int num4;
						int expr_199 = num4 = current2.Stype;
						num4 = dictionary2.get_Item(num4);
						expr_18F.set_Item(expr_199, num4 + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current2.Ship_id, slotSakuParam2);
						num3 += shipSakuParam2;
					}
				}
				if (dictionary.get_Item(3) <= 1)
				{
					this.setCommentData(MapBranchResult.enumProductionKind.A, ref this.comment_kind, ref this.production_kind);
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 3))
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else
			{
				if (this.mst_cell.No != 9 && this.mst_cell.No != 13)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_2C5 = dictionary3 = dictionary;
						int num4;
						int expr_2CF = num4 = current3.Stype;
						num4 = dictionary3.get_Item(num4);
						expr_2C5.set_Item(expr_2CF, num4 + 1);
						double slotSakuParam3 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current3.Ship_id, slotSakuParam3);
						num5 += shipSakuParam3;
					}
				}
				if (dictionary.get_Item(3) >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 5))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.A, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_72()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(4) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 + num5 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1CD = dictionary3 = dictionary;
						int num;
						int expr_1D7 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1CD.set_Item(expr_1D7, num + 1);
					}
				}
				int num6 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num8 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				if (num6 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 + num8 >= 6)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2CC = dictionary4 = dictionary;
						int num;
						int expr_2D6 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2CC.set_Item(expr_2D6, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				if (dictionary.get_Item(3) == 2 && dictionary.get_Item(2) == 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) == 1 && dictionary.get_Item(2) >= 4 && flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) == 1 && dictionary.get_Item(3) == 1 && dictionary.get_Item(2) == 3 && flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) == 2 && dictionary.get_Item(3) == 1 && dictionary.get_Item(2) == 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 13)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_43B = dictionary5 = dictionary;
						int num;
						int expr_445 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_43B.set_Item(expr_445, num + 1);
					}
				}
				int num9 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num10 = dictionary.get_Item(11) + dictionary.get_Item(18);
				if (dictionary.get_Item(4) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 + num9 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 14)
			{
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current5.Ship_id, slotSakuParam);
						num11 += shipSakuParam;
					}
				}
				if (num11 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 15))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num12 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current6.Ship_id, slotSakuParam2);
						num12 += shipSakuParam2;
					}
				}
				if (num12 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 20))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_73()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				double num2 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_D8 = dictionary2 = dictionary;
						int num3;
						int expr_E1 = num3 = current.Stype;
						num3 = dictionary2.get_Item(num3);
						expr_D8.set_Item(expr_E1, num3 + 1);
						double slotSakuParam = this.getSlotSakuParam(current, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current.Ship_id, slotSakuParam);
						num2 += shipSakuParam;
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num4 >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num5 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 20))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 12)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_233 = dictionary3 = dictionary;
						int num3;
						int expr_23D = num3 = current2.Stype;
						num3 = dictionary3.get_Item(num3);
						expr_233.set_Item(expr_23D, num3 + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num8 = dictionary.get_Item(2) + dictionary.get_Item(3);
				if (num6 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 + num7 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 5)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_338 = dictionary4 = dictionary;
						int num3;
						int expr_342 = num3 = current3.Stype;
						num3 = dictionary4.get_Item(num3);
						expr_338.set_Item(expr_342, num3 + 1);
					}
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 7 && this.mst_cell.No != 14)
				{
					return 0;
				}
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_42C = dictionary5 = dictionary;
						int num3;
						int expr_436 = num3 = current4.Stype;
						num3 = dictionary5.get_Item(num3);
						expr_42C.set_Item(expr_436, num3 + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num10 += shipSakuParam2;
					}
				}
				int num11 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num11 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 25))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_74()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(4) == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1A5 = dictionary3 = dictionary;
						int num;
						int expr_1AF = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1A5.set_Item(expr_1AF, num + 1);
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num4 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 14)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_29A = dictionary4 = dictionary;
						int num;
						int expr_2A4 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_29A.set_Item(expr_2A4, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (!flag && num8 <= 70)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 15)
			{
				double num9 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_3FC = dictionary5 = dictionary;
						int num;
						int expr_406 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_3FC.set_Item(expr_406, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num9 += shipSakuParam;
					}
				}
				if (num9 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 35))
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num10 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_3;
			}
			else
			{
				if (this.mst_cell.No != 7)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_527 = dictionary6 = dictionary;
						int num;
						int expr_531 = num = current5.Stype;
						num = dictionary6.get_Item(num);
						expr_527.set_Item(expr_531, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num11 += shipSakuParam2;
					}
				}
				int num12 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num11 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30))
				{
					return this.mst_cell.Next_no_1;
				}
				if (num12 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				int num13 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num13 <= 50)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_75()
		{
			return 0;
		}

		private int getMapCell_76()
		{
			return 0;
		}

		private int getMapCell_77()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No != 0)
			{
				return 0;
			}
			bool flag = true;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> expr_7B = dictionary2 = dictionary;
					int num;
					int expr_84 = num = current.Stype;
					num = dictionary2.get_Item(num);
					expr_7B.set_Item(expr_84, num + 1);
					if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
					{
						flag = false;
					}
				}
			}
			int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
			int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
			if (num3 >= 1)
			{
				return this.mst_cell.Next_no_1;
			}
			if (!flag)
			{
				return this.mst_cell.Next_no_1;
			}
			if (num2 >= 4)
			{
				return this.mst_cell.Next_no_1;
			}
			if (dictionary.get_Item(2) == 0 && dictionary.get_Item(3) == 0)
			{
				return this.mst_cell.Next_no_1;
			}
			return this.mst_cell.Next_no_2;
		}

		private int getMapCell_81()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 5)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_82()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = dictionary.get_Item(7) + dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(17);
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 35)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_224 = dictionary3 = dictionary;
						int num2;
						int expr_22E = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_224.set_Item(expr_22E, num2 + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = dictionary.get_Item(7) + dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(17);
				if (num6 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_83()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(5) >= 1 && dictionary.get_Item(5) <= 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 4 && this.mst_cell.No != 9)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_232 = dictionary3 = dictionary;
						int num2;
						int expr_23C = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_232.set_Item(expr_23C, num2 + 1);
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num6 = dictionary.get_Item(7) + dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(17);
				if (num5 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 1 && num6 <= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(17) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_84()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(5) >= 1 && dictionary.get_Item(5) <= 2 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 25)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_247 = dictionary3 = dictionary;
						int num2;
						int expr_251 = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_247.set_Item(expr_251, num2 + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num6 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 5 && this.mst_cell.No != 10)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_352 = dictionary4 = dictionary;
						int num2;
						int expr_35C = num2 = current3.Stype;
						num2 = dictionary4.get_Item(num2);
						expr_352.set_Item(expr_35C, num2 + 1);
					}
				}
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num8 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(17) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num10 <= 35)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_85()
		{
			return 0;
		}

		private int getMapCell_86()
		{
			return 0;
		}

		private int getMapCell_87()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 3 && this.mst_cell.No != 8)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_140 = dictionary3 = dictionary;
						int num;
						int expr_14A = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_140.set_Item(expr_14A, num + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(5) >= 1 && dictionary.get_Item(5) <= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(3) <= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_91()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(6) + dictionary.get_Item(7) + dictionary.get_Item(10) + dictionary.get_Item(11) + dictionary.get_Item(16) + dictionary.get_Item(18);
				if (num2 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 5)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_18C = dictionary3 = dictionary;
						int num;
						int expr_196 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_18C.set_Item(expr_196, num + 1);
					}
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(10) >= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(17) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2BE = dictionary4 = dictionary;
						int num;
						int expr_2C8 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2BE.set_Item(expr_2C8, num + 1);
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num7 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 6 && this.mst_cell.No != 12)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_3C8 = dictionary5 = dictionary;
						int num;
						int expr_3D2 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_3C8.set_Item(expr_3D2, num + 1);
					}
				}
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num9 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_92()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			int count = this.mem_ship.get_Count();
			if (this.mst_cell.No == 2)
			{
				if (count <= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_EE = dictionary2 = dictionary;
						int num2;
						int expr_F7 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_EE.set_Item(expr_F7, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (count >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 25)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1D8 = dictionary3 = dictionary;
						int num2;
						int expr_1E2 = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_1D8.set_Item(expr_1E2, num2 + 1);
					}
				}
				int num5 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num6 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				int num7 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (count >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 5)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2E7 = dictionary4 = dictionary;
						int num2;
						int expr_2F1 = num2 = current3.Stype;
						num2 = dictionary4.get_Item(num2);
						expr_2E7.set_Item(expr_2F1, num2 + 1);
					}
				}
				int num8 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (count <= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(7) + dictionary.get_Item(17) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(16) + dictionary.get_Item(6) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_93()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				if (dictionary.get_Item(7) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(10) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(5) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(6) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) <= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_16C = dictionary3 = dictionary;
						int num;
						int expr_175 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_16C.set_Item(expr_175, num + 1);
					}
				}
				if (dictionary.get_Item(3) == 1 && dictionary.get_Item(2) == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 75)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 7)
				{
					return 0;
				}
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_24D = dictionary4 = dictionary;
						int num;
						int expr_257 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_24D.set_Item(expr_257, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num3 += shipSakuParam;
					}
				}
				if (dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(7) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 7))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_94()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 1 || this.mst_cell.No == 11)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1A0 = dictionary3 = dictionary;
						int num;
						int expr_1AA = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1A0.set_Item(expr_1AA, num + 1);
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num6 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1 || dictionary.get_Item(3) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 5)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2A5 = dictionary4 = dictionary;
						int num;
						int expr_2AF = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2A5.set_Item(expr_2AF, num + 1);
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (dictionary.get_Item(10) >= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(17) >= 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_95()
		{
			return 0;
		}

		private int getMapCell_96()
		{
			return 0;
		}

		private int getMapCell_97()
		{
			return 0;
		}

		private int getMapCell_101()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No != 5)
			{
				return 0;
			}
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> expr_7A = dictionary2 = dictionary;
					int num;
					int expr_83 = num = current.Stype;
					num = dictionary2.get_Item(num);
					expr_7A.set_Item(expr_83, num + 1);
				}
			}
			int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
			int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
			{
				return this.mst_cell.Next_no_1;
			}
			if (num2 >= 1)
			{
				return this.mst_cell.Next_no_2;
			}
			if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
			{
				return this.mst_cell.Next_no_1;
			}
			if (dictionary.get_Item(2) <= 1 && num3 <= 60)
			{
				return this.mst_cell.Next_no_2;
			}
			if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
			{
				return this.mst_cell.Next_no_1;
			}
			return this.mst_cell.Next_no_2;
		}

		private int getMapCell_102()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(10) >= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag && num2 == 0 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2 && num2 == 0 && num3 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_103()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 3)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7C = dictionary2 = dictionary;
						int num;
						int expr_85 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7C.set_Item(expr_85, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2 && num4 <= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 2 && flag)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 5)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num5 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 37)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_104()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 <= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6)
			{
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 32)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 10)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num5 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 42)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_105()
		{
			return 0;
		}

		private int getMapCell_106()
		{
			return 0;
		}

		private int getMapCell_107()
		{
			return 0;
		}

		private int getMapCell_111()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2 || this.mst_cell.No == 8)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1B3 = dictionary3 = dictionary;
						int num2;
						int expr_1BD = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_1B3.set_Item(expr_1BD, num2 + 1);
					}
				}
				int num4 = dictionary.get_Item(6) + dictionary.get_Item(7) + dictionary.get_Item(10) + dictionary.get_Item(11) + dictionary.get_Item(16) + dictionary.get_Item(18);
				if (num4 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 3)
				{
					return 0;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_112()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_CD = dictionary2 = dictionary;
						int num2;
						int expr_D6 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_CD.set_Item(expr_D6, num2 + 1);
					}
				}
				int num3 = dictionary.get_Item(6) + dictionary.get_Item(7) + dictionary.get_Item(10) + dictionary.get_Item(11) + dictionary.get_Item(16) + dictionary.get_Item(18);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 1 && num4 <= 75)
				{
					return this.mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 12)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_204 = dictionary3 = dictionary;
						int num2;
						int expr_20E = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_204.set_Item(expr_20E, num2 + 1);
					}
				}
				if (dictionary.get_Item(16) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 70)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 7)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2CF = dictionary4 = dictionary;
						int num2;
						int expr_2D9 = num2 = current3.Stype;
						num2 = dictionary4.get_Item(num2);
						expr_2CF.set_Item(expr_2D9, num2 + 1);
					}
				}
				if (this.mem_ship.get_Item(0).Stype == 14)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3)
				{
					return this.mst_cell.Next_no_1;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(10) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_113()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(3) >= 2 && num2 == 0 && num3 <= 80)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && num2 == 0 && num3 <= 65)
				{
					return this.mst_cell.Next_no_2;
				}
				List<double> list = new List<double>();
				list.Add(35.0);
				list.Add(35.0);
				list.Add(30.0);
				int randomRateIndex = Utils.GetRandomRateIndex(list);
				if (randomRateIndex == 0)
				{
					return this.mst_cell.Next_no_3;
				}
				if (randomRateIndex == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1DD = dictionary3 = dictionary;
						int num;
						int expr_1E7 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1DD.set_Item(expr_1E7, num + 1);
					}
				}
				int num4 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num4 >= 3 && dictionary.get_Item(20) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 4 && dictionary.get_Item(14) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 8)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2F8 = dictionary4 = dictionary;
						int num;
						int expr_302 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2F8.set_Item(expr_302, num + 1);
					}
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 70)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 11 || this.mst_cell.No == 19)
			{
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 14)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_439 = dictionary5 = dictionary;
						int num;
						int expr_443 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_439.set_Item(expr_443, num + 1);
					}
				}
				int num8 = dictionary.get_Item(6) + dictionary.get_Item(7) + dictionary.get_Item(10) + dictionary.get_Item(11) + dictionary.get_Item(16) + dictionary.get_Item(18);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 <= 65)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_114()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				int num = 0;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num2;
						int expr_84 = num2 = current.Stype;
						num2 = dictionary2.get_Item(num2);
						expr_7B.set_Item(expr_84, num2 + 1);
						if (this.getDrumCount(this.mst_slotitems.get_Item(current.Rid)) > 0)
						{
							num++;
						}
					}
				}
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 1 || dictionary.get_Item(7) >= 1 || dictionary.get_Item(16) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 1)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_235 = dictionary3 = dictionary;
						int num2;
						int expr_23F = num2 = current2.Stype;
						num2 = dictionary3.get_Item(num2);
						expr_235.set_Item(expr_23F, num2 + 1);
					}
				}
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2D4 = dictionary4 = dictionary;
						int num2;
						int expr_2DE = num2 = current3.Stype;
						num2 = dictionary4.get_Item(num2);
						expr_2D4.set_Item(expr_2DE, num2 + 1);
					}
				}
				int num8 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num10 = dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num8 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 >= 2 && dictionary.get_Item(7) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 >= 2 && num11 <= 25)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1 && num11 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(16) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(8) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_478 = dictionary5 = dictionary;
						int num2;
						int expr_482 = num2 = current4.Stype;
						num2 = dictionary5.get_Item(num2);
						expr_478.set_Item(expr_482, num2 + 1);
					}
				}
				int num12 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num13 = dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num12 == 0 || dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(6) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(16) >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				int num14 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num13 >= 1 && num14 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				int num15 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num15 <= 25)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6)
			{
				int num16 = 0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_621 = dictionary6 = dictionary;
						int num2;
						int expr_62B = num2 = current5.Stype;
						num2 = dictionary6.get_Item(num2);
						expr_621.set_Item(expr_62B, num2 + 1);
						if (this.getDrumCount(this.mst_slotitems.get_Item(current5.Rid)) > 0)
						{
							num16++;
						}
					}
				}
				int num17 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num18 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num17 >= 1 || dictionary.get_Item(7) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num18 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num16 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(5) == 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(6) == 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				int num19 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num19 <= 35)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 13 || this.mst_cell.No == 14)
			{
				double num20 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current6.Ship_id, slotSakuParam);
						num20 += shipSakuParam;
					}
				}
				if (num20 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				int num21 = 0;
				double num22 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator7 = this.mem_ship.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						Mem_ship current7 = enumerator7.get_Current();
						if (this.getDrumCount(this.mst_slotitems.get_Item(current7.Rid)) > 0)
						{
							num21++;
						}
						double slotSakuParam2 = this.getSlotSakuParam(current7, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current7.Ship_id, slotSakuParam2);
						num22 += shipSakuParam2;
					}
				}
				if (num21 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num22 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25))
				{
					return this.mst_cell.Next_no_2;
				}
				if (num22 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40))
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_3;
			}
		}

		private int getMapCell_115()
		{
			return 0;
		}

		private int getMapCell_116()
		{
			return 0;
		}

		private int getMapCell_117()
		{
			return 0;
		}

		private int getMapCell_121()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(3) + dictionary.get_Item(21);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 2 && num3 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Count() == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 13)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_183 = dictionary3 = dictionary;
						int num;
						int expr_18D = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_183.set_Item(expr_18D, num + 1);
					}
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.mem_ship.get_Count() <= 4 && num4 <= 75)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.mem_ship.get_Count() <= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 8)
			{
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num5 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 6)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num6 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_122()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 1)
			{
				bool flag = false;
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7F = dictionary2 = dictionary;
						int num;
						int expr_88 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7F.set_Item(expr_88, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 5)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_26D = dictionary3 = dictionary;
						int num;
						int expr_277 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_26D.set_Item(expr_277, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num7 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else
			{
				if (this.mst_cell.No != 8 && this.mst_cell.No != 12)
				{
					return 0;
				}
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num8 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num8, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_123()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag && num2 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1DA = dictionary3 = dictionary;
						int num;
						int expr_1E4 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1DA.set_Item(expr_1E4, num + 1);
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 12)
			{
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_3BB = dictionary4 = dictionary;
						int num;
						int expr_3C5 = num = current4.Stype;
						num = dictionary4.get_Item(num);
						expr_3BB.set_Item(expr_3C5, num + 1);
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num7 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				double num9 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_4AE = dictionary5 = dictionary;
						int num;
						int expr_4B8 = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_4AE.set_Item(expr_4B8, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num9 += shipSakuParam2;
					}
				}
				int num10 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num10 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.getMapSakuParam(num9, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 42)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_5CD = dictionary6 = dictionary;
						int num;
						int expr_5D7 = num = current6.Stype;
						num = dictionary6.get_Item(num);
						expr_5CD.set_Item(expr_5D7, num + 1);
						double slotSakuParam3 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current6.Ship_id, slotSakuParam3);
						num11 += shipSakuParam3;
					}
				}
				int num12 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num12 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 32)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_124()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag && num2 <= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag && dictionary.get_Item(6) >= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_21D = dictionary3 = dictionary;
						int num;
						int expr_227 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_21D.set_Item(expr_227, num + 1);
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				bool flag2 = true;
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_332 = dictionary4 = dictionary;
						int num;
						int expr_33C = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_332.set_Item(expr_33C, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num8 += shipSakuParam;
					}
				}
				int num9 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num10 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.getMapSakuParam(num8, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 28)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 <= 2 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(6) >= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num10 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num11 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				double num12 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_544 = dictionary5 = dictionary;
						int num;
						int expr_54E = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_544.set_Item(expr_54E, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num12 += shipSakuParam2;
					}
				}
				int num13 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num13 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (this.getMapSakuParam(num12, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 43)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 5)
			{
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_658 = dictionary6 = dictionary;
						int num;
						int expr_662 = num = current5.Stype;
						num = dictionary6.get_Item(num);
						expr_658.set_Item(expr_662, num + 1);
					}
				}
				int num14 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num15 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num16 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num14 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num15 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num16 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				double num17 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						Dictionary<int, int> dictionary7;
						Dictionary<int, int> expr_77C = dictionary7 = dictionary;
						int num;
						int expr_786 = num = current6.Stype;
						num = dictionary7.get_Item(num);
						expr_77C.set_Item(expr_786, num + 1);
						double slotSakuParam3 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current6.Ship_id, slotSakuParam3);
						num17 += shipSakuParam3;
					}
				}
				int num18 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num18 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.getMapSakuParam(num17, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8 && this.mst_cell.No != 14)
				{
					return 0;
				}
				double num19 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator7 = this.mem_ship.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						Mem_ship current7 = enumerator7.get_Current();
						double slotSakuParam4 = this.getSlotSakuParam(current7, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam4 = this.getShipSakuParam(current7.Ship_id, slotSakuParam4);
						num19 += shipSakuParam4;
					}
				}
				if (this.getMapSakuParam(num19, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 48)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_125()
		{
			return 0;
		}

		private int getMapCell_126()
		{
			return 0;
		}

		private int getMapCell_127()
		{
			return 0;
		}

		private int getMapCell_131()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(22) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(19) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				double num2 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_161 = dictionary3 = dictionary;
						int num;
						int expr_16B = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_161.set_Item(expr_16B, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num2 += shipSakuParam;
					}
				}
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num4 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				if (this.getMapSakuParam(num2, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 6)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 5 && this.mst_cell.No != 8)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2C0 = dictionary4 = dictionary;
						int num;
						int expr_2CA = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2C0.set_Item(expr_2CA, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num5 += shipSakuParam2;
					}
				}
				int num6 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num7 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				int num8 = dictionary.get_Item(3) + dictionary.get_Item(21);
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 + dictionary.get_Item(2) <= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_132()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 30)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 3)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1A6 = dictionary3 = dictionary;
						int num;
						int expr_1B0 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1A6.set_Item(expr_1B0, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num5 = dictionary.get_Item(7) + dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(17);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 13)
			{
				double num9 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num9 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num9, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num10 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num10, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_133()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num2 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 <= 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 2)
			{
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_182 = dictionary3 = dictionary;
						int num;
						int expr_18C = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_182.set_Item(expr_18C, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 20)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2DF = dictionary4 = dictionary;
						int num;
						int expr_2E9 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2DF.set_Item(expr_2E9, num + 1);
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num8 = dictionary.get_Item(5) + dictionary.get_Item(6);
				if (num7 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num8 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 13)
			{
				double num9 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num9 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num9, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 33)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_49E = dictionary5 = dictionary;
						int num;
						int expr_4A8 = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_49E.set_Item(expr_4A8, num + 1);
						double slotSakuParam3 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current5.Ship_id, slotSakuParam3);
						num10 += shipSakuParam3;
					}
				}
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.getMapSakuParam(num10, MapBranchResult.enumScoutingKind.K2) > (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 15)
				{
					this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
					return this.mst_cell.Next_no_3;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num11 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						Dictionary<int, int> dictionary6;
						Dictionary<int, int> expr_60D = dictionary6 = dictionary;
						int num;
						int expr_617 = num = current6.Stype;
						num = dictionary6.get_Item(num);
						expr_60D.set_Item(expr_617, num + 1);
					}
				}
				int num12 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num12 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_134()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				if (num2 >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_115 = dictionary3 = dictionary;
						int num;
						int expr_11F = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_115.set_Item(expr_11F, num + 1);
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 <= 3 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 <= 2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_238 = dictionary4 = dictionary;
						int num;
						int expr_242 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_238.set_Item(expr_242, num + 1);
					}
				}
				int num5 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num6 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 9 || this.mst_cell.No == 15)
			{
				double num7 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num7 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num7, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 11)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_422 = dictionary5 = dictionary;
						int num;
						int expr_42C = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_422.set_Item(expr_42C, num + 1);
					}
				}
				if (dictionary.get_Item(2) + dictionary.get_Item(3) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
		}

		private int getMapCell_135()
		{
			return 0;
		}

		private int getMapCell_136()
		{
			return 0;
		}

		private int getMapCell_137()
		{
			return 0;
		}

		private int getMapCell_141()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 4)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7C = dictionary2 = dictionary;
						int num;
						int expr_85 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7C.set_Item(expr_85, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				int num3 = dictionary.get_Item(5) + dictionary.get_Item(6);
				if (dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 + num3 <= 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 10)
			{
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1C1 = dictionary3 = dictionary;
						int num;
						int expr_1CB = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1C1.set_Item(expr_1CB, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				int num5 = dictionary.get_Item(3) + dictionary.get_Item(4) + dictionary.get_Item(21);
				int num6 = dictionary.get_Item(5) + dictionary.get_Item(6);
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num5 + num6 >= 5)
				{
					return this.mst_cell.Next_no_3;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 6 && this.mst_cell.No != 11)
				{
					return 0;
				}
				double num7 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_326 = dictionary4 = dictionary;
						int num;
						int expr_330 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_326.set_Item(expr_330, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num7 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num7, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 20)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_142()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1B0 = dictionary3 = dictionary;
						int num;
						int expr_1BA = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1B0.set_Item(expr_1BA, num + 1);
					}
				}
				int num4 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2CD = dictionary4 = dictionary;
						int num;
						int expr_2D7 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2CD.set_Item(expr_2D7, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8)
			{
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num8 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num8, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 33)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 12)
				{
					return 0;
				}
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_4D2 = dictionary5 = dictionary;
						int num;
						int expr_4DC = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_4D2.set_Item(expr_4DC, num + 1);
					}
				}
				int num9 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num10 = dictionary.get_Item(3) + dictionary.get_Item(21);
				int num11 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num11 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num10 + dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_143()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 2)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 >= 4 && num5 <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 3 && num5 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 4 && num5 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1FC = dictionary3 = dictionary;
						int num;
						int expr_206 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1FC.set_Item(expr_206, num + 1);
					}
				}
				if (dictionary.get_Item(17) >= 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2A8 = dictionary4 = dictionary;
						int num;
						int expr_2B2 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2A8.set_Item(expr_2B2, num + 1);
					}
				}
				int num6 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(3) + dictionary.get_Item(21);
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 4 && num10 <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3 && num10 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 4 && num10 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) + num9 == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8)
			{
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num11 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num12 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_51D = dictionary5 = dictionary;
						int num;
						int expr_527 = num = current5.Stype;
						num = dictionary5.get_Item(num);
						expr_51D.set_Item(expr_527, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num12 += shipSakuParam2;
					}
				}
				int num13 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num14 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.getMapSakuParam(num12, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 46)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num13 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num13 >= 5 && num14 <= 75)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num13 >= 4 && num14 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_3;
			}
		}

		private int getMapCell_144()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 == 6)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num5 >= 5 && num6 <= 75)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num5 >= 4 && num6 <= 50)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num2 >= 5)
				{
					return this.mst_cell.Next_no_3;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 15)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_291 = dictionary3 = dictionary;
						int num;
						int expr_29B = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_291.set_Item(expr_29B, num + 1);
					}
				}
				int num7 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10) + dictionary.get_Item(5) + dictionary.get_Item(6) + dictionary.get_Item(4);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num9 == 6)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 == 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(17) >= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (num8 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 9)
			{
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num10 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num10, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 38)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 10 && this.mst_cell.No != 16)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num11 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 43)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_145()
		{
			return 0;
		}

		private int getMapCell_146()
		{
			return 0;
		}

		private int getMapCell_147()
		{
			return 0;
		}

		private int getMapCell_151()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 4)
			{
				bool flag = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7C = dictionary2 = dictionary;
						int num;
						int expr_85 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7C.set_Item(expr_85, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag = true;
						}
					}
				}
				if (dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 13)
			{
				double num2 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num2 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num2, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 8)
				{
					return 0;
				}
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_2FF = dictionary3 = dictionary;
						int num;
						int expr_309 = num = current4.Stype;
						num = dictionary3.get_Item(num);
						expr_2FF.set_Item(expr_309, num + 1);
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num3 += shipSakuParam2;
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.getMapSakuParam(num3, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_152()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 0)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6)
			{
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num3 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num3, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 10)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_22B = dictionary3 = dictionary;
						int num;
						int expr_235 = num = current3.Stype;
						num = dictionary3.get_Item(num);
						expr_22B.set_Item(expr_235, num + 1);
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num4 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 12)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num5 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_153()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				bool flag2 = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7E = dictionary2 = dictionary;
						int num;
						int expr_87 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7E.set_Item(expr_87, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag2 = true;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_3;
				}
				if (flag2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 9)
			{
				double num3 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num3 += shipSakuParam;
					}
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (this.getMapSakuParam(num3, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				if (num4 <= 40)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 11)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num5 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_154()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				bool flag2 = false;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7E = dictionary2 = dictionary;
						int num;
						int expr_87 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7E.set_Item(expr_87, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Yomi == "あきつしま")
						{
							flag2 = true;
						}
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag2 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num2 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 12)
			{
				bool flag3 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_22B = dictionary3 = dictionary;
						int num;
						int expr_235 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_22B.set_Item(expr_235, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag3 = false;
						}
					}
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag3 && this.mem_ship.get_Item(0).Stype == 3 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag3 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2 && num3 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag3 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_392 = dictionary4 = dictionary;
						int num;
						int expr_39C = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_392.set_Item(expr_39C, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				if (dictionary.get_Item(2) >= 3 && this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 15)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) == 2 && this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 20)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(2) <= 1 && this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_155()
		{
			return 0;
		}

		private int getMapCell_156()
		{
			return 0;
		}

		private int getMapCell_157()
		{
			return 0;
		}

		private int getMapCell_161()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(20);
				int count = this.mem_ship.get_Count();
				if (num2 == count)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 5 && num3 == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 4 && num3 == 1 && count == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 3 && num3 == 1 && count == 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && count == 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 4 && num3 == 1 && dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 3 && num3 == 1 && dictionary.get_Item(2) == 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(2) == 2 && count == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_268 = dictionary3 = dictionary;
						int num;
						int expr_272 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_268.set_Item(expr_272, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(16) > 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 8)
			{
				bool flag2 = false;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_38E = dictionary4 = dictionary;
						int num;
						int expr_398 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_38E.set_Item(expr_398, num + 1);
						if (Enumerable.Count<Mst_slotitem>(this.mst_slotitems.get_Item(current3.Rid), (Mst_slotitem x) => x.Id == 68) > 0)
						{
							flag2 = true;
						}
					}
				}
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 4)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(17) + dictionary.get_Item(16) > 0)
				{
					return this.mst_cell.Next_no_1;
				}
				if (flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 9 || this.mst_cell.No == 16 || this.mst_cell.No == 17)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 8)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 12 && this.mst_cell.No != 18)
				{
					return 0;
				}
				double num7 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num7 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num7, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_162()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(20);
				int count = this.mem_ship.get_Count();
				if (num2 == count)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 5 && num3 == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 4 && num3 == 1 && count == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 3 && num3 == 1 && count == 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && count == 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 4 && num3 == 1 && dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 3 && num3 == 1 && dictionary.get_Item(2) == 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(2) == 2 && count == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4)
			{
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_265 = dictionary3 = dictionary;
						int num;
						int expr_26F = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_265.set_Item(expr_26F, num + 1);
					}
				}
				int num4 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num5 = dictionary.get_Item(20);
				int count2 = this.mem_ship.get_Count();
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num4 == count2 && num6 <= 40)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 6)
			{
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_35D = dictionary4 = dictionary;
						int num;
						int expr_367 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_35D.set_Item(expr_367, num + 1);
					}
				}
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num8 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int count3 = this.mem_ship.get_Count();
				if (num7 == count3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 15)
			{
				double num10 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num10 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num10, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 8)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 10)
				{
					return 0;
				}
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num11 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_163()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(20);
				int count = this.mem_ship.get_Count();
				if (num2 == count)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 5 && num3 == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 4 && num3 == 1 && count == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 3 && num3 == 1 && count == 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && count == 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 4 && num3 == 1 && dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 3 && num3 == 1 && dictionary.get_Item(2) == 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(2) == 2 && count == 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6 || this.mst_cell.No == 14)
			{
				double num4 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current2, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current2.Ship_id, slotSakuParam);
						num4 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num4, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 9)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 11 && this.mst_cell.No != 16)
				{
					return 0;
				}
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current3.Ship_id, slotSakuParam2);
						num5 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_164()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_79 = dictionary2 = dictionary;
						int num;
						int expr_82 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_79.set_Item(expr_82, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(20);
				int count = this.mem_ship.get_Count();
				if (num2 == count)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 5 && num3 == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 4 && num3 == 1 && count == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 3 && num3 == 1 && count == 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && count == 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 4 && num3 == 1 && dictionary.get_Item(2) == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 3 && num3 == 1 && dictionary.get_Item(2) == 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 == 2 && num3 == 1 && dictionary.get_Item(2) == 2 && count == 5)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_268 = dictionary3 = dictionary;
						int num;
						int expr_272 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_268.set_Item(expr_272, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num5 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 15 || this.mst_cell.No == 16)
			{
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_3AF = dictionary4 = dictionary;
						int num;
						int expr_3B9 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_3AF.set_Item(expr_3B9, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num6 += shipSakuParam;
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num7 >= 3 || dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_3;
			}
			else
			{
				if (this.mst_cell.No != 10 && this.mst_cell.No != 18)
				{
					return 0;
				}
				double num8 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num8 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num8, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 10)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_165()
		{
			return 0;
		}

		private int getMapCell_166()
		{
			return 0;
		}

		private int getMapCell_167()
		{
			return 0;
		}

		private int getMapCell_171()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 3)
			{
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7A = dictionary2 = dictionary;
						int num;
						int expr_83 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7A.set_Item(expr_83, num + 1);
					}
				}
				int num2 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num3 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num4 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 + num4 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(2) == 0)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num5 <= 50)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_1;
			}
			else if (this.mst_cell.No == 4)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_1AC = dictionary3 = dictionary;
						int num;
						int expr_1B6 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_1AC.set_Item(expr_1B6, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num6 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num6 >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 5 || this.mst_cell.No == 13)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_309 = dictionary4 = dictionary;
						int num;
						int expr_313 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_309.set_Item(expr_313, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num7 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num8 = dictionary.get_Item(5) + dictionary.get_Item(6);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 + num9 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num8 >= 2 && num10 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 6)
			{
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_45B = dictionary5 = dictionary;
						int num;
						int expr_465 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_45B.set_Item(expr_465, num + 1);
						double slotSakuParam = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current4.Ship_id, slotSakuParam);
						num11 += shipSakuParam;
					}
				}
				int num12 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				if (dictionary.get_Item(17) + num12 >= 1)
				{
					return this.mst_cell.Next_no_3;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 7 || this.mst_cell.No == 14)
			{
				double num13 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current5.Ship_id, slotSakuParam2);
						num13 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num13, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9 && this.mst_cell.No != 15)
				{
					return 0;
				}
				double num14 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						double slotSakuParam3 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam3 = this.getShipSakuParam(current6.Ship_id, slotSakuParam3);
						num14 += shipSakuParam3;
					}
				}
				if (this.getMapSakuParam(num14, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 18)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_172()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 3)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_16A = dictionary3 = dictionary;
						int num;
						int expr_174 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_16A.set_Item(expr_174, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num3 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num4 = dictionary.get_Item(20);
				int num5 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(7);
				int num6 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (num3 >= 4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num3 >= 3 && num4 == 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num5 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num6 >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 4 || this.mst_cell.No == 12)
			{
				bool flag3 = true;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						Dictionary<int, int> dictionary4;
						Dictionary<int, int> expr_2DA = dictionary4 = dictionary;
						int num;
						int expr_2E4 = num = current3.Stype;
						num = dictionary4.get_Item(num);
						expr_2DA.set_Item(expr_2E4, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Soku < 10)
						{
							flag3 = false;
						}
					}
				}
				int num7 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (dictionary.get_Item(2) <= 1)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num7 >= 3)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(6) + dictionary.get_Item(16) + dictionary.get_Item(2) >= 4)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 7)
			{
				bool flag4 = true;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						Dictionary<int, int> dictionary5;
						Dictionary<int, int> expr_437 = dictionary5 = dictionary;
						int num;
						int expr_441 = num = current4.Stype;
						num = dictionary5.get_Item(num);
						expr_437.set_Item(expr_441, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current4.Ship_id).Soku < 10)
						{
							flag4 = false;
						}
					}
				}
				int num8 = dictionary.get_Item(13) + dictionary.get_Item(14);
				int num9 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (!flag4)
				{
					return this.mst_cell.Next_no_2;
				}
				if (num9 >= 5)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_1;
				}
				if (dictionary.get_Item(16) + dictionary.get_Item(2) >= 3)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 <= 2 && num10 <= 70)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 == 3 && num10 <= 50)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num9 == 4 && num10 <= 30)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 8 || this.mst_cell.No == 14 || this.mst_cell.No == 15)
			{
				double num11 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator5 = this.mem_ship.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						Mem_ship current5 = enumerator5.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current5, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current5.Ship_id, slotSakuParam);
						num11 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num11, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 9)
				{
					return 0;
				}
				double num12 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator6 = this.mem_ship.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						Mem_ship current6 = enumerator6.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current6, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current6.Ship_id, slotSakuParam2);
						num12 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num12, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 16)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_173()
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<Mst_stype, int, int>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (this.mst_cell.No == 0)
			{
				bool flag = true;
				using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						Dictionary<int, int> dictionary2;
						Dictionary<int, int> expr_7B = dictionary2 = dictionary;
						int num;
						int expr_84 = num = current.Stype;
						num = dictionary2.get_Item(num);
						expr_7B.set_Item(expr_84, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id).Soku < 10)
						{
							flag = false;
						}
					}
				}
				int num2 = dictionary.get_Item(11) + dictionary.get_Item(18) + dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (!flag)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num2 >= 5)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_2;
			}
			else if (this.mst_cell.No == 2 || this.mst_cell.No == 15)
			{
				bool flag2 = true;
				using (List<Mem_ship>.Enumerator enumerator2 = this.mem_ship.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_17C = dictionary3 = dictionary;
						int num;
						int expr_186 = num = current2.Stype;
						num = dictionary3.get_Item(num);
						expr_17C.set_Item(expr_186, num + 1);
						if (Mst_DataManager.Instance.Mst_ship.get_Item(current2.Ship_id).Soku < 10)
						{
							flag2 = false;
						}
					}
				}
				int num3 = dictionary.get_Item(11) + dictionary.get_Item(18);
				int num4 = dictionary.get_Item(8) + dictionary.get_Item(9) + dictionary.get_Item(10);
				if (dictionary.get_Item(3) >= 1 && dictionary.get_Item(2) >= 2)
				{
					return this.mst_cell.Next_no_2;
				}
				if (dictionary.get_Item(16) + dictionary.get_Item(2) >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 80)
				{
					return this.mst_cell.Next_no_2;
				}
				if (!flag2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 75)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num3 >= 3 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if (num4 >= 3 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return this.mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 35)
				{
					return this.mst_cell.Next_no_1;
				}
				return this.mst_cell.Next_no_3;
			}
			else if (this.mst_cell.No == 10)
			{
				double num5 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator3 = this.mem_ship.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						double slotSakuParam = this.getSlotSakuParam(current3, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam = this.getShipSakuParam(current3.Ship_id, slotSakuParam);
						num5 += shipSakuParam;
					}
				}
				if (this.getMapSakuParam(num5, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 55)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
			else
			{
				if (this.mst_cell.No != 11)
				{
					return 0;
				}
				double num6 = 0.0;
				using (List<Mem_ship>.Enumerator enumerator4 = this.mem_ship.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Mem_ship current4 = enumerator4.get_Current();
						double slotSakuParam2 = this.getSlotSakuParam(current4, MapBranchResult.enumScoutingKind.K2);
						double shipSakuParam2 = this.getShipSakuParam(current4.Ship_id, slotSakuParam2);
						num6 += shipSakuParam2;
					}
				}
				if (this.getMapSakuParam(num6, MapBranchResult.enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return this.mst_cell.Next_no_1;
				}
				this.setCommentData(MapBranchResult.enumProductionKind.B, ref this.comment_kind, ref this.production_kind);
				return this.mst_cell.Next_no_2;
			}
		}

		private int getMapCell_174()
		{
			return 0;
		}

		private int getMapCell_175()
		{
			return 0;
		}

		private int getMapCell_176()
		{
			return 0;
		}

		private int getMapCell_177()
		{
			return 0;
		}

		private int getMapCell_181()
		{
			return 0;
		}

		private int getMapCell_182()
		{
			return 0;
		}

		private int getMapCell_183()
		{
			return 0;
		}

		private int getMapCell_184()
		{
			return 0;
		}

		private int getMapCell_185()
		{
			return 0;
		}

		private int getMapCell_186()
		{
			return 0;
		}

		private int getMapCell_187()
		{
			return 0;
		}

		private int getMapCell_191()
		{
			return 0;
		}

		private int getMapCell_192()
		{
			return 0;
		}

		private int getMapCell_193()
		{
			return 0;
		}

		private int getMapCell_194()
		{
			return 0;
		}

		private int getMapCell_195()
		{
			return 0;
		}

		private int getMapCell_196()
		{
			return 0;
		}

		private int getMapCell_197()
		{
			return 0;
		}

		private int getMapCell_201()
		{
			return 0;
		}

		private int getMapCell_202()
		{
			return 0;
		}

		private int getMapCell_203()
		{
			return 0;
		}

		private int getMapCell_204()
		{
			return 0;
		}

		private int getMapCell_205()
		{
			return 0;
		}

		private int getMapCell_206()
		{
			return 0;
		}

		private int getMapCell_207()
		{
			return 0;
		}

		private Dictionary<int, int> getSlotitemLevel()
		{
			Dictionary<int, int> ret = new Dictionary<int, int>();
			this.mem_ship.ForEach(delegate(Mem_ship ship)
			{
				ship.Slot.ForEach(delegate(int slot)
				{
					if (slot > 0)
					{
						ret.Add(slot, Comm_UserDatas.Instance.User_slot.get_Item(slot).Level);
					}
				});
			});
			return ret;
		}

		private double getSlotSakuParam(Mem_ship ship, MapBranchResult.enumScoutingKind scouting_kind)
		{
			double num = 0.0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
			double num2 = 0.0;
			Dictionary<int, double> dictionary3;
			if (scouting_kind == MapBranchResult.enumScoutingKind.C || scouting_kind == MapBranchResult.enumScoutingKind.D)
			{
				dictionary3 = new Dictionary<int, double>();
				dictionary3.Add(10, 1.2);
				dictionary3.Add(11, 1.1);
				dictionary3.Add(9, 1.0);
				dictionary3.Add(8, 0.8);
				dictionary = dictionary3;
				num2 = 0.6;
			}
			else if (scouting_kind == MapBranchResult.enumScoutingKind.E)
			{
				dictionary3 = new Dictionary<int, double>();
				dictionary3.Add(10, 4.8);
				dictionary3.Add(11, 4.4);
				dictionary3.Add(9, 4.0);
				dictionary3.Add(8, 3.2);
				dictionary = dictionary3;
				num2 = 2.4;
			}
			else if (scouting_kind == MapBranchResult.enumScoutingKind.E2 || scouting_kind == MapBranchResult.enumScoutingKind.K1 || scouting_kind == MapBranchResult.enumScoutingKind.K2)
			{
				dictionary3 = new Dictionary<int, double>();
				dictionary3.Add(10, 3.5999999999999996);
				dictionary3.Add(11, 3.3000000000000003);
				dictionary3.Add(9, 3.0);
				dictionary3.Add(8, 2.4000000000000004);
				dictionary = dictionary3;
				num2 = 1.7999999999999998;
			}
			List<int> list = new List<int>();
			list.Add(9);
			list.Add(10);
			list.Add(11);
			list.Add(12);
			list.Add(13);
			list.Add(26);
			list.Add(41);
			List<int> list2 = list;
			dictionary3 = new Dictionary<int, double>();
			dictionary3.Add(13, 1.4);
			dictionary3.Add(12, 1.25);
			dictionary3.Add(9, 1.2);
			dictionary3.Add(10, 1.2);
			dictionary3.Add(41, 1.2);
			dictionary3.Add(11, 1.15);
			dictionary3.Add(26, 1.0);
			dictionary2 = dictionary3;
			double num3 = 0.0;
			double num4 = 0.0;
			for (int i = 0; i < Enumerable.Count<Mst_slotitem>(this.mst_slotitems.get_Item(ship.Rid)); i++)
			{
				Mst_slotitem mst_slotitem = this.mst_slotitems.get_Item(ship.Rid).get_Item(i);
				int api_mapbattle_type = mst_slotitem.Api_mapbattle_type3;
				if (!dictionary.TryGetValue(api_mapbattle_type, ref num3))
				{
					num3 = num2;
				}
				double num5 = 0.0;
				if (list2.Contains(api_mapbattle_type))
				{
					if (!dictionary2.TryGetValue(api_mapbattle_type, ref num4))
					{
						num4 = 1.0;
					}
					num5 = Math.Sqrt((double)this.slotitem_level.get_Item(ship.Slot.get_Item(i))) * num4;
				}
				num += ((double)mst_slotitem.Saku + num5) * num3;
			}
			if (scouting_kind == MapBranchResult.enumScoutingKind.K2)
			{
				Dictionary<DifficultKind, int> dictionary4 = new Dictionary<DifficultKind, int>();
				dictionary4.Add(DifficultKind.TEI, 1);
				dictionary4.Add(DifficultKind.HEI, 3);
				dictionary4.Add(DifficultKind.OTU, 6);
				dictionary4.Add(DifficultKind.KOU, 12);
				dictionary4.Add(DifficultKind.SHI, 18);
				Dictionary<DifficultKind, int> dictionary5 = dictionary4;
				num -= (double)(dictionary5.get_Item(this.user_difficult) * 2);
			}
			return num;
		}

		private double getShipSakuParam(int ship_id, double slot_saku)
		{
			return Math.Sqrt((double)Mst_DataManager.Instance.Mst_ship.get_Item(ship_id).Saku) - 2.0 + slot_saku;
		}

		private int getMapSakuParam(double saku_total, MapBranchResult.enumScoutingKind scouting_kind)
		{
			return (int)saku_total;
		}

		public void setCommentData(MapBranchResult.enumProductionKind type, ref MapCommentKind comment, ref MapProductionKind production)
		{
			if (type == MapBranchResult.enumProductionKind.A)
			{
				comment = MapCommentKind.Enemy;
				production = MapProductionKind.None;
			}
			else if (type == MapBranchResult.enumProductionKind.B)
			{
				comment = MapCommentKind.Enemy;
				production = MapProductionKind.WaterPlane;
			}
			else if (type == MapBranchResult.enumProductionKind.C1)
			{
				comment = MapCommentKind.Atack;
				production = MapProductionKind.WaterPlane;
			}
			else if (type == MapBranchResult.enumProductionKind.C2)
			{
				comment = MapCommentKind.CoursePatrol;
				production = MapProductionKind.WaterPlane;
			}
		}

		private int getDrumCount(List<Mst_slotitem> slotitems)
		{
			return Enumerable.Count<Mst_slotitem>(slotitems, (Mst_slotitem x) => x.Id == 75);
		}
	}
}
