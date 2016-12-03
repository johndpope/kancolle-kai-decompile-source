using System;
using System.Collections.Generic;

public class MenuStack
{
	private MenuLayout activeMenu;

	private List<MenuLayout> menuStack = new List<MenuLayout>();

	public void SetMenu(MenuLayout menu)
	{
		if (this.activeMenu != null)
		{
			this.activeMenu.GetOwner().OnExit();
		}
		this.menuStack.Clear();
		this.activeMenu = menu;
		this.activeMenu.GetOwner().OnEnter();
	}

	public MenuLayout GetMenu()
	{
		return this.activeMenu;
	}

	public void PushMenu(MenuLayout menu)
	{
		if (this.activeMenu != null)
		{
			this.activeMenu.GetOwner().OnExit();
		}
		this.menuStack.Add(this.activeMenu);
		this.activeMenu = menu;
		this.activeMenu.GetOwner().OnEnter();
	}

	public void PopMenu()
	{
		if (this.menuStack.get_Count() > 0)
		{
			this.activeMenu.GetOwner().OnExit();
			this.activeMenu = this.menuStack.get_Item(this.menuStack.get_Count() - 1);
			this.menuStack.RemoveAt(this.menuStack.get_Count() - 1);
		}
	}
}
