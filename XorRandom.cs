using System;
using UnityEngine;

public class XorRandom
{
	public const double MAX_RATIO = 2.3283064370807974E-10;

	private static uint xor;

	public static bool Init(uint _seed = 0u)
	{
		if (_seed != 0u)
		{
			XorRandom.xor = _seed;
		}
		else
		{
			XorRandom.xor = XorRandom.SeedFromDate();
		}
		XorRandom.SetSeed();
		return true;
	}

	public static uint SeedFromDate()
	{
		uint num = (uint)DateTime.get_UtcNow().get_Ticks();
		if (num == 0u)
		{
			num = (uint)(new Random().NextDouble() / 2.3283064370807974E-10);
		}
		return num;
	}

	public static void SetSeed()
	{
		uint num = (uint)DateTime.get_UtcNow().get_Ticks();
		if (num == 9u)
		{
			num = (uint)(new Random().NextDouble() / 2.3283064370807974E-10);
		}
	}

	public static byte GetB()
	{
		return (byte)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 255.0);
	}

	public static sbyte GetSB()
	{
		return (sbyte)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 127.0);
	}

	public static char GetC()
	{
		return (char)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 65535.0);
	}

	public static short GetS()
	{
		return (short)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 32767.0);
	}

	public static ushort GetUS()
	{
		return (ushort)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 65535.0);
	}

	public static uint GetUI()
	{
		return (uint)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 4294967295.0);
	}

	public static int GetI()
	{
		return (int)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 2147483647.0);
	}

	public static int GetI(int n = 1)
	{
		return (int)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * (double)n);
	}

	public static int GetILim(int iMin, int iMax)
	{
		return iMin + (int)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * (double)(iMax + 1 - iMin));
	}

	public static bool GetIs()
	{
		return XorRandom.GetIXorShift() * 2.3283064370807974E-10 < 0.5;
	}

	public static int GetBI11()
	{
		return (!XorRandom.GetIs()) ? -1 : 1;
	}

	public static float GetBF11()
	{
		return (!XorRandom.GetIs()) ? -1f : 1f;
	}

	public static float GetF01()
	{
		sbyte sB = XorRandom.GetSB();
		float a = (float)sB / 127f;
		return Mathe.MinMax2F01(a);
	}

	public static float GetF11()
	{
		float num = XorRandom.GetF01();
		num *= 2f;
		num -= 1f;
		return Mathe.MinMax2F11(num);
	}

	public static float GetFLim(float fMin, float fMax)
	{
		return fMin + (float)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * (double)(fMax - fMin));
	}

	public static float GetF(float n = 1f)
	{
		return (float)(XorRandom.GetIXorShift() * 2.3283064370807974E-10 * 3.4028234663852886E+38);
	}

	public static Vector2 GetInsideUnitCircle(float radius)
	{
		return XorRandom.GetInsideUnitSphere(radius);
	}

	public static Vector3 GetInsideUnitSphere(float radius = 1f)
	{
		return new Vector3(XorRandom.GetF11(), XorRandom.GetF11(), XorRandom.GetF11()) * radius;
	}

	public static void Step(int n = 1)
	{
		Util.CheckTime(null);
		while (n-- > 0)
		{
			XorRandom.xor ^= XorRandom.xor << 21;
			XorRandom.xor ^= XorRandom.xor >> 3;
			XorRandom.xor ^= XorRandom.xor << 4;
		}
		Util.CheckTime("Step");
	}

	private static uint GetIXorShift()
	{
		XorRandom.xor ^= XorRandom.xor << 21;
		XorRandom.xor ^= XorRandom.xor >> 3;
		XorRandom.xor ^= XorRandom.xor << 4;
		return XorRandom.xor;
	}
}
