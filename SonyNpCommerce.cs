using Sony.NP;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpCommerce : IScreen
{
	private MenuLayout menu;

	private SonyNpCommerceEntitlements entitlements;

	private SonyNpCommerceStore store;

	private SonyNpCommerceInGameStore inGameStore;

	public SonyNpCommerce()
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
		this.store = new SonyNpCommerceStore();
		this.inGameStore = new SonyNpCommerceInGameStore();
		this.entitlements = new SonyNpCommerceEntitlements();
		Commerce.add_OnError(new Messages.EventHandler(this.OnCommerceError));
		Commerce.add_OnDownloadListStarted(new Messages.EventHandler(this.OnSomeEvent));
		Commerce.add_OnDownloadListFinished(new Messages.EventHandler(this.OnSomeEvent));
	}

	public void OnEnter()
	{
	}

	public void OnExit()
	{
	}

	public static ErrorCode ErrorHandler(ErrorCode errorCode = 3)
	{
		if (errorCode != null)
		{
			ResultCode resultCode = default(ResultCode);
			Commerce.GetLastError(ref resultCode);
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
		bool enabled = User.get_IsSignedInPSN() && !Commerce.IsBusy();
		this.menu.Update();
		if (this.menu.AddItem("Store", enabled))
		{
			stack.PushMenu(this.store.GetMenu());
		}
		if (this.menu.AddItem("In Game Store", true))
		{
			stack.PushMenu(this.inGameStore.GetMenu());
		}
		if (this.menu.AddItem("Downloads", true))
		{
			Commerce.DisplayDownloadList();
		}
		if (this.menu.AddItem("Entitlements", true))
		{
			stack.PushMenu(this.entitlements.GetMenu());
		}
		if (this.menu.AddItem("Find Installed Content", true))
		{
			this.EnumerateDRMContent();
		}
		if (this.menu.AddBackIndex("Back", true))
		{
			stack.PopMenu();
		}
	}

	private int EnumerateDRMContentFiles(string contentDir)
	{
		int num = 0;
		PSVitaDRM.ContentOpen(contentDir);
		string text = "addcont0:" + contentDir;
		OnScreenLog.Add("Found content folder: " + text);
		string[] files = Directory.GetFiles(text);
		OnScreenLog.Add(" containing " + files.Length + " files");
		string[] array = files;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			OnScreenLog.Add("  " + text2);
			num++;
			if (text2.Contains(".unity3d"))
			{
				AssetBundle assetBundle = AssetBundle.CreateFromFile(text2);
				Object[] array2 = assetBundle.LoadAllAssets();
				OnScreenLog.Add("  Loaded " + array2.Length + " assets from asset bundle.");
				assetBundle.Unload(false);
			}
		}
		PSVitaDRM.ContentClose(contentDir);
		return num;
	}

	private void EnumerateDRMContent()
	{
		int num = 0;
		PSVitaDRM.DrmContentFinder drmContentFinder = default(PSVitaDRM.DrmContentFinder);
		drmContentFinder.dirHandle = -1;
		if (PSVitaDRM.ContentFinderOpen(ref drmContentFinder))
		{
			num += this.EnumerateDRMContentFiles(drmContentFinder.get_contentDir());
			while (PSVitaDRM.ContentFinderNext(ref drmContentFinder))
			{
				num += this.EnumerateDRMContentFiles(drmContentFinder.get_contentDir());
			}
			PSVitaDRM.ContentFinderClose(ref drmContentFinder);
		}
		OnScreenLog.Add("Found " + num + " files in installed DRM content");
	}

	private void OnCommerceError(Messages.PluginMessage msg)
	{
		SonyNpCommerce.ErrorHandler(3);
	}

	private void OnSomeEvent(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Event: " + msg.type);
	}
}
