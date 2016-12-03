using Sony.NP;
using System;

public class SonyNpCommerceInGameStore : IScreen
{
	private MenuLayout menu;

	private bool sessionCreated;

	private string testCategoryID = "ED1633-NPXB01864_00-WEAPS_01";

	private string testProductID = "ED1633-NPXB01864_00-A000010000000000";

	private string[] testProductSkuIDs = new string[]
	{
		"ED1633-NPXB01864_00-A000010000000000-E001",
		"ED1633-NPXB01864_00-A000020000000000-E001"
	};

	public SonyNpCommerceInGameStore()
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
		Commerce.add_OnSessionCreated(new Messages.EventHandler(this.OnSessionCreated));
		Commerce.add_OnSessionAborted(new Messages.EventHandler(this.OnSomeEvent));
		Commerce.add_OnGotCategoryInfo(new Messages.EventHandler(this.OnGotCategoryInfo));
		Commerce.add_OnGotProductList(new Messages.EventHandler(this.OnGotProductList));
		Commerce.add_OnGotProductInfo(new Messages.EventHandler(this.OnGotProductInfo));
		Commerce.add_OnCheckoutStarted(new Messages.EventHandler(this.OnSomeEvent));
		Commerce.add_OnCheckoutFinished(new Messages.EventHandler(this.OnSomeEvent));
	}

	public void CreateSession()
	{
		SonyNpCommerce.ErrorHandler(Commerce.CreateSession());
	}

	public void OnEnter()
	{
		this.CreateSession();
		Commerce.ShowStoreIcon(1);
	}

	public void OnExit()
	{
		Commerce.HideStoreIcon();
	}

	public void Process(MenuStack stack)
	{
		bool enabled = User.get_IsSignedInPSN() && this.sessionCreated && !Commerce.IsBusy();
		this.menu.Update();
		if (this.menu.AddItem("Category Info", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestCategoryInfo(string.Empty));
		}
		if (this.menu.AddItem("Product List", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestProductList(this.testCategoryID));
		}
		if (this.menu.AddItem("Product Info", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.RequestDetailedProductInfo(this.testProductID));
		}
		if (this.menu.AddItem("Browse Product", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.BrowseProduct(this.testProductID));
		}
		if (this.menu.AddItem("Checkout", enabled))
		{
			Commerce.CommerceProductInfo[] productList = Commerce.GetProductList();
			SonyNpCommerce.ErrorHandler(Commerce.Checkout(this.testProductSkuIDs));
		}
		if (this.menu.AddItem("Redeem Voucher", enabled))
		{
			SonyNpCommerce.ErrorHandler(Commerce.VoucherInput());
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}

	private void OnSessionCreated(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Commerce Session Created");
		this.sessionCreated = true;
	}

	private void OnGotCategoryInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Category Info");
		Commerce.CommerceCategoryInfo categoryInfo = Commerce.GetCategoryInfo();
		OnScreenLog.Add("Category Id: " + categoryInfo.get_categoryId());
		OnScreenLog.Add("Category Name: " + categoryInfo.get_categoryName());
		OnScreenLog.Add("Category num products: " + categoryInfo.countOfProducts);
		OnScreenLog.Add("Category num sub categories: " + categoryInfo.countOfSubCategories);
		for (int i = 0; i < categoryInfo.countOfSubCategories; i++)
		{
			Commerce.CommerceCategoryInfo subCategoryInfo = Commerce.GetSubCategoryInfo(i);
			OnScreenLog.Add("SubCategory Id: " + subCategoryInfo.get_categoryId());
			OnScreenLog.Add("SubCategory Name: " + subCategoryInfo.get_categoryName());
			if (i == 0)
			{
				SonyNpCommerce.ErrorHandler(Commerce.RequestCategoryInfo(subCategoryInfo.get_categoryId()));
			}
		}
	}

	private void OnGotProductList(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Product List");
		Commerce.CommerceProductInfo[] productList = Commerce.GetProductList();
		Commerce.CommerceProductInfo[] array = productList;
		for (int i = 0; i < array.Length; i++)
		{
			Commerce.CommerceProductInfo commerceProductInfo = array[i];
			OnScreenLog.Add("Product: " + commerceProductInfo.get_productName() + " - " + commerceProductInfo.get_price());
		}
	}

	private void OnGotProductInfo(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got Detailed Product Info");
		Commerce.CommerceProductInfoDetailed detailedProductInfo = Commerce.GetDetailedProductInfo();
		OnScreenLog.Add("Product: " + detailedProductInfo.get_productName() + " - " + detailedProductInfo.get_price());
		OnScreenLog.Add("Long desc: " + detailedProductInfo.get_longDescription());
	}
}
