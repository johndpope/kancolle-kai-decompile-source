using KCV.Scene.Port;
using KCV.Scene.Strategy.Result;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Mission
{
	public class UIMissionResultStatus : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_DeckName;

		[SerializeField]
		private UIStrategyResultShipInfo[] mUIStrategyResultShipInfos;

		private MissionResultModel mMissionResultModel;

		public void Inititalize(MissionResultModel missionResultModel)
		{
			this.mMissionResultModel = missionResultModel;
			ShipModel[] ships = this.mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				this.mUIStrategyResultShipInfos[i].SetActive(false);
				this.mUIStrategyResultShipInfos[i].Initialize(i, shipModel, missionResultModel.GetShipExpInfo(shipModel.MemId));
			}
		}

		private void ChainAnimation(Action<Action> chainFrom, Action<Action> chainTo, Action onFinished)
		{
			chainFrom.Invoke(delegate
			{
				chainTo.Invoke(delegate
				{
					if (onFinished != null)
					{
						onFinished.Invoke();
					}
				});
			});
		}

		private void ShowBanners(Action onFinished)
		{
			ShipModel[] ships = this.mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				this.mUIStrategyResultShipInfos[i].SetActive(true);
				if (i == ships.Length - 1)
				{
					this.mUIStrategyResultShipInfos[i].PlayShowBannerAnimation(onFinished);
				}
				else
				{
					this.mUIStrategyResultShipInfos[i].PlayShowBannerAnimation(null);
				}
			}
		}

		private void ShowStatuses(Action onFinished)
		{
			ShipModel[] ships = this.mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				if (i == ships.Length - 1)
				{
					this.mUIStrategyResultShipInfos[i].PlayShowStatusAnimation(onFinished);
				}
				else
				{
					this.mUIStrategyResultShipInfos[i].PlayShowStatusAnimation(null);
				}
			}
		}

		private void ShowExpUpdate(Action onFinished)
		{
			ShipModel[] ships = this.mMissionResultModel.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				if (i == ships.Length - 1)
				{
					this.mUIStrategyResultShipInfos[i].PlayExpAnimation(onFinished);
				}
				else
				{
					this.mUIStrategyResultShipInfos[i].PlayExpAnimation(null);
				}
			}
		}

		public void PlayShowBanners(Action onFinished)
		{
			this.ChainAnimation(new Action<Action>(this.ShowBanners), new Action<Action>(this.ShowStatuses), onFinished);
		}

		public void PlayShowBannersExp(Action onFinished)
		{
			this.ShowExpUpdate(onFinished);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_DeckName);
			if (this.mUIStrategyResultShipInfos != null)
			{
				for (int i = 0; i < this.mUIStrategyResultShipInfos.Length; i++)
				{
					this.mUIStrategyResultShipInfos[i] = null;
				}
			}
			this.mUIStrategyResultShipInfos = null;
			this.mMissionResultModel = null;
		}
	}
}
