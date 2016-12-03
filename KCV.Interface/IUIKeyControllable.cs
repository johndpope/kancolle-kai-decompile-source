using KCV.Display;
using System;

namespace KCV.Interface
{
	public interface IUIKeyControllable
	{
		void SetKeyController(KeyControl keyController);

		void SetKeyController(KeyControl keyController, UIDisplaySwipeEventRegion region);

		void OnUpdatedKeyController();

		void OnReleaseKeyController();
	}
}
