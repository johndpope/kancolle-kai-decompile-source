using Server_Common.Formats.Battle;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DebugBattleMaker
{
	public static string currentDir = "//" + Application.get_streamingAssetsPath() + "/Xml/DebugDatas/";

	public static void SerializeBattleStart()
	{
	}

	public static bool SerializeDayBattle(AllBattleFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = DebugBattleMaker.currentDir + "DayBattleFmt.xml";
		XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
		return DebugBattleMaker.writeBattleFmt(fileName, serializer, fmt);
	}

	public static bool SerializeNightBattle(AllBattleFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = DebugBattleMaker.currentDir + "NightBattleFmt.xml";
		XmlSerializer serializer = new XmlSerializer(typeof(AllBattleFmt));
		return DebugBattleMaker.writeBattleFmt(fileName, serializer, fmt);
	}

	public static bool SerializeBattleResult(BattleResultFmt fmt)
	{
		if (fmt == null)
		{
			return false;
		}
		string fileName = DebugBattleMaker.currentDir + "BattleResultFmt.xml";
		XmlSerializer serializer = new XmlSerializer(typeof(BattleResultFmt));
		return DebugBattleMaker.writeBattleFmt(fileName, serializer, fmt);
	}

	public static void LoadBattleData(out AllBattleFmt day, out AllBattleFmt night, out BattleResultFmt result)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(AllBattleFmt));
		day = null;
		if (File.Exists(DebugBattleMaker.currentDir + "DayBattleFmt.xml"))
		{
			StreamReader streamReader = new StreamReader(DebugBattleMaker.currentDir + "DayBattleFmt.xml");
			day = (AllBattleFmt)xmlSerializer.Deserialize(streamReader);
			streamReader.Close();
		}
		night = null;
		if (File.Exists(DebugBattleMaker.currentDir + "NightBattleFmt.xml"))
		{
			StreamReader streamReader2 = new StreamReader(DebugBattleMaker.currentDir + "NightBattleFmt.xml");
			night = (AllBattleFmt)xmlSerializer.Deserialize(streamReader2);
			streamReader2.Close();
		}
		result = null;
		xmlSerializer = new XmlSerializer(typeof(BattleResultFmt));
		if (File.Exists(DebugBattleMaker.currentDir + "BattleResultFmt.xml"))
		{
			StreamReader streamReader3 = new StreamReader(DebugBattleMaker.currentDir + "BattleResultFmt.xml");
			result = (BattleResultFmt)xmlSerializer.Deserialize(streamReader3);
			streamReader3.Close();
		}
	}

	private static bool writeBattleFmt(string fileName, XmlSerializer serializer, object data)
	{
		return true;
	}
}
