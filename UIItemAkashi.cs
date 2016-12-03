using DG.Tweening;
using KCV;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIItemAkashi : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Akashi;

	[SerializeField]
	private Texture[] mTextureAkashi;

	[SerializeField]
	private UIButton mButton_Akashi;

	[SerializeField]
	private Vector3 mVector3_Akashi_ShowLocalPosition;

	[SerializeField]
	private Vector3 mVector3_Akashi_GoBackLocalPosition;

	[SerializeField]
	private Vector3 mVector3_Akashi_WaitingLocalPosition;

	private bool mShown;

	private Action mOnHidenCallBack;

	private KeyControl mKeyController;

	private void Start()
	{
		this.mTexture_Akashi.get_transform().set_localPosition(this.mVector3_Akashi_WaitingLocalPosition);
		this.mTexture_Akashi.mainTexture = this.mTextureAkashi[0];
		this.mShown = false;
	}

	private void Update()
	{
		if (this.mKeyController != null && this.mShown && this.mKeyController.IsAnyKey)
		{
			this.Hide(0.5f);
		}
	}

	public void Show()
	{
		TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMove(this.mTexture_Akashi.get_transform(), this.mVector3_Akashi_ShowLocalPosition, 0.4f, false), delegate
		{
			this.mShown = true;
		}), 3);
	}

	public void SetClickable(bool clickable)
	{
		this.mButton_Akashi.isEnabled = clickable;
	}

	public void Hide()
	{
		this.Hide(0f);
	}

	private void Hide(float delay)
	{
		this.mTexture_Akashi.mainTexture = this.mTextureAkashi[1];
		TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMove(this.mTexture_Akashi.get_transform(), this.mVector3_Akashi_GoBackLocalPosition, 0.4f, false), delay), delegate
		{
			this.mShown = false;
			this.OnHidden();
		}), 3);
	}

	public void SetOnHiddenCallBack(Action onHidenCallBack)
	{
		this.mOnHidenCallBack = onHidenCallBack;
	}

	private void OnHidden()
	{
		if (this.mOnHidenCallBack != null)
		{
			this.mOnHidenCallBack.Invoke();
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void OnTouchAkashi()
	{
		if (this.mShown)
		{
			this.Hide();
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextureAkashi, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Akashi, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Akashi);
	}
}
