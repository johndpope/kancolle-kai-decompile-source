using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportAerialPhase2 : MonoBehaviour
	{
		private enum AttackState
		{
			FriendBomb,
			FriendRaigeki,
			FriendExplosion,
			End,
			None
		}

		private enum HitType
		{
			Bomb,
			Torpedo,
			Miss,
			None
		}

		private ProdSupportAerialPhase2.AttackState _attackState;

		[SerializeField]
		private GameObject _mainObj;

		[SerializeField]
		private GameObject[] _aircraftObj;

		private ProdAerialRescueCutIn _rescueCutIn;

		private BattleFieldCamera _camAerial;

		private bool _isEx;

		private bool _isMiss;

		private bool _isProtectE;

		private bool _isPlaying;

		private Vector3[] _eHpPos;

		private Animation _anime;

		private Action _actCallback;

		private ShienModel_Air _clsAerial;

		private BakuRaiDamageModel[] _fBakuraiModel;

		private BakuRaiDamageModel[] _eBakuraiModel;

		private PSTorpedoWakes _torpedoWakes;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private BattleHPGauges _battleHpGauges;

		private HpHitState[] _eHitState;

		private ProdSupportAerialPhase2.HitType[] _dicHitType;

		private float _explosionTime;

		[DebuggerHidden]
		public IEnumerator _init()
		{
			ProdSupportAerialPhase2.<_init>c__IteratorEC <_init>c__IteratorEC = new ProdSupportAerialPhase2.<_init>c__IteratorEC();
			<_init>c__IteratorEC.<>f__this = this;
			return <_init>c__IteratorEC;
		}

		private void _initHPGauge()
		{
			if (this._battleHpGauges != null)
			{
				this._battleHpGauges.Dispose();
			}
		}

		public void CreateHpGauge(FleetType type)
		{
			if (this._battleHpGauges == null)
			{
				this._battleHpGauges = new BattleHPGauges();
			}
			for (int i = 0; i < 6; i++)
			{
				this._battleHpGauges.AddInstantiates(base.get_gameObject(), true, true, false, false);
			}
		}

		private void OnDestroy()
		{
			this._initParticleList();
			Mem.Del<ProdSupportAerialPhase2.AttackState>(ref this._attackState);
			Mem.Del<GameObject>(ref this._mainObj);
			Mem.Del<GameObject[]>(ref this._aircraftObj);
			Mem.Del<BattleFieldCamera>(ref this._camAerial);
			Mem.Del<Vector3[]>(ref this._eHpPos);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ShienModel_Air>(ref this._clsAerial);
			Mem.Del<BakuRaiDamageModel[]>(ref this._fBakuraiModel);
			Mem.Del<BakuRaiDamageModel[]>(ref this._eBakuraiModel);
			Mem.Del<ProdSupportAerialPhase2.HitType[]>(ref this._dicHitType);
			Mem.DelListSafe<bool>(ref this._listBombCritical);
			Mem.DelListSafe<ParticleSystem>(ref this._listExplosion);
			Mem.DelListSafe<ParticleSystem>(ref this._listMiss);
			Mem.Del<HpHitState[]>(ref this._eHitState);
			Mem.Del<BakuRaiDamageModel[]>(ref this._eBakuraiModel);
			Mem.Del<ProdAerialRescueCutIn>(ref this._rescueCutIn);
			if (this._battleHpGauges != null)
			{
				this._battleHpGauges.Dispose();
			}
			Mem.Del<BattleHPGauges>(ref this._battleHpGauges);
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.SetDestroy();
			}
		}

		public static ProdSupportAerialPhase2 Instantiate(ProdSupportAerialPhase2 prefab, ShienModel_Air model, Transform parent)
		{
			ProdSupportAerialPhase2 prodSupportAerialPhase = Object.Instantiate<ProdSupportAerialPhase2>(prefab);
			prodSupportAerialPhase.get_transform().set_parent(parent);
			prodSupportAerialPhase.get_transform().set_localPosition(Vector3.get_zero());
			prodSupportAerialPhase.get_transform().set_localScale(Vector3.get_one());
			prodSupportAerialPhase._clsAerial = model;
			prodSupportAerialPhase.StartCoroutine(prodSupportAerialPhase._init());
			return prodSupportAerialPhase;
		}

		public bool Update()
		{
			if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendExplosion && this._isEx)
			{
				this._explosionTime += Time.get_deltaTime();
				if (this._explosionTime > 3f)
				{
					this._onFinishedCutIn();
					this._changeState();
					this.setState();
					this._explosionTime = 0f;
					this._isEx = false;
					this._isMiss = false;
				}
			}
			return true;
		}

		public void Play(Action callback)
		{
			base.GetComponent<UIPanel>().widgetsAreStatic = false;
			this._isPlaying = true;
			this._actCallback = callback;
			this._attackState = ProdSupportAerialPhase2.AttackState.None;
			this._fBakuraiModel = this._clsAerial.GetRaigekiData_e();
			this._eBakuraiModel = this._clsAerial.GetRaigekiData_f();
			this._camAerial.get_transform().set_localPosition(new Vector3(0f, this._camAerial.get_transform().get_localPosition().y, 90f));
			this._eHitState = new HpHitState[this._eBakuraiModel.Length];
			this._dicHitType = new ProdSupportAerialPhase2.HitType[this._eBakuraiModel.Length];
			for (int i = 0; i < this._eBakuraiModel.Length; i++)
			{
				this._eHitState[i] = HpHitState.None;
				this._dicHitType[i] = ProdSupportAerialPhase2.HitType.None;
			}
			this._changeState();
			this.setState();
		}

		private void _changeState()
		{
			switch (this._attackState)
			{
			case ProdSupportAerialPhase2.AttackState.FriendBomb:
				this._attackState = ((!this._clsAerial.IsRaigeki_e()) ? ProdSupportAerialPhase2.AttackState.FriendExplosion : ProdSupportAerialPhase2.AttackState.FriendRaigeki);
				break;
			case ProdSupportAerialPhase2.AttackState.FriendRaigeki:
				this._attackState = ProdSupportAerialPhase2.AttackState.FriendExplosion;
				break;
			case ProdSupportAerialPhase2.AttackState.FriendExplosion:
				this._attackState = ProdSupportAerialPhase2.AttackState.End;
				break;
			case ProdSupportAerialPhase2.AttackState.None:
				if (this._clsAerial.IsBakugeki_e())
				{
					this._attackState = ProdSupportAerialPhase2.AttackState.FriendBomb;
				}
				else if (this._clsAerial.IsRaigeki_e())
				{
					this._attackState = ProdSupportAerialPhase2.AttackState.FriendRaigeki;
				}
				else
				{
					this._attackState = ProdSupportAerialPhase2.AttackState.End;
				}
				break;
			}
		}

		private void setState()
		{
			if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendBomb)
			{
				this._startState("Bomb", FleetType.Friend);
			}
			else if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendRaigeki)
			{
				this._startState("Torpedo", FleetType.Friend);
			}
			else if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendExplosion)
			{
				this._startState("Explosion", FleetType.Friend);
			}
			else if (this._attackState == ProdSupportAerialPhase2.AttackState.End)
			{
				this._startState("End", FleetType.Friend);
			}
		}

		private void _startState(string type, FleetType fleetType)
		{
			if (type != null)
			{
				if (ProdSupportAerialPhase2.<>f__switch$map17 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Bomb", 0);
					dictionary.Add("Torpedo", 1);
					dictionary.Add("Explosion", 2);
					dictionary.Add("End", 3);
					ProdSupportAerialPhase2.<>f__switch$map17 = dictionary;
				}
				int num;
				if (ProdSupportAerialPhase2.<>f__switch$map17.TryGetValue(type, ref num))
				{
					switch (num)
					{
					case 0:
					{
						this._battleHpGauges.Init();
						this._viewAircrafts(fleetType, this._attackState, this._fBakuraiModel);
						this._camAerial.get_transform().set_localPosition(new Vector3(20f, 15f, 0f));
						this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(-16f, 90f, 0f)));
						Hashtable hashtable = new Hashtable();
						hashtable.Add("rotation", new Vector3(-13.5f, 95f, 0f));
						hashtable.Add("isLocal", true);
						hashtable.Add("time", 1.349f);
						hashtable.Add("easeType", iTween.EaseType.linear);
						this._camAerial.get_gameObject().RotateTo(hashtable);
						this._anime.Play("AerialStartPhase2_1");
						Animation component = base.get_transform().FindChild("CloudPanel").GetComponent<Animation>();
						component.Play();
						break;
					}
					case 1:
						this._initParticleList();
						this._battleHpGauges.Init();
						this._viewAircrafts(fleetType, this._attackState, this._fBakuraiModel);
						this._createTorpedoWake();
						this._camAerial.get_transform().set_localPosition(new Vector3(-21.3f, 6.2f, -7f));
						this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(16.29f, 90f, 0f)));
						BattleTaskManager.GetBattleField().seaLevel.set_waveSpeed(new Vector4(-4f, -2000f, 5f, -1600f));
						this._anime.Play("AerialStartPhase2_2");
						break;
					case 2:
						this._setHpGauge();
						this._moveExplosionCamera();
						break;
					case 3:
						this._battleHpGauges.Init();
						this._aerialCombatPhase1Finished();
						break;
					}
				}
			}
		}

		private void _initParticleList()
		{
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.Dispose();
			}
			if (this._listBombCritical != null)
			{
				this._listBombCritical.Clear();
			}
			this._listBombCritical = null;
			if (this._listExplosion != null)
			{
				using (List<ParticleSystem>.Enumerator enumerator = this._listExplosion.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ParticleSystem current = enumerator.get_Current();
						Object.Destroy(current.get_gameObject());
					}
				}
				this._listExplosion.Clear();
			}
			this._listExplosion = null;
			if (this._listMiss != null)
			{
				using (List<ParticleSystem>.Enumerator enumerator2 = this._listMiss.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ParticleSystem current2 = enumerator2.get_Current();
						Object.Destroy(current2.get_gameObject());
					}
				}
				this._listMiss.Clear();
			}
			this._listMiss = null;
		}

		private void _viewAircrafts(FleetType type, ProdSupportAerialPhase2.AttackState attack, BakuRaiDamageModel[] model)
		{
			if (attack == ProdSupportAerialPhase2.AttackState.FriendBomb)
			{
				this._viewAircraft(type, true, model);
			}
			else if (attack == ProdSupportAerialPhase2.AttackState.FriendRaigeki)
			{
				this._viewAircraft(type, false, model);
			}
		}

		private void _viewAircraft(FleetType type, bool isBomb, BakuRaiDamageModel[] model)
		{
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				this._aircraftObj[i].SetActive(false);
			}
			for (int j = 0; j < model.Length; j++)
			{
				if (num >= 3)
				{
					break;
				}
				if (model[j] != null)
				{
					bool flag = (!isBomb) ? model[j].IsRaigeki() : model[j].IsBakugeki();
					if (flag)
					{
						this._aircraftObj[num].SetActive(true);
						UITexture component = this._aircraftObj[num].get_transform().FindChild("Aircraft").GetComponent<UITexture>();
						this.setAircraftTexture(type, isBomb, component, num);
						num++;
					}
				}
			}
		}

		private void setAircraftTexture(FleetType type, bool isBomb, UITexture tex, int num)
		{
			bool is_friend = type == FleetType.Friend;
			SlotitemModel_Battle[] array = (!isBomb) ? this._clsAerial.GetRaigekiPlanes(is_friend) : this._clsAerial.GetBakugekiPlanes(is_friend);
			int num2 = (array.Length <= 3) ? array.Length : 3;
			if (num2 < num + 1)
			{
				return;
			}
			if (type == FleetType.Friend)
			{
				tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(array[num].MstId, 6);
			}
			else if (type == FleetType.Enemy)
			{
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(array[num].MstId, 6);
				}
				else
				{
					tex.mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadTexture(array[num]);
					tex.flip = UIBasicSprite.Flip.Horizontally;
					tex.MakePixelPerfect();
					tex.get_transform().set_localScale((array[num].MstId > 500) ? new Vector3(0.8f, 0.8f, 0.8f) : new Vector3(1f, 1f, 1f));
				}
			}
		}

		private void _moveExplosionCamera()
		{
			this._camAerial.get_transform().set_localPosition(new Vector3(0f, 3f, 35f));
			this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", new Vector3(0f, 3f, -40f));
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.9f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_playRescueCutIn");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._camAerial.get_gameObject().MoveTo(hashtable);
		}

		private void _playRescueCutIn()
		{
			if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendExplosion && this._isProtectE)
			{
				this._camAerial.get_transform().set_localPosition(new Vector3(this._rescueCutIn._listBattleShip.get_Item(0).get_transform().get_position().x, 3f, -40f));
				this._rescueCutIn.Play(new Action(this._onFinishedRescueCutIn));
			}
			else
			{
				this._onFinishedRescueCutIn();
			}
		}

		private void _onFinishedRescueCutIn()
		{
			this._camAerial.get_transform().set_localPosition(new Vector3(0f, 3f, -40f));
			this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			this._onFinishedMoveCamera();
		}

		private void _onFinishedCutIn()
		{
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.ReStartAll();
			}
		}

		private void _setProtect()
		{
			for (int i = 0; i < this._fBakuraiModel.Length; i++)
			{
				if (this._fBakuraiModel[i] != null)
				{
					ShipModel_Defender defender = this._fBakuraiModel[i].Defender;
					if (this._fBakuraiModel[i].GetProtectEffect())
					{
						this._isProtectE = true;
						this._rescueCutIn.AddShipList(this._eBattleship.get_Item(0), this._eBattleship.get_Item(defender.Index));
						break;
					}
				}
			}
		}

		private void _createTorpedoWake()
		{
			this._torpedoWakes = new PSTorpedoWakes();
			int num = 0;
			this._setProtect();
			for (int i = 0; i < this._fBakuraiModel.Length; i++)
			{
				if (this._fBakuraiModel[i] != null && this._fBakuraiModel[i].IsRaigeki())
				{
					ShipModel_Defender defender = this._fBakuraiModel[i].Defender;
					int num2 = (!this._fBakuraiModel[i].GetProtectEffect()) ? defender.Index : 0;
					bool flag = this._fBakuraiModel[i].GetHitState() == BattleHitStatus.Miss;
					float num3 = (!flag) ? 0f : 1f;
					Vector3 injectionVec = this._setTorpedoVec(num, true);
					num++;
					this._dicHitType[num2] = this.setHitType(FleetType.Enemy, defender.Index, flag, ProdSupportAerialPhase2.HitType.Torpedo);
					Vector3 targetVec = new Vector3(this._eBattleship.get_Item(num2).get_transform().get_position().x + num3, 0f, this._eBattleship.get_Item(num2).get_transform().get_position().z + 1f);
					if (this._fBakuraiModel[i].GetProtectEffect())
					{
						Vector3 position = this._eBattleship.get_Item(defender.Index).get_transform().get_position();
						this._torpedoWakes.SetProtectVector(num, new Vector3(position.x + 2f, 0f, position.z));
					}
					this._torpedoWakes.AddInstantiates(BattleTaskManager.GetBattleField().get_transform(), injectionVec, targetVec, true, i, 2f, false, flag);
				}
			}
		}

		private Vector3 _setTorpedoVec(int count, bool isFriend)
		{
			int num = (count < 3) ? count : (count - 3);
			return (!isFriend) ? BattleDefines.AERIAL_ENEMY_TORPEDO_POS[num] : BattleDefines.AERIAL_FRIEND_TORPEDO_POS[num];
		}

		private ProdSupportAerialPhase2.HitType setHitType(FleetType fleetType, int index, bool miss, ProdSupportAerialPhase2.HitType setType)
		{
			switch (this._dicHitType[index])
			{
			case ProdSupportAerialPhase2.HitType.Bomb:
				if (setType == ProdSupportAerialPhase2.HitType.Torpedo)
				{
					return (!miss) ? setType : ProdSupportAerialPhase2.HitType.Bomb;
				}
				return ProdSupportAerialPhase2.HitType.Bomb;
			case ProdSupportAerialPhase2.HitType.Torpedo:
				return ProdSupportAerialPhase2.HitType.Torpedo;
			case ProdSupportAerialPhase2.HitType.Miss:
				return (!miss) ? setType : ProdSupportAerialPhase2.HitType.Miss;
			case ProdSupportAerialPhase2.HitType.None:
				return (!miss) ? setType : ProdSupportAerialPhase2.HitType.Miss;
			default:
				return ProdSupportAerialPhase2.HitType.None;
			}
		}

		private void _setHpGauge()
		{
			this._eHpPos = new Vector3[this._eBattleship.get_Count()];
			this._eHpPos = this._setHpPosition(FleetType.Enemy, this._eBattleship.get_Count());
			List<ShipModel_Defender> defenders = this._clsAerial.GetDefenders(false, true);
			for (int i = 0; i < this._fBakuraiModel.Length; i++)
			{
				if (this._fBakuraiModel[i] != null)
				{
					ShipModel_Defender defender = this._fBakuraiModel[i].Defender;
					if (this._fBakuraiModel[i].GetHitState() == BattleHitStatus.Clitical)
					{
						this._eHitState[defender.Index] = HpHitState.Critical;
					}
					else if (this._fBakuraiModel[i].GetHitState() == BattleHitStatus.Miss)
					{
						if (this._eHitState[defender.Index] == HpHitState.None)
						{
							this._eHitState[defender.Index] = HpHitState.Miss;
						}
					}
					else if (this._fBakuraiModel[i].GetHitState() == BattleHitStatus.Normal && this._eHitState[defender.Index] != HpHitState.Critical)
					{
						this._eHitState[defender.Index] = HpHitState.Hit;
					}
				}
			}
			for (int j = 0; j < this._eBattleship.get_Count(); j++)
			{
				BattleHitStatus status;
				if (this._eHitState[j] == HpHitState.Miss)
				{
					status = BattleHitStatus.Miss;
				}
				else if (this._eHitState[j] == HpHitState.Critical)
				{
					status = BattleHitStatus.Clitical;
				}
				else
				{
					status = BattleHitStatus.Normal;
				}
				this._battleHpGauges.SetGauge(j, false, true, false, false);
				this._battleHpGauges.SetHp(j, defenders.get_Item(j).MaxHp, defenders.get_Item(j).HpBefore, defenders.get_Item(j).HpAfter, this._clsAerial.GetAttackDamage(defenders.get_Item(j).TmpId).GetDamage(), status, false);
			}
		}

		private Vector3[] _setHpPosition(FleetType type, int count)
		{
			Vector3[] array = new Vector3[count];
			if (count == 1)
			{
				array[0] = new Vector3(0f, 170f, 0f);
			}
			else if (count == 2)
			{
				for (int i = 0; i < 2; i++)
				{
					array[i] = new Vector3(-80f + 160f * (float)i, 170f, 0f);
				}
			}
			else if (count == 3)
			{
				for (int j = 0; j < 3; j++)
				{
					array[j] = new Vector3(-165f + 160f * (float)j, 170f, 0f);
				}
			}
			else if (count == 4)
			{
				for (int k = 0; k < 4; k++)
				{
					array[k] = new Vector3(-250f + 160f * (float)k, 170f, 0f);
				}
			}
			else if (count == 5)
			{
				for (int l = 0; l < 5; l++)
				{
					array[l] = new Vector3(-320f + 160f * (float)l, 170f, 0f);
				}
			}
			else if (count == 6)
			{
				for (int m = 0; m < 6; m++)
				{
					array[m] = new Vector3(-400f + 160f * (float)m, 170f, 0f);
				}
			}
			return array;
		}

		private void _setShinking(FleetType type)
		{
			for (int i = 0; i < this._fBakuraiModel.Length; i++)
			{
				if (this._fBakuraiModel[i] != null)
				{
					ShipModel_Defender defender = this._fBakuraiModel[i].Defender;
					if (defender.DmgStateAfter == DamageState_Battle.Gekichin)
					{
						if (type == FleetType.Friend)
						{
							this._eBattleship.get_Item(i).PlayProdSinking(null);
						}
						else if (type == FleetType.Enemy)
						{
							this._fBattleship.get_Item(i).PlayProdSinking(null);
						}
					}
				}
			}
		}

		private void _createExplotion()
		{
			this._listBombCritical = new List<bool>();
			this._listExplosion = new List<ParticleSystem>();
			if (this._attackState == ProdSupportAerialPhase2.AttackState.FriendExplosion)
			{
				for (int i = 0; i < this._eBattleship.get_Count(); i++)
				{
					this._battleHpGauges.Show(i, this._eHpPos[i], new Vector3(0.35f, 0.35f, 0.35f), false);
					this._battleHpGauges.SetDamagePosition(i, new Vector3(this._battleHpGauges.GetDamagePosition(i).x, -525f, 0f));
					this._battleHpGauges.PlayHp(i, null);
				}
				this._setShinking(FleetType.Friend);
				for (int j = 0; j < this._fBakuraiModel.Length; j++)
				{
					if (this._fBakuraiModel[j] != null && this._fBakuraiModel[j].IsBakugeki())
					{
						ShipModel_Battle defender = this._fBakuraiModel[j].Defender;
						int num = (!this._fBakuraiModel[j].GetProtectEffect()) ? defender.Index : defender.Index;
						bool flag = this._fBakuraiModel[j].GetHitState() == BattleHitStatus.Miss;
						FleetType fleetType = FleetType.Enemy;
						this._dicHitType[num] = this.setHitType(fleetType, num, flag, ProdSupportAerialPhase2.HitType.Bomb);
						ProdSupportAerialPhase2.HitType hitType = this._dicHitType[num];
						if (!flag)
						{
							if (hitType == ProdSupportAerialPhase2.HitType.Bomb)
							{
								Vector3 position = new Vector3(this._eBattleship.get_Item(num).get_transform().get_position().x, 3f, this._eBattleship.get_Item(num).get_transform().get_position().z);
								ParticleSystem particleSystem = (!(BattleTaskManager.GetParticleFile().explosionAerial == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
								particleSystem.SetActive(true);
								particleSystem.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
								particleSystem.get_transform().set_position(position);
								this._listExplosion.Add(particleSystem);
								this._listBombCritical.Add(this._fBakuraiModel[j].GetHitState() == BattleHitStatus.Clitical);
							}
						}
						else if (hitType == ProdSupportAerialPhase2.HitType.Miss)
						{
							float num2 = XorRandom.GetFLim(this._eBattleship.get_Item(num).get_transform().get_position().x - 0.5f, this._eBattleship.get_Item(num).get_transform().get_position().x + 0.5f);
							float num3 = XorRandom.GetFLim(this._eBattleship.get_Item(num).get_transform().get_position().z - 1f, this._eBattleship.get_Item(num).get_transform().get_position().z + 1f);
							num2 = ((num2 < this._eBattleship.get_Item(num).get_transform().get_position().x) ? (num2 - 0.5f) : (num2 + 0.5f));
							num3 = ((num3 < this._eBattleship.get_Item(num).get_transform().get_position().z) ? (num3 - 0.5f) : (num3 + 0.5f));
							ParticleSystem particleSystem2 = (!(BattleTaskManager.GetParticleFile().splashMiss == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
							particleSystem2.SetActive(true);
							particleSystem2.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
							particleSystem2.get_transform().set_position(new Vector3(num2, 0f, num3));
							this._listMiss.Add(particleSystem2);
						}
					}
				}
			}
			int arg_408_0 = (this._listExplosion == null) ? 0 : this._listExplosion.get_Count();
			int arg_426_0 = (this._listMiss == null) ? 0 : this._listMiss.get_Count();
			this._isEx = true;
			if (this._listExplosion != null)
			{
				KCV.Utils.SoundUtils.PlaySE((this._listExplosion.get_Count() <= 1) ? SEFIleInfos.SE_930 : SEFIleInfos.SE_931);
			}
			Observable.FromCoroutine(new Func<IEnumerator>(this._explosionPlay), false).Subscribe(delegate(Unit _)
			{
				this._isEx = true;
			});
			Observable.FromCoroutine(new Func<IEnumerator>(this._missSplashPlay), false).Subscribe(delegate(Unit _)
			{
				this._isMiss = true;
			});
			this._playSE(FleetType.Friend);
		}

		[DebuggerHidden]
		private IEnumerator _explosionPlay()
		{
			ProdSupportAerialPhase2.<_explosionPlay>c__IteratorED <_explosionPlay>c__IteratorED = new ProdSupportAerialPhase2.<_explosionPlay>c__IteratorED();
			<_explosionPlay>c__IteratorED.<>f__this = this;
			return <_explosionPlay>c__IteratorED;
		}

		[DebuggerHidden]
		private IEnumerator _missSplashPlay()
		{
			ProdSupportAerialPhase2.<_missSplashPlay>c__IteratorEE <_missSplashPlay>c__IteratorEE = new ProdSupportAerialPhase2.<_missSplashPlay>c__IteratorEE();
			<_missSplashPlay>c__IteratorEE.<>f__this = this;
			return <_missSplashPlay>c__IteratorEE;
		}

		private void _playSE(FleetType fleetType)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this._dicHitType.Length; i++)
			{
				if (this._dicHitType[i] == ProdSupportAerialPhase2.HitType.Torpedo)
				{
					flag = true;
				}
				if (this._dicHitType[i] == ProdSupportAerialPhase2.HitType.Miss)
				{
					flag2 = true;
				}
			}
			if (flag)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_932);
			}
			if (flag2)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_908);
			}
		}

		private void _playAnimationSE(int num)
		{
			if (num != 0)
			{
				if (num == 1)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_915);
				}
			}
			else
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_914);
			}
		}

		private void _onFinishedMoveCamera()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.PlaySplashAll();
			}
			this._camAerial.cameraShake.ShakeRot(null);
			this._createExplotion();
			this._isEx = true;
		}

		private void _injectionTrpedo()
		{
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.InjectionAll();
			}
		}

		private void _aerialAttackPhase1Finished()
		{
			this._changeState();
			this.setState();
		}

		private void _aerialAttackPhase2Finished()
		{
			this._changeState();
			this.setState();
		}

		private void _aerialCombatPhase2Attack()
		{
			this._injectionTrpedo();
		}

		private void _aerialCombatPhase1Finished()
		{
			this._initParticleList();
			if (this._rescueCutIn != null)
			{
				Object.Destroy(this._rescueCutIn.get_gameObject());
			}
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
