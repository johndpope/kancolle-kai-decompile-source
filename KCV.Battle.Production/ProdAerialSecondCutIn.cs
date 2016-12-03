using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialSecondCutIn : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] _uiAirObj;

		[SerializeField]
		private GameObject[] _uiShipObj;

		[SerializeField]
		private ParticleSystem _cloudParticle;

		[SerializeField]
		private UITexture[] _uiAircraft;

		[SerializeField]
		public UITexture[] _uiShip;

		private Animation[] _airAnimation;

		private bool _isAnimeFinished;

		private bool _isPlaying;

		private Action _actCallback;

		private CutInType _iType;

		private BattleFieldCamera _camFieldCamera;

		private KoukuuModel _koukuuModel;

		private ProdAntiAerialCutIn _prodAntiAerialCutIn;

		private bool _init()
		{
			this._isAnimeFinished = false;
			GameObject gameObject = base.get_transform().FindChild("Aircraft").get_gameObject();
			this._uiAirObj = new GameObject[3];
			this._uiAircraft = new UITexture[3];
			this._airAnimation = new Animation[3];
			for (int i = 0; i < 3; i++)
			{
				if (this._uiAirObj[i] == null)
				{
					this._uiAirObj[i] = gameObject.get_transform().FindChild("Aircraft" + (i + 1)).get_gameObject();
				}
				if (this._uiAircraft[i] == null)
				{
					this._uiAircraft[i] = this._uiAirObj[i].get_transform().FindChild("Swing/Aircraft").GetComponent<UITexture>();
				}
				this._airAnimation[i] = this._uiAircraft[i].GetComponent<Animation>();
			}
			this._uiShipObj = new GameObject[2];
			this._uiShip = new UITexture[2];
			for (int j = 0; j < 2; j++)
			{
				if (this._uiShipObj[j] == null)
				{
					this._uiShipObj[j] = base.get_transform().FindChild("ShipObj" + (j + 1)).get_gameObject();
				}
				if (this._uiShip[j] == null)
				{
					this._uiShip[j] = this._uiShipObj[j].get_transform().FindChild("Ship").GetComponent<UITexture>();
				}
			}
			if (this._cloudParticle == null)
			{
				this._cloudParticle = base.get_transform().FindChild("Cloud").GetComponent<ParticleSystem>();
			}
			this._camFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._camFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			return true;
		}

		private void OnDestroy()
		{
			Mem.DelArySafe<GameObject>(ref this._uiAirObj);
			Mem.DelArySafe<GameObject>(ref this._uiShipObj);
			Mem.Del(ref this._cloudParticle);
			Mem.DelArySafe<UITexture>(ref this._uiAircraft);
			Mem.DelArySafe<UITexture>(ref this._uiShip);
			Mem.DelArySafe<Animation>(ref this._airAnimation);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<CutInType>(ref this._iType);
			Mem.Del<BattleFieldCamera>(ref this._camFieldCamera);
			Mem.Del<KoukuuModel>(ref this._koukuuModel);
			if (this._prodAntiAerialCutIn != null)
			{
				Object.Destroy(this._prodAntiAerialCutIn.get_gameObject());
			}
			Mem.Del<ProdAntiAerialCutIn>(ref this._prodAntiAerialCutIn);
		}

		public static ProdAerialSecondCutIn Instantiate(ProdAerialSecondCutIn prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialSecondCutIn prodAerialSecondCutIn = Object.Instantiate<ProdAerialSecondCutIn>(prefab);
			prodAerialSecondCutIn.get_transform().set_parent(parent);
			prodAerialSecondCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodAerialSecondCutIn.get_transform().set_localScale(Vector3.get_one());
			prodAerialSecondCutIn._koukuuModel = model;
			return prodAerialSecondCutIn;
		}

		private void _setShipTexture(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				List<ShipModel_Attacker> list = (this._koukuuModel.GetCaptainShip(true) == null) ? null : this._koukuuModel.GetAttackers(true);
				if (list != null)
				{
					this._uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list.get_Item(0));
					this._uiShip[0].MakePixelPerfect();
					this._uiShip[0].get_transform().set_localPosition(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list.get_Item(0), MstShipGraphColumn.CutInSp1));
					if (list.get_Count() >= 2)
					{
						this._uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list.get_Item(1));
						this._uiShip[1].MakePixelPerfect();
						this._uiShip[1].get_transform().set_localPosition(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list.get_Item(1), MstShipGraphColumn.CutInSp1));
					}
					else
					{
						this._uiShip[1].mainTexture = null;
					}
				}
				else
				{
					this._uiShip[0].mainTexture = null;
					this._uiShip[1].mainTexture = null;
				}
			}
			else if (type == FleetType.Enemy)
			{
				List<ShipModel_Attacker> list2 = (this._koukuuModel.GetCaptainShip(false) == null) ? null : this._koukuuModel.GetAttackers(false);
				if (list2 != null)
				{
					this._uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list2.get_Item(0));
					this._uiShip[0].MakePixelPerfect();
					this._uiShip[0].flip = UIBasicSprite.Flip.Horizontally;
					Vector3 shipOffsPos = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list2.get_Item(0), MstShipGraphColumn.CutInSp1);
					this._uiShip[0].get_transform().set_localPosition(new Vector3(shipOffsPos.x * -1f, shipOffsPos.y, shipOffsPos.z));
					if (list2.get_Count() >= 2)
					{
						this._uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list2.get_Item(1));
						this._uiShip[1].MakePixelPerfect();
						this._uiShip[1].flip = UIBasicSprite.Flip.Horizontally;
						Vector3 shipOffsPos2 = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list2.get_Item(1), MstShipGraphColumn.CutInSp1);
						this._uiShip[1].get_transform().set_localPosition(new Vector3(shipOffsPos2.x * -1f, shipOffsPos2.y, shipOffsPos2.z));
					}
					else
					{
						this._uiShip[1].mainTexture = null;
					}
				}
				else
				{
					this._uiShip[0].mainTexture = null;
					this._uiShip[1].mainTexture = null;
				}
			}
		}

		public void Play(Action callback)
		{
			this._isPlaying = true;
			this._actCallback = callback;
			base.get_transform().GetComponent<UIPanel>().widgetsAreStatic = false;
			this._init();
			this._iType = this._chkCutInType();
			this._setCutin(this._iType);
		}

		public CutInType _chkCutInType()
		{
			if (this._koukuuModel.GetCaptainShip(true) != null && this._koukuuModel.GetCaptainShip(false) != null)
			{
				return CutInType.Both;
			}
			if (this._koukuuModel.GetCaptainShip(true) != null)
			{
				return CutInType.FriendOnly;
			}
			return CutInType.EnemyOnly;
		}

		public bool _cutinPhaseCheck()
		{
			return this._iType == CutInType.Both;
		}

		private void _setCutin(CutInType type)
		{
			if (type == CutInType.Both || type == CutInType.FriendOnly)
			{
				this._setShipTexture(FleetType.Friend);
				this._setAircraftTexture(FleetType.Friend);
				for (int i = 0; i < 3; i++)
				{
					this._airAnimation[i].Stop();
					this._airAnimation[i].Play("AircraftCutin" + (i + 1));
				}
				Animation component = base.get_transform().GetComponent<Animation>();
				component.Stop();
				component.Play("AerialSecondCutIn1");
			}
			else if (type == CutInType.EnemyOnly)
			{
				this._setShipTexture(FleetType.Enemy);
				this._setAircraftTexture(FleetType.Enemy);
				this._changeSeaWave(FleetType.Enemy);
				base.get_transform().set_localEulerAngles(new Vector3(0f, -180f, 0f));
				for (int j = 0; j < 3; j++)
				{
					this._airAnimation[j].Stop();
					this._airAnimation[j].Play("AircraftCutin" + (j + 1));
				}
				Animation component2 = base.get_transform().GetComponent<Animation>();
				component2.Stop();
				component2.Play("AerialSecondCutIn1");
				this._isAnimeFinished = true;
			}
		}

		private void _setAircraftTexture(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				if (this._koukuuModel.GetCaptainShip(true) != null)
				{
					PlaneModelBase[] plane = this._koukuuModel.GetPlane(this._koukuuModel.GetCaptainShip(true).TmpId);
					if (plane != null)
					{
						for (int i = 0; i < plane.Length; i++)
						{
							if (i >= 3)
							{
								break;
							}
							if (plane[i] != null)
							{
								this._uiAirObj[i].get_transform().set_localPosition(new Vector3(267f, 176f, 0f));
								this._uiAircraft[i].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6);
								this._uiAirObj[i].get_transform().set_localEulerAngles(new Vector3(0f, 0f, -25.5f));
								AircraftOffsetInfo aircraftOffsetInfo = KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[i].MstId);
							}
							else
							{
								this._uiAircraft[i].mainTexture = null;
							}
						}
					}
				}
			}
			else if (type == FleetType.Enemy && this._koukuuModel.GetCaptainShip(false) != null)
			{
				PlaneModelBase[] plane = this._koukuuModel.GetPlane(this._koukuuModel.GetCaptainShip(false).TmpId);
				if (plane != null)
				{
					for (int j = 0; j < plane.Length; j++)
					{
						if (j >= 3)
						{
							break;
						}
						if (plane[j] != null)
						{
							if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
							{
								this._uiAircraft[j].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[j].MstId, 6);
							}
							else
							{
								this._uiAirObj[j].get_transform().set_localPosition(new Vector3(267f, 176f, 0f));
								this._uiAircraft[j].mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadTexture(plane[j]);
								this._uiAircraft[j].MakePixelPerfect();
								this._uiAirObj[j].get_transform().set_localEulerAngles(Vector3.get_zero());
								if (KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[j].MstId).isFlipHorizontal)
								{
									this._uiAircraft[j].flip = UIBasicSprite.Flip.Nothing;
								}
								else
								{
									this._uiAircraft[j].flip = UIBasicSprite.Flip.Horizontally;
								}
							}
						}
						else
						{
							this._uiAircraft[j].mainTexture = null;
						}
					}
				}
			}
		}

		private void _changeSeaWave(FleetType type)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			if (type == FleetType.Friend)
			{
				battleField.seaLevel.set_waveSpeed(new Vector4(-4f, -500f, 5f, -400f));
				this._camFieldCamera.get_transform().set_localPosition(new Vector3(50f, 4f, 100f));
				this._camFieldCamera.get_transform().set_rotation(Quaternion.Euler(new Vector3(22f, 20f, 0f)));
			}
			else if (type == FleetType.Enemy)
			{
				battleField.seaLevel.set_waveSpeed(new Vector4(-4f, 500f, 5f, 400f));
				this._camFieldCamera.get_transform().set_localPosition(new Vector3(50f, 4f, 100f));
				this._camFieldCamera.get_transform().set_rotation(Quaternion.Euler(new Vector3(22f, 160f, 0f)));
			}
		}

		private void _startAerialCombatCutIn()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
			if (this._iType == CutInType.Both || this._iType == CutInType.FriendOnly)
			{
				KCV.Battle.Utils.ShipUtils.PlayAircraftCutInVoice(this._koukuuModel.GetCaptainShip(true));
				this._changeSeaWave(FleetType.Friend);
			}
			else if (this._iType == CutInType.EnemyOnly)
			{
				this._changeSeaWave(FleetType.Enemy);
			}
		}

		public void _playCloudParticle()
		{
			this._cloudParticle.Play();
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.RadarDeployment(false);
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SwitchMainCamera(FleetType.Friend);
			battleCameras.InitEnemyFieldCameraDefault();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
		}

		private void _playSE(int id)
		{
			if (id == 0)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_048);
			}
		}

		public void _cutinAnimationFinished()
		{
			if (this._iType == CutInType.FriendOnly || this._iType == CutInType.EnemyOnly)
			{
				this._startAntiAerialCutIn();
			}
			else if (this._iType == CutInType.Both)
			{
				if (!this._isAnimeFinished)
				{
					this._setCutin(CutInType.EnemyOnly);
					Animation component = base.get_transform().GetComponent<Animation>();
					component.Play("AerialSecondCutIn2");
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
				}
				else
				{
					this._startAntiAerialCutIn();
				}
			}
		}

		public void _startAntiAerialCutIn()
		{
			if (this._koukuuModel.GetTaikuShip(true) != null)
			{
				if (this._prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					this._prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), this._koukuuModel, cutInEffectCamera.get_transform());
				}
				this._prodAntiAerialCutIn.Play(new Action(this._compAntiAerialCutInEnemy), true);
			}
			else if (this._koukuuModel.GetTaikuShip(false) != null)
			{
				if (this._prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera2 = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					this._prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), this._koukuuModel, cutInEffectCamera2.get_transform());
				}
				this._prodAntiAerialCutIn.Play(new Action(this._compAntiAerialCutInEnemy), false);
			}
			else
			{
				this._compAntiAerialCutInEnemy();
			}
		}

		private void _compAntiAerialCutInFriend()
		{
			if (this._koukuuModel.GetTaikuShip(false) != null)
			{
				if (this._prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					this._prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), this._koukuuModel, cutInEffectCamera.get_transform());
				}
				this._prodAntiAerialCutIn.Play(new Action(this._compAntiAerialCutInEnemy), false);
			}
			else
			{
				this._compAntiAerialCutInEnemy();
			}
		}

		private void _compAntiAerialCutInEnemy()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}
	}
}
