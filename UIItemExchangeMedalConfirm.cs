using Common.Enum;
using KCV;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIItemExchangeMedalConfirm : MonoBehaviour
{
	[SerializeField]
	private DialogAnimation mDialogAnimation;

	[SerializeField]
	private UIButton mButton_Plan;

	[SerializeField]
	private UIButton mButton_Screw;

	[SerializeField]
	private UIButton mButton_Materials;

	private UIButton[] mFocasableButtons;

	private UIButton mFocusButton;

	private UIPanel mPanelThis;

	private ItemlistModel mModel;

	private ItemlistManager mItemlistCheckUtils;

	private KeyControl mKeyController;

	private Action<ItemExchangeKinds> mOnExchangeCallBack;

	private Action mOnCancelCallBack;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 0f;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mFocusButton);
				int num2 = num - 1;
				if (0 <= num2)
				{
					this.ChangeFocus(this.mFocasableButtons[num2], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mFocusButton);
				int num4 = num3 + 1;
				if (num4 < this.mFocasableButtons.Length)
				{
					this.ChangeFocus(this.mFocasableButtons[num4], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				if (this.mButton_Plan.Equals(this.mFocusButton))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
					this.SelectExchange(ItemExchangeKinds.PLAN);
				}
				else if (this.mButton_Screw.Equals(this.mFocusButton))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
					this.SelectExchange(ItemExchangeKinds.REMODEL);
				}
				else if (this.mButton_Materials.Equals(this.mFocusButton))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter3);
					this.SelectExchange(ItemExchangeKinds.NONE);
				}
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.CancelExchange();
			}
		}
	}

	public void Initialize(ItemlistModel itemStoreModel, ItemlistManager checkUtils)
	{
		this.mModel = itemStoreModel;
		this.mItemlistCheckUtils = checkUtils;
		List<UIButton> list = new List<UIButton>();
		ItemlistModel listItem = checkUtils.GetListItem(57);
		if (listItem != null && 0 < listItem.Count)
		{
			if (4 <= listItem.Count)
			{
				this.mButton_Plan.isEnabled = true;
				list.Add(this.mButton_Plan);
			}
			else
			{
				this.mButton_Plan.isEnabled = false;
			}
			list.Add(this.mButton_Screw);
			list.Add(this.mButton_Materials);
			this.mFocasableButtons = list.ToArray();
			this.ChangeFocus(this.mFocasableButtons[0], false);
		}
		this.mButton_Plan.get_transform().get_parent().FindChild("Label_Message").GetComponent<UILabel>().text = "「資源」や「改修資材」に交換出来ます。\nまた、勲章4個を「改装設計図」1枚に交換可能です。";
	}

	public void Show(Action onFinished)
	{
		this.mPanelThis.alpha = 1f;
		if (!this.mDialogAnimation.IsOpen)
		{
			this.mDialogAnimation.OpenAction = delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			};
			this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, true);
		}
	}

	public void Close(Action onFinished)
	{
		if (this.mDialogAnimation.IsOpen)
		{
			this.mDialogAnimation.CloseAction = delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
				this.mPanelThis.alpha = 0f;
			};
			this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, false);
		}
	}

	public void SetOnExchangeItemSelectedCallBack(Action<ItemExchangeKinds> onExchangeCallBack)
	{
		this.mOnExchangeCallBack = onExchangeCallBack;
	}

	private void SelectExchange(ItemExchangeKinds exchangeKinds)
	{
		bool flag = this.mKeyController != null;
		if (flag && this.mOnExchangeCallBack != null)
		{
			this.mOnExchangeCallBack.Invoke(exchangeKinds);
		}
	}

	public void SetOnCancelCallBack(Action onCancelCallBack)
	{
		this.mOnCancelCallBack = onCancelCallBack;
	}

	private void OnCancel()
	{
		bool flag = this.mKeyController != null;
		if (flag && this.mOnCancelCallBack != null)
		{
			this.mOnCancelCallBack.Invoke();
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	private void ChangeFocus(UIButton targetButton, bool needSe)
	{
		if (this.mFocusButton != null)
		{
			this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
		}
		this.mFocusButton = targetButton;
		if (this.mFocusButton != null)
		{
			if (needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
		}
	}

	public void OnTouchPlan()
	{
		bool flag = 0 <= Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButton_Plan);
		if (flag)
		{
			this.ChangeFocus(this.mButton_Plan, false);
			this.SelectExchange(ItemExchangeKinds.PLAN);
		}
	}

	public void OnTouchScrew()
	{
		bool flag = 0 <= Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButton_Screw);
		if (flag)
		{
			this.ChangeFocus(this.mButton_Screw, false);
			this.SelectExchange(ItemExchangeKinds.REMODEL);
		}
	}

	public void OnTouchMaterials()
	{
		bool flag = 0 <= Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButton_Materials);
		if (flag)
		{
			this.ChangeFocus(this.mButton_Materials, false);
			this.SelectExchange(ItemExchangeKinds.NONE);
		}
	}

	private void CancelExchange()
	{
		SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
		this.OnCancel();
	}

	public void OnClickNegative()
	{
		this.ChangeFocus(null, false);
		this.CancelExchange();
	}

	public void Release()
	{
		this.mModel = null;
		this.mItemlistCheckUtils = null;
		this.mFocusButton = null;
		this.mFocasableButtons = null;
	}
}
