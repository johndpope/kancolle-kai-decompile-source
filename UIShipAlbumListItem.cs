using DG.Tweening;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), SelectionBase]
public class UIShipAlbumListItem : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_ShipCard;

	[SerializeField]
	private UILabel mLabel_Number;

	[SerializeField]
	private Animation mAnimation_MarriagedRing;

	private UIWidget mWidgetThis;

	private IAlbumModel mAlbumModel;

	private Action<UIShipAlbumListItem> mOnSelectedListener;

	public IAlbumModel GetAlbumModel()
	{
		return this.mAlbumModel;
	}

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
	}

	[DebuggerHidden]
	public IEnumerator GenerateInitializeCoroutine(IAlbumModel albumModel)
	{
		UIShipAlbumListItem.<GenerateInitializeCoroutine>c__Iterator29 <GenerateInitializeCoroutine>c__Iterator = new UIShipAlbumListItem.<GenerateInitializeCoroutine>c__Iterator29();
		<GenerateInitializeCoroutine>c__Iterator.albumModel = albumModel;
		<GenerateInitializeCoroutine>c__Iterator.<$>albumModel = albumModel;
		<GenerateInitializeCoroutine>c__Iterator.<>f__this = this;
		return <GenerateInitializeCoroutine>c__Iterator;
	}

	public void SetOnSelectedListener(Action<UIShipAlbumListItem> onSelectedListener)
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
		this.mTexture_ShipCard = null;
		this.mLabel_Number = null;
		this.mWidgetThis = null;
		this.mAlbumModel = null;
		this.mOnSelectedListener = null;
		this.mAnimation_MarriagedRing = null;
	}
}
