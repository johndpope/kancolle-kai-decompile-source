using Sony.NP;
using System;
using System.Text;
using UnityEngine;

public class SonyNpFriends : IScreen
{
	private MenuLayout menuFriends;

	public SonyNpFriends()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuFriends;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	private ErrorCode ErrorHandlerFriends(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			Friends.GetLastError(ref resultCode);
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

	private ErrorCode ErrorHandlerPresence(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			User.GetLastPresenceError(ref resultCode);
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

	private ErrorCode ErrorHandlerTwitter(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			Twitter.GetLastError(ref resultCode);
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

	private ErrorCode ErrorHandlerFacebook(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			Facebook.GetLastError(ref resultCode);
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
		this.MenuFriends(stack);
	}

	public void Initialize()
	{
		this.menuFriends = new MenuLayout(this, 450, 34);
		Friends.add_OnFriendsListUpdated(new Messages.EventHandler(this.OnFriendsListUpdated));
		Friends.add_OnFriendsPresenceUpdated(new Messages.EventHandler(this.OnFriendsListUpdated));
		Friends.add_OnGotFriendsList(new Messages.EventHandler(this.OnFriendsGotList));
		Friends.add_OnFriendsListError(new Messages.EventHandler(this.OnFriendsListError));
		User.add_OnPresenceSet(new Messages.EventHandler(this.OnSomeEvent));
		User.add_OnPresenceError(new Messages.EventHandler(this.OnPresenceError));
		Facebook.add_OnFacebookDialogStarted(new Messages.EventHandler(this.OnSomeEvent));
		Facebook.add_OnFacebookDialogFinished(new Messages.EventHandler(this.OnSomeEvent));
		Facebook.add_OnFacebookMessagePosted(new Messages.EventHandler(this.OnSomeEvent));
		Facebook.add_OnFacebookMessagePostFailed(new Messages.EventHandler(this.OnFacebookMessagePostFailed));
		Twitter.add_OnTwitterDialogStarted(new Messages.EventHandler(this.OnSomeEvent));
		Twitter.add_OnTwitterDialogCanceled(new Messages.EventHandler(this.OnSomeEvent));
		Twitter.add_OnTwitterDialogFinished(new Messages.EventHandler(this.OnSomeEvent));
		Twitter.add_OnTwitterMessagePosted(new Messages.EventHandler(this.OnSomeEvent));
		Twitter.add_OnTwitterMessagePostFailed(new Messages.EventHandler(this.OnTwitterMessagePostFailed));
	}

	public void MenuFriends(MenuStack menuStack)
	{
		this.menuFriends.Update();
		if (this.menuFriends.AddItem("Friends", User.get_IsSignedInPSN() && !Friends.FriendsListIsBusy()))
		{
			this.ErrorHandlerFriends(Friends.RequestFriendsList());
		}
		if (this.menuFriends.AddItem("Set Presence", User.get_IsSignedInPSN() && !User.OnlinePresenceIsBusy()))
		{
			this.ErrorHandlerPresence(User.SetOnlinePresence("Testing UnityNpToolkit"));
		}
		if (this.menuFriends.AddItem("Clear Presence", User.get_IsSignedInPSN() && !User.OnlinePresenceIsBusy()))
		{
			this.ErrorHandlerPresence(User.SetOnlinePresence(string.Empty));
		}
		if (this.menuFriends.AddItem("Post On Facebook", User.get_IsSignedInPSN() && !Facebook.IsBusy()))
		{
			this.ErrorHandlerFacebook(Facebook.PostMessage(new Facebook.PostFacebook
			{
				appID = 701792156521339L,
				userText = "I'm testing Unity's facebook integration !",
				photoURL = "http://uk.playstation.com/media/RZXT_744/159/PlayStationNetworkFeaturedImage.jpg",
				photoTitle = "Title",
				photoCaption = "This is the caption",
				photoDescription = "This is the description",
				actionLinkName = "Go To Unity3d.com",
				actionLinkURL = "http://unity3d.com/"
			}));
		}
		if (this.menuFriends.AddItem("Post On Twitter", User.get_IsSignedInPSN() && !Twitter.IsBusy()))
		{
			this.ErrorHandlerTwitter(Twitter.PostMessage(new Twitter.PostTwitter
			{
				userText = "I'm testing Unity's Twitter integration !",
				imagePath = Application.get_streamingAssetsPath() + "/TweetUnity.png",
				forbidAttachPhoto = false,
				disableEditTweetMsg = true,
				forbidOnlyImageTweet = false,
				forbidNoImageTweet = false,
				disableChangeImage = false,
				limitToScreenShot = true
			}));
		}
		if (this.menuFriends.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnFacebookMessagePostFailed(Messages.PluginMessage msg)
	{
		this.ErrorHandlerFacebook(3);
	}

	private void OnTwitterMessagePostFailed(Messages.PluginMessage msg)
	{
		this.ErrorHandlerTwitter(3);
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnFriendsListUpdated(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Requesting Friends List for Event: " + msg.type);
		this.ErrorHandlerFriends(Friends.RequestFriendsList());
	}

	private string OnlinePresenceType(Friends.EnumNpPresenceType type)
	{
		switch (type + 1)
		{
		case 0:
			return "unknown";
		case 1:
			return "none";
		case 2:
			return "default";
		case 3:
			return "joining";
		case 4:
			return "joining party";
		case 5:
			return "join ack";
		default:
			return "unknown";
		}
	}

	private string OnlineStatus(Friends.EnumNpOnlineStatus status)
	{
		switch (status)
		{
		case 1:
			return "offline";
		case 2:
			return "afk";
		case 3:
			return "online";
		default:
			return "unknown";
		}
	}

	private void OnFriendsGotList(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Friends List!");
		Friends.Friend[] cachedFriendsList = Friends.GetCachedFriendsList();
		Friends.Friend[] array = cachedFriendsList;
		for (int i = 0; i < array.Length; i++)
		{
			Friends.Friend friend = array[i];
			string @string = Encoding.get_Default().GetString(friend.get_npID());
			OnScreenLog.Add(string.Concat(new string[]
			{
				friend.get_npOnlineID(),
				", np(",
				@string,
				"), os(",
				this.OnlineStatus(friend.npOnlineStatus),
				"), pt(",
				this.OnlinePresenceType(friend.npPresenceType),
				"), prsc(",
				friend.get_npPresenceTitle(),
				", ",
				friend.get_npPresenceStatus(),
				"),",
				friend.get_npComment()
			}));
		}
	}

	private void OnFriendsListError(Messages.PluginMessage msg)
	{
		this.ErrorHandlerFriends(3);
	}

	private void OnPresenceError(Messages.PluginMessage msg)
	{
		this.ErrorHandlerPresence(3);
	}
}
