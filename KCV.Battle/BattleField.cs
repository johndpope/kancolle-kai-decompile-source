using KCV.Battle.Utils;
using KCV.Generic;
using Librarys.UnitySettings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleField : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> _traSeaLevelList;

		[SerializeField]
		private Transform _traFieldCenter;

		[SerializeField]
		private List<Transform> _traFleetAnchorList;

		[SerializeField]
		private List<Material> _listFriendSeaLevels;

		[SerializeField]
		private List<Material> _listEnemySeaLevels;

		[SerializeField]
		private List<Material> _listMatSkyboxes;

		[SerializeField]
		private List<Texture2D> _listTexReflectives;

		private TimeZone _iTimeZone;

		private Dictionary<FleetType, Transform> _dicFleetAnchor;

		private Dictionary<FleetType, Vector3> _dicFleetAnchorOrigine;

		private Dictionary<FleetType, Vector4> _dicWaveDirs;

		private Dictionary<FleetType, ParticleSystem> _dicPSClouds;

		private Dictionary<FleetType, Water> _dicSeaLevels;

		private Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>> _dicCameraAnchors;

		public Transform fieldCenter
		{
			get
			{
				return this._traFieldCenter;
			}
		}

		public Dictionary<FleetType, Transform> dicFleetAnchor
		{
			get
			{
				return this._dicFleetAnchor;
			}
		}

		public TimeZone timeZoon
		{
			get
			{
				return this._iTimeZone;
			}
		}

		public Water seaLevel
		{
			get
			{
				return this._dicSeaLevels.get_Item(FleetType.Friend);
			}
			set
			{
				this._dicSeaLevels.set_Item(FleetType.Friend, value);
			}
		}

		public Water enemySeaLevel
		{
			get
			{
				return this._dicSeaLevels.get_Item(FleetType.Enemy);
			}
			set
			{
				this._dicSeaLevels.set_Item(FleetType.Enemy, value);
			}
		}

		public Dictionary<FleetType, Water> seaLevels
		{
			get
			{
				return this._dicSeaLevels;
			}
		}

		public Vector3 seaLevelPos
		{
			get
			{
				return this._dicSeaLevels.get_Item(FleetType.Friend).get_transform().get_position();
			}
		}

		public bool isEnemySeaLevelActive
		{
			get
			{
				return this._dicSeaLevels.get_Item(FleetType.Enemy).get_gameObject().get_activeInHierarchy();
			}
			set
			{
				this._dicSeaLevels.get_Item(FleetType.Enemy).SetActive(value);
			}
		}

		public Dictionary<FleetType, ParticleSystem> dicParticleClouds
		{
			get
			{
				return this._dicPSClouds;
			}
		}

		public Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>> dicCameraAnchors
		{
			get
			{
				return this._dicCameraAnchors;
			}
		}

		public Dictionary<FleetType, Vector3> fleetAnchorOrigine
		{
			get
			{
				return this._dicFleetAnchorOrigine;
			}
		}

		private void Awake()
		{
			this._iTimeZone = TimeZone.DayTime;
			if (this._traFieldCenter == null)
			{
				Util.FindParentToChild<Transform>(ref this._traFieldCenter, base.get_transform(), "CenterAnchor");
			}
			this._dicFleetAnchor = new Dictionary<FleetType, Transform>();
			int num = 0;
			using (List<Transform>.Enumerator enumerator = this._traFleetAnchorList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform current = enumerator.get_Current();
					this._dicFleetAnchor.Add((FleetType)num, current);
					num++;
				}
			}
			this._dicCameraAnchors = new Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>>();
			Dictionary<FleetType, Transform> dictionary = new Dictionary<FleetType, Transform>();
			using (IEnumerator enumerator2 = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator2.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						dictionary.Add(fleetType, base.get_transform().FindChild(string.Format("CameraAnchors/{0}OneRowAnchor", fleetType.ToString())).get_transform());
					}
				}
			}
			this._dicCameraAnchors.Add(CameraAnchorType.OneRowAnchor, dictionary);
			this._dicFleetAnchorOrigine = new Dictionary<FleetType, Vector3>();
			this._dicFleetAnchorOrigine.Add(FleetType.Friend, this._dicFleetAnchor.get_Item(FleetType.Friend).get_transform().get_position());
			this._dicFleetAnchorOrigine.Add(FleetType.Enemy, this._dicFleetAnchor.get_Item(FleetType.Enemy).get_transform().get_position());
			this._dicSeaLevels = new Dictionary<FleetType, Water>();
			int num2 = 0;
			using (List<Transform>.Enumerator enumerator3 = this._traSeaLevelList.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Transform current2 = enumerator3.get_Current();
					this._dicSeaLevels.Add((FleetType)num2, current2.GetComponent<Water>());
					this._dicSeaLevels.get_Item((FleetType)num2).m_WaterMode = 1;
					this._dicSeaLevels.get_Item((FleetType)num2).set_waveScale(0.02f);
					this._dicSeaLevels.get_Item((FleetType)num2).set_reflectionDistort(1.5f);
					num2++;
				}
			}
			this._dicSeaLevels.get_Item(FleetType.Enemy).SetLayer(Generics.Layers.SplitWater.IntLayer());
			this.isEnemySeaLevelActive = false;
			this._dicWaveDirs = new Dictionary<FleetType, Vector4>();
			this._dicWaveDirs.Add(FleetType.Friend, new Vector4(-3.58f, -22.85f, 1f, -100f));
			this._dicWaveDirs.Add(FleetType.Enemy, new Vector4(3.58f, 22.85f, -1f, 100f));
			this._dicPSClouds = new Dictionary<FleetType, ParticleSystem>();
			using (IEnumerator enumerator4 = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					FleetType fleetType2 = (FleetType)((int)enumerator4.get_Current());
					if (fleetType2 != FleetType.CombinedFleet)
					{
						ParticleSystem particleSystem = ParticleFile.Instantiate<ParticleSystem>(ParticleFileInfos.BattleAdventFleetCloud);
						particleSystem.set_name(string.Format("Cloud{0}", fleetType2));
						particleSystem.get_transform().set_parent(base.get_transform());
						particleSystem.SetRenderQueue(3500);
						particleSystem.get_transform().set_localScale(Vector3.get_one());
						particleSystem.get_transform().set_position(Vector3.get_zero());
						particleSystem.set_playOnAwake(false);
						particleSystem.SetActive(false);
						this._dicPSClouds.Add(fleetType2, particleSystem);
					}
				}
			}
		}

		private void OnDestroy()
		{
			Transform meshTrans = null;
			if (this._traSeaLevelList != null)
			{
				this._traSeaLevelList.ForEach(delegate(Transform x)
				{
					meshTrans = x;
					Mem.DelMeshSafe(ref meshTrans);
				});
			}
			Mem.DelListSafe<Transform>(ref this._traSeaLevelList);
			Mem.Del<Transform>(ref this._traFieldCenter);
			Mem.DelListSafe<Transform>(ref this._traFleetAnchorList);
			Mem.DelListSafe<Material>(ref this._listFriendSeaLevels);
			Mem.DelListSafe<Material>(ref this._listEnemySeaLevels);
			Mem.DelListSafe<Material>(ref this._listMatSkyboxes);
			Mem.DelListSafe<Texture2D>(ref this._listTexReflectives);
			Mem.Del<TimeZone>(ref this._iTimeZone);
			Mem.DelDictionarySafe<FleetType, Transform>(ref this._dicFleetAnchor);
			Mem.DelDictionarySafe<FleetType, Vector3>(ref this._dicFleetAnchorOrigine);
			Mem.DelDictionarySafe<FleetType, Vector4>(ref this._dicWaveDirs);
			Mem.DelDictionarySafe<FleetType, ParticleSystem>(ref this._dicPSClouds);
			Mem.DelDictionarySafe<FleetType, Water>(ref this._dicSeaLevels);
			if (this._dicCameraAnchors != null)
			{
				this._dicCameraAnchors.ForEach(delegate(KeyValuePair<CameraAnchorType, Dictionary<FleetType, Transform>> x)
				{
					x.get_Value().Clear();
				});
			}
			Mem.DelDictionarySafe<CameraAnchorType, Dictionary<FleetType, Transform>>(ref this._dicCameraAnchors);
			Mem.Del<Transform>(ref meshTrans);
		}

		public void ReqTimeZone(TimeZone iTime, SkyType iSkyType)
		{
			this._fogSettings(iTime);
			Color seaColor = this.GetSeaColor(iTime, iSkyType);
			using (Dictionary<FleetType, Water>.Enumerator enumerator = this._dicSeaLevels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<FleetType, Water> current = enumerator.get_Current();
					current.get_Value().set_reflectionColorTexture(this._listTexReflectives.get_Item((int)iTime));
					current.get_Value().GetComponent<MeshRenderer>().set_material((current.get_Key() != FleetType.Friend) ? this._listEnemySeaLevels.get_Item((int)iTime) : this._listFriendSeaLevels.get_Item((int)iTime));
					current.get_Value().GetComponent<MeshRenderer>().get_material().SetColor("_PostMultiplyColor", seaColor);
				}
			}
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.fieldCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.skybox.set_material(this._listMatSkyboxes.get_Item((int)iTime));
			});
			battleCameras.fieldDimCamera.skybox.set_material(this._listMatSkyboxes.get_Item((int)iTime));
			this._iTimeZone = iTime;
		}

		public void AlterWaveDirection(FleetType iType)
		{
			this._dicSeaLevels.get_Item(FleetType.Friend).set_waveSpeed(this._dicWaveDirs.get_Item(iType));
		}

		public void AlterWaveDirection(FleetType iFleetType, FleetType iWaveType)
		{
			this._dicSeaLevels.get_Item(iFleetType).set_waveSpeed(this._dicWaveDirs.get_Item(iWaveType));
		}

		public void ResetFleetAnchorPosition()
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						this._dicFleetAnchor.get_Item(fleetType).get_transform().set_position(this._dicFleetAnchorOrigine.get_Item(fleetType));
						this._dicFleetAnchor.get_Item(fleetType).get_transform().set_rotation(new Quaternion(0f, 0f, 0f, 0f));
						this._dicFleetAnchor.get_Item(fleetType).get_transform().localScaleOne();
					}
				}
			}
		}

		private void _fogSettings(TimeZone iTime)
		{
			Fog.fog = true;
			Fog.fogMode = 1;
			Fog.fogDensity = 0.14f;
			Fog.fogStartDistance = 20f;
			Fog.fogEndDistance = 130f;
			Color fogColor = (iTime != TimeZone.DayTime) ? new Color(Mathe.Rate(0f, 255f, 65f), Mathe.Rate(0f, 255f, 129f), Mathe.Rate(0f, 255f, 161f), Mathe.Rate(0f, 255f, 255f)) : new Color(Mathe.Rate(0f, 255f, 187f), Mathe.Rate(0f, 255f, 229f), Mathe.Rate(0f, 255f, 240f), Mathe.Rate(0f, 255f, 255f));
			Fog.fogColor = fogColor;
		}

		private Color GetSeaColor(TimeZone iTime, SkyType iSkyType)
		{
			Color result = Color.get_white();
			if (iSkyType == SkyType.Normal)
			{
				result = KCVColor.ConvertColor(90f, 173f, 177f, 255f);
			}
			else
			{
				int length = Enum.GetValues(typeof(SkyType)).get_Length();
				SkyType skyType;
				switch (iSkyType)
				{
				case SkyType.FinalArea171:
					skyType = SkyType.FinalArea172;
					break;
				case SkyType.FinalArea172:
					skyType = SkyType.FinalArea173;
					break;
				case SkyType.FinalArea173:
					skyType = SkyType.FinalArea174;
					break;
				case SkyType.FinalArea174:
					skyType = SkyType.FinalArea174;
					break;
				default:
					skyType = SkyType.FinalArea174;
					break;
				}
				float t = (float)skyType / (float)(length - 1);
				result = KCVColor.ConvertColor(Mathe.Lerp(90f, 255f, t), Mathe.Lerp(173f, 68f, t), Mathe.Lerp(177f, 68f, t), 255f);
			}
			return result;
		}

		private Material GetSkyboxMaterial(TimeZone iTime, SkyType iSkyType)
		{
			Material result;
			if (iSkyType == SkyType.Normal)
			{
				result = ((iTime != TimeZone.DayTime) ? this._listMatSkyboxes.get_Item(1) : this._listMatSkyboxes.get_Item(0));
			}
			else
			{
				result = ((iSkyType != SkyType.FinalArea171 && iSkyType != SkyType.FinalArea172) ? this._listMatSkyboxes.get_Item(3) : this._listMatSkyboxes.get_Item(2));
			}
			return result;
		}
	}
}
