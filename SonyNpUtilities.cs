using Sony.NP;
using System;

public class SonyNpUtilities : IScreen
{
	private MenuLayout menu;

	private SonyNpTicketing ticketing;

	private SonyNpDialogs dialogs;

	public SonyNpUtilities()
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

	private ErrorCode ErrorHandlerSystem(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			System.GetLastError(ref resultCode);
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
		this.MenuUtilities(stack);
	}

	public void Initialize()
	{
		this.menu = new MenuLayout(this, 450, 34);
		this.ticketing = new SonyNpTicketing();
		this.dialogs = new SonyNpDialogs();
		System.add_OnGotBandwidth(new Messages.EventHandler(this.OnSystemGotBandwidth));
		System.add_OnGotNetInfo(new Messages.EventHandler(this.OnSystemGotNetInfo));
		System.add_OnNetInfoError(new Messages.EventHandler(this.OnNetInfoError));
		WordFilter.add_OnCommentCensored(new Messages.EventHandler(this.OnWordFilterCensored));
		WordFilter.add_OnCommentNotCensored(new Messages.EventHandler(this.OnWordFilterNotCensored));
		WordFilter.add_OnCommentSanitized(new Messages.EventHandler(this.OnWordFilterSanitized));
		WordFilter.add_OnWordFilterError(new Messages.EventHandler(this.OnWordFilterError));
	}

	public void MenuUtilities(MenuStack menuStack)
	{
		this.menu.Update();
		if (this.menu.AddItem("Get Network Time", System.get_IsConnected()))
		{
			DateTime dateTime = new DateTime(System.GetNetworkTime(), 1);
			OnScreenLog.Add("networkTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
		if (this.menu.AddItem("Bandwidth", System.get_IsConnected() && !System.RequestBandwidthInfoIsBusy()))
		{
			this.ErrorHandlerSystem(System.RequestBandwidthInfo());
		}
		if (this.menu.AddItem("Net Info", !System.RequestBandwidthInfoIsBusy()))
		{
			this.ErrorHandlerSystem(System.RequestNetInfo());
		}
		if (this.menu.AddItem("Net Device Type", true))
		{
			System.NetDeviceType networkDeviceType = System.GetNetworkDeviceType();
			OnScreenLog.Add("Network device: " + networkDeviceType);
		}
		if (User.get_IsSignedInPSN())
		{
			if (this.menu.AddItem("Dialogs", true))
			{
				menuStack.PushMenu(this.dialogs.GetMenu());
			}
			if (this.menu.AddItem("Auth Ticketing", true))
			{
				menuStack.PushMenu(this.ticketing.GetMenu());
			}
			if (this.menu.AddItem("Censor Bad Comment", System.get_IsConnected() && !WordFilter.IsBusy()))
			{
				WordFilter.CensorComment("Censor a shit comment");
			}
			if (this.menu.AddItem("Sanitize Bad Comment", System.get_IsConnected() && !WordFilter.IsBusy()))
			{
				WordFilter.SanitizeComment("Sanitize a shit comment");
			}
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSystemGotBandwidth(Messages.PluginMessage msg)
	{
		System.Bandwidth bandwidthInfo = System.GetBandwidthInfo();
		OnScreenLog.Add("bandwidth download : " + bandwidthInfo.downloadBPS / 8192f + " KBs");
		OnScreenLog.Add("bandwidth upload : " + bandwidthInfo.uploadBPS / 8192f + " KBs");
	}

	private void OnSystemGotNetInfo(Messages.PluginMessage msg)
	{
		System.NetInfoBasic netInfo = System.GetNetInfo();
		OnScreenLog.Add("Got Net info");
		OnScreenLog.Add(" Connection status: " + netInfo.connectionStatus);
		OnScreenLog.Add(" IP address: " + netInfo.get_ipAddress());
		OnScreenLog.Add(" NAT type: " + netInfo.natType);
		OnScreenLog.Add(" NAT stun status: " + netInfo.natStunStatus);
		OnScreenLog.Add(" NAT mapped addr: 0x" + netInfo.natMappedAddr.ToString("X8"));
	}

	private void OnNetInfoError(Messages.PluginMessage msg)
	{
		this.ErrorHandlerSystem(3);
	}

	private void OnWordFilterCensored(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Censored: changed=",
			result.wasChanged,
			", comment='",
			result.get_comment(),
			"'"
		}));
	}

	private void OnWordFilterNotCensored(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Not censored: changed=",
			result.wasChanged,
			", comment='",
			result.get_comment(),
			"'"
		}));
	}

	private void OnWordFilterSanitized(Messages.PluginMessage msg)
	{
		WordFilter.FilteredComment result = WordFilter.GetResult();
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Sanitized: changed=",
			result.wasChanged,
			", comment='",
			result.get_comment(),
			"'"
		}));
	}

	private void OnWordFilterError(Messages.PluginMessage msg)
	{
		ResultCode resultCode = default(ResultCode);
		WordFilter.GetLastError(ref resultCode);
		OnScreenLog.Add(string.Concat(new object[]
		{
			resultCode.get_className(),
			": ",
			resultCode.lastError,
			", sce error 0x",
			resultCode.lastErrorSCE.ToString("X8")
		}));
	}
}
