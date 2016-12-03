using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.managers;
using local.models;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialCombatCutinP : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] _uiAirObj;

		[SerializeField]
		private GameObject _uiShipObj;

		[SerializeField]
		private ParticleSystem _cloudParticle;

		[SerializeField]
		private UITexture[] _uiAircraft;

		[SerializeField]
		public UITexture _uiShip;

		private Animation[] _airAnimation;

		private bool isAnimeFinished;

		private bool _isPlaying;

		private Action _actCallback;

		private CutInType _iType;

		private KoukuuModel _koukuuModel;

		private BattleFieldCamera _camFieldCamera;

		private ProdAntiAerialCutIn _prodAntiAerialCutIn;

		private bool _init()
		{
			this.isAnimeFinished = false;
			this._isPlaying = false;
			this._actCallback = null;
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
				Util.FindParentToChild<UITexture>(ref this._uiAircraft[i], this._uiAirObj[i].get_transform(), "Swing/Aircraft");
				this._airAnimation[i] = this._uiAircraft[i].GetComponent<Animation>();
			}
			if (this._uiShipObj == null)
			{
				this._uiShipObj = base.get_transform().FindChild("ShipObj").get_gameObject();
			}
			Util.FindParentToChild<UITexture>(ref this._uiShip, this._uiShipObj.get_transform(), "Ship");
			Util.FindParentToChild<ParticleSystem>(ref this._cloudParticle, base.get_transform(), "Cloud");
			this._camFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			this._camFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			return true;
		}

		private void OnDestroy()
		{
			this.isAnimeFinished = false;
			this._isPlaying = false;
			if (this._prodAntiAerialCutIn != null)
			{
				Object.Destroy(this._prodAntiAerialCutIn.get_gameObject());
			}
			this._prodAntiAerialCutIn = null;
			Mem.DelArySafe<GameObject>(ref this._uiAirObj);
			Mem.Del<GameObject>(ref this._uiShipObj);
			Mem.Del(ref this._cloudParticle);
			Mem.DelArySafe<UITexture>(ref this._uiAircraft);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.DelArySafe<Animation>(ref this._airAnimation);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<CutInType>(ref this._iType);
			Mem.Del<KoukuuModel>(ref this._koukuuModel);
			Mem.Del<BattleFieldCamera>(ref this._camFieldCamera);
		}

		public static ProdAerialCombatCutinP Instantiate(ProdAerialCombatCutinP prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialCombatCutinP prodAerialCombatCutinP = Object.Instantiate<ProdAerialCombatCutinP>(prefab);
			prodAerialCombatCutinP.get_transform().set_parent(parent);
			prodAerialCombatCutinP.get_transform().set_localPosition(Vector3.get_zero());
			prodAerialCombatCutinP.get_transform().set_localScale(Vector3.get_one());
			prodAerialCombatCutinP._koukuuModel = model;
			prodAerialCombatCutinP._init();
			return prodAerialCombatCutinP;
		}

		private void _setShipTexture(bool isFriend)
		{
			ShipModel_Attacker shipModel_Attacker = (this._koukuuModel.GetCaptainShip(isFriend) == null) ? null : this._koukuuModel.GetCaptainShip(isFriend);
			if (shipModel_Attacker != null)
			{
				this._uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(shipModel_Attacker);
				this._uiShip.MakePixelPerfect();
				this._uiShip.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
				Vector3 shipOffsPos = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(shipModel_Attacker, shipModel_Attacker.DamagedFlg, MstShipGraphColumn.CutInSp1);
				this._uiShip.get_transform().set_localPosition((!isFriend) ? new Vector3(shipOffsPos.x * -1f, shipOffsPos.y, shipOffsPos.z) : shipOffsPos);
			}
			else
			{
				this._uiShip.mainTexture = null;
			}
		}

		public void Play(Action callback)
		{
			this._isPlaying = true;
			this._actCallback = callback;
			base.GetComponent<UIPanel>().widgetsAreStatic = false;
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
			this._uiShip.mainTexture = null;
			for (int i = 0; i < 3; i++)
			{
				this._uiAircraft[i].mainTexture = null;
			}
			if (type == CutInType.Both || type == CutInType.FriendOnly)
			{
				this._setShipTexture(true);
				this._setAircraftTexture(FleetType.Friend);
			}
			else if (type == CutInType.EnemyOnly)
			{
				this._setShipTexture(false);
				this._setAircraftTexture(FleetType.Enemy);
				this._changeSeaWave(false);
				base.get_transform().set_localEulerAngles(new Vector3(0f, -180f, 0f));
				this.isAnimeFinished = true;
			}
			for (int j = 0; j < 3; j++)
			{
				this._airAnimation[j].Stop();
				this._airAnimation[j].Play("AircraftCutin" + (j + 1));
			}
			Animation component = base.get_transform().GetComponent<Animation>();
			component.Stop();
			component.Play("AircraftCutinP_1");
		}

		private void _setAircraftTexture(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				this._setAircraftTexture(true);
			}
			else if (type == FleetType.Enemy)
			{
				this._setAircraftTexture(false);
			}
		}

		private void _setAircraftTexture(bool isFriend)
		{
			if (this._koukuuModel.GetCaptainShip(isFriend) != null)
			{
				PlaneModelBase[] plane = this._koukuuModel.GetPlane(this._koukuuModel.GetCaptainShip(isFriend).TmpId);
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
							if (!isFriend && BattleTaskManager.GetBattleManager() is PracticeBattleManager)
							{
								this._uiAirObj[i].get_transform().set_localPosition(new Vector3(267f, 176f, 0f));
								this._uiAirObj[i].get_transform().set_localScale(Vector3.get_one());
								this._uiAircraft[i].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6);
								this._uiAirObj[i].get_transform().set_localEulerAngles(new Vector3(0f, 0f, -25.5f));
								this._uiAircraft[i].flip = UIBasicSprite.Flip.Nothing;
							}
							else
							{
								this._uiAirObj[i].get_transform().set_localPosition(new Vector3(267f, 176f, 0f));
								this._uiAircraft[i].mainTexture = ((!isFriend) ? KCV.Battle.Utils.SlotItemUtils.LoadTexture(plane[i]) : SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6));
								this._uiAirObj[i].get_transform().set_localEulerAngles((!isFriend) ? Vector3.get_zero() : new Vector3(0f, 0f, -25.5f));
								if (!isFriend)
								{
									this._uiAircraft[i].MakePixelPerfect();
									this._uiAirObj[i].get_transform().set_localScale((plane[i].MstId < 500) ? Vector3.get_one() : new Vector3(0.8f, 0.8f, 0.8f));
									AircraftOffsetInfo aircraftOffsetInfo = KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[i].MstId);
									this._uiAircraft[i].flip = ((!aircraftOffsetInfo.isFlipHorizontal) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
								}
							}
						}
						else
						{
							this._uiAircraft[i].mainTexture = null;
						}
					}
				}
			}
		}

		private void _changeSeaWave(bool isFriend)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.seaLevel.set_waveSpeed((!isFriend) ? new Vector4(-4f, 500f, 5f, 400f) : new Vector4(-4f, -500f, 5f, -400f));
			this._camFieldCamera.get_transform().set_localPosition((!isFriend) ? new Vector3(300f, 4f, 100f) : new Vector3(300f, 4f, 100f));
			this._camFieldCamera.get_transform().set_rotation((!isFriend) ? Quaternion.Euler(new Vector3(22f, 160f, 0f)) : Quaternion.Euler(new Vector3(22f, 20f, 0f)));
		}

		private void _startAerialCombatCutIn()
		{
			this._playSE(2);
			if (this._iType == CutInType.Both || this._iType == CutInType.FriendOnly)
			{
				KCV.Battle.Utils.ShipUtils.PlayAircraftCutInVoice(this._koukuuModel.GetCaptainShip(true));
				this._changeSeaWave(true);
			}
			else if (this._iType == CutInType.EnemyOnly)
			{
				this._changeSeaWave(false);
			}
		}

		public void _playCloudParticle()
		{
			this._cloudParticle.Play();
			BattleTaskManager.GetBattleField().ResetFleetAnchorPosition();
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
			if (id == 1)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_048);
			}
			else if (id == 2)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
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
				if (!this.isAnimeFinished)
				{
					this._setCutin(CutInType.EnemyOnly);
					Animation component = base.get_transform().GetComponent<Animation>();
					component.Stop();
					component.Play("AircraftCutinP_2");
					this._playSE(2);
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
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				this._prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), this._koukuuModel, cutInEffectCamera.get_transform());
				this._prodAntiAerialCutIn.Play(new Action(this._compAntiAerialCutInEnemy), true);
			}
			else if (this._koukuuModel.GetTaikuShip(true) != null)
			{
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
					this._prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), this._koukuuModel, BattleTaskManager.GetBattleCameras().cutInEffectCamera.get_transform());
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
