using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public abstract class UIFurniture : MonoBehaviour
	{
		public class UIFurnitureModel
		{
			private FurnitureModel mFurnitureModel;

			private DeckModel mDeckModel;

			public UIFurnitureModel(FurnitureModel furnitureModel, DeckModel deckModel)
			{
				this.mFurnitureModel = furnitureModel;
				this.mDeckModel = deckModel;
			}

			public DeckModel GetDeck()
			{
				return this.mDeckModel;
			}

			public DateTime GetDateTime()
			{
				return DateTime.get_UtcNow().ToLocalTime();
			}

			public FurnitureModel GetFurnitureModel()
			{
				return this.mFurnitureModel;
			}
		}

		private const string FURNITURE_TEXTURE_PATH = "Textures/Furnitures/{0}/{1}";

		protected UIFurniture.UIFurnitureModel mFurnitureModel;

		private void Awake()
		{
			this.OnAwake();
		}

		private void Start()
		{
			this.OnStart();
		}

		private void OnDestroy()
		{
			this.mFurnitureModel = null;
			this.OnDestroyEvent();
		}

		private void Update()
		{
			this.OnUpdate();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnDestroyEvent()
		{
		}

		public void Initialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			this.mFurnitureModel = uiFurnitureModel;
			this.OnInitialize(uiFurnitureModel);
		}

		protected virtual void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
		}

		public static Texture LoadTexture(FurnitureModel furnitureModel)
		{
			string text = string.Concat(new object[]
			{
				"Textures/Furnitures/",
				UIFurniture.FurnitureTypeToString(furnitureModel.Type),
				"/",
				furnitureModel.NoInType + 1
			});
			return Resources.Load(text) as Texture;
		}

		private static string FurnitureTypeToString(FurnitureKinds furnitureType)
		{
			string result = string.Empty;
			switch (furnitureType)
			{
			case FurnitureKinds.Floor:
				result = "Floor";
				break;
			case FurnitureKinds.Wall:
				result = "Wall";
				break;
			case FurnitureKinds.Window:
				result = "Window";
				break;
			case FurnitureKinds.Hangings:
				result = "Hangings";
				break;
			case FurnitureKinds.Chest:
				result = "Chest";
				break;
			case FurnitureKinds.Desk:
				result = "Desk";
				break;
			}
			return result;
		}
	}
}
