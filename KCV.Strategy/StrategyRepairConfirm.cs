using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyRepairConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UILabel ShipLV;

		[SerializeField]
		private UILabel SteelNum;

		[SerializeField]
		private UILabel FuelNum;

		[SerializeField]
		private UILabel NeedDay;

		[SerializeField]
		private UILabel UseKit;

		[SerializeField]
		private YesNoButton YesNoBtn;

		[SerializeField]
		private CommonShipBanner shipBanner;

		private TweenScale ts;

		private void Awake()
		{
			base.get_transform().localScaleZero();
			this.ts = base.GetComponent<TweenScale>();
		}

		private void OnDestroy()
		{
			this.ShipName = null;
			this.ShipLV = null;
			this.SteelNum = null;
			this.FuelNum = null;
			this.NeedDay = null;
			this.UseKit = null;
			this.YesNoBtn = null;
		}

		public void SetModel(ShipModel ship)
		{
			this.ShipName.text = ship.Name;
			this.ShipLV.textInt = ship.Level;
			this.SteelNum.textInt = ship.GetResourcesForRepair().Steel;
			this.FuelNum.textInt = ship.GetResourcesForRepair().Fuel;
			this.NeedDay.textInt = ship.RepairTime;
			this.UseKit.text = "使用しない";
			this.shipBanner.SetShipData(ship);
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
			StrategyRepairConfirm.<Close>c__Iterator144 <Close>c__Iterator = new StrategyRepairConfirm.<Close>c__Iterator144();
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
