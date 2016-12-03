using local.managers;
using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyOrganizeGuideMessage : MonoBehaviour
	{
		public bool isVisible
		{
			get;
			private set;
		}

		private void Start()
		{
			base.GetComponent<UIWidget>().alpha = 0f;
			this.isVisible = true;
			this.setVisible(false);
		}

		public void UpdateVisible()
		{
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			if (Enumerable.Any<DeckModel>(logicManager.Area.get_Item(1).GetDecks(), (DeckModel x) => x.GetFlagShip() == null))
			{
				this.setVisible(true);
			}
			else
			{
				this.setVisible(false);
			}
		}

		public void setVisible(bool isVisible)
		{
			if (this.isVisible == isVisible)
			{
				return;
			}
			this.isVisible = isVisible;
			if (isVisible)
			{
				TweenAlpha ta = base.GetComponent<TweenAlpha>();
				ta.onFinished.Clear();
				ta.to = 1f;
				ta.from = 0f;
				ta.duration = 1f;
				ta.style = UITweener.Style.Once;
				ta.SetOnFinished(delegate
				{
					ta.onFinished.Clear();
					ta.from = 1f;
					ta.to = 0.6f;
					ta.style = UITweener.Style.PingPong;
					ta.duration = 2f;
					ta.ResetToBeginning();
					this.DelayActionFrame(1, delegate
					{
						ta.PlayForward();
					});
				});
				ta.ResetToBeginning();
				this.DelayActionFrame(1, delegate
				{
					ta.PlayForward();
				});
			}
			else
			{
				TweenAlpha ta = base.GetComponent<TweenAlpha>();
				ta.onFinished.Clear();
				ta.duration = 0.3f;
				ta.from = base.GetComponent<UIWidget>().alpha;
				ta.to = 0f;
				ta.style = UITweener.Style.Once;
				ta.ResetToBeginning();
				ta.SetOnFinished(delegate
				{
				});
				this.DelayActionFrame(1, delegate
				{
					ta.PlayForward();
				});
			}
		}
	}
}
