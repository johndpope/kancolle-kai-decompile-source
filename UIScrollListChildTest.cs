using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIScrollListChildTest : MonoBehaviour, UIScrollListItem<ShipModel, UIScrollListChildTest>
{
	[SerializeField]
	private UILabel mLabel_ShipName;

	[SerializeField]
	private UITexture mTexture_ShipBanner;

	[SerializeField]
	private UITexture mTexture_Background;

	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	private Transform mTransform;

	private Action<UIScrollListChildTest> mOnTouchListener;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-09f;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = model;
		this.mTexture_ShipBanner.mainTexture = UIScrollListTest.sShipBannerManager.GetShipBanner(this.mShipModel);
		this.mLabel_ShipName.text = this.mShipModel.Name;
		NGUITools.ImmediatelyCreateDrawCalls(base.get_gameObject());
		this.mWidgetThis.alpha = 1f;
		base.set_name(string.Concat(new object[]
		{
			"[",
			realIndex,
			"]【",
			model.Name,
			"】"
		}));
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mTexture_ShipBanner.mainTexture = null;
		this.mLabel_ShipName.text = string.Empty;
		this.mShipModel = null;
		this.mWidgetThis.alpha = 1E-09f;
		base.set_name("[" + realIndex + "]");
	}

	public Transform GetTransform()
	{
		if (this.mTransform == null)
		{
			this.mTransform = base.get_transform();
		}
		return this.mTransform;
	}

	public ShipModel GetModel()
	{
		return this.mShipModel;
	}

	public int GetHeight()
	{
		return 75;
	}

	private void OnClick()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}

	public void SetOnTouchListener(Action<UIScrollListChildTest> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		this.mTexture_Background.color = Color.get_cyan();
	}

	public void RemoveHover()
	{
		this.mTexture_Background.color = Color.get_white();
	}

	public float GetBottomPositionY()
	{
		return this.GetTransform().get_localPosition().y + (float)this.GetHeight();
	}

	public float GetHeadPositionY()
	{
		return this.GetTransform().get_localPosition().y;
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}
}
