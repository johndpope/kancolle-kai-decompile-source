using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	public class Defines : IDisposable
	{
		public const string HEADER_MESSAGE_ADMIRAL_NAME = "提督名入力";

		public const string HEADER_MESSAGE_STARTER_SELECT = "ゲーム開始";

		public const string HEADER_MESSAGE_FIRST_SHIP = "初期艦選択";

		public const string HEADER_MESSAGE_TUTORIAL = "チュートリアル";

		public const float STARTER_PARTNER_SWIPE_RANGE = 0.15f;

		public const int PARTNERSHIP_SELECT_VOICE_NUM = 26;

		private static List<List<int>> _listStarterShipsID;

		private static List<List<Vector3>> _listStarterTextSize;

		public static List<List<int>> STARTER_PARTNER_SHIPS_ID
		{
			get
			{
				return Defines._listStarterShipsID;
			}
		}

		public static List<List<Vector3>> STARTER_PARTNER_TEXT_SIZE
		{
			get
			{
				return Defines._listStarterTextSize;
			}
		}

		public Defines()
		{
			List<List<int>> list = new List<List<int>>();
			List<List<int>> arg_2C_0 = list;
			List<int> list2 = new List<int>();
			list2.Add(54);
			list2.Add(55);
			list2.Add(56);
			arg_2C_0.Add(list2);
			List<List<int>> arg_78_0 = list;
			list2 = new List<int>();
			list2.Add(9);
			list2.Add(33);
			list2.Add(37);
			list2.Add(46);
			list2.Add(94);
			list2.Add(1);
			list2.Add(43);
			list2.Add(96);
			arg_78_0.Add(list2);
			Defines._listStarterShipsID = list;
			List<List<Vector3>> list3 = new List<List<Vector3>>();
			List<List<Vector3>> arg_DF_0 = list3;
			List<Vector3> list4 = new List<Vector3>();
			list4.Add(new Vector3(816f, 350f, 0f));
			list4.Add(new Vector3(760f, 370f, 0f));
			list4.Add(new Vector3(758f, 368f, 0f));
			arg_DF_0.Add(list4);
			List<List<Vector3>> arg_1BC_0 = list3;
			list4 = new List<Vector3>();
			list4.Add(new Vector3(770f, 348f, 0f));
			list4.Add(new Vector3(766f, 352f, 0f));
			list4.Add(new Vector3(804f, 368f, 0f));
			list4.Add(new Vector3(762f, 368f, 0f));
			list4.Add(new Vector3(764f, 368f, 0f));
			list4.Add(new Vector3(666f, 380f, 0f));
			list4.Add(new Vector3(712f, 382f, 0f));
			list4.Add(new Vector3(784f, 352f, 0f));
			arg_1BC_0.Add(list4);
			Defines._listStarterTextSize = list3;
		}

		public void Dispose()
		{
			Mem.DelListSafe<List<int>>(ref Defines._listStarterShipsID);
			Mem.DelListSafe<List<Vector3>>(ref Defines._listStarterTextSize);
		}
	}
}
