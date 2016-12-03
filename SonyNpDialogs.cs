using Sony.NP;
using System;

public class SonyNpDialogs : IScreen
{
	private MenuLayout menu;

	public SonyNpDialogs()
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
			ResultCode resultCode = default(ResultCode);
			Dialogs.GetLastError(ref resultCode);
			OnScreenLog.Add(string.Concat(new object[]
			{
				"Error: ",
				resultCode.get_className(),
				": ",
				resultCode.lastError,
				", sce error 0x",
				resultCode.lastErrorSCE.ToString("X8")
			}));
		}
		return errorCode;
	}

	public void Process(MenuStack stack)
	{
		this.menu.Update();
		bool enabled = User.get_IsSignedInPSN() && !Dialogs.get_IsDialogOpen();
		if (this.menu.AddItem("Friends Dialog", enabled))
		{
			this.ErrorHandler(Dialogs.FriendsList());
		}
		if (this.menu.AddItem("Shared History Dialog", enabled))
		{
			this.ErrorHandler(Dialogs.SharedPlayHistory());
		}
		if (this.menu.AddItem("Profile Dialog", enabled))
		{
			this.ErrorHandler(Dialogs.Profile(User.GetCachedUserProfile().get_npID()));
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	public void Initialize()
	{
		this.menu = new MenuLayout(this, 450, 34);
		Dialogs.add_OnDlgFriendsListClosed(new Messages.EventHandler(this.OnFriendDialogClosed));
		Dialogs.add_OnDlgSharedPlayHistoryClosed(new Messages.EventHandler(this.OnSharedPlayHistoryDialogClosed));
		Dialogs.add_OnDlgProfileClosed(new Messages.EventHandler(this.OnProfileDialogClosed));
		Dialogs.add_OnDlgCommerceClosed(new Messages.EventHandler(this.OnCommerceDialogClosed));
	}

	private void OnFriendDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Friends Dialog closed with result: " + dialogResult.result);
		if (dialogResult.result == 1)
		{
			Dialogs.Profile(dialogResult.get_npID());
		}
	}

	private void OnSharedPlayHistoryDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add("Shared play history dialog closed with result: " + dialogResult.result);
		if (dialogResult.result == 1)
		{
			Dialogs.Profile(dialogResult.get_npID());
		}
	}

	private void OnProfileDialogClosed(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Profile dialog closed with result: " + Dialogs.GetDialogResult().result);
	}

	private void OnCommerceDialogClosed(Messages.PluginMessage msg)
	{
		Dialogs.NpDialogReturn dialogResult = Dialogs.GetDialogResult();
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Commerce dialog closed with result: ",
			dialogResult.result,
			" PlusAllowed:",
			dialogResult.plusAllowed
		}));
	}
}
