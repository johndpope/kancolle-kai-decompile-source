using KCV.EscortOrganize;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class TaskDeployTop : SceneTaskMono
	{
		[SerializeField]
		private DeployMainPanel MainPanel;

		[SerializeField]
		private DeployTransportPanel TransportPanel;

		private KeyControl CommandKeyController;

		public bool isDeployPanel;

		public bool isEscortPanel;

		public int TankerCount;

		public int areaID;

		public bool isChangeMode;

		private bool isInit;

		private bool isGoeiChange;

		private void Start()
		{
			this.MainPanel.SetActive(false);
			this.TransportPanel.SetActive(false);
		}

		protected override bool Init()
		{
			this.MainPanel.GetComponent<TweenAlpha>().onFinished.Clear();
			this.MainPanel.SetActive(true);
			this.TransportPanel.SetActive(true);
			StrategyTopTaskManager.Instance.setActiveStrategy(false);
			if (!this.isInit)
			{
				this.isGoeiChange = true;
				this.areaID = StrategyAreaManager.FocusAreaID;
				this.CommandKeyController = KeyControlManager.Instance.KeyController;
				this.TankerCount = StrategyTopTaskManager.GetLogicManager().Area.get_Item(this.areaID).GetTankerCount().GetCount();
				EscortOrganizeTaskManager.CreateLogicManager();
			}
			this.isChangeMode = true;
			this.DelayActionFrame(3, delegate
			{
				this.isInit = true;
			});
			return true;
		}

		protected override bool UnInit()
		{
			this.isGoeiChange = true;
			return true;
		}

		protected override bool Run()
		{
			if (!this.isInit)
			{
				return true;
			}
			if (this.isDeployPanel)
			{
				if (this.isChangeMode)
				{
					this.TransportPanel.Init();
					this.isGoeiChange = false;
				}
				return this.TransportPanel.Run();
			}
			if (this.isChangeMode)
			{
				this.MainPanel.Init(this.isGoeiChange);
			}
			return this.MainPanel.Run();
		}

		public void backToCommandMenu()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.DelayActionFrame(1, delegate
			{
				this.MainPanel.SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				TweenAlpha component = this.MainPanel.GetComponent<TweenAlpha>();
				StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(false, null);
				component.onFinished.Clear();
				component.SetOnFinished(delegate
				{
					this.DelayAction(0.15f, delegate
					{
						this.MainPanel.SetActive(false);
						this.TransportPanel.SetActive(false);
						StrategyTopTaskManager.Instance.GetInfoMng().changeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
						this.MainPanel.DestroyEscortOrganize();
						this.DelayActionFrame(3, delegate
						{
							KeyControlManager.Instance.KeyController = this.CommandKeyController;
							StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
							StrategyTaskManager.SceneCallBack();
						});
					});
				});
				base.Close();
				this.isInit = false;
			});
		}

		private void OnDestroy()
		{
			this.MainPanel = null;
			this.TransportPanel = null;
			this.CommandKeyController = null;
		}
	}
}
