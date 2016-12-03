using Common.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class ISDefine
	{
		public static readonly float OBJECT_DEFAULT_MOVE_TIME = 1f;

		public static readonly Dictionary<FurnitureKinds, int> CATEGORY_AREA_BUTTON_INDEX;

		public static readonly Dictionary<FurnitureKinds, Vector3> FURNITURE_SIZE;

		public static readonly Dictionary<FurnitureKinds, float> FURNITURE_THUM_MAGNIFICATION;

		static ISDefine()
		{
			// Note: this type is marked as 'beforefieldinit'.
			Dictionary<FurnitureKinds, int> dictionary = new Dictionary<FurnitureKinds, int>();
			dictionary.Add(FurnitureKinds.Hangings, 0);
			dictionary.Add(FurnitureKinds.Desk, 1);
			dictionary.Add(FurnitureKinds.Window, 2);
			dictionary.Add(FurnitureKinds.Floor, 3);
			dictionary.Add(FurnitureKinds.Wall, 4);
			dictionary.Add(FurnitureKinds.Chest, 5);
			ISDefine.CATEGORY_AREA_BUTTON_INDEX = dictionary;
			Dictionary<FurnitureKinds, Vector3> dictionary2 = new Dictionary<FurnitureKinds, Vector3>();
			dictionary2.Add(FurnitureKinds.Window, new Vector3(684f, 400f, 0f));
			dictionary2.Add(FurnitureKinds.Chest, new Vector3(495f, 544f, 0f));
			dictionary2.Add(FurnitureKinds.Desk, new Vector3(630f, 544f, 0f));
			dictionary2.Add(FurnitureKinds.Floor, new Vector3(963f, 236f, 0f));
			dictionary2.Add(FurnitureKinds.Hangings, new Vector3(258f, 394f, 0f));
			dictionary2.Add(FurnitureKinds.Wall, new Vector3(960f, 438f, 0f));
			ISDefine.FURNITURE_SIZE = dictionary2;
			Dictionary<FurnitureKinds, float> dictionary3 = new Dictionary<FurnitureKinds, float>();
			dictionary3.Add(FurnitureKinds.Window, 0.6435f);
			dictionary3.Add(FurnitureKinds.Chest, 0.7315f);
			dictionary3.Add(FurnitureKinds.Floor, 1.45f);
			dictionary3.Add(FurnitureKinds.Wall, 0.645f);
			dictionary3.Add(FurnitureKinds.Hangings, 1f);
			dictionary3.Add(FurnitureKinds.Desk, 0.567f);
			ISDefine.FURNITURE_THUM_MAGNIFICATION = dictionary3;
		}
	}
}
