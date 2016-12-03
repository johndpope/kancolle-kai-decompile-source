using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialCombatP1 : MonoBehaviour
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

		[SerializeField]
		private UIPanel[] _labelPanel;

		[SerializeField]
		private UITexture[] _supremacyTxt;

		private ProdAerialAircraft _aerialAircraft;

		private Action _actCallback;

		private KoukuuModel _clsKoukuu;

		private CutInType _iType;

		private BattleFieldCamera _fieldCamera;

		private List<ProdAerialAircraft> _listAircraft;

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
			this._labelPanel = new UIPanel[2];
			this._supremacyTxt = new UITexture[2];
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
						Util.FindParentToChild<UIPanel>(ref this._labelPanel[(int)fleetType], base.get_transform(), string.Format("{0}LabelPanel", fleetType.ToString()));
						Util.FindParentToChild<UITexture>(ref this._supremacyTxt[(int)fleetType], this._labelPanel[(int)fleetType].get_transform(), "SupremacyTxt");
					}
				}
			}
			this._labelPanel[1].SetActive(false);
			if (this._iType == CutInType.Both)
			{
				this._labelPanel[0].SetLayer(8);
				this._labelPanel[1].SetLayer(1);
				this._labelPanel[1].SetActive(true);
			}
			else if (this._iType == CutInType.EnemyOnly)
			{
				this._supremacyTxt[0].get_transform().set_localScale(Vector3.get_one());
				this._labelPanel[0].SetLayer(14);
			}
			else if (this._iType == CutInType.FriendOnly)
			{
				this._supremacyTxt[0].get_transform().set_localScale(Vector3.get_one());
				this._labelPanel[0].SetLayer(14);
			}
			this._createAsyncAircrafts();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel[]>(ref this._uiPanel);
			Mem.DelArySafe<GameObject>(ref this._uiAirObjF);
			Mem.DelArySafe<GameObject>(ref this._uiAirObjE);
			Mem.DelArySafe<UITexture>(ref this._bgTex);
			Mem.DelArySafe<UIPanel>(ref this._cloudPanel);
			Mem.DelArySafe<UIPanel>(ref this._cloudParPanel);
			Mem.DelArySafe<ParticleSystem>(ref this._cloudPar);
			Mem.DelArySafe<ParticleSystem>(ref this._gunPar);
			Mem.DelArySafe<UIPanel>(ref this._labelPanel);
			Mem.DelArySafe<UITexture>(ref this._supremacyTxt);
			Mem.Del<ProdAerialAircraft>(ref this._aerialAircraft);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<KoukuuModel>(ref this._clsKoukuu);
			Mem.Del<CutInType>(ref this._iType);
			Mem.Del<BattleFieldCamera>(ref this._fieldCamera);
			Mem.DelListSafe<ProdAerialAircraft>(ref this._listAircraft);
		}

		public static ProdAerialCombatP1 Instantiate(ProdAerialCombatP1 prefab, KoukuuModel model, Transform parent, CutInType iType)
		{
			ProdAerialCombatP1 prodAerialCombatP = Object.Instantiate<ProdAerialCombatP1>(prefab);
			prodAerialCombatP.get_transform().set_parent(parent);
			prodAerialCombatP.get_transform().set_localPosition(Vector3.get_zero());
			prodAerialCombatP.get_transform().set_localScale(Vector3.get_one());
			prodAerialCombatP._clsKoukuu = model;
			prodAerialCombatP._iType = iType;
			prodAerialCombatP._init();
			return prodAerialCombatP;
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
			this.setAirSupremacyLabel();
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
				this._labelPanel[0].get_transform().set_localPosition(new Vector3(array[1].x, array[1].y + 272f, array[1].z));
				this._labelPanel[1].get_transform().set_localPosition(new Vector3(cutInCamera.get_transform().get_localPosition().x, cutInCamera.get_transform().get_localPosition().y - 272f, cutInCamera.get_transform().get_localPosition().z));
				this._gunPar[0].get_transform().set_localPosition(new Vector3(400f, this._gunPar[0].get_transform().get_localPosition().y, 0f));
				this._gunPar[1].get_transform().set_localPosition(new Vector3(400f, this._gunPar[1].get_transform().get_localPosition().y, 0f));
			}
			else if (this._iType == CutInType.FriendOnly)
			{
				this._setParticlePanel(FleetType.Friend, cutInCamera.get_transform());
				this._labelPanel[0].get_transform().set_localPosition(new Vector3(array[0].x, array[0].y - 175f, array[0].z));
				base.get_transform().set_position(cutInCamera.get_transform().get_position());
				this._uiPanel[0].get_transform().set_localPosition(Vector3.get_zero());
				this._uiAirObjF[0].get_transform().set_localPosition(new Vector3(-280f, 0f, 0f));
				this._gunPar[0].get_transform().set_localPosition(new Vector3(0f, this._gunPar[0].get_transform().get_localPosition().y, 0f));
				this._cloudPanel[1].SetActive(false);
			}
			else if (this._iType == CutInType.EnemyOnly)
			{
				this._setParticlePanel(FleetType.Enemy, cutInEffectCamera.get_transform());
				this._labelPanel[0].get_transform().set_localPosition(new Vector3(array[1].x, array[1].y - 175f, array[1].z));
				base.get_transform().set_position(cutInEffectCamera.get_transform().get_position());
				this._uiPanel[1].get_transform().set_localPosition(Vector3.get_zero());
				this._uiAirObjE[1].get_transform().set_localPosition(new Vector3(280f, 0f, 0f));
				this._gunPar[1].get_transform().set_localPosition(new Vector3(0f, this._gunPar[1].get_transform().get_localPosition().y, 0f));
				this._cloudPanel[0].SetActive(false);
				cutInEffectCamera.isCulling = true;
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

		private void setAirSupremacyLabel()
		{
			switch (this._clsKoukuu.SeikuKind)
			{
			case BattleSeikuKinds.None:
				this._labelPanel[0].SetActive(false);
				this._labelPanel[1].SetActive(false);
				this._supremacyTxt[0].mainTexture = null;
				this._supremacyTxt[1].mainTexture = null;
				break;
			case BattleSeikuKinds.Kakuho:
				this._supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_control1") as Texture2D);
				this._supremacyTxt[1].mainTexture = this._supremacyTxt[0].mainTexture;
				break;
			case BattleSeikuKinds.Yuusei:
				this._supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_superior") as Texture2D);
				this._supremacyTxt[1].mainTexture = this._supremacyTxt[0].mainTexture;
				break;
			case BattleSeikuKinds.Ressei:
				this._labelPanel[0].SetActive(false);
				this._labelPanel[1].SetActive(false);
				this._supremacyTxt[0].mainTexture = null;
				this._supremacyTxt[1].mainTexture = null;
				break;
			case BattleSeikuKinds.Lost:
				this._supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_control2") as Texture2D);
				this._supremacyTxt[1].mainTexture = this._supremacyTxt[0].mainTexture;
				break;
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
			for (int i = 0; i < this._gunPar.Length; i++)
			{
				this._gunPar[i].Play();
			}
		}

		private void _stopGunParticle()
		{
			for (int i = 0; i < this._gunPar.Length; i++)
			{
				this._gunPar[i].Stop();
			}
		}

		private void _createAsyncAircrafts()
		{
			this._listAircraft = new List<ProdAerialAircraft>();
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			if (this._iType == CutInType.Both || this._iType == CutInType.FriendOnly)
			{
				this._createAircraft(dicFriendBattleShips, FleetType.Friend, this._uiAirObjF);
			}
			if (this._iType == CutInType.Both || this._iType == CutInType.EnemyOnly)
			{
				this._createAircraft(dicEnemyBattleShips, FleetType.Enemy, this._uiAirObjE);
			}
		}

		private void _createAircraft(Dictionary<int, UIBattleShip> ships, FleetType type, GameObject[] objects)
		{
			int num = (type != FleetType.Friend) ? 1 : 0;
			int num2 = (type != FleetType.Friend) ? 0 : 1;
			for (int i = 0; i < ships.get_Count(); i++)
			{
				if (ships.get_Item(i) != null && ships.get_Item(i).shipModel != null)
				{
					PlaneModelBase[] plane = this._clsKoukuu.GetPlane(ships.get_Item(i).shipModel.TmpId);
					if (plane != null)
					{
						for (int j = 0; j < plane.Length; j++)
						{
							if (plane[j] != null)
							{
								this._listAircraft.Add(this._instantiateAircraft(objects[num].get_transform(), i, plane[j], type));
								if (this._iType == CutInType.Both)
								{
									this._listAircraft.Add(this._instantiateAircraft(objects[num2].get_transform(), i, plane[j], type));
								}
								break;
							}
						}
					}
				}
			}
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
			for (int i = 0; i < 2; i++)
			{
				Animation component = this._labelPanel[i].get_transform().GetComponent<Animation>();
				component.Stop();
				component.Play();
			}
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_934);
		}

		private void _playSE(int id)
		{
			if (id == 1)
			{
				bool flag = false;
				ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
				switch (this._iType)
				{
				case CutInType.EnemyOnly:
					for (int i = 0; i < ships_f.Length; i++)
					{
						if (ships_f[i] != null && ships_f[i].ClassType == 54)
						{
							flag = true;
						}
					}
					break;
				case CutInType.Both:
					for (int j = 0; j < ships_f.Length; j++)
					{
						if (ships_f[j] != null && ships_f[j].ClassType == 54)
						{
							flag = true;
						}
					}
					break;
				}
				KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_916 : SEFIleInfos.SE_917);
			}
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
			this._actCallback.Invoke();
		}
	}
}
