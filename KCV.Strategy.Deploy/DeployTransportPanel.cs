using KCV.EscortOrganize;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployTransportPanel : MonoBehaviour
	{
		[SerializeField]
		private UISprite upButton;

		[SerializeField]
		private UISprite downButton;

		[SerializeField]
		private UILabel mLabel_UsedTankerValue;

		[SerializeField]
		private UILabel needTankerNum;

		[SerializeField]
		private UILabel mLabel_SetTankerValue;

		[SerializeField]
		private DeployMaterials deployMaterials;

		[SerializeField]
		private TaskDeployTop top;

		private KeyControl TransportKeyController;

		private StrategyMapManager mStrategyMapManager;

		private int mMaximumSetTankerValue;

		private int mMinimumSetTankerValue;

		private int mSetTankerValue;

		private int mAlreadySetTankerValue;

		private int mOwnUsableTankerValue;

		public void Init()
		{
			this.TransportKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.mStrategyMapManager = StrategyTopTaskManager.GetLogicManager();
			this.mMinimumSetTankerValue = 0;
			this.mMaximumSetTankerValue = 30;
			this.mAlreadySetTankerValue = this.mStrategyMapManager.Area.get_Item(this.top.areaID).GetTankerCount().GetCount();
			this.mOwnUsableTankerValue = this.mStrategyMapManager.GetNonDeploymentTankerCount().GetCountNoMove();
			if (this.mOwnUsableTankerValue + this.mAlreadySetTankerValue < 30)
			{
				this.mMaximumSetTankerValue = this.mOwnUsableTankerValue + this.mAlreadySetTankerValue;
			}
			this.needTankerNum.textInt = this.mStrategyMapManager.Area.get_Item(this.top.areaID).GetTankerCount().GetReqCount();
			this.mSetTankerValue = this.mAlreadySetTankerValue + (this.top.TankerCount - this.mAlreadySetTankerValue);
			this.OnUpdatedTankerValue(this.mSetTankerValue);
			base.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			this.top.isChangeMode = false;
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			bool flag = SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.TankerDeploy, null, delegate
			{
				this.TransportKeyController.IsRun = true;
			});
			if (flag)
			{
				this.TransportKeyController.IsRun = false;
			}
		}

		public bool Run()
		{
			this.TransportKeyController.Update();
			if (this.TransportKeyController.keyState.get_Item(1).down || this.TransportKeyController.keyState.get_Item(0).down)
			{
				this.Back();
			}
			else if (this.TransportKeyController.keyState.get_Item(8).down)
			{
				this.UpTankerValue();
			}
			else if (this.TransportKeyController.keyState.get_Item(12).down)
			{
				this.DownTankerValue();
			}
			else if (this.TransportKeyController.keyState.get_Item(5).down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
			}
			return true;
		}

		[Obsolete("外部UI[輸送船増ボタン]から参照して使用します")]
		public void OnClickUpTankerValue()
		{
			this.UpTankerValue();
		}

		[Obsolete("外部UI[輸送船減ボタン]から参照して使用します")]
		public void OnClickDownTankerValue()
		{
			this.DownTankerValue();
		}

		[Obsolete("外部UI[バックボタン（背景）]から参照して使用します")]
		public void OnClickBack()
		{
			this.Back();
		}

		private void UpTankerValue()
		{
			if (!this.TransportKeyController.IsRun)
			{
				return;
			}
			bool flag = this.RangeTanker(this.mSetTankerValue + 1, this.mMinimumSetTankerValue, this.mMaximumSetTankerValue);
			if (flag)
			{
				this.mSetTankerValue++;
				this.OnUpdatedTankerValue(this.mSetTankerValue);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void DownTankerValue()
		{
			if (!this.TransportKeyController.IsRun)
			{
				return;
			}
			bool flag = this.RangeTanker(this.mSetTankerValue - 1, this.mMinimumSetTankerValue, this.mMaximumSetTankerValue);
			if (flag)
			{
				this.mSetTankerValue--;
				this.OnUpdatedTankerValue(this.mSetTankerValue);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		private void OnUpdatedTankerValue(int tankerValue)
		{
			this.mLabel_SetTankerValue.text = tankerValue.ToString();
			int num = this.mOwnUsableTankerValue + (this.mAlreadySetTankerValue - tankerValue);
			this.mLabel_UsedTankerValue.text = num.ToString();
			this.deployMaterials.updateMaterials(this.top.areaID, tankerValue, EscortOrganizeTaskManager.GetEscortManager());
		}

		private bool RangeTanker(int checkValue, int minimumValue, int maximumValue)
		{
			return minimumValue <= checkValue && checkValue <= maximumValue;
		}

		private void Back()
		{
			if (!this.TransportKeyController.IsRun)
			{
				return;
			}
			if (this.top.isDeployPanel)
			{
				base.get_gameObject().SafeGetTweenAlpha(1f, 0f, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				this.top.TankerCount = this.mSetTankerValue;
				this.top.isDeployPanel = false;
				this.top.isChangeMode = true;
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			}
		}
	}
}
