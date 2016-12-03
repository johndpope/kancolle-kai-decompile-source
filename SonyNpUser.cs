using Sony.NP;
using System;
using System.Text;

public class SonyNpUser : IScreen
{
	private MenuLayout menuUser;

	private string remoteOnlineID = "Q-ZLqkCtBK-GB-EN";

	private byte[] remoteNpID;

	private string[] sUserColors = new string[]
	{
		"BLUE",
		"RED",
		"GREEN",
		"PINK"
	};

	public SonyNpUser()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuUser;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		this.MenuUser(stack);
	}

	public void Initialize()
	{
		this.menuUser = new MenuLayout(this, 560, 34);
		User.add_OnGotUserProfile(new Messages.EventHandler(this.OnUserGotProfile));
		User.add_OnGotRemoteUserNpID(new Messages.EventHandler(this.OnGotRemoteUserNpID));
		User.add_OnGotRemoteUserProfile(new Messages.EventHandler(this.OnGotRemoteUserProfile));
		User.add_OnUserProfileError(new Messages.EventHandler(this.OnUserProfileError));
	}

	public void MenuUser(MenuStack menuStack)
	{
		bool isSignedInPSN = User.get_IsSignedInPSN();
		this.menuUser.Update();
		if (this.menuUser.AddItem("Get My Profile", !User.get_IsUserProfileBusy()))
		{
			ErrorCode errorCode = User.RequestUserProfile();
			if (errorCode != null)
			{
				ResultCode resultCode = default(ResultCode);
				User.GetLastUserProfileError(ref resultCode);
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
		if (this.menuUser.AddItem("Get Remote Profile (onlineID)", isSignedInPSN && !User.get_IsUserProfileBusy()))
		{
			ErrorCode errorCode2 = User.RequestRemoteUserProfileForOnlineID(this.remoteOnlineID);
			if (errorCode2 != null)
			{
				ResultCode resultCode2 = default(ResultCode);
				User.GetLastUserProfileError(ref resultCode2);
				OnScreenLog.Add(string.Concat(new object[]
				{
					resultCode2.get_className(),
					": ",
					resultCode2.lastError,
					", sce error 0x",
					resultCode2.lastErrorSCE.ToString("X8")
				}));
			}
		}
		if (this.menuUser.AddItem("Get Remote NpID", isSignedInPSN && !User.get_IsUserProfileBusy()))
		{
			ErrorCode errorCode3 = User.RequestRemoteUserNpID(this.remoteOnlineID);
			if (errorCode3 != null)
			{
				ResultCode resultCode3 = default(ResultCode);
				User.GetLastUserProfileError(ref resultCode3);
				OnScreenLog.Add(string.Concat(new object[]
				{
					resultCode3.get_className(),
					": ",
					resultCode3.lastError,
					", sce error 0x",
					resultCode3.lastErrorSCE.ToString("X8")
				}));
			}
		}
		if (this.menuUser.AddItem("Get Remote Profile (npID)", this.remoteNpID != null && isSignedInPSN && !User.get_IsUserProfileBusy()))
		{
			ErrorCode errorCode4 = User.RequestRemoteUserProfileForNpID(this.remoteNpID);
			if (errorCode4 != null)
			{
				ResultCode resultCode4 = default(ResultCode);
				User.GetLastUserProfileError(ref resultCode4);
				OnScreenLog.Add(string.Concat(new object[]
				{
					resultCode4.get_className(),
					": ",
					resultCode4.lastError,
					", sce error 0x",
					resultCode4.lastErrorSCE.ToString("X8")
				}));
			}
		}
		if (this.menuUser.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnUserGotProfile(Messages.PluginMessage msg)
	{
		User.UserProfile cachedUserProfile = User.GetCachedUserProfile();
		OnScreenLog.Add(msg.ToString());
		OnScreenLog.Add(" OnlineID: " + cachedUserProfile.get_onlineID());
		string @string = Encoding.get_Default().GetString(cachedUserProfile.get_npID());
		OnScreenLog.Add(" NpID: " + @string);
		OnScreenLog.Add(" Avatar URL: " + cachedUserProfile.get_avatarURL());
		OnScreenLog.Add(" Country Code: " + cachedUserProfile.get_countryCode());
		OnScreenLog.Add(" Language: " + cachedUserProfile.language);
		OnScreenLog.Add(" Age: " + cachedUserProfile.age);
		OnScreenLog.Add(" Chat Restrict: " + cachedUserProfile.chatRestricted);
		OnScreenLog.Add(" Content Restrict: " + cachedUserProfile.contentRestricted);
		SonyNpMain.SetAvatarURL(cachedUserProfile.get_avatarURL(), 0);
	}

	private void OnGotRemoteUserNpID(Messages.PluginMessage msg)
	{
		this.remoteNpID = User.GetCachedRemoteUserNpID();
		string @string = Encoding.get_Default().GetString(this.remoteNpID);
		OnScreenLog.Add("Got Remote User NpID: " + @string);
	}

	private void OnGotRemoteUserProfile(Messages.PluginMessage msg)
	{
		User.RemoteUserProfile cachedRemoteUserProfile = User.GetCachedRemoteUserProfile();
		OnScreenLog.Add("Got Remote User Profile");
		OnScreenLog.Add(" OnlineID: " + cachedRemoteUserProfile.get_onlineID());
		string @string = Encoding.get_Default().GetString(cachedRemoteUserProfile.get_npID());
		OnScreenLog.Add(" NpID: " + @string);
		OnScreenLog.Add(" Avatar URL: " + cachedRemoteUserProfile.get_avatarURL());
		OnScreenLog.Add(" Country Code: " + cachedRemoteUserProfile.get_countryCode());
		OnScreenLog.Add(" Language: " + cachedRemoteUserProfile.language);
		SonyNpMain.SetAvatarURL(cachedRemoteUserProfile.get_avatarURL(), 1);
	}

	private void OnUserProfileError(Messages.PluginMessage msg)
	{
		ResultCode resultCode = default(ResultCode);
		User.GetLastUserProfileError(ref resultCode);
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
