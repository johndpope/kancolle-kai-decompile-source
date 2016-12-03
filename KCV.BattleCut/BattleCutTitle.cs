using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BattleCutTitle : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiPhaseTitle;

		private UIPanel _uiPanel;

		private Dictionary<BattleCutPhase, string> _dicPhaseTitle;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static BattleCutTitle Instantiate(BattleCutTitle prefab, Transform parent, Vector3 pos)
		{
			return Object.Instantiate<BattleCutTitle>(prefab);
		}

		private void Awake()
		{
			this._dicPhaseTitle = new Dictionary<BattleCutPhase, string>();
			this._dicPhaseTitle.Add(BattleCutPhase.BattleCutPhase_ST, "Formation");
			this._dicPhaseTitle.Add(BattleCutPhase.Command, "Command");
			this._dicPhaseTitle.Add(BattleCutPhase.DayBattle, "Battle");
			this._dicPhaseTitle.Add(BattleCutPhase.Battle_End, "Battle End");
			this._dicPhaseTitle.Add(BattleCutPhase.WithdrawalDecision, "Decision");
			this._dicPhaseTitle.Add(BattleCutPhase.Judge, "Result");
			this._dicPhaseTitle.Add(BattleCutPhase.NightBattle, "NightBattle");
			this._dicPhaseTitle.Add(BattleCutPhase.Result, "Result");
			this._dicPhaseTitle.Add(BattleCutPhase.AdvancingWithdrawal, "Decision");
			this.panel.widgetsAreStatic = true;
		}

		private void OnDestroy()
		{
			this._uiPhaseTitle.get_transform().LTCancel();
			this._uiPhaseTitle = null;
			this._uiPanel = null;
			Mem.DelDictionarySafe<BattleCutPhase, string>(ref this._dicPhaseTitle);
		}

		public void SetPhaseText(BattleCutPhase iPhase)
		{
			if (!this._dicPhaseTitle.ContainsKey(iPhase))
			{
				return;
			}
			if (this._uiPhaseTitle.text != null && this._uiPhaseTitle.text == this._dicPhaseTitle.get_Item(iPhase))
			{
				return;
			}
			this.panel.widgetsAreStatic = false;
			float fadeTime = 0.2f;
			this._uiPhaseTitle.get_transform().LTCancel();
			this._uiPhaseTitle.get_transform().LTValue(this._uiPhaseTitle.alpha, 0f, fadeTime).setOnUpdate(delegate(float x)
			{
				this._uiPhaseTitle.alpha = x;
			}).setEase(LeanTweenType.linear).setOnComplete(delegate
			{
				this._uiPhaseTitle.text = this._dicPhaseTitle.get_Item(iPhase);
				this._uiPhaseTitle.get_transform().LTValue(this._uiPhaseTitle.alpha, 0.5f, fadeTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiPhaseTitle.alpha = x;
				}).setOnComplete(delegate
				{
					this.panel.widgetsAreStatic = true;
				});
			});
		}
	}
}
