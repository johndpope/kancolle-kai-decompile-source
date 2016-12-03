using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
{
	public enum ShipTexType
	{
		BANNER = 1,
		BANNER_D,
		CARD,
		CARD_D,
		CHARA_HALF,
		CHARA_HALF_D,
		CHARA_SMALL,
		CHARA_SMALL_D,
		CHARA,
		CHARA_D,
		CHARA_FACE,
		CHARA_FACE_D,
		CHARA_NAME,
		BANNER_LONG,
		BANNER_LONG_D
	}

	public abstract class BaseResource
	{
		public abstract void ClearAll();
	}

	[Serializable]
	public class ShipTextureResources : ResourceManager.BaseResource
	{
		[Serializable]
		public class SingleShipDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class ShipsTextureDictionary : SerializableDictionary<int, ResourceManager.ShipTextureResources.SingleShipDictionary>
		{
		}

		[SerializeField]
		private ResourceManager.ShipTextureResources.ShipsTextureDictionary _dicShipsTexture;

		public ShipTextureResources()
		{
			this._dicShipsTexture = new ResourceManager.ShipTextureResources.ShipsTextureDictionary();
		}

		public Texture2D Load(int shipID, int texNum)
		{
			Texture2D texture2D = this.LoadResource(shipID, texNum);
			if (texture2D == null)
			{
				DebugUtils.Warning(string.Format("Texture Load Warning : Ship Texture is Not Found (ShipID:{0} - TexNum:{1})", shipID, texNum));
				return null;
			}
			return texture2D;
		}

		public override void ClearAll()
		{
			this._dicShipsTexture.Clear();
		}

		public void Clear(ShipModel_Battle[] shipsModel)
		{
			for (int i = 0; i < shipsModel.Length; i++)
			{
				if (this._dicShipsTexture.ContainsKey(shipsModel[i].MstId))
				{
					this._dicShipsTexture[shipsModel[i].MstId].Clear();
				}
			}
		}

		private Texture2D LoadResource(int shipID, int texNum)
		{
			return ResourceManager.LoadResourceOrAssetBundle(string.Format("Textures/Ships/{0}/{1}", shipID, texNum)) as Texture2D;
		}
	}

	[Serializable]
	public class SlotItemTextureResource : ResourceManager.BaseResource
	{
		[Serializable]
		public class SingleSlotItemDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class SlotItemsTextureDictionary : SerializableDictionary<int, ResourceManager.SlotItemTextureResource.SingleSlotItemDictionary>
		{
		}

		[SerializeField]
		private ResourceManager.SlotItemTextureResource.SlotItemsTextureDictionary _dicSlotItemTexture;

		public SlotItemTextureResource()
		{
			this._dicSlotItemTexture = new ResourceManager.SlotItemTextureResource.SlotItemsTextureDictionary();
		}

		public Texture2D Load(int slotItemID, int texNum)
		{
			Texture2D texture2D = this._loadResource(slotItemID, texNum);
			if (texture2D == null)
			{
				return null;
			}
			return texture2D;
		}

		public override void ClearAll()
		{
			this._dicSlotItemTexture.Clear();
		}

		private Texture2D _loadResource(int slotItemID, int texnum)
		{
			return Resources.Load(string.Format("Textures/SlotItems/{0}/{1}", slotItemID, texnum)) as Texture2D;
		}
	}

	[Serializable]
	public class ShipVoiceResources : ResourceManager.BaseResource
	{
		[Serializable]
		public class SingleShipDictionary : SerializableDictionary<int, AudioClip>
		{
		}

		[Serializable]
		public class ShipsVoiceDictionary : SerializableDictionary<int, ResourceManager.ShipVoiceResources.SingleShipDictionary>
		{
		}

		[SerializeField]
		private ResourceManager.ShipVoiceResources.ShipsVoiceDictionary _dicShipsVoice;

		public ShipVoiceResources()
		{
			this._dicShipsVoice = new ResourceManager.ShipVoiceResources.ShipsVoiceDictionary();
		}

		public AudioClip Load(int shipID, int voiceNum)
		{
			AudioClip audioClip = this.LoadResource(shipID, voiceNum);
			if (audioClip == null)
			{
				return null;
			}
			return audioClip;
		}

		public override void ClearAll()
		{
			this._dicShipsVoice.Clear();
		}

		private AudioClip LoadResource(int shipID, int voiceNum)
		{
			return Resources.Load(string.Format("Sounds/Voice/kc{0}/{1}", shipID, voiceNum)) as AudioClip;
		}
	}

	[Serializable]
	public class FurnitureResource : ResourceManager.BaseResource
	{
		[Serializable]
		public class SingleFurnitureDictionary : SerializableDictionary<int, Texture2D>
		{
		}

		[Serializable]
		public class FurnituresTextureDictionary : SerializableDictionary<FurnitureKinds, ResourceManager.FurnitureResource.SingleFurnitureDictionary>
		{
		}

		[SerializeField]
		private ResourceManager.FurnitureResource.FurnituresTextureDictionary _dicFurnitureTexture;

		public FurnitureResource()
		{
			this._dicFurnitureTexture = new ResourceManager.FurnitureResource.FurnituresTextureDictionary();
		}

		public Texture2D LoadInteriorStoreFurniture(FurnitureKinds iType, int furnitureNumber)
		{
			return Resources.Load(string.Format("Textures/InteriorStore/Furnitures/{0}/{1}", iType, furnitureNumber)) as Texture2D;
		}

		public Texture2D Load(FurnitureKinds iType, int furnitureNum)
		{
			if (this._dicFurnitureTexture.ContainsKey(iType))
			{
				if (this._dicFurnitureTexture[iType].ContainsKey(furnitureNum))
				{
					if (this._dicFurnitureTexture[iType][furnitureNum] != null)
					{
						return this._dicFurnitureTexture[iType][furnitureNum];
					}
					Texture2D texture2D = this.LoadResource(iType, furnitureNum);
					if (texture2D == null)
					{
						return null;
					}
					this._dicFurnitureTexture[iType][furnitureNum] = texture2D;
				}
				else
				{
					Texture2D texture2D = this.LoadResource(iType, furnitureNum);
					if (texture2D == null)
					{
						return null;
					}
					this._dicFurnitureTexture[iType].Add(furnitureNum, texture2D);
				}
			}
			else
			{
				Texture2D texture2D = this.LoadResource(iType, furnitureNum);
				if (texture2D == null)
				{
					return null;
				}
				ResourceManager.FurnitureResource.SingleFurnitureDictionary singleFurnitureDictionary = new ResourceManager.FurnitureResource.SingleFurnitureDictionary();
				singleFurnitureDictionary.Add(furnitureNum, texture2D);
				this._dicFurnitureTexture.Add(iType, singleFurnitureDictionary);
			}
			return this._dicFurnitureTexture[iType][furnitureNum];
		}

		public override void ClearAll()
		{
			this._dicFurnitureTexture.Clear();
		}

		private Texture2D LoadResource(FurnitureKinds iType, int furNum)
		{
			return Resources.Load(string.Format("Textures/Furnitures/{0}/{1}", iType, furNum)) as Texture2D;
		}
	}

	[Serializable]
	public class ShaderResources : ResourceManager.BaseResource
	{
		private List<string> SHADER_NAME;

		private List<Shader> _listShader;

		private bool _isInit;

		public List<Shader> shaderList
		{
			get
			{
				return this._listShader;
			}
		}

		public ShaderResources()
		{
			List<string> list = new List<string>();
			list.Add("CC2/Grayscale");
			list.Add("CC2/Transparent Colored");
			list.Add("Skybox/6 Sided");
			list.Add("FX/Water");
			list.Add("FX/Flare");
			list.Add("Particles/Additive");
			list.Add("Particles/Alpha Blended");
			list.Add("Sprites/Default");
			list.Add("Unlit/Transparent Packed");
			list.Add("Unlit/Transparent Colored");
			list.Add("KCV/Water");
			this.SHADER_NAME = list;
			base..ctor();
		}

		public void Load()
		{
			if (this._isInit)
			{
				return;
			}
			this._listShader = new List<Shader>();
			using (List<string>.Enumerator enumerator = this.SHADER_NAME.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					this._listShader.Add(Shader.Find(current));
				}
			}
			this._isInit = true;
		}

		public override void ClearAll()
		{
			this._isInit = false;
			this.SHADER_NAME.Clear();
			this.SHADER_NAME = null;
			this._listShader.Clear();
			this._listShader = null;
		}
	}

	private const string SHIP_TEXTURE_PATH = "Textures/Ships/{0}/{1}";

	private const string SHIP_VOICE_PATH = "Sounds/Voice/kc{0}/{1}";

	private const string SLOTITEM_TEXTURE_PATH = "Textures/SlotItems/{0}/{1}";

	private const string FURNITURE_TEXTURE_PATH = "Textures/Furnitures/{0}/{1}";

	private const string STRATEGY_MAP_STAGE_PATH = "Textures/Strategy/MapSelectGraph/stage{0}-{1}";

	private const string FURNITURE_STORE_TEXTURE_PATH = "Textures/InteriorStore/Furnitures/{0}/{1}";

	private const string SHIP_TYPE_ICON_TEXTURE = "Textures/Common/Ship/TypeIcon/{0}-{1}";

	private const string BGM_PATH = "Sounds/BGM/{0}";

	private const int SHIP_CTRL_MAX = 0;

	private const int SLOTITEM_CTRL_MAX = 0;

	public static readonly Dictionary<int, Vector2> SHIP_TEXTURE_SIZE;

	public static readonly Dictionary<int, Vector2> SLOTITEM_TEXTURE_SIZE;

	[SerializeField]
	private ResourceManager.ShipVoiceResources _clsShipVoice = new ResourceManager.ShipVoiceResources();

	[SerializeField]
	private ResourceManager.ShipTextureResources _clsShipTexture = new ResourceManager.ShipTextureResources();

	[SerializeField]
	private ResourceManager.SlotItemTextureResource _clsSlotItemTexture = new ResourceManager.SlotItemTextureResource();

	[SerializeField]
	private ResourceManager.FurnitureResource _clsFurniture = new ResourceManager.FurnitureResource();

	[SerializeField]
	private ResourceManager.ShaderResources _clsShader = new ResourceManager.ShaderResources();

	public ResourceManager.ShipVoiceResources ShipVoice
	{
		get
		{
			return this._clsShipVoice;
		}
	}

	public ResourceManager.ShipTextureResources ShipTexture
	{
		get
		{
			return this._clsShipTexture;
		}
	}

	public ResourceManager.SlotItemTextureResource SlotItemTexture
	{
		get
		{
			return this._clsSlotItemTexture;
		}
	}

	public ResourceManager.FurnitureResource Furniture
	{
		get
		{
			return this._clsFurniture;
		}
	}

	public ResourceManager.ShaderResources shader
	{
		get
		{
			return this._clsShader;
		}
	}

	static ResourceManager()
	{
		// Note: this type is marked as 'beforefieldinit'.
		Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
		dictionary.Add(1, new Vector2(360f, 90f));
		dictionary.Add(2, new Vector2(360f, 90f));
		dictionary.Add(3, new Vector2(218f, 300f));
		dictionary.Add(4, new Vector2(218f, 300f));
		ResourceManager.SHIP_TEXTURE_SIZE = dictionary;
		dictionary = new Dictionary<int, Vector2>();
		dictionary.Add(1, new Vector2(260f, 260f));
		dictionary.Add(2, new Vector2(287f, 430f));
		dictionary.Add(3, new Vector2(287f, 430f));
		dictionary.Add(4, new Vector2(287f, 430f));
		dictionary.Add(6, new Vector2(300f, 300f));
		dictionary.Add(7, new Vector2(300f, 300f));
		ResourceManager.SLOTITEM_TEXTURE_SIZE = dictionary;
	}

	public static ResourceRequest LoadStageCoverAsync(int areaId, int mapId)
	{
		return Resources.LoadAsync(string.Format("Textures/Strategy/MapSelectGraph/stage{0}-{1}", areaId, mapId), typeof(Texture2D));
	}

	public static Texture2D LoadStageCover(int areaId, int mapId)
	{
		return ResourceManager.LoadResourceOrAssetBundle(string.Format("Textures/Strategy/MapSelectGraph/stage{0}-{1}", areaId, mapId)) as Texture2D;
	}

	public static Texture LoadShipTypeIcon(ShipModelMst shipModel)
	{
		int num;
		switch (shipModel.Rare)
		{
		case 4:
		case 5:
			num = 2;
			goto IL_52;
		case 6:
			num = 3;
			goto IL_52;
		case 7:
		case 8:
			num = 4;
			goto IL_52;
		}
		num = 1;
		IL_52:
		int shipType = shipModel.ShipType;
		int num2;
		if (shipType != 8)
		{
			num2 = shipModel.ShipType;
		}
		else
		{
			num2 = 9;
		}
		if (num == -1)
		{
			return null;
		}
		if (shipModel == null)
		{
			return null;
		}
		return Resources.Load(string.Format("Textures/Common/Ship/TypeIcon/{0}-{1}", num2, num)) as Texture;
	}

	protected override void Awake()
	{
		base.Awake();
		this._clsShader.Load();
	}

	private void OnDestroy()
	{
		this.AllRelease();
	}

	public void AllRelease()
	{
		this._clsShipTexture.ClearAll();
		this._clsFurniture.ClearAll();
		this._clsSlotItemTexture.ClearAll();
		this._clsShipVoice.ClearAll();
		this._clsShader.ClearAll();
	}

	public static Object LoadResourceOrAssetBundle(string filePath)
	{
		bool flag = File.Exists(ABDataPath.AssetBundlePath + "/" + filePath + ".unity3d");
		if (flag)
		{
			AssetBundle assetBundle = AssetBundle.CreateFromFile(ABDataPath.AssetBundlePath + "/" + filePath + ".unity3d");
			string fileName = Path.GetFileName(filePath);
			Object @object = assetBundle.LoadAsset(fileName);
			if (@object == null)
			{
				@object = assetBundle.LoadAsset(fileName + ".bytes");
			}
			assetBundle.Unload(false);
			return @object;
		}
		return Resources.Load(filePath);
	}
}
