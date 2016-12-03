using System;
using UnityEngine;

namespace LT.Tweening
{
	public class LTGUI
	{
		public enum Element_Type
		{
			Texture,
			Label
		}

		public static int RECT_LEVELS = 5;

		public static int RECTS_PER_LEVEL = 10;

		public static int BUTTONS_MAX = 24;

		private static LTRect[] levels;

		private static int[] levelDepths;

		private static Rect[] buttons;

		private static int[] buttonLevels;

		private static int[] buttonLastFrame;

		private static LTRect r;

		private static Color color = Color.get_white();

		private static bool isGUIEnabled = false;

		private static int global_counter = 0;

		public static void init()
		{
			if (LTGUI.levels == null)
			{
				LTGUI.levels = new LTRect[LTGUI.RECT_LEVELS * LTGUI.RECTS_PER_LEVEL];
				LTGUI.levelDepths = new int[LTGUI.RECT_LEVELS];
			}
		}

		public static void initRectCheck()
		{
			if (LTGUI.buttons == null)
			{
				LTGUI.buttons = new Rect[LTGUI.BUTTONS_MAX];
				LTGUI.buttonLevels = new int[LTGUI.BUTTONS_MAX];
				LTGUI.buttonLastFrame = new int[LTGUI.BUTTONS_MAX];
				for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
				{
					LTGUI.buttonLevels[i] = -1;
				}
			}
		}

		public static void reset()
		{
			if (LTGUI.isGUIEnabled)
			{
				LTGUI.isGUIEnabled = false;
				for (int i = 0; i < LTGUI.levels.Length; i++)
				{
					LTGUI.levels[i] = null;
				}
				for (int j = 0; j < LTGUI.levelDepths.Length; j++)
				{
					LTGUI.levelDepths[j] = 0;
				}
			}
		}

		public static void update(int updateLevel)
		{
			if (LTGUI.isGUIEnabled)
			{
				LTGUI.init();
				if (LTGUI.levelDepths[updateLevel] > 0)
				{
					LTGUI.color = GUI.get_color();
					int num = updateLevel * LTGUI.RECTS_PER_LEVEL;
					int num2 = num + LTGUI.levelDepths[updateLevel];
					for (int i = num; i < num2; i++)
					{
						LTGUI.r = LTGUI.levels[i];
						if (LTGUI.r != null)
						{
							if (LTGUI.r.useColor)
							{
								GUI.set_color(LTGUI.r.color);
							}
							if (LTGUI.r.type == LTGUI.Element_Type.Label)
							{
								if (LTGUI.r.style != null)
								{
									GUI.get_skin().set_label(LTGUI.r.style);
								}
								if (LTGUI.r.useSimpleScale)
								{
									GUI.Label(new Rect((LTGUI.r.rect.get_x() + LTGUI.r.margin.x + LTGUI.r.relativeRect.get_x()) * LTGUI.r.relativeRect.get_width(), (LTGUI.r.rect.get_y() + LTGUI.r.margin.y + LTGUI.r.relativeRect.get_y()) * LTGUI.r.relativeRect.get_height(), LTGUI.r.rect.get_width() * LTGUI.r.relativeRect.get_width(), LTGUI.r.rect.get_height() * LTGUI.r.relativeRect.get_height()), LTGUI.r.labelStr);
								}
								else
								{
									GUI.Label(new Rect(LTGUI.r.rect.get_x() + LTGUI.r.margin.x, LTGUI.r.rect.get_y() + LTGUI.r.margin.y, LTGUI.r.rect.get_width(), LTGUI.r.rect.get_height()), LTGUI.r.labelStr);
								}
							}
							else if (LTGUI.r.type == LTGUI.Element_Type.Texture && LTGUI.r.texture != null)
							{
								Vector2 vector = (!LTGUI.r.useSimpleScale) ? new Vector2(LTGUI.r.rect.get_width(), LTGUI.r.rect.get_height()) : new Vector2(0f, LTGUI.r.rect.get_height() * LTGUI.r.relativeRect.get_height());
								if (LTGUI.r.sizeByHeight)
								{
									vector.x = (float)LTGUI.r.texture.get_width() / (float)LTGUI.r.texture.get_height() * vector.y;
								}
								if (LTGUI.r.useSimpleScale)
								{
									GUI.DrawTexture(new Rect((LTGUI.r.rect.get_x() + LTGUI.r.margin.x + LTGUI.r.relativeRect.get_x()) * LTGUI.r.relativeRect.get_width(), (LTGUI.r.rect.get_y() + LTGUI.r.margin.y + LTGUI.r.relativeRect.get_y()) * LTGUI.r.relativeRect.get_height(), vector.x, vector.y), LTGUI.r.texture);
								}
								else
								{
									GUI.DrawTexture(new Rect(LTGUI.r.rect.get_x() + LTGUI.r.margin.x, LTGUI.r.rect.get_y() + LTGUI.r.margin.y, vector.x, vector.y), LTGUI.r.texture);
								}
							}
						}
					}
					GUI.set_color(LTGUI.color);
				}
			}
		}

		public static bool checkOnScreen(Rect rect)
		{
			bool flag = rect.get_x() + rect.get_width() < 0f;
			bool flag2 = rect.get_x() > (float)Screen.get_width();
			bool flag3 = rect.get_y() > (float)Screen.get_height();
			bool flag4 = rect.get_y() + rect.get_height() < 0f;
			return !flag && !flag2 && !flag3 && !flag4;
		}

		public static void destroy(int id)
		{
			int num = id & 65535;
			int num2 = id >> 16;
			if (id >= 0 && LTGUI.levels[num] != null && LTGUI.levels[num].hasInitiliazed && LTGUI.levels[num].counter == num2)
			{
				LTGUI.levels[num] = null;
			}
		}

		public static void destroyAll(int depth)
		{
			int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
			int num2 = depth * LTGUI.RECTS_PER_LEVEL;
			while (LTGUI.levels != null && num2 < num)
			{
				LTGUI.levels[num2] = null;
				num2++;
			}
		}

		public static LTRect label(Rect rect, string label, int depth)
		{
			return LTGUI.label(new LTRect(rect), label, depth);
		}

		public static LTRect label(LTRect rect, string label, int depth)
		{
			rect.type = LTGUI.Element_Type.Label;
			rect.labelStr = label;
			return LTGUI.element(rect, depth);
		}

		public static LTRect texture(Rect rect, Texture texture, int depth)
		{
			return LTGUI.texture(new LTRect(rect), texture, depth);
		}

		public static LTRect texture(LTRect rect, Texture texture, int depth)
		{
			rect.type = LTGUI.Element_Type.Texture;
			rect.texture = texture;
			return LTGUI.element(rect, depth);
		}

		public static LTRect element(LTRect rect, int depth)
		{
			LTGUI.isGUIEnabled = true;
			LTGUI.init();
			int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
			int num2 = 0;
			if (rect != null)
			{
				LTGUI.destroy(rect.id);
			}
			if (rect.type == LTGUI.Element_Type.Label && rect.style != null && rect.style.get_normal().get_textColor().a <= 0f)
			{
				Debug.LogWarning("Your GUI normal color has an alpha of zero, and will not be rendered.");
			}
			if (rect.relativeRect.get_width() == float.PositiveInfinity)
			{
				rect.relativeRect = new Rect(0f, 0f, (float)Screen.get_width(), (float)Screen.get_height());
			}
			for (int i = depth * LTGUI.RECTS_PER_LEVEL; i < num; i++)
			{
				LTGUI.r = LTGUI.levels[i];
				if (LTGUI.r == null)
				{
					LTGUI.r = rect;
					LTGUI.r.rotateEnabled = true;
					LTGUI.r.alphaEnabled = true;
					LTGUI.r.setId(i, LTGUI.global_counter);
					LTGUI.levels[i] = LTGUI.r;
					if (num2 >= LTGUI.levelDepths[depth])
					{
						LTGUI.levelDepths[depth] = num2 + 1;
					}
					LTGUI.global_counter++;
					return LTGUI.r;
				}
				num2++;
			}
			Debug.LogError("You ran out of GUI Element spaces");
			return null;
		}

		public static bool hasNoOverlap(Rect rect, int depth)
		{
			LTGUI.initRectCheck();
			bool result = true;
			bool flag = false;
			for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
			{
				if (LTGUI.buttonLevels[i] >= 0)
				{
					if (LTGUI.buttonLastFrame[i] + 1 < Time.get_frameCount())
					{
						LTGUI.buttonLevels[i] = -1;
					}
					else if (LTGUI.buttonLevels[i] > depth && LTGUI.pressedWithinRect(LTGUI.buttons[i]))
					{
						result = false;
					}
				}
				if (!flag && LTGUI.buttonLevels[i] < 0)
				{
					flag = true;
					LTGUI.buttonLevels[i] = depth;
					LTGUI.buttons[i] = rect;
					LTGUI.buttonLastFrame[i] = Time.get_frameCount();
				}
			}
			return result;
		}

		public static bool pressedWithinRect(Rect rect)
		{
			Vector2 vector = LTGUI.firstTouch();
			if (vector.x < 0f)
			{
				return false;
			}
			float num = (float)Screen.get_height() - vector.y;
			return vector.x > rect.get_x() && vector.x < rect.get_x() + rect.get_width() && num > rect.get_y() && num < rect.get_y() + rect.get_height();
		}

		public static bool checkWithinRect(Vector2 vec2, Rect rect)
		{
			vec2.y = (float)Screen.get_height() - vec2.y;
			return vec2.x > rect.get_x() && vec2.x < rect.get_x() + rect.get_width() && vec2.y > rect.get_y() && vec2.y < rect.get_y() + rect.get_height();
		}

		public static Vector2 firstTouch()
		{
			if (Input.get_touchCount() > 0)
			{
				return Input.get_touches()[0].get_position();
			}
			if (Input.GetMouseButton(0))
			{
				return Input.get_mousePosition();
			}
			return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		}
	}
}
