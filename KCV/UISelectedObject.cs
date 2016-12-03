using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UISelectedObject
	{
		private static bool ZoomInOut;

		public static void SelectedObjectZoom(GameObject[] Buttons, int Index, float Zoom_Rate_Normal, float Zoom_Rate_Zoom, float time)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.get_one() * Zoom_Rate_Zoom);
					tweenScale.duration = time;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.get_one());
					tweenScale2.duration = time;
				}
			}
		}

		public static void SelectedObjectZoomUpDown(GameObject[] Buttons, int Index, float Zoom_Rate_Normal, float Zoom_Rate_Zoom, float time)
		{
			if (!UISelectedObject.ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.get_one() * Zoom_Rate_Zoom);
					tweenScale.duration = time;
					tweenScale.style = UITweener.Style.PingPong;
					tweenScale.ignoreTimeScale = true;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], Zoom_Rate_Normal, Vector3.get_one());
					tweenScale2.duration = time;
					tweenScale2.style = UITweener.Style.Once;
					tweenScale2.ignoreTimeScale = true;
				}
			}
		}

		public static void SelectedObjectBlink(GameObject[] Buttons, int Index)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenColor tweenColor = TweenColor.Begin(Buttons[i].get_gameObject(), 0.2f, Util.CursolColor);
					tweenColor.from.r = Util.CursolColor.r;
					tweenColor.from.g = Util.CursolColor.g;
					tweenColor.from.b = Util.CursolColor.b;
					tweenColor.to.r = Color.get_white().r * 0.8f + Util.CursolColor.r * 0.2f;
					tweenColor.to.g = Color.get_white().g * 0.8f + Util.CursolColor.g * 0.2f;
					tweenColor.to.b = Color.get_white().b * 0.8f + Util.CursolColor.b * 0.2f;
					tweenColor.duration = Util.CursolBarDurationTime;
					tweenColor.method = UITweener.Method.EaseInOut;
					tweenColor.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenColor.Begin(Buttons[i].get_gameObject(), 0.2f, Color.get_white());
				}
			}
		}

		public static void SelectedObjectBlink(UIButton[] Buttons, int Index)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenColor tweenColor = TweenColor.Begin(Buttons[i].get_gameObject(), 0.2f, Util.CursolColor);
					tweenColor.from.r = Util.CursolColor.r;
					tweenColor.from.g = Util.CursolColor.g;
					tweenColor.from.b = Util.CursolColor.b;
					tweenColor.to.r = Color.get_white().r * 0.8f + Util.CursolColor.r * 0.2f;
					tweenColor.to.g = Color.get_white().g * 0.8f + Util.CursolColor.g * 0.2f;
					tweenColor.to.b = Color.get_white().b * 0.8f + Util.CursolColor.b * 0.2f;
					tweenColor.duration = Util.CursolBarDurationTime;
					tweenColor.method = UITweener.Method.EaseInOut;
					tweenColor.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenColor.Begin(Buttons[i].get_gameObject(), 0.2f, Color.get_white());
				}
			}
		}

		public static void SelectedOneObjectBlink(UIButton Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.get_gameObject(), 0.2f, Util.CursolColor);
				tweenColor.from.r = Util.CursolColor.r;
				tweenColor.from.g = Util.CursolColor.g;
				tweenColor.from.b = Util.CursolColor.b;
				tweenColor.to.r = Color.get_white().r * 0.8f + Util.CursolColor.r * 0.2f;
				tweenColor.to.g = Color.get_white().g * 0.8f + Util.CursolColor.g * 0.2f;
				tweenColor.to.b = Color.get_white().b * 0.8f + Util.CursolColor.b * 0.2f;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.get_gameObject(), 0.2f, Color.get_white());
			}
		}

		public static void SelectedOneObjectBlink(GameObject Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.get_gameObject(), 0.2f, Util.CursolColor);
				tweenColor.from.r = Color.get_white().r * 0.8f + Util.CursolColor.r * 0.2f;
				tweenColor.from.g = Color.get_white().g * 0.8f + Util.CursolColor.g * 0.2f;
				tweenColor.from.b = Color.get_white().b * 0.8f + Util.CursolColor.b * 0.2f;
				tweenColor.to.r = Util.CursolColor.r;
				tweenColor.to.g = Util.CursolColor.g;
				tweenColor.to.b = Util.CursolColor.b;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.get_gameObject(), 0f, Color.get_white()).duration = 0f;
			}
		}

		public static void SelectedOneObjectBlinkArsenal(GameObject Button, bool value)
		{
			if (value)
			{
				TweenColor tweenColor = TweenColor.Begin(Button.get_gameObject(), 0.2f, Util.CursolColor);
				tweenColor.from = Color.get_white();
				tweenColor.to.r = Color.get_white().r * 0f;
				tweenColor.to.g = Color.get_white().g * 0.63f;
				tweenColor.to.b = Color.get_white().b * 1f;
				tweenColor.duration = Util.CursolBarDurationTime;
				tweenColor.method = UITweener.Method.EaseInOut;
				tweenColor.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenColor.Begin(Button.get_gameObject(), 0f, Color.get_white()).duration = 0f;
			}
		}

		public static void StartListItemBlink(Transform target, Color currentColor)
		{
			TweenColor tweenColor = TweenColor.Begin(target.get_gameObject(), 0.2f, Util.CursolColor);
			tweenColor.from.r = Util.CursolColor.r;
			tweenColor.from.g = Util.CursolColor.g;
			tweenColor.from.b = Util.CursolColor.b;
			tweenColor.value = currentColor;
			tweenColor.to.r = Color.get_white().r * 0.8f + Util.CursolColor.r * 0.2f;
			tweenColor.to.g = Color.get_white().g * 0.8f + Util.CursolColor.g * 0.2f;
			tweenColor.to.b = Color.get_white().b * 0.8f + Util.CursolColor.b * 0.2f;
			tweenColor.duration = Util.CursolBarDurationTime;
			tweenColor.method = UITweener.Method.EaseInOut;
			tweenColor.style = UITweener.Style.PingPong;
		}

		public static Color StopListItemBlink(Transform target)
		{
			Color result = Color.get_white();
			TweenColor component = target.GetComponent<TweenColor>();
			if (component != null)
			{
				result = component.value;
				TweenColor.Begin(target.get_gameObject(), 0f, Color.get_white());
			}
			return result;
		}

		public static void SelectedOneButtonZoomUpDown(UIButton Button, bool value)
		{
			if (!UISelectedObject.ZoomInOut)
			{
				return;
			}
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button.get_gameObject(), Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
				tweenScale.from = Vector3.get_one();
				tweenScale.duration = Util.ButtonDurationTime;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button.get_gameObject(), 0f, Vector3.get_one());
				tweenScale2.from = Vector3.get_one();
				tweenScale2.to = Vector3.get_one();
			}
		}

		public static void SelectedOneButtonZoomUpDown(GameObject Button, bool value)
		{
			if (!UISelectedObject.ZoomInOut)
			{
				return;
			}
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
				tweenScale.from = Vector3.get_one();
				tweenScale.duration = Util.ButtonDurationTime;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.get_one());
				tweenScale2.from = Vector3.get_one();
				tweenScale2.to = Vector3.get_one();
			}
		}

		public static void SelectedOneBoardZoomUpDown(GameObject Button, bool value)
		{
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
				tweenScale.from = Vector3.get_one();
				tweenScale.duration = Util.ButtonDurationTime;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.get_one());
				tweenScale2.from = Vector3.get_one();
				tweenScale2.to = Vector3.get_one();
			}
		}

		public static void SelectedOneBoardZoomUpDownStartup(GameObject Button, bool value)
		{
			if (value)
			{
				TweenScale tweenScale = TweenScale.Begin(Button, Util.ButtonDurationTime * 2f, Vector3.get_one() * 1.01f);
				tweenScale.from = Vector3.get_one();
				tweenScale.duration = Util.ButtonDurationTime * 2f;
				tweenScale.style = UITweener.Style.PingPong;
			}
			else
			{
				TweenScale tweenScale2 = TweenScale.Begin(Button, 0f, Vector3.get_one());
				tweenScale2.from = Vector3.get_one();
				tweenScale2.to = Vector3.get_one();
			}
		}

		public static void SelectedButtonsZoomUpDown(UIButton[] Buttons, int Index)
		{
			if (!UISelectedObject.ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i].get_gameObject(), Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
					tweenScale.from = Vector3.get_one();
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i].get_gameObject(), 0f, Vector3.get_one());
					tweenScale2.from = Vector3.get_one();
					tweenScale2.to = Vector3.get_one();
				}
			}
		}

		public static void SelectedButtonsZoomUpDown(GameObject[] Buttons, int Index)
		{
			if (!UISelectedObject.ZoomInOut)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Index == i)
				{
					TweenScale tweenScale = TweenScale.Begin(Buttons[i], Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
					tweenScale.from = Vector3.get_one();
					tweenScale.duration = Util.ButtonDurationTime;
					tweenScale.style = UITweener.Style.PingPong;
				}
				else
				{
					TweenScale tweenScale2 = TweenScale.Begin(Buttons[i], 0f, Vector3.get_one());
					tweenScale2.from = Vector3.get_one();
					tweenScale2.to = Vector3.get_one();
				}
			}
		}

		public static void SelectDicButtonZoomUpDown<T>(Dictionary<T, UIButton> dictionary, Enum iEnum)
		{
			using (Dictionary<T, UIButton>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<T, UIButton> current = enumerator.get_Current();
					T key = current.get_Key();
					if (key.ToString() == iEnum.ToString())
					{
						TweenScale tweenScale = TweenScale.Begin(current.get_Value().get_gameObject(), Util.ButtonDurationTime, Vector3.get_one() * Util.ButtonZoomUp);
						tweenScale.from = Vector3.get_one();
						tweenScale.duration = Util.ButtonDurationTime;
						tweenScale.style = UITweener.Style.PingPong;
					}
					else
					{
						TweenScale tweenScale2 = TweenScale.Begin(current.get_Value().get_gameObject(), 0f, Vector3.get_one());
						tweenScale2.from = Vector3.get_one();
						tweenScale2.to = Vector3.get_one();
					}
				}
			}
		}
	}
}
