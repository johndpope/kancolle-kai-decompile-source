using System;

namespace KCV.View.Scroll
{
	public interface UIScrollListParentHandler<View>
	{
		void OnSelect(int index, View child);

		void OnChangeFocus(int index, View child);

		void OnCancel();
	}
}
