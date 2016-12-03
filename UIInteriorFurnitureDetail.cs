using DG.Tweening;
using KCV;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIInteriorFurnitureDetail : MonoBehaviour
{
	private enum TweenAnimationType
	{
		ShowHide,
		Background
	}

	[SerializeField]
	private UITexture mTexture_Thumbnail;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UIButton mButton_Change;

	[SerializeField]
	private UIButton mButton_Preview;

	[SerializeField]
	private UITexture mTexture_TouchBackArea;

	[SerializeField]
	private OnClickEventSender mOnClickEventSender_TouchBackArea;

	[SerializeField]
	private Transform mEquipMark;

	private UIButton[] mFocasableButtons;

	private UIButton mFocusButton;

	private UIButtonManager mButtonManager;

	private int mDeckId;

	private FurnitureModel mFurnitureModel;

	private KeyControl mKeyController;

	private Action mOnSelectBackListener;

	private Action mOnSelectChangeListener;

	private Action mOnSelectPreviewListener;

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void Initialize(int deckId, FurnitureModel furnitureModel)
	{
		this.mDeckId = deckId;
		this.mFurnitureModel = furnitureModel;
		this.mTexture_Thumbnail.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(furnitureModel.Type, furnitureModel.MstId);
		this.mLabel_Name.text = furnitureModel.Name;
		this.mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(14, 1, furnitureModel.Description);
		bool settingFlg = this.mFurnitureModel.GetSettingFlg(this.mDeckId);
		if (settingFlg)
		{
			this.mEquipMark.SetActive(true);
		}
		else
		{
			this.mEquipMark.SetActive(false);
		}
		this.mOnClickEventSender_TouchBackArea.SetClickable(false);
	}

	public void SetOnSelectBackListener(Action onSelectBackListener)
	{
		this.mOnSelectBackListener = onSelectBackListener;
	}

	public void SetOnSelectChangeListener(Action onSelectChangeListener)
	{
		this.mOnSelectChangeListener = onSelectChangeListener;
	}

	public void SetOnSelectPreviewListener(Action onSelectPreviewListener)
	{
		this.mOnSelectPreviewListener = onSelectPreviewListener;
	}

	private void Start()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(this.mButton_Change);
		list.Add(this.mButton_Preview);
		this.mFocasableButtons = list.ToArray();
	}

	private void Awake()
	{
		this.mButtonManager = base.GetComponent<UIButtonManager>();
		this.mButtonManager.IndexChangeAct = delegate
		{
			this.ChangeFocus(this.mButtonManager.nowForcusButton);
		};
	}

	public void StartState()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureDetail.TweenAnimationType.Background))
		{
			DOTween.Kill(UIInteriorFurnitureDetail.TweenAnimationType.Background, false);
		}
		Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_TouchBackArea.alpha, 0.5f, 0.3f, delegate(float alpha)
		{
			this.mTexture_TouchBackArea.alpha = alpha;
		}), UIInteriorFurnitureDetail.TweenAnimationType.Background);
		UIButton[] array = this.mFocasableButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIButton uIButton = array[i];
			uIButton.set_enabled(true);
		}
		this.ChangeFocus(this.mFocasableButtons[0]);
		this.mOnClickEventSender_TouchBackArea.SetClickable(true);
	}

	public void Show()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureDetail.TweenAnimationType.ShowHide))
		{
			DOTween.Kill(UIInteriorFurnitureDetail.TweenAnimationType.ShowHide, false);
		}
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.3f);
		tweenPosition.from = base.get_transform().get_localPosition();
		tweenPosition.to = new Vector3(0f, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
		tweenPosition.ignoreTimeScale = true;
	}

	public void Hide()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureDetail.TweenAnimationType.ShowHide))
		{
			DOTween.Kill(UIInteriorFurnitureDetail.TweenAnimationType.ShowHide, false);
		}
		TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), 960f, 0.3f, false), UIInteriorFurnitureDetail.TweenAnimationType.ShowHide);
	}

	public void QuitState()
	{
		this.mKeyController = null;
		this.ChangeFocus(this.mFocasableButtons[0]);
		UIButton[] array = this.mFocasableButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIButton uIButton = array[i];
			uIButton.set_enabled(false);
		}
		if (DOTween.IsTweening(UIInteriorFurnitureDetail.TweenAnimationType.Background))
		{
			DOTween.Kill(UIInteriorFurnitureDetail.TweenAnimationType.Background, false);
		}
		Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_TouchBackArea.alpha, 0.0001f, 0.15f, delegate(float alpha)
		{
			this.mTexture_TouchBackArea.alpha = alpha;
		}), UIInteriorFurnitureDetail.TweenAnimationType.Background);
		this.mOnClickEventSender_TouchBackArea.SetClickable(false);
	}

	private void Update()
	{
		if (this.mKeyController != null && this.mFocusButton != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mFocusButton);
				int num2 = num - 1;
				bool flag = 0 <= num2;
				if (flag)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.ChangeFocus(this.mFocasableButtons[num2]);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mFocusButton);
				int num4 = num3 + 1;
				bool flag2 = num4 < this.mFocasableButtons.Length;
				if (flag2)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					this.ChangeFocus(this.mFocasableButtons[num4]);
				}
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				if (this.mFocusButton != null)
				{
					if (this.mFocusButton.Equals(this.mButton_Change))
					{
						this.OnSelectChange();
					}
					else if (this.mFocusButton.Equals(this.mButton_Preview))
					{
						this.OnSelectPreview();
					}
				}
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.OnSelectBack();
			}
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchSelectChange()
	{
		if (this.mKeyController != null)
		{
			this.OnSelectChange();
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchSelectPreview()
	{
		if (this.mKeyController != null)
		{
			this.OnSelectPreview();
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchBack()
	{
		this.OnSelectBack();
	}

	private void OnSelectBack()
	{
		if (this.mOnSelectBackListener != null)
		{
			this.mOnSelectBackListener.Invoke();
		}
	}

	private void OnSelectChange()
	{
		if (this.mOnSelectChangeListener != null)
		{
			this.mOnSelectChangeListener.Invoke();
		}
	}

	private void OnSelectPreview()
	{
		if (this.mOnSelectPreviewListener != null)
		{
			this.mOnSelectPreviewListener.Invoke();
		}
	}

	private void ChangeFocus(UIButton targetButton)
	{
		if (this.mFocusButton != null)
		{
			this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
		}
		this.mFocusButton = targetButton;
		if (this.mFocusButton != null)
		{
			this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
		}
	}

	public void ResumeState()
	{
		UIButton[] array = this.mFocasableButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIButton uIButton = array[i];
			uIButton.isEnabled = true;
		}
		this.ChangeFocus(this.mFocusButton);
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref this.mFocasableButtons);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Thumbnail, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Name);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Description);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Change);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Preview);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_TouchBackArea, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mFocusButton);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Thumbnail, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Thumbnail, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Thumbnail, false);
		this.mOnClickEventSender_TouchBackArea = null;
		this.mEquipMark = null;
		this.mFurnitureModel = null;
		this.mKeyController = null;
		this.mOnSelectBackListener = null;
		this.mOnSelectChangeListener = null;
		this.mOnSelectPreviewListener = null;
	}
}
