using System;
using System.Collections;

public static class RetentionData
{
	private static Hashtable _hData;

	public static void SetData(Hashtable data)
	{
		if (data == null)
		{
			return;
		}
		if (RetentionData._hData != null)
		{
			RetentionData._hData.Clear();
		}
		else
		{
			RetentionData._hData = new Hashtable();
		}
		RetentionData._hData = data;
	}

	public static Hashtable GetData()
	{
		if (RetentionData._hData != null)
		{
			return RetentionData._hData;
		}
		return null;
	}

	public static void Release()
	{
		if (RetentionData._hData != null)
		{
			RetentionData._hData.Clear();
		}
		RetentionData._hData = null;
	}
}
