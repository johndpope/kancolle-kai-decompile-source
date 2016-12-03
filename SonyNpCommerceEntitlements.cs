using Sony.NP;
using System;

public class SonyNpCommerceEntitlements : IScreen
{
	private MenuLayout menu;

	public SonyNpCommerceEntitlements()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menu;
	}

	public void Initialize()
	{
		this.menu = new MenuLayout(this, 450, 34);
		Commerce.add_OnGotEntitlementList(new Messages.EventHandler(this.OnGotEntitlementList));
		Commerce.add_OnConsumedEntitlement(new Messages.EventHandler(this.OnConsumedEntitlement));
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public void Process(MenuStack stack)
	{
		bool enabled = User.get_IsSignedInPSN() && !Commerce.IsBusy();
		this.menu.Update();
		if (this.menu.AddItem("Get Entitlement List", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestEntitlementList());
		}
		if (this.menu.AddItem("Consume Entitlement", enabled))
		{
			Commerce.CommerceEntitlement[] entitlementList = Commerce.GetEntitlementList();
			if (entitlementList.Length > 0)
			{
				SonyNpCommerce.ErrorHandler(Commerce.ConsumeEntitlement(entitlementList[0].get_id(), entitlementList[0].remainingCount));
			}
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	private void OnGotEntitlementList(Messages.PluginMessage msg)
	{
		Commerce.CommerceEntitlement[] entitlementList = Commerce.GetEntitlementList();
		OnScreenLog.Add("Got Entitlement List, ");
		if (entitlementList.Length > 0)
		{
			Commerce.CommerceEntitlement[] array = entitlementList;
			for (int i = 0; i < array.Length; i++)
			{
				Commerce.CommerceEntitlement commerceEntitlement = array[i];
				OnScreenLog.Add(string.Concat(new object[]
				{
					" ",
					commerceEntitlement.get_id(),
					" rc: ",
					commerceEntitlement.remainingCount,
					" cc: ",
					commerceEntitlement.consumedCount,
					" type: ",
					commerceEntitlement.type
				}));
			}
		}
		else
		{
			OnScreenLog.Add("You do not have any entitlements.");
		}
	}

	private void OnConsumedEntitlement(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Consumed Entitlement");
	}
}
