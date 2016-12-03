using KCV.View.Scroll;
using System;

namespace KCV.Remodel
{
	public abstract class AbstractUIRemodelListChild<T> : UIScrollListChild<T> where T : class
	{
		public override void Show()
		{
			base.Show();
			this.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			this.SetActive(false);
		}
	}
}
