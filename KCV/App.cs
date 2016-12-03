using Common.Enum;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV
{
	public static class App
	{
		public const string MASTER_VERSION = "1.02";

		public const string MASTER_VERSION_STR = "Version 1.02";

		public const int RECEIVE_SHIP_VOICE_NUM = 1;

		public const int FLEET_SHIP_MAX_NUM = 6;

		public const int LEAVE_VOICE_NUM = 29;

		public const long LEAVE_VOICE_INTERVAL = 300000L;

		public const int TIMESIGNAL_VOICE_OFFS = 30;

		public const int TARGET_FRAMERATE = 60;

		public const float DEFAULT_TIMESCALE = 1f;

		public const int INPUT_ADMIRAL_NAME_MAX = 12;

		public const float TIMESCALE_DEFAULT_SPEED = 1f;

		public const float TIMESCALE_HIGH_SPEED = 8f;

		public const float FADE_TIME = 0.2f;

		public const float CROSSFADE_TIME = 0.4f;

		public const float DIALOG_DEFAUTLT_POPUP_TIME = 0.4f;

		public const string RETENTION_KEY_DIFFICULTY = "difficulty";

		public const string RETENTION_KEY_MAP_MANAGER = "mapManager";

		public const string RETENTION_KEY_SORTIE_MAP_MANAGER = "sortieMapManager";

		public const string RETENTION_KEY_REBELLION_MAP_MANAGER = "rebellionMapManager";

		public const string RETENTION_KEY_PRACTICE_MANAGER = "practiceManager";

		public const string RETENTION_KEY_FORMATION = "formation";

		public const string RETENTION_KEY_AREA_ID = "areaId";

		public const string RETENTION_KEY_DECK_ID = "deckID";

		public const string RETENTION_KEY_ROOT_TYPE = "rootType";

		public const string RETENTION_KEY_SHIP_RECOVERY_TYPE = "shipRecoveryType";

		public const string RETENSION_KEY_NEW_OPEN_AREA_IDS = "newOpenAreaIDs";

		public const string RETENSION_KEY_NEW_OPEN_MAP_IDS = "newOpenMapIDs";

		public const string RETENSION_KEY_INHERIT_DATA = "isInherit";

		public const string RETENSION_KEY_INHERIT_ADMIRAL_NAME = "InheritAdmiralName";

		public const string KeyGuide_Button_L = "提督コマンド";

		public const string KeyGuide_Button_R = "戻る";

		public static readonly Random rand = new Random((int)DateTime.get_Now().get_Ticks() & 65535);

		public static readonly string[] SYATEI_TEXT = new string[]
		{
			"無",
			"短",
			"中",
			"長",
			"超長"
		};

		public static readonly Vector3 SCREEN_RESOLUTION = new Vector3(960f, 544f, 0f);

		public static readonly string DATE_FORMAT = "MM/DD\nHH:mm";

		public static string DEBUG_ADMIRAL_NAME = "横須賀提督";

		public static int DEBUG_DEFAULT_FLAGSHIP_ID = 26;

		public static int DEBUG_TARGET_AREA_ID = 1;

		public static DifficultKind DEBUG_DIFFICULT = DifficultKind.OTU;

		private static bool _isAtKeastOneMstLoadThread = false;

		private static bool _isMasterInit = false;

		private static bool _isTrophyInit = false;

		public static KeyControl OnlyController;

		public static bool isFirstUpdate;

		private static TitleManager _clsTitleManager;

		public static bool isInvincible;

		public static bool isMasterInit
		{
			get
			{
				return App._isMasterInit;
			}
			private set
			{
				App._isMasterInit = value;
			}
		}

		public static bool isTrophyInit
		{
			get
			{
				return App._isTrophyInit;
			}
			private set
			{
				App._isTrophyInit = value;
			}
		}

		public static DateTime SystemDateTime
		{
			get
			{
				DateTime dateTime = default(DateTime);
				return DateTime.get_Now().AddHours(9.0);
			}
		}

		public static DayOfWeek DayOfWeek
		{
			get
			{
				return App.SystemDateTime.get_DayOfWeek();
			}
		}

		public static TitleManager GetTitleManager()
		{
			return App._clsTitleManager;
		}

		public static bool Initialize()
		{
			if (App._clsTitleManager == null)
			{
				App._clsTitleManager = new TitleManager();
			}
			App._clsTitleManager.CreateSaveData(App.DEBUG_ADMIRAL_NAME, App.DEBUG_DEFAULT_FLAGSHIP_ID, App.DEBUG_DIFFICULT);
			App._clsTitleManager = null;
			App.InitSystems();
			AppInitializeManager.IsInitialize = true;
			return true;
		}

		public static bool InitSystems()
		{
			XorRandom.Init(0u);
			App.TimeScale(1f);
			return true;
		}

		public static bool CreateSaveDataNInitialize(string admiralName, int partnerShipId, DifficultKind iType, bool isInherit)
		{
			if (App._clsTitleManager == null)
			{
				App._clsTitleManager = new TitleManager();
			}
			if (!isInherit)
			{
				new Api_req_Member().PurgeUserData();
				App._clsTitleManager.CreateSaveData(admiralName, partnerShipId, iType);
			}
			else
			{
				App._clsTitleManager.CreateSaveDataPlus(admiralName, partnerShipId, iType);
			}
			XorRandom.Init(0u);
			App.TimeScale(1f);
			AppInitializeManager.IsInitialize = true;
			App._clsTitleManager = null;
			return true;
		}

		private static void DebugInit()
		{
			Diagnostics.set_enableHUD(true);
			Debug_Mod debug_Mod = new Debug_Mod();
			List<int> list = new List<int>();
			Debug.Log("ADD SHIP");
			debug_Mod.Add_Ship(list);
			list.Add(330);
			list.Add(24);
			list.Add(175);
			list.Add(117);
			list.Add(75);
			list.Add(321);
			list.Add(182);
			for (int i = 100; i < 110; i++)
			{
				list.Add(i);
			}
			debug_Mod.Add_Ship(list);
			OrganizeManager organizeManager = new OrganizeManager(1);
			debug_Mod.Add_Deck(2);
			debug_Mod.Add_Deck(3);
			debug_Mod.Add_Deck(4);
			debug_Mod.Add_Deck(5);
			debug_Mod.Add_Deck(6);
			ManagerBase.initialize();
			organizeManager.ChangeOrganize(1, 2, 2);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int j = 0; j < 100; j++)
			{
				if ((1 <= j && j <= 3) || (10 <= j && j <= 12) || (49 <= j && j <= 59))
				{
					dictionary.set_Item(j, 1);
				}
				dictionary.set_Item(54, 0);
				dictionary.set_Item(59, 10);
			}
			debug_Mod.Add_UseItem(dictionary);
			debug_Mod.Add_Materials(enumMaterialCategory.Fuel, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Bull, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Steel, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Bauxite, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Repair_Kit, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Dev_Kit, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Revamp_Kit, 2000);
			debug_Mod.Add_Materials(enumMaterialCategory.Build_Kit, 2000);
			debug_Mod.Add_Coin(80000);
			List<int> list2 = new List<int>();
			list2.Add(1);
			list2.Add(1);
			list2.Add(1);
			list2.Add(1);
			list2.Add(1);
			list2.Add(1);
			for (int k = 0; k < 30; k++)
			{
				list2.Add(14);
			}
			for (int l = 1; l < 100; l++)
			{
				list2.Add(l);
			}
			for (int m = 0; m < 30; m++)
			{
				list2.Add(25);
			}
			for (int n = 0; n < 6; n++)
			{
				list2.Add(42);
			}
			for (int num = 1; num < 100; num++)
			{
				Debug_Mod.ChangeSlotLevel(list2.get_Item(num), 9);
			}
			debug_Mod.Add_SlotItem(list2);
			DebugUtils.SLog("DEBUG_MOD OK");
		}

		public static void InitLoadMasterDataManager()
		{
			if (!App._isAtKeastOneMstLoadThread)
			{
				App._isAtKeastOneMstLoadThread = true;
				Mst_DataManager.Instance.LoadStartMaster(delegate
				{
					Mst_DataManager.Instance.SetStartMasterData();
					App.isMasterInit = true;
				});
				SingletonMonoBehaviour<TrophyManager>.Instance.Initialize(delegate(bool x)
				{
					App._isTrophyInit = x;
				});
			}
		}

		public static void SetFramerate(int framerate)
		{
			Application.set_targetFrameRate(framerate);
		}

		public static void SetSoundSettings(SettingModel setting)
		{
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = (float)setting.VolumeBGM / 100f;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.SE = (float)setting.VolumeSE / 100f;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.Voice = (float)setting.VolumeVoice / 100f;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.Mute = false;
		}

		public static void TimeScale(float timeScale)
		{
			Time.set_timeScale(timeScale);
		}

		public static string CurrentSceneName()
		{
			return Application.get_loadedLevelName();
		}

		public static void SetTitleManager(TitleManager manager)
		{
			App._clsTitleManager = manager;
		}

		public static DayOfWeek KCVDayOfWeek(ManagerBase manager)
		{
			DateTime dateTime = new DateTime(manager.Datetime.get_Year(), manager.Datetime.get_Month(), manager.Datetime.get_Day());
			return dateTime.get_DayOfWeek();
		}

		public static string GetFormationText(BattleFormationKinds1 iKind)
		{
			switch (iKind)
			{
			case BattleFormationKinds1.TanJuu:
				return "単縦陣";
			case BattleFormationKinds1.FukuJuu:
				return "複縦陣";
			case BattleFormationKinds1.Rinkei:
				return "輪形陣";
			case BattleFormationKinds1.Teikei:
				return "梯形陣";
			case BattleFormationKinds1.TanOu:
				return "単横陣";
			default:
				return string.Empty;
			}
		}
	}
}
