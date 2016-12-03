using System;
using System.ComponentModel;

public static class EnumExtensions
{
	public static bool TryParse<T>(this Enum theEnum, string valueToParse, out T returnValue)
	{
		returnValue = default(T);
		if (Enum.IsDefined(typeof(T), valueToParse))
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			returnValue = (T)((object)converter.ConvertFromString(valueToParse));
			return true;
		}
		return false;
	}

	public static bool HasFlag(this Enum self, Enum flag)
	{
		if (self.GetType() != flag.GetType())
		{
			throw new ArgumentException("flag の型が、現在のインスタンスの型と異なっています。");
		}
		ulong num = Convert.ToUInt64(self);
		ulong num2 = Convert.ToUInt64(flag);
		return (num & num2) == num;
	}
}
