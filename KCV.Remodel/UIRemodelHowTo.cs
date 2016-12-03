using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget)), SelectionBase]
	public class UIRemodelHowTo : UIHowTo, UIRemodelView
	{
		virtual void Show()
		{
			base.Show();
		}

		virtual void Hide()
		{
			base.Hide();
		}
	}
}
