using System;
using UnityEngine;

namespace UniRx
{
	public interface IPresenter
	{
		IPresenter Parent
		{
			get;
		}

		GameObject gameObject
		{
			get;
		}

		void RegisterParent(IPresenter parent);

		void InitializeCore();

		void StartCapturePhase();
	}
}
