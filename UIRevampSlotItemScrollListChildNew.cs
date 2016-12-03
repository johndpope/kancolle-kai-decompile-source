using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIRevampSlotItemScrollListChildNew : MonoBehaviour, UIScrollListItem<SlotitemModel, UIRevampSlotItemScrollListChildNew>
{
	private const int LEVEL_MAX = 10;

	private UIWidget mWidgetThis;

	[SerializeField]
	private UISprite mSprite_WeaponTypeIcon;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Level;

	[SerializeField]
	private UISprite mSprite_LevelMax;

	[SerializeField]
	private GameObject mLevel;

	[SerializeField]
	private Transform mTransform_BlinkObject;

	[SerializeField]
	private Transform mLock;

	private Action<UIRevampSlotItemScrollListChildNew> mOnTouchListener;

	private Transform mCachedTransform;

	private SlotitemModel mSlotItemModel;

	private int mRealIndex;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
	}

	public void Initialize(int realIndex, SlotitemModel model)
	{
		this.mRealIndex = realIndex;
		this.mSlotItemModel = model;
		this.mWidgetThis.alpha = 1f;
		if (model.Level == 10)
		{
			this.mLevel.SetActive(true);
			this.mSprite_LevelMax.SetActive(true);
		}
		else if (0 < model.Level && model.Level < 10)
		{
			this.mSprite_LevelMax.SetActive(false);
			this.mLevel.SetActive(true);
			this.mLabel_Level.text = model.Level.ToString();
		}
		else
		{
			this.mLevel.SetActive(false);
		}
		this.mLabel_Name.text = model.Name;
		this.mSprite_WeaponTypeIcon.spriteName = "icon_slot" + model.Type4;
		this.mLock.SetActive(model.IsLocked());
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mSlotItemModel = null;
		this.mWidgetThis.alpha = 1E-08f;
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}

	public SlotitemModel GetModel()
	{
		return this.mSlotItemModel;
	}

	public int GetHeight()
	{
		return 90;
	}

	public void SetOnTouchListener(Action<UIRevampSlotItemScrollListChildNew> onTouchListener)
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
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_BlinkObject.get_gameObject(), true);
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
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_BlinkObject.get_gameObject(), false);
	}

	private void OnDestroy()
	{
		this.mWidgetThis = null;
		this.mSprite_WeaponTypeIcon = null;
		this.mLabel_Name = null;
		this.mLabel_Level = null;
		this.mSprite_LevelMax = null;
		this.mLevel = null;
		this.mTransform_BlinkObject = null;
		this.mOnTouchListener = null;
		this.mCachedTransform = null;
		this.mSlotItemModel = null;
	}
}
