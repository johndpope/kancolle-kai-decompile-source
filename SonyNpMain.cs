using Sony.NP;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpMain : MonoBehaviour, IScreen
{
	private struct Avatar
	{
		public string url;

		public bool pendingDownload;

		public Texture2D texture;

		public GameObject gameObject;

		public Avatar(GameObject gameObject)
		{
			this.gameObject = gameObject;
			this.url = string.Empty;
			this.pendingDownload = false;
			this.texture = null;
		}
	}

	public struct SharedSessionData
	{
		public int id;

		public string text;

		public int item1;

		public int item2;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.id);
			binaryWriter.Write(this.text);
			binaryWriter.Write(this.item1);
			binaryWriter.Write(this.item2);
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public void ReadFromBuffer(byte[] buffer)
		{
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			this.id = binaryReader.ReadInt32();
			this.text = binaryReader.ReadString();
			this.item1 = binaryReader.ReadInt32();
			this.item2 = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private MenuStack menuStack;

	private MenuLayout menuMain;

	private bool npReady;

	private SonyNpUser user;

	private SonyNpFriends friends;

	private SonyNpTrophy trophies;

	private SonyNpRanking ranking;

	private SonyNpSession sessions;

	private int sendCount;

	private float sendingInterval = 1f;

	private SonyNpMessaging messaging;

	private SonyNpCloud cloudStorage;

	private SonyNpUtilities utilities;

	private SonyNpCommerce commerce;

	private SonyNpRequests requests;

	private static SonyNpMain.Avatar[] avatars = new SonyNpMain.Avatar[2];

	public static Texture2D avatarTexture = null;

	private void Start()
	{
		SonyNpMain.avatars[0] = new SonyNpMain.Avatar(GameObject.Find("UserAvatar"));
		SonyNpMain.avatars[1] = new SonyNpMain.Avatar(GameObject.Find("RemoteUserAvatar"));
		this.menuMain = new MenuLayout(this, 500, 34);
		this.menuStack = new MenuStack();
		this.menuStack.SetMenu(this.menuMain);
		Main.add_OnNPInitialized(new Messages.EventHandler(this.OnNPInitialized));
		OnScreenLog.Add("Initializing NP");
		Main.set_enableInternalLogging(true);
		Main.add_OnLog(new Messages.EventHandler(this.OnLog));
		Main.add_OnLogWarning(new Messages.EventHandler(this.OnLogWarning));
		Main.add_OnLogError(new Messages.EventHandler(this.OnLogError));
		int kNpToolkitCreate_CacheTrophyIcons = Main.kNpToolkitCreate_CacheTrophyIcons;
		Main.Initialize(kNpToolkitCreate_CacheTrophyIcons);
		string sessionImage = Application.get_streamingAssetsPath() + "/PSP2SessionImage.jpg";
		Main.SetSessionImage(sessionImage);
		System.add_OnConnectionUp(new Messages.EventHandler(this.OnSomeEvent));
		System.add_OnConnectionDown(new Messages.EventHandler(this.OnConnectionDown));
		System.add_OnSysResume(new Messages.EventHandler(this.OnSomeEvent));
		System.add_OnSysNpMessageArrived(new Messages.EventHandler(this.OnSomeEvent));
		System.add_OnSysStorePurchase(new Messages.EventHandler(this.OnSomeEvent));
		System.add_OnSysStoreRedemption(new Messages.EventHandler(this.OnSomeEvent));
		System.add_OnSysEvent(new Messages.EventHandler(this.OnSomeEvent));
		Messaging.add_OnSessionInviteMessageRetrieved(new Messages.EventHandler(this.OnMessagingSessionInviteRetrieved));
		Messaging.add_OnMessageSessionInviteReceived(new Messages.EventHandler(this.OnMessagingSessionInviteReceived));
		Messaging.add_OnMessageSessionInviteAccepted(new Messages.EventHandler(this.OnMessagingSessionInviteAccepted));
		User.add_OnSignedIn(new Messages.EventHandler(this.OnSignedIn));
		User.add_OnSignedOut(new Messages.EventHandler(this.OnSomeEvent));
		User.add_OnSignInError(new Messages.EventHandler(this.OnSignInError));
		this.user = new SonyNpUser();
		this.friends = new SonyNpFriends();
		this.trophies = new SonyNpTrophy();
		this.ranking = new SonyNpRanking();
		this.sessions = new SonyNpSession();
		this.messaging = new SonyNpMessaging();
		this.commerce = new SonyNpCommerce();
		this.cloudStorage = new SonyNpCloud();
		this.utilities = new SonyNpUtilities();
		Utility.SkuFlags skuFlags = Utility.get_skuFlags();
		if (skuFlags == 1)
		{
			OnScreenLog.Add("Trial Mode, purchase the full app to get extra features.");
		}
	}

	public static void SetAvatarURL(string url, int index)
	{
		SonyNpMain.avatars[index].url = url;
		SonyNpMain.avatars[index].pendingDownload = true;
	}

	[DebuggerHidden]
	private IEnumerator DownloadAvatar(int index)
	{
		SonyNpMain.<DownloadAvatar>c__Iterator1C9 <DownloadAvatar>c__Iterator1C = new SonyNpMain.<DownloadAvatar>c__Iterator1C9();
		<DownloadAvatar>c__Iterator1C.index = index;
		<DownloadAvatar>c__Iterator1C.<$>index = index;
		return <DownloadAvatar>c__Iterator1C;
	}

	private void Update()
	{
		Main.Update();
		if (this.sessions.sendingData)
		{
			this.sendingInterval -= Time.get_deltaTime();
			if (this.sendingInterval <= 0f)
			{
				this.SendSessionData();
				this.sendingInterval = 1f;
			}
		}
		for (int i = 0; i < SonyNpMain.avatars.Length; i++)
		{
			if (SonyNpMain.avatars[i].pendingDownload)
			{
				SonyNpMain.avatars[i].pendingDownload = false;
				base.StartCoroutine(this.DownloadAvatar(i));
			}
		}
	}

	private void OnNPInitialized(Messages.PluginMessage msg)
	{
		this.npReady = true;
	}

	private void MenuMain()
	{
		this.menuMain.Update();
		bool isSignedInPSN = User.get_IsSignedInPSN();
		if (this.npReady)
		{
			if (!isSignedInPSN && this.menuMain.AddItem("Sign In To PSN", this.npReady))
			{
				OnScreenLog.Add("Begin sign in");
				User.SignIn();
			}
			if (this.menuMain.AddItem("Trophies", true))
			{
				this.menuStack.PushMenu(this.trophies.GetMenu());
			}
			if (this.menuMain.AddItem("User", true))
			{
				this.menuStack.PushMenu(this.user.GetMenu());
			}
			if (this.menuMain.AddItem("Utilities, Dialogs & Auth", true))
			{
				this.menuStack.PushMenu(this.utilities.GetMenu());
			}
			if (isSignedInPSN)
			{
				if (this.menuMain.AddItem("Friends & SNS", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.friends.GetMenu());
				}
				if (this.menuMain.AddItem("Ranking", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.ranking.GetMenu());
				}
				if (this.menuMain.AddItem("Matching", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.sessions.GetMenu());
				}
				if (this.menuMain.AddItem("Messaging", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.messaging.GetMenu());
				}
				if (this.menuMain.AddItem("Cloud Storage (TUS/TSS)", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.cloudStorage.GetMenu());
				}
				if (this.menuMain.AddItem("Commerce", isSignedInPSN))
				{
					this.menuStack.PushMenu(this.commerce.GetMenu());
				}
			}
		}
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		this.MenuMain();
	}

	private void OnGUI()
	{
		MenuLayout menu = this.menuStack.GetMenu();
		menu.GetOwner().Process(this.menuStack);
	}

	private void OnLog(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.get_Text());
	}

	private void OnLogWarning(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("WARNING: " + msg.get_Text());
	}

	private void OnLogError(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("ERROR?: " + msg.get_Text());
	}

	private void OnSignedIn(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.ToString());
		ResultCode resultCode = default(ResultCode);
		User.GetLastSignInError(ref resultCode);
		if (resultCode.lastError == 1)
		{
			OnScreenLog.Add("INFO: Signed in but flight mode is on");
		}
		else if (resultCode.lastError != null)
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
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(msg.ToString());
	}

	private void OnConnectionDown(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Connection Down");
		ResultCode resultCode = default(ResultCode);
		System.GetLastConnectionError(ref resultCode);
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Reason: ",
			resultCode.lastError,
			", sce error 0x",
			resultCode.lastErrorSCE.ToString("X8")
		}));
	}

	private void OnSignInError(Messages.PluginMessage msg)
	{
		ResultCode resultCode = default(ResultCode);
		User.GetLastSignInError(ref resultCode);
		OnScreenLog.Add(string.Concat(new object[]
		{
			resultCode.get_className(),
			": ",
			resultCode.lastError,
			", sce error 0x",
			resultCode.lastErrorSCE.ToString("X8")
		}));
	}

	[DebuggerHidden]
	private IEnumerator DoJoinSessionFromInvite()
	{
		SonyNpMain.<DoJoinSessionFromInvite>c__Iterator1CA <DoJoinSessionFromInvite>c__Iterator1CA = new SonyNpMain.<DoJoinSessionFromInvite>c__Iterator1CA();
		<DoJoinSessionFromInvite>c__Iterator1CA.<>f__this = this;
		return <DoJoinSessionFromInvite>c__Iterator1CA;
	}

	private void OnMessagingSessionInviteRetrieved(Messages.PluginMessage msg)
	{
		base.StartCoroutine("DoJoinSessionFromInvite");
	}

	private void OnMessagingSessionInviteReceived(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessagingSessionInviteReceived ");
	}

	private void OnMessagingSessionInviteAccepted(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessagingSessionInviteAccepted ");
	}

	private void OnServerInitialized(NetworkPlayer player)
	{
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Server Initialized: ",
			player.get_ipAddress(),
			":",
			player.get_port()
		}));
		OnScreenLog.Add(" Network.isServer: " + Network.get_isServer());
		OnScreenLog.Add(" Network.isClient: " + Network.get_isClient());
		OnScreenLog.Add(" Network.peerType: " + Network.get_peerType());
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Player connected from ",
			player.get_ipAddress(),
			":",
			player.get_port()
		}));
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		OnScreenLog.Add("Player disconnected " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	private void SendSessionData()
	{
		SonyNpMain.SharedSessionData sharedSessionData = default(SonyNpMain.SharedSessionData);
		sharedSessionData.id = this.sendCount++;
		sharedSessionData.text = "Here's some RPC data";
		sharedSessionData.item1 = 2;
		sharedSessionData.item2 = 987654321;
		byte[] array = sharedSessionData.WriteToBuffer();
		base.GetComponent<NetworkView>().RPC("RecieveSharedSessionData", 1, new object[]
		{
			array
		});
	}

	[RPC]
	private void RecieveSharedSessionData(byte[] buffer)
	{
		SonyNpMain.SharedSessionData sharedSessionData = default(SonyNpMain.SharedSessionData);
		sharedSessionData.ReadFromBuffer(buffer);
		OnScreenLog.Add(string.Concat(new object[]
		{
			"RPC Rec: id ",
			sharedSessionData.id,
			" - ",
			sharedSessionData.text,
			" item1: ",
			sharedSessionData.item1,
			" item2: ",
			sharedSessionData.item2
		}));
	}

	private void OnConnectedToServer()
	{
		OnScreenLog.Add("Connected to server...");
		OnScreenLog.Add(" Network.isServer: " + Network.get_isServer());
		OnScreenLog.Add(" Network.isClient: " + Network.get_isClient());
		OnScreenLog.Add(" Network.peerType: " + Network.get_peerType());
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		OnScreenLog.Add("Disconnected from server " + info);
		this.sessions.sendingData = false;
		this.sendCount = 0;
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		OnScreenLog.Add("Could not connect to server: " + error);
	}
}
