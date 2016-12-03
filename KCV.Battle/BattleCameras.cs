using KCV.Battle.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleCameras : IDisposable
	{
		private BattleCutInCamera _camCutInCamera;

		private BattleCutInEffectCamera _camCutInEffectCamera;

		private List<BattleFieldCamera> _listCameras;

		private BattleFieldDimCamera _camFieldDimCamera;

		private bool _isSplit;

		private bool _isSplit2d;

		public BattleCutInCamera cutInCamera
		{
			get
			{
				return this._camCutInCamera;
			}
		}

		public BattleCutInEffectCamera cutInEffectCamera
		{
			get
			{
				return this._camCutInEffectCamera;
			}
		}

		public List<BattleFieldCamera> fieldCameras
		{
			get
			{
				return this._listCameras;
			}
		}

		public BattleFieldCamera friendFieldCamera
		{
			get
			{
				return this.fieldCameras.get_Item(0);
			}
		}

		public BattleFieldCamera enemyFieldCamera
		{
			get
			{
				return this.fieldCameras.get_Item(1);
			}
		}

		public BattleFieldDimCamera fieldDimCamera
		{
			get
			{
				return this._camFieldDimCamera;
			}
		}

		public bool isSplit
		{
			get
			{
				return this._isSplit;
			}
			set
			{
				this.SetSplitCameras(value);
			}
		}

		public bool isSplit2D
		{
			get
			{
				return this._isSplit2d;
			}
			set
			{
				this.SetSplitCameras2D(value);
			}
		}

		public bool isFieldDimCameraEnabled
		{
			get
			{
				return this._camFieldDimCamera.get_enabled();
			}
			set
			{
				if (!value)
				{
					this._camFieldDimCamera.cullingMask = Generics.Layers.Nothing;
					this._camFieldDimCamera.isCulling = false;
					this._camFieldDimCamera.isSync = false;
					this._camFieldDimCamera.maskAlpha = 0f;
				}
				this._camFieldDimCamera.set_enabled(value);
			}
		}

		public BattleCameras()
		{
			this._camCutInCamera = GameObject.Find("UIRoot/CutInCamera").GetComponent<BattleCutInCamera>();
			this._camCutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			this._camCutInCamera.depth = 6f;
			this._camCutInCamera.clearFlags = 3;
			this._camCutInEffectCamera = GameObject.Find("UIRoot/CutInEffectCamera").GetComponent<BattleCutInEffectCamera>();
			this._camCutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			this._camCutInEffectCamera.isCulling = false;
			this._camCutInEffectCamera.depth = 5f;
			this._camCutInEffectCamera.clearFlags = 3;
			this._listCameras = new List<BattleFieldCamera>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(FleetType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FleetType fleetType = (FleetType)((int)enumerator.get_Current());
					if (fleetType != FleetType.CombinedFleet)
					{
						BattleFieldCamera battleFieldCamera = (!GameObject.Find(string.Format("Stage/{0}FieldCamera", fleetType))) ? null : GameObject.Find(string.Format("Stage/{0}FieldCamera", fleetType)).GetComponent<BattleFieldCamera>();
						this._listCameras.Add(battleFieldCamera);
						if (this._listCameras.get_Item((int)fleetType) != null)
						{
							this._listCameras.get_Item((int)fleetType).cullingMask = this.GetDefaultLayers();
							this._listCameras.get_Item((int)fleetType).ResetMotionBlur();
							this._listCameras.get_Item((int)fleetType).depth = 0f;
						}
					}
				}
			}
			this._camFieldDimCamera = BattleFieldDimCamera.Instantiate(BattleTaskManager.GetPrefabFile().prefabFieldDimCamera.GetComponent<BattleFieldDimCamera>(), BattleTaskManager.GetStage());
			this._camFieldDimCamera.syncTarget = this._listCameras.get_Item(0).get_transform();
			this._camFieldDimCamera.cullingMask = this.GetDefaultDimLayers();
			this._camFieldDimCamera.isCulling = false;
			this._camFieldDimCamera.depth = -1f;
		}

		public bool Init()
		{
			return true;
		}

		public void Dispose()
		{
			Mem.Del<BattleCutInCamera>(ref this._camCutInCamera);
			Mem.Del<BattleCutInEffectCamera>(ref this._camCutInEffectCamera);
			Mem.DelListSafe<BattleFieldCamera>(ref this._listCameras);
			Mem.Del<BattleFieldDimCamera>(ref this._camFieldDimCamera);
			Mem.Del<bool>(ref this._isSplit);
			Mem.Del<bool>(ref this._isSplit2d);
		}

		public bool ReleaseBeforeResult()
		{
			if (this._listCameras != null)
			{
				this._listCameras.ForEach(delegate(BattleFieldCamera x)
				{
					if (x.get_gameObject() != null)
					{
						Object.Destroy(x.get_gameObject());
					}
				});
				Mem.DelListSafe<BattleFieldCamera>(ref this._listCameras);
			}
			Mem.DelComponentSafe<BattleFieldDimCamera>(ref this._camFieldDimCamera);
			return true;
		}

		public bool InitEnemyFieldCameraDefault()
		{
			this.enemyFieldCamera.isCulling = false;
			this.enemyFieldCamera.eyePosition = new Vector3(0f, 4f, 0f);
			this.enemyFieldCamera.eyeRotation = Quaternion.get_identity();
			this.enemyFieldCamera.fieldOfView = 30f;
			return true;
		}

		public bool InitShellingPhaseCamera()
		{
			Vector3 eyePosition = new Vector3(0f, 4f, 0f);
			Quaternion identity = Quaternion.get_identity();
			this._listCameras.get_Item(0).eyePosition = eyePosition;
			this._listCameras.get_Item(0).eyeRotation = identity;
			this._listCameras.get_Item(1).eyePosition = eyePosition;
			this._listCameras.get_Item(1).eyeRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			return true;
		}

		public void SwitchMainCamera(FleetType iType)
		{
			int cnt = 0;
			this._listCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.isCulling = (cnt == (int)iType);
				cnt++;
			});
		}

		public void ResetFieldCamSettings(FleetType iType)
		{
			this._listCameras.get_Item((int)iType).ResetMotionBlur();
			this._listCameras.get_Item((int)iType).clearFlags = 1;
			this._listCameras.get_Item((int)iType).cullingMask = this.GetDefaultLayers();
		}

		public void fieldDimCameraEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				this._camFieldDimCamera.SyncCameraProperty();
				this._camFieldDimCamera.isCulling = true;
				this._listCameras.get_Item(0).cullingMask = Generics.Layers.FocusDim;
			}
			else
			{
				this._camFieldDimCamera.cullingMask = Generics.Layers.Nothing;
				this._camFieldDimCamera.isCulling = false;
				this._camFieldDimCamera.isSync = false;
				this._camFieldDimCamera.maskAlpha = 0f;
			}
		}

		public void SetFieldCameraEnabled(bool isEnabled)
		{
			this._listCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.isCulling = isEnabled;
			});
			this._camFieldDimCamera.isCulling = isEnabled;
		}

		public Generics.Layers GetDefaultLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects | Generics.Layers.UnRefrectEffects;
		}

		public Generics.Layers GetEnemyCamSplitLayers()
		{
			return Generics.Layers.Background | Generics.Layers.Transition | Generics.Layers.ShipGirl | Generics.Layers.Effects | Generics.Layers.UnRefrectEffects | Generics.Layers.SplitWater;
		}

		public Generics.Layers GetDefaultDimLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects;
		}

		public void SetSplitCameras(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 1f);
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			bool isCulling = isSplit;
			this._listCameras.get_Item(0).viewportRect = viewportRect;
			this._listCameras.get_Item(0).isCulling = true;
			this._listCameras.get_Item(1).viewportRect = viewportRect2;
			this._listCameras.get_Item(1).isCulling = isCulling;
			if (!isSplit)
			{
				this._camCutInCamera.camera.set_rect(new Rect(0f, 0f, 1f, 1f));
				this._camCutInCamera.camera.set_enabled(true);
				this._camCutInCamera.camera.set_orthographicSize(1f);
			}
			this._isSplit = isSplit;
		}

		public void SetSplitCameras(bool isSplit, bool is2DCamUpScreen, FleetType iType)
		{
			Rect rect = (!is2DCamUpScreen) ? new Rect(0f, 0f, 1f, 0.5f) : new Rect(0f, 0.5f, 1f, 1f);
			Rect rect3DCam = (!is2DCamUpScreen) ? new Rect(0f, 0.5f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			if (isSplit)
			{
				this._camCutInCamera.camera.set_rect(rect);
				this._camCutInCamera.camera.set_orthographicSize(0.5f);
				this._camCutInCamera.set_enabled(true);
				int cnt = 0;
				this._listCameras.ForEach(delegate(BattleFieldCamera x)
				{
					if (cnt == (int)iType)
					{
						x.viewportRect = rect3DCam;
						x.isCulling = true;
					}
					else
					{
						x.isCulling = false;
					}
					cnt++;
				});
			}
			else
			{
				this.SetSplitCameras(isSplit);
			}
			this._isSplit = isSplit;
		}

		public void SetVerticalSplitCameras(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 0.5f, 1f);
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0.5f, 0f, 0.5f, 1f);
			bool isCulling = isSplit;
			this._listCameras.get_Item(0).viewportRect = viewportRect;
			this._listCameras.get_Item(0).isCulling = true;
			this._listCameras.get_Item(1).viewportRect = viewportRect2;
			this._listCameras.get_Item(1).isCulling = isCulling;
			if (!isSplit)
			{
				this._camCutInCamera.camera.set_rect(new Rect(0f, 0f, 1f, 1f));
				this._camCutInCamera.camera.set_enabled(true);
				this._camCutInCamera.camera.set_orthographicSize(1f);
			}
			this._isSplit = isSplit;
		}

		public void SetSplitCameras2D(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 0.5f);
			Vector3 localPosition = (!isSplit) ? Vector3.get_zero() : (Vector3.get_left() * (float)Screen.get_width());
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			Vector3 localPosition2 = (!isSplit) ? (Vector3.get_down() * 3000f) : (Vector3.get_right() * (float)Screen.get_width());
			bool isCulling = isSplit;
			this.cutInCamera.viewportRect = viewportRect;
			this.cutInCamera.isCulling = true;
			this.cutInCamera.get_transform().set_localPosition(localPosition);
			this.cutInEffectCamera.viewportRect = viewportRect2;
			this.cutInEffectCamera.isCulling = isCulling;
			this.cutInEffectCamera.get_transform().set_localPosition(localPosition2);
			this._isSplit2d = isSplit;
		}

		public void SetSplitCamera2DSamePos(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 0.5f);
			Vector3 localPosition = (!isSplit) ? Vector3.get_zero() : Vector3.get_zero();
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			Vector3 localPosition2 = (!isSplit) ? (Vector3.get_down() * 3000f) : Vector3.get_zero();
			bool isCulling = isSplit;
			this.cutInCamera.viewportRect = viewportRect;
			this.cutInCamera.isCulling = true;
			this.cutInCamera.get_transform().set_localPosition(localPosition);
			this.cutInEffectCamera.viewportRect = viewportRect2;
			this.cutInEffectCamera.isCulling = isCulling;
			this.cutInEffectCamera.get_transform().set_localPosition(localPosition2);
			this._isSplit2d = isSplit;
		}
	}
}
