using Sony.NP;
using System;
using UnityEngine;

public class SonyNpTrophy : IScreen
{
	private MenuLayout menuTrophies;

	private int nextTrophyIndex = 1;

	private Trophies.GameInfo gameInfo;

	private Texture2D trophyIcon;

	private Texture2D trophyGroupIcon;

	public SonyNpTrophy()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuTrophies;
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
			Trophies.GetLastError(ref resultCode);
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
		this.MenuTrophies(stack);
	}

	public void Initialize()
	{
		this.menuTrophies = new MenuLayout(this, 450, 34);
		Trophies.add_OnGotGameInfo(new Messages.EventHandler(this.OnTrophyGotGameInfo));
		Trophies.add_OnGotGroupInfo(new Messages.EventHandler(this.OnTrophyGotGroupInfo));
		Trophies.add_OnGotTrophyInfo(new Messages.EventHandler(this.OnTrophyGotTrophyInfo));
		Trophies.add_OnGotProgress(new Messages.EventHandler(this.OnTrophyGotProgress));
		Trophies.add_OnAwardedTrophy(new Messages.EventHandler(this.OnSomeEvent));
		Trophies.add_OnAwardTrophyFailed(new Messages.EventHandler(this.OnSomeEvent));
		Trophies.add_OnAlreadyAwardedTrophy(new Messages.EventHandler(this.OnSomeEvent));
		Trophies.add_OnUnlockedPlatinum(new Messages.EventHandler(this.OnSomeEvent));
	}

	public void MenuTrophies(MenuStack menuStack)
	{
		this.menuTrophies.Update();
		bool trophiesAreAvailable = Trophies.get_TrophiesAreAvailable();
		if (this.menuTrophies.AddItem("Game Info", trophiesAreAvailable))
		{
			this.DumpGameInfo();
		}
		if (this.menuTrophies.AddItem("Group Info", trophiesAreAvailable && !Trophies.RequestGroupInfoIsBusy()))
		{
			this.ErrorHandler(Trophies.RequestGroupInfo());
		}
		if (this.menuTrophies.AddItem("Trophy Info", trophiesAreAvailable && !Trophies.RequestTrophyInfoIsBusy()))
		{
			this.ErrorHandler(Trophies.RequestTrophyInfo());
		}
		if (this.menuTrophies.AddItem("Trophy Progress", trophiesAreAvailable && !Trophies.RequestTrophyProgressIsBusy()))
		{
			this.ErrorHandler(Trophies.RequestTrophyProgress());
		}
		if (this.menuTrophies.AddItem("Award Trophy", trophiesAreAvailable) && this.ErrorHandler(Trophies.AwardTrophy(this.nextTrophyIndex)) == null)
		{
			this.nextTrophyIndex++;
			if (this.nextTrophyIndex == this.gameInfo.numTrophies)
			{
				this.nextTrophyIndex = 1;
			}
		}
		if (this.menuTrophies.AddItem("Award All Trophies", trophiesAreAvailable))
		{
			for (int i = 1; i < this.gameInfo.numTrophies; i++)
			{
				this.ErrorHandler(Trophies.AwardTrophy(i));
			}
		}
		if (this.menuTrophies.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void DumpGameInfo()
	{
		OnScreenLog.Add("title: " + this.gameInfo.get_title());
		OnScreenLog.Add("desc: " + this.gameInfo.get_description());
		OnScreenLog.Add("numTrophies: " + this.gameInfo.numTrophies);
		OnScreenLog.Add("numGroups: " + this.gameInfo.numGroups);
		OnScreenLog.Add("numBronze: " + this.gameInfo.numBronze);
		OnScreenLog.Add("numSilver: " + this.gameInfo.numSilver);
		OnScreenLog.Add("numGold: " + this.gameInfo.numGold);
		OnScreenLog.Add("numPlatinum: " + this.gameInfo.numPlatinum);
	}

	private void OnTrophyGotGameInfo(Messages.PluginMessage msg)
	{
		this.gameInfo = Trophies.GetCachedGameInfo();
		this.DumpGameInfo();
	}

	private void OnTrophyGotGroupInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Group List!");
		Trophies.GroupDetails[] cachedGroupDetails = Trophies.GetCachedGroupDetails();
		Trophies.GroupData[] cachedGroupData = Trophies.GetCachedGroupData();
		OnScreenLog.Add("Groups: " + cachedGroupDetails.Length);
		for (int i = 0; i < cachedGroupDetails.Length; i++)
		{
			if (cachedGroupDetails[i].get_hasIcon() && this.trophyGroupIcon == null)
			{
				this.trophyGroupIcon = cachedGroupDetails[i].get_icon();
				OnScreenLog.Add(string.Concat(new object[]
				{
					"Found icon: ",
					this.trophyGroupIcon.get_width(),
					", ",
					this.trophyGroupIcon.get_height()
				}));
			}
			OnScreenLog.Add(string.Concat(new object[]
			{
				" ",
				i,
				": ",
				cachedGroupDetails[i].groupId,
				", ",
				cachedGroupDetails[i].get_title(),
				", ",
				cachedGroupDetails[i].get_description(),
				", ",
				cachedGroupDetails[i].numTrophies,
				", ",
				cachedGroupDetails[i].numPlatinum,
				", ",
				cachedGroupDetails[i].numGold,
				", ",
				cachedGroupDetails[i].numSilver,
				", ",
				cachedGroupDetails[i].numBronze
			}));
			OnScreenLog.Add(string.Concat(new object[]
			{
				" ",
				i,
				": ",
				cachedGroupData[i].groupId,
				", ",
				cachedGroupData[i].unlockedTrophies,
				", ",
				cachedGroupData[i].unlockedPlatinum,
				", ",
				cachedGroupData[i].unlockedGold,
				", ",
				cachedGroupData[i].unlockedSilver,
				", ",
				cachedGroupData[i].unlockedBronze,
				", ",
				cachedGroupData[i].progressPercentage,
				cachedGroupData[i].userId.ToString("X")
			}));
		}
	}

	private void OnTrophyGotTrophyInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Trophy List!");
		Trophies.TrophyDetails[] cachedTrophyDetails = Trophies.GetCachedTrophyDetails();
		Trophies.TrophyData[] cachedTrophyData = Trophies.GetCachedTrophyData();
		OnScreenLog.Add("Trophies: " + cachedTrophyDetails.Length);
		for (int i = 0; i < cachedTrophyDetails.Length; i++)
		{
			if (cachedTrophyData[i].get_hasIcon() && this.trophyIcon == null)
			{
				this.trophyIcon = cachedTrophyData[i].get_icon();
				OnScreenLog.Add(string.Concat(new object[]
				{
					"Found icon: ",
					this.trophyIcon.get_width(),
					", ",
					this.trophyIcon.get_height()
				}));
			}
			OnScreenLog.Add(string.Concat(new object[]
			{
				" ",
				i,
				": ",
				cachedTrophyDetails[i].get_name(),
				", ",
				cachedTrophyDetails[i].trophyId,
				", ",
				cachedTrophyDetails[i].trophyGrade,
				", ",
				cachedTrophyDetails[i].groupId,
				", ",
				cachedTrophyDetails[i].hidden,
				", ",
				cachedTrophyData[i].unlocked,
				", ",
				cachedTrophyData[i].timestamp,
				", ",
				cachedTrophyData[i].userId.ToString("X")
			}));
		}
	}

	private void OnTrophyGotProgress(Messages.PluginMessage msg)
	{
		Trophies.TrophyProgress cachedTrophyProgress = Trophies.GetCachedTrophyProgress();
		OnScreenLog.Add("Progress for userId: 0x" + cachedTrophyProgress.userId.ToString("X"));
		OnScreenLog.Add("progressPercentage: " + cachedTrophyProgress.progressPercentage);
		OnScreenLog.Add("unlockedTrophies: " + cachedTrophyProgress.unlockedTrophies);
		OnScreenLog.Add("unlockedPlatinum: " + cachedTrophyProgress.unlockedPlatinum);
		OnScreenLog.Add("unlockedGold: " + cachedTrophyProgress.unlockedGold);
		OnScreenLog.Add("unlockedSilver: " + cachedTrophyProgress.unlockedSilver);
		OnScreenLog.Add("unlockedBronze: " + cachedTrophyProgress.unlockedBronze);
	}
}
