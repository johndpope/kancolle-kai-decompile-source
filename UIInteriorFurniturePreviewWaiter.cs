using KCV;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIInteriorFurniturePreviewWaiter : MonoBehaviour
{
	[SerializeField]
	private UIButton mButton_TouchBackArea;

	private KeyControl mKeyController;

	private Action mOnBackListener;

	private bool mIsWaiting;

	private void Awake()
	{
		if (this.mIsWaiting)
		{
			this.mButton_TouchBackArea.isEnabled = true;
		}
		else
		{
			this.mButton_TouchBackArea.isEnabled = false;
		}
	}

	public void SetOnBackListener(Action onBackListener)
	{
		this.mOnBackListener = onBackListener;
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void StartWait()
	{
		this.mIsWaiting = true;
		this.mButton_TouchBackArea.isEnabled = true;
	}

	public void ResumeWait()
	{
		this.mIsWaiting = true;
		this.mButton_TouchBackArea.isEnabled = true;
	}

	public void StopWait()
	{
		if (this.mIsWaiting)
		{
			this.mIsWaiting = false;
			this.mButton_TouchBackArea.isEnabled = false;
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		this.OnBack();
	}

	private void OnBack()
	{
		if (this.mIsWaiting)
		{
			this.mIsWaiting = false;
			this.mButton_TouchBackArea.isEnabled = false;
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}
	}

	private void Update()
	{
		if (this.mKeyController != null && this.mIsWaiting)
		{
			if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.OnBack();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.OnBack();
			}
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_TouchBackArea);
		this.mKeyController = null;
		this.mOnBackListener = null;
		this.mIsWaiting = false;
	}
}
