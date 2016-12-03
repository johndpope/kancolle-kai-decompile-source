using System;
using UnityEngine;

namespace KCV.Battle
{
	public class TorpedoHpGauges
	{
		private BattleHPGauges FBattleHpGauges;

		private BattleHPGauges EBattleHpGauges;

		private UIPanel UiHpGaugePanel;

		public BattleHPGauges FHpGauge
		{
			get
			{
				return this.FBattleHpGauges;
			}
			set
			{
				this.FBattleHpGauges = value;
			}
		}

		public BattleHPGauges EHpGauge
		{
			get
			{
				return this.EBattleHpGauges;
			}
			set
			{
				this.EBattleHpGauges = value;
			}
		}

		public UIPanel UiPanel
		{
			get
			{
				return this.UiHpGaugePanel;
			}
			set
			{
				this.UiHpGaugePanel = value;
			}
		}

		public void Hide()
		{
			if (this.UiHpGaugePanel != null)
			{
				this.UiHpGaugePanel.alpha = 0f;
			}
		}

		public void SetDestroy()
		{
			if (this.FBattleHpGauges != null)
			{
				this.FBattleHpGauges.Dispose();
			}
			if (this.EBattleHpGauges != null)
			{
				this.EBattleHpGauges.Dispose();
			}
			Mem.Del<BattleHPGauges>(ref this.FBattleHpGauges);
			Mem.Del<BattleHPGauges>(ref this.EBattleHpGauges);
			if (this.UiHpGaugePanel != null)
			{
				Object.Destroy(this.UiHpGaugePanel.get_gameObject());
			}
			Mem.Del<UIPanel>(ref this.UiHpGaugePanel);
		}

		public void InstancePanel(GameObject panel, GameObject parent)
		{
			if (parent.get_transform().FindChild("UICircleHpPanel"))
			{
				this.UiHpGaugePanel = parent.get_transform().FindChild("UICircleHpPanel").GetComponent<UIPanel>();
				this.UiHpGaugePanel.alpha = 0f;
			}
			else
			{
				this.UiHpGaugePanel = Util.Instantiate(panel, parent, false, false).GetComponent<UIPanel>();
				this.UiHpGaugePanel.alpha = 0f;
			}
		}
	}
}
