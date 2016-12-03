using Sony.NP;
using System;
using System.IO;
using UnityEngine;

public class SonyNpMessaging : IScreen
{
	private class GameInviteData
	{
		public string taunt;

		public int level;

		public int score;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream(16);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.taunt);
			binaryWriter.Write(this.level);
			binaryWriter.Write(this.score);
			binaryWriter.Close();
			return memoryStream.GetBuffer();
		}

		public void ReadFromBuffer(byte[] buffer)
		{
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			this.taunt = binaryReader.ReadString();
			this.level = binaryReader.ReadInt32();
			this.score = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private struct GameData
	{
		public string text;

		public int item1;

		public int item2;

		public byte[] WriteToBuffer()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
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
			this.text = binaryReader.ReadString();
			this.item1 = binaryReader.ReadInt32();
			this.item2 = binaryReader.ReadInt32();
			binaryReader.Close();
		}
	}

	private MenuLayout menuMessaging;

	public SonyNpMessaging()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuMessaging;
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		this.MenuMessaging(stack);
	}

	public void Initialize()
	{
		this.menuMessaging = new MenuLayout(this, 500, 34);
		Messaging.add_OnMessageSent(new Messages.EventHandler(this.OnSomeEvent));
		Messaging.add_OnMessageNotSent(new Messages.EventHandler(this.OnSomeEvent));
		Messaging.add_OnMessageCanceled(new Messages.EventHandler(this.OnSomeEvent));
		Messaging.add_OnCustomDataMessageRetrieved(new Messages.EventHandler(this.OnMessagingGotGameMessage));
		Messaging.add_OnCustomInviteMessageRetrieved(new Messages.EventHandler(this.OnMessagingGotGameInvite));
		Messaging.add_OnInGameDataMessageRetrieved(new Messages.EventHandler(this.OnMessagingGotInGameDataMessage));
		Messaging.add_OnMessageNotSentFreqTooHigh(new Messages.EventHandler(this.OnSomeEvent));
		Messaging.add_OnMessageError(new Messages.EventHandler(this.OnMessageError));
	}

	public void MenuMessaging(MenuStack menuStack)
	{
		this.menuMessaging.Update();
		if (this.menuMessaging.AddItem("Show Messages & Invites", User.get_IsSignedInPSN() && !Messaging.IsBusy()))
		{
			Messaging.ShowRecievedDataMessageDialog();
		}
		if (this.menuMessaging.AddItem("Send Session Invite", User.get_IsSignedInPSN() && Matching.get_InSession()))
		{
			string text = "Join my session";
			int num = 8;
			Matching.InviteToSession(text, num);
		}
		if (this.menuMessaging.AddItem("Send Game Invite", User.get_IsSignedInPSN() && !Messaging.IsBusy()))
		{
			byte[] data = new SonyNpMessaging.GameInviteData
			{
				taunt = "I got an awesome score, can you do better?",
				level = 1,
				score = 123456789
			}.WriteToBuffer();
			Messaging.MsgRequest msgRequest = new Messaging.MsgRequest();
			msgRequest.set_body("Game invite");
			msgRequest.expireMinutes = 30;
			msgRequest.set_data(data);
			msgRequest.npIDCount = 8;
			string dataDescription = "Some data to test invite messages";
			string dataName = "Test data";
			msgRequest.dataDescription = dataDescription;
			msgRequest.dataName = dataName;
			msgRequest.iconPath = Application.get_streamingAssetsPath() + "/PSP2SessionImage.jpg";
			Messaging.SendMessage(msgRequest);
		}
		if (this.menuMessaging.AddItem("Send Data Message", User.get_IsSignedInPSN() && !Messaging.IsBusy()))
		{
			SonyNpMessaging.GameData gameData = default(SonyNpMessaging.GameData);
			gameData.text = "Here's some data";
			gameData.item1 = 2;
			gameData.item2 = 987654321;
			byte[] data2 = gameData.WriteToBuffer();
			Messaging.MsgRequest msgRequest2 = new Messaging.MsgRequest();
			msgRequest2.set_body("Data message");
			msgRequest2.expireMinutes = 0;
			msgRequest2.set_data(data2);
			msgRequest2.npIDCount = 8;
			string dataDescription2 = "Some data to test messages";
			string dataName2 = "Test data";
			msgRequest2.dataDescription = dataDescription2;
			msgRequest2.dataName = dataName2;
			msgRequest2.iconPath = Application.get_streamingAssetsPath() + "/PSP2SessionImage.jpg";
			Messaging.SendMessage(msgRequest2);
		}
		if (this.menuMessaging.AddItem("Send In Game Data (Session)", Matching.get_InSession() && !Messaging.IsBusy()))
		{
			Matching.SessionMemberInfo[] members = Matching.GetSession().members;
			if (members == null)
			{
				return;
			}
			int num2 = -1;
			for (int i = 0; i < members.Length; i++)
			{
				if ((members[i].memberFlag & 4) == null)
				{
					num2 = i;
					break;
				}
			}
			if (num2 >= 0)
			{
				OnScreenLog.Add("Sending in game data message to " + members[num2].get_npOnlineID());
				SonyNpMessaging.GameData gameData2 = default(SonyNpMessaging.GameData);
				gameData2.text = "Here's some data";
				gameData2.item1 = 2;
				gameData2.item2 = 987654321;
				byte[] array = gameData2.WriteToBuffer();
				Messaging.SendInGameDataMessage(members[num2].get_npID(), array);
			}
			else
			{
				OnScreenLog.Add("No session member to send to.");
			}
		}
		if (this.menuMessaging.AddItem("Send In Game Message (Friend)", User.get_IsSignedInPSN() && !Messaging.IsBusy()))
		{
			Friends.Friend[] cachedFriendsList = Friends.GetCachedFriendsList();
			if (cachedFriendsList.Length > 0)
			{
				int num3 = 0;
				if (num3 >= 0)
				{
					OnScreenLog.Add("Sending in game data message to " + cachedFriendsList[num3].get_npOnlineID());
					SonyNpMessaging.GameData gameData3 = default(SonyNpMessaging.GameData);
					gameData3.text = "Here's some data";
					gameData3.item1 = 2;
					gameData3.item2 = 987654321;
					byte[] array2 = gameData3.WriteToBuffer();
					Messaging.SendInGameDataMessage(cachedFriendsList[num3].get_npID(), array2);
				}
				else
				{
					OnScreenLog.Add("No friends in this context.");
				}
			}
			else
			{
				OnScreenLog.Add("No friends cached.");
				OnScreenLog.Add("refresh the friends list then try again.");
			}
		}
		if (this.menuMessaging.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnMessagingGotGameInvite(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got game invite...");
		SonyNpMessaging.GameInviteData gameInviteData = new SonyNpMessaging.GameInviteData();
		byte[] gameInviteAttachment = Messaging.GetGameInviteAttachment();
		gameInviteData.ReadFromBuffer(gameInviteAttachment);
		OnScreenLog.Add(" taunt: " + gameInviteData.taunt);
		OnScreenLog.Add(" level: " + gameInviteData.level);
		OnScreenLog.Add(" score: " + gameInviteData.score);
	}

	private void OnMessagingGotGameMessage(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got message...");
		SonyNpMessaging.GameData gameData = default(SonyNpMessaging.GameData);
		byte[] messageAttachment = Messaging.GetMessageAttachment();
		gameData.ReadFromBuffer(messageAttachment);
		OnScreenLog.Add(" text: " + gameData.text);
		OnScreenLog.Add(" item1: " + gameData.item1);
		OnScreenLog.Add(" item2: " + gameData.item2);
	}

	private void OnMessagingGotInGameDataMessage(Messages.PluginMessage msg)
	{
		SonyNpMessaging.GameData gameData = default(SonyNpMessaging.GameData);
		OnScreenLog.Add("Got in-game data message...");
		while (Messaging.InGameDataMessagesRecieved())
		{
			Messaging.InGameDataMessage inGameDataMessage = Messaging.GetInGameDataMessage();
			gameData.ReadFromBuffer(inGameDataMessage.data);
			OnScreenLog.Add(string.Concat(new object[]
			{
				" ID: ",
				inGameDataMessage.messageID,
				" text: ",
				gameData.text,
				" item1: ",
				gameData.item1,
				" item2: ",
				gameData.item2
			}));
		}
	}

	private void OnMessageError(Messages.PluginMessage msg)
	{
		OnScreenLog.Add(" OnMessageError error code: " + Messaging.GetErrorFromMessage(msg).ToString("X"));
	}
}
