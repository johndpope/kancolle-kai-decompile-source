using System;
using UnityEngine;

namespace LT
{
	public class LeanTest
	{
		public static int expected;

		private static int tests;

		private static int passes;

		public static float timeout = 15f;

		public static bool timeoutStarted;

		public static bool testsFinished;

		public static void debug(string name, bool didPass, string failExplaination = null)
		{
			LeanTest.expect(didPass, name, failExplaination);
		}

		public static void expect(bool didPass, string definition, string failExplaination = null)
		{
			float num = LeanTest.printOutLength(definition);
			int num2 = 40 - (int)(num * 1.05f);
			string text = string.Empty.PadRight(num2, "_".get_Chars(0));
			string text2 = string.Concat(new string[]
			{
				LeanTest.formatB(definition),
				" ",
				text,
				" [ ",
				(!didPass) ? LeanTest.formatC("fail", "red") : LeanTest.formatC("pass", "green"),
				" ]"
			});
			if (!didPass && failExplaination != null)
			{
				text2 = text2 + " - " + failExplaination;
			}
			Debug.Log(text2);
			if (didPass)
			{
				LeanTest.passes++;
			}
			LeanTest.tests++;
			if (LeanTest.tests == LeanTest.expected && !LeanTest.testsFinished)
			{
				LeanTest.overview();
			}
			else if (LeanTest.tests > LeanTest.expected)
			{
				Debug.Log(LeanTest.formatB("Too many tests for a final report!") + " set LeanTest.expected = " + LeanTest.tests);
			}
			if (!LeanTest.timeoutStarted)
			{
				LeanTest.timeoutStarted = true;
				GameObject gameObject = new GameObject();
				gameObject.set_name("~LeanTest");
				LeanTester leanTester = gameObject.AddComponent(typeof(LeanTester)) as LeanTester;
				leanTester.timeout = LeanTest.timeout;
				gameObject.set_hideFlags(61);
			}
		}

		public static string padRight(int len)
		{
			string text = string.Empty;
			for (int i = 0; i < len; i++)
			{
				text += "_";
			}
			return text;
		}

		public static float printOutLength(string str)
		{
			float num = 0f;
			for (int i = 0; i < str.get_Length(); i++)
			{
				if (str.get_Chars(i) == "I".get_Chars(0))
				{
					num += 0.5f;
				}
				else if (str.get_Chars(i) == "J".get_Chars(0))
				{
					num += 0.85f;
				}
				else
				{
					num += 1f;
				}
			}
			return num;
		}

		public static string formatBC(string str, string color)
		{
			return LeanTest.formatC(LeanTest.formatB(str), color);
		}

		public static string formatB(string str)
		{
			return "<b>" + str + "</b>";
		}

		public static string formatC(string str, string color)
		{
			return string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				str,
				"</color>"
			});
		}

		public static void overview()
		{
			LeanTest.testsFinished = true;
			Debug.Log(string.Concat(new string[]
			{
				LeanTest.formatB("Final Report:"),
				" _____________________ PASSED: ",
				LeanTest.formatBC(string.Empty + LeanTest.passes, "green"),
				" FAILED: ",
				LeanTest.formatBC(string.Empty + (LeanTest.expected - LeanTest.passes), "red"),
				" "
			}));
		}
	}
}
