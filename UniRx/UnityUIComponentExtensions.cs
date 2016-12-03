using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx
{
	public static class UnityUIComponentExtensions
	{
		public static IDisposable SubscribeToText(this IObservable<string> source, Text text)
		{
			return source.Subscribe(delegate(string x)
			{
				text.set_text(x);
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text)
		{
			return source.Subscribe(delegate(T x)
			{
				text.set_text(x.ToString());
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text, Func<T, string> selector)
		{
			return source.Subscribe(delegate(T x)
			{
				text.set_text(selector.Invoke(x));
			});
		}

		public static IDisposable SubscribeToInteractable(this IObservable<bool> source, Selectable selectable)
		{
			return source.Subscribe(delegate(bool x)
			{
				selectable.set_interactable(x);
			});
		}

		public static IObservable<Unit> OnClickAsObservable(this Button button)
		{
			return button.get_onClick().AsObservable();
		}

		public static IObservable<bool> OnValueChangedAsObservable(this Toggle toggle)
		{
			return Observable.Create<bool>(delegate(IObserver<bool> observer)
			{
				observer.OnNext(toggle.get_isOn());
				return toggle.onValueChanged.AsObservable<bool>().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Scrollbar scrollbar)
		{
			return Observable.Create<float>(delegate(IObserver<float> observer)
			{
				observer.OnNext(scrollbar.get_value());
				return scrollbar.get_onValueChanged().AsObservable<float>().Subscribe(observer);
			});
		}

		public static IObservable<Vector2> OnValueChangedAsObservable(this ScrollRect scrollRect)
		{
			return Observable.Create<Vector2>(delegate(IObserver<Vector2> observer)
			{
				observer.OnNext(scrollRect.get_normalizedPosition());
				return scrollRect.get_onValueChanged().AsObservable<Vector2>().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Slider slider)
		{
			return Observable.Create<float>(delegate(IObserver<float> observer)
			{
				observer.OnNext(slider.get_value());
				return slider.get_onValueChanged().AsObservable<float>().Subscribe(observer);
			});
		}

		public static IObservable<string> OnEndEditAsObservable(this InputField inputField)
		{
			return inputField.get_onEndEdit().AsObservable<string>();
		}

		public static IObservable<string> OnValueChangeAsObservable(this InputField inputField)
		{
			return Observable.Create<string>(delegate(IObserver<string> observer)
			{
				observer.OnNext(inputField.get_text());
				return inputField.get_onValueChange().AsObservable<string>().Subscribe(observer);
			});
		}
	}
}
