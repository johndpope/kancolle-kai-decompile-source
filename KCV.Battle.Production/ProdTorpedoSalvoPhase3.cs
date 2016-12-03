using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoSalvoPhase3
	{
		public enum StateType
		{
			None,
			Attack,
			End
		}

		private float _fTime;

		private bool[] _isTorpedoHit;

		private bool[] _isProtect;

		private bool _isPlaying;

		private Action _actCallback;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private Dictionary<FleetType, bool[]> _dicIsCriticall;

		private Dictionary<FleetType, bool[]> _dicIsMiss;

		private RaigekiModel _clsRaigeki;

		private BattleFieldCamera[] _fieldCam;

		private List<PSTorpedoWake> _listPSTorpedoWake;

		private ProdTorpedoProtect _torpedoProtect;

		private PSTorpedoWake _torpedoParticle;

		private ProdTorpedoSalvoPhase3.StateType stateType;

		public Transform transform;

		public ProdTorpedoSalvoPhase3(Transform obj)
		{
			this.transform = obj;
		}

		public bool Initialize(RaigekiModel model, PSTorpedoWake psTorpedo)
		{
			this._fTime = 0f;
			this._isTorpedoHit = new bool[2];
			this._isProtect = new bool[2];
			this.stateType = ProdTorpedoSalvoPhase3.StateType.None;
			this._clsRaigeki = model;
			this._torpedoParticle = psTorpedo;
			this._fBattleship = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			this._eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			this._listPSTorpedoWake = new List<PSTorpedoWake>();
			this._fieldCam = new BattleFieldCamera[2];
			this._fieldCam[0] = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._fieldCam[1] = BattleTaskManager.GetBattleCameras().enemyFieldCamera;
			this._torpedoProtect = this.transform.SafeGetComponent<ProdTorpedoProtect>();
			this._torpedoProtect._init();
			return true;
		}

		public void OnSetDestroy()
		{
			if (this._listPSTorpedoWake != null)
			{
				using (List<PSTorpedoWake>.Enumerator enumerator = this._listPSTorpedoWake.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PSTorpedoWake current = enumerator.get_Current();
						Object.Destroy(current);
					}
				}
				this._listPSTorpedoWake.Clear();
			}
			this._listPSTorpedoWake = null;
			if (this._torpedoProtect != null)
			{
				Object.Destroy(this._torpedoProtect.get_gameObject());
			}
			this._torpedoProtect = null;
			Mem.Del<bool[]>(ref this._isTorpedoHit);
			Mem.Del<bool[]>(ref this._isProtect);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ProdTorpedoSalvoPhase3.StateType>(ref this.stateType);
			Mem.Del<Transform>(ref this.transform);
			Mem.DelListSafe<PSTorpedoWake>(ref this._listPSTorpedoWake);
			Mem.DelDictionarySafe<FleetType, bool[]>(ref this._dicIsCriticall);
			Mem.DelDictionarySafe<FleetType, bool[]>(ref this._dicIsMiss);
			if (this._torpedoProtect != null)
			{
				Object.Destroy(this._torpedoProtect.get_gameObject());
			}
			this._torpedoProtect = null;
			this._torpedoParticle = null;
			this._clsRaigeki = null;
			Mem.Del<BattleFieldCamera[]>(ref this._fieldCam);
		}

		public void Play(Action callBack)
		{
			this._isPlaying = true;
			this._actCallback = callBack;
			this.stateType = ProdTorpedoSalvoPhase3.StateType.Attack;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(true, this._fieldCam[0].get_transform());
			battleShips.SetBollboardTarget(false, this._fieldCam[1].get_transform());
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(true);
			this.CreateTorpedoWake();
			if (this._isProtect[0])
			{
				this._fieldCam[0].get_transform().set_position(new Vector3(this._torpedoProtect._listBattleShipF.get_Item(0).get_transform().get_position().x, 7.5f, -40f));
				this._fieldCam[0].get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 0f, 0f)));
			}
			if (this._isProtect[1])
			{
				this._fieldCam[1].get_transform().set_position(new Vector3(this._torpedoProtect._listBattleShipE.get_Item(0).get_transform().get_position().x, 7.5f, 42f));
				this._fieldCam[1].get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 180f, 0f)));
			}
			if (this._isProtect[0] || this._isProtect[1])
			{
				this._torpedoProtect.Play(new Action(this._torpedoInjection));
			}
			else
			{
				this._torpedoInjection();
			}
		}

		public bool Update()
		{
			if (this._isPlaying && this.stateType == ProdTorpedoSalvoPhase3.StateType.End)
			{
				this._fTime += Time.get_deltaTime();
				if (this._fTime > 2f)
				{
					this._setState(ProdTorpedoSalvoPhase3.StateType.None);
					this._onTorpedoAttackFinished();
					return true;
				}
			}
			return false;
		}

		private void _setState(ProdTorpedoSalvoPhase3.StateType state)
		{
			this.stateType = state;
			this._fTime = 0f;
		}

		public void SetHpGauge()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			if (BattleTaskManager.GetTorpedoHpGauges().UiPanel == null)
			{
				UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
				BattleTaskManager.GetTorpedoHpGauges().InstancePanel(uIPanel.get_gameObject(), cutInEffectCamera.get_transform().get_gameObject());
			}
			else
			{
				BattleTaskManager.GetTorpedoHpGauges().UiPanel.alpha = 0f;
			}
			this._dicIsCriticall = new Dictionary<FleetType, bool[]>();
			this._dicIsMiss = new Dictionary<FleetType, bool[]>();
			if (BattleTaskManager.GetTorpedoHpGauges().FHpGauge == null)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge = new BattleHPGauges();
			}
			int[] array = new int[this._fBattleship.get_Count()];
			bool[] array2 = new bool[this._fBattleship.get_Count()];
			bool[] array3 = new bool[this._fBattleship.get_Count()];
			List<ShipModel_Defender> defenders = this._clsRaigeki.GetDefenders(true, true);
			for (int i = 0; i < this._fBattleship.get_Count(); i++)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.AddInstantiatesSafe(BattleTaskManager.GetTorpedoHpGauges().UiPanel.get_gameObject(), true, false, true, false, i);
				array[i] = defenders.get_Item(i).HpBefore;
				array2[i] = false;
				array3[i] = false;
			}
			this._dicIsCriticall.Add(FleetType.Friend, array3);
			this._dicIsMiss.Add(FleetType.Friend, array3);
			if (BattleTaskManager.GetTorpedoHpGauges().EHpGauge == null)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge = new BattleHPGauges();
			}
			int[] array4 = new int[this._eBattleship.get_Count()];
			bool[] array5 = new bool[this._eBattleship.get_Count()];
			bool[] array6 = new bool[this._eBattleship.get_Count()];
			List<ShipModel_Defender> defenders2 = this._clsRaigeki.GetDefenders(false, true);
			int count = this._fBattleship.get_Count();
			for (int j = 0; j < this._eBattleship.get_Count(); j++)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.AddInstantiatesSafe(BattleTaskManager.GetTorpedoHpGauges().UiPanel.get_gameObject(), false, false, true, false, j);
				array4[j] = defenders2.get_Item(j).HpBefore;
				array5[j] = false;
				array6[j] = false;
			}
			this._dicIsCriticall.Add(FleetType.Enemy, array6);
			this._dicIsMiss.Add(FleetType.Enemy, array6);
			for (int k = 0; k < this._fBattleship.get_Count(); k++)
			{
				RaigekiDamageModel attackDamage = this._clsRaigeki.GetAttackDamage(defenders.get_Item(k).TmpId);
				int damage = (attackDamage != null) ? attackDamage.GetDamage() : -1;
				BattleHitStatus status = BattleHitStatus.Normal;
				if (this._dicIsMiss.get_Item(FleetType.Friend)[k])
				{
					status = BattleHitStatus.Miss;
				}
				if (this._dicIsCriticall.get_Item(FleetType.Friend)[k])
				{
					status = BattleHitStatus.Clitical;
				}
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.SetHp(k, defenders.get_Item(k).MaxHp, array[k], defenders.get_Item(k).HpAfter, damage, status, false);
			}
			for (int l = 0; l < this._eBattleship.get_Count(); l++)
			{
				RaigekiDamageModel attackDamage2 = this._clsRaigeki.GetAttackDamage(defenders2.get_Item(l).TmpId);
				int damage2 = (attackDamage2 != null) ? attackDamage2.GetDamage() : -1;
				BattleHitStatus status2 = BattleHitStatus.Normal;
				if (this._dicIsMiss.get_Item(FleetType.Enemy)[l])
				{
					status2 = BattleHitStatus.Miss;
				}
				if (this._dicIsCriticall.get_Item(FleetType.Enemy)[l])
				{
					status2 = BattleHitStatus.Clitical;
				}
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.SetHp(l, defenders2.get_Item(l).MaxHp, array4[l], defenders2.get_Item(l).HpAfter, damage2, status2, false);
			}
		}

		private int[] addHpGauge(bool isFriend, BattleHPGauges hpGauges)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			int num = (!isFriend) ? this._eBattleship.get_Count() : this._fBattleship.get_Count();
			int arg_3A_0 = (!isFriend) ? 1 : 0;
			int[] array = new int[num];
			List<ShipModel_Defender> defenders = this._clsRaigeki.GetDefenders(isFriend, true);
			for (int i = 0; i < num; i++)
			{
				hpGauges.AddInstantiates(cutInCamera.get_transform().get_gameObject(), isFriend, false, true, true);
				array[i] = defenders.get_Item(i).HpBefore;
				if (isFriend)
				{
					this._dicIsMiss.get_Item(FleetType.Friend)[i] = false;
					this._dicIsCriticall.get_Item(FleetType.Friend)[i] = false;
				}
				else
				{
					this._dicIsMiss.get_Item(FleetType.Enemy)[i] = false;
					this._dicIsCriticall.get_Item(FleetType.Enemy)[i] = false;
				}
			}
			return array;
		}

		private Vector3 setHpGaugePosition(bool isFriend, int shipCount, int index)
		{
			Vector3[] array = new Vector3[shipCount];
			if (isFriend)
			{
				if (shipCount == 1)
				{
					array[0] = new Vector3(-237f, -210f, 0f);
				}
				else if (shipCount == 2)
				{
					for (int i = 0; i < 2; i++)
					{
						array[i] = new Vector3(-200f - 80f * (float)i, -210f, 0f);
					}
				}
				else if (shipCount == 3)
				{
					for (int j = 0; j < 3; j++)
					{
						array[j] = new Vector3(-165f - 80f * (float)j, -210f, 0f);
					}
				}
				else if (shipCount == 4)
				{
					for (int k = 0; k < 4; k++)
					{
						array[k] = new Vector3(-120f - 80f * (float)k, -210f, 0f);
					}
				}
				else if (shipCount == 5)
				{
					for (int l = 0; l < 5; l++)
					{
						array[l] = new Vector3(-80f - 80f * (float)l, -210f, 0f);
					}
				}
				else if (shipCount == 6)
				{
					for (int m = 0; m < 6; m++)
					{
						array[m] = new Vector3(-42f - 80f * (float)m, -210f, 0f);
					}
				}
			}
			else if (shipCount == 1)
			{
				array[0] = new Vector3(237f, -210f, 0f);
			}
			else if (shipCount == 2)
			{
				for (int n = 0; n < 2; n++)
				{
					array[n] = new Vector3(200f + 80f * (float)n, -210f, 0f);
				}
			}
			else if (shipCount == 3)
			{
				for (int num = 0; num < 3; num++)
				{
					array[num] = new Vector3(165f + 80f * (float)num, -210f, 0f);
				}
			}
			else if (shipCount == 4)
			{
				for (int num2 = 0; num2 < 4; num2++)
				{
					array[num2] = new Vector3(120f + 80f * (float)num2, -210f, 0f);
				}
			}
			else if (shipCount == 5)
			{
				for (int num3 = 0; num3 < 5; num3++)
				{
					array[num3] = new Vector3(80f + 80f * (float)num3, -210f, 0f);
				}
			}
			else if (shipCount == 6)
			{
				for (int num4 = 0; num4 < 6; num4++)
				{
					array[num4] = new Vector3(42f + 80f * (float)num4, -210f, 0f);
				}
			}
			return array[index];
		}

		private void _setProtect()
		{
			this._addProtectShip(this._fBattleship, this._eBattleship, FleetType.Enemy);
			this._addProtectShip(this._eBattleship, this._fBattleship, FleetType.Friend);
		}

		private void _addProtectShip(Dictionary<int, UIBattleShip> attackers, Dictionary<int, UIBattleShip> defenders, FleetType defType)
		{
			bool is_friend = defType == FleetType.Friend;
			ShipModel_BattleAll[] array = (defType != FleetType.Friend) ? BattleTaskManager.GetBattleManager().Ships_f : BattleTaskManager.GetBattleManager().Ships_e;
			for (int i = 0; i < attackers.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = array[i];
				ShipModel_Defender attackTo = this._clsRaigeki.GetAttackTo(shipModel_BattleAll);
				if (shipModel_BattleAll != null && attackTo != null)
				{
					RaigekiDamageModel attackDamage = this._clsRaigeki.GetAttackDamage(attackTo.Index, is_friend);
					if (attackDamage.GetProtectEffect(shipModel_BattleAll.TmpId))
					{
						this._isProtect[(int)defType] = true;
						this._torpedoProtect.AddShipList(defenders.get_Item(0), defenders.get_Item(attackTo.Index), defType);
						break;
					}
				}
			}
		}

		public void CreateTorpedoWake()
		{
			ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
			ShipModel_BattleAll[] ships_e = BattleTaskManager.GetBattleManager().Ships_e;
			this._setProtect();
			for (int i = 0; i < this._fBattleship.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_f[i];
				ShipModel_Defender attackTo = this._clsRaigeki.GetAttackTo(shipModel_BattleAll);
				if (shipModel_BattleAll != null && attackTo != null)
				{
					RaigekiDamageModel attackDamage = this._clsRaigeki.GetAttackDamage(attackTo.Index, false);
					int arg_76_0 = (!attackDamage.GetProtectEffect(shipModel_BattleAll.TmpId)) ? attackTo.Index : 0;
					if (attackDamage.GetHitState(shipModel_BattleAll.TmpId) == BattleHitStatus.Miss)
					{
						this._dicIsMiss.get_Item(FleetType.Enemy)[attackTo.Index] = true;
					}
					if (attackDamage.GetHitState(shipModel_BattleAll.TmpId) == BattleHitStatus.Clitical)
					{
						this._dicIsCriticall.get_Item(FleetType.Enemy)[attackTo.Index] = true;
					}
					Vector3 injection = new Vector3(this._eBattleship.get_Item(attackTo.Index).get_transform().get_position().x, 1f, this._eBattleship.get_Item(attackTo.Index).get_transform().get_position().z + 13f);
					Vector3 target = new Vector3(this._eBattleship.get_Item(attackTo.Index).get_transform().get_position().x, this._eBattleship.get_Item(attackTo.Index).get_transform().get_position().y + 0.5f, this._eBattleship.get_Item(attackTo.Index).get_transform().get_position().z);
					this._listPSTorpedoWake.Add(this._createTorpedo(injection, target, true));
					this._isTorpedoHit[0] = true;
				}
			}
			for (int j = 0; j < this._eBattleship.get_Count(); j++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = ships_e[j];
				ShipModel_Defender attackTo2 = this._clsRaigeki.GetAttackTo(shipModel_BattleAll2);
				if (shipModel_BattleAll2 != null && attackTo2 != null)
				{
					RaigekiDamageModel attackDamage2 = this._clsRaigeki.GetAttackDamage(attackTo2.Index, true);
					int arg_233_0 = (!attackDamage2.GetProtectEffect(shipModel_BattleAll2.TmpId)) ? attackTo2.Index : 0;
					if (attackDamage2.GetHitState(shipModel_BattleAll2.TmpId) == BattleHitStatus.Miss)
					{
						this._dicIsMiss.get_Item(FleetType.Friend)[attackTo2.Index] = true;
					}
					if (attackDamage2.GetHitState(shipModel_BattleAll2.TmpId) == BattleHitStatus.Clitical)
					{
						this._dicIsCriticall.get_Item(FleetType.Friend)[attackTo2.Index] = true;
					}
					Vector3 injection2 = new Vector3(this._fBattleship.get_Item(attackTo2.Index).get_transform().get_position().x, 1f, this._fBattleship.get_Item(attackTo2.Index).get_transform().get_position().z - 13f);
					Vector3 target2 = new Vector3(this._fBattleship.get_Item(attackTo2.Index).get_transform().get_position().x, this._fBattleship.get_Item(attackTo2.Index).get_transform().get_position().y + 0.5f, this._fBattleship.get_Item(attackTo2.Index).get_transform().get_position().z);
					this._listPSTorpedoWake.Add(this._createTorpedo(injection2, target2, true));
					this._isTorpedoHit[1] = true;
				}
			}
		}

		private void _torpedoInjection()
		{
			this._fieldCam[0].get_transform().set_position(new Vector3(-51f, 8f, 90f));
			this._fieldCam[0].get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 70f, 0f)));
			this._fieldCam[1].get_transform().set_position(new Vector3(-51f, 8f, -90f));
			this._fieldCam[1].get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 111f, 0f)));
			bool isFirst = false;
			using (List<PSTorpedoWake>.Enumerator enumerator = this._listPSTorpedoWake.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PSTorpedoWake current = enumerator.get_Current();
					current.SetActive(true);
					current.Injection(iTween.EaseType.linear, false, false, delegate
					{
						if (!isFirst)
						{
							this._compTorpedoAttack();
							isFirst = true;
						}
					});
				}
			}
		}

		private void _setShinking(bool isFriend)
		{
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? this._fBattleship : this._eBattleship;
			List<ShipModel_Defender> list = (!isFriend) ? this._clsRaigeki.GetDefenders(true) : this._clsRaigeki.GetDefenders(false);
			for (int i = 0; i < list.get_Count(); i++)
			{
				if (list.get_Item(i).DmgStateAfter == DamageState_Battle.Gekichin)
				{
					dictionary.get_Item(list.get_Item(i).Index).PlayProdSinking(null);
				}
			}
		}

		private void _compTorpedoAttack()
		{
			if (this._isTorpedoHit[0])
			{
				bool flag = false;
				for (int i = 0; i < this._dicIsCriticall.get_Item(FleetType.Friend).Length; i++)
				{
					if (this._dicIsCriticall.get_Item(FleetType.Friend)[i])
					{
						flag = true;
					}
				}
				KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				this._fieldCam[1].cameraShake.ShakeRot(new Action(this._enemyCameraShakeFinished));
			}
			if (this._isTorpedoHit[1])
			{
				bool flag2 = false;
				for (int j = 0; j < this._dicIsCriticall.get_Item(FleetType.Enemy).Length; j++)
				{
					if (this._dicIsCriticall.get_Item(FleetType.Enemy)[j])
					{
						flag2 = true;
					}
				}
				KCV.Utils.SoundUtils.PlaySE((!flag2) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				this._fieldCam[0].cameraShake.ShakeRot(new Action(this._friendCameraShakeFinished));
			}
			for (int k = 0; k < this._eBattleship.get_Count(); k++)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.Show(k, this.setHpGaugePosition(false, this._eBattleship.get_Count(), k), new Vector3(0.22f, 0.22f, 0.22f), true);
			}
			for (int l = 0; l < this._fBattleship.get_Count(); l++)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.Show(l, this.setHpGaugePosition(true, this._fBattleship.get_Count(), l), new Vector3(0.22f, 0.22f, 0.22f), true);
			}
			BattleTaskManager.GetTorpedoHpGauges().UiPanel.alpha = 1f;
			if (this._isTorpedoHit[1])
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.PlayHpAll(null);
			}
			if (this._isTorpedoHit[0])
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.PlayHpAll(null);
			}
			this._setShinking(true);
			this._setShinking(false);
		}

		private PSTorpedoWake _createTorpedo(Vector3 injection, Vector3 target, bool isRescue)
		{
			PSTorpedoWake pSTorpedoWake = PSTorpedoWake.Instantiate(this._torpedoParticle, BattleTaskManager.GetBattleField().get_transform(), injection, target, 0, 0.6f, true, false);
			pSTorpedoWake.SetActive(false);
			return pSTorpedoWake;
		}

		private void _friendCameraShakeFinished()
		{
			if (this._fieldCam != null)
			{
				this._fieldCam[0].get_transform().set_rotation(Quaternion.Euler(new Vector3(10.5f, 70f, 0f)));
				this._fieldCam[0].get_transform().set_position(new Vector3(-51f, 8f, 90f));
			}
			this._setState(ProdTorpedoSalvoPhase3.StateType.End);
		}

		private void _enemyCameraShakeFinished()
		{
			if (this._fieldCam != null)
			{
				this._fieldCam[1].get_transform().set_rotation(Quaternion.Euler(new Vector3(10.5f, 111f, 0f)));
				this._fieldCam[1].get_transform().set_position(new Vector3(-51f, 8f, -90f));
			}
			this._setState(ProdTorpedoSalvoPhase3.StateType.End);
		}

		private void _onTorpedoAttackFinished()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this.OnSetDestroy();
		}
	}
}
