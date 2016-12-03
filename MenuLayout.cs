using System;
using UnityEngine;

public class MenuLayout
{
	private int width;

	private int height;

	private int ySpace;

	private int x;

	private int y;

	private GUIStyle style;

	private GUIStyle styleSelected;

	private int selectedItemIndex;

	private bool buttonPressed;

	private bool backButtonPressed;

	private int numItems;

	private int fontSize = 16;

	private int currCount;

	private IScreen owner;

	public MenuLayout(IScreen screen, int itemWidth, int itemFontSize)
	{
		this.owner = screen;
		this.numItems = 0;
		this.width = itemWidth;
		this.fontSize = itemFontSize;
	}

	public IScreen GetOwner()
	{
		return this.owner;
	}

	public void DoLayout()
	{
		this.numItems = this.currCount;
		this.style = new GUIStyle(GUI.get_skin().GetStyle("Button"));
		this.style.set_fontSize(this.fontSize);
		this.style.set_alignment(4);
		this.styleSelected = new GUIStyle(GUI.get_skin().GetStyle("Button"));
		this.styleSelected.set_fontSize(this.fontSize + 8);
		this.styleSelected.set_alignment(4);
		this.height = this.style.get_fontSize() + 16;
		this.ySpace = 8;
		this.x = (Screen.get_width() - this.width) / 2;
		this.y = (Screen.get_height() - (this.height + this.ySpace) * this.numItems) / 2;
		this.currCount = 0;
	}

	public void SetSelectedItem(int index)
	{
		if (index < 0)
		{
			this.selectedItemIndex = 0;
		}
		if (index > this.numItems - 1)
		{
			this.selectedItemIndex = this.numItems - 1;
		}
	}

	public void ItemNext()
	{
		if (this.numItems > 0)
		{
			this.selectedItemIndex++;
			if (this.selectedItemIndex >= this.numItems)
			{
				this.selectedItemIndex = 0;
			}
		}
	}

	public void ItemPrev()
	{
		if (this.numItems > 0)
		{
			this.selectedItemIndex--;
			if (this.selectedItemIndex < 0)
			{
				this.selectedItemIndex = this.numItems - 1;
			}
		}
	}

	public void Update()
	{
		this.DoLayout();
		this.HandleInput();
	}

	public void HandleInput()
	{
	}

	private bool AddButton(string text, bool enabled = true, bool selected = false)
	{
		GUI.set_enabled(enabled);
		bool result = GUI.Button(this.GetRect(), text, (!selected) ? this.style : this.styleSelected);
		this.y += this.height + this.ySpace;
		return result;
	}

	public bool AddItem(string name, bool enabled = true)
	{
		bool result = false;
		if (this.numItems > 0)
		{
			if (this.AddButton(name, enabled, this.selectedItemIndex == this.currCount))
			{
				this.selectedItemIndex = this.currCount;
				result = true;
			}
			else if (this.buttonPressed && enabled && this.selectedItemIndex == this.currCount)
			{
				result = true;
				this.buttonPressed = false;
			}
		}
		this.currCount++;
		return result;
	}

	public bool AddBackIndex(string name, bool enabled = true)
	{
		bool result = false;
		if (this.numItems > 0)
		{
			if (this.AddButton(name, enabled, this.selectedItemIndex == this.currCount))
			{
				this.selectedItemIndex = this.currCount;
				result = true;
			}
			else if (this.buttonPressed && enabled && this.selectedItemIndex == this.currCount)
			{
				result = true;
				this.buttonPressed = false;
			}
			else if (this.backButtonPressed && enabled)
			{
				result = true;
				this.backButtonPressed = false;
			}
		}
		this.currCount++;
		return result;
	}

	public Rect GetRect()
	{
		return new Rect((float)this.x, (float)this.y, (float)this.width, (float)this.height);
	}
}
