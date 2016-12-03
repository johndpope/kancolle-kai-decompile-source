using Common.Enum;
using Common.Struct;
using KCV;
using KCV.PopupString;
using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class Util
{
	public static Color CursolColor = new Color32(100, 200, 255, 255);

	public static float CursolBarDurationTime = 0.8f;

	public static float ButtonZoomUp = 1.05f;

	public static float ButtonDurationTime = 0.5f;

	private static readonly string[] RareString = new string[]
	{
		"コモン",
		"レア",
		"ホロ",
		"Sホロ",
		"SSホロ",
		"SSホロ"
	};

	private static readonly string[] DifficultyString = new string[]
	{
		string.Empty,
		"丁",
		"丙",
		"乙",
		"甲",
		"史"
	};

	private static float prevTime;

	public static string BytesToStr(byte[] data)
	{
		StringBuilder stringBuilder = new StringBuilder(64);
		for (int i = 0; i < data.Length; i++)
		{
			stringBuilder.AppendFormat("{0;x2}", data[i]);
		}
		return stringBuilder.ToString();
	}

	public static float LoopValue(object value, float min, float max)
	{
		float num = Convert.ToSingle(value);
		if (num < min)
		{
			num = max - (min - num) + 1f;
		}
		if (num > max)
		{
			num = min + (num - max) - 1f;
		}
		return num;
	}

	public static float RangeValue(object value, float min, float max)
	{
		float num = Convert.ToSingle(value);
		if (num < min)
		{
			num = min;
		}
		if (num > max)
		{
			num = max;
		}
		return num;
	}

	public static void FindParentToChild(ref GameObject target, Transform parent, string objName)
	{
		if (target != null)
		{
			return;
		}
		target = parent.FindChild(objName).get_gameObject();
		if (target == null)
		{
			DebugUtils.NullReferenceException(objName + " not found. parent is " + parent.get_name());
		}
	}

	public static void FindParentToChild(ref GameObject target, GameObject parent, string objName)
	{
		if (target != null)
		{
			return;
		}
		target = parent.get_transform().FindChild(objName).get_gameObject();
		if (target == null)
		{
			DebugUtils.NullReferenceException(objName + " not found. parent is " + parent.get_name());
		}
	}

	public static void FindParentToChild<T>(ref T target, Transform parent, string objName) where T : Component
	{
		if (target != null)
		{
			return;
		}
		if (parent.FindChild(objName).get_gameObject().GetComponent<T>() != null)
		{
			target = parent.FindChild(objName).get_gameObject().GetComponent<T>();
		}
		else
		{
			target = parent.FindChild(objName).get_gameObject().AddComponent<T>();
		}
		if (target == null)
		{
			DebugUtils.NullReferenceException(objName + " not found/ paremt is " + parent.get_name());
		}
	}

	public static void FindParentToChild<T>(ref T target, GameObject parent, string objName) where T : Component
	{
		if (target != null)
		{
			return;
		}
		if (parent.get_transform().FindChild(objName).get_gameObject().GetComponent<T>() != null)
		{
			target = parent.get_transform().FindChild(objName).get_gameObject().GetComponent<T>();
		}
		else
		{
			target = parent.get_transform().FindChild(objName).get_gameObject().AddComponent<T>();
		}
		if (target == null)
		{
			DebugUtils.NullReferenceException(objName + " not found/ parent is " + parent.get_name());
		}
	}

	public static void FindParentToChild<T>(ref T target, Transform parent) where T : Component
	{
		if (target != null)
		{
			return;
		}
		if (parent.GetComponentInChildren<T>() != null)
		{
			target = parent.get_transform().GetComponentInChildren<T>();
		}
		if (target == null)
		{
			DebugUtils.NullReferenceException("Class had not been inclided in the child... parent is " + parent.get_name());
		}
	}

	public static GameObject InstantiateGameObject(GameObject prefab, Transform parent)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(prefab);
		Transform transform = gameObject.get_transform();
		if (parent != null)
		{
			transform.set_parent(parent);
		}
		transform.set_localPosition(Vector3.get_zero());
		transform.set_localRotation(Quaternion.get_identity());
		transform.set_localScale(Vector3.get_one());
		gameObject.set_name(gameObject.get_name().Substring(0, gameObject.get_name().get_Length() - 7));
		return gameObject;
	}

	public static GameObject FindObjectWithTag(Generics.Tag tag)
	{
		return GameObject.FindGameObjectWithTag(tag.ToString());
	}

	public static T FindObjectWithTag<T>(Generics.Tag tag) where T : Component
	{
		return GameObject.FindGameObjectWithTag(tag.ToString()).SafeGetComponent<T>();
	}

	public static GameObject[] FindGameObjectsWithTag(Generics.Tag tag)
	{
		return GameObject.FindGameObjectsWithTag(tag.ToString());
	}

	public static T[] FindGameObjectsWithTag<T>(Generics.Tag tag) where T : Component
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag.ToString());
		T[] array2 = new T[array.Length];
		int num = 0;
		GameObject[] array3 = array;
		for (int i = 0; i < array3.Length; i++)
		{
			GameObject gameObject = array3[i];
			array2[num] = gameObject.GetComponent<T>();
			num++;
		}
		return array2;
	}

	public static void DestroyAllChildren(GameObject parent)
	{
		GameObject[] children = parent.get_gameObject().GetChildren(true);
		GameObject[] array = children;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			Object.Destroy(gameObject);
		}
		parent.get_transform().DetachChildren();
	}

	public static void LayerCollectiveSetting(Transform trans, Generics.Layers layer)
	{
		trans.get_gameObject().set_layer(layer.IntLayer());
		GameObject[] children = trans.get_gameObject().GetChildren(true);
		for (int i = 0; i < children.Length; i++)
		{
			GameObject gameObject = children[i];
			gameObject.set_layer(layer.IntLayer());
		}
	}

	public static string RankNameJ(int Rank)
	{
		switch (Rank)
		{
		case 1:
			return "元帥";
		case 2:
			return "大将";
		case 3:
			return "中将";
		case 4:
			return "少将";
		case 5:
			return "大佐";
		case 6:
			return "中佐";
		case 7:
			return "新米中佐";
		case 8:
			return "少佐";
		case 9:
			return "中堅少佐";
		case 10:
			return "新米少佐";
		default:
			return string.Empty;
		}
	}

	public static Vector3 Poi2Vec(Point point)
	{
		return new Vector3((float)point.x, (float)point.y, 0f);
	}

	public static Color HpGaugeColor2(int max, int now)
	{
		if ((float)now <= (float)max * 0.25f)
		{
			return new Color(1f, 0f, 0f, 1f);
		}
		if ((float)now <= (float)max * 0.5f)
		{
			return new Color(1f, 0.6f, 0f, 1f);
		}
		if ((float)now <= (float)max * 0.75f)
		{
			return new Color(1f, 0.9f, 0f, 1f);
		}
		return new Color(0f, 0.9f, 0.2f, 1f);
	}

	public static Color HpLabelColor(int max, int now)
	{
		if ((float)now <= (float)max * 0.25f)
		{
			return new Color(0.8f, 0f, 0f, 1f);
		}
		if ((float)now <= (float)max * 0.5f)
		{
			return new Color(0.9f, 0.4f, 0f, 1f);
		}
		if ((float)now <= (float)max * 0.75f)
		{
			return new Color(0.8f, 0.7f, 0f, 1f);
		}
		return new Color(0f, 0.7f, 0f, 1f);
	}

	public static Vector3[] CalcNWayPosX(Vector3 orgPos, int n, int width)
	{
		Vector3[] array = new Vector3[n];
		Vector3 vector = new Vector3((float)(width / 2 * (n - 1)), orgPos.y, orgPos.z);
		for (int i = 0; i < n; i++)
		{
			array[i] = new Vector3(-(vector.x - (float)(width * i)), vector.y, vector.z);
		}
		return array;
	}

	public static Vector3[] CalcNWayPosX(Vector3 orgPos, int n, float width)
	{
		Vector3[] array = new Vector3[n];
		Vector3 vector = new Vector3(width / 2f * (float)(n - 1), orgPos.y, orgPos.z);
		for (int i = 0; i < n; i++)
		{
			array[i] = new Vector3(-(vector.x - width * (float)i), vector.y, vector.z);
		}
		return array;
	}

	public static Vector3[] CalcNWayPosZ(Vector3 orgPos, int n, float width)
	{
		Vector3[] array = new Vector3[n];
		Vector3 vector = new Vector3(orgPos.x, orgPos.y, width / 2f * (float)(n - 1));
		for (int i = 0; i < n; i++)
		{
			array[i] = new Vector3(vector.x, vector.y, -(vector.z - width * (float)i));
		}
		return array;
	}

	public static Vector3 CameraLocationByZoom(Vector3 CameraOBJ, Vector3 TargetObj, float ZoomRate)
	{
		return new Vector3(CameraOBJ.x * ZoomRate + TargetObj.x * (1f - ZoomRate), CameraOBJ.y * ZoomRate + TargetObj.y * (1f - ZoomRate), CameraOBJ.z * ZoomRate + TargetObj.z * (1f - ZoomRate));
	}

	public static Vector3 CameraLocationByZoom_easeOutExpo(Vector3 CameraOBJ, Vector3 TargetObj, float ZoomRate)
	{
		return Util.CameraLocationByZoom_easeOutExpo(CameraOBJ, TargetObj, ZoomRate, -10f);
	}

	public static Vector3 CameraLocationByZoom_easeOutExpo(Vector3 CameraOBJ, Vector3 TargetObj, float ZoomRate, float EaseParam)
	{
		ZoomRate = 1f - (float)Math.Pow(2.0, (double)(EaseParam * ZoomRate));
		return new Vector3(CameraOBJ.x * ZoomRate + TargetObj.x * (1f - ZoomRate), CameraOBJ.y * ZoomRate + TargetObj.y * (1f - ZoomRate), CameraOBJ.z * ZoomRate + TargetObj.z * (1f - ZoomRate));
	}

	public static void SetRootContentSize(UIRoot root, Vector3 screenSize)
	{
		root.scalingStyle = UIRoot.Scaling.Constrained;
		root.fitHeight = true;
		root.fitWidth = true;
		root.manualWidth = (int)screenSize.x;
		root.manualHeight = (int)screenSize.y;
	}

	public static void InitMessage(UIButtonMessage[] messages, GameObject target, string functionName, UIButtonMessage.Trigger trigger)
	{
		for (int i = 0; i < messages.Length; i++)
		{
			UIButtonMessage uIButtonMessage = messages[i];
			uIButtonMessage.target = target;
			uIButtonMessage.functionName = functionName;
			uIButtonMessage.trigger = trigger;
		}
	}

	public static void SelectButtonState<T>(Dictionary<T, UIButton> dictionary, Enum iEnum)
	{
		using (Dictionary<T, UIButton>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<T, UIButton> current = enumerator.get_Current();
				T key = current.get_Key();
				if (key.ToString() == iEnum.ToString())
				{
					current.get_Value().state = UIButtonColor.State.Hover;
				}
				else
				{
					current.get_Value().state = UIButtonColor.State.Normal;
				}
			}
		}
	}

	public static void ButtonColliderEnabled(bool isEnabled, params UIButton[] buttons)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			UIButton uIButton = buttons[i];
			uIButton.GetComponent<Collider2D>().set_enabled(isEnabled);
		}
	}

	public static void ButtonColliderEnabled(List<UIButton> buttonList, bool isEnabled)
	{
		using (List<UIButton>.Enumerator enumerator = buttonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIButton current = enumerator.get_Current();
				current.GetComponent<Collider2D>().set_enabled(isEnabled);
			}
		}
	}

	public static void ButtonColliderEnabled<T>(Dictionary<T, UIButton> dictionary, bool isEnabled)
	{
		using (Dictionary<T, UIButton>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<T, UIButton> current = enumerator.get_Current();
				current.get_Value().GetComponent<Collider2D>().set_enabled(isEnabled);
			}
		}
	}

	public static List<EventDelegate> CreateEventDelegateList(MonoBehaviour target, string methodName, object parameter)
	{
		List<EventDelegate> list = new List<EventDelegate>();
		EventDelegate eventDelegate = new EventDelegate();
		eventDelegate.target = target;
		eventDelegate.methodName = methodName;
		list.Add(eventDelegate);
		if (parameter != null)
		{
			EventDelegate.Parameter parameter2 = eventDelegate.parameters[0];
			parameter2.value = parameter;
		}
		return list;
	}

	public static EventDelegate CreateEventDelegate(MonoBehaviour target, string methodName, object parameter)
	{
		EventDelegate eventDelegate = new EventDelegate(target, methodName);
		if (parameter != null)
		{
			EventDelegate.Parameter parameter2 = eventDelegate.parameters[0];
			parameter2.value = parameter;
		}
		return eventDelegate;
	}

	public static GameObject Instantiate(Object original, GameObject parent = null, bool addParentPos = false, bool doTween = false)
	{
		if (original == null)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.set_name(original.get_name());
		if (parent != null)
		{
			gameObject.get_transform().SetParent(parent.get_transform(), false);
			if (addParentPos)
			{
				gameObject.get_transform().set_position((original as GameObject).get_transform().get_position());
			}
		}
		if (doTween)
		{
			UITweener[] components = gameObject.get_transform().GetComponents<UITweener>();
			UITweener[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				UITweener uITweener = array[i];
				uITweener.set_enabled(true);
			}
		}
		return gameObject;
	}

	public static GameObject InstantiatePrefab(string prefabName, GameObject parent = null, bool doTween = false)
	{
		string text = "Prefabs/" + prefabName;
		GameObject gameObject = Util.Instantiate(Resources.Load(text), parent, false, doTween);
		if (gameObject == null)
		{
			Debug.LogError("prefab not found");
		}
		return gameObject;
	}

	public static T Instantiate<T>(ref T instance, ref Transform prefab, Transform parent)
	{
		instance = Util.Instantiate(prefab.get_gameObject(), parent.get_gameObject(), false, false).GetComponent<T>();
		prefab = null;
		return instance;
	}

	public static string Indentision(string text)
	{
		string text2 = text.Replace("\n", "\n");
		text2 = text2.Replace("\\n", "\n");
		return text2.Replace("<br>", "\n");
	}

	public static string Indentision(ref string text)
	{
		text = text.Replace("\n", "\n");
		text = text.Replace("\\n", "\n");
		text = text.Replace("<br>", "\n");
		return text;
	}

	public static string getPath(string fileName)
	{
		string[] filesMostDeep = Util.GetFilesMostDeep(Application.get_dataPath() + "/Resources/", fileName);
		if (filesMostDeep.Length == 0)
		{
			Debug.LogError("file is Not Found. ( prefabName : " + fileName + " )");
			return null;
		}
		filesMostDeep[0] = filesMostDeep[0].Replace("\\", "/");
		if (filesMostDeep.Length > 1)
		{
			Debug.LogWarning(fileName + "に該当するファイルは複数存在します。");
			for (int i = 0; i < filesMostDeep.Length; i++)
			{
				Debug.Log(filesMostDeep[i]);
			}
		}
		return filesMostDeep[0];
	}

	public static string[] GetFilesMostDeep(string stRootPath, string stPattern)
	{
		StringCollection stringCollection = new StringCollection();
		string[] files = Directory.GetFiles(stRootPath, stPattern);
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i];
			stringCollection.Add(text);
		}
		string[] directories = Directory.GetDirectories(stRootPath);
		for (int j = 0; j < directories.Length; j++)
		{
			string stRootPath2 = directories[j];
			string[] filesMostDeep = Util.GetFilesMostDeep(stRootPath2, stPattern);
			if (filesMostDeep != null)
			{
				stringCollection.AddRange(filesMostDeep);
			}
		}
		string[] array = new string[stringCollection.get_Count()];
		stringCollection.CopyTo(array, 0);
		return array;
	}

	public static string RareToString(int RareInt)
	{
		return Util.RareString[RareInt];
	}

	public static string getDifficultyString(DifficultKind dif)
	{
		return Util.DifficultyString[(int)dif];
	}

	public static string getCancelReason(IsGoCondition reasonEnum)
	{
		string result = string.Empty;
		switch (reasonEnum)
		{
		case IsGoCondition.AnotherArea:
			result = "艦隊は他の海域に居ます";
			break;
		case IsGoCondition.ActionEndDeck:
			result = "行動が終了している艦隊です";
			break;
		case IsGoCondition.Mission:
			result = "艦隊は遠征中です";
			break;
		case IsGoCondition.Deck1:
			result = "第一艦隊では実行できません";
			break;
		case IsGoCondition.InvalidDeck:
			result = "艦隊が編成されていません";
			break;
		case IsGoCondition.HasBling:
			result = "回航艦を含んでいます";
			break;
		case IsGoCondition.HasRepair:
			result = "艦隊に入渠中の艦がいます";
			break;
		case IsGoCondition.FlagShipTaiha:
			result = "旗艦が大破しています";
			break;
		case IsGoCondition.ReqFullSupply:
			result = "燃料・弾薬が最大の必要があります";
			break;
		case IsGoCondition.NeedSupply:
			result = "補給が必要な艦がいます";
			break;
		case IsGoCondition.ConditionRed:
			result = "疲労度の高い艦がいます";
			break;
		case IsGoCondition.Tanker:
			result = "輸送船が不足しています";
			break;
		case IsGoCondition.NecessaryStype:
			result = "特定の艦種が必要です";
			break;
		case IsGoCondition.InvalidOrganization:
			result = "今の編成ではこのマップに出撃できません";
			break;
		}
		return result;
	}

	public static string getPopupMessage(PopupMess reasonEnum)
	{
		string result = string.Empty;
		switch (reasonEnum)
		{
		case PopupMess.ActionEndShip:
			result = "行動が終了している艦です";
			break;
		case PopupMess.AlreadyRepair:
			result = "既に入渠している艦です";
			break;
		case PopupMess.NowRepairing:
			result = "入渠中の艦です";
			break;
		case PopupMess.NowBlinging:
			result = "回航中の艦です";
			break;
		case PopupMess.InEscortShip:
			result = "海上護衛艦です";
			break;
		case PopupMess.InMissionShip:
			result = "遠征中の艦です";
			break;
		case PopupMess.NotEnoughMaterial:
			result = "必要な資材が不足しています";
			break;
		case PopupMess.NoDamage:
			result = "入渠の必要はありません";
			break;
		case PopupMess.NoSlot:
			result = "装備スロットがありません";
			break;
		case PopupMess.NoDockKey:
			result = "ドック開放キーがありません";
			break;
		case PopupMess.MaterialUpperLimit:
			result = "上限を超えている資材があります";
			break;
		case PopupMess.CannotEquipArea:
			result = "この海域には配備出来ません";
			break;
		case PopupMess.CannotArsenalByLimitShip:
			result = "艦が保有上限に達し建造できません";
			break;
		case PopupMess.CannotArsenalByLimitItem:
			result = "装備数が保有上限に達し開発できません";
			break;
		case PopupMess.CannotGetArsenalByLimitShip:
			result = "艦の保有上限に達しています";
			break;
		case PopupMess.CannotGetArsenalByLimitItem:
			result = "装備の保有上限に達しています";
			break;
		case PopupMess.CannotArsenalByFullDeck:
			result = "ドックが満杯で建造できません";
			break;
		case PopupMess.NotEnoughHighSpeedRepairKit:
			result = "高速修復材が不足しています";
			break;
		case PopupMess.NotEnoughHighSpeedArsenalKit:
			result = "高速建造材が不足しています";
			break;
		}
		return result;
	}

	public static void CheckTime(string s = null)
	{
		if (s == null)
		{
			Util.prevTime = Time.get_realtimeSinceStartup();
			return;
		}
		Debug.Log(s + " : " + (Time.get_realtimeSinceStartup() - Util.prevTime).ToString());
	}

	public static int FixRangeValue(int value, int min, int max, int FixType)
	{
		if (value < min)
		{
			if (FixType == 0)
			{
				value = min;
			}
			else if (FixType == 1)
			{
				value = max;
			}
		}
		else if (value > max)
		{
			if (FixType == 0)
			{
				value = max;
			}
			else if (FixType == 1)
			{
				value = min;
			}
		}
		return value;
	}

	public static DayOfWeek DayOfWeek_System()
	{
		return App.SystemDateTime.get_DayOfWeek();
	}

	public static DayOfWeek DayOfWeek_Game(ManagerBase _ManagerBase)
	{
		DateTime dateTime = new DateTime(_ManagerBase.Datetime.get_Year(), _ManagerBase.Datetime.get_Month(), _ManagerBase.Datetime.get_Day());
		return dateTime.get_DayOfWeek();
	}

	[DebuggerHidden]
	public static IEnumerator WaitEndOfFrames(int FrameNum)
	{
		Util.<WaitEndOfFrames>c__Iterator53 <WaitEndOfFrames>c__Iterator = new Util.<WaitEndOfFrames>c__Iterator53();
		<WaitEndOfFrames>c__Iterator.FrameNum = FrameNum;
		<WaitEndOfFrames>c__Iterator.<$>FrameNum = FrameNum;
		return <WaitEndOfFrames>c__Iterator;
	}

	public static void MoveTo(GameObject go, float time, Vector3 localpos, iTween.EaseType ease)
	{
		iTween.MoveTo(go, iTween.Hash(new object[]
		{
			"time",
			time,
			"islocal",
			true,
			"position",
			localpos,
			"easetype",
			ease
		}));
	}
}
