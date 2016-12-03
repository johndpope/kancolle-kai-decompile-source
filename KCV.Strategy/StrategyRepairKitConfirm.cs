using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyRepairKitConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UILabel ShipLV;

		[SerializeField]
		private UILabel NeedDay;

		[SerializeField]
		private UILabel KitNumBefore;

		[SerializeField]
		private UILabel KitNumAfter;

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
			this.NeedDay = null;
			this.KitNumBefore = null;
			this.KitNumAfter = null;
			this.YesNoBtn = null;
		}

		public void SetModel(RepairDockModel dock, int RepairKitNum)
		{
			ShipModel ship = dock.GetShip();
			this.ShipName.text = ship.Name;
			this.ShipLV.textInt = ship.Level;
			this.NeedDay.textInt = dock.RemainingTurns;
			this.KitNumBefore.textInt = RepairKitNum;
			this.KitNumAfter.textInt = RepairKitNum - 1;
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
			StrategyRepairKitConfirm.<Close>c__Iterator145 <Close>c__Iterator = new StrategyRepairKitConfirm.<Close>c__Iterator145();
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
