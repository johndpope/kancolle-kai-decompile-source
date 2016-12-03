using System;

public class Dlg
{
	public static void Call(Action callback)
	{
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	public static void Call(ref Action callback)
	{
		if (callback != null)
		{
			callback.Invoke();
		}
	}

	public static void Call<T>(Action<T> callback, T val)
	{
		if (callback != null)
		{
			callback.Invoke(val);
		}
	}

	public static void Call<T1>(ref Action<T1> callback, T1 arg1)
	{
		if (callback != null)
		{
			callback.Invoke(arg1);
		}
	}

	public static void Call<T1, T2>(ref Action<T1, T2> callback, T1 arg1, T2 arg2)
	{
		if (callback != null)
		{
			callback.Invoke(arg1, arg2);
		}
	}

	public static void Call<T1, T2, T3>(ref Action<T1, T2, T3> callback, T1 arg1, T2 arg2, T3 arg3)
	{
		if (callback != null)
		{
			callback.Invoke(arg1, arg2, arg3);
		}
	}

	public static void Call<T1, T2, T3, T4>(ref Action<T1, T2, T3, T4> callback, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		if (callback != null)
		{
			callback.Invoke(arg1, arg2, arg3, arg4);
		}
	}

	public static bool Call<T>(ref Predicate<T> callback, T val)
	{
		return callback != null && callback.Invoke(val);
	}
}
