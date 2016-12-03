using Common.Enum;
using KCV.Battle.Utils;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleShips : IDisposable
	{
		private bool _isInitialize;

		private bool _isMakeRadar;

		private bool _isRadarDeployed;

		private Transform _traFriendFleetAnchor;

		private Transform _traEnemyFleetAnchor;

		private UIBattleShip _uiOriginalShip;

		private Dictionary<int, UIBattleShip> _dicFriendBattleShips;

		private Dictionary<int, UIBattleShip> _dicEnemyBattleShips;

		private List<UIBufferFleetCircle> _listBufferFleetCircle;

		private Dictionary<int, List<UIBufferCircle>> _dicBufferShipCircle;

		public bool isInitialize
		{
			get
			{
				return this._isInitialize;
			}
		}

		public bool isRadarDeployed
		{
			get
			{
				return this._isRadarDeployed;
			}
		}

		public Dictionary<int, UIBattleShip> dicFriendBattleShips
		{
			get
			{
				return this._dicFriendBattleShips;
			}
			set
			{
				this._dicFriendBattleShips = value;
			}
		}

		public Dictionary<int, UIBattleShip> dicEnemyBattleShips
		{
			get
			{
				return this._dicEnemyBattleShips;
			}
			set
			{
				this._dicEnemyBattleShips = value;
			}
		}

		public UIBattleShip flagShipFriend
		{
			get
			{
				return this._dicFriendBattleShips.get_Item(0);
			}
		}

		public UIBattleShip flagShipEnemy
		{
			get
			{
				return this._dicEnemyBattleShips.get_Item(0);
			}
		}

		public UIBattleShip lastIndexShipFriend
		{
			get
			{
				return Enumerable.Last<KeyValuePair<int, UIBattleShip>>(Enumerable.OrderBy<KeyValuePair<int, UIBattleShip>, int>(this._dicFriendBattleShips, (KeyValuePair<int, UIBattleShip> order) => order.get_Value().shipModel.Index)).get_Value();
			}
		}

		public UIBattleShip lastIndexShipEnemy
		{
			get
			{
				return Enumerable.Last<KeyValuePair<int, UIBattleShip>>(Enumerable.OrderBy<KeyValuePair<int, UIBattleShip>, int>(this.dicEnemyBattleShips, (KeyValuePair<int, UIBattleShip> order) => order.get_Value().shipModel.Index)).get_Value();
			}
		}

		public List<UIBufferFleetCircle> bufferFleetCircle
		{
			get
			{
				return this._listBufferFleetCircle;
			}
		}

		public Dictionary<int, List<UIBufferCircle>> bufferShipCirlce
		{
			get
			{
				return this._dicBufferShipCircle;
			}
		}

		public BattleShips()
		{
			this._uiOriginalShip = BattleTaskManager.GetPrefabFile().prefabBattleShip.GetComponent<UIBattleShip>();
			this._isInitialize = false;
			this._isMakeRadar = false;
		}

		public void Dispose()
		{
			Mem.Del<bool>(ref this._isInitialize);
			Mem.Del<bool>(ref this._isMakeRadar);
			Mem.Del<bool>(ref this._isRadarDeployed);
			Mem.Del<Transform>(ref this._traFriendFleetAnchor);
			Mem.Del<Transform>(ref this._traEnemyFleetAnchor);
			Mem.Del<UIBattleShip>(ref this._uiOriginalShip);
			if (this._listBufferFleetCircle != null)
			{
				this._listBufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
				{
					if (x != null)
					{
						Object.Destroy(x.get_gameObject());
					}
				});
			}
			if (this._dicBufferShipCircle != null)
			{
				this._dicBufferShipCircle.get_Item(0).ForEach(delegate(UIBufferCircle x)
				{
					if (x != null)
					{
						Object.Destroy(x.get_gameObject());
					}
				});
				this._dicBufferShipCircle.get_Item(1).ForEach(delegate(UIBufferCircle x)
				{
					if (x != null)
					{
						Object.Destroy(x.get_gameObject());
					}
				});
			}
			Mem.DelListSafe<UIBufferFleetCircle>(ref this._listBufferFleetCircle);
			Mem.DelDictionarySafe<int, List<UIBufferCircle>>(ref this._dicBufferShipCircle);
			if (this._dicFriendBattleShips != null)
			{
				this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.get_Value() != null)
					{
						x.get_Value().get_gameObject().Discard();
					}
				});
			}
			Mem.DelDictionary<int, UIBattleShip>(ref this._dicFriendBattleShips);
			if (this._dicEnemyBattleShips != null)
			{
				this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.get_Value() != null)
					{
						x.get_Value().get_gameObject().Discard();
					}
				});
			}
			Mem.DelDictionary<int, UIBattleShip>(ref this._dicEnemyBattleShips);
		}

		public bool Init(BattleManager manager)
		{
			if (manager == null)
			{
				return false;
			}
			this._traFriendFleetAnchor = GameObject.Find("FriendFleetAnchor").get_transform();
			this._traEnemyFleetAnchor = GameObject.Find("EnemyFleetAnchor").get_transform();
			this._dicFriendBattleShips = new Dictionary<int, UIBattleShip>();
			this._dicEnemyBattleShips = new Dictionary<int, UIBattleShip>();
			IObservable<Unit> source = Observable.FromCoroutine(() => this.CreateBattleShips(manager.Ships_f, manager.FormationId_f, this._traFriendFleetAnchor.get_gameObject(), this._dicFriendBattleShips, FleetType.Friend, manager.ShipCount_f, 0), false);
			IObservable<Unit> other = Observable.FromCoroutine(() => this.CreateBattleShips(manager.Ships_e, manager.FormationId_e, this._traEnemyFleetAnchor.get_gameObject(), this._dicEnemyBattleShips, FleetType.Enemy, manager.ShipCount_e, 0), false);
			source.SelectMany(other).Subscribe(delegate(Unit _)
			{
				this._uiOriginalShip = null;
				this._isInitialize = true;
			});
			return true;
		}

		public bool Update()
		{
			if (this.isRadarDeployed)
			{
				this.bufferShipCirlce.get_Item(0).ForEach(delegate(UIBufferCircle x)
				{
					x.Run();
				});
			}
			return true;
		}

		[DebuggerHidden]
		private IEnumerator CreateBattleShips(ShipModel_BattleAll[] ships, BattleFormationKinds1 iKind, GameObject parent, Dictionary<int, UIBattleShip> dic, FleetType iType, int fleetNum, int combineNum)
		{
			BattleShips.<CreateBattleShips>c__IteratorCD <CreateBattleShips>c__IteratorCD = new BattleShips.<CreateBattleShips>c__IteratorCD();
			<CreateBattleShips>c__IteratorCD.iKind = iKind;
			<CreateBattleShips>c__IteratorCD.fleetNum = fleetNum;
			<CreateBattleShips>c__IteratorCD.ships = ships;
			<CreateBattleShips>c__IteratorCD.iType = iType;
			<CreateBattleShips>c__IteratorCD.parent = parent;
			<CreateBattleShips>c__IteratorCD.dic = dic;
			<CreateBattleShips>c__IteratorCD.<$>iKind = iKind;
			<CreateBattleShips>c__IteratorCD.<$>fleetNum = fleetNum;
			<CreateBattleShips>c__IteratorCD.<$>ships = ships;
			<CreateBattleShips>c__IteratorCD.<$>iType = iType;
			<CreateBattleShips>c__IteratorCD.<$>parent = parent;
			<CreateBattleShips>c__IteratorCD.<$>dic = dic;
			<CreateBattleShips>c__IteratorCD.<>f__this = this;
			return <CreateBattleShips>c__IteratorCD;
		}

		public void UpdateDamageAll(IBattlePhase iPhase, bool isFriend)
		{
			List<ShipModel_Defender> defenders = iPhase.GetDefenders(isFriend);
			defenders.ForEach(delegate(ShipModel_Defender x)
			{
				if (isFriend)
				{
					this.dicFriendBattleShips.get_Item(x.Index).UpdateDamage(x);
				}
				else
				{
					this.dicEnemyBattleShips.get_Item(x.Index).UpdateDamage(x);
				}
			});
		}

		public void UpdateDamageAll(IBattlePhase iPhase)
		{
			this.UpdateDamageAll(iPhase, true);
			this.UpdateDamageAll(iPhase, false);
		}

		public void Restored(ShipModel_Defender defender)
		{
			UIBattleShip uIBattleShip = (!defender.IsFriend()) ? this._dicEnemyBattleShips.get_Item(defender.Index) : this._dicFriendBattleShips.get_Item(defender.Index);
			uIBattleShip.SetActive(true);
			uIBattleShip.Restored(defender);
		}

		public void SetStandingPosition(StandingPositionType iType)
		{
			this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().SetStandingPosition(iType);
				}
			});
			this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().SetStandingPosition(iType);
				}
			});
		}

		public void SetShipDrawType(ShipDrawType iType)
		{
			this.SetShipDrawType(FleetType.Friend, iType);
			this.SetShipDrawType(FleetType.Enemy, iType);
		}

		public void SetShipDrawType(FleetType iFleet, ShipDrawType iDrawType)
		{
			if (iFleet != FleetType.Friend)
			{
				if (iFleet == FleetType.Enemy)
				{
					this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
					{
						if (x.get_Value() != null)
						{
							x.get_Value().drawType = iDrawType;
						}
					});
				}
			}
			else
			{
				this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.get_Value() != null)
					{
						x.get_Value().drawType = iDrawType;
					}
				});
			}
		}

		public void SetLayer(Generics.Layers iLayer)
		{
			this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null && x.get_Value().layer != iLayer)
				{
					x.get_Value().layer = iLayer;
				}
			});
			this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null && x.get_Value().layer != iLayer)
				{
					x.get_Value().layer = iLayer;
				}
			});
		}

		public void SetBollboardTarget(Transform target)
		{
			this.SetBollboardTarget(true, target);
			this.SetBollboardTarget(false, target);
		}

		public void SetBollboardTarget(bool isFriend, Transform target)
		{
			if (isFriend)
			{
				this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.get_Value() != null)
					{
						x.get_Value().billboard.billboardTarget = target;
					}
				});
			}
			else
			{
				this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.get_Value() != null)
					{
						x.get_Value().billboard.billboardTarget = target;
					}
				});
			}
		}

		public void RadarDeployment(bool isDeploy)
		{
			if (!this._isMakeRadar)
			{
				this.MakeRadar();
			}
			this._listBufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			this._dicBufferShipCircle.get_Item(0).ForEach(delegate(UIBufferCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			this._dicBufferShipCircle.get_Item(1).ForEach(delegate(UIBufferCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			if (isDeploy)
			{
				this._dicBufferShipCircle.get_Item(0).ForEach(delegate(UIBufferCircle x)
				{
					x.PlayGearAnimation();
				});
				this._dicBufferShipCircle.get_Item(1).ForEach(delegate(UIBufferCircle x)
				{
					x.PlayGearAnimation();
				});
			}
			else
			{
				this._dicBufferShipCircle.get_Item(0).ForEach(delegate(UIBufferCircle x)
				{
					x.StopGearAnimation();
				});
				this._dicBufferShipCircle.get_Item(1).ForEach(delegate(UIBufferCircle x)
				{
					x.StopGearAnimation();
				});
			}
			this._isRadarDeployed = isDeploy;
		}

		private void MakeRadar()
		{
			BattleShips.<MakeRadar>c__AnonStorey361 <MakeRadar>c__AnonStorey = new BattleShips.<MakeRadar>c__AnonStorey361();
			<MakeRadar>c__AnonStorey.field = BattleTaskManager.GetBattleField();
			BattlePefabFile prefabFile = BattleTaskManager.GetPrefabFile();
			Transform prefabUIBufferFleetCircle = prefabFile.prefabUIBufferFleetCircle;
			this._listBufferFleetCircle = new List<UIBufferFleetCircle>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						this._listBufferFleetCircle.Add(UIBufferFleetCircle.Instantiate(prefabUIBufferFleetCircle.GetComponent<UIBufferFleetCircle>(), <MakeRadar>c__AnonStorey.field.dicFleetAnchor.get_Item(fleetType), fleetType));
						this._listBufferFleetCircle.get_Item((int)fleetType).get_transform().positionY(0.001f);
					}
				}
			}
			Mem.Del<Transform>(ref prefabUIBufferFleetCircle);
			Transform prefab = prefabFile.prefabUIBufferShipCircle;
			this._dicBufferShipCircle = new Dictionary<int, List<UIBufferCircle>>();
			int cnt = 0;
			List<UIBufferCircle> friendBufferCircle = new List<UIBufferCircle>();
			this.dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				friendBufferCircle.Add(UIBufferCircle.Instantiate(prefab.GetComponent<UIBufferCircle>(), x.get_Value().get_transform(), FleetType.Friend, <MakeRadar>c__AnonStorey.field.dicFleetAnchor.get_Item(FleetType.Enemy)));
				cnt++;
			});
			this._dicBufferShipCircle.Add(0, friendBufferCircle);
			cnt = 0;
			List<UIBufferCircle> enemyBufferCircle = new List<UIBufferCircle>();
			this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				enemyBufferCircle.Add(UIBufferCircle.Instantiate(prefab.GetComponent<UIBufferCircle>(), x.get_Value().get_transform(), FleetType.Enemy, <MakeRadar>c__AnonStorey.field.dicFleetAnchor.get_Item(FleetType.Friend)));
				cnt++;
			});
			this._dicBufferShipCircle.Add(1, enemyBufferCircle);
			Mem.Del<Transform>(ref prefab);
			this._isMakeRadar = true;
		}

		public void SetSprayColor()
		{
			this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().SetSprayColor();
				}
			});
			this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().SetSprayColor();
				}
			});
		}

		public void SetTorpedoSalvoWakeAngle(bool isSet)
		{
			this._dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().TorpedoSalvoWakeAngle(isSet);
				}
			});
			this._dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.get_Value() != null)
				{
					x.get_Value().TorpedoSalvoWakeAngle(isSet);
				}
			});
		}
	}
}
