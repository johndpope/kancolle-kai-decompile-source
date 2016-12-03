using System;
using UnityEngine;

namespace KCV
{
	public class UIShortCutTruss : MonoBehaviour
	{
		private TweenPosition tp;

		[SerializeField]
		private UIShortCutCrane Crane;

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
			this.Crane.StartAnimationNoReset();
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
				if (!this.isEnter)
				{
					this.Crane.AnimStop();
				}
			});
		}
	}
}
