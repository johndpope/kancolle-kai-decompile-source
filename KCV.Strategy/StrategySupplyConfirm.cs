using local.managers;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySupplyConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel AmmoNum;

		[SerializeField]
		private UILabel FuelNum;

		[SerializeField]
		private YesNoButton YesNoBtn;

		private TweenScale ts;

		private void Awake()
		{
			base.get_transform().localScaleZero();
			this.ts = base.GetComponent<TweenScale>();
		}

		private void OnDestroy()
		{
			this.AmmoNum = null;
			this.FuelNum = null;
			this.YesNoBtn = null;
		}

		public void SetModel(SupplyManager manager)
		{
			this.AmmoNum.textInt = manager.AmmoForSupply;
			this.FuelNum.textInt = manager.FuelForSupply;
			this.AmmoNum.color = ((manager.AmmoForSupply <= manager.Material.Ammo) ? Color.get_black() : Color.get_red());
			this.FuelNum.color = ((manager.FuelForSupply <= manager.Material.Fuel) ? Color.get_black() : Color.get_red());
		}

		public void Open()
		{
			this.ts.onFinished.Clear();
			this.ts.PlayForward();
			this.YesNoBtn.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f), true);
		}

		[DebuggerHidden]
		public IEnumerator Close()
		{
			StrategySupplyConfirm.<Close>c__Iterator146 <Close>c__Iterator = new StrategySupplyConfirm.<Close>c__Iterator146();
			<Close>c__Iterator.<>f__this = this;
			return <Close>c__Iterator;
		}

		public void SetOnSelectPositive(Action act)
		{
			this.YesNoBtn.SetOnSelectPositiveListener(act);
		}

		public void SetOnSelectNeagtive(Action act)
		{
			this.YesNoBtn.SetOnSelectNegativeListener(act);
		}
	}
}
