using Common.Enum;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdDamage : IDisposable
	{
		private bool _isPlaying;

		private bool _isFinished;

		private Action _actCallback;

		private IBattlePhase _iBattlePhase;

		private Queue<ShipModel_Defender> _queFriedShipModel;

		private List<ShipModel_Defender> _listFriendShipModel;

		private List<ShipModel_Defender> _listEnemyShipModel;

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

		public int remainingCnt
		{
			get
			{
				return this._queFriedShipModel.get_Count();
			}
		}

		public ProdDamage()
		{
			this._isPlaying = false;
			this._isFinished = false;
			this._actCallback = null;
			this._iBattlePhase = null;
			this._queFriedShipModel = new Queue<ShipModel_Defender>();
			this._listFriendShipModel = new List<ShipModel_Defender>();
			this._listEnemyShipModel = new List<ShipModel_Defender>();
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			this._isPlaying = false;
			this._isFinished = false;
			Mem.Del<IBattlePhase>(ref this._iBattlePhase);
			Mem.DelQueueSafe<ShipModel_Defender>(ref this._queFriedShipModel);
			Mem.DelListSafe<ShipModel_Defender>(ref this._listFriendShipModel);
			Mem.DelListSafe<ShipModel_Defender>(ref this._listEnemyShipModel);
			Mem.Del<Action>(ref this._actCallback);
			return true;
		}

		public void Dispose()
		{
			this.UnInit();
		}

		public void Play(IBattlePhase iBattlePhase, Action callback)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			this._iBattlePhase = iBattlePhase;
			this._actCallback = callback;
			this._isPlaying = true;
			BattleTaskManager.GetBattleShips().UpdateDamageAll(iBattlePhase, false);
			if (iBattlePhase.HasGekichinEvent())
			{
				iBattlePhase.GetGekichinShips().ForEach(delegate(ShipModel_Defender x)
				{
					this._queFriedShipModel.Enqueue(x);
				});
				if (iBattlePhase.HasTaihaEvent())
				{
					this._listFriendShipModel.AddRange(iBattlePhase.GetDefenders(true, DamagedStates.Taiha));
				}
				if (iBattlePhase.HasChuhaEvent())
				{
					this._listFriendShipModel.AddRange(iBattlePhase.GetDefenders(true, DamagedStates.Tyuuha));
				}
				if (this._listFriendShipModel.get_Count() != 0)
				{
					Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate(long _)
					{
						BattleShips ships = BattleTaskManager.GetBattleShips();
						this._listFriendShipModel.ForEach(delegate(ShipModel_Defender x)
						{
							ships.dicFriendBattleShips.get_Item(x.Index).UpdateDamage(x);
						});
					});
				}
				this.PlaySinking();
			}
			else if (iBattlePhase.HasTaihaEvent())
			{
				list.AddRange(iBattlePhase.GetDefenders(true, DamagedStates.Taiha));
				if (iBattlePhase.HasChuhaEvent())
				{
					list.AddRange(iBattlePhase.GetDefenders(true, DamagedStates.Tyuuha));
				}
				this._queFriedShipModel = new Queue<ShipModel_Defender>();
				int num = 0;
				using (List<ShipModel_Defender>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ShipModel_Defender current = enumerator.get_Current();
						if (num >= 3)
						{
							break;
						}
						this._queFriedShipModel.Enqueue(current);
						num++;
					}
				}
				this.PlayHeavyDamage(DamagedStates.Taiha);
			}
			else if (iBattlePhase.HasChuhaEvent())
			{
				list.AddRange(iBattlePhase.GetDefenders(true, DamagedStates.Tyuuha).ConvertAll<ShipModel_Defender>((ShipModel_Defender s) => s));
				int num2 = 0;
				using (List<ShipModel_Defender>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ShipModel_Defender current2 = enumerator2.get_Current();
						if (num2 >= 3)
						{
							break;
						}
						this._queFriedShipModel.Enqueue(current2);
						num2++;
					}
				}
				this.PlayHeavyDamage(DamagedStates.Tyuuha);
			}
			else
			{
				this.OnFinished();
			}
			list.Clear();
		}

		private void PlaySinking()
		{
			if (this._queFriedShipModel.get_Count() != 0)
			{
				ProdSinking prodSinking = BattleTaskManager.GetPrefabFile().prodSinking;
				prodSinking.SetSinkingData(this._queFriedShipModel.Dequeue());
				prodSinking.Play(delegate
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Executions();
					Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
					{
						this.PlaySinking();
					});
				});
			}
			else
			{
				this.OnFinished();
			}
		}

		private void PlayHeavyDamage(DamagedStates status)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			list.AddRange(this._queFriedShipModel.ToArray());
			this._queFriedShipModel.Clear();
			ProdDamageCutIn.DamageCutInType damageCutInType = (status != DamagedStates.Taiha) ? ProdDamageCutIn.DamageCutInType.Moderate : ProdDamageCutIn.DamageCutInType.Heavy;
			ProdDamageCutIn prodDamageCutIn = BattleTaskManager.GetPrefabFile().prodDamageCutIn;
			prodDamageCutIn.SetShipData(list, damageCutInType);
			prodDamageCutIn.Play(damageCutInType, delegate
			{
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.UpdateDamageAll(this._iBattlePhase);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				this.OnFinished();
			});
		}

		private void OnFinished()
		{
			this._isFinished = true;
			this._isPlaying = false;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
			{
				this.Dispose();
			});
		}
	}
}
