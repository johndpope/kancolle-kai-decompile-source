using System;

public static class Rand
{
	private struct STR_RAND_WRK
	{
		public uint iSeed;

		public ulong iXorShiftX;

		public ulong iXorShiftY;

		public ulong iXorShiftZ;

		public ulong iXorShiftW;

		public override string ToString()
		{
			return string.Format("STR_RAND_WRK->x(x.{0},y.{1},z.{2},w.{3})", new object[]
			{
				this.iXorShiftX,
				this.iXorShiftY,
				this.iXorShiftZ,
				this.iXorShiftW
			});
		}
	}

	private static int _nWrk;

	private static Rand.STR_RAND_WRK[] _pWrk;

	public static bool Init(int nWrk = 1)
	{
		Rand._nWrk = nWrk;
		Mem.NewAry<Rand.STR_RAND_WRK>(ref Rand._pWrk, nWrk);
		ulong ticks = (ulong)DateTime.get_Now().get_Ticks();
		for (int i = 0; i < nWrk; i++)
		{
			Rand.SetSeed((uint)ticks, i);
		}
		return true;
	}

	public static bool UnInit()
	{
		Mem.DelAry<Rand.STR_RAND_WRK>(ref Rand._pWrk);
		Rand._nWrk = 0;
		return true;
	}

	public static void SetSeed(uint iSeed, int iWrk = 0)
	{
		Rand._pWrk[iWrk].iSeed = iSeed;
		Rand._pWrk[iWrk].iXorShiftW = (ulong)Rand._pWrk[iWrk].iSeed;
		Rand._pWrk[iWrk].iXorShiftX = (Rand._pWrk[iWrk].iXorShiftW << 8 | Rand._pWrk[iWrk].iXorShiftW >> 8 | 12345678uL);
		Rand._pWrk[iWrk].iXorShiftY = (Rand._pWrk[iWrk].iXorShiftX & Rand._pWrk[iWrk].iXorShiftW);
		Rand._pWrk[iWrk].iXorShiftZ = (Rand._pWrk[iWrk].iXorShiftX ^ Rand._pWrk[iWrk].iXorShiftY);
	}

	public static uint GetSeed(int iWrk = 0)
	{
		return Rand._pWrk[iWrk].iSeed;
	}

	public static int Next()
	{
		return (int)Rand.GetIXorShift(0) & 2147483647;
	}

	public static int Next(int max)
	{
		return Rand.Next(0, max);
	}

	public static int Next(int min, int max)
	{
		return (Rand.Next(0) >> 1) % (max - min) + min;
	}

	public static byte GetB(int iWrk = 0)
	{
		return (byte)Rand.GetIXorShift(iWrk) & 255;
	}

	public static sbyte GetSB(int iWrk = 0)
	{
		return (sbyte)((int)((sbyte)Rand.GetIXorShift(iWrk)) & 127);
	}

	public static ulong GetC(int iWrk = 0)
	{
		return Rand.GetIXorShift(iWrk) & 65535uL;
	}

	public static short GetS(int iWrk = 0)
	{
		return (short)Rand.GetIXorShift(iWrk) & 32767;
	}

	public static ushort GetUS(int iWrk = 0)
	{
		return (ushort)(Rand.GetIXorShift(iWrk) & 65535uL);
	}

	public static uint GetUI(int iWrk = 0)
	{
		return (uint)(Rand.GetIXorShift(iWrk) & (ulong)-1);
	}

	public static int GetI(int iWrk = 0)
	{
		return (int)Rand.GetIXorShift(iWrk) & 2147483647;
	}

	public static int GetILim(int iMin, int iMax, int iWrk = 0)
	{
		float f = Rand.GetF01(iWrk);
		int a = (int)Mathe.Lerp((float)iMin, (float)iMax, f);
		return Mathe.MinMax2(a, iMin, iMax);
	}

	public static bool GetIs(int iWrk = 0)
	{
		return (int)Rand.GetSB(iWrk) % 2 == 1;
	}

	public static int GetBI11(int iWrk = 0)
	{
		return (!Rand.GetIs(iWrk)) ? -1 : 1;
	}

	public static float GetBF11(int iWrk = 0)
	{
		return (!Rand.GetIs(iWrk)) ? -1f : 1f;
	}

	public static float GetF01(int iWrk = 0)
	{
		sbyte sB = Rand.GetSB(iWrk);
		float a = (float)sB / 127f;
		return Mathe.MinMax2F01(a);
	}

	public static float GetF11(int iWrk = 0)
	{
		float num = Rand.GetF01(iWrk);
		num *= 2f;
		num -= 1f;
		return Mathe.MinMax2F11(num);
	}

	public static float GetFLim(float fMin, float fMax, int iWrk = 0)
	{
		float num = Rand.GetF01(iWrk);
		num = Mathe.Lerp(fMin, fMax, num);
		return Mathe.MinMax2(num, fMin, fMax);
	}

	public static ulong GetIXorShift(int iWrk = 0)
	{
		ulong num = Rand._pWrk[iWrk].iXorShiftX ^ Rand._pWrk[iWrk].iXorShiftX << 11;
		Rand._pWrk[iWrk].iXorShiftX = Rand._pWrk[iWrk].iXorShiftY;
		Rand._pWrk[iWrk].iXorShiftZ = Rand._pWrk[iWrk].iXorShiftW;
		Rand._pWrk[iWrk].iXorShiftW = (Rand._pWrk[iWrk].iXorShiftW - (Rand._pWrk[iWrk].iXorShiftW >> 19) ^ (num ^ num >> 8));
		return Rand._pWrk[iWrk].iXorShiftW;
	}

	private static Rand.STR_RAND_WRK GetWrk(int iWrk)
	{
		return Rand._pWrk[iWrk];
	}
}
