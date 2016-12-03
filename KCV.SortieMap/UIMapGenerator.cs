using System;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UIMapGenerator : MonoBehaviour
	{
		[Serializable]
		private struct MapParam
		{
			[Range(1f, 17f)]
			public int mapAreaID;

			[Range(1f, 7f)]
			public int mapInfoNo;

			public UISortieMapCell uiMapCell;

			public UISortieMapRoot uiMapRoute;

			public UIWobblingIcon uiWobblingIcon;

			public int makeAirRecPointNum;
		}

		private const string MAP_NAME_FORMAT = "Map{0}{1}";

		private const string MAP_BG_PATH = "Textures/SortieMap/MapBG/{0}/{0}{1}";

		private const string MAP_SAIL_LINE_ATLAS_PATH = "Atlases/SortieMap/SailLine/{0}/{1}/SailLine_{0}{1}_Atlas";

		private const string MAP_SAIL_LINE_GROW_ATLAS_PATH = "Atlases/SortieMap/SailLine/{0}/{1}/SailLine_Grow_{0}{1}_Atlas";

		private UIMapManager _uiMapManager;

		[SerializeField]
		private UIMapGenerator.MapParam _strMapParam = default(UIMapGenerator.MapParam);

		[Button("CreateFoundationMap", "海域ベース生成", new object[]
		{

		}), SerializeField]
		private int _nCreateFoundationMap = 1;

		[Button("LoadMap", "海域読み込み", new object[]
		{

		}), SerializeField]
		private int _nLoadMap = 2;

		[Button("AddSortieMapRoot", "海域ルートスクリプト追加", new object[]
		{

		}), SerializeField]
		private int _nAddSortieMapRoot = 4;

		[Button("MakeAirRecPoints", "航空偵察ポイントオブジェクト生成", new object[]
		{

		}), SerializeField]
		private int _nMakeAirRecPoints = 4;

		[Button("MakeWobblingIcon", "ゆらゆらアイコンオブジェジェクト生成", new object[]
		{

		}), SerializeField]
		private int _nMakeWobblingIcon = 4;

		[Button("DestroyMap", "海域オブジェクト破棄", new object[]
		{

		}), SerializeField]
		private int _nDestroyMap = 3;
	}
}
