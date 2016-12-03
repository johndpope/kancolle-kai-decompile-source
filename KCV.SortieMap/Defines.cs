using System;
using UnityEngine;

namespace KCV.SortieMap
{
	public class Defines
	{
		public const int UNDERWAY_REPLENISHMENT_VOICE_NUM = 26;

		public const float SORTIEMAP_MAP_SIZE_MAGNIFICATION = 1.1f;

		public const string FRAME_MESSAGE_COMPASS = "どこに進む？";

		public const string FRAME_MESSAGE_OCCURS_UZUSIO = "艦隊の前方にうずしおが発生しました！";

		public const string FRAME_MESSAGE_GET_ITEM = "{0}x{1}\nを入手しました！";

		public const string FRAME_MESSAGE_LOST_ITEM = "{0}x{1}を\n落としてしまいました…。";

		public const string FRAME_MESSAGE_LOST_ITEM_RADAR = "{0}x{1}を\n落としてしまいました…。\n(電探が役立って、被害を抑えられた！)";

		public const string FRAME_MESSAGE_FORMATION_SELECT = "陣形を選択してください。";

		public const string FRAME_MESSAGE_IMAGINATION = "気のせいだった。";

		public const string FRAME_MESSAGE_NOT_SEEN_ENEMY_SHADOW = "敵影を見ず。";

		public const string FRAME_MESSAGE_ACTIVE_BRANCHING = "艦隊の針路を選択できます。\n提督、どちらの針路を選択しますか？";

		public const string FRAME_MESSAGE_AIR_RECONNAISSANCE_WATER_PLANE = "水上偵察機による\n航空偵察を実施します。";

		public const string FRAME_MESSAGE_AIR_RECONNAISSANCE_LARGE_PLANE = "大型飛行艇による\n航空偵察を実施します。";

		public const string FRAME_MESSAGE_AIR_RECONNAISSANCE_IMPOSSIBLE = "航空偵察予定地点に到着しましたが、\n稼働偵察機がないため、偵察を中止します。";

		public const string FRAME_MESSAGE_EXECUTE_UNDERWAY_REPLENISHMENT = "艦隊に洋上補給を行います。";

		public const string BALOON_MESSAGE_ACTIVE_BRANCHING = "艦隊針路\n選択可能!";

		public const string BALOON_MESSAGE_NOT_SCREEN_ENEMY_SHADOW = "敵影を見ず。";

		public static readonly Vector3 SORTIEMAP_MAP_BG_SIZE = new Vector3(768f, 435f, 0f);
	}
}
