using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIFurnitureStoreTabListChild : MonoBehaviour, UIScrollListItem<FurnitureModel, UIFurnitureStoreTabListChild>
{
	private UIWidget mWidgetThis;

	private int mRealIndex;

	private FurnitureModel mFurnitureModel;

	private Action<UIFurnitureStoreTabListChild> mOnTouchListener;

	private Transform mCachedTransform;

	[SerializeField]
	private GameObject mBackground;

	[SerializeField]
	private UILabel CoinValue;

	[SerializeField]
	private UILabel Name;

	[SerializeField]
	private UILabel Detail;

	[SerializeField]
	private UISprite[] Stars;

	[SerializeField]
	private UILabel SoldOut;

	[SerializeField]
	private UITexture texture;

	private Action<Texture> mReleaseRequestFurnitureTexture;

	public UITexture GetFurnitureTextureView()
	{
		return this.texture;
	}

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-07f;
	}

	public void Initialize(int realIndex, FurnitureModel model)
	{
		this.mRealIndex = realIndex;
		this.mFurnitureModel = model;
		this.CoinValue.textInt = model.Price;
		this.Name.text = model.Name;
		this.Detail.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(36, 1, model.Description);
		this.SetStars(model.Rarity);
		this.SoldOut.set_enabled(model.IsPossession());
		if (this.texture.mainTexture != null)
		{
			this.ReleaseRequestFurnitureTexture(this.texture.mainTexture);
		}
		this.texture.mainTexture = null;
		this.texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(model.Type, model.MstId);
		this.mWidgetThis.alpha = 1f;
	}

	private void SetStars(int num)
	{
		for (int i = 0; i < this.Stars.Length; i++)
		{
			this.Stars[i].SetActive(num > i);
		}
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mFurnitureModel = null;
		if (this.texture.mainTexture != null)
		{
			this.ReleaseRequestFurnitureTexture(this.texture.mainTexture);
		}
		this.texture.mainTexture = null;
		this.mWidgetThis.alpha = 1E-07f;
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}

	public FurnitureModel GetModel()
	{
		return this.mFurnitureModel;
	}

	public int GetHeight()
	{
		return 154;
	}

	public void SetOnTouchListener(Action<UIFurnitureStoreTabListChild> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void Touch()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mBackground, true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mBackground, false);
	}

	public Transform GetTransform()
	{
		if (this.mCachedTransform == null)
		{
			this.mCachedTransform = base.get_transform();
		}
		return this.mCachedTransform;
	}

	public void SetOnReleaseRequestFurnitureTextureListener(Action<Texture> releaseRequestFurnitureTexture)
	{
		this.mReleaseRequestFurnitureTexture = releaseRequestFurnitureTexture;
	}

	private void ReleaseRequestFurnitureTexture(Texture requestTexture)
	{
		if (this.mReleaseRequestFurnitureTexture != null)
		{
			this.mReleaseRequestFurnitureTexture.Invoke(requestTexture);
		}
	}

	private void OnDestroy()
	{
		this.mReleaseRequestFurnitureTexture = null;
		this.mWidgetThis = null;
		this.mFurnitureModel = null;
		this.mOnTouchListener = null;
		this.mCachedTransform = null;
		this.mBackground = null;
		this.CoinValue = null;
		this.Name = null;
		this.Detail = null;
		this.Stars = null;
		this.SoldOut = null;
		this.texture = null;
	}
}
