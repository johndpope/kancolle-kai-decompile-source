using Sony.NP;
using System;

public class SonyNpCloudTSS : IScreen
{
	private MenuLayout menuTss;

	public SonyNpCloudTSS()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuTss;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			TusTss.GetLastError(ref resultCode);
			if (resultCode.lastError != null)
			{
				OnScreenLog.Add(string.Concat(new object[]
				{
					"Error: ",
					resultCode.get_className(),
					": ",
					resultCode.lastError,
					", sce error 0x",
					resultCode.lastErrorSCE.ToString("X8")
				}));
				return resultCode.lastError;
			}
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		this.MenuTss(stack);
	}

	public void Initialize()
	{
		this.menuTss = new MenuLayout(this, 550, 34);
		TusTss.add_OnTssDataRecieved(new Messages.EventHandler(this.OnGotTssData));
		TusTss.add_OnTssNoData(new Messages.EventHandler(this.OnSomeEvent));
		TusTss.add_OnTusTssError(new Messages.EventHandler(this.OnTusTssError));
	}

	public void MenuTss(MenuStack menuStack)
	{
		this.menuTss.Update();
		bool enabled = User.get_IsSignedInPSN() && !TusTss.IsTssBusy();
		if (this.menuTss.AddItem("TSS Request Data", enabled))
		{
			this.ErrorHandler(TusTss.RequestTssData());
		}
		if (this.menuTss.AddItem("TSS Request Data from slot", enabled))
		{
			int num = 1;
			this.ErrorHandler(TusTss.RequestTssDataFromSlot(num));
		}
		if (this.menuTss.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnTusTssError(Messages.PluginMessage msg)
	{
		this.ErrorHandler(3);
	}

	private void OnGotTssData(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got TSS Data");
		byte[] tssData = TusTss.GetTssData();
		OnScreenLog.Add(" Data size: " + tssData.Length);
		string text = string.Empty;
		int num = 0;
		while (num < 16 && num < tssData.Length)
		{
			text = text + tssData[num].ToString() + ", ";
			num++;
		}
		OnScreenLog.Add(" Data: " + text);
	}
}
