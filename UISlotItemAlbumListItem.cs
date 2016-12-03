using DG.Tweening;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UISlotItemAlbumListItem : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_SlotItemCard;

	[SerializeField]
	private UILabel mLabel_Number;

	private UIWidget mWidgetThis;

	private IAlbumModel mAlbumModel;

	private Action<UISlotItemAlbumListItem> mOnSelectedListener;

	private IEnumerator mInitializeCoroutine;

	public IAlbumModel GetAlbumModel()
	{
		return this.mAlbumModel;
	}

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 0f;
	}

	public void InitializeDefailt()
	{
		if (this.mInitializeCoroutine != null)
		{
			base.StopCoroutine(this.mInitializeCoroutine);
		}
		this.mAlbumModel = null;
		this.mLabel_Number.text = string.Empty;
		this.mTexture_SlotItemCard.mainTexture = null;
		this.mWidgetThis.alpha = 0f;
	}

	public void SetOnSelectedListener(Action<UISlotItemAlbumListItem> onSelectedListener)
	{
		this.mOnSelectedListener = onSelectedListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchListItem()
	{
		this.OnSelectedListItem();
	}

	public void SelectItem()
	{
		this.OnSelectedListItem();
	}

	private void OnSelectedListItem()
	{
		if (this.mOnSelectedListener != null)
		{
			this.mOnSelectedListener.Invoke(this);
		}
	}

	[DebuggerHidden]
	public IEnumerator GenerateInitializeCoroutine(IAlbumModel albumModel)
	{
		UISlotItemAlbumListItem.<GenerateInitializeCoroutine>c__Iterator2B <GenerateInitializeCoroutine>c__Iterator2B = new UISlotItemAlbumListItem.<GenerateInitializeCoroutine>c__Iterator2B();
		<GenerateInitializeCoroutine>c__Iterator2B.albumModel = albumModel;
		<GenerateInitializeCoroutine>c__Iterator2B.<$>albumModel = albumModel;
		<GenerateInitializeCoroutine>c__Iterator2B.<>f__this = this;
		return <GenerateInitializeCoroutine>c__Iterator2B;
	}

	public void Hide()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		this.mWidgetThis.alpha = 0f;
	}

	public void Show()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
		TweenCallback tweenCallback = delegate
		{
			this.mWidgetThis.alpha = 0f;
		};
		Tween tween = DOVirtual.Float(this.mWidgetThis.alpha, 1f, 0.15f, delegate(float alpha)
		{
			this.mWidgetThis.alpha = alpha;
		});
		TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
		TweenSettingsExtensions.Append(sequence, tween);
	}

	private void OnDestroy()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		this.mTexture_SlotItemCard = null;
		this.mLabel_Number = null;
		this.mWidgetThis = null;
		this.mAlbumModel = null;
		this.mOnSelectedListener = null;
	}
}
