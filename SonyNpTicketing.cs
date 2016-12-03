using Sony.NP;
using System;

public class SonyNpTicketing : IScreen
{
	private MenuLayout menuTicketing;

	private bool gotTicket;

	private Ticketing.Ticket ticket;

	public SonyNpTicketing()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuTicketing;
	}

	public void Initialize()
	{
		this.menuTicketing = new MenuLayout(this, 450, 34);
		Ticketing.add_OnGotTicket(new Messages.EventHandler(this.OnGotTicket));
		Ticketing.add_OnError(new Messages.EventHandler(this.OnError));
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
			Ticketing.GetLastError(ref resultCode);
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
		this.menuTicketing.Update();
		bool enabled = User.get_IsSignedInPSN() && !Ticketing.IsBusy();
		if (this.menuTicketing.AddItem("Request Ticket", enabled))
		{
			this.ErrorHandler(Ticketing.RequestTicket());
		}
		if (this.menuTicketing.AddItem("Request Cached Ticket", enabled))
		{
			this.ErrorHandler(Ticketing.RequestCachedTicket());
		}
		if (this.menuTicketing.AddItem("Get Ticket Entitlements", this.gotTicket))
		{
			Ticketing.TicketEntitlement[] ticketEntitlements = Ticketing.GetTicketEntitlements(this.ticket);
			OnScreenLog.Add("Ticket contains " + ticketEntitlements.Length + " entitlements");
			for (int i = 0; i < ticketEntitlements.Length; i++)
			{
				OnScreenLog.Add("Entitlement " + i);
				OnScreenLog.Add(string.Concat(new object[]
				{
					" ",
					ticketEntitlements[i].get_id(),
					" rc: ",
					ticketEntitlements[i].remainingCount,
					" cc: ",
					ticketEntitlements[i].consumedCount,
					" type: ",
					ticketEntitlements[i].type
				}));
			}
		}
		if (this.menuTicketing.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	private void OnGotTicket(Messages.PluginMessage msg)
	{
		this.ticket = Ticketing.GetTicket();
		this.gotTicket = true;
		OnScreenLog.Add("GotTicket");
		OnScreenLog.Add(" dataSize: " + this.ticket.dataSize);
		Ticketing.TicketInfo ticketInfo = Ticketing.GetTicketInfo(this.ticket);
		OnScreenLog.Add(" Issuer ID: " + ticketInfo.issuerID);
		DateTime dateTime = new DateTime(ticketInfo.issuedDate, 1);
		OnScreenLog.Add(" Issue date: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		DateTime dateTime2 = new DateTime(ticketInfo.expireDate, 1);
		OnScreenLog.Add(" Expire date: " + dateTime2.ToLongDateString() + " - " + dateTime2.ToLongTimeString());
		OnScreenLog.Add(" Account ID: 0x" + ticketInfo.subjectAccountID.ToString("X8"));
		OnScreenLog.Add(" Online ID: " + ticketInfo.get_subjectOnlineID());
		OnScreenLog.Add(" Service ID: " + ticketInfo.get_serviceID());
		OnScreenLog.Add(" Domain: " + ticketInfo.get_subjectDomain());
		OnScreenLog.Add(" Country Code: " + ticketInfo.get_countryCode());
		OnScreenLog.Add(" Language Code: " + ticketInfo.languageCode);
		OnScreenLog.Add(" Age: " + ticketInfo.subjectAge);
		OnScreenLog.Add(" Chat disabled: " + ticketInfo.chatDisabled);
		OnScreenLog.Add(" Content rating: " + ticketInfo.contentRating);
	}

	private void OnError(Messages.PluginMessage msg)
	{
		this.ErrorHandler(3);
	}
}
