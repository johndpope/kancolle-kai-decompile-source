using System;

public class SonyNpCloud : IScreen
{
	private MenuLayout menuCloud;

	private SonyNpCloudTUS tus;

	private SonyNpCloudTSS tss;

	public SonyNpCloud()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuCloud;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		this.MenuCloud(stack);
	}

	public void Initialize()
	{
		this.menuCloud = new MenuLayout(this, 550, 34);
		this.tus = new SonyNpCloudTUS();
		this.tss = new SonyNpCloudTSS();
	}

	public void MenuCloud(MenuStack stack)
	{
		this.menuCloud.Update();
		if (this.menuCloud.AddItem("Title User Storage", true))
		{
			stack.PushMenu(this.tus.GetMenu());
		}
		if (this.menuCloud.AddItem("Title Small Storage", true))
		{
			stack.PushMenu(this.tss.GetMenu());
		}
		if (this.menuCloud.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}
}
