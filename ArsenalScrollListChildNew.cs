using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class ArsenalScrollListChildNew : MonoBehaviour, UIScrollListItem<ShipModel, ArsenalScrollListChildNew>
{
	public delegate bool CheckSelectedDelegate(ShipModel shipModel);

	[SerializeField]
	private UILabel _labelName;

	[SerializeField]
	private Transform mBackground;

	[SerializeField]
	private UILabel _labelLv;

	[SerializeField]
	private UISprite _check;

	[SerializeField]
	private UISprite _ShipType;

	[SerializeField]
	private UISprite _LockType;

	private UIWidget mWidgetThis;

	private ShipModel mShipModel;

	private int mRealIndex;

	private Action<ArsenalScrollListChildNew> mOnTouchListener;

	private ArsenalScrollListChildNew.CheckSelectedDelegate mCheckSelectedDelegate;

	private Transform mCachedTransform;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-08f;
	}

	public void refresh()
	{
		this._check.alpha = 0f;
	}

	private void UpdateCheckState(bool enabled)
	{
		if (enabled)
		{
			this._check.alpha = 1f;
		}
		else
		{
			this._check.alpha = 0f;
		}
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = model;
		this.refresh();
		this._ShipType.spriteName = "ship" + this.mShipModel.ShipType;
		this._labelName.text = this.mShipModel.Name;
		this._labelLv.textInt = this.mShipModel.Level;
		if (this.mShipModel.IsLocked())
		{
			this._LockType.spriteName = "lock_ship";
		}
		else if (this.mShipModel.HasLocked())
		{
			this._LockType.spriteName = "lock_on";
		}
		else
		{
			this._LockType.spriteName = null;
		}
		bool flag = this.CheckSelected(this.mShipModel);
		if (flag)
		{
			this.UpdateCheckState(true);
		}
		else
		{
			this.UpdateCheckState(false);
		}
		this.mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
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
		return 61;
	}

	public void SetOnTouchListener(Action<ArsenalScrollListChildNew> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public void SetCheckSelectedDelegate(ArsenalScrollListChildNew.CheckSelectedDelegate checkSelectedDelegate)
	{
		this.mCheckSelectedDelegate = checkSelectedDelegate;
	}

	private bool CheckSelected(ShipModel shipModel)
	{
		return this.mCheckSelectedDelegate != null && this.mCheckSelectedDelegate(shipModel);
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
		UISelectedObject.SelectedOneObjectBlink(this.mBackground.get_gameObject(), true);
	}

	public Transform GetTransform()
	{
		if (this.mCachedTransform == null)
		{
			this.mCachedTransform = base.get_transform();
		}
		return this.mCachedTransform;
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mBackground.get_gameObject(), false);
	}

	private void OnDestroy()
	{
		this._labelName = null;
		this.mBackground = null;
		this._labelLv = null;
		this._check = null;
		this._ShipType = null;
		this._LockType = null;
		this.mWidgetThis = null;
		this.mShipModel = null;
	}
}
