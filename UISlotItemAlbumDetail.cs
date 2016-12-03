using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIPanel)), SelectionBase]
public class UISlotItemAlbumDetail : MonoBehaviour
{
	[Serializable]
	private class Parameter
	{
		[SerializeField]
		private Transform mTransformParent;

		[SerializeField]
		private UILabel mLabel_Param;

		[SerializeField]
		private UITexture mTexture_Param;

		public Transform GetTransform()
		{
			return this.mTransformParent;
		}

		public UILabel GetLabel()
		{
			return this.mLabel_Param;
		}

		public UITexture GetTexture()
		{
			return this.mTexture_Param;
		}
	}

	private class SlotItemAlbumDetailTextureInfo
	{
		public enum GraphicType
		{
			First,
			Second,
			Third,
			Fourth
		}

		private UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType mGraphicType;

		private int mGraphicSlotItemMstId;

		private Texture mTexture2d_Cache;

		private int mWidth;

		private int mHeight;

		private Vector3 mVector3_Offset;

		private Vector3 mVector3_Scale;

		private bool mNeedPixelPerfect;

		public SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType graphicType, int graphicSlotItemId, Vector3 scale, Vector3 offset, int width, int height)
		{
			this.mGraphicSlotItemMstId = graphicSlotItemId;
			this.mGraphicType = graphicType;
			this.mWidth = width;
			this.mHeight = height;
			this.mVector3_Offset = offset;
			this.mVector3_Scale = scale;
			this.mNeedPixelPerfect = false;
		}

		public SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType graphicType, int graphicSlotItemId, Vector3 scale, Vector3 offset, bool needPixelPerfect)
		{
			this.mGraphicSlotItemMstId = graphicSlotItemId;
			this.mGraphicType = graphicType;
			this.mWidth = -1;
			this.mHeight = -1;
			this.mVector3_Offset = offset;
			this.mVector3_Scale = scale;
			this.mNeedPixelPerfect = needPixelPerfect;
		}

		public bool NeedPixelPerfect()
		{
			return this.mNeedPixelPerfect;
		}

		public Vector3 GetOffset()
		{
			return this.mVector3_Offset;
		}

		public int GetWidth()
		{
			return this.mWidth;
		}

		public int GetHeight()
		{
			return this.mHeight;
		}

		public Vector3 GetScale()
		{
			return this.mVector3_Scale;
		}

		public Texture RequestTexture()
		{
			if (this.mTexture2d_Cache == null)
			{
				this.mTexture2d_Cache = this.LoadTexture(this.mGraphicType, this.mGraphicSlotItemMstId);
			}
			return this.mTexture2d_Cache;
		}

		private Texture LoadTexture(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType graphicType, int graphicShipId)
		{
			switch (graphicType)
			{
			case UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.First:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mGraphicSlotItemMstId, 1);
			case UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Second:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mGraphicSlotItemMstId, 2);
			case UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Third:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mGraphicSlotItemMstId, 3);
			case UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Fourth:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this.mGraphicSlotItemMstId, 4);
			default:
				return null;
			}
		}

		public void ReleaseTexture()
		{
			if (this.mTexture2d_Cache != null && this.mGraphicType != UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.First)
			{
				Resources.UnloadAsset(this.mTexture2d_Cache);
				this.mTexture2d_Cache = null;
			}
		}

		public static UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo[] GenerateSlotItemGraphicsInfo(AlbumSlotModel albumSlotItemModel)
		{
			List<UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo> list = new List<UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo>();
			list.Add(new UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Second, albumSlotItemModel.MstId, Vector3.get_one(), Vector3.get_zero(), 287, 430));
			list.Add(new UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.First, albumSlotItemModel.MstId, Vector3.get_one(), Vector3.get_zero(), 260, 260));
			list.Add(new UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Third, albumSlotItemModel.MstId, Vector3.get_one(), Vector3.get_zero(), 287, 430));
			list.Add(new UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GraphicType.Fourth, albumSlotItemModel.MstId, Vector3.get_one(), Vector3.get_zero(), 287, 430));
			return list.ToArray();
		}

		private static Vector3 GenerateDefaultScaleBySlotId(int ShipId)
		{
			if (ShipId == 192)
			{
				return Vector3.get_one() * 0.36f;
			}
			if (ShipId == 193)
			{
				return Vector3.get_one() * 0.4f;
			}
			if (ShipId == 319)
			{
				return Vector3.get_one() * 0.5f;
			}
			if (ShipId != 416)
			{
				return new Vector3(0.5f, 0.5f);
			}
			return Vector3.get_one() * 0.5f;
		}

		private static Vector3 GenerateOffsetByShipId(int ShipId)
		{
			if (ShipId == 192)
			{
				return Vector3.get_up() * -10f;
			}
			if (ShipId == 193)
			{
				return Vector3.get_up() * -20f;
			}
			if (ShipId == 319)
			{
				return Vector3.get_up() * -10f;
			}
			if (ShipId != 416)
			{
				return Vector3.get_zero();
			}
			return Vector3.get_up() * -20f;
		}
	}

	private UIPanel mPanelThis;

	private UIButtonManager mButtonManager;

	[SerializeField]
	private Transform mTransform_OffsetForTexture;

	[SerializeField]
	private UITexture mTexture_SlotItem;

	[SerializeField]
	private UITexture mTexture_TypeIcon;

	[SerializeField]
	private UITexture mTexture_SlotItemTypeBackground;

	[SerializeField]
	protected UIButton mButton_Next;

	[SerializeField]
	protected UIButton mButton_Prev;

	[SerializeField]
	private UILabel mLabel_No;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UILabel mLabel_TypeName;

	[SerializeField]
	private UISlotItemAlbumDetail.Parameter[] mParameters;

	[SerializeField]
	private UITexture[] mTextures_EquipmentShipIcon;

	private UIButton[] mButtons_Focasable;

	private UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo[] mSlotItemAlbumDetailTextureInfos;

	private UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo mCurrentShipAlbumDetailTextureInfo;

	protected UIButton mCurrentFocusButton;

	protected KeyControl mKeyController;

	private AlbumSlotModel mAlbumSlotModel;

	public bool _Stc_R;

	private Action<Tween> mOnBackListener;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 0f;
		this.mButtonManager = base.GetComponent<UIButtonManager>();
		this.mButtonManager.IndexChangeAct = delegate
		{
			int num = Array.IndexOf<UIButton>(this.mButtons_Focasable, this.mButtonManager.nowForcusButton);
			if (-1 < num)
			{
				this.ChangeFocusButton(this.mButtonManager.nowForcusButton, false);
			}
		};
	}

	public void Initialize(AlbumSlotModel albumSlotModel)
	{
		this._Stc_R = false;
		this.mAlbumSlotModel = albumSlotModel;
		int maxLineInFullWidthChar = 22;
		int fullWidthCharBuffer = 1;
		if (this.mSlotItemAlbumDetailTextureInfos != null)
		{
			UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo[] array = this.mSlotItemAlbumDetailTextureInfos;
			for (int i = 0; i < array.Length; i++)
			{
				UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo = array[i];
				slotItemAlbumDetailTextureInfo.ReleaseTexture();
			}
		}
		this.mSlotItemAlbumDetailTextureInfos = UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo.GenerateSlotItemGraphicsInfo(albumSlotModel);
		if (this.mTexture_SlotItemTypeBackground.mainTexture != null)
		{
			Resources.UnloadAsset(this.mTexture_SlotItemTypeBackground.mainTexture);
			this.mTexture_SlotItemTypeBackground.mainTexture = null;
		}
		this.mTexture_SlotItemTypeBackground.mainTexture = this.RequestTextureShipTypeBackGround(this.mAlbumSlotModel.Type2);
		this.mLabel_TypeName.text = Utils.GetSlotitemType3Name(this.mAlbumSlotModel.Type3);
		this.mLabel_No.text = string.Format("{0:000}", this.mAlbumSlotModel.Id);
		this.mLabel_Name.text = this.mAlbumSlotModel.Name;
		this.InitializeEquipmentShipIcons(this.mAlbumSlotModel);
		this.InitializeParameters(this.mAlbumSlotModel);
		if (this.mTexture_TypeIcon.mainTexture != null)
		{
			Resources.UnloadAsset(this.mTexture_TypeIcon.mainTexture);
		}
		this.mTexture_TypeIcon.mainTexture = null;
		this.mTexture_TypeIcon.mainTexture = this.RequestTextureTypeIcon(this.mAlbumSlotModel.Type4);
		this.mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(maxLineInFullWidthChar, fullWidthCharBuffer, this.mAlbumSlotModel.Detail);
		this.mButtons_Focasable = this.GetFocasableButtons();
		this.ChangeFocusTexture(this.mSlotItemAlbumDetailTextureInfos[0]);
	}

	private void InitializeParameters(AlbumSlotModel albumSlotModel)
	{
		string[] array = new string[]
		{
			"無",
			"短",
			"中",
			"長",
			"超長"
		};
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		if (0 < albumSlotModel.Soukou)
		{
			dictionary.Add(2, albumSlotModel.Soukou.ToString());
		}
		if (0 < albumSlotModel.Hougeki)
		{
			dictionary.Add(7, albumSlotModel.Hougeki.ToString());
		}
		if (0 < albumSlotModel.Raigeki)
		{
			dictionary.Add(8, albumSlotModel.Raigeki.ToString());
		}
		if (0 < albumSlotModel.Bakugeki)
		{
			dictionary.Add(14, albumSlotModel.Bakugeki.ToString());
		}
		if (0 < albumSlotModel.Taikuu)
		{
			dictionary.Add(9, albumSlotModel.Taikuu.ToString());
		}
		if (0 < albumSlotModel.Taisen)
		{
			dictionary.Add(10, albumSlotModel.Taisen.ToString());
		}
		if (0 < albumSlotModel.HouMeityu)
		{
			dictionary.Add(13, albumSlotModel.HouMeityu.ToString());
		}
		if (0 < albumSlotModel.Kaihi)
		{
			dictionary.Add(3, albumSlotModel.Kaihi.ToString());
		}
		if (0 < albumSlotModel.Sakuteki)
		{
			dictionary.Add(11, albumSlotModel.Sakuteki.ToString());
		}
		if (0 < albumSlotModel.Syatei)
		{
			dictionary.Add(6, array[albumSlotModel.Syatei]);
		}
		int[] array2 = Enumerable.ToArray<int>(dictionary.get_Keys());
		for (int i = 0; i < this.mParameters.Length; i++)
		{
			UISlotItemAlbumDetail.Parameter parameter = this.mParameters[i];
			if (parameter.GetTexture().mainTexture != null)
			{
				Resources.UnloadAsset(parameter.GetTexture().mainTexture);
				parameter.GetTexture().mainTexture = null;
			}
		}
		for (int j = 0; j < this.mParameters.Length; j++)
		{
			UISlotItemAlbumDetail.Parameter parameter2 = this.mParameters[j];
			if (j < array2.Length)
			{
				int num = array2[j];
				parameter2.GetTransform().SetActive(true);
				parameter2.GetLabel().text = dictionary.get_Item(num);
				parameter2.GetTexture().mainTexture = (Resources.Load(string.Format("Textures/Album/status_icon/status_{0}", num)) as Texture);
			}
			else
			{
				parameter2.GetTransform().SetActive(false);
			}
		}
	}

	private void InitializeEquipmentShipIcons(AlbumSlotModel albumSlotModel)
	{
		List<string> list = new List<string>();
		if (albumSlotModel.CanEquip(SType.Destroyter))
		{
			list.Add("ship1");
		}
		if (albumSlotModel.CanEquip(SType.LightCruiser))
		{
			list.Add("ship2");
		}
		if (albumSlotModel.CanEquip(SType.HeavyCruiser))
		{
			list.Add("ship3");
		}
		if (albumSlotModel.CanEquip(SType.BattleShip))
		{
			list.Add("ship6");
		}
		if (albumSlotModel.CanEquip(SType.LightAircraftCarrier))
		{
			list.Add("ship8");
		}
		if (albumSlotModel.CanEquip(SType.AircraftCarrier))
		{
			list.Add("ship9");
		}
		if (albumSlotModel.CanEquip(SType.SeaplaneTender))
		{
			list.Add("ship12");
		}
		if (albumSlotModel.CanEquip(SType.AviationBattleShip))
		{
			list.Add("ship7");
		}
		for (int i = 0; i < this.mTextures_EquipmentShipIcon.Length; i++)
		{
			if (i < list.get_Count())
			{
				this.mTextures_EquipmentShipIcon[i].mainTexture = this.RequestTextureShipTypeIcon(list.get_Item(i));
			}
			else
			{
				this.mTextures_EquipmentShipIcon[i].mainTexture = null;
			}
		}
	}

	protected virtual UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(this.mButton_Prev);
		list.Add(this.mButton_Next);
		return list.ToArray();
	}

	private Texture RequestTextureShipTypeIcon(string resourceName)
	{
		return Resources.Load<Texture2D>(string.Format("Textures/Album/ship_type_icon/{0}", resourceName));
	}

	private Texture RequestTextureShipTypeBackGround(int shipTypeId)
	{
		int num = -1;
		switch (shipTypeId)
		{
		case 1:
			num = 1;
			break;
		case 2:
			num = 2;
			break;
		case 3:
			num = 3;
			break;
		case 4:
			num = 31;
			break;
		case 5:
			num = 4;
			break;
		case 6:
			num = 8;
			break;
		case 7:
			num = 5;
			break;
		case 8:
			num = 6;
			break;
		case 9:
			num = 9;
			break;
		case 10:
			num = 28;
			break;
		case 14:
			num = 10;
			break;
		case 15:
			num = 11;
			break;
		case 16:
			num = 12;
			break;
		case 17:
			num = 13;
			break;
		case 18:
			num = 14;
			break;
		case 19:
			num = 15;
			break;
		case 20:
			num = 16;
			break;
		case 21:
			num = 17;
			break;
		case 22:
			num = 18;
			break;
		case 23:
			num = 19;
			break;
		case 24:
			num = 20;
			break;
		case 25:
			num = 21;
			break;
		case 26:
			num = 22;
			break;
		case 27:
			num = 23;
			break;
		case 28:
			num = 24;
			break;
		case 29:
			num = 25;
			break;
		case 30:
			num = 26;
			break;
		case 31:
			num = 9;
			break;
		case 32:
			num = 27;
			break;
		case 33:
			num = 29;
			break;
		case 34:
			num = 30;
			break;
		case 35:
			num = 15;
			break;
		}
		return Resources.Load<Texture2D>(string.Format("Textures/Album/weapon_bg/weapon_{0}", num));
	}

	private Texture RequestTextureTypeIcon(int typeIcon)
	{
		return Resources.Load<Texture2D>(string.Format("Textures/Album/Emblem/icon_1_weapon{0}", typeIcon));
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchNextTexture()
	{
		this.ChangeFocusButton(this.mButton_Next, false);
		this.NextImage();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPrevTexture()
	{
		this.ChangeFocusButton(this.mButton_Prev, false);
		this.PrevImage();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnToucBack()
	{
		this.OnBack();
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UIButton>(this.mButtons_Focasable, this.mButtonManager.nowForcusButton);
				int num2 = num - 1;
				if (0 <= num2)
				{
					this.ChangeFocusButton(this.mButtons_Focasable[num2], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UIButton>(this.mButtons_Focasable, this.mCurrentFocusButton);
				int num4 = num3 + 1;
				if (num4 < this.mButtons_Focasable.Length)
				{
					this.ChangeFocusButton(this.mButtons_Focasable[num4], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.OnSelectCircleButton();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.OnBack();
			}
		}
	}

	private void OnDestroy()
	{
		this.mPanelThis = null;
		this.mButtonManager = null;
		this.mTransform_OffsetForTexture = null;
		this.mTexture_SlotItem = null;
		this.mTexture_TypeIcon = null;
		this.mTexture_SlotItemTypeBackground = null;
		this.mButton_Next = null;
		this.mButton_Prev = null;
		this.mLabel_No = null;
		this.mLabel_Name = null;
		this.mLabel_Description = null;
		this.mLabel_TypeName = null;
		this.mParameters = null;
		this.mTextures_EquipmentShipIcon = null;
		this.mButtons_Focasable = null;
		this.mSlotItemAlbumDetailTextureInfos = null;
		this.mCurrentShipAlbumDetailTextureInfo = null;
		this.mCurrentFocusButton = null;
		this.mKeyController = null;
		this.mAlbumSlotModel = null;
	}

	protected virtual void OnSelectCircleButton()
	{
		if (this.mButton_Prev.Equals(this.mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.PrevImage();
		}
		else if (this.mButton_Next.Equals(this.mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.NextImage();
		}
	}

	public void SetOnBackListener(Action<Tween> onBackListener)
	{
		this.mOnBackListener = onBackListener;
	}

	private void OnBack()
	{
		this._Stc_R = true;
		Tween tween = this.GenerateTweenClose();
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke(tween);
		}
	}

	public void StartState()
	{
		this.ChangeFocusButton(this.mButtons_Focasable[0], false);
		this.ChangeFocusTexture(this.mSlotItemAlbumDetailTextureInfos[0]);
	}

	protected void NextImage()
	{
		int num = Array.IndexOf<UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo>(this.mSlotItemAlbumDetailTextureInfos, this.mCurrentShipAlbumDetailTextureInfo);
		int num2 = num + 1;
		UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo;
		if (num2 < this.mSlotItemAlbumDetailTextureInfos.Length)
		{
			slotItemAlbumDetailTextureInfo = this.mSlotItemAlbumDetailTextureInfos[num2];
		}
		else
		{
			slotItemAlbumDetailTextureInfo = this.mSlotItemAlbumDetailTextureInfos[0];
		}
		this.ChangeFocusTexture(slotItemAlbumDetailTextureInfo);
	}

	protected void PrevImage()
	{
		int num = Array.IndexOf<UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo>(this.mSlotItemAlbumDetailTextureInfos, this.mCurrentShipAlbumDetailTextureInfo);
		int num2 = num - 1;
		UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo;
		if (0 <= num2)
		{
			slotItemAlbumDetailTextureInfo = this.mSlotItemAlbumDetailTextureInfos[num2];
		}
		else
		{
			slotItemAlbumDetailTextureInfo = this.mSlotItemAlbumDetailTextureInfos[this.mSlotItemAlbumDetailTextureInfos.Length - 1];
		}
		this.ChangeFocusTexture(slotItemAlbumDetailTextureInfo);
	}

	private void ChangeFocusTexture(UISlotItemAlbumDetail.SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo)
	{
		this.mCurrentShipAlbumDetailTextureInfo = slotItemAlbumDetailTextureInfo;
		this.mTexture_SlotItem.mainTexture = this.mCurrentShipAlbumDetailTextureInfo.RequestTexture();
		bool flag = this.mCurrentShipAlbumDetailTextureInfo.NeedPixelPerfect();
		if (flag)
		{
			this.mTexture_SlotItem.MakePixelPerfect();
		}
		else
		{
			int width = this.mCurrentShipAlbumDetailTextureInfo.GetWidth();
			int height = this.mCurrentShipAlbumDetailTextureInfo.GetHeight();
			this.mTexture_SlotItem.SetDimensions(width, height);
		}
		this.mTransform_OffsetForTexture.get_transform().set_localScale(this.mCurrentShipAlbumDetailTextureInfo.GetScale());
		this.mTransform_OffsetForTexture.get_transform().set_localPosition(this.mCurrentShipAlbumDetailTextureInfo.GetOffset());
	}

	private void ChangeFocusButton(UIButton targetButton, bool needSe)
	{
		if (this.mCurrentFocusButton != null)
		{
			if (!this.mCurrentFocusButton.Equals(targetButton) && needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this.mCurrentFocusButton.SetState(UIButtonColor.State.Normal, true);
		}
		this.mCurrentFocusButton = targetButton;
		if (this.mCurrentFocusButton != null)
		{
			this.mCurrentFocusButton.SetState(UIButtonColor.State.Hover, true);
		}
	}

	public void Show()
	{
		this._Stc_R = false;
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, true);
		}
		TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
		{
			this.mPanelThis.alpha = alpha;
		}), this);
	}

	private Tween GenerateTweenClose()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, true);
		}
		SingletonMonoBehaviour<SoundManager>.Instance.StopVoice();
		return TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
		{
			this.mPanelThis.alpha = alpha;
		}), this);
	}
}
