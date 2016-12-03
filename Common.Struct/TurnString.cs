using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.Struct
{
	public struct TurnString
	{
		public string Year;

		public string Month;

		public string Day;

		public string DayOfWeek;

		private readonly DateTimeFormatInfo monthFormat;

		private readonly Dictionary<string, string> yearFormat;

		public TurnString(int elapsed_year, DateTime systemDate)
		{
			this.monthFormat = new CultureInfo("ja-JP").get_DateTimeFormat();
			this.monthFormat.set_MonthNames(new string[]
			{
				"睦月",
				"如月",
				"弥生",
				"卯月",
				"皐月",
				"水無月",
				"文月",
				"葉月",
				"長月",
				"神無月",
				"霜月",
				"師走",
				string.Empty
			});
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("0", "零");
			dictionary.Add("1", "壱");
			dictionary.Add("2", "弐");
			dictionary.Add("3", "参");
			dictionary.Add("4", "肆");
			dictionary.Add("5", "伍");
			dictionary.Add("6", "陸");
			dictionary.Add("7", "質");
			dictionary.Add("8", "捌");
			dictionary.Add("9", "玖");
			dictionary.Add("10", "拾");
			this.yearFormat = dictionary;
			string text = elapsed_year.ToString();
			if (text.get_Length() == 1)
			{
				text = this.yearFormat.get_Item(text);
			}
			else if (text.get_Length() == 2)
			{
				string text2 = string.Empty;
				if (text.get_Chars(0).Equals('1'))
				{
					text2 = this.yearFormat.get_Item("10");
				}
				else
				{
					text2 = this.yearFormat.get_Item(text.get_Chars(0).ToString());
					text2 += this.yearFormat.get_Item("10");
				}
				if (!text.get_Chars(1).Equals('0'))
				{
					text2 += this.yearFormat.get_Item(text.get_Chars(1).ToString());
				}
				text = text2;
			}
			this.Year = text;
			this.Month = systemDate.ToString("MMMM", this.monthFormat);
			this.Day = systemDate.get_Day().ToString();
			this.DayOfWeek = systemDate.ToString("dddd");
		}
	}
}
