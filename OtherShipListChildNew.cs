using KCV.Supply;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class OtherShipListChildNew : MonoBehaviour, UIScrollListItem<ShipModel, OtherShipListChildNew>
{
	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	[SerializeField]
	public UISupplyOtherShipBanner shipBanner;

	private Transform mTransform;

	private Action<OtherShipListChildNew> mOnTouchListener;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
	}

	public void Initialize(int realIndex, ShipModel shipModel)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = shipModel;
		base.set_enabled(true);
		this.shipBanner.Init();
		this.shipBanner.SetBanner(shipModel, this.mRealIndex);
		this.shipBanner.Select(SupplyMainManager.Instance._otherListParent.isSelected(this.mShipModel));
		this.mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = null;
		this.mWidgetThis.alpha = 1E-09f;
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
		return 56;
	}

	private void OnClick()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}

	public void SetOnTouchListener(Action<OtherShipListChildNew> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		this.shipBanner.Hover(true);
	}

	public void RemoveHover()
	{
		this.shipBanner.Hover(false);
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
