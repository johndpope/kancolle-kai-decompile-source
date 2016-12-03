using Sony.NP;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.PSVita;

public class SonyNpCloudTUS : IScreen
{
	private enum TUSDataRequestType
	{
		None,
		SaveRawData,
		LoadRawData,
		SavePlayerPrefs,
		LoadPlayerPrefs
	}

	private const int kTUS_DataSlot_RawData = 1;

	private const int kTUS_DataSlot_PlayerPrefs = 3;

	private MenuLayout menuTus;

	private string virtualUserOnlineID = "_ERGVirtualUser1";

	private SonyNpCloudTUS.TUSDataRequestType m_TUSDataRequestType;

	public SonyNpCloudTUS()
	{
		this.Initialize();
	}

	public MenuLayout GetMenu()
	{
		return this.menuTus;
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
			TusTss.GetLastError(ref resultCode);
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
		this.MenuTus(stack);
	}

	public void Initialize()
	{
		this.menuTus = new MenuLayout(this, 550, 34);
		TusTss.add_OnTusDataSet(new Messages.EventHandler(this.OnSetTusData));
		TusTss.add_OnTusDataRecieved(new Messages.EventHandler(this.OnGotTusData));
		TusTss.add_OnTusVariablesSet(new Messages.EventHandler(this.OnSetTusVariables));
		TusTss.add_OnTusVariablesModified(new Messages.EventHandler(this.OnModifiedTusVariables));
		TusTss.add_OnTusVariablesRecieved(new Messages.EventHandler(this.OnGotTusVariables));
		TusTss.add_OnTusTssError(new Messages.EventHandler(this.OnTusTssError));
	}

	public void MenuTus(MenuStack menuStack)
	{
		this.menuTus.Update();
		bool enabled = User.get_IsSignedInPSN() && !TusTss.IsTusBusy();
		if (this.menuTus.AddItem("TUS Set Data", enabled))
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = (byte)(3 - i);
			}
			OnScreenLog.Add(" Data size: " + array.Length);
			string text = string.Empty;
			int num = 0;
			while (num < 16 && num < array.Length)
			{
				text = text + array[num].ToString() + ", ";
				num++;
			}
			OnScreenLog.Add(" Data: " + text);
			this.m_TUSDataRequestType = SonyNpCloudTUS.TUSDataRequestType.SaveRawData;
			this.ErrorHandler(TusTss.SetTusData(1, array));
		}
		if (this.menuTus.AddItem("TUS Request Data", enabled))
		{
			this.m_TUSDataRequestType = SonyNpCloudTUS.TUSDataRequestType.LoadRawData;
			this.ErrorHandler(TusTss.RequestTusData(1));
		}
		if (this.menuTus.AddItem("TUS Save PlayerPrefs", enabled))
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt("keyA", 1);
			PlayerPrefs.SetString("keyB", "Hello");
			PlayerPrefs.SetInt("keyC", 3);
			PlayerPrefs.SetInt("keyD", 4);
			byte[] array2 = PSVitaPlayerPrefs.SaveToByteArray();
			this.m_TUSDataRequestType = SonyNpCloudTUS.TUSDataRequestType.SavePlayerPrefs;
			this.ErrorHandler(TusTss.SetTusData(3, array2));
		}
		if (this.menuTus.AddItem("TUS Load PlayerPrefs", enabled))
		{
			this.m_TUSDataRequestType = SonyNpCloudTUS.TUSDataRequestType.LoadPlayerPrefs;
			this.ErrorHandler(TusTss.RequestTusData(3));
		}
		if (this.menuTus.AddItem("TUS Set Variables", enabled))
		{
			this.ErrorHandler(TusTss.SetTusVariables(new TusTss.TusVariable[]
			{
				new TusTss.TusVariable(1, 110L),
				new TusTss.TusVariable(2, 220L),
				new TusTss.TusVariable(3, 330L),
				new TusTss.TusVariable(4, 440L)
			}));
		}
		if (this.menuTus.AddItem("TUS Request Variables", enabled))
		{
			int[] array3 = new int[]
			{
				1,
				2,
				3,
				4
			};
			this.ErrorHandler(TusTss.RequestTusVariables(array3));
		}
		if (this.menuTus.AddItem("TUS Set Variables VU", enabled))
		{
			TusTss.TusVariable[] array4 = new TusTss.TusVariable[]
			{
				new TusTss.TusVariable(5, 12345L)
			};
			this.ErrorHandler(TusTss.SetTusVariablesForVirtualUser(this.virtualUserOnlineID, array4));
		}
		if (this.menuTus.AddItem("TUS Request Variables VU", enabled))
		{
			int[] array5 = new int[]
			{
				5
			};
			this.ErrorHandler(TusTss.RequestTusVariablesForVirtualUser(this.virtualUserOnlineID, array5));
		}
		if (this.menuTus.AddItem("TUS Modify Variables VU", enabled))
		{
			TusTss.TusVariable[] array6 = new TusTss.TusVariable[]
			{
				new TusTss.TusVariable(5, 1L)
			};
			this.ErrorHandler(TusTss.ModifyTusVariablesForVirtualUser(this.virtualUserOnlineID, array6));
		}
		if (this.menuTus.AddBackIndex("Back", true))
		{
			menuStack.PopMenu();
		}
	}

	private void OnTusTssError(Messages.PluginMessage msg)
	{
		this.ErrorHandler(3);
	}

	private void OnSetTusData(Messages.PluginMessage msg)
	{
		switch (this.m_TUSDataRequestType)
		{
		case SonyNpCloudTUS.TUSDataRequestType.SaveRawData:
			OnScreenLog.Add("Sent data to TUS");
			break;
		case SonyNpCloudTUS.TUSDataRequestType.SavePlayerPrefs:
			OnScreenLog.Add("Sent PlayerPrefs to TUS");
			break;
		}
	}

	private void OnGotTusData(Messages.PluginMessage msg)
	{
		switch (this.m_TUSDataRequestType)
		{
		case SonyNpCloudTUS.TUSDataRequestType.LoadRawData:
		{
			OnScreenLog.Add("Got TUS Data");
			byte[] tusData = TusTss.GetTusData();
			OnScreenLog.Add(" Data size: " + tusData.Length);
			string text = string.Empty;
			int num = 0;
			while (num < 16 && num < tusData.Length)
			{
				text = text + tusData[num].ToString() + ", ";
				num++;
			}
			OnScreenLog.Add(" Data: " + text);
			break;
		}
		case SonyNpCloudTUS.TUSDataRequestType.LoadPlayerPrefs:
		{
			OnScreenLog.Add("Got PlayerPrefs from TUS...");
			byte[] tusData = TusTss.GetTusData();
			PSVitaPlayerPrefs.LoadFromByteArray(tusData);
			OnScreenLog.Add(" keyA = " + PlayerPrefs.GetInt("keyA"));
			OnScreenLog.Add(" keyB = " + PlayerPrefs.GetString("keyB"));
			OnScreenLog.Add(" keyC = " + PlayerPrefs.GetInt("keyC"));
			OnScreenLog.Add(" keyD = " + PlayerPrefs.GetInt("keyD"));
			break;
		}
		}
	}

	private void OnSetTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Sent TUS variables");
	}

	private void OnGotTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Got TUS Variables");
		TusTss.TusRetrievedVariable[] tusVariables = TusTss.GetTusVariables();
		for (int i = 0; i < tusVariables.Length; i++)
		{
			string @string = Encoding.get_Default().GetString(tusVariables[i].get_ownerNpID());
			string string2 = Encoding.get_Default().GetString(tusVariables[i].get_lastChangeAuthorNpID());
			DateTime dateTime = new DateTime(tusVariables[i].lastChangedDate, 1);
			OnScreenLog.Add(" HasData: " + tusVariables[i].hasData);
			OnScreenLog.Add(" Value: " + tusVariables[i].variable);
			OnScreenLog.Add(" OwnerNpID: " + @string);
			OnScreenLog.Add(" lastChangeNpID: " + string2);
			OnScreenLog.Add(" lastChangeTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
	}

	private void OnModifiedTusVariables(Messages.PluginMessage msg)
	{
		OnScreenLog.Add("Modified TUS Variables");
		TusTss.TusRetrievedVariable[] tusVariables = TusTss.GetTusVariables();
		for (int i = 0; i < tusVariables.Length; i++)
		{
			string @string = Encoding.get_Default().GetString(tusVariables[i].get_ownerNpID());
			string string2 = Encoding.get_Default().GetString(tusVariables[i].get_lastChangeAuthorNpID());
			DateTime dateTime = new DateTime(tusVariables[i].lastChangedDate, 1);
			OnScreenLog.Add(" HasData: " + tusVariables[i].hasData);
			OnScreenLog.Add(" Value: " + tusVariables[i].variable);
			OnScreenLog.Add(" OwnerNpID: " + @string);
			OnScreenLog.Add(" lastChangeNpID: " + string2);
			OnScreenLog.Add(" lastChangeTime: " + dateTime.ToLongDateString() + " - " + dateTime.ToLongTimeString());
		}
	}
}
