using KCV;
using KCV.Display;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[SelectionBase]
public class UIShipAlbumList : MonoBehaviour
{
	private const int PAGE_IN_ITEM_COUNT = 10;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UIShipAlbumListItem[] mUIShipAlbumListItems;

	[SerializeField]
	private UILabel mLabel_PageCurrent;

	[SerializeField]
	private UILabel mLabel_PageTotal;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

	private int mCurrentPageIndex;

	private UIShipAlbumListItem mCurrentFocusListItem;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private Action<IAlbumModel> mOnSelectedListItemListener;

	private Action mOnBackListener;

	private IEnumerator mChangePageCoroutine;

	private void Awake()
	{
		UIShipAlbumListItem[] array = this.mUIShipAlbumListItems;
		for (int i = 0; i < array.Length; i++)
		{
			UIShipAlbumListItem uIShipAlbumListItem = array[i];
			uIShipAlbumListItem.SetOnSelectedListener(new Action<UIShipAlbumListItem>(this.OnSelectedListItemListener));
		}
		this.mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.SwipeJudgeDelegate));
	}

	private void SwipeJudgeDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
	{
		if (this.mKeyController != null)
		{
			if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
			{
				if (0.2f < movePercentageX)
				{
					this.PrevPage();
				}
				else if (movePercentageX < -0.1f)
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
				int num = Array.IndexOf<UIShipAlbumListItem>(this.mUIShipAlbumListItems, this.mCurrentFocusListItem);
				int num2 = num - 1;
				if (0 <= num2)
				{
					UIShipAlbumListItem target = this.mUIShipAlbumListItems[num2];
					this.ChangeFocus(target, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UIShipAlbumListItem>(this.mUIShipAlbumListItems, this.mCurrentFocusListItem);
				int num4 = num3 + 1;
				if (num4 < this.mUIShipAlbumListItems.Length)
				{
					UIShipAlbumListItem target2 = this.mUIShipAlbumListItems[num4];
					this.ChangeFocus(target2, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				int num5 = Array.IndexOf<UIShipAlbumListItem>(this.mUIShipAlbumListItems, this.mCurrentFocusListItem);
				int num6 = num5 - 5;
				if (0 <= num6)
				{
					UIShipAlbumListItem target3 = this.mUIShipAlbumListItems[num6];
					this.ChangeFocus(target3, true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(12).down)
			{
				int num7 = Array.IndexOf<UIShipAlbumListItem>(this.mUIShipAlbumListItems, this.mCurrentFocusListItem);
				int num8 = num7 + 5;
				if (num8 < this.mUIShipAlbumListItems.Length)
				{
					UIShipAlbumListItem target4 = this.mUIShipAlbumListItems[num8];
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
		this.ChangeFocus(this.mUIShipAlbumListItems[0], false);
	}

	private void ChangeFocus(UIShipAlbumListItem target, bool needSe)
	{
		if (this.mCurrentFocusListItem != null && !this.mCurrentFocusListItem.Equals(target) && needSe)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}
		this.mCurrentFocusListItem = target;
		if (this.mCurrentFocusListItem != null)
		{
			this.mTexture_Focus.get_transform().set_localPosition(this.mCurrentFocusListItem.get_transform().get_localPosition());
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Focus.get_gameObject(), true);
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
			this.ChangeFocus(this.mUIShipAlbumListItems[0], false);
		}
	}

	private void PrevPage()
	{
		int pageIndex = this.mCurrentPageIndex - 1;
		bool flag = this.HasPageAt(pageIndex);
		if (flag)
		{
			this.ChangePage(pageIndex, true);
			this.ChangeFocus(this.mUIShipAlbumListItems[0], false);
		}
	}

	[Obsolete("Inspector上で選択して使用します.")]
	public void OnTouchBack()
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

	private void OnDestroy()
	{
		this.mTexture_Focus = null;
		this.mUIShipAlbumListItems = null;
		this.mLabel_PageCurrent = null;
		this.mLabel_PageTotal = null;
		this.mUIDisplaySwipeEventRegion = null;
		this.mCurrentFocusListItem = null;
		this.mKeyController = null;
		this.mAlbumModels = null;
		this.mOnSelectedListItemListener = null;
		this.mOnBackListener = null;
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
		UIShipAlbumList.<ChangePageCoroutine>c__Iterator28 <ChangePageCoroutine>c__Iterator = new UIShipAlbumList.<ChangePageCoroutine>c__Iterator28();
		<ChangePageCoroutine>c__Iterator.pageIndex = pageIndex;
		<ChangePageCoroutine>c__Iterator.<$>pageIndex = pageIndex;
		<ChangePageCoroutine>c__Iterator.<>f__this = this;
		return <ChangePageCoroutine>c__Iterator;
	}

	private void OnSelectedListItemListener(UIShipAlbumListItem calledObject)
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
}
