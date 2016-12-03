using GenericOperator;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Mathe
{
	public const long siEXA = 1000000000000000000L;

	public const long siPETA = 1000000000000000L;

	public const long siTERA = 1000000000000L;

	public const long siGIGA = 1000000000L;

	public const long siMEGA = 1000000L;

	public const long siKILO = 1000L;

	public const long siHECTO = 100L;

	public const long siDECA = 10L;

	public const double siDECI = 0.10000000149011612;

	public const double siCENTI = 0.0099999997764825821;

	public const double siMILLI = 0.0010000000474974513;

	public const double siMICRO = 9.9999999747524271E-07;

	public const double siNANO = 9.9999997171806854E-10;

	public const double siPICO = 9.999999960041972E-13;

	public const double siFEMT = 1.0000000036274937E-15;

	public const double siATTO = 1.000000045813705E-18;

	public const double siZEPTO = 9.9999996826552254E-22;

	public const double siYPCTO = 1.0000000195414814E-24;

	public const long bnKIBI = 1024L;

	public const long bnMEBI = 1048576L;

	public const long bnGIBI = 1073741824L;

	public const long bnTEBI = 1099511627776L;

	public const long bnPEBI = 1125899906842624L;

	public const long bnEXBI = 1152921504606846976L;

	public const float GravityAcc = 9.80665f;

	public const long SpeedOfLight = 299792458L;

	public const float SpeedOfSound = 331.5f;

	public const float AbsoluteZero = -273.15f;

	public const float Pi = 3.14159274f;

	public const float PiMul2 = 6.28318548f;

	public const float PiMul3 = 9.424778f;

	public const float PiMul4 = 12.566371f;

	public const float PiMul8 = 25.1327419f;

	public const float PiDiv2 = 1.57079637f;

	public const float PiDiv3 = 1.04719758f;

	public const float PiDiv4 = 0.7853982f;

	public const float PiDiv8 = 0.3926991f;

	public const float PiDiv180 = 0.0174532924f;

	public const float PiDiv360 = 0.008726646f;

	public const float DivPiMul2 = 0.159154937f;

	public const float DivPi = 0.318309873f;

	public const float Mul180DivPi = 57.29578f;

	public static float Infinity
	{
		get
		{
			return float.PositiveInfinity;
		}
	}

	public static float NegativeInfinity
	{
		get
		{
			return float.NegativeInfinity;
		}
	}

	public static uint BitTypeSizByt
	{
		get
		{
			return 4u;
		}
	}

	public static uint BitTypeNumBit
	{
		get
		{
			return Mathe.BitTypeSizByt << 3;
		}
	}

	public static float PhyForce(float m, float a)
	{
		return m * a;
	}

	public static float PhyUniMot(float v, float t)
	{
		return v * t;
	}

	public static float PhyUniAccMotV(float vo, float a, float t)
	{
		return vo + a * t;
	}

	public static float PhyUniAccMotS(float vo, float a, float t)
	{
		return vo * t + a * (float)Mathe.Pow((double)t, 2.0) / 2f;
	}

	public static float PhyElasticForce(float k, float x)
	{
		return -(k * x);
	}

	public static float PhyBuoyancy(float p, float v, float g)
	{
		return p * v * g;
	}

	public static float PhyFrictionForce(float u, float n)
	{
		return u * n;
	}

	public static Vector2 PhyCenterOfGravity(Vector2 pos1, Vector2 pos2, float m1, float m2)
	{
		Vector2 result;
		result.x = (m1 * pos1.x + m2 * pos2.x) / (m1 + m2);
		result.y = (m1 * pos1.y + m2 * pos2.y) / (m1 + m2);
		return result;
	}

	public static Vector3 PhyCenterOfGravity(Vector3 pos1, Vector3 pos2, float m1, float m2)
	{
		Vector3 result;
		result.x = (m1 * pos1.x + m2 * pos2.x) / (m1 + m2);
		result.y = Vector3.get_zero().y;
		result.z = (m1 * pos1.z + m2 * pos2.z) / (m1 + m2);
		return result;
	}

	public static float SpeedofSound(float celsius)
	{
		return 331.5f + 0.61f * celsius;
	}

	public static float Abs(float f)
	{
		return (f >= 0f) ? f : (-f);
	}

	public static float Epsilon()
	{
		return 1.401298E-45f;
	}

	public static T Add<T>(T a, T b)
	{
		return Operator<T>.Add.Invoke(a, b);
	}

	public static Vector3 Add(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static T Sub<T>(T a, T b)
	{
		return Operator<T>.Subtract.Invoke(a, b);
	}

	public static Vector3 Sub(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static T Mul<T>(T a, T b)
	{
		return Operator<T>.Multiply.Invoke(a, b);
	}

	public static T Div<T>(T a, T b)
	{
		return Operator<T>.Divide.Invoke(a, b);
	}

	public static T Pul<T>(T a)
	{
		return Operator<T>.Plus.Invoke(a);
	}

	public static int Sum(List<int> a)
	{
		int num = 0;
		using (List<int>.Enumerator enumerator = a.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.get_Current();
				num += current;
			}
		}
		return num;
	}

	public static int Sum(params int[] a)
	{
		int num = 0;
		for (int i = 0; i < a.Length; i++)
		{
			int num2 = a[i];
			num += num2;
		}
		return num;
	}

	public static int Min2(int a, int b)
	{
		return (a >= b) ? b : a;
	}

	public static float Min2(float a, float b)
	{
		return (a >= b) ? b : a;
	}

	public static int Max2(int a, int b)
	{
		return (a <= b) ? b : a;
	}

	public static float Max2(float a, float b)
	{
		return (a <= b) ? b : a;
	}

	public static int MinMax2(int a, int mn, int mx)
	{
		return Mathe.Min2(Mathe.Max2(a, mn), mx);
	}

	public static float MinMax2(float a, float mn, float mx)
	{
		return Mathe.Min2(Mathe.Max2(a, mn), mx);
	}

	public static int MinMax2Rev(int a, int mn, int mx)
	{
		if (a < mn)
		{
			return mx;
		}
		if (a > mx)
		{
			return mn;
		}
		return a;
	}

	public static float MinMax2Rev(float a, float mn, float mx)
	{
		if (a < mn)
		{
			return mx;
		}
		if (a > mx)
		{
			return mn;
		}
		return a;
	}

	public static float MinMax2F01(float a)
	{
		return Mathe.MinMax2(a, 0f, 1f);
	}

	public static float MinMax2F11(float a)
	{
		return Mathe.MinMax2(a, -1f, 1f);
	}

	public static float Min3(int a, int b, int c)
	{
		return (float)Mathe.Min2(Mathe.Min2(a, b), c);
	}

	public static float Min3(float a, float b, float c)
	{
		return Mathe.Min2(Mathe.Min2(a, b), c);
	}

	public static int Max3(int a, int b, int c)
	{
		return Mathe.Max2(Mathe.Max2(a, b), c);
	}

	public static float Max3(float a, float b, float c)
	{
		return Mathe.Max2(Mathe.Max2(a, b), c);
	}

	public static int ClipMin2(int a, int mn, int mx)
	{
		return (a >= mn) ? a : mx;
	}

	public static float ClipMin2(float a, float mn, float mx)
	{
		return (a >= mn) ? a : mx;
	}

	public static int ClipMax2(int a, int mn, int mx)
	{
		return (a <= mx) ? a : mn;
	}

	public static float ClipMax2(float a, float mn, float mx)
	{
		return (a <= mx) ? a : mn;
	}

	public static int Clip(int a, int mn, int mx)
	{
		int num = (a <= mx) ? a : mn;
		return (a >= mn) ? num : mx;
	}

	public static float Clip(float a, float mn, float mx)
	{
		float num = (a <= mx) ? a : mn;
		return (a >= mn) ? num : mx;
	}

	public static Vector3 Vec2ToVec3XY(Vector2 vec)
	{
		return new Vector3(vec.x, vec.y, 0f);
	}

	public static Vector3 Vec2ToVec3XZ(Vector2 vec)
	{
		return new Vector3(vec.x, 0f, vec.y);
	}

	public static Vector4 QuaToVec4(Quaternion rot)
	{
		return new Vector4(rot.x, rot.y, rot.z, rot.w);
	}

	public static Quaternion Vec4ToQua(Vector4 rot)
	{
		return new Quaternion(rot.x, rot.y, rot.z, rot.w);
	}

	public static float Euler2Deg(float eulerAngle)
	{
		float num = eulerAngle;
		if (Math.Sign(num) == -1)
		{
			num = ((num != -0f) ? (num + 360f) : 0f);
		}
		return num;
	}

	public static float Deg2Rad(float fDeg)
	{
		return fDeg * 0.0174532924f;
	}

	public static float Rad2Deg(float fRad)
	{
		return fRad * 57.29578f;
	}

	public static float PiLimF01(float a)
	{
		return Mathe.MinMax2(a, 0f, 3.14159274f);
	}

	public static float PiLimF02(float a)
	{
		return Mathe.MinMax2(a, 0f, 6.28318548f);
	}

	public static float PiLimF11(float a)
	{
		return Mathe.MinMax2(a, -3.14159274f, 3.14159274f);
	}

	public static float PiClip(float r)
	{
		float num = (r >= 0f) ? 3.14159274f : -3.14159274f;
		int num2 = (int)((r + num) / 6.28318548f);
		return r - (float)num2 * 6.28318548f;
	}

	public static float PiLengthMin(float r0, float r1)
	{
		float num = r1 - r0;
		num = Mathe.PiClip(num);
		float num2 = Mathe.Abs(num);
		if (num2 >= 3.14159274f)
		{
			num2 = 6.28318548f - num2;
		}
		return num2;
	}

	public static float PiLengthMax(float r0, float r1)
	{
		return 6.28318548f - Mathe.PiLengthMin(r0, r1);
	}

	public static float PiLerp(float r0, float r1, float t)
	{
		float r2 = r1 - r0;
		float num = Mathe.PiClip(r2);
		num += t;
		float r3 = r0 + num;
		return Mathe.PiClip(r3);
	}

	public static float PiLimRng(float rsrc, float raim, float frng)
	{
		if (Mathe.PiLengthMin(rsrc, raim) <= frng)
		{
			return raim;
		}
		float num = Mathe.PiClip(rsrc - frng);
		float num2 = Mathe.PiClip(rsrc + frng);
		if (Mathe.PiLengthMin(num, num2) < Mathe.PiLengthMin(num2, raim))
		{
			return num;
		}
		return num2;
	}

	public static float Rate(float mn, float mx, float nw)
	{
		return (nw - mn) / (mx - mn);
	}

	public static float Lerp(float a0, float a1, float t)
	{
		return a1 * t + a0 * (1f - t);
	}

	public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
	{
		return (value - inputMin) * ((outputMax - outputMin) / (inputMax - inputMin)) + outputMin;
	}

	public static uint BitAryNumCalc(int iArg, int nBit)
	{
		return (uint)((iArg + (nBit - 1)) / nBit);
	}

	public static uint BitAryNum(int iArg)
	{
		return Mathe.BitAryNumCalc(iArg, (int)Mathe.BitTypeNumBit);
	}

	public static byte Byte2KByteI(int by)
	{
		int num = by >> 10;
		return (byte)num;
	}

	public static byte Byte2MByteI(int by)
	{
		int num = (int)Mathe.Byte2KByteI(by);
		return (byte)num;
	}

	public static byte Byte2GByteI(int by)
	{
		int num = (int)Mathe.Byte2MByteI(by);
		return (byte)num;
	}

	public static byte KByte2ByteI(int kbyte)
	{
		int num = kbyte << 10;
		return (byte)num;
	}

	public static byte MByte2ByteI(int Mbyte)
	{
		int num = (int)Mathe.KByte2ByteI(Mbyte) << 10;
		return (byte)num;
	}

	public static byte GByte2ByteI(int Gbyte)
	{
		int num = (int)Mathe.MByte2ByteI(Gbyte) << 10;
		return (byte)num;
	}

	public static float KByte2ByteF(float Kbyte)
	{
		return Kbyte * 1024f;
	}

	public static float MByte2ByteF(float Mbyte)
	{
		return Mathe.KByte2ByteF(Mbyte * 1024f);
	}

	public static float GByte2ByteF(float Gbyte)
	{
		return Mathe.MByte2ByteF(Gbyte * 1024f);
	}

	public static double Pow(double f1, double f2)
	{
		return Math.Pow(f1, f2);
	}

	public static double Pow2(double f1)
	{
		return f1 * f1;
	}

	public static double Pow3(double f1)
	{
		return f1 * f1 * f1;
	}

	public static double Pow4(double f1)
	{
		return f1 * f1 * f1 * f1;
	}

	public static uint Factorial(uint n)
	{
		if (n > 1u)
		{
			return n * Mathe.Factorial(n - 1u);
		}
		if (n == 1u)
		{
			return 1u;
		}
		return 0u;
	}

	public static float Frame2Sec(int frame, float fps = 60f)
	{
		return 1f / fps * (float)frame;
	}

	public static Vector3 Direction(Vector3 from, Vector3 to)
	{
		return to - from;
	}

	public static Vector2 Direction(Vector2 from, Vector2 to)
	{
		return to - from;
	}

	public static Vector3 NormalizeDirection(Vector3 from, Vector3 to)
	{
		return Mathe.Direction(from, to).get_normalized();
	}

	public static Vector2 NormalizeDirection(Vector2 from, Vector2 to)
	{
		return Mathe.Direction(from, to).get_normalized();
	}

	public static float Hermite(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, t * t * (3f - 2f * t));
	}

	public static float Sinerp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(t * 3.14159274f * 0.5f));
	}

	public static float Coserp(float start, float end, float t)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(t * 3.14159274f * 0.5f));
	}

	public static float Berp(float start, float end, float t)
	{
		t = Mathf.Clamp01(t);
		t = Mathf.Sin(t * 3.14159274f + (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t;
		return start + (end - start) * t;
	}

	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	public static float Lerp_(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
		return lineStart + num * vector;
	}

	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.Normalize(vector);
		float num = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f)) * (1f - x));
	}

	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return (val - about).get_sqrMagnitude() < range * range;
	}

	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	public static int NextElement(int now, int min, int max, bool isFoward)
	{
		int num = (!isFoward) ? -1 : 1;
		now += num;
		now = Mathe.MinMax2(now, min, max);
		return now;
	}

	public static void NextElement(ref int now, int min, int max, bool isFoward)
	{
		int num = (!isFoward) ? -1 : 1;
		now += num;
		now = Mathe.MinMax2(now, min, max);
	}

	public static int NextElement(int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = (!isFoward) ? -1 : 1;
		int num2 = now + num;
		num2 = Mathe.MinMax2(num2, min, max);
		while (!notConditions.Invoke(num2))
		{
			if (num2 == min || num2 == max)
			{
				num2 = (notConditions.Invoke(num2) ? num2 : now);
				break;
			}
			num2 += num;
			num2 = Mathe.MinMax2(num2, min, max);
		}
		return num2;
	}

	public static void NextElement(ref int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = (!isFoward) ? -1 : 1;
		int num2 = now;
		num2 += num;
		num2 = Mathe.MinMax2(num2, min, max);
		while (!notConditions.Invoke(num2))
		{
			if (num2 == min || num2 == max)
			{
				num2 = (notConditions.Invoke(num2) ? num2 : now);
				break;
			}
			num2 += num;
			num2 = Mathe.MinMax2(num2, min, max);
		}
		now = num2;
	}

	public static int NextElementRev(int now, int min, int max, bool isFoward)
	{
		int num = (!isFoward) ? -1 : 1;
		now += num;
		now = Mathe.MinMax2Rev(now, min, max);
		return now;
	}

	public static void NextElementRev(ref int now, int min, int max, bool isFoward)
	{
		int num = (!isFoward) ? -1 : 1;
		now += num;
		now = Mathe.MinMax2Rev(now, min, max);
	}

	public static int NextElementRev(int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = (!isFoward) ? -1 : 1;
		int num2 = now + num;
		num2 = Mathe.MinMax2Rev(num2, min, max);
		while (!notConditions.Invoke(num2))
		{
			num2 += num;
			num2 = Mathe.MinMax2Rev(num2, min, max);
		}
		return num2;
	}

	public static void NextElementRev(ref int now, int min, int max, bool isFoward, Predicate<int> notConditions)
	{
		int num = (!isFoward) ? -1 : 1;
		int num2 = now;
		num2 += num;
		num2 = Mathe.MinMax2Rev(num2, min, max);
		List<bool> list = new List<bool>(max);
		while (!notConditions.Invoke(num2))
		{
			num2 += num;
			num2 = Mathe.MinMax2Rev(num2, min, max);
		}
		now = num2;
	}

	public static Vector2[] RegularPolygonVertices(int verNum, float radius, float angle)
	{
		Vector2[] array = new Vector2[verNum];
		float num = 6.28318548f / (float)verNum;
		for (int i = 0; i < verNum; i++)
		{
			float num2 = Mathf.Cos(num * (float)i + Mathe.Deg2Rad(angle)) * radius;
			float num3 = Mathf.Sin(num * (float)i + Mathe.Deg2Rad(angle)) * radius;
			array[i] = new Vector2(num2, num3);
		}
		return array;
	}

	public static int Clip(ref int value, int min, int max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static float Clip(ref float value, float min, float max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static DateTime Clip(ref DateTime value, DateTime min, DateTime max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return max;
	}

	public static int ValueRewind(int current, int max, ref int startCnt)
	{
		if (current < max)
		{
			return ++current;
		}
		return ++startCnt;
	}

	public static int CrossValue(int nMax, int nValue)
	{
		return nMax - 1 - nValue;
	}

	public static float Bar2Playtime(int bpm, int measure, int bar)
	{
		return (float)(60 * measure * bar / bpm);
	}

	public static int Bar2Frame(int bpm, int measure, int bar)
	{
		return 60 * measure * bar / bpm * 60;
	}
}
