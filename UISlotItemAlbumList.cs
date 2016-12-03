using KCV;
using KCV.Display;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UISlotItemAlbumList : MonoBehaviour
{
	private const int PAGE_IN_ITEM_COUNT = 10;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UISlotItemAlbumListItem[] mUISlotItemAlbumListItems;

	[SerializeField]
	private UILabel mLabel_PageCurrent;

	[SerializeField]
	private UILabel mLabel_PageTotal;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

	private int mCurrentPageIndex;

	private UISlotItemAlbumListItem mCurrentFocusListItem;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private Action<IAlbumModel> mOnSelectedListItemListener;

	private Action mOnBackListener;

	private IEnumerator mChangePageCoroutine;

	private void Awake()
	{
		UISlotItemAlbumListItem[] array = this.mUISlotItemAlbumListItems;
		for (int i = 0; i < array.Length; i++)
		{
			UISlotItemAlbumListItem uISlotItemAlbumListItem = array[i];
			uISlotItemAlbumListItem.SetOnSelectedListener(new Action<UISlotItemAlbumListItem>(this.OnSelectedListItemListener));
		}
		this.mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.SwipeJudgeDelegate));
	}

	private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
	{
		if (this.mKeyController != null)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (0.3f < movePercentageX)
				{
					this.PrevPage();
				}
				else if (movePercentageX < -0.3f)
				{
					this.NextPage();
				}
			}
		}
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (this.mKeyController.IsRSLeftDown())
			{
				this.PrevPage();
			}
			else if (this.mKeyController.IsRSRightDown())
			{
				this.NextPage();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.OnBack();
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.OnSelectedListItemListener(this.mCurrentFocusListItem);
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UISlotItemAlbumListItem>(this.mUISlotItemAlbumListItems, this.mCurrentFocusListItem);
				int num2 = num - 1;
				if (0 <= num2)
				{
					UISlotItemAlbumListItem target = this.mUISlotItemAlbumListItems[num2];
					this.ChangeFocus(target, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UISlotItemAlbumListItem>(this.mUISlotItemAlbumListItems, this.mCurrentFocusListItem);
				int num4 = num3 + 1;
				if (num4 < this.mUISlotItemAlbumListItems.Length)
				{
					UISlotItemAlbumListItem target2 = this.mUISlotItemAlbumListItems[num4];
					this.ChangeFocus(target2, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				int num5 = Array.IndexOf<UISlotItemAlbumListItem>(this.mUISlotItemAlbumListItems, this.mCurrentFocusListItem);
				int num6 = num5 - 5;
				if (0 <= num6)
				{
					UISlotItemAlbumListItem target3 = this.mUISlotItemAlbumListItems[num6];
					this.ChangeFocus(target3, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(12).down)
			{
				int num7 = Array.IndexOf<UISlotItemAlbumListItem>(this.mUISlotItemAlbumListItems, this.mCurrentFocusListItem);
				int num8 = num7 + 5;
				if (num8 < this.mUISlotItemAlbumListItems.Length)
				{
					UISlotItemAlbumListItem target4 = this.mUISlotItemAlbumListItems[num8];
					this.ChangeFocus(target4, true);
				}
			}
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		this.mCurrentPageIndex = 0;
		this.mAlbumModels = albumModels;
	}

	public void SetOnSelectedListItemListener(Action<IAlbumModel> onSelectedListItemListener)
	{
		this.mOnSelectedListItemListener = onSelectedListItemListener;
	}

	public void SetOnBackListener(Action onBackListener)
	{
		this.mOnBackListener = onBackListener;
	}

	public void StartState()
	{
		this.ChangePage(0, false);
		this.ChangeFocus(this.mUISlotItemAlbumListItems[0], false);
	}

	private void ChangeFocus(UISlotItemAlbumListItem target, bool needSe)
	{
		if (this.mCurrentFocusListItem != null && !this.mCurrentFocusListItem.Equals(target))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
		this.mCurrentFocusListItem = target;
		if (this.mCurrentFocusListItem != null)
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Focus.get_gameObject(), true);
			this.mTexture_Focus.get_transform().set_localPosition(this.mCurrentFocusListItem.get_transform().get_localPosition());
		}
		else
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Focus.get_gameObject(), false);
		}
	}

	private void NextPage()
	{
		int pageIndex = this.mCurrentPageIndex + 1;
		bool flag = this.HasPageAt(pageIndex);
		if (flag)
		{
			this.ChangePage(pageIndex, true);
			this.ChangeFocus(this.mUISlotItemAlbumListItems[0], false);
		}
	}

	private void PrevPage()
	{
		int pageIndex = this.mCurrentPageIndex - 1;
		bool flag = this.HasPageAt(pageIndex);
		if (flag)
		{
			this.ChangePage(pageIndex, true);
			this.ChangeFocus(this.mUISlotItemAlbumListItems[0], false);
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchEvent()
	{
		this.OnBack();
	}

	private void OnBack()
	{
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke();
		}
	}

	private bool HasPageAt(int pageIndex)
	{
		return 0 <= pageIndex && pageIndex < this.mAlbumModels.Length / 10 + ((this.mAlbumModels.Length % 10 != 0) ? 1 : 0);
	}

	private void ChangePage(int pageIndex, bool needSe)
	{
		if (this.mChangePageCoroutine != null)
		{
			base.StopCoroutine(this.mChangePageCoroutine);
		}
		this.mChangePageCoroutine = this.ChangePageCoroutine(pageIndex);
		if (needSe)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_002);
		}
		base.StartCoroutine(this.mChangePageCoroutine);
	}

	[DebuggerHidden]
	private IEnumerator ChangePageCoroutine(int pageIndex)
	{
		UISlotItemAlbumList.<ChangePageCoroutine>c__Iterator2A <ChangePageCoroutine>c__Iterator2A = new UISlotItemAlbumList.<ChangePageCoroutine>c__Iterator2A();
		<ChangePageCoroutine>c__Iterator2A.pageIndex = pageIndex;
		<ChangePageCoroutine>c__Iterator2A.<$>pageIndex = pageIndex;
		<ChangePageCoroutine>c__Iterator2A.<>f__this = this;
		return <ChangePageCoroutine>c__Iterator2A;
	}

	private void OnSelectedListItemListener(UISlotItemAlbumListItem calledObject)
	{
		IAlbumModel albumModel = calledObject.GetAlbumModel();
		bool flag = albumModel != null;
		bool flag2 = this.mOnSelectedListItemListener != null;
		if (flag2 && flag)
		{
			this.ChangeFocus(calledObject, false);
			this.mOnSelectedListItemListener.Invoke(albumModel);
		}
	}

	public void ResumeState()
	{
	}

	private void OnDestroy()
	{
		this.mTexture_Focus = null;
		this.mUISlotItemAlbumListItems = null;
		this.mLabel_PageCurrent = null;
		this.mLabel_PageTotal = null;
		this.mUIDisplaySwipeEventRegion = null;
		this.mCurrentFocusListItem = null;
		this.mKeyController = null;
		this.mAlbumModels = null;
		this.mOnSelectedListItemListener = null;
		this.mOnBackListener = null;
	}
}
