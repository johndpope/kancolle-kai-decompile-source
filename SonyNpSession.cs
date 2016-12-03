using Sony.NP;
using System;
using UnityEngine;

public class SonyNpSession : IScreen
{
	private MenuLayout menuSession;

	private MenuLayout menuInSessionHosting;

	private MenuLayout menuInSessionClient;

	private bool matchingIsReady;

	private int gameDetails = 100;

	private int cartype;

	private int serverPort = 25001;

	private int serverMaxConnections = 32;

	private int appVersion = 200;

	public string sessionPassword = "password";

	public bool sendingData;

	private Matching.Session[] availableSessions;

	private Matching.SessionMemberInfo? host;

	private Matching.SessionMemberInfo? myself;

	private Matching.SessionMemberInfo? connected;

	private Matching.FlagSessionCreate SignallingType = 4;

	public SonyNpSession()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuSession;
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
			Matching.GetLastError(ref resultCode);
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
		this.MenuSession(stack);
	}

	public void Initialize()
	{
		this.menuSession = new MenuLayout(this, 550, 34);
		this.menuInSessionHosting = new MenuLayout(this, 450, 34);
		this.menuInSessionClient = new MenuLayout(this, 450, 34);
		Matching.add_OnCreatedSession(new Messages.EventHandler(this.OnMatchingCreatedSession));
		Matching.add_OnFoundSessions(new Messages.EventHandler(this.OnMatchingFoundSessions));
		Matching.add_OnJoinedSession(new Messages.EventHandler(this.OnMatchingJoinedSession));
		Matching.add_OnJoinInvalidSession(new Messages.EventHandler(this.OnMatchingJoinInvalidSession));
		Matching.add_OnUpdatedSession(new Messages.EventHandler(this.OnMatchingUpdatedSession));
		Matching.add_OnLeftSession(new Messages.EventHandler(this.OnMatchingLeftSession));
		Matching.add_OnSessionDestroyed(new Messages.EventHandler(this.OnMatchingSessionDestroyed));
		Matching.add_OnKickedOut(new Messages.EventHandler(this.OnMatchingKickedOut));
		Matching.add_OnSessionError(new Messages.EventHandler(this.OnSessionError));
		Matching.ClearAttributeDefinitions();
		Matching.AddAttributeDefinitionInt("LEVEL", 2);
		Matching.AddAttributeDefinitionBin("RACE_TRACK", 4, 2);
		Matching.AddAttributeDefinitionBin("CAR_TYPE", 16, 4);
		Matching.AddAttributeDefinitionInt("GAME_DETAILS", 8);
		Matching.AddAttributeDefinitionInt("APP_VERSION", 2);
		Matching.AddAttributeDefinitionBin("TEST_BIN_SEARCH", 2, 8);
		Matching.AddAttributeDefinitionBin("PASSWORD", 8, 2);
		this.ErrorHandler(Matching.RegisterAttributeDefinitions());
	}

	public void MenuSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.get_IsSignedInPSN();
		bool inSession = Matching.get_InSession();
		if (!this.matchingIsReady && isSignedInPSN)
		{
			this.matchingIsReady = true;
		}
		if (inSession)
		{
			this.MenuInSession(menuStack);
		}
		else
		{
			this.MenuSetupSession(menuStack);
		}
	}

	public void MenuSetupSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.get_IsSignedInPSN();
		bool inSession = Matching.get_InSession();
		bool sessionIsBusy = Matching.get_SessionIsBusy();
		this.menuSession.Update();
		if (this.menuSession.AddItem("Create & Join Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating session...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("APP_VERSION");
			sessionAttribute.intValue = this.appVersion;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("PASSWORD");
			sessionAttribute.set_binValue("NO");
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("CAR_TYPE");
			sessionAttribute.set_binValue("CATMOB");
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("LEVEL");
			sessionAttribute.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("RACE_TRACK");
			sessionAttribute.set_binValue("TURKEY");
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("GAME_DETAILS");
			sessionAttribute.intValue = this.gameDetails;
			Matching.AddSessionAttribute(sessionAttribute);
			sessionAttribute = new Matching.SessionAttribute();
			sessionAttribute.set_name("TEST_BIN_SEARCH");
			sessionAttribute.set_binValue("BIN_VALUE");
			Matching.AddSessionAttribute(sessionAttribute);
			string text = "Test Session";
			int num = 0;
			int num2 = 0;
			int num3 = 8;
			string empty = string.Empty;
			string text2 = "Toolkit Sample Session";
			this.ErrorHandler(Matching.CreateSession(text, num, num2, num3, empty, this.SignallingType, 4, text2));
		}
		if (this.menuSession.AddItem("Create & Join Private Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating private session... password is required");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("APP_VERSION");
			sessionAttribute2.intValue = this.appVersion;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("PASSWORD");
			sessionAttribute2.set_binValue("YES");
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("CAR_TYPE");
			sessionAttribute2.set_binValue("CATMOB");
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("LEVEL");
			sessionAttribute2.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("RACE_TRACK");
			sessionAttribute2.set_binValue("TURKEY");
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("GAME_DETAILS");
			sessionAttribute2.intValue = this.gameDetails;
			Matching.AddSessionAttribute(sessionAttribute2);
			sessionAttribute2 = new Matching.SessionAttribute();
			sessionAttribute2.set_name("TEST_BIN_SEARCH");
			sessionAttribute2.set_binValue("BIN_VALUE");
			Matching.AddSessionAttribute(sessionAttribute2);
			string text3 = "Test Session";
			int num4 = 0;
			int num5 = 0;
			int num6 = 8;
			string text4 = this.sessionPassword;
			string text5 = "Toolkit Sample Session";
			this.ErrorHandler(Matching.CreateSession(text3, num4, num5, num6, text4, this.SignallingType | 16, 8, text5));
		}
		if (this.menuSession.AddItem("Create & Join Friend Session", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Creating Friend session...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("APP_VERSION");
			sessionAttribute3.intValue = this.appVersion;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("PASSWORD");
			sessionAttribute3.set_binValue("YES");
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("CAR_TYPE");
			sessionAttribute3.set_binValue("CATMOB");
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("LEVEL");
			sessionAttribute3.intValue = 1;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("RACE_TRACK");
			sessionAttribute3.set_binValue("TURKEY");
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("GAME_DETAILS");
			sessionAttribute3.intValue = this.gameDetails;
			Matching.AddSessionAttribute(sessionAttribute3);
			sessionAttribute3 = new Matching.SessionAttribute();
			sessionAttribute3.set_name("TEST_BIN_SEARCH");
			sessionAttribute3.set_binValue("BIN_VALUE");
			Matching.AddSessionAttribute(sessionAttribute3);
			string text6 = "Test Session";
			int num7 = 0;
			int num8 = 0;
			int num9 = 8;
			int num10 = 8;
			string text7 = this.sessionPassword;
			string text8 = "Toolkit Sample Session";
			this.ErrorHandler(Matching.CreateFriendsSession(text6, num7, num8, num9, num10, text7, this.SignallingType | 16, text8));
		}
		if (this.menuSession.AddItem("Find Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute4 = new Matching.SessionAttribute();
			sessionAttribute4.set_name("APP_VERSION");
			sessionAttribute4.intValue = this.appVersion;
			sessionAttribute4.searchOperator = 1;
			Matching.AddSessionAttribute(sessionAttribute4);
			int num11 = 0;
			int num12 = 0;
			this.ErrorHandler(Matching.FindSession(num11, num12));
		}
		if (this.menuSession.AddItem("Find Sessions (bin search)", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute5 = new Matching.SessionAttribute();
			sessionAttribute5.set_name("TEST_BIN_SEARCH");
			sessionAttribute5.set_binValue("BIN_VALUE");
			sessionAttribute5.searchOperator = 1;
			Matching.AddSessionAttribute(sessionAttribute5);
			int num13 = 0;
			int num14 = 0;
			this.ErrorHandler(Matching.FindSession(num13, num14, 4096));
		}
		if (this.menuSession.AddItem("Find Friend Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding friend sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute6 = new Matching.SessionAttribute();
			sessionAttribute6.set_name("APP_VERSION");
			sessionAttribute6.intValue = this.appVersion;
			sessionAttribute6.searchOperator = 1;
			Matching.AddSessionAttribute(sessionAttribute6);
			int num15 = 0;
			int num16 = 0;
			this.ErrorHandler(Matching.FindSession(num15, num16, 1024));
		}
		if (this.menuSession.AddItem("Find Regional Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding friend sessions...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute7 = new Matching.SessionAttribute();
			sessionAttribute7.set_name("APP_VERSION");
			sessionAttribute7.intValue = this.appVersion;
			sessionAttribute7.searchOperator = 1;
			Matching.AddSessionAttribute(sessionAttribute7);
			int num17 = 0;
			int num18 = 0;
			this.ErrorHandler(Matching.FindSession(num17, num18, 4096));
		}
		if (this.menuSession.AddItem("Find Random Sessions", isSignedInPSN && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Finding sessions in a random order...");
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute8 = new Matching.SessionAttribute();
			sessionAttribute8.set_name("APP_VERSION");
			sessionAttribute8.intValue = this.appVersion;
			sessionAttribute8.searchOperator = 1;
			Matching.AddSessionAttribute(sessionAttribute8);
			int num19 = 0;
			int num20 = 0;
			this.ErrorHandler(Matching.FindSession(num19, num20, 262144));
		}
		bool flag = this.availableSessions != null && this.availableSessions.Length > 0;
		if (this.menuSession.AddItem("Join 1st Found Session", isSignedInPSN && flag && !inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Joining PSN session: " + this.availableSessions[0].sessionInfo.get_sessionName());
			Matching.ClearSessionAttributes();
			Matching.SessionAttribute sessionAttribute9 = new Matching.SessionAttribute();
			sessionAttribute9.set_name("CAR_TYPE");
			sessionAttribute9.set_binValue("CATMOB");
			Matching.AddSessionAttribute(sessionAttribute9);
			this.ErrorHandler(Matching.JoinSession(this.availableSessions[0].sessionInfo.sessionID, this.sessionPassword));
		}
		if (this.menuSession.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	public void MenuInSession(MenuStack menuStack)
	{
		bool isSignedInPSN = User.get_IsSignedInPSN();
		bool inSession = Matching.get_InSession();
		bool sessionIsBusy = Matching.get_SessionIsBusy();
		bool isHost = Matching.get_IsHost();
		MenuLayout menuLayout = (!isHost) ? this.menuInSessionClient : this.menuInSessionHosting;
		menuLayout.Update();
		if (isHost && menuLayout.AddItem("Modify Session", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Modifying session...");
			Matching.ClearModifySessionAttributes();
			this.gameDetails += 100;
			Matching.ModifySessionAttribute modifySessionAttribute = new Matching.ModifySessionAttribute();
			modifySessionAttribute.set_name("GAME_DETAILS");
			modifySessionAttribute.intValue = this.gameDetails;
			Matching.AddModifySessionAttribute(modifySessionAttribute);
			this.ErrorHandler(Matching.ModifySession(8));
		}
		if (menuLayout.AddItem("Modify Member Attribute", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Modifying Member Attribute...");
			Matching.ClearModifySessionAttributes();
			Matching.ModifySessionAttribute modifySessionAttribute2 = new Matching.ModifySessionAttribute();
			modifySessionAttribute2.set_name("CAR_TYPE");
			this.cartype++;
			if (this.cartype > 3)
			{
				this.cartype = 0;
			}
			switch (this.cartype)
			{
			case 0:
				modifySessionAttribute2.set_binValue("CATMOB");
				break;
			case 1:
				modifySessionAttribute2.set_binValue("CARTYPE1");
				break;
			case 2:
				modifySessionAttribute2.set_binValue("CARTYPE2");
				break;
			case 3:
				modifySessionAttribute2.set_binValue("CARTYPE3");
				break;
			}
			modifySessionAttribute2.intValue = this.gameDetails;
			Matching.AddModifySessionAttribute(modifySessionAttribute2);
			this.ErrorHandler(Matching.ModifySession(16));
		}
		if (!this.sendingData)
		{
			if (menuLayout.AddItem("Start Sending Data", isSignedInPSN && inSession && !sessionIsBusy))
			{
				this.sendingData = true;
			}
		}
		else if (menuLayout.AddItem("Stop Sending Data", isSignedInPSN && inSession && !sessionIsBusy))
		{
			this.sendingData = false;
		}
		if (menuLayout.AddItem("Leave Session", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Leaving session...");
			this.ErrorHandler(Matching.LeaveSession());
		}
		if (menuLayout.AddItem("List session members", isSignedInPSN && inSession && !sessionIsBusy))
		{
			Matching.SessionMemberInfo[] members = Matching.GetSession().members;
			for (int i = 0; i < members.Length; i++)
			{
				Matching.SessionMemberInfo sessionMemberInfo = members[i];
				string msg = string.Concat(new object[]
				{
					i,
					"/memberId:",
					sessionMemberInfo.memberId,
					"/memberFlag:",
					sessionMemberInfo.memberFlag,
					"/addr:",
					sessionMemberInfo.addr,
					"/natType:",
					sessionMemberInfo.natType,
					"/port:",
					sessionMemberInfo.port
				});
				OnScreenLog.Add(msg);
			}
		}
		if (menuLayout.AddItem("Invite Friend", isSignedInPSN && inSession && !sessionIsBusy))
		{
			OnScreenLog.Add("Invite Friend...");
			this.ErrorHandler(Matching.InviteToSession("Invite Test", 8));
		}
		if (menuLayout.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
		if (Matching.get_IsHost())
		{
			NetworkPlayer[] connections = Network.get_connections();
			GUI.Label(new Rect((float)(Screen.get_width() - 200), (float)(Screen.get_height() - 200), 200f, 64f), connections.Length.ToString());
		}
	}

	private Matching.SessionMemberInfo? FindHostMember(Matching.Session session)
	{
		Matching.SessionMemberInfo[] members = session.members;
		for (int i = 0; i < members.Length; i++)
		{
			if ((members[i].memberFlag & 2) != null)
			{
				return new Matching.SessionMemberInfo?(members[i]);
			}
		}
		return default(Matching.SessionMemberInfo?);
	}

	private Matching.SessionMemberInfo? FindSelfMember(Matching.Session session)
	{
		Matching.SessionMemberInfo[] members = session.members;
		for (int i = 0; i < members.Length; i++)
		{
			if ((members[i].memberFlag & 4) != null)
			{
				return new Matching.SessionMemberInfo?(members[i]);
			}
		}
		return default(Matching.SessionMemberInfo?);
	}

	private bool InitializeHostAndSelf(Matching.Session session)
	{
		this.host = this.FindHostMember(session);
		Matching.SessionMemberInfo? sessionMemberInfo = this.host;
		if (!sessionMemberInfo.get_HasValue())
		{
			OnScreenLog.Add("Host member not found!");
			return false;
		}
		this.myself = this.FindSelfMember(session);
		Matching.SessionMemberInfo? sessionMemberInfo2 = this.myself;
		if (!sessionMemberInfo2.get_HasValue())
		{
			OnScreenLog.Add("Self member not found!");
			return false;
		}
		return true;
	}

	private void OnMatchingFoundSessions(Messages.PluginMessage msg)
	{
		Matching.Session[] foundSessionList = Matching.GetFoundSessionList();
		OnScreenLog.Add("Found " + foundSessionList.Length + " sessions");
		for (int i = 0; i < foundSessionList.Length; i++)
		{
			this.DumpSessionInfo(foundSessionList[i]);
		}
		this.availableSessions = foundSessionList;
	}

	private string IntIPToIPString(int ip)
	{
		int num = ip & 255;
		int num2 = ip >> 8 & 255;
		int num3 = ip >> 16 & 255;
		int num4 = ip >> 24 & 255;
		return string.Concat(new string[]
		{
			num.ToString(),
			".",
			num2.ToString(),
			".",
			num3.ToString(),
			".",
			num4.ToString()
		});
	}

	private void DumpSessionInfo(Matching.Session session)
	{
		Matching.SessionInfo sessionInfo = session.sessionInfo;
		Matching.SessionAttributeInfo[] array = session.sessionAttributes;
		Matching.SessionMemberInfo[] members = session.members;
		OnScreenLog.Add(string.Concat(new object[]
		{
			"session: ",
			sessionInfo.get_sessionName(),
			", ",
			sessionInfo.numMembers,
			", ",
			sessionInfo.maxMembers,
			", ",
			sessionInfo.openSlots,
			", ",
			sessionInfo.reservedSlots,
			", ",
			sessionInfo.worldId,
			", ",
			sessionInfo.roomId
		}));
		int i = 0;
		while (i < array.Length)
		{
			string text = string.Concat(new object[]
			{
				" Attribute ",
				i,
				": ",
				array[i].get_attributeName()
			});
			switch (array[i].attributeValueType)
			{
			case 2:
				text = text + " = " + array[i].attributeIntValue;
				break;
			case 3:
				goto IL_171;
			case 4:
				text = text + " = " + array[i].get_attributeBinValue();
				break;
			default:
				goto IL_171;
			}
			IL_184:
			text = text + ", " + array[i].attributeType;
			OnScreenLog.Add(text);
			i++;
			continue;
			IL_171:
			text += ", has bad value type";
			goto IL_184;
		}
		if (members == null)
		{
			return;
		}
		for (int j = 0; j < members.Length; j++)
		{
			OnScreenLog.Add(string.Concat(new object[]
			{
				" Member ",
				j,
				": ",
				members[j].get_npOnlineID(),
				", Type: ",
				members[j].memberFlag
			}));
			if (members[j].addr != 0)
			{
				OnScreenLog.Add(string.Concat(new object[]
				{
					"  IP: ",
					this.IntIPToIPString(members[j].addr),
					" port ",
					members[j].port,
					" 0x",
					members[j].port.ToString("X")
				}));
			}
			else
			{
				OnScreenLog.Add("  IP: unknown ");
			}
			array = session.memberAttributes.get_Item(j);
			if (array.Length == 0)
			{
				OnScreenLog.Add("  No Member Attributes");
			}
			int k = 0;
			while (k < array.Length)
			{
				string text2 = string.Concat(new object[]
				{
					"  Attribute ",
					k,
					": ",
					array[k].get_attributeName()
				});
				switch (array[k].attributeValueType)
				{
				case 2:
					text2 = text2 + " = " + array[k].attributeIntValue;
					break;
				case 3:
					goto IL_379;
				case 4:
					text2 = text2 + " = " + array[k].get_attributeBinValue();
					break;
				default:
					goto IL_379;
				}
				IL_38C:
				OnScreenLog.Add(text2);
				k++;
				continue;
				IL_379:
				text2 += ", has bad value type";
				goto IL_38C;
			}
		}
	}

	private void OnMatchingCreatedSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Created session...");
		Matching.Session session = Matching.GetSession();
		this.DumpSessionInfo(session);
		if (!this.InitializeHostAndSelf(session))
		{
			OnScreenLog.Add("ERROR: Expected members not found!");
		}
		NetworkConnectionError networkConnectionError = Network.InitializeServer(this.serverMaxConnections, this.serverPort, false);
		if (networkConnectionError != null)
		{
			OnScreenLog.Add("Server err: " + networkConnectionError);
		}
		else
		{
			OnScreenLog.Add("Started Server");
		}
	}

	private void OnMatchingJoinedSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Joined PSN matching session... waiting on session info in OnMatchingUpdatedSession()");
	}

	private void OnMatchingUpdatedSession(Messages.PluginMessage msg)
	{
		IntPtr sessionInformationPtr = Matching.GetSessionInformationPtr();
		OnScreenLog.Add("Session info updated...");
		Matching.Session session = Matching.GetSession();
		this.DumpSessionInfo(session);
		if (!this.InitializeHostAndSelf(session))
		{
			OnScreenLog.Add("ERROR: Expected members not found!");
		}
		if (!Matching.get_IsHost())
		{
			Matching.SessionMemberInfo? sessionMemberInfo = this.connected;
			if (!sessionMemberInfo.get_HasValue())
			{
				if (this.host.get_Value().addr == 0)
				{
					OnScreenLog.Add("Unable to retrieve host IP address");
					this.ErrorHandler(Matching.LeaveSession());
					return;
				}
				string text = this.IntIPToIPString(this.host.get_Value().addr);
				OnScreenLog.Add(string.Concat(new object[]
				{
					"Connecting to ",
					text,
					":",
					this.serverPort,
					" using signalling port:",
					this.host.get_Value().port
				}));
				NetworkConnectionError networkConnectionError = Network.Connect(text, this.serverPort);
				if (networkConnectionError != null)
				{
					OnScreenLog.Add("Connection failed: " + networkConnectionError);
				}
				else
				{
					OnScreenLog.Add(string.Concat(new object[]
					{
						"Connected to host ",
						text,
						" : ",
						this.serverPort
					}));
					this.connected = this.host;
				}
			}
		}
	}

	private void OnMatchingJoinInvalidSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Failed to join session...");
		OnScreenLog.Add(" Session search results may be stale.");
	}

	private void OnMatchingLeftSession(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Left the session");
		Network.Disconnect(1);
		this.host = default(Matching.SessionMemberInfo?);
		this.connected = default(Matching.SessionMemberInfo?);
		this.myself = default(Matching.SessionMemberInfo?);
	}

	private void OnMatchingSessionDestroyed(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Session Destroyed");
		Network.Disconnect(1);
		this.host = default(Matching.SessionMemberInfo?);
		this.connected = default(Matching.SessionMemberInfo?);
		this.myself = default(Matching.SessionMemberInfo?);
	}

	private void OnMatchingKickedOut(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Kicked out of session");
		Network.Disconnect(1);
		this.host = default(Matching.SessionMemberInfo?);
		this.connected = default(Matching.SessionMemberInfo?);
		this.myself = default(Matching.SessionMemberInfo?);
	}

	private void OnSessionError(Messages.PluginMessage msg)
	{
		this.ErrorHandler(3);
	}
}
