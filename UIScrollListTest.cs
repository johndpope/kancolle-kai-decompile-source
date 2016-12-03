using KCV;
using KCV.Resource;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UIScrollListTest : UIScrollList<ShipModel, UIScrollListChildTest>
{
	public static ShipBannerManager sShipBannerManager;

	[SerializeField]
	private ShipBannerManager mShipBannerManager;

	[SerializeField]
	private UILabel mLabel_Scrollisec;

	[SerializeField]
	private UILabel mLabel_ShipName;

	private KeyControl mKeyController;

	protected override void OnAwake()
	{
		UIScrollListTest.sShipBannerManager = this.mShipBannerManager;
	}

	protected override void OnStart()
	{
		PortManager portManager = new PortManager(1);
		ShipModel[] array = portManager.UserInfo.__GetShipList__().ToArray();
		this.mShipBannerManager.Initialize(array);
		this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
		string text = string.Empty;
		ShipModel[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ShipModel shipModel = array2[i];
			text += shipModel.Name;
		}
		this.mLabel_ShipName.text = text;
		base.Initialize(array);
		base.HeadFocus();
		base.StartControl();
	}

	[DebuggerHidden]
	private IEnumerator OnStartCoroutine()
	{
		UIScrollListTest.<OnStartCoroutine>c__Iterator1D3 <OnStartCoroutine>c__Iterator1D = new UIScrollListTest.<OnStartCoroutine>c__Iterator1D3();
		<OnStartCoroutine>c__Iterator1D.<>f__this = this;
		return <OnStartCoroutine>c__Iterator1D;
	}

	protected override void OnUpdate()
	{
		if (this.mKeyController != null && base.mState == UIScrollList<ShipModel, UIScrollListChildTest>.ListState.Waiting)
		{
			this.mKeyController.Update();
			if (this.mKeyController.keyState.get_Item(12).down)
			{
				base.NextFocus();
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				base.PrevFocus();
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				base.PrevPageOrHeadFocus();
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				base.NextPageOrTailFocus();
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				base.Select();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.mKeyController = null;
				base.StartCoroutine(this.MoveToPort());
			}
			else if (this.mKeyController.keyState.get_Item(4).down | this.mKeyController.keyState.get_Item(4).press)
			{
				this.mScrollCheckLevel -= 1f;
				this.mLabel_Scrollisec.text = this.mScrollCheckLevel.ToString();
			}
			else if (this.mKeyController.keyState.get_Item(5).down | this.mKeyController.keyState.get_Item(5).press)
			{
				this.mScrollCheckLevel += 1f;
				this.mLabel_Scrollisec.text = this.mScrollCheckLevel.ToString();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator MoveToPort()
	{
		return new UIScrollListTest.<MoveToPort>c__Iterator1D4();
	}

	protected override void OnChangedFocusView(UIScrollListChildTest focusToView)
	{
		if (focusToView == null)
		{
			return;
		}
		string mes = string.Concat(new string[]
		{
			focusToView.GetModel().Name,
			":",
			(focusToView.GetRealIndex() + 1).ToString(),
			"/",
			this.mModels.Length.ToString()
		});
		if (CommonPopupDialog.Instance != null)
		{
			CommonPopupDialog.Instance.StartPopup(mes);
		}
	}

	protected override void OnSelect(UIScrollListChildTest view)
	{
		if (view == null)
		{
			return;
		}
	}
}
