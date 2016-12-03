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
	public class ProdSupportTorpedoP2
	{
		public enum StateType
		{
			None,
			Attack,
			End
		}

		public ProdSupportTorpedoP2.StateType stateType;

		private HpHitState[] _eHitState;

		private Action _actCallback;

		private ShienModel_Rai _clsTorpedo;

		private BattleFieldCamera _camFriend;

		private PSTorpedoWake _torpedoParticle;

		private List<PSTorpedoWake> _listPSTorpedoWake;

		private ProdAerialRescueCutIn _rescueCutIn;

		private BattleHPGauges _battleHpGauges;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private UIPanel _uiHpGaugePanel;

		private bool _isPlaying;

		private bool _isProtect;

		private bool _isAttackE;

		private float _fTime;

		public Transform transform;

		public ProdSupportTorpedoP2(Transform obj)
		{
			this.transform = obj;
		}

		public void Initialize(ShienModel_Rai model, PSTorpedoWake torpedoWake)
		{
			this._fTime = 0f;
			this.stateType = ProdSupportTorpedoP2.StateType.None;
			this._clsTorpedo = model;
			this._isAttackE = false;
			this._camFriend = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(false, this._camFriend.get_transform());
			this._torpedoParticle = torpedoWake;
			TaskBattleTorpedoSalvo taskTorpedoSalvo = BattleTaskManager.GetTaskTorpedoSalvo();
			Transform prefabProdTorpedoResucueCutIn = BattleTaskManager.GetPrefabFile().prefabProdTorpedoResucueCutIn;
			this._rescueCutIn = this.transform.SafeGetComponent<ProdAerialRescueCutIn>();
			this._rescueCutIn._init();
		}

		public void OnSetDestroy()
		{
			Mem.Del<ProdSupportTorpedoP2.StateType>(ref this.stateType);
			Mem.Del<HpHitState[]>(ref this._eHitState);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ShienModel_Rai>(ref this._clsTorpedo);
			Mem.Del<BattleFieldCamera>(ref this._camFriend);
			Mem.Del<PSTorpedoWake>(ref this._torpedoParticle);
			Mem.Del<Transform>(ref this.transform);
			Mem.DelListSafe<PSTorpedoWake>(ref this._listPSTorpedoWake);
			if (this._rescueCutIn != null)
			{
				this._rescueCutIn.get_gameObject().Discard();
			}
			Mem.Del<ProdAerialRescueCutIn>(ref this._rescueCutIn);
			if (this._battleHpGauges != null)
			{
				this._battleHpGauges.Dispose();
			}
			Mem.Del<BattleHPGauges>(ref this._battleHpGauges);
			if (this._uiHpGaugePanel != null)
			{
				Object.Destroy(this._uiHpGaugePanel.get_gameObject());
			}
			Mem.Del<UIPanel>(ref this._uiHpGaugePanel);
		}

		public void CreateHpGauge(FleetType type)
		{
			if (this._battleHpGauges == null)
			{
				this._battleHpGauges = new BattleHPGauges();
			}
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
			this._uiHpGaugePanel = Util.Instantiate(uIPanel.get_gameObject(), cutInEffectCamera.get_transform().get_gameObject(), false, false).GetComponent<UIPanel>();
			for (int i = 0; i < 6; i++)
			{
				this._battleHpGauges.AddInstantiates(this._uiHpGaugePanel.get_gameObject(), true, true, false, false);
			}
		}

		public void Play(Action callBack)
		{
			this._listPSTorpedoWake = new List<PSTorpedoWake>();
			this._isPlaying = true;
			this.stateType = ProdSupportTorpedoP2.StateType.Attack;
			this._actCallback = callBack;
			this._eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			this._setHpGauge();
			this._createTorpedoWake();
			if (this._isProtect)
			{
				this._camFriend.get_transform().set_localPosition(new Vector3(this._rescueCutIn._listBattleShip.get_Item(0).get_transform().get_position().x, 3f, -40f));
				this._camFriend.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
				this._rescueCutIn.Play(new Action(this._torpedoInjection));
			}
			else
			{
				this._torpedoInjection();
			}
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(true);
			this._camFriend.motionBlur.set_enabled(false);
		}

		public bool Update()
		{
			if (this._isPlaying && this.stateType == ProdSupportTorpedoP2.StateType.End)
			{
				this._fTime += Time.get_deltaTime();
				if (this._fTime > 2.4f)
				{
					this._setState(ProdSupportTorpedoP2.StateType.None);
					this._onTorpedoAttackFinished();
					return true;
				}
			}
			return false;
		}

		private void _setState(ProdSupportTorpedoP2.StateType state)
		{
			this.stateType = state;
			this._fTime = 0f;
		}

		private void _setHpGauge()
		{
			bool flag = false;
			this._eHitState = new HpHitState[this._eBattleship.get_Count()];
			List<ShipModel_Defender> defenders = this._clsTorpedo.GetDefenders(false);
			for (int i = 0; i < defenders.get_Count(); i++)
			{
				DamageModel attackDamage = this._clsTorpedo.GetAttackDamage(defenders.get_Item(i).TmpId);
				switch (attackDamage.GetHitState())
				{
				case BattleHitStatus.Miss:
					if (this._eHitState[i] == HpHitState.None)
					{
						this._eHitState[i] = HpHitState.Miss;
					}
					break;
				case BattleHitStatus.Normal:
					if (this._eHitState[i] != HpHitState.Critical)
					{
						this._eHitState[i] = HpHitState.Hit;
					}
					break;
				case BattleHitStatus.Clitical:
					this._eHitState[i] = HpHitState.Critical;
					break;
				}
				BattleHitStatus status;
				if (this._eHitState[i] == HpHitState.Miss)
				{
					status = BattleHitStatus.Miss;
				}
				else if (this._eHitState[i] == HpHitState.Critical)
				{
					status = BattleHitStatus.Clitical;
				}
				else
				{
					status = BattleHitStatus.Normal;
				}
				this._battleHpGauges.SetGauge(i, false, false, true, false);
				this._battleHpGauges.SetHp(i, defenders.get_Item(i).MaxHp, defenders.get_Item(i).HpBefore, defenders.get_Item(i).HpAfter, attackDamage.GetDamage(), status, false);
				if (attackDamage.GetProtectEffect() && !flag)
				{
					flag = true;
					this._isProtect = true;
					this._rescueCutIn.AddShipList(this._eBattleship.get_Item(0), this._eBattleship.get_Item(i));
				}
			}
		}

		private void _createTorpedoWake()
		{
			ShipModel_BattleAll[] ships_e = BattleTaskManager.GetBattleManager().Ships_e;
			List<ShipModel_Defender> defenders = this._clsTorpedo.GetDefenders(false);
			for (int i = 0; i < defenders.get_Count(); i++)
			{
				DamageModel attackDamage = this._clsTorpedo.GetAttackDamage(defenders.get_Item(i).TmpId);
				Vector3 vector = (!attackDamage.GetProtectEffect()) ? this._eBattleship.get_Item(i).get_transform().get_position() : this._eBattleship.get_Item(i).get_transform().get_position();
				Vector3 target = (attackDamage.GetHitState() != BattleHitStatus.Miss) ? new Vector3(vector.x, vector.y + 1f, vector.z) : new Vector3(vector.x - 1.5f, vector.y - 1.5f, vector.z - 20f);
				this._listPSTorpedoWake.Add(this._createTorpedo(new Vector3(this._eBattleship.get_Item(i).get_transform().get_position().x, 1f, this._eBattleship.get_Item(i).get_transform().get_position().z + 40f), target, (attackDamage.GetHitState() != BattleHitStatus.Miss) ? 0.6f : 1f, attackDamage.GetHitState() != BattleHitStatus.Miss));
				if (attackDamage.GetHitState() != BattleHitStatus.Miss)
				{
					this._isAttackE = true;
				}
			}
		}

		private void _torpedoInjection()
		{
			this._camFriend.get_transform().set_position(new Vector3(-38f, 8f, -74f));
			this._camFriend.get_transform().set_localRotation(Quaternion.Euler(new Vector3(9.5f, 137.5f, 0f)));
			bool isFirst = false;
			using (List<PSTorpedoWake>.Enumerator enumerator = this._listPSTorpedoWake.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PSTorpedoWake current = enumerator.get_Current();
					current.Injection(iTween.EaseType.linear, false, false, delegate
					{
						if (!isFirst)
						{
							this._setShinking();
							this._compTorpedoAttack();
							isFirst = true;
						}
					});
				}
			}
		}

		private void _setShinking()
		{
			List<ShipModel_Defender> defenders = this._clsTorpedo.GetDefenders(false);
			for (int i = 0; i < defenders.get_Count(); i++)
			{
				if (defenders.get_Item(i).DmgStateAfter == DamageState_Battle.Gekichin)
				{
					this._eBattleship.get_Item(i).PlayProdSinking(null);
				}
			}
		}

		private void _compTorpedoAttack()
		{
			bool flag = false;
			float[] array = new float[]
			{
				-416f,
				-310f,
				-205f,
				-70f,
				115f,
				350f
			};
			List<ShipModel_Defender> defenders = this._clsTorpedo.GetDefenders(false);
			for (int i = 0; i < defenders.get_Count(); i++)
			{
				this._battleHpGauges.Show(i, new Vector3(this.setHpGaugePosition(defenders.get_Count(), i), -210f, 0f), new Vector3(0.22f, 0.22f, 0.22f), false);
				this._battleHpGauges.PlayHp(i, null);
				DamageModel attackDamage = this._clsTorpedo.GetAttackDamage(defenders.get_Item(i).TmpId);
				if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					this._battleHpGauges.PlayMiss(i);
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					flag = true;
					this._battleHpGauges.PlayCritical(i);
				}
			}
			if (this._isAttackE)
			{
				KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				this._camFriend.cameraShake.ShakeRot(null);
			}
			this._setState(ProdSupportTorpedoP2.StateType.End);
		}

		private float setHpGaugePosition(int shipCount, int index)
		{
			float[] array = null;
			if (shipCount == 1)
			{
				return -150f;
			}
			if (shipCount == 2)
			{
				array = new float[]
				{
					-209f,
					-61f
				};
			}
			else if (shipCount == 3)
			{
				array = new float[]
				{
					-278f,
					-150f,
					-12f
				};
			}
			else if (shipCount == 4)
			{
				array = new float[]
				{
					-321f,
					-202f,
					-72f,
					110f
				};
			}
			else if (shipCount == 5)
			{
				array = new float[]
				{
					-372f,
					-265f,
					-160f,
					-9f,
					229f
				};
			}
			else if (shipCount == 6)
			{
				array = new float[]
				{
					-416.5f,
					-310f,
					-205f,
					-70f,
					115f,
					350f
				};
			}
			return array[index];
		}

		private PSTorpedoWake _createTorpedo(Vector3 injection, Vector3 target, float time, bool isDet)
		{
			return PSTorpedoWake.Instantiate(this._torpedoParticle, BattleTaskManager.GetBattleField().get_transform(), injection, target, 0, time, isDet, false);
		}

		private void _onTorpedoAttackFinished()
		{
			if (this._battleHpGauges != null)
			{
				this._battleHpGauges.Dispose();
			}
			this._battleHpGauges = null;
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
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this.OnSetDestroy();
		}
	}
}
