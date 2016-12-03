using Sony.NP;
using System;

public class SonyNpCommerceStore : IScreen
{
	private MenuLayout menu;

	public SonyNpCommerceStore()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menu;
	}

	public void Initialize()
	{
		this.menu = new MenuLayout(this, 450, 34);
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		bool enabled = User.get_IsSignedInPSN() && !Commerce.IsBusy();
		this.menu.Update();
		if (this.menu.AddItem("Browse Category", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.BrowseCategory(string.Empty));
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}
}
