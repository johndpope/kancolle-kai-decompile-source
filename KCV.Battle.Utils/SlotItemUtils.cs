using local.models;
using local.models.battle;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class SlotItemUtils
	{
		private enum AircraftType
		{
			Type1 = -1,
			Type2,
			Type3,
			Type4,
			Type5,
			Type6
		}

		public static readonly List<int>[] ENEMY_AIRCRAFT_TYPE;

		public static List<AircraftOffsetInfo> AIRCRAFT_OFFSET_INFOS;

		private static Dictionary<SlotItemUtils.AircraftType, List<int>> _dicAircraftType;

		static SlotItemUtils()
		{
			List<int>[] expr_06 = new List<int>[5];
			int arg_30_1 = 0;
			List<int> list = new List<int>();
			list.Add(517);
			list.Add(520);
			list.Add(524);
			expr_06[arg_30_1] = list;
			int arg_7C_1 = 1;
			list = new List<int>();
			list.Add(518);
			list.Add(521);
			list.Add(525);
			list.Add(526);
			list.Add(544);
			list.Add(546);
			expr_06[arg_7C_1] = list;
			int arg_91_1 = 2;
			list = new List<int>();
			list.Add(547);
			expr_06[arg_91_1] = list;
			int arg_A6_1 = 3;
			list = new List<int>();
			list.Add(548);
			expr_06[arg_A6_1] = list;
			int arg_BB_1 = 4;
			list = new List<int>();
			list.Add(549);
			expr_06[arg_BB_1] = list;
			SlotItemUtils.ENEMY_AIRCRAFT_TYPE = expr_06;
			List<AircraftOffsetInfo> list2 = new List<AircraftOffsetInfo>();
			list2.Add(new AircraftOffsetInfo(19, false, 30f, new Vector3(0f, 13f, 0f)));
			list2.Add(new AircraftOffsetInfo(20, true, 33f, new Vector3(1f, 14f, 0f)));
			list2.Add(new AircraftOffsetInfo(21, false, 15f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(23, false, 31f, new Vector3(0f, 7f, 0f)));
			list2.Add(new AircraftOffsetInfo(24, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(55, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(57, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(60, false, 340f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(60, false, 90f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(69, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(79, false, 17f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(80, false, 316f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(81, false, 317f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(83, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(96, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(98, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(99, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(102, false, 24f, new Vector3(0f, 9f, 0f)));
			list2.Add(new AircraftOffsetInfo(109, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(110, true, 21f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(111, true, 345f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(112, true, 0f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(113, true, 350f, Vector3.get_zero()));
			list2.Add(new AircraftOffsetInfo(115, false, 9f, Vector3.get_zero()));
			SlotItemUtils.AIRCRAFT_OFFSET_INFOS = list2;
			SlotItemUtils._dicAircraftType = new Dictionary<SlotItemUtils.AircraftType, List<int>>();
			SlotItemUtils._dicAircraftType.Add(SlotItemUtils.AircraftType.Type2, SlotItemUtils.ENEMY_AIRCRAFT_TYPE[0]);
			SlotItemUtils._dicAircraftType.Add(SlotItemUtils.AircraftType.Type3, SlotItemUtils.ENEMY_AIRCRAFT_TYPE[1]);
			SlotItemUtils._dicAircraftType.Add(SlotItemUtils.AircraftType.Type4, SlotItemUtils.ENEMY_AIRCRAFT_TYPE[2]);
			SlotItemUtils._dicAircraftType.Add(SlotItemUtils.AircraftType.Type5, SlotItemUtils.ENEMY_AIRCRAFT_TYPE[3]);
			SlotItemUtils._dicAircraftType.Add(SlotItemUtils.AircraftType.Type6, SlotItemUtils.ENEMY_AIRCRAFT_TYPE[4]);
		}

		public static string GetEnemyAircraftType(SlotitemModel_Battle model)
		{
			for (int i = 0; i < SlotItemUtils._dicAircraftType.get_Count(); i++)
			{
				List<int> list = SlotItemUtils._dicAircraftType.get_Item((SlotItemUtils.AircraftType)i);
				if (list.IndexOf(model.MstId) != -1)
				{
					return ((SlotItemUtils.AircraftType)i).ToString();
				}
			}
			return SlotItemUtils.AircraftType.Type1.ToString();
		}

		public static string GetEnemyAircraftType(int mstID)
		{
			for (int i = 0; i < SlotItemUtils._dicAircraftType.get_Count(); i++)
			{
				List<int> list = SlotItemUtils._dicAircraftType.get_Item((SlotItemUtils.AircraftType)i);
				if (list.IndexOf(mstID) != -1)
				{
					return ((SlotItemUtils.AircraftType)i).ToString();
				}
			}
			return SlotItemUtils.AircraftType.Type1.ToString();
		}

		public static AircraftOffsetInfo GetAircraftOffsetInfo(int mstID)
		{
			return SlotItemUtils.AIRCRAFT_OFFSET_INFOS.Find((AircraftOffsetInfo order) => order.mstID == mstID);
		}

		public static AircraftOffsetInfo GetAircraftOffsetInfo(SlotitemModel_Battle model)
		{
			return SlotItemUtils.GetAircraftOffsetInfo(model.MstId);
		}

		public static SlotitemModel_Battle GetDetectionScoutingPlane(List<List<SlotitemModel_Battle>> models)
		{
			using (List<List<SlotitemModel_Battle>>.Enumerator enumerator = models.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<SlotitemModel_Battle> current = enumerator.get_Current();
					if (current != null)
					{
						using (List<SlotitemModel_Battle>.Enumerator enumerator2 = current.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SlotitemModel_Battle current2 = enumerator2.get_Current();
								if (current2 != null)
								{
									return current2;
								}
							}
						}
					}
				}
			}
			return null;
		}

		public static Texture2D LoadTexture(SlotitemModel_Battle model)
		{
			if (model == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(model.GetGraphicId(), 4);
		}

		public static Texture2D LoadTexture(PlaneModelBase model)
		{
			if (model == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(Utils.GetSlotitemGraphicId(model.MstId), 4);
		}

		public static UITexture LoadTexture(UITexture tex, SlotitemModel_Battle model)
		{
			tex.mainTexture = SlotItemUtils.LoadTexture(model);
			tex.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4);
			return tex;
		}

		public static UITexture LoadTexture(ref UITexture tex, SlotitemModel_Battle model)
		{
			tex.mainTexture = SlotItemUtils.LoadTexture(model);
			tex.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4);
			return tex;
		}

		public static Texture2D LoadUniDirTexture(SlotitemModel_Battle model)
		{
			return SlotItemUtils.LoadTexture(model.GetGraphicId(), 6);
		}

		public static Texture2D LoadUniDirGlowTexture(SlotitemModel_Battle model)
		{
			return SlotItemUtils.LoadTexture(model.GetGraphicId(), 7);
		}

		private static Texture2D LoadTexture(int nID, int nNum)
		{
			if (SingletonMonoBehaviour<ResourceManager>.Instance == null)
			{
				return null;
			}
			return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(nID, nNum);
		}
	}
}
