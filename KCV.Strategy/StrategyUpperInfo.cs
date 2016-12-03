using Common.Struct;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUpperInfo : MonoBehaviour
	{
		private const float RotateSpeed = -10f;

		[SerializeField]
		private UILabel Year;

		[SerializeField]
		private UILabel Month;

		[SerializeField]
		private UILabel Day;

		[SerializeField]
		private UILabel MovingTankerNum;

		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private UISprite UpperIconLabel;

		public void Update()
		{
			this.UpperIconLabel.get_gameObject().get_transform().Rotate(0f, 0f, -10f * Time.get_deltaTime(), 0);
		}

		public void UpdateUpperInfo()
		{
			this.UpdateMovingTankerNum();
			this.UpdateNonMoveTankerNum();
			this.UpdateDayLabel();
		}

		public void Enter()
		{
			this.UpdateUpperInfo();
			this.TweenPos.PlayForward();
		}

		public void Exit()
		{
			this.TweenPos.PlayReverse();
		}

		private void UpdateMovingTankerNum()
		{
			this.MovingTankerNum.textInt = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountMove();
		}

		private void UpdateNonMoveTankerNum()
		{
			this.TankerNum.textInt = StrategyTopTaskManager.GetLogicManager().GetNonDeploymentTankerCount().GetCountNoMove();
		}

		private void UpdateDayLabel()
		{
			TurnString datetimeString = StrategyTopTaskManager.GetLogicManager().DatetimeString;
			this.Year.text = datetimeString.Year + "の年";
			this.Month.text = datetimeString.Month;
			this.Day.text = datetimeString.Day;
		}
	}
}
