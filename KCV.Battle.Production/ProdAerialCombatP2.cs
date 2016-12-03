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
	public class ProdAerialCombatP2 : MonoBehaviour
	{
		private enum AttackState
		{
			FriendBomb,
			FriendRaigeki,
			FriendExplosion,
			EnemyBomb,
			EnemyRaigeki,
			EnemyExplosion,
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

		private ProdAerialCombatP2.AttackState _attackState;

		[SerializeField]
		private GameObject _mainObj;

		[SerializeField]
		private GameObject[] _aircraftObj;

		private ProdAerialRescueCutIn _rescueCutIn;

		private BattleFieldCamera _camAerial;

		private bool _isEx;

		private bool _isPlaying;

		private bool[] _isProtect;

		private Animation _anime;

		private Action _actCallback;

		private KoukuuModel _clsKoukuu;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private Dictionary<FleetType, ProdAerialCombatP2.HitType[]> _dicHitType;

		private Dictionary<FleetType, HpHitState[]> _dicHitState;

		private Dictionary<FleetType, BakuRaiDamageModel[]> _dicBakuraiModel;

		private BattleHPGauges _battleHpGauges;

		private float _explosionTime;

		private PSTorpedoWakes _torpedoWakes;

		[DebuggerHidden]
		public IEnumerator _init()
		{
			ProdAerialCombatP2.<_init>c__IteratorD3 <_init>c__IteratorD = new ProdAerialCombatP2.<_init>c__IteratorD3();
			<_init>c__IteratorD.<>f__this = this;
			return <_init>c__IteratorD;
		}

		private void OnDestroy()
		{
			Mem.Del<ProdAerialCombatP2.AttackState>(ref this._attackState);
			Mem.Del<GameObject>(ref this._mainObj);
			Mem.DelArySafe<GameObject>(ref this._aircraftObj);
			Mem.Del<BattleFieldCamera>(ref this._camAerial);
			Mem.DelArySafe<bool>(ref this._isProtect);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<KoukuuModel>(ref this._clsKoukuu);
			Mem.DelDictionarySafe<FleetType, BakuRaiDamageModel[]>(ref this._dicBakuraiModel);
			Mem.DelListSafe<bool>(ref this._listBombCritical);
			Mem.DelListSafe<ParticleSystem>(ref this._listExplosion);
			Mem.DelListSafe<ParticleSystem>(ref this._listMiss);
			Mem.DelDictionarySafe<FleetType, ProdAerialCombatP2.HitType[]>(ref this._dicHitType);
			Mem.DelDictionarySafe<FleetType, HpHitState[]>(ref this._dicHitState);
			Mem.Del<BattleHPGauges>(ref this._battleHpGauges);
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.SetDestroy();
			}
			if (this._rescueCutIn != null)
			{
				Object.Destroy(this._rescueCutIn.get_gameObject());
			}
			Mem.Del<ProdAerialRescueCutIn>(ref this._rescueCutIn);
		}

		public static ProdAerialCombatP2 Instantiate(ProdAerialCombatP2 prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialCombatP2 prodAerialCombatP = Object.Instantiate<ProdAerialCombatP2>(prefab);
			prodAerialCombatP.get_transform().set_parent(parent);
			prodAerialCombatP.get_transform().set_localPosition(Vector3.get_zero());
			prodAerialCombatP.get_transform().set_localScale(Vector3.get_one());
			prodAerialCombatP._clsKoukuu = model;
			prodAerialCombatP.StartCoroutine(prodAerialCombatP._init());
			return prodAerialCombatP;
		}

		private void _destroyHPGauge()
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

		public bool Update()
		{
			if ((this._attackState == ProdAerialCombatP2.AttackState.FriendExplosion && this._isEx) || (this._attackState == ProdAerialCombatP2.AttackState.EnemyExplosion && this._isEx))
			{
				this._explosionTime += Time.get_deltaTime();
				if (this._explosionTime > 3f)
				{
					this._onFinishedCutIn();
					this._changeState();
					this._setState();
					this._explosionTime = 0f;
					this._isEx = false;
				}
			}
			return true;
		}

		private static CutInType _chkCutInType(KoukuuModel model)
		{
			if (model.GetCaptainShip(true) != null && model.GetCaptainShip(false) != null)
			{
				return CutInType.Both;
			}
			if (model.GetCaptainShip(true) != null)
			{
				return CutInType.FriendOnly;
			}
			return CutInType.EnemyOnly;
		}

		public void Play(Action callback, Dictionary<int, UIBattleShip> fBattleShips, Dictionary<int, UIBattleShip> eBattleShips)
		{
			base.GetComponent<UIPanel>().widgetsAreStatic = false;
			this._isPlaying = true;
			this._actCallback = callback;
			this._attackState = ProdAerialCombatP2.AttackState.None;
			this._fBattleship = fBattleShips;
			this._eBattleship = eBattleShips;
			this._dicBakuraiModel = new Dictionary<FleetType, BakuRaiDamageModel[]>();
			this._dicBakuraiModel.Add(FleetType.Friend, this._clsKoukuu.GetRaigekiData_e());
			this._dicBakuraiModel.Add(FleetType.Enemy, this._clsKoukuu.GetRaigekiData_f());
			this._camAerial.get_transform().set_localPosition(new Vector3(0f, this._camAerial.get_transform().get_localPosition().y, 90f));
			HpHitState[] array = new HpHitState[this._dicBakuraiModel.get_Item(FleetType.Friend).Length];
			HpHitState[] array2 = new HpHitState[this._dicBakuraiModel.get_Item(FleetType.Enemy).Length];
			ProdAerialCombatP2.HitType[] array3 = new ProdAerialCombatP2.HitType[this._dicBakuraiModel.get_Item(FleetType.Friend).Length];
			ProdAerialCombatP2.HitType[] array4 = new ProdAerialCombatP2.HitType[this._dicBakuraiModel.get_Item(FleetType.Enemy).Length];
			for (int i = 0; i < this._dicBakuraiModel.get_Item(FleetType.Friend).Length; i++)
			{
				array[i] = HpHitState.None;
				array3[i] = ProdAerialCombatP2.HitType.None;
			}
			for (int j = 0; j < this._dicBakuraiModel.get_Item(FleetType.Enemy).Length; j++)
			{
				array2[j] = HpHitState.None;
				array4[j] = ProdAerialCombatP2.HitType.None;
			}
			this._dicHitType = new Dictionary<FleetType, ProdAerialCombatP2.HitType[]>();
			this._dicHitType.Add(FleetType.Friend, array3);
			this._dicHitType.Add(FleetType.Enemy, array4);
			this._dicHitState = new Dictionary<FleetType, HpHitState[]>();
			this._dicHitState.Add(FleetType.Friend, array);
			this._dicHitState.Add(FleetType.Enemy, array2);
			this._changeState();
			this._setState();
		}

		private void _changeState()
		{
			switch (this._attackState)
			{
			case ProdAerialCombatP2.AttackState.FriendBomb:
				this._attackState = ((!this._clsKoukuu.IsRaigeki_e()) ? ProdAerialCombatP2.AttackState.FriendExplosion : ProdAerialCombatP2.AttackState.FriendRaigeki);
				break;
			case ProdAerialCombatP2.AttackState.FriendRaigeki:
				this._attackState = ProdAerialCombatP2.AttackState.FriendExplosion;
				break;
			case ProdAerialCombatP2.AttackState.FriendExplosion:
				if (this._clsKoukuu.IsBakugeki_f())
				{
					this._attackState = ProdAerialCombatP2.AttackState.EnemyBomb;
				}
				else if (this._clsKoukuu.IsRaigeki_f())
				{
					this._attackState = ProdAerialCombatP2.AttackState.EnemyRaigeki;
				}
				else
				{
					this._attackState = ProdAerialCombatP2.AttackState.End;
				}
				break;
			case ProdAerialCombatP2.AttackState.EnemyBomb:
				this._attackState = ((!this._clsKoukuu.IsRaigeki_f()) ? ProdAerialCombatP2.AttackState.EnemyExplosion : ProdAerialCombatP2.AttackState.EnemyRaigeki);
				break;
			case ProdAerialCombatP2.AttackState.EnemyRaigeki:
				this._attackState = ProdAerialCombatP2.AttackState.EnemyExplosion;
				break;
			case ProdAerialCombatP2.AttackState.EnemyExplosion:
				this._attackState = ProdAerialCombatP2.AttackState.End;
				break;
			case ProdAerialCombatP2.AttackState.None:
				if (this._clsKoukuu.IsBakugeki_e())
				{
					this._attackState = ProdAerialCombatP2.AttackState.FriendBomb;
				}
				else if (this._clsKoukuu.IsRaigeki_e())
				{
					this._attackState = ProdAerialCombatP2.AttackState.FriendRaigeki;
				}
				else if (this._clsKoukuu.IsBakugeki_f())
				{
					this._attackState = ProdAerialCombatP2.AttackState.EnemyBomb;
				}
				else if (this._clsKoukuu.IsRaigeki_f())
				{
					this._attackState = ProdAerialCombatP2.AttackState.EnemyRaigeki;
				}
				else
				{
					this._attackState = ProdAerialCombatP2.AttackState.End;
				}
				break;
			}
		}

		private void _setState()
		{
			if (this._attackState == ProdAerialCombatP2.AttackState.FriendBomb)
			{
				this._startState("Bomb", FleetType.Friend);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.FriendRaigeki)
			{
				this._startState("Torpedo", FleetType.Friend);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.FriendExplosion)
			{
				this._startState("Explosion", FleetType.Friend);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.EnemyBomb)
			{
				this._startState("Bomb", FleetType.Enemy);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.EnemyRaigeki)
			{
				this._startState("Torpedo", FleetType.Enemy);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.EnemyExplosion)
			{
				this._startState("Explosion", FleetType.Enemy);
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.End)
			{
				this._startState("End", FleetType.Friend);
			}
		}

		private void _startState(string type, FleetType fleetType)
		{
			if (type != null)
			{
				if (ProdAerialCombatP2.<>f__switch$map16 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Bomb", 0);
					dictionary.Add("Torpedo", 1);
					dictionary.Add("Explosion", 2);
					dictionary.Add("End", 3);
					ProdAerialCombatP2.<>f__switch$map16 = dictionary;
				}
				int num;
				if (ProdAerialCombatP2.<>f__switch$map16.TryGetValue(type, ref num))
				{
					switch (num)
					{
					case 0:
					{
						this._battleHpGauges.Init();
						this._viewAircrafts(fleetType, this._attackState, this._dicBakuraiModel.get_Item(fleetType));
						this._camAerial.get_transform().set_localPosition(BattleDefines.AERIAL_BOMB_CAM_POSITION[(int)fleetType]);
						this._camAerial.get_transform().set_rotation(BattleDefines.AERIAL_BOMB_CAM_ROTATION[(int)fleetType]);
						base.get_transform().set_localEulerAngles(BattleDefines.AERIAL_BOMB_TRANS_ANGLE[(int)fleetType]);
						Hashtable hashtable = new Hashtable();
						hashtable.Add("rotation", (fleetType != FleetType.Friend) ? new Vector3(-13.5f, -95f, 0f) : new Vector3(-13.5f, 95f, 0f));
						hashtable.Add("isLocal", true);
						hashtable.Add("time", 1.35f);
						hashtable.Add("easeType", iTween.EaseType.linear);
						this._camAerial.get_gameObject().RotateTo(hashtable);
						this._anime.Stop();
						this._anime.Play("AerialStartPhase2_1");
						base.get_transform().FindChild("CloudPanel").GetComponent<Animation>().Play();
						break;
					}
					case 1:
						this._battleHpGauges.Init();
						this._initParticleList();
						BattleTaskManager.GetBattleField().seaLevel.set_waveSpeed(BattleDefines.AERIAL_TORPEDO_WAVESPEED[(int)fleetType]);
						this._viewAircrafts(fleetType, this._attackState, this._dicBakuraiModel.get_Item(fleetType));
						this._createTorpedoWake(fleetType);
						this._camAerial.get_transform().set_localPosition(BattleDefines.AERIAL_TORPEDO_CAM_POSITION[(int)fleetType]);
						this._camAerial.get_transform().set_rotation(BattleDefines.AERIAL_TORPEDO_CAM_ROTATION[(int)fleetType]);
						base.get_transform().set_localEulerAngles(BattleDefines.AERIAL_BOMB_TRANS_ANGLE[(int)fleetType]);
						this._anime.Stop();
						this._anime.Play("AerialStartPhase2_2");
						break;
					case 2:
						this._setHpGauge(fleetType);
						this._moveExplosionCamera();
						break;
					case 3:
						this._aerialCombatPhase1Finished();
						break;
					}
				}
			}
		}

		private void _viewAircrafts(FleetType type, ProdAerialCombatP2.AttackState attack, BakuRaiDamageModel[] model)
		{
			if (attack == ProdAerialCombatP2.AttackState.FriendBomb || attack == ProdAerialCombatP2.AttackState.EnemyBomb)
			{
				this._viewAircraft(type, true, model);
			}
			else if (attack == ProdAerialCombatP2.AttackState.FriendRaigeki || attack == ProdAerialCombatP2.AttackState.EnemyRaigeki)
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
			SlotitemModel_Battle[] array = (!isBomb) ? this._clsKoukuu.GetRaigekiPlanes(is_friend) : this._clsKoukuu.GetBakugekiPlanes(is_friend);
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
			bool flag = this._attackState == ProdAerialCombatP2.AttackState.FriendExplosion;
			this._camAerial.get_transform().set_localPosition((!flag) ? new Vector3(0f, 7.5f, -35f) : new Vector3(0f, 7.5f, 35f));
			this._camAerial.get_transform().set_rotation((!flag) ? Quaternion.Euler(new Vector3(0f, 0f, 0f)) : Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			base.get_transform().set_rotation((!flag) ? Quaternion.Euler(new Vector3(0f, 0f, 0f)) : Quaternion.Euler(new Vector3(0f, 0f, 0f)));
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", (!flag) ? new Vector3(0f, 7.5f, 40f) : new Vector3(0f, 7.5f, -40f));
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.9f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_playRescueCutIn");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._camAerial.get_gameObject().MoveTo(hashtable);
		}

		private void _playRescueCutIn()
		{
			if (this._attackState == ProdAerialCombatP2.AttackState.FriendExplosion && this._isProtect[1])
			{
				this._camAerial.get_transform().set_localPosition(new Vector3(this._rescueCutIn._listBattleShip.get_Item(0).get_transform().get_position().x, 7.5f, -40f));
				this._rescueCutIn.Play(new Action(this._onFinishedRescueCutIn));
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.EnemyExplosion && this._isProtect[0])
			{
				this._camAerial.get_transform().set_localPosition(new Vector3(this._rescueCutIn._listBattleShip.get_Item(0).get_transform().get_position().x, 7.5f, 40f));
				this._rescueCutIn.Play(new Action(this._onFinishedRescueCutIn));
			}
			else
			{
				this._onFinishedRescueCutIn();
			}
		}

		private void _onFinishedRescueCutIn()
		{
			if (this._attackState == ProdAerialCombatP2.AttackState.FriendExplosion)
			{
				this._camAerial.get_transform().set_localPosition(new Vector3(0f, 7.5f, -40f));
				this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			}
			else if (this._attackState == ProdAerialCombatP2.AttackState.EnemyExplosion)
			{
				this._camAerial.get_transform().set_localPosition(new Vector3(0f, 7.5f, 40f));
				this._camAerial.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 0f, 0f)));
			}
			this._playHitAnimation();
			this._isEx = true;
		}

		private void _onFinishedCutIn()
		{
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.ReStartAll();
			}
		}

		private void _playHitAnimation()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			if (this._torpedoWakes != null)
			{
				this._torpedoWakes.PlaySplashAll();
			}
			this._camAerial.cameraShake.ShakeRot(null);
			this._setExplotion();
		}

		private void _setProtect(FleetType type)
		{
			int num = (type != FleetType.Friend) ? 0 : 1;
			for (int i = 0; i < this._dicBakuraiModel.get_Item(type).Length; i++)
			{
				if (this._dicBakuraiModel.get_Item(type)[i] != null && this._dicBakuraiModel.get_Item(type)[i].GetProtectEffect())
				{
					this._isProtect[num] = true;
					ShipModel_Defender defender = this._dicBakuraiModel.get_Item(type)[i].Defender;
					if (type == FleetType.Friend)
					{
						this._rescueCutIn.AddShipList(this._eBattleship.get_Item(0), this._eBattleship.get_Item(defender.Index));
					}
					else
					{
						this._rescueCutIn.AddShipList(this._fBattleship.get_Item(0), this._fBattleship.get_Item(defender.Index));
					}
					break;
				}
			}
		}

		private void _createTorpedoWake(FleetType type)
		{
			this._torpedoWakes = new PSTorpedoWakes();
			this._setProtect(type);
			this._createTorupedoWakes(type == FleetType.Friend);
		}

		private void _createTorupedoWakes(bool isFriend)
		{
			int num = 0;
			FleetType fleetType = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			FleetType fleetType2 = (!isFriend) ? FleetType.Friend : FleetType.Enemy;
			this._torpedoWakes.InitProtectVector();
			for (int i = 0; i < this._dicBakuraiModel.get_Item(fleetType).Length; i++)
			{
				if (this._dicBakuraiModel.get_Item(fleetType)[i] != null && this._dicBakuraiModel.get_Item(fleetType)[i].IsRaigeki())
				{
					ShipModel_Defender defender = this._dicBakuraiModel.get_Item(fleetType)[i].Defender;
					int num2 = (!this._dicBakuraiModel.get_Item(fleetType)[i].GetProtectEffect()) ? defender.Index : 0;
					bool flag = this._dicBakuraiModel.get_Item(fleetType)[i].GetHitState() == BattleHitStatus.Miss;
					float num3 = (!flag) ? 0f : 1f;
					Vector3 injectionVec = this._setTorpedoVec(num, isFriend);
					num++;
					this._dicHitType.get_Item(fleetType2)[num2] = this.setHitType(fleetType2, defender.Index, flag, ProdAerialCombatP2.HitType.Torpedo);
					Vector3 targetVec = (!isFriend) ? new Vector3(this._fBattleship.get_Item(num2).get_transform().get_position().x + num3, 0f, this._fBattleship.get_Item(num2).get_transform().get_position().z - 1f) : new Vector3(this._eBattleship.get_Item(num2).get_transform().get_position().x + num3, 0f, this._eBattleship.get_Item(num2).get_transform().get_position().z + 1f);
					if (this._dicBakuraiModel.get_Item(fleetType)[i].GetProtectEffect())
					{
						Vector3 vector = (!isFriend) ? this._fBattleship.get_Item(defender.Index).get_transform().get_position() : this._eBattleship.get_Item(defender.Index).get_transform().get_position();
						this._torpedoWakes.SetProtectVector(num, (!isFriend) ? new Vector3(vector.x - 2f, 0f, vector.z) : new Vector3(vector.x + 2f, 0f, vector.z));
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

		private ProdAerialCombatP2.HitType setHitType(FleetType fleetType, int index, bool miss, ProdAerialCombatP2.HitType setType)
		{
			switch (this._dicHitType.get_Item(fleetType)[index])
			{
			case ProdAerialCombatP2.HitType.Bomb:
				if (setType == ProdAerialCombatP2.HitType.Torpedo)
				{
					return (!miss) ? setType : ProdAerialCombatP2.HitType.Bomb;
				}
				return ProdAerialCombatP2.HitType.Bomb;
			case ProdAerialCombatP2.HitType.Torpedo:
				return ProdAerialCombatP2.HitType.Torpedo;
			case ProdAerialCombatP2.HitType.Miss:
				return (!miss) ? setType : ProdAerialCombatP2.HitType.Miss;
			case ProdAerialCombatP2.HitType.None:
				return (!miss) ? setType : ProdAerialCombatP2.HitType.Miss;
			default:
				return ProdAerialCombatP2.HitType.None;
			}
		}

		private void _setHpGauge(FleetType type)
		{
			int num = (type != FleetType.Friend) ? this._fBattleship.get_Count() : this._eBattleship.get_Count();
			bool flag = type == FleetType.Friend;
			FleetType fleetType = (!flag) ? FleetType.Friend : FleetType.Enemy;
			List<ShipModel_Defender> defenders = this._clsKoukuu.GetDefenders(!flag, true);
			for (int i = 0; i < this._dicBakuraiModel.get_Item(type).Length; i++)
			{
				if (this._dicBakuraiModel.get_Item(type)[i] != null)
				{
					ShipModel_Defender defender = this._dicBakuraiModel.get_Item(type)[i].Defender;
					switch (this._dicBakuraiModel.get_Item(type)[i].GetHitState())
					{
					case BattleHitStatus.Miss:
						if (this._dicHitState.get_Item(fleetType)[defender.Index] == HpHitState.None)
						{
							this._dicHitState.get_Item(fleetType)[defender.Index] = HpHitState.Miss;
						}
						break;
					case BattleHitStatus.Normal:
						if (this._dicHitState.get_Item(fleetType)[defender.Index] != HpHitState.Critical)
						{
							this._dicHitState.get_Item(fleetType)[defender.Index] = HpHitState.Hit;
						}
						break;
					case BattleHitStatus.Clitical:
						this._dicHitState.get_Item(fleetType)[defender.Index] = HpHitState.Critical;
						break;
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				BakuRaiDamageModel attackDamage = this._clsKoukuu.GetAttackDamage(defenders.get_Item(j).TmpId);
				int damage = (attackDamage != null) ? attackDamage.GetDamage() : -1;
				BattleHitStatus status;
				if (this._dicHitState.get_Item(fleetType)[j] == HpHitState.Miss)
				{
					status = BattleHitStatus.Miss;
				}
				else if (this._dicHitState.get_Item(fleetType)[j] == HpHitState.Critical)
				{
					status = BattleHitStatus.Clitical;
				}
				else
				{
					status = BattleHitStatus.Normal;
				}
				this._battleHpGauges.Init();
				this._battleHpGauges.SetGauge(j, flag, true, false, false);
				if (flag)
				{
					this._battleHpGauges.SetHp(j, defenders.get_Item(j).MaxHp, defenders.get_Item(j).HpBefore, defenders.get_Item(j).HpAfter, damage, status, false);
				}
				else
				{
					this._battleHpGauges.SetHp(j, defenders.get_Item(j).MaxHp, defenders.get_Item(j).HpBefore, defenders.get_Item(j).HpAfter, damage, status, false);
				}
			}
		}

		private Vector3[] _setHpPosition(FleetType type, int count)
		{
			Vector3[] array = new Vector3[count];
			int num = (type != FleetType.Friend) ? 1 : -1;
			int num2 = (type != FleetType.Friend) ? -1 : 1;
			if (count == 1)
			{
				array[0] = new Vector3(0f, 100f, 0f);
			}
			else if (count == 2)
			{
				for (int i = 0; i < 2; i++)
				{
					array[i] = new Vector3(80f * (float)num + 160f * (float)i * (float)num2, 100f, 0f);
				}
			}
			else if (count == 3)
			{
				for (int j = 0; j < 3; j++)
				{
					array[j] = new Vector3(165f * (float)num + 160f * (float)j * (float)num2, 100f, 0f);
				}
			}
			else if (count == 4)
			{
				for (int k = 0; k < 4; k++)
				{
					array[k] = new Vector3(245f * (float)num + 160f * (float)k * (float)num2, 100f, 0f);
				}
			}
			else if (count == 5)
			{
				for (int l = 0; l < 5; l++)
				{
					array[l] = new Vector3(320f * (float)num + 160f * (float)l * (float)num2, 100f, 0f);
				}
			}
			else if (count == 6)
			{
				for (int m = 0; m < 6; m++)
				{
					array[m] = new Vector3(400f * (float)num + 160f * (float)m * (float)num2, 100f, 0f);
				}
			}
			return array;
		}

		private void _setShinking(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				BattleTaskManager.GetBattleShips().UpdateDamageAll(this._clsKoukuu, false);
			}
			for (int i = 0; i < this._dicBakuraiModel.get_Item(type).Length; i++)
			{
				if (this._dicBakuraiModel.get_Item(type)[i] != null)
				{
					ShipModel_Defender defender = this._dicBakuraiModel.get_Item(type)[i].Defender;
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

		private void _setExplotion()
		{
			FleetType fleetType = (this._attackState != ProdAerialCombatP2.AttackState.FriendExplosion) ? FleetType.Enemy : FleetType.Friend;
			int num = (fleetType != FleetType.Friend) ? this._fBattleship.get_Count() : this._eBattleship.get_Count();
			Vector3[] array = this._setHpPosition(fleetType, num);
			for (int i = 0; i < num; i++)
			{
				this._battleHpGauges.Show(i, array[i], new Vector3(0.35f, 0.35f, 0.35f), false);
				this._battleHpGauges.SetDamagePosition(i, new Vector3(this._battleHpGauges.GetDamagePosition(i).x, -525f, 0f));
				this._battleHpGauges.PlayHp(i, null);
			}
			this._setShinking(fleetType);
			this._createExplotion(fleetType == FleetType.Friend);
			if (this._listExplosion != null)
			{
				KCV.Utils.SoundUtils.PlaySE((this._listExplosion.get_Count() <= 1) ? SEFIleInfos.SE_930 : SEFIleInfos.SE_931);
				Observable.FromCoroutine(new Func<IEnumerator>(this._explosionPlay), false).Subscribe(delegate(Unit _)
				{
				});
			}
			using (List<ParticleSystem>.Enumerator enumerator = this._listMiss.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ParticleSystem current = enumerator.get_Current();
					current.Play();
				}
			}
			this._playSE(fleetType);
		}

		[DebuggerHidden]
		private IEnumerator _explosionPlay()
		{
			ProdAerialCombatP2.<_explosionPlay>c__IteratorD4 <_explosionPlay>c__IteratorD = new ProdAerialCombatP2.<_explosionPlay>c__IteratorD4();
			<_explosionPlay>c__IteratorD.<>f__this = this;
			return <_explosionPlay>c__IteratorD;
		}

		private void _createExplotion(bool isFriend)
		{
			this._listBombCritical = new List<bool>();
			this._listExplosion = new List<ParticleSystem>();
			this._listMiss = new List<ParticleSystem>();
			FleetType fleetType = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			Dictionary<int, UIBattleShip> arg_48_0 = (!isFriend) ? this._eBattleship : this._fBattleship;
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? this._fBattleship : this._eBattleship;
			for (int i = 0; i < this._dicBakuraiModel.get_Item(fleetType).Length; i++)
			{
				if (this._dicBakuraiModel.get_Item(fleetType)[i] != null && this._dicBakuraiModel.get_Item(fleetType)[i].IsBakugeki())
				{
					ShipModel_Battle defender = this._dicBakuraiModel.get_Item(fleetType)[i].Defender;
					int num = (!this._dicBakuraiModel.get_Item(fleetType)[i].GetProtectEffect()) ? defender.Index : defender.Index;
					bool flag = this._dicBakuraiModel.get_Item(fleetType)[i].GetHitState() == BattleHitStatus.Miss;
					FleetType fleetType2 = (!isFriend) ? FleetType.Friend : FleetType.Enemy;
					this._dicHitType.get_Item(fleetType2)[num] = this.setHitType(fleetType2, num, flag, ProdAerialCombatP2.HitType.Bomb);
					ProdAerialCombatP2.HitType hitType = (!isFriend) ? this._dicHitType.get_Item(FleetType.Friend)[num] : this._dicHitType.get_Item(FleetType.Enemy)[num];
					if (!flag)
					{
						if (hitType == ProdAerialCombatP2.HitType.Bomb)
						{
							Vector3 position = new Vector3(dictionary.get_Item(num).get_transform().get_position().x, 3f, dictionary.get_Item(num).get_transform().get_position().z);
							ParticleSystem particleSystem = (!(BattleTaskManager.GetParticleFile().explosionAerial == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
							particleSystem.SetActive(true);
							particleSystem.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
							particleSystem.get_transform().set_position(position);
							this._listExplosion.Add(particleSystem);
							this._listBombCritical.Add(this._dicBakuraiModel.get_Item(fleetType)[i].GetHitState() == BattleHitStatus.Clitical);
						}
					}
					else if (hitType == ProdAerialCombatP2.HitType.Miss)
					{
						float num2 = XorRandom.GetFLim(dictionary.get_Item(num).get_transform().get_position().x - 0.5f, dictionary.get_Item(num).get_transform().get_position().x + 0.5f);
						float num3 = XorRandom.GetFLim(dictionary.get_Item(num).get_transform().get_position().z - 1f, dictionary.get_Item(num).get_transform().get_position().z + 1f);
						num2 = ((num2 < dictionary.get_Item(num).get_transform().get_position().x) ? (num2 - 0.5f) : (num2 + 0.5f));
						num3 = ((num3 < dictionary.get_Item(num).get_transform().get_position().z) ? (num3 - 0.5f) : (num3 + 0.5f));
						ParticleSystem particleSystem2 = (!(BattleTaskManager.GetParticleFile().splashMiss == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
						particleSystem2.SetActive(true);
						particleSystem2.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
						particleSystem2.get_transform().set_position(new Vector3(num2, 0f, num3));
						this._listMiss.Add(particleSystem2);
					}
				}
			}
		}

		private void _playSE(FleetType fleetType)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this._dicHitType.get_Item(fleetType).Length; i++)
			{
				if (this._dicHitType.get_Item(fleetType)[i] == ProdAerialCombatP2.HitType.Torpedo)
				{
					flag = true;
				}
				if (this._dicHitType.get_Item(fleetType)[i] == ProdAerialCombatP2.HitType.Miss)
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
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_918);
				}
			}
			else
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_920);
			}
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
			this._setState();
		}

		private void _aerialAttackPhase2Finished()
		{
			this._aerialAttackPhase1Finished();
		}

		private void _aerialCombatPhase2Attack()
		{
			this._injectionTrpedo();
		}

		private void _aerialCombatPhase1Finished()
		{
			this._destroyHPGauge();
			this._initParticleList();
			if (this._rescueCutIn != null)
			{
				Object.Destroy(this._rescueCutIn.get_gameObject());
			}
			this._rescueCutIn = null;
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
