using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInteriorChangeFurnitureCategorySelector : MonoBehaviour
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

	private void Start()
	{
		List<UIInteriorMenuButton> list = new List<UIInteriorMenuButton>();
		list.Add(this.mUIInteriorMenuButton_Hangings);
		list.Add(this.mUIInteriorMenuButton_Window);
		list.Add(this.mUIInteriorMenuButton_Wall);
		list.Add(this.mUIInteriorMenuButton_Chest);
		list.Add(this.mUIInteriorMenuButton_Floor);
		list.Add(this.mUIInteriorMenuButton_Desk);
		this.mFocasableUIInteriorMenuButtons = list.ToArray();
		this.ChangeFocus(this.mFocasableUIInteriorMenuButtons[0]);
	}

	private void ChangeFocus(UIInteriorMenuButton uiInteriorMenuButton)
	{
		if (this.mFocusUIInteriorMenuButton != null)
		{
			this.mFocusUIInteriorMenuButton.RemoveFocus();
		}
		this.mFocusUIInteriorMenuButton = uiInteriorMenuButton;
		if (this.mFocusUIInteriorMenuButton != null)
		{
			this.mFocusUIInteriorMenuButton.Focus();
		}
	}

	private void Update()
	{
	}
}
