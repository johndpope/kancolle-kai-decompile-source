using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySideAreaInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel TankerNum;

		[SerializeField]
		private StrategySideEscortDeck escortDeck;

		[SerializeField]
		private UILabel[] TurnGetMaterialNums;

		[SerializeField]
		private UIWidget myUIWidget;

		public void UpdateSideAreaPanel()
		{
			MapAreaModel focusAreaModel = StrategyTopTaskManager.Instance.GetAreaMng().FocusAreaModel;
			int countNoMove = focusAreaModel.GetTankerCount().GetCountNoMove();
			int countMove = focusAreaModel.GetTankerCount().GetCountMove();
			this.setTankerCount(countNoMove, countMove);
			this.escortDeck.UpdateEscortDeck(focusAreaModel.GetEscortDeck());
			this.setMaterialNums(focusAreaModel, countNoMove);
		}

		private void setTankerCount(int tankerCount, int moveTankerCount)
		{
			if (moveTankerCount == 0)
			{
				this.TankerNum.text = "× " + tankerCount;
			}
			else
			{
				this.TankerNum.text = string.Concat(new object[]
				{
					"× ",
					tankerCount,
					" + ",
					moveTankerCount
				});
			}
		}

		private void setMaterialNums(MapAreaModel areaModel, int tankerCount)
		{
			this.TurnGetMaterialNums[0].text = "× " + areaModel.GetResources(tankerCount).get_Item(enumMaterialCategory.Fuel);
			this.TurnGetMaterialNums[1].text = "× " + areaModel.GetResources(tankerCount).get_Item(enumMaterialCategory.Bull);
			this.TurnGetMaterialNums[2].text = "× " + areaModel.GetResources(tankerCount).get_Item(enumMaterialCategory.Steel);
			this.TurnGetMaterialNums[3].text = "× " + areaModel.GetResources(tankerCount).get_Item(enumMaterialCategory.Bauxite);
		}

		public void EnterAreaInfoPanel(float delay)
		{
			this.UpdateSideAreaPanel();
			TweenAlpha.Begin(base.get_gameObject(), 0.2f, 1f).delay = delay;
		}

		public void ExitAreaInfoPanel()
		{
			TweenAlpha.Begin(base.get_gameObject(), 0.2f, 0f).delay = 0f;
		}

		public void setVisible(bool isVisible)
		{
			if (isVisible)
			{
				this.myUIWidget.alpha = 1f;
			}
			else
			{
				this.myUIWidget.alpha = 0f;
			}
		}
	}
}
