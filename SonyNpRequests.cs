using Sony.NP;
using System;

public class SonyNpRequests : IScreen
{
	private MenuLayout menu;

	public SonyNpRequests()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menu;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode)
	{
		if (errorCode != null)
		{
			OnScreenLog.Add("Error: " + errorCode);
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		this.menu.Update();
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	public void Initialize()
	{
		this.menu = new MenuLayout(this, 450, 34);
		Requests.add_OnCheckPlusResult(new Messages.EventHandler(this.OnCheckPlusResult));
		Requests.add_OnAccountLanguageResult(new Messages.EventHandler(this.OnAccountLanguageResult));
		Requests.add_OnParentalControlResult(new Messages.EventHandler(this.OnParentalControlResult));
	}

	private void OnCheckPlusResult(Messages.PluginMessage msg)
	{
		bool flag;
		int num;
		Requests.GetCheckPlusResult(msg, ref flag, ref num);
		OnScreenLog.Add(string.Concat(new object[]
		{
			"OnPlusCheckResult  returned:",
			flag,
			" userId :0x",
			num.ToString("X")
		}));
	}

	private void OnAccountLanguageResult(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("OnAccountLanguageResult  AccountLanguage:" + Requests.GetAccountLanguageResult(msg) + " OnlineID: " + Requests.GetRequestOnlineId(msg));
	}

	private void OnParentalControlResult(Messages.PluginMessage msg)
	{
		int num;
		bool flag;
		bool flag2;
		Requests.GetParentalControlInfoResult(msg, ref num, ref flag, ref flag2);
		OnScreenLog.Add(string.Concat(new object[]
		{
			"OnParentalControlResult  Age:",
			num,
			" chatRestriction:",
			flag,
			" ugcRestriction:",
			flag2,
			" OnlineID: ",
			Requests.GetRequestOnlineId(msg)
		}));
	}
}
