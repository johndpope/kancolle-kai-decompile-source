using Sony.NP;
using System;

public class SonyNpRanking : IScreen
{
	private MenuLayout rankingMenu;

	private ulong currentScore;

	private int rankBoardID;

	private int LastRankDisplayed;

	private int LastRankingMaxCount = 999;

	public SonyNpRanking()
	{
		this.Initialize();
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		this.MenuRanking(stack);
	}

	public MenuLayout GetMenu()
	{
		return this.rankingMenu;
	}

	public void Initialize()
	{
		this.rankingMenu = new MenuLayout(this, 450, 34);
		Ranking.add_OnCacheRegistered(new Messages.EventHandler(this.OnSomeEvent));
		Ranking.add_OnRegisteredNewBestScore(new Messages.EventHandler(this.OnRegisteredNewBestScore));
		Ranking.add_OnNotBestScore(new Messages.EventHandler(this.OnSomeEvent));
		Ranking.add_OnGotOwnRank(new Messages.EventHandler(this.OnRankingGotOwnRank));
		Ranking.add_OnGotFriendRank(new Messages.EventHandler(this.OnRankingGotFriendRank));
		Ranking.add_OnGotRankList(new Messages.EventHandler(this.OnRankingGotRankList));
		Ranking.add_OnRankingError(new Messages.EventHandler(this.OnRankingError));
	}

	private ErrorCode ErrorHandler(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			Ranking.GetLastError(ref resultCode);
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

	public void MenuRanking(MenuStack menuStack)
	{
		bool isSignedInPSN = User.get_IsSignedInPSN();
		this.rankingMenu.Update();
		if (this.rankingMenu.AddItem("Register Score", isSignedInPSN && !Ranking.RegisterScoreIsBusy()))
		{
			OnScreenLog.Add("Registering score: " + this.currentScore);
			this.ErrorHandler(Ranking.RegisterScore(this.rankBoardID, this.currentScore, "Insert comment here"));
			this.currentScore += 1uL;
		}
		if (this.rankingMenu.AddItem("Register score & data", isSignedInPSN && !Ranking.RegisterScoreIsBusy()))
		{
			OnScreenLog.Add("Registering score: " + this.currentScore);
			byte[] array = new byte[64];
			for (byte b = 0; b < 64; b += 1)
			{
				array[(int)b] = b;
			}
			this.ErrorHandler(Ranking.RegisterScoreWithData(this.rankBoardID, this.currentScore, "Insert comment here", array));
			this.currentScore += 1uL;
		}
		if (this.rankingMenu.AddItem("Own Rank", isSignedInPSN && !Ranking.RefreshOwnRankIsBusy()))
		{
			this.ErrorHandler(Ranking.RefreshOwnRank(this.rankBoardID));
		}
		if (this.rankingMenu.AddItem("Friend Rank", isSignedInPSN && !Ranking.RefreshFriendRankIsBusy()))
		{
			this.ErrorHandler(Ranking.RefreshFriendRank(this.rankBoardID));
		}
		if (this.rankingMenu.AddItem("Rank List", isSignedInPSN && !Ranking.RefreshRankListIsBusy()))
		{
			int num = this.LastRankDisplayed + 1;
			int num2 = Math.Min(10, this.LastRankingMaxCount - num + 1);
			this.ErrorHandler(Ranking.RefreshRankList(this.rankBoardID, num, num2));
		}
		if (this.rankingMenu.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnRegisteredNewBestScore(Messages.PluginMessage msg)
	{
		Ranking.Rank ownRank = Ranking.GetOwnRank();
		OnScreenLog.Add("New best score...");
		OnScreenLog.Add(string.Concat(new object[]
		{
			"rank #",
			ownRank.rank,
			", provisional rank #",
			ownRank.provisional,
			", online id:",
			ownRank.get_onlineId(),
			", score:",
			ownRank.score,
			", comment:",
			ownRank.get_comment()
		}));
	}

	private void LogRank(Ranking.Rank rank)
	{
		long num = 10L;
		DateTime dateTime = new DateTime((long)(rank.recordDate * (ulong)num));
		OnScreenLog.Add(string.Concat(new object[]
		{
			"#",
			rank.rank,
			" (provisionally #",
			rank.provisional,
			"), online id:",
			rank.get_onlineId(),
			", score:",
			rank.score,
			", comment:",
			rank.get_comment(),
			", recorded on:",
			dateTime.ToString()
		}));
		if (rank.gameInfoSize > 0)
		{
			int num2 = 0;
			string text = string.Empty;
			byte[] gameInfoData = rank.get_gameInfoData();
			for (int i = 0; i < gameInfoData.Length; i++)
			{
				byte b = gameInfoData[i];
				text = text + b.ToString() + ",";
				if (num2++ > 8)
				{
					break;
				}
			}
			text += "...";
			OnScreenLog.Add(string.Concat(new object[]
			{
				"  dataSize: ",
				rank.gameInfoSize,
				", data: ",
				text
			}));
		}
	}

	private void OnRankingGotOwnRank(Messages.PluginMessage msg)
	{
		Ranking.Rank ownRank = Ranking.GetOwnRank();
		OnScreenLog.Add("Own rank...");
		if (ownRank.rank > 0)
		{
			this.LogRank(ownRank);
		}
		else
		{
			OnScreenLog.Add("rank #: Not Ranked, " + ownRank.get_onlineId());
		}
	}

	private void OnRankingGotFriendRank(Messages.PluginMessage msg)
	{
		Ranking.Rank[] friendRanks = Ranking.GetFriendRanks();
		OnScreenLog.Add("Friend ranks...");
		for (int i = 0; i < friendRanks.Length; i++)
		{
			this.LogRank(friendRanks[i]);
		}
	}

	private void OnRankingGotRankList(Messages.PluginMessage msg)
	{
		Ranking.Rank[] rankList = Ranking.GetRankList();
		OnScreenLog.Add("Ranks...");
		OnScreenLog.Add(string.Concat(new object[]
		{
			"Showing ",
			rankList[0].serialRank,
			"-> ",
			rankList[0].serialRank + rankList.Length - 1,
			" out of ",
			Ranking.GetRanksCountOnServer()
		}));
		for (int i = 0; i < rankList.Length; i++)
		{
			this.LogRank(rankList[i]);
		}
		this.LastRankDisplayed = rankList[0].serialRank + rankList.Length - 1;
		this.LastRankingMaxCount = Ranking.GetRanksCountOnServer();
		Console.WriteLine(string.Concat(new object[]
		{
			"LastRankDisplayed:",
			this.LastRankDisplayed,
			" LastRankingMaxCount:",
			this.LastRankingMaxCount
		}));
		if (this.LastRankDisplayed >= this.LastRankingMaxCount)
		{
			this.LastRankDisplayed = 0;
		}
	}

	private void OnRankingError(Messages.PluginMessage msg)
	{
		this.ErrorHandler(3);
	}
}
