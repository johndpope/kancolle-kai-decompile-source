using KCV.Battle.Utils;
using Librarys.InspectorExtension;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleShip : MonoBehaviour
	{
		public enum AnimationName
		{
			ProdShellingNormalAttack,
			ProdTranscendenceAttack
		}

		[Serializable]
		private class Wakes : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private ParticleSystem _psShipSpray;

			[SerializeField]
			private ParticleSystem _psSinkSpray;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public ParticleSystem shipSpray
			{
				get
				{
					return this._psShipSpray;
				}
			}

			public ParticleSystem sinkSpray
			{
				get
				{
					return this._psSinkSpray;
				}
			}

			public Wakes(Transform transform)
			{
				if (this._tra == null)
				{
					this._tra = transform;
				}
				if (this._psShipSpray == null)
				{
					Util.FindParentToChild<ParticleSystem>(ref this._psShipSpray, this.transform, "ShipSpray");
				}
				if (this._psSinkSpray == null)
				{
					Util.FindParentToChild<ParticleSystem>(ref this._psSinkSpray, this.transform, "SinkSpray");
				}
			}

			public void Dispose()
			{
				Transform transform = this._psShipSpray.get_transform();
				Mem.DelMeshSafe(ref transform);
				transform = this._psShipSpray.get_transform();
				Mem.DelMeshSafe(ref transform);
				using (IEnumerator enumerator = this._psSinkSpray.get_transform().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Transform transform2 = (Transform)enumerator.get_Current();
						transform = transform2;
						Mem.DelMeshSafe(ref transform);
					}
				}
				Mem.Del<Transform>(ref this._tra);
				Mem.Del(ref this._psShipSpray);
				Mem.Del(ref this._psSinkSpray);
				Mem.Del<Transform>(ref transform);
			}
		}

		[SerializeField]
		private BillboardObject _uiBillboard;

		[SerializeField]
		private Transform _traPog;

		[SerializeField]
		private Transform _traShipAnchor;

		[SerializeField]
		private Transform _traTorpedoAnchor;

		[SerializeField]
		private Transform _traSPPog;

		[SerializeField]
		private Animation _animShipAnimation;

		[SerializeField]
		private ShipModel_Battle _clsShip;

		[SerializeField]
		private Object3D _clsObject3D;

		[SerializeField]
		private UIBattleShip.Wakes _clsWakes;

		private FleetType _iFleetType;

		private StandingPositionType _iStandingPositionType;

		private Dictionary<StandingPositionType, Vector3> _dicStandingPos;

		private ShipDrawType _iShipDrawType;

		public ShipModel_Battle shipModel
		{
			get
			{
				if (this._clsShip != null)
				{
					return this._clsShip;
				}
				return null;
			}
		}

		public Vector3 pointOfGaze
		{
			get
			{
				return this._traPog.get_position();
			}
		}

		public Vector3 localPointOfGaze
		{
			get
			{
				return this._traPog.get_localPosition();
			}
			set
			{
				if (this._traPog.get_localPosition() != value)
				{
					this._traPog.set_localPosition(value);
				}
			}
		}

		public Vector3 difPointOfGazeFmFleet
		{
			get
			{
				return this.pointOfGaze - base.get_transform().get_position();
			}
		}

		public Vector3 spPointOfGaze
		{
			get
			{
				return this._traSPPog.get_position();
			}
		}

		public Vector3 localSPPointOfGaze
		{
			get
			{
				return this._traSPPog.get_localPosition();
			}
			set
			{
				if (this._traSPPog.get_localPosition() != value)
				{
					this._traSPPog.set_localPosition(value);
				}
			}
		}

		public Vector3 difSPPointOfGazeFmFleet
		{
			get
			{
				return this.spPointOfGaze - base.get_transform().get_position();
			}
		}

		public Vector3 torpedoAnchor
		{
			get
			{
				return this._traTorpedoAnchor.get_position();
			}
		}

		public BillboardObject billboard
		{
			get
			{
				return this._uiBillboard;
			}
		}

		public FleetType fleetType
		{
			get
			{
				return this._iFleetType;
			}
		}

		public Object3D object3D
		{
			get
			{
				return this._clsObject3D;
			}
		}

		public Dictionary<StandingPositionType, Vector3> dicStandingPos
		{
			get
			{
				return this._dicStandingPos;
			}
		}

		public Generics.Layers layer
		{
			get
			{
				if (this.object3D == null)
				{
					return Generics.Layers.Nothing;
				}
				return (Generics.Layers)this.object3D.get_gameObject().get_layer();
			}
			set
			{
				if (base.get_gameObject() != null)
				{
					this.object3D.SetLayer(value.IntLayer());
				}
			}
		}

		public StandingPositionType standingPositionType
		{
			get
			{
				return this._iStandingPositionType;
			}
			set
			{
				if (this._dicStandingPos.ContainsKey(value) && base.get_transform().get_localPosition() != this._dicStandingPos.get_Item(value))
				{
					this._iStandingPositionType = value;
					base.get_transform().set_localPosition(this._dicStandingPos.get_Item(value));
				}
			}
		}

		public ShipDrawType drawType
		{
			get
			{
				return this._iShipDrawType;
			}
			set
			{
				if (this._iShipDrawType != value)
				{
					if (value != ShipDrawType.Normal)
					{
						if (value == ShipDrawType.Silhouette)
						{
							this.object3D.color = new Color(Mathe.Rate(0f, 255f, 20f), Mathe.Rate(0f, 255f, 20f), Mathe.Rate(0f, 255f, 20f), 1f);
						}
					}
					else
					{
						this.object3D.color = Color.get_white();
					}
					this._iShipDrawType = value;
				}
			}
		}

		public static UIBattleShip Instantiate(UIBattleShip prefab, Transform parent)
		{
			UIBattleShip uIBattleShip = Object.Instantiate<UIBattleShip>(prefab);
			uIBattleShip.get_transform().set_parent(parent);
			uIBattleShip.Init();
			return uIBattleShip;
		}

		private void OnDestroy()
		{
			Mem.Del<BillboardObject>(ref this._uiBillboard);
			Mem.Del<Transform>(ref this._traPog);
			Mem.Del<Transform>(ref this._traShipAnchor);
			Mem.Del<Transform>(ref this._traTorpedoAnchor);
			Mem.Del<Transform>(ref this._traSPPog);
			Mem.Del<Animation>(ref this._animShipAnimation);
			Mem.Del<ShipModel_Battle>(ref this._clsShip);
			Mem.Del<Object3D>(ref this._clsObject3D);
			Mem.DelIDisposableSafe<UIBattleShip.Wakes>(ref this._clsWakes);
			Mem.Del<FleetType>(ref this._iFleetType);
			Mem.Del<StandingPositionType>(ref this._iStandingPositionType);
			Mem.DelDictionarySafe<StandingPositionType, Vector3>(ref this._dicStandingPos);
			Mem.Del<ShipDrawType>(ref this._iShipDrawType);
		}

		private bool Init()
		{
			if (this._uiBillboard == null)
			{
				this._uiBillboard = base.GetComponent<BillboardObject>();
			}
			this._uiBillboard.isBillboard = true;
			if (this._traPog == null)
			{
				Util.FindParentToChild<Transform>(ref this._traPog, base.get_transform(), "POG");
			}
			if (this._traSPPog == null)
			{
				Util.FindParentToChild<Transform>(ref this._traSPPog, base.get_transform(), "SPPog");
			}
			if (this._traTorpedoAnchor == null)
			{
				Util.FindParentToChild<Transform>(ref this._traTorpedoAnchor, base.get_transform(), "TorpedoAnchor");
			}
			if (this._clsObject3D == null)
			{
				Util.FindParentToChild<Object3D>(ref this._clsObject3D, base.get_transform(), "ShipAnchor/Object3D");
			}
			if (this._animShipAnimation == null)
			{
				this._animShipAnimation = base.GetComponent<Animation>();
			}
			this._animShipAnimation.set_playAutomatically(false);
			this._animShipAnimation.Stop();
			this._iStandingPositionType = StandingPositionType.Free;
			this._dicStandingPos = new Dictionary<StandingPositionType, Vector3>();
			this._iShipDrawType = ShipDrawType.Normal;
			return true;
		}

		public void SetStandingPosition(StandingPositionType iType)
		{
			if (this._dicStandingPos.ContainsKey(iType) && base.get_transform().get_localPosition() != this._dicStandingPos.get_Item(iType))
			{
				this._iStandingPositionType = iType;
				base.get_transform().set_localPosition(this._dicStandingPos.get_Item(iType));
			}
		}

		public void TorpedoSalvoWakeAngle(bool isSet)
		{
			if (this._clsShip == null)
			{
				return;
			}
			Vector3 localEulerAngles = (!isSet) ? Vector3.get_zero() : ((!this._clsShip.IsFriend()) ? BattleDefines.BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE.get_Item(1) : BattleDefines.BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE.get_Item(0));
			this._clsWakes.transform.set_localEulerAngles(localEulerAngles);
		}

		public void UpdateDamage()
		{
		}

		public void UpdateDamage(ShipModel_Defender model)
		{
			this.SetShipTexture(model, true);
			this.SetPointOfGaze(model, true);
			this.SetSPPointOfGaze(model, true);
		}

		public void PlayShipAnimation(UIBattleShip.AnimationName iName)
		{
			this._animShipAnimation.Play(iName.ToString());
		}

		public void PlayProtectAnimation()
		{
			Material material = new Material(Resources.Load("Textures/battle/Torpedo/ProtectShip") as Material);
			this._clsObject3D.material = material;
			this._clsObject3D.meshRenderer.set_sharedMaterial(material);
			this._clsObject3D.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f));
			base.get_transform().LTValue(0.5f, 1f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._clsObject3D.material.SetColor("_TintColor", new Color(x, x, x));
			}).setOnComplete(new Action(this._protectAfterAnimation));
		}

		private void _protectAfterAnimation()
		{
			base.get_transform().LTValue(1f, 0.5f, 1f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				this._clsObject3D.material.SetColor("_TintColor", new Color(x, x, x));
			}).setOnComplete(new Action(this._endProtectAnimation));
		}

		private void _endProtectAnimation()
		{
			this._clsObject3D.material = (Resources.Load("Materials/Battle/Ship") as Material);
			this._clsObject3D.color = new Color(1f, 1f, 1f);
		}

		public void PlayProdSinking(Action callback)
		{
			this._clsWakes.shipSpray.SetActive(false);
			this._clsWakes.shipSpray.SetActive(true);
			this._clsWakes.shipSpray.Play();
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
			{
				this.object3D.color = Color.get_gray();
				Vector3 localPosition = this.get_transform().get_localPosition();
				localPosition.y = -5f;
				this.get_transform().LTMoveLocal(localPosition, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(2)).setEase(LeanTweenType.easeInQuad).setOnComplete(delegate
				{
					Dlg.Call(ref callback);
					this.get_transform().SetActive(false);
				});
				this.object3D.get_transform().LTRotateAround(Vector3.get_back(), 30f, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME.get_Item(2)).setEase(LeanTweenType.easeInQuad);
			});
		}

		public void Restored(ShipModel_Defender defender)
		{
			this.object3D.get_transform().LTCancel();
			this.SetShipTextureRestore(defender);
			this.SetPointOfGaze2Restore(defender);
			this.SetSPPointOfGaze2Restore(defender);
			this._clsWakes.shipSpray.SetActive(true);
			this._clsWakes.shipSpray.Play();
			this._clsWakes.sinkSpray.SetActive(false);
			this.object3D.color = Color.get_white();
			this.standingPositionType = StandingPositionType.OneRow;
			this.object3D.get_transform().set_localRotation(Quaternion.Euler(Vector3.get_zero()));
		}

		public void SetShipInfos(ShipModel_BattleAll model, bool isStart)
		{
			if (model == null)
			{
				this._clsShip = model;
				this.SetActive(false);
				return;
			}
			this._clsShip = model;
			this._iFleetType = ((!model.IsFriend()) ? FleetType.Enemy : FleetType.Friend);
			this.SetShipTexture(model, isStart);
			this.SetPointOfGaze(model, isStart);
			this.SetSPPointOfGaze(model, isStart);
			base.set_name(model.Name);
		}

		private void SetShipTexture(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), isDamaged);
			if (this._clsObject3D.mainTexture != null && this._clsObject3D.mainTexture.get_name() == shipStandingTexID.ToString())
			{
				return;
			}
			this._clsObject3D.mainTexture = ShipUtils.LoadTexture(model, isStart);
			this._clsObject3D.MakePixelPerfect();
			this._clsObject3D.get_transform().set_localScale(this._clsObject3D.get_transform().get_localScale() * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgStart));
			this._clsObject3D.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.Foot));
		}

		private void SetShipTexture(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), isDamaged);
			if (this._clsObject3D.mainTexture != null && this._clsObject3D.mainTexture.get_name() == shipStandingTexID.ToString())
			{
				return;
			}
			this._clsObject3D.mainTexture = ShipUtils.LoadTexture(model, isAfter);
			this._clsObject3D.MakePixelPerfect();
			this._clsObject3D.get_transform().set_localScale(this._clsObject3D.get_transform().get_localScale() * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgAfter));
			this._clsObject3D.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.Foot));
		}

		private void SetShipTextureRestore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), damagedFlgAfterRecovery);
			if (this._clsObject3D.mainTexture != null && this._clsObject3D.mainTexture.get_name() == shipStandingTexID.ToString())
			{
				return;
			}
			this._clsObject3D.mainTexture = ShipUtils.LoadTexture2Restore(model);
			this._clsObject3D.MakePixelPerfect();
			this._clsObject3D.get_transform().set_localScale(this._clsObject3D.get_transform().get_localScale() * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgAfter));
			this._clsObject3D.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.Foot));
		}

		private void SetPointOfGaze(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			this._traPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.PointOfGaze));
		}

		private void SetPointOfGaze(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			this._traPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.PointOfGaze));
		}

		private void SetPointOfGaze2Restore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			this._traPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.PointOfGaze));
		}

		public void SetSPPointOfGaze(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			this._traSPPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.SPPointOfGaze));
		}

		public void SetSPPointOfGaze(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			this._traSPPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.SPPointOfGaze));
		}

		public void SetSPPointOfGaze2Restore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			this._traSPPog.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.SPPointOfGaze));
		}

		public void SetSprayColor()
		{
			ParticleSystem component = base.get_transform().FindChild("ShipSpray").GetComponent<ParticleSystem>();
			if (BattleTaskManager.GetTimeZone() == TimeZone.DayTime)
			{
				component.GetComponent<Renderer>().get_material().set_shader(SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(5));
			}
			else if (BattleTaskManager.GetTimeZone() == TimeZone.Night)
			{
				component.GetComponent<Renderer>().get_material().set_shader(SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(6));
			}
		}
	}
}
