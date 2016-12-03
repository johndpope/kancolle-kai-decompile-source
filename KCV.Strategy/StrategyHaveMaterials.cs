using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHaveMaterials : MonoBehaviour
	{
		public UILabel[] MaterialNum;

		public GameObject ParentObject;

		private int[] MaterialsNumInt;

		private int[] PrevMaterialsNumInt;

		private bool isInitialize;

		private Coroutine[] coroutines;

		private void Awake()
		{
			this.MaterialsNumInt = new int[this.MaterialNum.Length];
			this.PrevMaterialsNumInt = new int[this.MaterialNum.Length];
			this.coroutines = new Coroutine[this.MaterialNum.Length];
		}

		public void Initialize()
		{
			this.UpdateNum();
			this.UpdateLabel();
			for (int i = 0; i < 4; i++)
			{
				this.UpdateColor(i);
			}
			this.isInitialize = true;
		}

		public void UpdateFooterMaterials()
		{
			if (this.isInitialize)
			{
				this.UpdateNumAnimation();
			}
			else
			{
				this.Initialize();
			}
		}

		private void UpdateNumAnimation()
		{
			for (int i = 0; i < this.MaterialNum.Length; i++)
			{
				this.PrevMaterialsNumInt[i] = this.MaterialsNumInt[i];
			}
			this.UpdateNum();
			for (int j = 0; j < this.MaterialNum.Length; j++)
			{
				if (this.MaterialsNumInt[j] != this.PrevMaterialsNumInt[j] && this.coroutines[j] == null)
				{
					this.coroutines[j] = base.StartCoroutine(this.ChangeNumAnimation(this.MaterialNum[j], j));
				}
			}
		}

		private void UpdateNum()
		{
			this.MaterialsNumInt[0] = StrategyTopTaskManager.GetLogicManager().Material.Fuel;
			this.MaterialsNumInt[1] = StrategyTopTaskManager.GetLogicManager().Material.Steel;
			this.MaterialsNumInt[2] = StrategyTopTaskManager.GetLogicManager().Material.Ammo;
			this.MaterialsNumInt[3] = StrategyTopTaskManager.GetLogicManager().Material.Baux;
			this.MaterialsNumInt[4] = StrategyTopTaskManager.GetLogicManager().Material.Devkit;
			this.MaterialsNumInt[5] = StrategyTopTaskManager.GetLogicManager().Material.RepairKit;
		}

		private void UpdateLabel()
		{
			for (int i = 0; i < this.MaterialNum.Length; i++)
			{
				this.MaterialNum[i].text = this.MaterialsNumInt[i].ToString();
			}
		}

		[DebuggerHidden]
		private IEnumerator ChangeNumAnimation(UILabel label, int LabelNo)
		{
			StrategyHaveMaterials.<ChangeNumAnimation>c__Iterator16F <ChangeNumAnimation>c__Iterator16F = new StrategyHaveMaterials.<ChangeNumAnimation>c__Iterator16F();
			<ChangeNumAnimation>c__Iterator16F.LabelNo = LabelNo;
			<ChangeNumAnimation>c__Iterator16F.label = label;
			<ChangeNumAnimation>c__Iterator16F.<$>LabelNo = LabelNo;
			<ChangeNumAnimation>c__Iterator16F.<$>label = label;
			<ChangeNumAnimation>c__Iterator16F.<>f__this = this;
			return <ChangeNumAnimation>c__Iterator16F;
		}

		private void UpdateColor(int LabelNo)
		{
			int materialMaxNum = StrategyTopTaskManager.GetLogicManager().UserInfo.GetMaterialMaxNum();
			this.MaterialNum[LabelNo].color = ((materialMaxNum > this.MaterialsNumInt[LabelNo]) ? Color.get_white() : Color.get_yellow());
		}

		private void TweenLabel(int LabelNo, int from, int to)
		{
			Tween tween = new UINumberCounter(this.MaterialNum[LabelNo]).SetFrom(from).SetTo(to).SetDuration(0.5f).SetAnimationType(UINumberCounter.AnimationType.Count).SetOnFinishedCallBack(delegate
			{
				if (LabelNo < 4)
				{
					this.UpdateColor(LabelNo);
				}
			}).Buld();
			TweenExtensions.Play<Tween>(tween);
		}
	}
}
