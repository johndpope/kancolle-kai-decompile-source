using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(UIButtonManager)), SelectionBase]
public class UIShipAlbumDetail : MonoBehaviour
{
	public class ShipAlbumDetailTextureInfo
	{
		public enum GraphicType
		{
			Card,
			ShipNormal,
			ShipLive2d,
			ShipTaiha
		}

		private UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType mGraphicType;

		private int mGraphicShipId;

		private Texture mTexture2d_Cache;

		private int mWidth;

		private int mHeight;

		private Vector3 mVector3_Offset;

		private Vector3 mVector3_Scale;

		private bool mNeedPixelPerfect;

		public ShipAlbumDetailTextureInfo(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType graphicType, int graphicShipId, Vector3 scale, Vector3 offset, int width, int height)
		{
			this.mGraphicShipId = graphicShipId;
			this.mGraphicType = graphicType;
			this.mWidth = width;
			this.mHeight = height;
			this.mVector3_Offset = offset;
			this.mVector3_Scale = scale;
			this.mNeedPixelPerfect = false;
		}

		public ShipAlbumDetailTextureInfo(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType graphicType, int graphicShipId, Vector3 scale, Vector3 offset, bool needPixelPerfect)
		{
			this.mGraphicShipId = graphicShipId;
			this.mGraphicType = graphicType;
			this.mWidth = -1;
			this.mHeight = -1;
			this.mVector3_Offset = offset;
			this.mVector3_Scale = scale;
			this.mNeedPixelPerfect = needPixelPerfect;
		}

		public int GetGraphicShipId()
		{
			return this.mGraphicShipId;
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
				this.mTexture2d_Cache = this.LoadTexture(this.mGraphicType, this.mGraphicShipId);
			}
			return this.mTexture2d_Cache;
		}

		private Texture LoadTexture(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType graphicType, int graphicShipId)
		{
			switch (graphicType)
			{
			case UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.Card:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mGraphicShipId, 3);
			case UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.ShipNormal:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mGraphicShipId, 9);
			case UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.ShipTaiha:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mGraphicShipId, 10);
			}
			return null;
		}

		public void ReleaseTexture()
		{
			if (this.mTexture2d_Cache != null && this.mGraphicType != UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.Card)
			{
				Resources.UnloadAsset(this.mTexture2d_Cache);
				this.mTexture2d_Cache = null;
			}
		}

		public static UIShipAlbumDetail.ShipAlbumDetailTextureInfo[] GenerateShipGraphicsInfo(AlbumShipModel albumShipModel)
		{
			List<UIShipAlbumDetail.ShipAlbumDetailTextureInfo> list = new List<UIShipAlbumDetail.ShipAlbumDetailTextureInfo>();
			int[] mstIDs = albumShipModel.MstIDs;
			for (int i = 0; i < mstIDs.Length; i++)
			{
				int num = mstIDs[i];
				list.Add(new UIShipAlbumDetail.ShipAlbumDetailTextureInfo(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.ShipNormal, num, UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GenerateDefaultScaleByShipId(num), UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GenerateOffsetByShipId(num), true));
				list.Add(new UIShipAlbumDetail.ShipAlbumDetailTextureInfo(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.Card, num, Vector3.get_one(), Vector3.get_zero(), 218, 300));
				bool flag = albumShipModel.IsDamaged(num);
				if (flag)
				{
					list.Add(new UIShipAlbumDetail.ShipAlbumDetailTextureInfo(UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GraphicType.ShipTaiha, num, UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GenerateDefaultScaleByShipId(num), UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GenerateOffsetByShipId(num), true));
				}
			}
			return list.ToArray();
		}

		private static Vector3 GenerateDefaultScaleByShipId(int ShipId)
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
	private UITexture mTexture_Ship;

	[SerializeField]
	private UITexture mTexture_ShipTypeBackground;

	[SerializeField]
	private UISprite mSprite_ShipTypeIcon;

	[SerializeField]
	protected UIButton mButton_Next;

	[SerializeField]
	protected UIButton mButton_Prev;

	[SerializeField]
	protected UIButton mButton_Voice;

	[SerializeField]
	private UILabel mLabel_No;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UILabel mLabel_karyoku;

	[SerializeField]
	private UILabel mLabel_Taikyu;

	[SerializeField]
	private UILabel mLabel_Raisou;

	[SerializeField]
	private UILabel mLabel_Kaihi;

	[SerializeField]
	private UILabel mLabel_Taiku;

	[SerializeField]
	private UILabel mLabel_ShipTypeText;

	[SerializeField]
	private UIPentagonChart mPentagonChart;

	private UIButton[] mButtons_Focasable;

	private UIShipAlbumDetail.ShipAlbumDetailTextureInfo[] mShipAlbumDetailTextureInfos;

	private UIShipAlbumDetail.ShipAlbumDetailTextureInfo mCurrentShipAlbumDetailTextureInfo;

	protected UIButton mCurrentFocusButton;

	protected KeyControl mKeyController;

	private AlbumShipModel mAlbumShipModel;

	private AudioSource mVoiceAudioSource;

	public bool _Stc_R;

	private Action<Tween> mOnBackListener;

	private IEnumerator mChangeFocusTextureCoroutine;

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

	public void Initialize(AlbumShipModel albumShipModel)
	{
		this._Stc_R = false;
		this.mAlbumShipModel = albumShipModel;
		int maxLineInFullWidthChar = 22;
		int fullWidthCharBuffer = 1;
		if (this.mShipAlbumDetailTextureInfos != null)
		{
			UIShipAlbumDetail.ShipAlbumDetailTextureInfo[] array = this.mShipAlbumDetailTextureInfos;
			for (int i = 0; i < array.Length; i++)
			{
				UIShipAlbumDetail.ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo = array[i];
				shipAlbumDetailTextureInfo.ReleaseTexture();
			}
		}
		if (this.mTexture_ShipTypeBackground.mainTexture != null)
		{
			Resources.UnloadAsset(this.mTexture_ShipTypeBackground.mainTexture);
			this.mTexture_ShipTypeBackground.mainTexture = null;
		}
		this.mLabel_No.text = string.Format("{0:000}", this.mAlbumShipModel.Id);
		this.mLabel_Name.text = this.mAlbumShipModel.Name;
		this.mLabel_Taikyu.text = this.mAlbumShipModel.Taikyu.ToString();
		this.mLabel_Taiku.text = this.mAlbumShipModel.Taiku.ToString();
		this.mLabel_Raisou.text = this.mAlbumShipModel.Raisou.ToString();
		this.mLabel_karyoku.text = this.mAlbumShipModel.Karyoku.ToString();
		this.mLabel_Kaihi.text = this.mAlbumShipModel.Kaihi.ToString();
		this.mPentagonChart.Initialize(this.mAlbumShipModel.Karyoku, this.mAlbumShipModel.Raisou, this.mAlbumShipModel.Taiku, this.mAlbumShipModel.Kaihi, this.mAlbumShipModel.Taikyu);
		this.mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(maxLineInFullWidthChar, fullWidthCharBuffer, this.mAlbumShipModel.Detail);
		this.mShipAlbumDetailTextureInfos = UIShipAlbumDetail.ShipAlbumDetailTextureInfo.GenerateShipGraphicsInfo(this.mAlbumShipModel);
		this.mTexture_ShipTypeBackground.mainTexture = this.RequestShipTypeBackGround(this.mAlbumShipModel.ShipType);
		this.mSprite_ShipTypeIcon.spriteName = string.Format("ship{0}", albumShipModel.ShipType);
		this.mLabel_ShipTypeText.text = string.Format("{0} {1}番艦", albumShipModel.ClassTypeName, albumShipModel.ClassNum);
		this.mButtons_Focasable = this.GetFocasableButtons();
		this.ChangeFocusTexture(this.mShipAlbumDetailTextureInfos[0]);
	}

	public UIShipAlbumDetail.ShipAlbumDetailTextureInfo FocusTextureInfo()
	{
		return this.mCurrentShipAlbumDetailTextureInfo;
	}

	protected virtual UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(this.mButton_Prev);
		list.Add(this.mButton_Voice);
		list.Add(this.mButton_Next);
		return list.ToArray();
	}

	private Texture RequestShipTypeBackGround(int shipTypeId)
	{
		int num = -1;
		switch (shipTypeId)
		{
		case 2:
			num = 1;
			break;
		case 3:
			num = 2;
			break;
		case 4:
			num = 3;
			break;
		case 5:
			num = 4;
			break;
		case 6:
			num = 5;
			break;
		case 7:
			num = 8;
			break;
		case 8:
		case 9:
			num = 6;
			break;
		case 10:
			num = 7;
			break;
		case 11:
		case 18:
			num = 9;
			break;
		case 13:
			num = 10;
			break;
		case 14:
			num = 11;
			break;
		case 16:
			num = 12;
			break;
		case 17:
			num = 16;
			break;
		case 19:
			num = 15;
			break;
		case 20:
			num = 13;
			break;
		case 21:
			num = 19;
			break;
		}
		return Resources.Load<Texture2D>(string.Format("Textures/Album/ship_bg/ship_{0}", num));
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
	public void OnTouchVoice()
	{
		this.ChangeFocusButton(this.mButton_Voice, false);
		this.PlayVoice();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnToucBack()
	{
		this.OnBack();
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
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
			else if (this.mKeyController.IsRDown())
			{
				if (this.mVoiceAudioSource != null)
				{
					this.mVoiceAudioSource.Stop();
				}
				this.mVoiceAudioSource = null;
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
		}
	}

	protected virtual void OnSelectCircleButton()
	{
		if (this.mButton_Prev.Equals(this.mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.PrevImage();
		}
		else if (this.mButton_Voice.Equals(this.mCurrentFocusButton))
		{
			this.PlayVoice();
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
		this.mPentagonChart.PlayHide();
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke(tween);
		}
	}

	public void StartState()
	{
		this.ChangeFocusButton(this.mButtons_Focasable[0], false);
		this.ChangeFocusTexture(this.mShipAlbumDetailTextureInfos[0]);
	}

	protected void PlayVoice()
	{
		if (this.mVoiceAudioSource != null)
		{
			this.mVoiceAudioSource.Stop();
			AudioClip clip = this.mVoiceAudioSource.get_clip();
			Resources.UnloadAsset(clip);
		}
		int voiceMstId = this.mAlbumShipModel.GetVoiceMstId(25);
		AudioClip clip2 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(voiceMstId, 25);
		this.mVoiceAudioSource = SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(clip2);
	}

	protected void NextImage()
	{
		int num = Array.IndexOf<UIShipAlbumDetail.ShipAlbumDetailTextureInfo>(this.mShipAlbumDetailTextureInfos, this.mCurrentShipAlbumDetailTextureInfo);
		int num2 = num + 1;
		UIShipAlbumDetail.ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo;
		if (num2 < this.mShipAlbumDetailTextureInfos.Length)
		{
			shipAlbumDetailTextureInfo = this.mShipAlbumDetailTextureInfos[num2];
		}
		else
		{
			shipAlbumDetailTextureInfo = this.mShipAlbumDetailTextureInfos[0];
		}
		this.ChangeFocusTexture(shipAlbumDetailTextureInfo);
	}

	protected void PrevImage()
	{
		int num = Array.IndexOf<UIShipAlbumDetail.ShipAlbumDetailTextureInfo>(this.mShipAlbumDetailTextureInfos, this.mCurrentShipAlbumDetailTextureInfo);
		int num2 = num - 1;
		UIShipAlbumDetail.ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo;
		if (0 <= num2)
		{
			shipAlbumDetailTextureInfo = this.mShipAlbumDetailTextureInfos[num2];
		}
		else
		{
			shipAlbumDetailTextureInfo = this.mShipAlbumDetailTextureInfos[this.mShipAlbumDetailTextureInfos.Length - 1];
		}
		this.ChangeFocusTexture(shipAlbumDetailTextureInfo);
	}

	private void ChangeFocusTexture(UIShipAlbumDetail.ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo)
	{
		if (this.mChangeFocusTextureCoroutine != null)
		{
			base.StopCoroutine(this.mChangeFocusTextureCoroutine);
		}
		this.mChangeFocusTextureCoroutine = this.ChangeFocusTextureCoroutine(shipAlbumDetailTextureInfo);
		base.StartCoroutine(this.mChangeFocusTextureCoroutine);
	}

	[DebuggerHidden]
	private IEnumerator ChangeFocusTextureCoroutine(UIShipAlbumDetail.ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo)
	{
		UIShipAlbumDetail.<ChangeFocusTextureCoroutine>c__Iterator27 <ChangeFocusTextureCoroutine>c__Iterator = new UIShipAlbumDetail.<ChangeFocusTextureCoroutine>c__Iterator27();
		<ChangeFocusTextureCoroutine>c__Iterator.shipAlbumDetailTextureInfo = shipAlbumDetailTextureInfo;
		<ChangeFocusTextureCoroutine>c__Iterator.<$>shipAlbumDetailTextureInfo = shipAlbumDetailTextureInfo;
		<ChangeFocusTextureCoroutine>c__Iterator.<>f__this = this;
		return <ChangeFocusTextureCoroutine>c__Iterator;
	}

	private void ChangeFocusButton(UIButton targetButton, bool needSe)
	{
		if (this.mCurrentFocusButton != null)
		{
			if (!this.mCurrentFocusButton.Equals(targetButton))
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
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, true);
		}
		this.mPentagonChart.Play();
		this.PlayVoice();
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
		if (this.mVoiceAudioSource != null)
		{
			this.mVoiceAudioSource.Stop();
		}
		return TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
		{
			this.mPanelThis.alpha = alpha;
		}), this);
	}
}
