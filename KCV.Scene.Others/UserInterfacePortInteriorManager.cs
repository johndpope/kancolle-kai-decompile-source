using Common.Enum;
using KCV.Furniture;
using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Others
{
	public class UserInterfacePortInteriorManager : MonoBehaviour
	{
		private class UIPortInteriorFactory
		{
			private static string FURNITURES_PREFAB_PATH = "Prefabs/Furnitures";

			private static string STATIC_FURNITURE = "UIStaticFurniture";

			public static readonly int[] DYNAMIC_FURNITURE_WINDOW = new int[]
			{
				33,
				34,
				36,
				37
			};

			public static readonly int[] DYNAMIC_FURNITURE_WALL = new int[0];

			public static readonly int[] DYNAMIC_FURNITURE_FLOOR = new int[]
			{
				6,
				23
			};

			public static readonly int[] DYNAMIC_FURNITURE_HANGINGS = new int[]
			{
				29,
				35,
				42,
				45
			};

			public static readonly int[] DYNAMIC_FURNITURE_DESK = new int[]
			{
				42,
				44,
				52,
				54,
				55
			};

			public static readonly int[] DYNAMIC_FURNITURE_CHEST = new int[]
			{
				29,
				31,
				35,
				38
			};

			public static UIFurniture GenerateFurniturePrefab(FurnitureModel furnitureModel)
			{
				bool flag = UserInterfacePortInteriorManager.UIPortInteriorFactory.IsDynamicFurniture(furnitureModel);
				if (!flag)
				{
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
				}
				switch (furnitureModel.Type)
				{
				case FurnitureKinds.Floor:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureFloorPrefab(furnitureModel);
				case FurnitureKinds.Wall:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureWallPrefab(furnitureModel);
				case FurnitureKinds.Window:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureWindowPrefab(furnitureModel);
				case FurnitureKinds.Hangings:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureHangingsPrefab(furnitureModel);
				case FurnitureKinds.Chest:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureChestPrefab(furnitureModel);
				case FurnitureKinds.Desk:
					return UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurnitureDeskPrefab(furnitureModel);
				default:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
				}
			}

			private static UIFurniture GenerateFurnitureWindowPrefab(FurnitureModel windowFurnitureModel)
			{
				switch (windowFurnitureModel.NoInType + 1)
				{
				case 33:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_211);
				case 34:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_215);
				case 36:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_230);
				case 37:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_244);
				}
				return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_WINDOW_FURNITURE);
			}

			private static UIFurniture GenerateFurnitureWallPrefab(FurnitureModel wallFurnitureModel)
			{
				return null;
			}

			private static UIFurniture GenerateFurnitureFloorPrefab(FurnitureModel floorFurnitureModel)
			{
				int num = floorFurnitureModel.NoInType + 1;
				int num2 = num;
				if (num2 == 6)
				{
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_6);
				}
				if (num2 != 23)
				{
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
				}
				return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_23);
			}

			private static UIFurniture GenerateFurnitureHangingsPrefab(FurnitureModel hangingsFurnitureModel)
			{
				int num = hangingsFurnitureModel.NoInType + 1;
				int num2 = num;
				switch (num2)
				{
				case 42:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_235);
				case 43:
				case 44:
					IL_24:
					if (num2 != 35)
					{
						return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
					}
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_206);
				case 45:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_247);
				}
				goto IL_24;
			}

			private static UIFurniture GenerateFurnitureDeskPrefab(FurnitureModel deskFurnitureModel)
			{
				int num = deskFurnitureModel.NoInType + 1;
				int num2 = num;
				switch (num2)
				{
				case 52:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_14);
				case 53:
					IL_24:
					switch (num2)
					{
					case 42:
						return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_216);
					case 44:
						return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_218);
					}
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
				case 54:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_15);
				case 55:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_16);
				}
				goto IL_24;
			}

			private static UIFurniture GenerateFurnitureChestPrefab(FurnitureModel chestFurnitureModel)
			{
				int num = chestFurnitureModel.NoInType + 1;
				int num2 = num;
				switch (num2)
				{
				case 35:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_222);
				case 36:
				case 37:
					IL_24:
					switch (num2)
					{
					case 29:
						return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_17);
					case 31:
						return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_163);
					}
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.UIPortInteriorFactory.STATIC_FURNITURE);
				case 38:
					return Resources.Load<UIFurniture>(UserInterfacePortInteriorManager.UIPortInteriorFactory.FURNITURES_PREFAB_PATH + "/" + UserInterfacePortInteriorManager.DynamicFurniture.DYNAMIC_FURNITURE_239);
				}
				goto IL_24;
			}

			private static bool IsDynamicFurniture(FurnitureModel furnitureModel)
			{
				int num = furnitureModel.NoInType + 1;
				switch (furnitureModel.Type)
				{
				case FurnitureKinds.Floor:
					return Enumerable.Contains<int>(UserInterfacePortInteriorManager.UIPortInteriorFactory.DYNAMIC_FURNITURE_FLOOR, num);
				case FurnitureKinds.Wall:
					return Enumerable.Contains<int>(UserInterfacePortInteriorManager.UIPortInteriorFactory.DYNAMIC_FURNITURE_WALL, num);
				case FurnitureKinds.Window:
					return true;
				case FurnitureKinds.Hangings:
					return Enumerable.Contains<int>(UserInterfacePortInteriorManager.UIPortInteriorFactory.DYNAMIC_FURNITURE_HANGINGS, num);
				case FurnitureKinds.Chest:
					return Enumerable.Contains<int>(UserInterfacePortInteriorManager.UIPortInteriorFactory.DYNAMIC_FURNITURE_CHEST, num);
				case FurnitureKinds.Desk:
					return Enumerable.Contains<int>(UserInterfacePortInteriorManager.UIPortInteriorFactory.DYNAMIC_FURNITURE_DESK, num);
				default:
					return false;
				}
			}
		}

		private class DynamicFurniture
		{
			public static string DYNAMIC_FURNITURE_206 = "UIDynamicHangingsFurnitureBigClock";

			public static string DYNAMIC_FURNITURE_230 = "UIDynamicWindowFurnitureCounterBar";

			public static string DYNAMIC_FURNITURE_239 = "UIDynamicChestFurnitureBathhouse";

			public static string DYNAMIC_FURNITURE_244 = "UIDynamicWindowFurnitureTeruteru";

			public static string DYNAMIC_FURNITURE_163 = "UIDynamicChestFurnitureDaruma";

			public static string DYNAMIC_FURNITURE_247 = "UIDynamicHangingsFurniture3MillionPeopleMemorial";

			public static string DYNAMIC_FURNITURE_6 = "UIDynamicFloorFurnitureSandBeach";

			public static string DYNAMIC_FURNITURE_216 = "UIDynamicDeskFurnitureInflatablePool";

			public static string DYNAMIC_FURNITURE_235 = "UIDynamicHangingsFurnitureMusashiMemorial";

			public static string DYNAMIC_FURNITURE_23 = "UIDynamicFloorFurnitureSnowField";

			public static string DYNAMIC_FURNITURE_211 = "UIDynamicWindowFurnitureFuurin";

			public static string DYNAMIC_FURNITURE_215 = "UIDynamicWindowFurnitureHanabi";

			public static string DYNAMIC_FURNITURE_218 = "UIDynamicDeskFurnitureJukeBox";

			public static string DYNAMIC_FURNITURE_222 = "UIDynamicChestFurnitureJukeBoxKai";

			public static string DYNAMIC_FURNITURE_14 = "UIDynamicDeskFurnitureColdWaterBath";

			public static string DYNAMIC_FURNITURE_16 = "UIDynamicDeskFurnitureShootingGallery";

			public static string DYNAMIC_FURNITURE_15 = "UIDynamicDeskFurnitureYakisoba";

			public static string DYNAMIC_FURNITURE_17 = "UIDynamicChestFurnitureKagatan";

			public static string DYNAMIC_WINDOW_FURNITURE = "UIDynamicWindowFurniture";
		}

		[SerializeField]
		private Transform mFloor;

		[SerializeField]
		private Transform mWall;

		[SerializeField]
		private Transform mWindow;

		[SerializeField]
		private Transform mChest;

		[SerializeField]
		private Transform mDesk;

		[SerializeField]
		private Transform mHangings;

		private DeckModel mDeckModel;

		private Action mOnRequestJukeBoxEvent;

		private Dictionary<FurnitureKinds, FurnitureModel> mFurnituresSet;

		private void Awake()
		{
			this.mFloor.set_localPosition(new Vector3(0f, -272f));
			this.mWall.set_localPosition(new Vector3(0f, 272f));
			this.mWindow.set_localPosition(new Vector3(480f, 272f));
			this.mChest.set_localPosition(new Vector3(480f, 0f));
			this.mDesk.set_localPosition(new Vector3(-480f, 0f));
			this.mHangings.set_localPosition(new Vector3(-480f, 272f));
		}

		private void OnDestroy()
		{
			this.ClearFurnitures();
			Mem.Del<Transform>(ref this.mFloor);
			Mem.Del<Transform>(ref this.mWall);
			Mem.Del<Transform>(ref this.mWindow);
			Mem.Del<Transform>(ref this.mChest);
			Mem.Del<Transform>(ref this.mDesk);
			Mem.Del<Transform>(ref this.mHangings);
			Mem.Del<DeckModel>(ref this.mDeckModel);
			Mem.Del<Action>(ref this.mOnRequestJukeBoxEvent);
			Mem.DelDictionarySafe<FurnitureKinds, FurnitureModel>(ref this.mFurnituresSet);
		}

		public void InitializeFurnitures(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			this.mDeckModel = deckModel;
			this.mFurnituresSet = furnitureSet;
			using (Dictionary<FurnitureKinds, FurnitureModel>.Enumerator enumerator = furnitureSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FurnitureKinds, FurnitureModel> current = enumerator.get_Current();
					this.UpdateFurniture(deckModel, current.get_Key(), current.get_Value());
				}
			}
		}

		public void SetOnRequestJukeBoxEvent(Action onRequestJukeBoxEvent)
		{
			this.mOnRequestJukeBoxEvent = onRequestJukeBoxEvent;
		}

		private void OnRequestJukeBoxEvent()
		{
			if (this.mOnRequestJukeBoxEvent != null)
			{
				this.mOnRequestJukeBoxEvent.Invoke();
			}
		}

		public void InitializeFurnituresForConfirmation(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			this.mDeckModel = deckModel;
			this.mFurnituresSet = furnitureSet;
			using (Dictionary<FurnitureKinds, FurnitureModel>.Enumerator enumerator = furnitureSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FurnitureKinds, FurnitureModel> current = enumerator.get_Current();
					this.UpdateFurniture(deckModel, current.get_Key(), current.get_Value());
				}
			}
		}

		public void UpdateFurnitures(DeckModel deckModel, Dictionary<FurnitureKinds, FurnitureModel> furnitureSet)
		{
			this.mDeckModel = deckModel;
			using (Dictionary<FurnitureKinds, FurnitureModel>.Enumerator enumerator = furnitureSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FurnitureKinds, FurnitureModel> current = enumerator.get_Current();
					bool flag = this.IsNeedUpdateFurniture(current.get_Key(), current.get_Value());
					if (flag)
					{
						this.UpdateFurniture(deckModel, current.get_Key(), current.get_Value());
						this.mFurnituresSet.set_Item(current.get_Key(), current.get_Value());
					}
				}
			}
		}

		public void UpdateFurniture(DeckModel deckModel, FurnitureKinds furnitureKind, FurnitureModel changeToFurniture)
		{
			UIFurniture uIFurniture = UserInterfacePortInteriorManager.UIPortInteriorFactory.GenerateFurniturePrefab(changeToFurniture);
			UIFurniture.UIFurnitureModel uiFurnitureModel = new UIFurniture.UIFurnitureModel(changeToFurniture, deckModel);
			UIFurniture uIFurniture2 = null;
			switch (furnitureKind)
			{
			case FurnitureKinds.Floor:
				this.ClearFurniture(this.mFloor);
				uIFurniture2 = NGUITools.AddChild(this.mFloor.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Wall:
				this.ClearFurniture(this.mWall);
				uIFurniture2 = NGUITools.AddChild(this.mWall.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Window:
				this.ClearFurniture(this.mWindow);
				uIFurniture2 = NGUITools.AddChild(this.mWindow.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Hangings:
				this.ClearFurniture(this.mHangings);
				uIFurniture2 = NGUITools.AddChild(this.mHangings.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Chest:
				this.ClearFurniture(this.mChest);
				uIFurniture2 = NGUITools.AddChild(this.mChest.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			case FurnitureKinds.Desk:
				this.ClearFurniture(this.mDesk);
				uIFurniture2 = NGUITools.AddChild(this.mDesk.get_gameObject(), uIFurniture.get_gameObject()).GetComponent<UIFurniture>();
				uIFurniture2.Initialize(uiFurnitureModel);
				break;
			}
			bool flag = uIFurniture2.GetComponent<UIDynamicFurniture>() != null;
			if (flag)
			{
				uIFurniture2.GetComponent<UIDynamicFurniture>().SetOnActionEvent(new Action<UIDynamicFurniture>(this.OnFurnitureActionEvent));
			}
		}

		public void OnFurnitureActionEvent(UIFurniture uiFurniture)
		{
			bool flag = this.IsConfigureJukeBox(uiFurniture);
			if (flag)
			{
				this.OnRequestJukeBoxEvent();
			}
		}

		private bool IsConfigureJukeBox(UIFurniture furniture)
		{
			if (furniture == null)
			{
				return false;
			}
			bool flag = furniture is UIDynamicDeskFurnitureJukeBox;
			return flag | furniture is UIDynamicChestFurnitureJukeBoxKai;
		}

		private bool IsConfigureJukeBox(FurnitureModel furniture)
		{
			if (furniture == null)
			{
				return false;
			}
			bool flag = furniture.MstId == 218;
			return flag | furniture.MstId == 222;
		}

		private void ClearFurniture(Transform target)
		{
			if (target == null)
			{
				return;
			}
			using (IEnumerator enumerator = target.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					UITexture component = transform.GetComponent<UITexture>();
					if (component != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref component, false);
					}
					UISprite component2 = transform.GetComponent<UISprite>();
					if (component2 != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref component2);
					}
					Object.Destroy(transform.get_gameObject());
				}
			}
		}

		private void ClearFurnitures()
		{
			this.ClearFurniture(this.mWindow);
			this.ClearFurniture(this.mFloor);
			this.ClearFurniture(this.mWall);
			this.ClearFurniture(this.mDesk);
			this.ClearFurniture(this.mChest);
			this.ClearFurniture(this.mHangings);
		}

		private bool IsNeedUpdateFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			FurnitureModel furnitureModel2;
			return !this.mFurnituresSet.TryGetValue(furnitureKind, ref furnitureModel2);
		}

		public bool IsConfigureJukeBox()
		{
			using (Dictionary<FurnitureKinds, FurnitureModel>.ValueCollection.Enumerator enumerator = this.mFurnituresSet.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FurnitureModel current = enumerator.get_Current();
					if (this.IsConfigureJukeBox(current))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
