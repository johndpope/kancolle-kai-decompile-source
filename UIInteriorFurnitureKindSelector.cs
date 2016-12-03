using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIInteriorFurnitureKindSelector : MonoBehaviour
{
	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Hangings;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Window;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Wall;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Chest;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Floor;

	[SerializeField]
	private UIInteriorMenuButton mUIInteriorMenuButton_Desk;

	private UIInteriorMenuButton[] mFocasableUIInteriorMenuButtons;

	private UIInteriorMenuButton mFocusUIInteriorMenuButton;

	private UIButtonManager mButtonManager;

	private KeyControl mKeyController;

	private Action<FurnitureKinds> mOnSelectFurnitureKind;

	private Action mOnSelectCancelListener;

	private void Awake()
	{
		this.mButtonManager = base.GetComponent<UIButtonManager>();
	}

	public void Initialize()
	{
		DOTween.Kill(this, false);
		List<UIInteriorMenuButton> list = new List<UIInteriorMenuButton>();
		this.mUIInteriorMenuButton_Hangings.Initialize(FurnitureKinds.Hangings);
		this.mUIInteriorMenuButton_Window.Initialize(FurnitureKinds.Window);
		this.mUIInteriorMenuButton_Wall.Initialize(FurnitureKinds.Wall);
		this.mUIInteriorMenuButton_Chest.Initialize(FurnitureKinds.Chest);
		this.mUIInteriorMenuButton_Floor.Initialize(FurnitureKinds.Floor);
		this.mUIInteriorMenuButton_Desk.Initialize(FurnitureKinds.Desk);
		list.Add(this.mUIInteriorMenuButton_Hangings);
		list.Add(this.mUIInteriorMenuButton_Window);
		list.Add(this.mUIInteriorMenuButton_Wall);
		list.Add(this.mUIInteriorMenuButton_Desk);
		list.Add(this.mUIInteriorMenuButton_Floor);
		list.Add(this.mUIInteriorMenuButton_Chest);
		this.mFocasableUIInteriorMenuButtons = list.ToArray();
		UIInteriorMenuButton[] array = this.mFocasableUIInteriorMenuButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIInteriorMenuButton uIInteriorMenuButton = array[i];
			uIInteriorMenuButton.SetOnClickListener(new Action(this.OnClickMenuListener));
		}
		this.mButtonManager.IndexChangeAct = delegate
		{
			UIInteriorMenuButton uiInteriorMenuButton = this.mFocasableUIInteriorMenuButtons[this.mButtonManager.nowForcusIndex];
			this.ChangeFocus(uiInteriorMenuButton, false);
		};
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.mFocusUIInteriorMenuButton.Click();
			}
			else if (this.mKeyController.keyState.get_Item(0).down)
			{
				this.OnSelectCancel();
			}
			else if (this.mKeyController.keyState.get_Item(14).down)
			{
				int num = Array.IndexOf<UIInteriorMenuButton>(this.mFocasableUIInteriorMenuButtons, this.mFocusUIInteriorMenuButton);
				int num2 = num - 1;
				if (0 <= num2)
				{
					this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[num2], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				int num3 = Array.IndexOf<UIInteriorMenuButton>(this.mFocasableUIInteriorMenuButtons, this.mFocusUIInteriorMenuButton);
				int num4 = num3 + 1;
				if (num4 < this.mFocasableUIInteriorMenuButtons.Length)
				{
					this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[num4], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(8).down)
			{
				int num5 = Array.IndexOf<UIInteriorMenuButton>(this.mFocasableUIInteriorMenuButtons, this.mFocusUIInteriorMenuButton);
				int num6 = num5 - 3;
				if (0 <= num6)
				{
					this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[num6], true);
				}
			}
			else if (this.mKeyController.keyState.get_Item(12).down)
			{
				int num7 = Array.IndexOf<UIInteriorMenuButton>(this.mFocasableUIInteriorMenuButtons, this.mFocusUIInteriorMenuButton);
				int num8 = num7 + 3;
				if (num8 < this.mFocasableUIInteriorMenuButtons.Length)
				{
					this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[num8], true);
				}
			}
		}
	}

	public void StartState()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, false);
		}
		Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
		UIInteriorMenuButton[] array = this.mFocasableUIInteriorMenuButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIInteriorMenuButton uIInteriorMenuButton = array[i];
			uIInteriorMenuButton.get_transform().set_localScale(new Vector3(0f, 0f));
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(uIInteriorMenuButton.get_transform(), Vector3.get_one(), 0.25f), 21), this);
			TweenSettingsExtensions.SetDelay<Tween>(tween, 0.075f);
			TweenSettingsExtensions.Join(sequence, tween);
		}
		UIInteriorMenuButton[] array2 = this.mFocasableUIInteriorMenuButtons;
		for (int j = 0; j < array2.Length; j++)
		{
			UIInteriorMenuButton uIInteriorMenuButton2 = array2[j];
			uIInteriorMenuButton2.SetEnableButton(true);
		}
		this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[0], false);
	}

	public void ResumeState()
	{
		UIInteriorMenuButton[] array = this.mFocasableUIInteriorMenuButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIInteriorMenuButton uIInteriorMenuButton = array[i];
			uIInteriorMenuButton.SetEnableButton(true);
		}
		this.ChangeFocus(this.mFocusUIInteriorMenuButton, false);
	}

	private void OnClickMenuListener()
	{
		this.mKeyController.ClearKeyAll();
		this.mKeyController.firstUpdate = true;
		UIInteriorMenuButton[] array = this.mFocasableUIInteriorMenuButtons;
		for (int i = 0; i < array.Length; i++)
		{
			UIInteriorMenuButton uIInteriorMenuButton = array[i];
			uIInteriorMenuButton.SetEnableButton(false);
		}
		SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		this.OnSelectFurnitureKind(this.mFocusUIInteriorMenuButton.mFurnitureKind);
	}

	private void ChangeFocus(UIInteriorMenuButton uiInteriorMenuButton, bool needSe)
	{
		if (this.mFocusUIInteriorMenuButton != null)
		{
			this.mFocusUIInteriorMenuButton.RemoveFocus();
		}
		this.mFocusUIInteriorMenuButton = uiInteriorMenuButton;
		if (this.mFocusUIInteriorMenuButton != null)
		{
			if (needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this.mFocusUIInteriorMenuButton.Focus();
		}
	}

	private void OnSelectFurnitureKind(FurnitureKinds furnitureKind)
	{
		if (this.mOnSelectFurnitureKind != null)
		{
			this.mOnSelectFurnitureKind.Invoke(furnitureKind);
		}
	}

	public void SetOnSelectFurnitureKindListener(Action<FurnitureKinds> onSelectFurnitureKind)
	{
		this.mOnSelectFurnitureKind = onSelectFurnitureKind;
	}

	public void SetOnSelectCancelListener(Action onSelectCancelListener)
	{
		this.mOnSelectCancelListener = onSelectCancelListener;
	}

	private void OnSelectCancel()
	{
		if (this.mOnSelectCancelListener != null)
		{
			this.mOnSelectCancelListener.Invoke();
		}
	}

	private void OnDestroy()
	{
		DOTween.Kill(this, false);
		for (int i = 0; i < this.mFocasableUIInteriorMenuButtons.Length; i++)
		{
			this.mFocasableUIInteriorMenuButtons[i] = null;
		}
		this.mUIInteriorMenuButton_Hangings = null;
		this.mUIInteriorMenuButton_Window = null;
		this.mUIInteriorMenuButton_Wall = null;
		this.mUIInteriorMenuButton_Chest = null;
		this.mUIInteriorMenuButton_Floor = null;
		this.mUIInteriorMenuButton_Desk = null;
		this.mFocasableUIInteriorMenuButtons = null;
		this.mFocusUIInteriorMenuButton = null;
		this.mButtonManager = null;
	}
}
