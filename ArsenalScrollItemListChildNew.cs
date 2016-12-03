using KCV;
using KCV.Arsenal;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class ArsenalScrollItemListChildNew : MonoBehaviour, UIScrollListItem<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>
{
	private ArsenalScrollSlotItemListChoiceModel mArsenalScrollItemListChoiceModel;

	private UIWidget mWidgetThis;

	[SerializeField]
	private UILabel mLabel_Rare;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UISprite mSprite_Check;

	[SerializeField]
	private UISprite mSprite_CheckBox;

	[SerializeField]
	private UISprite mSprite_SlotItem;

	[SerializeField]
	private UISprite mSprite_LockIcon;

	[SerializeField]
	private Transform mTransform_Background;

	private Transform mCachedTransform;

	private Action<ArsenalScrollItemListChildNew> mOnTouchListener;

	private int mRealIndex;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-08f;
	}

	private void InitializeItemInfo(SlotitemModel slotItemModel)
	{
		this.mLabel_Rare.text = Util.RareToString(slotItemModel.Rare);
		this.mLabel_Name.text = slotItemModel.Name;
		this.mSprite_SlotItem.spriteName = "icon_slot" + slotItemModel.Type4;
		this.mSprite_LockIcon.spriteName = ((!slotItemModel.IsLocked()) ? null : "lock_on");
	}

	private void UpdateListSelect(bool enabled)
	{
		this.mSprite_CheckBox.spriteName = ((!enabled) ? "check_bg" : "check_bg_on");
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}

	public int GetHeight()
	{
		return 54;
	}

	public void SetOnTouchListener(Action<ArsenalScrollItemListChildNew> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), true);
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
		UISelectedObject.SelectedOneObjectBlink(this.mTransform_Background.get_gameObject(), false);
	}

	private void OnDestroy()
	{
		this.mArsenalScrollItemListChoiceModel = null;
		this.mLabel_Rare = null;
		this.mLabel_Name = null;
		this.mSprite_Check = null;
		this.mSprite_CheckBox = null;
		this.mSprite_SlotItem = null;
		this.mSprite_LockIcon = null;
		this.mTransform_Background = null;
		this.mCachedTransform = null;
		this.mOnTouchListener = null;
	}

	public void Initialize(int realIndex, ArsenalScrollSlotItemListChoiceModel model)
	{
		this.mRealIndex = realIndex;
		this.mArsenalScrollItemListChoiceModel = model;
		this.mSprite_Check.alpha = 1E-08f;
		this.InitializeItemInfo(this.mArsenalScrollItemListChoiceModel.GetSlotItemModel());
		this.UpdateSelectedView(this.mArsenalScrollItemListChoiceModel.Selected);
		this.mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mArsenalScrollItemListChoiceModel = null;
		this.mWidgetThis.alpha = 1E-08f;
	}

	private void UpdateSelectedView(bool selected)
	{
		this.mSprite_Check.alpha = ((!selected) ? 1E-09f : 1f);
	}

	public ArsenalScrollSlotItemListChoiceModel GetModel()
	{
		return this.mArsenalScrollItemListChoiceModel;
	}

	public void Touch()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}
}
