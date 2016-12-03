using System;
using UnityEngine;

namespace KCV
{
	public class UIShortCutGears : MonoBehaviour
	{
		private TweenPosition tp;

		[SerializeField]
		private TweenRotation[] tweenRots;

		private bool isEnter;

		private void Awake()
		{
			this.tp = base.GetComponent<TweenPosition>();
		}

		public void Enter()
		{
			if (this.isEnter)
			{
				return;
			}
			this.isEnter = true;
			this.tp.onFinished.Clear();
			this.tp.PlayForward();
			TweenRotation[] array = this.tweenRots;
			for (int i = 0; i < array.Length; i++)
			{
				TweenRotation tweenRotation = array[i];
				tweenRotation.PlayForward();
			}
		}

		public void Exit()
		{
			if (!this.isEnter)
			{
				return;
			}
			this.isEnter = false;
			this.tp.PlayReverse();
			this.tp.SetOnFinished(delegate
			{
				TweenRotation[] array = this.tweenRots;
				for (int i = 0; i < array.Length; i++)
				{
					TweenRotation tweenRotation = array[i];
					tweenRotation.set_enabled(false);
				}
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.isCloseAnimNow = false;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.SetActiveChildren(false);
			});
		}
	}
}
