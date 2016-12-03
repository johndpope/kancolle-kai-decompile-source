using Common.Enum;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;

namespace KCV.Battle.Production
{
	public class ProdShellingAttack : IDisposable
	{
		private class Statement : IDisposable
		{
			private DelProdShellingAttack _delProdShellingAttack;

			private DelProdShellingUpdate _delProdShellingUpdate;

			private DelProdShellingClear _delProdShellingClear;

			public DelProdShellingAttack Init
			{
				get
				{
					return this._delProdShellingAttack;
				}
				private set
				{
					this._delProdShellingAttack = value;
				}
			}

			public DelProdShellingUpdate Update
			{
				get
				{
					return this._delProdShellingUpdate;
				}
				private set
				{
					this._delProdShellingUpdate = value;
				}
			}

			public DelProdShellingClear Clear
			{
				get
				{
					return this._delProdShellingClear;
				}
				private set
				{
					this._delProdShellingClear = value;
				}
			}

			public void Dispose()
			{
				this.Release();
			}

			public void AddState(DelProdShellingAttack initdelegate, DelProdShellingUpdate updatedelegate, DelProdShellingClear cleardelegate)
			{
				this._delProdShellingAttack = initdelegate;
				this._delProdShellingUpdate = updatedelegate;
				this._delProdShellingClear = cleardelegate;
			}

			public void Release()
			{
				this._delProdShellingAttack = null;
				this._delProdShellingUpdate = null;
				this._delProdShellingClear = null;
			}
		}

		private ProdShellingAttack.Statement _clsStatement;

		private bool _isFinished;

		private bool _isPlaying;

		private HougekiModel _clsHougekiModel;

		private Action _actCallback;

		private Dictionary<FleetType, bool> _dicAttackFleet;

		private ProdNormalAttack _prodNormalAttack;

		private ProdAntiGroundAttack _prodAntiGroundAttack;

		private ProdTorpedoAttack _prodTorpedoAttack;

		private ProdAircraftAttack _prodAircraftAttack;

		private ProdDepthChargeAttack _prodDepthChargeAttack;

		private ProdLaserAttack _prodLaserAttack;

		private ProdSuccessiveAttack _prodSuccessiveAttack;

		private ProdObservedShellingAttack _prodObservedShellingAttack;

		private ProdTranscendenceAttack _prodTranscendenceAttack;

		public HougekiModel hougekiModel
		{
			get
			{
				return this._clsHougekiModel;
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isPlaying;
			}
		}

		public bool isFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		public Dictionary<FleetType, bool> attackFleet
		{
			get
			{
				return this._dicAttackFleet;
			}
		}

		public ProdShellingAttack()
		{
			this._clsStatement = new ProdShellingAttack.Statement();
			this._dicAttackFleet = new Dictionary<FleetType, bool>();
			this._dicAttackFleet.Add(FleetType.Friend, false);
			this._dicAttackFleet.Add(FleetType.Enemy, false);
		}

		public void Dispose()
		{
			Mem.DelIDisposableSafe<ProdShellingAttack.Statement>(ref this._clsStatement);
			Mem.Del<HougekiModel>(ref this._clsHougekiModel);
			Mem.Del<Action>(ref this._actCallback);
			Mem.DelDictionarySafe<FleetType, bool>(ref this._dicAttackFleet);
			Mem.DelIDisposableSafe<ProdNormalAttack>(ref this._prodNormalAttack);
			Mem.DelIDisposableSafe<ProdAntiGroundAttack>(ref this._prodAntiGroundAttack);
			Mem.DelIDisposableSafe<ProdTorpedoAttack>(ref this._prodTorpedoAttack);
			Mem.DelIDisposableSafe<ProdAircraftAttack>(ref this._prodAircraftAttack);
			Mem.DelIDisposableSafe<ProdDepthChargeAttack>(ref this._prodDepthChargeAttack);
			Mem.DelIDisposableSafe<ProdLaserAttack>(ref this._prodLaserAttack);
			Mem.DelIDisposableSafe<ProdSuccessiveAttack>(ref this._prodSuccessiveAttack);
			Mem.DelIDisposableSafe<ProdObservedShellingAttack>(ref this._prodObservedShellingAttack);
			Mem.DelIDisposableSafe<ProdTranscendenceAttack>(ref this._prodTranscendenceAttack);
		}

		public void Clear()
		{
			Mem.Del<HougekiModel>(ref this._clsHougekiModel);
			this._isFinished = false;
			this._isPlaying = false;
			Mem.Del<Action>(ref this._actCallback);
			if (this._clsStatement.Clear != null)
			{
				this._clsStatement.Clear();
			}
			this._clsStatement.Release();
		}

		public void Play(HougekiModel hougeki, int nCurrentShellingCnt, bool isNextAttack, Action onFinished)
		{
			if (hougeki == null)
			{
				this.OnShellingFinished();
			}
			this._clsHougekiModel = hougeki;
			this._isFinished = false;
			this._isPlaying = true;
			this._actCallback = onFinished;
			switch (hougeki.AttackType)
			{
			case BattleAttackKind.Normal:
				if (this._clsHougekiModel.GetRocketEffenct())
				{
					if (this._prodAntiGroundAttack == null)
					{
						this._prodAntiGroundAttack = new ProdAntiGroundAttack();
					}
					this._clsStatement.AddState(new DelProdShellingAttack(this._prodAntiGroundAttack.PlayAttack), new DelProdShellingUpdate(this._prodAntiGroundAttack.Update), new DelProdShellingClear(this._prodAntiGroundAttack.Clear));
				}
				else
				{
					if (this._prodNormalAttack == null)
					{
						this._prodNormalAttack = new ProdNormalAttack();
					}
					this._clsStatement.AddState(new DelProdShellingAttack(this._prodNormalAttack.PlayAttack), new DelProdShellingUpdate(this._prodNormalAttack.Update), new DelProdShellingClear(this._prodNormalAttack.Clear));
				}
				break;
			case BattleAttackKind.Bakurai:
				if (this._prodDepthChargeAttack == null)
				{
					this._prodDepthChargeAttack = new ProdDepthChargeAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodDepthChargeAttack.PlayAttack), new DelProdShellingUpdate(this._prodDepthChargeAttack.Update), new DelProdShellingClear(this._prodDepthChargeAttack.Clear));
				break;
			case BattleAttackKind.Gyorai:
				if (this._prodTorpedoAttack == null)
				{
					this._prodTorpedoAttack = new ProdTorpedoAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodTorpedoAttack.PlayAttack), new DelProdShellingUpdate(this._prodTorpedoAttack.Update), new DelProdShellingClear(this._prodTorpedoAttack.Clear));
				break;
			case BattleAttackKind.AirAttack:
				if (this._prodAircraftAttack == null)
				{
					this._prodAircraftAttack = new ProdAircraftAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodAircraftAttack.PlayAttack), new DelProdShellingUpdate(this._prodAircraftAttack.Update), new DelProdShellingClear(this._prodAircraftAttack.Clear));
				break;
			case BattleAttackKind.Laser:
				if (this._prodLaserAttack == null)
				{
					this._prodLaserAttack = new ProdLaserAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodLaserAttack.PlayAttack), new DelProdShellingUpdate(this._prodLaserAttack.Update), new DelProdShellingClear(this._prodLaserAttack.Clear));
				break;
			case BattleAttackKind.Renzoku:
				if (this._prodSuccessiveAttack == null)
				{
					this._prodSuccessiveAttack = new ProdSuccessiveAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodSuccessiveAttack.PlayAttack), new DelProdShellingUpdate(this._prodSuccessiveAttack.Update), new DelProdShellingClear(this._prodSuccessiveAttack.Clear));
				break;
			case BattleAttackKind.Sp1:
			case BattleAttackKind.Sp2:
			case BattleAttackKind.Sp3:
			case BattleAttackKind.Sp4:
				if (this._prodObservedShellingAttack == null)
				{
					this._prodObservedShellingAttack = new ProdObservedShellingAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodObservedShellingAttack.PlayAttack), new DelProdShellingUpdate(this._prodObservedShellingAttack.Update), new DelProdShellingClear(this._prodObservedShellingAttack.Clear));
				break;
			case BattleAttackKind.Syu_Rai:
			case BattleAttackKind.Rai_Rai:
			case BattleAttackKind.Syu_Syu_Fuku:
			case BattleAttackKind.Syu_Syu_Syu:
				if (this._prodTranscendenceAttack == null)
				{
					this._prodTranscendenceAttack = new ProdTranscendenceAttack();
				}
				this._clsStatement.AddState(new DelProdShellingAttack(this._prodTranscendenceAttack.PlayAttack), new DelProdShellingUpdate(this._prodTranscendenceAttack.Update), new DelProdShellingClear(this._prodTranscendenceAttack.Clear));
				break;
			}
			this._clsStatement.Init(this._clsHougekiModel, nCurrentShellingCnt, isNextAttack, this._dicAttackFleet.get_Item((!this.hougekiModel.Attacker.IsFriend()) ? FleetType.Enemy : FleetType.Friend), new Action(this.OnShellingFinished));
		}

		public void Update()
		{
			if (this._clsHougekiModel == null)
			{
				return;
			}
			if (this._clsStatement.Update != null)
			{
				this._clsStatement.Update();
			}
		}

		private void OnShellingFinished()
		{
			FleetType fleetType = (!this._clsHougekiModel.Attacker.IsFriend()) ? FleetType.Enemy : FleetType.Friend;
			if (!this._dicAttackFleet.get_Item(fleetType))
			{
				this._dicAttackFleet.set_Item(fleetType, true);
			}
			this._isFinished = true;
			Dlg.Call(ref this._actCallback);
		}
	}
}
