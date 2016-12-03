using KCV.Tutorial.Guide;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class TutorialGuideManager : SingletonMonoBehaviour<TutorialGuideManager>
	{
		public enum TutorialID
		{
			Step1,
			Step2,
			Step3,
			Step3_2,
			Step4,
			Step5,
			Step6,
			Step7,
			Step8,
			Step9,
			StrategyText,
			PortTopText,
			BattleCommand,
			RepairInfo,
			SupplyInfo,
			StrategyPoint,
			BattleShortCutInfo,
			Raider,
			RebellionPreparation,
			Rebellion_EnableIntercept,
			Rebellion_DisableIntercept,
			Rebellion_CombinedFleet,
			Rebellion_Lose,
			ResourceRecovery,
			TankerDeploy,
			EscortOrganize,
			Bring,
			BuildShip,
			SpeedBuild,
			Organize,
			EndGame
		}

		private TutorialDialog tutorialDialog;

		private ResourceRequest req;

		public TutorialModel model;

		[SerializeField]
		private bool[] Debug_TutorialFlag;

		public TutorialGuideManager.TutorialID Debug_TutorialID;

		public TutorialDialog GetTutorialDialog()
		{
			return this.tutorialDialog;
		}

		public bool isRequest()
		{
			return this.req != null;
		}

		public bool CheckAndShowFirstTutorial(TutorialModel model, TutorialGuideManager.TutorialID ID, Action OnLoaded)
		{
			if (!model.GetKeyTutorialFlg((int)ID) && this.CheckTutorialCondition(ID) && this.req == null)
			{
				base.StartCoroutine(this.LoadTutorial(model, ID, OnLoaded));
				return true;
			}
			return false;
		}

		public bool CheckAndShowFirstTutorial(TutorialModel model, TutorialGuideManager.TutorialID ID, Action OnLoaded, Action OnFinished)
		{
			Action onLoaded = delegate
			{
				if (OnLoaded != null)
				{
					OnLoaded.Invoke();
				}
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.GetTutorialDialog().OnClosed = OnFinished;
			};
			if (!model.GetKeyTutorialFlg((int)ID) && this.CheckTutorialCondition(ID) && this.req == null)
			{
				base.StartCoroutine(this.LoadTutorial(model, ID, onLoaded));
				return true;
			}
			if (OnFinished != null)
			{
				OnFinished.Invoke();
			}
			return false;
		}

		public void ShowFirstTutorial(TutorialModel model, TutorialGuideManager.TutorialID ID, Action OnLoaded)
		{
			base.StartCoroutine(this.LoadTutorial(model, ID, OnLoaded));
		}

		public bool CheckFirstTutorial(TutorialModel model, TutorialGuideManager.TutorialID ID)
		{
			return !model.GetKeyTutorialFlg((int)ID) && this.CheckTutorialCondition(ID) && this.req == null;
		}

		[DebuggerHidden]
		private IEnumerator LoadTutorial(TutorialModel model, TutorialGuideManager.TutorialID ID, Action OnLoaded)
		{
			TutorialGuideManager.<LoadTutorial>c__Iterator42 <LoadTutorial>c__Iterator = new TutorialGuideManager.<LoadTutorial>c__Iterator42();
			<LoadTutorial>c__Iterator.ID = ID;
			<LoadTutorial>c__Iterator.model = model;
			<LoadTutorial>c__Iterator.OnLoaded = OnLoaded;
			<LoadTutorial>c__Iterator.<$>ID = ID;
			<LoadTutorial>c__Iterator.<$>model = model;
			<LoadTutorial>c__Iterator.<$>OnLoaded = OnLoaded;
			<LoadTutorial>c__Iterator.<>f__this = this;
			return <LoadTutorial>c__Iterator;
		}

		private bool CheckTutorialCondition(TutorialGuideManager.TutorialID ID)
		{
			if (ID == TutorialGuideManager.TutorialID.RepairInfo)
			{
				return Enumerable.Any<ShipModel>(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips(), (ShipModel x) => (float)(x.NowHp / x.MaxHp) < 0.7f);
			}
			if (ID != TutorialGuideManager.TutorialID.SupplyInfo)
			{
				return true;
			}
			return Enumerable.Any<ShipModel>(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetShips(), (ShipModel x) => x.AmmoRate < 50.0 || x.FuelRate < 50.0);
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<TutorialGuideManager>.instance = null;
			this.tutorialDialog = null;
			this.req = null;
			this.model = null;
		}
	}
}
