using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportShelling : MonoBehaviour
	{
		private enum AttackType
		{
			Bomb,
			Torpedo,
			Both,
			None
		}

		private ProdSupportShelling.AttackType _attackType;

		private HpHitState[] _eHitState;

		private Action _actCallback;

		private ShienModel_Hou _clsShelling;

		private BattleFieldCamera _fieldCam;

		private BattleHPGauges _battleHpGauges;

		private ProdAerialRescueCutIn _rescueCutIn;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private List<ShipModel_Defender> _defenders;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private UIPanel _uiHpGaugePanel;

		private bool _isProtect;

		private bool _isEx;

		private bool _isAttack;

		private float _explosionTime;

		private Vector3[] _eHpPos;

		private bool _init()
		{
			this._isEx = false;
			this._isAttack = false;
			this._isProtect = false;
			this._explosionTime = 0f;
			this._eHpPos = null;
			this._defenders = this._clsShelling.GetDefenders(false);
			this._fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			this._rescueCutIn = base.get_transform().SafeGetComponent<ProdAerialRescueCutIn>();
			this._rescueCutIn._init();
			return true;
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
			for (int i = 0; i < this._defenders.get_Count(); i++)
			{
				this._battleHpGauges.AddInstantiates(base.get_gameObject(), true, true, false, false);
			}
		}

		private void OnDestroy()
		{
			this._initParticleList();
			Mem.Del<ProdSupportShelling.AttackType>(ref this._attackType);
			Mem.Del<HpHitState[]>(ref this._eHitState);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ShienModel_Hou>(ref this._clsShelling);
			Mem.Del<BattleFieldCamera>(ref this._fieldCam);
			Mem.Del<UIPanel>(ref this._uiHpGaugePanel);
			Mem.DelListSafe<bool>(ref this._listBombCritical);
			Mem.DelListSafe<ParticleSystem>(ref this._listExplosion);
			Mem.DelListSafe<ParticleSystem>(ref this._listMiss);
			Mem.DelListSafe<ShipModel_Defender>(ref this._defenders);
			if (this._battleHpGauges != null)
			{
				this._battleHpGauges.Dispose();
			}
			Mem.Del<BattleHPGauges>(ref this._battleHpGauges);
			Mem.Del<ProdAerialRescueCutIn>(ref this._rescueCutIn);
			if (this._uiHpGaugePanel != null)
			{
				Object.Destroy(this._uiHpGaugePanel.get_gameObject());
			}
			Mem.Del<UIPanel>(ref this._uiHpGaugePanel);
		}

		public static ProdSupportShelling Instantiate(ProdSupportShelling prefab, ShienModel_Hou model, Transform parent)
		{
			ProdSupportShelling prodSupportShelling = Object.Instantiate<ProdSupportShelling>(prefab);
			prodSupportShelling.get_transform().set_parent(parent);
			prodSupportShelling.get_transform().set_localPosition(Vector3.get_zero());
			prodSupportShelling.get_transform().set_localScale(Vector3.get_one());
			prodSupportShelling._clsShelling = model;
			prodSupportShelling._init();
			return prodSupportShelling;
		}

		public bool Update()
		{
			if (this._attackType == ProdSupportShelling.AttackType.Bomb && this._isEx)
			{
				this._explosionTime += Time.get_deltaTime();
				if (this._explosionTime > 3f)
				{
					this._finishedShelling();
					this._explosionTime = 0f;
					this._isEx = false;
				}
			}
			return true;
		}

		public void Play(Action callback)
		{
			this._actCallback = callback;
			this._eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			this._fieldCam.get_transform().set_localPosition(new Vector3(0f, this._fieldCam.get_transform().get_localPosition().y, 90f));
			BattleTaskManager.GetBattleCameras().cutInCamera.set_enabled(true);
			this._setHpGauge();
			this._createExplosion();
			this._moveCamera();
		}

		private void _initParticleList()
		{
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

		private void _moveCamera()
		{
			this._fieldCam.get_transform().set_localPosition(new Vector3(0f, 3f, 30f));
			this._fieldCam.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			this._fieldCam.get_transform().LTMoveLocal(new Vector3(0f, 3f, -40f), 0.9f).setEase(LeanTweenType.easeInCubic).setOnComplete(delegate
			{
				this._playRescueCutIn();
			});
			this._attackType = ProdSupportShelling.AttackType.Bomb;
		}

		private void _playRescueCutIn()
		{
			if (this._isProtect)
			{
				this._fieldCam.get_transform().set_localPosition(new Vector3(this._rescueCutIn._listBattleShip.get_Item(0).get_transform().get_position().x, 3f, -40f));
				this._rescueCutIn.Play(new Action(this._finishedRescueCutIn));
			}
			else
			{
				this._finishedRescueCutIn();
			}
		}

		private void _finishedRescueCutIn()
		{
			this._fieldCam.get_transform().set_localPosition(new Vector3(0f, 3f, -40f));
			this._fieldCam.get_transform().set_rotation(Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			this._hitExplosion();
		}

		private void _createExplosion()
		{
			this._listBombCritical = new List<bool>();
			this._listExplosion = new List<ParticleSystem>();
			this._listMiss = new List<ParticleSystem>();
			for (int i = 0; i < this._defenders.get_Count(); i++)
			{
				DamageModel attackDamage = this._clsShelling.GetAttackDamage(this._defenders.get_Item(i).TmpId);
				int num = (!attackDamage.GetProtectEffect()) ? this._defenders.get_Item(i).Index : this._defenders.get_Item(i).Index;
				if (attackDamage.GetHitState() != BattleHitStatus.Miss)
				{
					this._isAttack = true;
					ParticleSystem particleSystem = (!(BattleTaskManager.GetParticleFile().explosionAerial == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
					particleSystem.SetActive(true);
					particleSystem.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
					particleSystem.get_transform().set_position(new Vector3(this._eBattleship.get_Item(num).get_transform().get_position().x, 3f, this._eBattleship.get_Item(num).get_transform().get_position().z));
					this._listExplosion.Add(particleSystem);
					this._listBombCritical.Add(attackDamage.GetHitState() == BattleHitStatus.Clitical);
				}
				else
				{
					int iLim = XorRandom.GetILim(0, 2);
					Vector3[] array = new Vector3[]
					{
						new Vector3(5f, 0f, -5f),
						new Vector3(-3f, 0f, 5f),
						new Vector3(4f, 0f, -7f)
					};
					Vector3 position = new Vector3(this._eBattleship.get_Item(num).get_transform().get_position().x + array[iLim].x, 0f, this._eBattleship.get_Item(num).get_transform().get_position().z + array[iLim].z);
					ParticleSystem particleSystem2 = (!(BattleTaskManager.GetParticleFile().splashMiss == null)) ? Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
					particleSystem2.SetActive(true);
					particleSystem2.get_transform().set_parent(BattleTaskManager.GetBattleField().get_transform());
					particleSystem2.get_transform().set_position(position);
					particleSystem2.Stop();
					this._listMiss.Add(particleSystem2);
				}
			}
		}

		private void _hitExplosion()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			BattleTaskManager.GetBattleCameras().cutInCamera.set_enabled(true);
			if (this._isAttack)
			{
				this._fieldCam.cameraShake.ShakeRot(null);
			}
			for (int i = 0; i < this._defenders.get_Count(); i++)
			{
				DamageModel attackDamage = this._clsShelling.GetAttackDamage(this._defenders.get_Item(i).TmpId);
				this._battleHpGauges.Show(i, this._eHpPos[i], new Vector3(0.35f, 0.35f, 0.35f), false);
				this._battleHpGauges.SetDamagePosition(i, new Vector3(this._battleHpGauges.GetDamagePosition(i).x, -525f, 0f));
				this._battleHpGauges.PlayHp(i, null);
				if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					this._battleHpGauges.PlayMiss(i);
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					this._battleHpGauges.PlayCritical(i);
				}
			}
			this._setShinking();
			int eCnt = (this._listExplosion == null) ? 0 : this._listExplosion.get_Count();
			int mCnt = (this._listMiss == null) ? 0 : this._listMiss.get_Count();
			Observable.FromCoroutine(new Func<IEnumerator>(this._playExplosion), false).Subscribe(delegate(Unit _)
			{
				if (eCnt >= mCnt)
				{
					this._isEx = true;
				}
			});
			Observable.FromCoroutine(new Func<IEnumerator>(this._playMissSplash), false).Subscribe(delegate(Unit _)
			{
				if (mCnt >= eCnt)
				{
					this._isEx = true;
				}
			});
			if (this._listExplosion == null && this._listMiss == null)
			{
				this._isEx = true;
			}
			if (this._listExplosion != null & this._listExplosion.get_Count() > 0)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.Explode);
			}
			if (this._listMiss != null & this._listMiss.get_Count() > 0)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
			}
		}

		[DebuggerHidden]
		private IEnumerator _playExplosion()
		{
			ProdSupportShelling.<_playExplosion>c__IteratorEF <_playExplosion>c__IteratorEF = new ProdSupportShelling.<_playExplosion>c__IteratorEF();
			<_playExplosion>c__IteratorEF.<>f__this = this;
			return <_playExplosion>c__IteratorEF;
		}

		[DebuggerHidden]
		private IEnumerator _playMissSplash()
		{
			ProdSupportShelling.<_playMissSplash>c__IteratorF0 <_playMissSplash>c__IteratorF = new ProdSupportShelling.<_playMissSplash>c__IteratorF0();
			<_playMissSplash>c__IteratorF.<>f__this = this;
			return <_playMissSplash>c__IteratorF;
		}

		private void _setShinking()
		{
			for (int i = 0; i < this._defenders.get_Count(); i++)
			{
				if (this._defenders.get_Item(i).DmgStateAfter == DamageState_Battle.Gekichin)
				{
					this._eBattleship.get_Item(i).PlayProdSinking(null);
				}
			}
		}

		private void _setHpGauge()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			bool flag = false;
			this._eHpPos = this._setHpGaugePosition(this._eBattleship.get_Count());
			this._eHitState = new HpHitState[this._eBattleship.get_Count()];
			UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
			this._uiHpGaugePanel = Util.Instantiate(uIPanel.get_gameObject(), cutInCamera.get_transform().get_gameObject(), false, false).GetComponent<UIPanel>();
			for (int i = 0; i < this._defenders.get_Count(); i++)
			{
				DamageModel attackDamage = this._clsShelling.GetAttackDamage(this._defenders.get_Item(i).TmpId);
				if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					this._eHitState[attackDamage.Defender.Index] = HpHitState.Critical;
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					if (this._eHitState[attackDamage.Defender.Index] == HpHitState.None)
					{
						this._eHitState[attackDamage.Defender.Index] = HpHitState.Miss;
					}
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Normal && this._eHitState[attackDamage.Defender.Index] != HpHitState.Critical)
				{
					this._eHitState[attackDamage.Defender.Index] = HpHitState.Hit;
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
				this._battleHpGauges.SetGauge(i, false, true, false, false);
				this._battleHpGauges.SetHp(i, this._defenders.get_Item(i).MaxHp, this._defenders.get_Item(i).HpBefore, this._defenders.get_Item(i).HpAfter, attackDamage.GetDamage(), status, false);
				if (attackDamage.GetProtectEffect() && !flag)
				{
					flag = true;
					this._isProtect = true;
					this._rescueCutIn.AddShipList(this._eBattleship.get_Item(0), this._eBattleship.get_Item(i));
				}
			}
		}

		private Vector3[] _setHpGaugePosition(int count)
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

		private void _finishedShelling()
		{
			this._initParticleList();
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
