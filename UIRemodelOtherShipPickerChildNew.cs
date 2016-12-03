using DG.Tweening;
using KCV;
using KCV.Remodel;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIRemodelOtherShipPickerChildNew : MonoBehaviour, UIScrollListItem<ShipModel, UIRemodelOtherShipPickerChildNew>
{
	private enum AnimationType
	{
		Show
	}

	private UIWidget mWidgetThis;

	[SerializeField]
	private UILabel shipName;

	[SerializeField]
	private UILabel shipLevel;

	[SerializeField]
	private UiStarManager stars;

	[SerializeField]
	private CommonShipBanner CommonShipBanner;

	[SerializeField]
	private Transform mTransform_Background;

	private int mRealIndex;

	private ShipModel mShipModel;

	private Transform mTransformCache;

	private Action<UIRemodelOtherShipPickerChildNew> mOnTouchListener;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-07f;
	}

	public CommonShipBanner GetBanner()
	{
		return this.CommonShipBanner;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		UITexture uITexture = this.CommonShipBanner.GetUITexture();
		Texture mainTexture = uITexture.mainTexture;
		uITexture.mainTexture = null;
		UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
		this.mRealIndex = realIndex;
		this.mShipModel = model;
		this.CommonShipBanner.SetShipData(this.mShipModel);
		this.CommonShipBanner.StopParticle();
		uITexture.alpha = 1f;
		this.shipName.text = this.mShipModel.Name;
		this.shipLevel.text = this.mShipModel.Level.ToString();
		this.stars.init(this.mShipModel.Srate);
		this.mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		UITexture uITexture = this.CommonShipBanner.GetUITexture();
		Texture mainTexture = uITexture.mainTexture;
		uITexture.mainTexture = null;
		UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
		this.mRealIndex = realIndex;
		this.mShipModel = null;
		this.mWidgetThis.alpha = 1E-08f;
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}

	public ShipModel GetModel()
	{
		return this.mShipModel;
	}

	public int GetHeight()
	{
		return 58;
	}

	private void OnClick()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}

	public void SetOnTouchListener(Action<UIRemodelOtherShipPickerChildNew> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public Transform GetTransform()
	{
		if (this.mTransformCache == null)
		{
			this.mTransformCache = base.get_transform();
		}
		return this.mTransformCache;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), false);
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, false);
		}
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.shipName);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.shipLevel);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.CommonShipBanner, true);
		this.stars = null;
		this.CommonShipBanner = null;
		this.mTransform_Background = null;
		this.mShipModel = null;
		this.mTransformCache = null;
		this.mOnTouchListener = null;
	}
}
