using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UserInterfaceInteriorTransitionManager : MonoBehaviour
{
	[SerializeField]
	private UIFurnitureYousei mUIFurnitureYousei;

	[SerializeField]
	private UITexture mTexture_Background;

	private UIPanel mPanelThis;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 1E-06f;
	}

	public void SwitchToStore(Action onFinishedAnimation)
	{
		DOTween.Kill(this, false);
		TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
		{
			this.mPanelThis.alpha = alpha;
		}), this);
		this.mUIFurnitureYousei.Initialize(UIFurnitureYousei.YouseiType.Store);
		this.mUIFurnitureYousei.StartWalk();
		this.mUIFurnitureYousei.get_transform().set_localPosition(Vector3.get_zero());
		this.mUIFurnitureYousei.get_transform().set_localScale(Vector3.get_zero());
		TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mUIFurnitureYousei.get_transform(), 12.5f, 0.2f, false), 30, 1), this);
		TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mUIFurnitureYousei.get_transform(), Vector3.get_one(), 0.6f), 15), delegate
		{
			TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.mUIFurnitureYousei.get_transform(), new Vector3(2f, 2f, 2f), 1f), 0.5f), this);
			TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(this.mUIFurnitureYousei.alpha, 0f, 0.6f, delegate(float alpha)
			{
				this.mUIFurnitureYousei.alpha = alpha;
			}), 0.5f), 18), delegate
			{
				TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(1f, 0f, 0.3f, delegate(float alpha)
				{
					this.mPanelThis.alpha = alpha;
				}), this);
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), this);
		}), this);
	}

	public void SwitchToHome(Action onFinishedAnimation)
	{
		DOTween.Kill(this, false);
		TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
		{
			this.mPanelThis.alpha = alpha;
		}), this);
		this.mUIFurnitureYousei.Initialize(UIFurnitureYousei.YouseiType.Room);
		this.mUIFurnitureYousei.StartWalk();
		this.mUIFurnitureYousei.get_transform().set_localPosition(Vector3.get_zero());
		this.mUIFurnitureYousei.get_transform().set_localScale(Vector3.get_zero());
		TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mUIFurnitureYousei.get_transform(), 12.5f, 0.2f, false), 30, 1), this);
		TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mUIFurnitureYousei.get_transform(), Vector3.get_one(), 0.6f), 15), delegate
		{
			TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.mUIFurnitureYousei.get_transform(), new Vector3(2f, 2f, 2f), 1f), 0.5f), this);
			TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(this.mUIFurnitureYousei.alpha, 0f, 0.6f, delegate(float alpha)
			{
				this.mUIFurnitureYousei.alpha = alpha;
			}), 0.5f), 18), delegate
			{
				TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(1f, 0f, 0.3f, delegate(float alpha)
				{
					this.mPanelThis.alpha = alpha;
				}), this);
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), this);
		}), this);
	}

	public void Release()
	{
		DOTween.Kill(this, false);
		this.mUIFurnitureYousei.Release();
		this.mTexture_Background.mainTexture = null;
		this.mTexture_Background = null;
		this.mUIFurnitureYousei = null;
		this.mPanelThis = null;
	}
}
