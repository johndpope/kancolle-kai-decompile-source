using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public static class NGUIText
{
	public enum Alignment
	{
		Automatic,
		Left,
		Center,
		Right,
		Justified
	}

	public enum SymbolStyle
	{
		None,
		Normal,
		Colored
	}

	public class GlyphInfo
	{
		public Vector2 v0;

		public Vector2 v1;

		public Vector2 u0;

		public Vector2 u1;

		public Vector2 u2;

		public Vector2 u3;

		public float advance;

		public int channel;
	}

	public static UIFont bitmapFont;

	public static Font dynamicFont;

	public static NGUIText.GlyphInfo glyph = new NGUIText.GlyphInfo();

	public static int fontSize = 16;

	public static float fontScale = 1f;

	public static float pixelDensity = 1f;

	public static FontStyle fontStyle = 0;

	public static NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	public static Color tint = Color.get_white();

	public static int rectWidth = 1000000;

	public static int rectHeight = 1000000;

	public static int regionWidth = 1000000;

	public static int regionHeight = 1000000;

	public static int maxLines = 0;

	public static bool gradient = false;

	public static Color gradientBottom = Color.get_white();

	public static Color gradientTop = Color.get_white();

	public static bool encoding = false;

	public static float spacingX = 0f;

	public static float spacingY = 0f;

	public static bool premultiply = false;

	public static NGUIText.SymbolStyle symbolStyle;

	public static int finalSize = 0;

	public static float finalSpacingX = 0f;

	public static float finalLineHeight = 0f;

	public static float baseline = 0f;

	public static bool useSymbols = false;

	private static Color mInvisible = new Color(0f, 0f, 0f, 0f);

	private static BetterList<Color> mColors = new BetterList<Color>();

	private static float mAlpha = 1f;

	private static CharacterInfo mTempChar;

	private static BetterList<float> mSizes = new BetterList<float>();

	private static Color32 s_c0;

	private static Color32 s_c1;

	private static float[] mBoldOffset = new float[]
	{
		-0.25f,
		0f,
		0.25f,
		0f,
		0f,
		-0.25f,
		0f,
		0.25f
	};

	public static void Update()
	{
		NGUIText.Update(true);
	}

	public static void Update(bool request)
	{
		NGUIText.finalSize = Mathf.RoundToInt((float)NGUIText.fontSize / NGUIText.pixelDensity);
		NGUIText.finalSpacingX = NGUIText.spacingX * NGUIText.fontScale;
		NGUIText.finalLineHeight = ((float)NGUIText.fontSize + NGUIText.spacingY) * NGUIText.fontScale;
		NGUIText.useSymbols = (NGUIText.bitmapFont != null && NGUIText.bitmapFont.hasSymbols && NGUIText.encoding && NGUIText.symbolStyle != NGUIText.SymbolStyle.None);
		if (NGUIText.dynamicFont != null && request)
		{
			NGUIText.dynamicFont.RequestCharactersInTexture(")_-", NGUIText.finalSize, NGUIText.fontStyle);
			if (!NGUIText.dynamicFont.GetCharacterInfo(')', ref NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle) || (float)NGUIText.mTempChar.get_maxY() == 0f)
			{
				NGUIText.dynamicFont.RequestCharactersInTexture("A", NGUIText.finalSize, NGUIText.fontStyle);
				if (!NGUIText.dynamicFont.GetCharacterInfo('A', ref NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
				{
					NGUIText.baseline = 0f;
					return;
				}
			}
			float num = (float)NGUIText.mTempChar.get_maxY();
			float num2 = (float)NGUIText.mTempChar.get_minY();
			NGUIText.baseline = Mathf.Round(num + ((float)NGUIText.finalSize - num + num2) * 0.5f);
		}
	}

	public static void Prepare(string text)
	{
		if (NGUIText.dynamicFont != null)
		{
			NGUIText.dynamicFont.RequestCharactersInTexture(text, NGUIText.finalSize, NGUIText.fontStyle);
		}
	}

	public static BMSymbol GetSymbol(string text, int index, int textLength)
	{
		return (!(NGUIText.bitmapFont != null)) ? null : NGUIText.bitmapFont.MatchSymbol(text, index, textLength);
	}

	public static float GetGlyphWidth(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph bMGlyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (bMGlyph != null)
			{
				int num = bMGlyph.advance;
				if (flag)
				{
					num >>= 1;
				}
				return NGUIText.fontScale * (float)((prev == 0) ? bMGlyph.advance : (num + bMGlyph.GetKerning(prev)));
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, ref NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			return (float)NGUIText.mTempChar.get_advance() * NGUIText.fontScale * NGUIText.pixelDensity;
		}
		return 0f;
	}

	public static NGUIText.GlyphInfo GetGlyph(int ch, int prev)
	{
		if (NGUIText.bitmapFont != null)
		{
			bool flag = false;
			if (ch == 8201)
			{
				flag = true;
				ch = 32;
			}
			BMGlyph bMGlyph = NGUIText.bitmapFont.bmFont.GetGlyph(ch);
			if (bMGlyph != null)
			{
				int num = (prev == 0) ? 0 : bMGlyph.GetKerning(prev);
				NGUIText.glyph.v0.x = (float)((prev == 0) ? bMGlyph.offsetX : (bMGlyph.offsetX + num));
				NGUIText.glyph.v1.y = (float)(-(float)bMGlyph.offsetY);
				NGUIText.glyph.v1.x = NGUIText.glyph.v0.x + (float)bMGlyph.width;
				NGUIText.glyph.v0.y = NGUIText.glyph.v1.y - (float)bMGlyph.height;
				NGUIText.glyph.u0.x = (float)bMGlyph.x;
				NGUIText.glyph.u0.y = (float)(bMGlyph.y + bMGlyph.height);
				NGUIText.glyph.u2.x = (float)(bMGlyph.x + bMGlyph.width);
				NGUIText.glyph.u2.y = (float)bMGlyph.y;
				NGUIText.glyph.u1.x = NGUIText.glyph.u0.x;
				NGUIText.glyph.u1.y = NGUIText.glyph.u2.y;
				NGUIText.glyph.u3.x = NGUIText.glyph.u2.x;
				NGUIText.glyph.u3.y = NGUIText.glyph.u0.y;
				int num2 = bMGlyph.advance;
				if (flag)
				{
					num2 >>= 1;
				}
				NGUIText.glyph.advance = (float)(num2 + num);
				NGUIText.glyph.channel = bMGlyph.channel;
				if (NGUIText.fontScale != 1f)
				{
					NGUIText.glyph.v0 *= NGUIText.fontScale;
					NGUIText.glyph.v1 *= NGUIText.fontScale;
					NGUIText.glyph.advance *= NGUIText.fontScale;
				}
				return NGUIText.glyph;
			}
		}
		else if (NGUIText.dynamicFont != null && NGUIText.dynamicFont.GetCharacterInfo((char)ch, ref NGUIText.mTempChar, NGUIText.finalSize, NGUIText.fontStyle))
		{
			NGUIText.glyph.v0.x = (float)NGUIText.mTempChar.get_minX();
			NGUIText.glyph.v1.x = (float)NGUIText.mTempChar.get_maxX();
			NGUIText.glyph.v0.y = (float)NGUIText.mTempChar.get_maxY() - NGUIText.baseline;
			NGUIText.glyph.v1.y = (float)NGUIText.mTempChar.get_minY() - NGUIText.baseline;
			NGUIText.glyph.u0 = NGUIText.mTempChar.get_uvTopLeft();
			NGUIText.glyph.u1 = NGUIText.mTempChar.get_uvBottomLeft();
			NGUIText.glyph.u2 = NGUIText.mTempChar.get_uvBottomRight();
			NGUIText.glyph.u3 = NGUIText.mTempChar.get_uvTopRight();
			NGUIText.glyph.advance = (float)NGUIText.mTempChar.get_advance();
			NGUIText.glyph.channel = 0;
			NGUIText.glyph.v0.x = Mathf.Round(NGUIText.glyph.v0.x);
			NGUIText.glyph.v0.y = Mathf.Round(NGUIText.glyph.v0.y);
			NGUIText.glyph.v1.x = Mathf.Round(NGUIText.glyph.v1.x);
			NGUIText.glyph.v1.y = Mathf.Round(NGUIText.glyph.v1.y);
			float num3 = NGUIText.fontScale * NGUIText.pixelDensity;
			if (num3 != 1f)
			{
				NGUIText.glyph.v0 *= num3;
				NGUIText.glyph.v1 *= num3;
				NGUIText.glyph.advance *= num3;
			}
			return NGUIText.glyph;
		}
		return null;
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static float ParseAlpha(string text, int index)
	{
		int num = NGUIMath.HexToDecimal(text.get_Chars(index + 1)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(index + 2));
		return Mathf.Clamp01((float)num / 255f);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static Color ParseColor24(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text.get_Chars(offset)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 1));
		int num2 = NGUIMath.HexToDecimal(text.get_Chars(offset + 2)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 3));
		int num3 = NGUIMath.HexToDecimal(text.get_Chars(offset + 4)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 5));
		float num4 = 0.003921569f;
		return new Color(num4 * (float)num, num4 * (float)num2, num4 * (float)num3);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static Color ParseColor32(string text, int offset)
	{
		int num = NGUIMath.HexToDecimal(text.get_Chars(offset)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 1));
		int num2 = NGUIMath.HexToDecimal(text.get_Chars(offset + 2)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 3));
		int num3 = NGUIMath.HexToDecimal(text.get_Chars(offset + 4)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 5));
		int num4 = NGUIMath.HexToDecimal(text.get_Chars(offset + 6)) << 4 | NGUIMath.HexToDecimal(text.get_Chars(offset + 7));
		float num5 = 0.003921569f;
		return new Color(num5 * (float)num, num5 * (float)num2, num5 * (float)num3, num5 * (float)num4);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static string EncodeColor(string text, Color c)
	{
		return string.Concat(new string[]
		{
			"[c][",
			NGUIText.EncodeColor24(c),
			"]",
			text,
			"[-][/c]"
		});
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static string EncodeAlpha(float a)
	{
		int num = Mathf.Clamp(Mathf.RoundToInt(a * 255f), 0, 255);
		return NGUIMath.DecimalToHex8(num);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static string EncodeColor24(Color c)
	{
		int num = 16777215 & NGUIMath.ColorToInt(c) >> 8;
		return NGUIMath.DecimalToHex24(num);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static string EncodeColor32(Color c)
	{
		int num = NGUIMath.ColorToInt(c);
		return NGUIMath.DecimalToHex32(num);
	}

	public static bool ParseSymbol(string text, ref int index)
	{
		int num = 1;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		return NGUIText.ParseSymbol(text, ref index, null, false, ref num, ref flag, ref flag2, ref flag3, ref flag4, ref flag5);
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static bool IsHex(char ch)
	{
		return (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
	}

	public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply, ref int sub, ref bool bold, ref bool italic, ref bool underline, ref bool strike, ref bool ignoreColor)
	{
		int length = text.get_Length();
		if (index + 3 > length || text.get_Chars(index) != '[')
		{
			return false;
		}
		int num;
		if (text.get_Chars(index + 2) == ']')
		{
			if (text.get_Chars(index + 1) == '-')
			{
				if (colors != null && colors.size > 1)
				{
					colors.RemoveAt(colors.size - 1);
				}
				index += 3;
				return true;
			}
			string text2 = text.Substring(index, 3);
			string text3 = text2;
			if (text3 != null)
			{
				if (NGUIText.<>f__switch$map0 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("[b]", 0);
					dictionary.Add("[i]", 1);
					dictionary.Add("[u]", 2);
					dictionary.Add("[s]", 3);
					dictionary.Add("[c]", 4);
					NGUIText.<>f__switch$map0 = dictionary;
				}
				if (NGUIText.<>f__switch$map0.TryGetValue(text3, ref num))
				{
					switch (num)
					{
					case 0:
						bold = true;
						index += 3;
						return true;
					case 1:
						italic = true;
						index += 3;
						return true;
					case 2:
						underline = true;
						index += 3;
						return true;
					case 3:
						strike = true;
						index += 3;
						return true;
					case 4:
						ignoreColor = true;
						index += 3;
						return true;
					}
				}
			}
		}
		if (index + 4 > length)
		{
			return false;
		}
		if (text.get_Chars(index + 3) == ']')
		{
			string text4 = text.Substring(index, 4);
			string text3 = text4;
			if (text3 != null)
			{
				if (NGUIText.<>f__switch$map1 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("[/b]", 0);
					dictionary.Add("[/i]", 1);
					dictionary.Add("[/u]", 2);
					dictionary.Add("[/s]", 3);
					dictionary.Add("[/c]", 4);
					NGUIText.<>f__switch$map1 = dictionary;
				}
				if (NGUIText.<>f__switch$map1.TryGetValue(text3, ref num))
				{
					switch (num)
					{
					case 0:
						bold = false;
						index += 4;
						return true;
					case 1:
						italic = false;
						index += 4;
						return true;
					case 2:
						underline = false;
						index += 4;
						return true;
					case 3:
						strike = false;
						index += 4;
						return true;
					case 4:
						ignoreColor = false;
						index += 4;
						return true;
					}
				}
			}
			char ch = text.get_Chars(index + 1);
			char ch2 = text.get_Chars(index + 2);
			if (NGUIText.IsHex(ch) && NGUIText.IsHex(ch2))
			{
				int num2 = NGUIMath.HexToDecimal(ch) << 4 | NGUIMath.HexToDecimal(ch2);
				NGUIText.mAlpha = (float)num2 / 255f;
				index += 4;
				return true;
			}
		}
		if (index + 5 > length)
		{
			return false;
		}
		if (text.get_Chars(index + 4) == ']')
		{
			string text5 = text.Substring(index, 5);
			string text3 = text5;
			if (text3 != null)
			{
				if (NGUIText.<>f__switch$map2 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("[sub]", 0);
					dictionary.Add("[sup]", 1);
					NGUIText.<>f__switch$map2 = dictionary;
				}
				if (NGUIText.<>f__switch$map2.TryGetValue(text3, ref num))
				{
					if (num == 0)
					{
						sub = 1;
						index += 5;
						return true;
					}
					if (num == 1)
					{
						sub = 2;
						index += 5;
						return true;
					}
				}
			}
		}
		if (index + 6 > length)
		{
			return false;
		}
		if (text.get_Chars(index + 5) == ']')
		{
			string text6 = text.Substring(index, 6);
			string text3 = text6;
			if (text3 != null)
			{
				if (NGUIText.<>f__switch$map3 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("[/sub]", 0);
					dictionary.Add("[/sup]", 1);
					dictionary.Add("[/url]", 2);
					NGUIText.<>f__switch$map3 = dictionary;
				}
				if (NGUIText.<>f__switch$map3.TryGetValue(text3, ref num))
				{
					switch (num)
					{
					case 0:
						sub = 0;
						index += 6;
						return true;
					case 1:
						sub = 0;
						index += 6;
						return true;
					case 2:
						index += 6;
						return true;
					}
				}
			}
		}
		if (text.get_Chars(index + 1) == 'u' && text.get_Chars(index + 2) == 'r' && text.get_Chars(index + 3) == 'l' && text.get_Chars(index + 4) == '=')
		{
			int num3 = text.IndexOf(']', index + 4);
			if (num3 != -1)
			{
				index = num3 + 1;
				return true;
			}
			index = text.get_Length();
			return true;
		}
		else
		{
			if (index + 8 > length)
			{
				return false;
			}
			if (text.get_Chars(index + 7) == ']')
			{
				Color color = NGUIText.ParseColor24(text, index + 1);
				if (NGUIText.EncodeColor24(color) != text.Substring(index + 1, 6).ToUpper())
				{
					return false;
				}
				if (colors != null)
				{
					color.a = colors[colors.size - 1].a;
					if (premultiply && color.a != 1f)
					{
						color = Color.Lerp(NGUIText.mInvisible, color, color.a);
					}
					colors.Add(color);
				}
				index += 8;
				return true;
			}
			else
			{
				if (index + 10 > length)
				{
					return false;
				}
				if (text.get_Chars(index + 9) != ']')
				{
					return false;
				}
				Color color2 = NGUIText.ParseColor32(text, index + 1);
				if (NGUIText.EncodeColor32(color2) != text.Substring(index + 1, 8).ToUpper())
				{
					return false;
				}
				if (colors != null)
				{
					if (premultiply && color2.a != 1f)
					{
						color2 = Color.Lerp(NGUIText.mInvisible, color2, color2.a);
					}
					colors.Add(color2);
				}
				index += 10;
				return true;
			}
		}
	}

	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			int i = 0;
			int length = text.get_Length();
			while (i < length)
			{
				char c = text.get_Chars(i);
				if (c == '[')
				{
					int num = 0;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					int num2 = i;
					if (NGUIText.ParseSymbol(text, ref num2, null, false, ref num, ref flag, ref flag2, ref flag3, ref flag4, ref flag5))
					{
						text = text.Remove(i, num2 - i);
						length = text.get_Length();
						continue;
					}
				}
				i++;
			}
		}
		return text;
	}

	public static void Align(BetterList<Vector3> verts, int indexOffset, float printedWidth)
	{
		switch (NGUIText.alignment)
		{
		case NGUIText.Alignment.Center:
		{
			float num = ((float)NGUIText.rectWidth - printedWidth) * 0.5f;
			if (num < 0f)
			{
				return;
			}
			int num2 = Mathf.RoundToInt((float)NGUIText.rectWidth - printedWidth);
			int num3 = Mathf.RoundToInt((float)NGUIText.rectWidth);
			bool flag = (num2 & 1) == 1;
			bool flag2 = (num3 & 1) == 1;
			if ((flag && !flag2) || (!flag && flag2))
			{
				num += 0.5f * NGUIText.fontScale;
			}
			for (int i = indexOffset; i < verts.size; i++)
			{
				Vector3[] expr_F1_cp_0 = verts.buffer;
				int expr_F1_cp_1 = i;
				expr_F1_cp_0[expr_F1_cp_1].x = expr_F1_cp_0[expr_F1_cp_1].x + num;
			}
			break;
		}
		case NGUIText.Alignment.Right:
		{
			float num4 = (float)NGUIText.rectWidth - printedWidth;
			if (num4 < 0f)
			{
				return;
			}
			for (int j = indexOffset; j < verts.size; j++)
			{
				Vector3[] expr_49_cp_0 = verts.buffer;
				int expr_49_cp_1 = j;
				expr_49_cp_0[expr_49_cp_1].x = expr_49_cp_0[expr_49_cp_1].x + num4;
			}
			break;
		}
		case NGUIText.Alignment.Justified:
		{
			if (printedWidth < (float)NGUIText.rectWidth * 0.65f)
			{
				return;
			}
			float num5 = ((float)NGUIText.rectWidth - printedWidth) * 0.5f;
			if (num5 < 1f)
			{
				return;
			}
			int num6 = (verts.size - indexOffset) / 4;
			if (num6 < 1)
			{
				return;
			}
			float num7 = 1f / (float)(num6 - 1);
			float num8 = (float)NGUIText.rectWidth / printedWidth;
			int k = indexOffset + 4;
			int num9 = 1;
			while (k < verts.size)
			{
				float num10 = verts.buffer[k].x;
				float num11 = verts.buffer[k + 2].x;
				float num12 = num11 - num10;
				float num13 = num10 * num8;
				float num14 = num13 + num12;
				float num15 = num11 * num8;
				float num16 = num15 - num12;
				float num17 = (float)num9 * num7;
				num10 = Mathf.Lerp(num13, num16, num17);
				num11 = Mathf.Lerp(num14, num15, num17);
				num10 = Mathf.Round(num10);
				num11 = Mathf.Round(num11);
				verts.buffer[k++].x = num10;
				verts.buffer[k++].x = num10;
				verts.buffer[k++].x = num11;
				verts.buffer[k++].x = num11;
				num9++;
			}
			break;
		}
		}
	}

	public static int GetExactCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		for (int i = 0; i < indices.size; i++)
		{
			int num = i << 1;
			int i2 = num + 1;
			float x = verts[num].x;
			if (pos.x >= x)
			{
				float x2 = verts[i2].x;
				if (pos.x <= x2)
				{
					float y = verts[num].y;
					if (pos.y >= y)
					{
						float y2 = verts[i2].y;
						if (pos.y <= y2)
						{
							return indices[i];
						}
					}
				}
			}
		}
		return 0;
	}

	public static int GetApproximateCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
	{
		float num = 3.40282347E+38f;
		float num2 = 3.40282347E+38f;
		int i = 0;
		for (int j = 0; j < verts.size; j++)
		{
			float num3 = Mathf.Abs(pos.y - verts[j].y);
			if (num3 <= num2)
			{
				float num4 = Mathf.Abs(pos.x - verts[j].x);
				if (num3 < num2)
				{
					num2 = num3;
					num = num4;
					i = j;
				}
				else if (num4 < num)
				{
					num = num4;
					i = j;
				}
			}
		}
		return indices[i];
	}

	[DebuggerHidden, DebuggerStepThrough]
	private static bool IsSpace(int ch)
	{
		return ch == 32 || ch == 8202 || ch == 8203 || ch == 8201;
	}

	[DebuggerHidden, DebuggerStepThrough]
	public static void EndLine(ref StringBuilder s)
	{
		int num = s.get_Length() - 1;
		if (num > 0 && NGUIText.IsSpace((int)s.get_Chars(num)))
		{
			s.set_Chars(num, '\n');
		}
		else
		{
			s.Append('\n');
		}
	}

	[DebuggerHidden, DebuggerStepThrough]
	private static void ReplaceSpaceWithNewline(ref StringBuilder s)
	{
		int num = s.get_Length() - 1;
		if (num > 0 && NGUIText.IsSpace((int)s.get_Chars(num)))
		{
			s.set_Chars(num, '\n');
		}
	}

	public static Vector2 CalculatePrintedSize(string text)
	{
		Vector2 zero = Vector2.get_zero();
		if (!string.IsNullOrEmpty(text))
		{
			if (NGUIText.encoding)
			{
				text = NGUIText.StripSymbols(text);
			}
			NGUIText.Prepare(text);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int length = text.get_Length();
			int prev = 0;
			for (int i = 0; i < length; i++)
			{
				int num4 = (int)text.get_Chars(i);
				if (num4 == 10)
				{
					if (num > num3)
					{
						num3 = num;
					}
					num = 0f;
					num2 += NGUIText.finalLineHeight;
				}
				else if (num4 >= 32)
				{
					BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
					if (bMSymbol == null)
					{
						float num5 = NGUIText.GetGlyphWidth(num4, prev);
						if (num5 != 0f)
						{
							num5 += NGUIText.finalSpacingX;
							if (Mathf.RoundToInt(num + num5) > NGUIText.regionWidth)
							{
								if (num > num3)
								{
									num3 = num - NGUIText.finalSpacingX;
								}
								num = num5;
								num2 += NGUIText.finalLineHeight;
							}
							else
							{
								num += num5;
							}
							prev = num4;
						}
					}
					else
					{
						float num6 = NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale;
						if (Mathf.RoundToInt(num + num6) > NGUIText.regionWidth)
						{
							if (num > num3)
							{
								num3 = num - NGUIText.finalSpacingX;
							}
							num = num6;
							num2 += NGUIText.finalLineHeight;
						}
						else
						{
							num += num6;
						}
						i += bMSymbol.sequence.get_Length() - 1;
						prev = 0;
					}
				}
			}
			zero.x = ((num <= num3) ? num3 : (num - NGUIText.finalSpacingX));
			zero.y = num2 + NGUIText.finalLineHeight;
		}
		return zero;
	}

	public static int CalculateOffsetToFit(string text)
	{
		if (string.IsNullOrEmpty(text) || NGUIText.regionWidth < 1)
		{
			return 0;
		}
		NGUIText.Prepare(text);
		int length = text.get_Length();
		int prev = 0;
		int i = 0;
		int length2 = text.get_Length();
		while (i < length2)
		{
			BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
			if (bMSymbol == null)
			{
				int num = (int)text.get_Chars(i);
				float glyphWidth = NGUIText.GetGlyphWidth(num, prev);
				if (glyphWidth != 0f)
				{
					NGUIText.mSizes.Add(NGUIText.finalSpacingX + glyphWidth);
				}
				prev = num;
			}
			else
			{
				NGUIText.mSizes.Add(NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale);
				int j = 0;
				int num2 = bMSymbol.sequence.get_Length() - 1;
				while (j < num2)
				{
					NGUIText.mSizes.Add(0f);
					j++;
				}
				i += bMSymbol.sequence.get_Length() - 1;
				prev = 0;
			}
			i++;
		}
		float num3 = (float)NGUIText.regionWidth;
		int num4 = NGUIText.mSizes.size;
		while (num4 > 0 && num3 > 0f)
		{
			num3 -= NGUIText.mSizes[--num4];
		}
		NGUIText.mSizes.Clear();
		if (num3 < 0f)
		{
			num4++;
		}
		return num4;
	}

	public static string GetEndOfLineThatFits(string text)
	{
		int length = text.get_Length();
		int num = NGUIText.CalculateOffsetToFit(text);
		return text.Substring(num, length - num);
	}

	public static bool WrapText(string text, out string finalText)
	{
		return NGUIText.WrapText(text, out finalText, false);
	}

	public static bool WrapText(string text, out string finalText, bool keepCharCount)
	{
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 1 || NGUIText.finalLineHeight < 1f)
		{
			finalText = string.Empty;
			return false;
		}
		float num = (NGUIText.maxLines <= 0) ? ((float)NGUIText.regionHeight) : Mathf.Min((float)NGUIText.regionHeight, NGUIText.finalLineHeight * (float)NGUIText.maxLines);
		int num2 = (NGUIText.maxLines <= 0) ? 1000000 : NGUIText.maxLines;
		num2 = Mathf.FloorToInt(Mathf.Min((float)num2, num / NGUIText.finalLineHeight) + 0.01f);
		if (num2 == 0)
		{
			finalText = string.Empty;
			return false;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		StringBuilder stringBuilder = new StringBuilder();
		int length = text.get_Length();
		float num3 = (float)NGUIText.regionWidth;
		int num4 = 0;
		int i = 0;
		int num5 = 1;
		int prev = 0;
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		while (i < length)
		{
			char c = text.get_Chars(i);
			if (c > '⿿')
			{
				flag3 = true;
			}
			if (c == '\n')
			{
				if (num5 == num2)
				{
					break;
				}
				num3 = (float)NGUIText.regionWidth;
				if (num4 < i)
				{
					stringBuilder.Append(text.Substring(num4, i - num4 + 1));
				}
				else
				{
					stringBuilder.Append(c);
				}
				flag = true;
				num5++;
				num4 = i + 1;
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				float num6;
				if (bMSymbol == null)
				{
					float glyphWidth = NGUIText.GetGlyphWidth((int)c, prev);
					if (glyphWidth == 0f)
					{
						goto IL_38A;
					}
					num6 = NGUIText.finalSpacingX + glyphWidth;
				}
				else
				{
					num6 = NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale;
				}
				num3 -= num6;
				if (NGUIText.IsSpace((int)c) && !flag3 && num4 < i)
				{
					int num7 = i - num4 + 1;
					if (num5 == num2 && num3 <= 0f && i < length)
					{
						char c2 = text.get_Chars(i);
						if (c2 < ' ' || NGUIText.IsSpace((int)c2))
						{
							num7--;
						}
					}
					stringBuilder.Append(text.Substring(num4, num7));
					flag = false;
					num4 = i + 1;
				}
				if (Mathf.RoundToInt(num3) < 0)
				{
					if (flag || num5 == num2)
					{
						stringBuilder.Append(text.Substring(num4, Mathf.Max(0, i - num4)));
						bool flag4 = NGUIText.IsSpace((int)c);
						if (!flag4 && !flag3)
						{
							flag2 = false;
						}
						if (num5++ == num2)
						{
							num4 = i;
							break;
						}
						if (keepCharCount)
						{
							NGUIText.ReplaceSpaceWithNewline(ref stringBuilder);
						}
						else
						{
							NGUIText.EndLine(ref stringBuilder);
						}
						flag = true;
						if (flag4)
						{
							num4 = i + 1;
							num3 = (float)NGUIText.regionWidth;
						}
						else
						{
							num4 = i;
							num3 = (float)NGUIText.regionWidth - num6;
						}
						prev = 0;
					}
					else
					{
						flag = true;
						num3 = (float)NGUIText.regionWidth;
						i = num4 - 1;
						prev = 0;
						if (num5++ == num2)
						{
							break;
						}
						if (keepCharCount)
						{
							NGUIText.ReplaceSpaceWithNewline(ref stringBuilder);
						}
						else
						{
							NGUIText.EndLine(ref stringBuilder);
						}
						goto IL_38A;
					}
				}
				else
				{
					prev = (int)c;
				}
				if (bMSymbol != null)
				{
					i += bMSymbol.length - 1;
					prev = 0;
				}
			}
			IL_38A:
			i++;
		}
		if (num4 < i)
		{
			stringBuilder.Append(text.Substring(num4, i - num4));
		}
		finalText = stringBuilder.ToString();
		return flag2 && (i == length || num5 <= Mathf.Min(NGUIText.maxLines, num2));
	}

	public static void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int size = verts.size;
		NGUIText.Prepare(text);
		NGUIText.mColors.Add(Color.get_white());
		NGUIText.mAlpha = 1f;
		int prev = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)NGUIText.finalSize;
		Color color = NGUIText.tint * NGUIText.gradientBottom;
		Color color2 = NGUIText.tint * NGUIText.gradientTop;
		Color32 color3 = NGUIText.tint;
		int length = text.get_Length();
		Rect rect = default(Rect);
		float num5 = 0f;
		float num6 = 0f;
		float num7 = num4 * NGUIText.pixelDensity;
		bool flag = false;
		int num8 = 0;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		if (NGUIText.bitmapFont != null)
		{
			rect = NGUIText.bitmapFont.uvRect;
			num5 = rect.get_width() / (float)NGUIText.bitmapFont.texWidth;
			num6 = rect.get_height() / (float)NGUIText.bitmapFont.texHeight;
		}
		for (int i = 0; i < length; i++)
		{
			int num9 = (int)text.get_Chars(i);
			float num10 = num;
			if (num9 == 10)
			{
				if (num > num3)
				{
					num3 = num;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num = 0f;
				num2 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num9 < 32)
			{
				prev = num9;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i, NGUIText.mColors, NGUIText.premultiply, ref num8, ref flag2, ref flag3, ref flag4, ref flag5, ref flag6))
			{
				Color color4;
				if (flag6)
				{
					color4 = NGUIText.mColors[NGUIText.mColors.size - 1];
					color4.a *= NGUIText.mAlpha * NGUIText.tint.a;
				}
				else
				{
					color4 = NGUIText.tint * NGUIText.mColors[NGUIText.mColors.size - 1];
					color4.a *= NGUIText.mAlpha;
				}
				color3 = color4;
				int j = 0;
				int num11 = NGUIText.mColors.size - 2;
				while (j < num11)
				{
					color4.a *= NGUIText.mColors[j].a;
					j++;
				}
				if (NGUIText.gradient)
				{
					color = NGUIText.gradientBottom * color4;
					color2 = NGUIText.gradientTop * color4;
				}
				i--;
			}
			else
			{
				BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bMSymbol != null)
				{
					float num12 = num + (float)bMSymbol.offsetX * NGUIText.fontScale;
					float num13 = num12 + (float)bMSymbol.width * NGUIText.fontScale;
					float num14 = -(num2 + (float)bMSymbol.offsetY * NGUIText.fontScale);
					float num15 = num14 - (float)bMSymbol.height * NGUIText.fontScale;
					if (Mathf.RoundToInt(num + (float)bMSymbol.advance * NGUIText.fontScale) > NGUIText.regionWidth)
					{
						if (num == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num12 -= num;
						num13 -= num;
						num15 -= NGUIText.finalLineHeight;
						num14 -= NGUIText.finalLineHeight;
						num = 0f;
						num2 += NGUIText.finalLineHeight;
					}
					verts.Add(new Vector3(num12, num15));
					verts.Add(new Vector3(num12, num14));
					verts.Add(new Vector3(num13, num14));
					verts.Add(new Vector3(num13, num15));
					num += NGUIText.finalSpacingX + (float)bMSymbol.advance * NGUIText.fontScale;
					i += bMSymbol.length - 1;
					prev = 0;
					if (uvs != null)
					{
						Rect uvRect = bMSymbol.uvRect;
						float xMin = uvRect.get_xMin();
						float yMin = uvRect.get_yMin();
						float xMax = uvRect.get_xMax();
						float yMax = uvRect.get_yMax();
						uvs.Add(new Vector2(xMin, yMin));
						uvs.Add(new Vector2(xMin, yMax));
						uvs.Add(new Vector2(xMax, yMax));
						uvs.Add(new Vector2(xMax, yMin));
					}
					if (cols != null)
					{
						if (NGUIText.symbolStyle == NGUIText.SymbolStyle.Colored)
						{
							for (int k = 0; k < 4; k++)
							{
								cols.Add(color3);
							}
						}
						else
						{
							Color32 item = Color.get_white();
							item.a = color3.a;
							for (int l = 0; l < 4; l++)
							{
								cols.Add(item);
							}
						}
					}
				}
				else
				{
					NGUIText.GlyphInfo glyphInfo = NGUIText.GetGlyph(num9, prev);
					if (glyphInfo != null)
					{
						prev = num9;
						if (num8 != 0)
						{
							NGUIText.GlyphInfo expr_502_cp_0 = glyphInfo;
							expr_502_cp_0.v0.x = expr_502_cp_0.v0.x * 0.75f;
							NGUIText.GlyphInfo expr_51A_cp_0 = glyphInfo;
							expr_51A_cp_0.v0.y = expr_51A_cp_0.v0.y * 0.75f;
							NGUIText.GlyphInfo expr_532_cp_0 = glyphInfo;
							expr_532_cp_0.v1.x = expr_532_cp_0.v1.x * 0.75f;
							NGUIText.GlyphInfo expr_54A_cp_0 = glyphInfo;
							expr_54A_cp_0.v1.y = expr_54A_cp_0.v1.y * 0.75f;
							if (num8 == 1)
							{
								NGUIText.GlyphInfo expr_56A_cp_0 = glyphInfo;
								expr_56A_cp_0.v0.y = expr_56A_cp_0.v0.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
								NGUIText.GlyphInfo expr_58F_cp_0 = glyphInfo;
								expr_58F_cp_0.v1.y = expr_58F_cp_0.v1.y - NGUIText.fontScale * (float)NGUIText.fontSize * 0.4f;
							}
							else
							{
								NGUIText.GlyphInfo expr_5B9_cp_0 = glyphInfo;
								expr_5B9_cp_0.v0.y = expr_5B9_cp_0.v0.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
								NGUIText.GlyphInfo expr_5DE_cp_0 = glyphInfo;
								expr_5DE_cp_0.v1.y = expr_5DE_cp_0.v1.y + NGUIText.fontScale * (float)NGUIText.fontSize * 0.05f;
							}
						}
						float num12 = glyphInfo.v0.x + num;
						float num15 = glyphInfo.v0.y - num2;
						float num13 = glyphInfo.v1.x + num;
						float num14 = glyphInfo.v1.y - num2;
						float num16 = glyphInfo.advance;
						if (NGUIText.finalSpacingX < 0f)
						{
							num16 += NGUIText.finalSpacingX;
						}
						if (Mathf.RoundToInt(num + num16) > NGUIText.regionWidth)
						{
							if (num == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num12 -= num;
							num13 -= num;
							num15 -= NGUIText.finalLineHeight;
							num14 -= NGUIText.finalLineHeight;
							num = 0f;
							num2 += NGUIText.finalLineHeight;
							num10 = 0f;
						}
						if (NGUIText.IsSpace(num9))
						{
							if (flag4)
							{
								num9 = 95;
							}
							else if (flag5)
							{
								num9 = 45;
							}
						}
						num += ((num8 != 0) ? ((NGUIText.finalSpacingX + glyphInfo.advance) * 0.75f) : (NGUIText.finalSpacingX + glyphInfo.advance));
						if (!NGUIText.IsSpace(num9))
						{
							if (uvs != null)
							{
								if (NGUIText.bitmapFont != null)
								{
									glyphInfo.u0.x = rect.get_xMin() + num5 * glyphInfo.u0.x;
									glyphInfo.u2.x = rect.get_xMin() + num5 * glyphInfo.u2.x;
									glyphInfo.u0.y = rect.get_yMax() - num6 * glyphInfo.u0.y;
									glyphInfo.u2.y = rect.get_yMax() - num6 * glyphInfo.u2.y;
									glyphInfo.u1.x = glyphInfo.u0.x;
									glyphInfo.u1.y = glyphInfo.u2.y;
									glyphInfo.u3.x = glyphInfo.u2.x;
									glyphInfo.u3.y = glyphInfo.u0.y;
								}
								int m = 0;
								int num17 = (!flag2) ? 1 : 4;
								while (m < num17)
								{
									uvs.Add(glyphInfo.u0);
									uvs.Add(glyphInfo.u1);
									uvs.Add(glyphInfo.u2);
									uvs.Add(glyphInfo.u3);
									m++;
								}
							}
							if (cols != null)
							{
								if (glyphInfo.channel == 0 || glyphInfo.channel == 15)
								{
									if (NGUIText.gradient)
									{
										float num18 = num7 + glyphInfo.v0.y / NGUIText.fontScale;
										float num19 = num7 + glyphInfo.v1.y / NGUIText.fontScale;
										num18 /= num7;
										num19 /= num7;
										NGUIText.s_c0 = Color.Lerp(color, color2, num18);
										NGUIText.s_c1 = Color.Lerp(color, color2, num19);
										int n = 0;
										int num20 = (!flag2) ? 1 : 4;
										while (n < num20)
										{
											cols.Add(NGUIText.s_c0);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c0);
											n++;
										}
									}
									else
									{
										int num21 = 0;
										int num22 = (!flag2) ? 4 : 16;
										while (num21 < num22)
										{
											cols.Add(color3);
											num21++;
										}
									}
								}
								else
								{
									Color color5 = color3;
									color5 *= 0.49f;
									switch (glyphInfo.channel)
									{
									case 1:
										color5.b += 0.51f;
										break;
									case 2:
										color5.g += 0.51f;
										break;
									case 4:
										color5.r += 0.51f;
										break;
									case 8:
										color5.a += 0.51f;
										break;
									}
									Color32 item2 = color5;
									int num23 = 0;
									int num24 = (!flag2) ? 4 : 16;
									while (num23 < num24)
									{
										cols.Add(item2);
										num23++;
									}
								}
							}
							if (!flag2)
							{
								if (!flag3)
								{
									verts.Add(new Vector3(num12, num15));
									verts.Add(new Vector3(num12, num14));
									verts.Add(new Vector3(num13, num14));
									verts.Add(new Vector3(num13, num15));
								}
								else
								{
									float num25 = (float)NGUIText.fontSize * 0.1f * ((num14 - num15) / (float)NGUIText.fontSize);
									verts.Add(new Vector3(num12 - num25, num15));
									verts.Add(new Vector3(num12 + num25, num14));
									verts.Add(new Vector3(num13 + num25, num14));
									verts.Add(new Vector3(num13 - num25, num15));
								}
							}
							else
							{
								for (int num26 = 0; num26 < 4; num26++)
								{
									float num27 = NGUIText.mBoldOffset[num26 * 2];
									float num28 = NGUIText.mBoldOffset[num26 * 2 + 1];
									float num29 = (!flag3) ? 0f : ((float)NGUIText.fontSize * 0.1f * ((num14 - num15) / (float)NGUIText.fontSize));
									verts.Add(new Vector3(num12 + num27 - num29, num15 + num28));
									verts.Add(new Vector3(num12 + num27 + num29, num14 + num28));
									verts.Add(new Vector3(num13 + num27 + num29, num14 + num28));
									verts.Add(new Vector3(num13 + num27 - num29, num15 + num28));
								}
							}
							if (flag4 || flag5)
							{
								NGUIText.GlyphInfo glyphInfo2 = NGUIText.GetGlyph((!flag5) ? 95 : 45, prev);
								if (glyphInfo2 != null)
								{
									if (uvs != null)
									{
										if (NGUIText.bitmapFont != null)
										{
											glyphInfo2.u0.x = rect.get_xMin() + num5 * glyphInfo2.u0.x;
											glyphInfo2.u2.x = rect.get_xMin() + num5 * glyphInfo2.u2.x;
											glyphInfo2.u0.y = rect.get_yMax() - num6 * glyphInfo2.u0.y;
											glyphInfo2.u2.y = rect.get_yMax() - num6 * glyphInfo2.u2.y;
										}
										float num30 = (glyphInfo2.u0.x + glyphInfo2.u2.x) * 0.5f;
										int num31 = 0;
										int num32 = (!flag2) ? 1 : 4;
										while (num31 < num32)
										{
											uvs.Add(new Vector2(num30, glyphInfo2.u0.y));
											uvs.Add(new Vector2(num30, glyphInfo2.u2.y));
											uvs.Add(new Vector2(num30, glyphInfo2.u2.y));
											uvs.Add(new Vector2(num30, glyphInfo2.u0.y));
											num31++;
										}
									}
									if (flag && flag5)
									{
										num15 = (-num2 + glyphInfo2.v0.y) * 0.75f;
										num14 = (-num2 + glyphInfo2.v1.y) * 0.75f;
									}
									else
									{
										num15 = -num2 + glyphInfo2.v0.y;
										num14 = -num2 + glyphInfo2.v1.y;
									}
									if (flag2)
									{
										for (int num33 = 0; num33 < 4; num33++)
										{
											float num34 = NGUIText.mBoldOffset[num33 * 2];
											float num35 = NGUIText.mBoldOffset[num33 * 2 + 1];
											verts.Add(new Vector3(num10 + num34, num15 + num35));
											verts.Add(new Vector3(num10 + num34, num14 + num35));
											verts.Add(new Vector3(num + num34, num14 + num35));
											verts.Add(new Vector3(num + num34, num15 + num35));
										}
									}
									else
									{
										verts.Add(new Vector3(num10, num15));
										verts.Add(new Vector3(num10, num14));
										verts.Add(new Vector3(num, num14));
										verts.Add(new Vector3(num, num15));
									}
									if (NGUIText.gradient)
									{
										float num36 = num7 + glyphInfo2.v0.y / NGUIText.fontScale;
										float num37 = num7 + glyphInfo2.v1.y / NGUIText.fontScale;
										num36 /= num7;
										num37 /= num7;
										NGUIText.s_c0 = Color.Lerp(color, color2, num36);
										NGUIText.s_c1 = Color.Lerp(color, color2, num37);
										int num38 = 0;
										int num39 = (!flag2) ? 1 : 4;
										while (num38 < num39)
										{
											cols.Add(NGUIText.s_c0);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c1);
											cols.Add(NGUIText.s_c0);
											num38++;
										}
									}
									else
									{
										int num40 = 0;
										int num41 = (!flag2) ? 4 : 16;
										while (num40 < num41)
										{
											cols.Add(color3);
											num40++;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
			size = verts.size;
		}
		NGUIText.mColors.Clear();
	}

	public static void PrintApproximateCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)NGUIText.fontSize * NGUIText.fontScale * 0.5f;
		int length = text.get_Length();
		int size = verts.size;
		int prev = 0;
		for (int i = 0; i < length; i++)
		{
			int num5 = (int)text.get_Chars(i);
			verts.Add(new Vector3(num, -num2 - num4));
			indices.Add(i);
			if (num5 == 10)
			{
				if (num > num3)
				{
					num3 = num;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num = 0f;
				num2 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num5 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bMSymbol == null)
				{
					float num6 = NGUIText.GetGlyphWidth(num5, prev);
					if (num6 != 0f)
					{
						num6 += NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(num + num6) > NGUIText.regionWidth)
						{
							if (num == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num = num6;
							num2 += NGUIText.finalLineHeight;
						}
						else
						{
							num += num6;
						}
						verts.Add(new Vector3(num, -num2 - num4));
						indices.Add(i + 1);
						prev = num5;
					}
				}
				else
				{
					float num7 = (float)bMSymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(num + num7) > NGUIText.regionWidth)
					{
						if (num == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num = num7;
						num2 += NGUIText.finalLineHeight;
					}
					else
					{
						num += num7;
					}
					verts.Add(new Vector3(num, -num2 - num4));
					indices.Add(i + 1);
					i += bMSymbol.sequence.get_Length() - 1;
					prev = 0;
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num - NGUIText.finalSpacingX);
		}
	}

	public static void PrintExactCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		float num = (float)NGUIText.fontSize * NGUIText.fontScale;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		int length = text.get_Length();
		int size = verts.size;
		int prev = 0;
		for (int i = 0; i < length; i++)
		{
			int num5 = (int)text.get_Chars(i);
			if (num5 == 10)
			{
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (NGUIText.alignment != NGUIText.Alignment.Left)
				{
					NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
					size = verts.size;
				}
				num2 = 0f;
				num3 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num5 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				if (bMSymbol == null)
				{
					float glyphWidth = NGUIText.GetGlyphWidth(num5, prev);
					if (glyphWidth != 0f)
					{
						float num6 = glyphWidth + NGUIText.finalSpacingX;
						if (Mathf.RoundToInt(num2 + num6) > NGUIText.regionWidth)
						{
							if (num2 == 0f)
							{
								return;
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
							{
								NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
								size = verts.size;
							}
							num2 = 0f;
							num3 += NGUIText.finalLineHeight;
							prev = 0;
							i--;
						}
						else
						{
							indices.Add(i);
							verts.Add(new Vector3(num2, -num3 - num));
							verts.Add(new Vector3(num2 + num6, -num3));
							prev = num5;
							num2 += num6;
						}
					}
				}
				else
				{
					float num7 = (float)bMSymbol.advance * NGUIText.fontScale + NGUIText.finalSpacingX;
					if (Mathf.RoundToInt(num2 + num7) > NGUIText.regionWidth)
					{
						if (num2 == 0f)
						{
							return;
						}
						if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
						{
							NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
							size = verts.size;
						}
						num2 = 0f;
						num3 += NGUIText.finalLineHeight;
						prev = 0;
						i--;
					}
					else
					{
						indices.Add(i);
						verts.Add(new Vector3(num2, -num3 - num));
						verts.Add(new Vector3(num2 + num7, -num3));
						i += bMSymbol.sequence.get_Length() - 1;
						num2 += num7;
						prev = 0;
					}
				}
			}
		}
		if (NGUIText.alignment != NGUIText.Alignment.Left && size < verts.size)
		{
			NGUIText.Align(verts, size, num2 - NGUIText.finalSpacingX);
		}
	}

	public static void PrintCaretAndSelection(string text, int start, int end, BetterList<Vector3> caret, BetterList<Vector3> highlight)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		NGUIText.Prepare(text);
		int num = end;
		if (start > end)
		{
			end = start;
			start = num;
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = (float)NGUIText.fontSize * NGUIText.fontScale;
		int indexOffset = (caret == null) ? 0 : caret.size;
		int num6 = (highlight == null) ? 0 : highlight.size;
		int length = text.get_Length();
		int i = 0;
		int prev = 0;
		bool flag = false;
		bool flag2 = false;
		Vector2 zero = Vector2.get_zero();
		Vector2 zero2 = Vector2.get_zero();
		while (i < length)
		{
			if (caret != null && !flag2 && num <= i)
			{
				flag2 = true;
				caret.Add(new Vector3(num2 - 1f, -num3 - num5));
				caret.Add(new Vector3(num2 - 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3 - num5));
			}
			int num7 = (int)text.get_Chars(i);
			if (num7 == 10)
			{
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (caret != null && flag2)
				{
					if (NGUIText.alignment != NGUIText.Alignment.Left)
					{
						NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
					}
					caret = null;
				}
				if (highlight != null)
				{
					if (flag)
					{
						flag = false;
						highlight.Add(zero2);
						highlight.Add(zero);
					}
					else if (start <= i && end > i)
					{
						highlight.Add(new Vector3(num2, -num3 - num5));
						highlight.Add(new Vector3(num2, -num3));
						highlight.Add(new Vector3(num2 + 2f, -num3));
						highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
					}
					if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
					{
						NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
						num6 = highlight.size;
					}
				}
				num2 = 0f;
				num3 += NGUIText.finalLineHeight;
				prev = 0;
			}
			else if (num7 < 32)
			{
				prev = 0;
			}
			else if (NGUIText.encoding && NGUIText.ParseSymbol(text, ref i))
			{
				i--;
			}
			else
			{
				BMSymbol bMSymbol = (!NGUIText.useSymbols) ? null : NGUIText.GetSymbol(text, i, length);
				float num8 = (bMSymbol == null) ? NGUIText.GetGlyphWidth(num7, prev) : ((float)bMSymbol.advance * NGUIText.fontScale);
				if (num8 != 0f)
				{
					float num9 = num2;
					float num10 = num2 + num8;
					float num11 = -num3 - num5;
					float num12 = -num3;
					if (Mathf.RoundToInt(num10 + NGUIText.finalSpacingX) > NGUIText.regionWidth)
					{
						if (num2 == 0f)
						{
							return;
						}
						if (num2 > num4)
						{
							num4 = num2;
						}
						if (caret != null && flag2)
						{
							if (NGUIText.alignment != NGUIText.Alignment.Left)
							{
								NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
							}
							caret = null;
						}
						if (highlight != null)
						{
							if (flag)
							{
								flag = false;
								highlight.Add(zero2);
								highlight.Add(zero);
							}
							else if (start <= i && end > i)
							{
								highlight.Add(new Vector3(num2, -num3 - num5));
								highlight.Add(new Vector3(num2, -num3));
								highlight.Add(new Vector3(num2 + 2f, -num3));
								highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
							}
							if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
							{
								NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
								num6 = highlight.size;
							}
						}
						num9 -= num2;
						num10 -= num2;
						num11 -= NGUIText.finalLineHeight;
						num12 -= NGUIText.finalLineHeight;
						num2 = 0f;
						num3 += NGUIText.finalLineHeight;
					}
					num2 += num8 + NGUIText.finalSpacingX;
					if (highlight != null)
					{
						if (start > i || end <= i)
						{
							if (flag)
							{
								flag = false;
								highlight.Add(zero2);
								highlight.Add(zero);
							}
						}
						else if (!flag)
						{
							flag = true;
							highlight.Add(new Vector3(num9, num11));
							highlight.Add(new Vector3(num9, num12));
						}
					}
					zero = new Vector2(num10, num11);
					zero2 = new Vector2(num10, num12);
					prev = num7;
				}
			}
			i++;
		}
		if (caret != null)
		{
			if (!flag2)
			{
				caret.Add(new Vector3(num2 - 1f, -num3 - num5));
				caret.Add(new Vector3(num2 - 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3));
				caret.Add(new Vector3(num2 + 1f, -num3 - num5));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left)
			{
				NGUIText.Align(caret, indexOffset, num2 - NGUIText.finalSpacingX);
			}
		}
		if (highlight != null)
		{
			if (flag)
			{
				highlight.Add(zero2);
				highlight.Add(zero);
			}
			else if (start < i && end == i)
			{
				highlight.Add(new Vector3(num2, -num3 - num5));
				highlight.Add(new Vector3(num2, -num3));
				highlight.Add(new Vector3(num2 + 2f, -num3));
				highlight.Add(new Vector3(num2 + 2f, -num3 - num5));
			}
			if (NGUIText.alignment != NGUIText.Alignment.Left && num6 < highlight.size)
			{
				NGUIText.Align(highlight, num6, num2 - NGUIText.finalSpacingX);
			}
		}
	}
}
