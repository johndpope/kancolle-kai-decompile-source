using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportAerialPhase1 : MonoBehaviour
	{
		[SerializeField]
		private UIPanel[] _uiPanel;

		[SerializeField]
		private GameObject[] _uiAirObjF;

		[SerializeField]
		private GameObject[] _uiAirObjE;

		[SerializeField]
		private UITexture[] _bgTex;

		[SerializeField]
		private UIPanel[] _cloudPanel;

		[SerializeField]
		private UIPanel[] _cloudParPanel;

		[SerializeField]
		private ParticleSystem[] _cloudPar;

		[SerializeField]
		private ParticleSystem[] _gunPar;

		private ProdAerialAircraft _aerialAircraft;

		private bool _isPlaying;

		private CutInType _iType;

		private Action _actCallback;

		private ShienModel_Air _clsAerial;

		private BattleFieldCamera _fieldCamera;

		private List<ProdAerialAircraft> _listAircraft;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		public bool _init()
		{
			this._fieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._uiPanel = new UIPanel[2];
			this._uiAirObjF = new GameObject[2];
			this._uiAirObjE = new GameObject[2];
			this._cloudPanel = new UIPanel[2];
			this._bgTex = new UITexture[2];
			this._cloudParPanel = new UIPanel[2];
			this._cloudPar = new ParticleSystem[2];
			this._gunPar = new ParticleSystem[2];
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						Util.FindParentToChild<UIPanel>(ref this._uiPanel[(int)fleetType], base.get_transform(), string.Format("{0}Panel", fleetType.ToString()));
						if (this._uiAirObjF[(int)fleetType] == null)
						{
							this._uiAirObjF[(int)fleetType] = this._uiPanel[(int)fleetType].get_transform().FindChild("FAircraft").get_gameObject();
						}
						if (this._uiAirObjE[(int)fleetType] == null)
						{
							this._uiAirObjE[(int)fleetType] = this._uiPanel[(int)fleetType].get_transform().FindChild("EAircraft").get_gameObject();
						}
						Util.FindParentToChild<UIPanel>(ref this._cloudPanel[(int)fleetType], base.get_transform(), string.Format("{0}CloudPanel", fleetType.ToString()));
						Util.FindParentToChild<UITexture>(ref this._bgTex[(int)fleetType], this._cloudPanel[(int)fleetType].get_transform(), "Bg");
						Util.FindParentToChild<UIPanel>(ref this._cloudParPanel[(int)fleetType], base.get_transform(), string.Format("{0}CloudParPanel", fleetType.ToString()));
						Util.FindParentToChild<ParticleSystem>(ref this._gunPar[(int)fleetType], this._cloudPanel[(int)fleetType].get_transform(), "Gun");
					}
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (this._clsAerial.IsBakugeki_f() || this._clsAerial.IsRaigeki_f())
			{
				flag = true;
			}
			if (this._clsAerial.IsBakugeki_e() || this._clsAerial.IsRaigeki_e())
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				this._iType = CutInType.Both;
			}
			else if (flag && !flag2)
			{
				this._iType = CutInType.FriendOnly;
			}
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			if (this._iType == CutInType.Both)
			{
				battleCameras.SetSplitCameras2D(true);
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else if (this._iType == CutInType.FriendOnly)
			{
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			Observable.FromCoroutine(new Func<IEnumerator>(this._createAsyncAircrafts), false).Subscribe(delegate(Unit _)
			{
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel[]>(ref this._uiPanel);
			Mem.Del<GameObject[]>(ref this._uiAirObjF);
			Mem.Del<GameObject[]>(ref this._uiAirObjE);
			Mem.Del<UITexture[]>(ref this._bgTex);
			Mem.Del<UIPanel[]>(ref this._cloudParPanel);
			Mem.Del<ParticleSystem[]>(ref this._cloudPar);
			Mem.Del<ParticleSystem[]>(ref this._gunPar);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<ShienModel_Air>(ref this._clsAerial);
			Mem.Del<BattleFieldCamera>(ref this._fieldCamera);
			Mem.Del<CutInType>(ref this._iType);
			Mem.DelListSafe<ProdAerialAircraft>(ref this._listAircraft);
		}

		public static ProdSupportAerialPhase1 Instantiate(ProdSupportAerialPhase1 prefab, ShienModel_Air model, Transform parent, Dictionary<int, UIBattleShip> fShips, Dictionary<int, UIBattleShip> eShips)
		{
			ProdSupportAerialPhase1 prodSupportAerialPhase = Object.Instantiate<ProdSupportAerialPhase1>(prefab);
			prodSupportAerialPhase.get_transform().set_parent(parent);
			prodSupportAerialPhase.get_transform().set_localPosition(Vector3.get_zero());
			prodSupportAerialPhase.get_transform().set_localScale(Vector3.get_one());
			prodSupportAerialPhase._clsAerial = model;
			prodSupportAerialPhase._fBattleship = fShips;
			prodSupportAerialPhase._eBattleship = eShips;
			prodSupportAerialPhase._init();
			return prodSupportAerialPhase;
		}

		public void Play(Action callback)
		{
			this._actCallback = callback;
			this._fieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			this._fieldCamera.get_transform().set_localPosition(new Vector3(0f, 12f, 0f));
			this._fieldCamera.get_transform().set_localRotation(Quaternion.Euler(-16f, 0f, 0f));
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.TransparentFX | Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInCamera.depth = 4f;
			cutInEffectCamera.cullingMask = (Generics.Layers.Background | Generics.Layers.CutIn);
			cutInEffectCamera.depth = 5f;
			cutInEffectCamera.glowEffect.set_enabled(false);
			Vector3[] array = new Vector3[]
			{
				cutInCamera.get_transform().get_localPosition(),
				cutInEffectCamera.get_transform().get_localPosition()
			};
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						this._uiPanel[(int)fleetType].get_transform().set_localPosition(array[(int)fleetType]);
						this._cloudPanel[(int)fleetType].get_transform().set_parent((fleetType != FleetType.Friend) ? cutInEffectCamera.get_transform() : cutInCamera.get_transform());
						this._cloudParPanel[(int)fleetType] = null;
						this._cloudPanel[(int)fleetType].get_transform().set_localPosition(Vector3.get_zero());
					}
				}
			}
			if (this._iType == CutInType.Both)
			{
				this._setParticlePanel(FleetType.Friend, cutInCamera.get_transform());
				this._setParticlePanel(FleetType.Enemy, cutInEffectCamera.get_transform());
				this._gunPar[0].get_transform().set_localPosition(new Vector3(400f, this._gunPar[0].get_transform().get_localPosition().y, 0f));
				this._gunPar[1].get_transform().set_localPosition(new Vector3(400f, this._gunPar[1].get_transform().get_localPosition().y, 0f));
			}
			else if (this._iType == CutInType.FriendOnly)
			{
				this._setParticlePanel(FleetType.Friend, cutInCamera.get_transform());
				base.get_transform().set_position(cutInCamera.get_transform().get_position());
				this._uiPanel[0].get_transform().set_localPosition(Vector3.get_zero());
				this._uiAirObjF[0].get_transform().set_localPosition(new Vector3(-280f, 0f, 0f));
				this._gunPar[0].get_transform().set_localPosition(new Vector3(0f, this._gunPar[0].get_transform().get_localPosition().y, 0f));
				this._cloudPanel[1].SetActive(false);
			}
			else if (this._iType == CutInType.EnemyOnly)
			{
			}
			for (int i = 0; i < 2; i++)
			{
				this._cloudPanel[i].get_transform().GetComponent<Animation>().Play();
				if (this._cloudParPanel[i] != null)
				{
					this._cloudPar[i].Play();
				}
			}
			this._playAircraft();
			this._playGunParticle();
			Animation component = base.get_transform().GetComponent<Animation>();
			component.Stop();
			component.Play("AerialStartPhase1_1");
			for (int j = 0; j < 2; j++)
			{
				this._baseMoveTo(new Vector3(this._uiPanel[j].get_transform().get_localPosition().x, 0f, this._uiPanel[j].get_transform().get_localPosition().z), 1.2f, 0.5f, iTween.EaseType.easeOutBack, string.Empty, this._uiPanel[j].get_transform());
			}
		}

		private void _setParticlePanel(FleetType type, Transform trans)
		{
			this._cloudParPanel[(int)type] = base.get_transform().FindChild(string.Format("{0}CloudParPanel", type.ToString())).GetComponent<UIPanel>();
			this._cloudParPanel[(int)type].get_transform().set_parent(trans);
			this._cloudParPanel[(int)type].get_transform().set_localScale(Vector3.get_one());
			this._cloudParPanel[(int)type].get_transform().set_localPosition(Vector3.get_one());
			this._cloudPar[(int)type] = this._cloudParPanel[(int)type].get_transform().FindChild("Smoke").GetComponent<ParticleSystem>();
			this._cloudPar[(int)type].get_transform().set_localEulerAngles((type != FleetType.Friend) ? new Vector3(0f, 90f, 90f) : new Vector3(0f, -90f, 90f));
			this._cloudPar[(int)type].Play();
		}

		private void _playGunParticle()
		{
		}

		private void _stopGunParticle()
		{
		}

		[DebuggerHidden]
		private IEnumerator _createAsyncAircrafts()
		{
			ProdSupportAerialPhase1.<_createAsyncAircrafts>c__IteratorEB <_createAsyncAircrafts>c__IteratorEB = new ProdSupportAerialPhase1.<_createAsyncAircrafts>c__IteratorEB();
			<_createAsyncAircrafts>c__IteratorEB.<>f__this = this;
			return <_createAsyncAircrafts>c__IteratorEB;
		}

		private void _playAircraft()
		{
			for (int i = 0; i < this._listAircraft.get_Count(); i++)
			{
				this._listAircraft.get_Item(i).SubPlay();
			}
		}

		private void _playAircraftPhase2()
		{
			bool flag = false;
			for (int i = 0; i < this._listAircraft.get_Count(); i++)
			{
				this._listAircraft.get_Item(i).Injection(null);
				if (this._listAircraft.get_Item(i).GetPlane().State_Stage2End == PlaneState.Crush)
				{
					flag = true;
				}
			}
			if (flag)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_906);
			}
		}

		private void _moveCamera()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			this._uiPanel[0].get_transform().set_parent(cutInCamera.get_transform());
			this._uiPanel[0].get_transform().set_localPosition(Vector3.get_one());
			this._uiPanel[1].get_transform().set_parent(cutInEffectCamera.get_transform());
			this._uiPanel[1].get_transform().set_localPosition(Vector3.get_one());
			for (int i = 0; i < this._listAircraft.get_Count(); i++)
			{
				if (!(this._listAircraft.get_Item(i) == null))
				{
					if (this._listAircraft.get_Item(i).get_transform().get_parent().get_parent().get_name() == "FriendPanel")
					{
						if (this._listAircraft.get_Item(i)._fleetType == FleetType.Friend)
						{
							this._listAircraft.get_Item(i).EndMove(2000f, 0.8f);
						}
						else if (this._listAircraft.get_Item(i)._fleetType == FleetType.Enemy)
						{
							this._listAircraft.get_Item(i).EndMove(3000f, 0.8f);
						}
					}
					if (this._listAircraft.get_Item(i).get_transform().get_parent().get_parent().get_name() == "EnemyPanel")
					{
						if (this._listAircraft.get_Item(i)._fleetType == FleetType.Friend)
						{
							this._listAircraft.get_Item(i).EndMove(3000f, 0.8f);
						}
						else if (this._listAircraft.get_Item(i)._fleetType == FleetType.Enemy)
						{
							this._listAircraft.get_Item(i).EndMove(2000f, 0.8f);
						}
					}
				}
			}
			for (int j = 0; j < 2; j++)
			{
				this._baseMoveTo(Vector3.get_zero(), 1f, 0f, iTween.EaseType.linear, string.Empty, this._bgTex[j].get_transform());
			}
		}

		private ProdAerialAircraft _instantiateAircraft(Transform target, int num, PlaneModelBase plane, FleetType fleetType)
		{
			if (this._aerialAircraft == null)
			{
				this._aerialAircraft = Resources.Load<ProdAerialAircraft>("Prefabs/Battle/Production/AerialCombat/Aircraft");
			}
			return ProdAerialAircraft.Instantiate(Resources.Load<ProdAerialAircraft>("Prefabs/Battle/Production/AerialCombat/Aircraft"), target, num, 0, plane, fleetType);
		}

		private void _baseMoveTo(Vector3 pos, float time, float delay, iTween.EaseType easeType, string comp, Transform trans)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", pos);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", delay);
			hashtable.Add("time", time);
			hashtable.Add("easeType", easeType);
			hashtable.Add("oncomplete", comp);
			hashtable.Add("oncompletetarget", base.get_gameObject());
			trans.MoveTo(hashtable);
		}

		private void _compAerialAttack()
		{
			this._playAircraftPhase2();
		}

		private void _aerialMoveContact()
		{
			this._moveCamera();
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_934);
		}

		private void _playSE(int id)
		{
		}

		private void _aerialCombatPhase1Finished()
		{
			if (this._listAircraft != null)
			{
				using (List<ProdAerialAircraft>.Enumerator enumerator = this._listAircraft.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProdAerialAircraft current = enumerator.get_Current();
						Object.Destroy(current.get_gameObject());
					}
				}
				this._listAircraft.Clear();
			}
			this._listAircraft = null;
			this._cloudPanel[0].get_transform().set_parent(base.get_transform());
			this._cloudPanel[1].get_transform().set_parent(base.get_transform());
			if (this._cloudParPanel[0] != null)
			{
				this._cloudParPanel[0].get_transform().set_parent(base.get_transform());
			}
			if (this._cloudParPanel[1] != null)
			{
				this._cloudParPanel[1].get_transform().set_parent(base.get_transform());
			}
			Object.Destroy(this._uiPanel[0].get_gameObject());
			Object.Destroy(this._uiPanel[1].get_gameObject());
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
