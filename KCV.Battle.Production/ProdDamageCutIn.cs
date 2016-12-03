using KCV.Battle.Utils;
using KCV.Generic;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdDamageCutIn : BaseBattleAnimation
	{
		public enum DamageCutInType
		{
			Moderate,
			Heavy
		}

		private enum DamageCutInList
		{
			ProdDamageCutInModerateFirst,
			ProdDamageCutInModerateSecond,
			ProdDamageCutInHeavyFirst,
			ProdDamageCutInHeavySecond
		}

		[Serializable]
		private class DamageShip : IDisposable
		{
			[SerializeField]
			private Transform _traShip;

			[SerializeField]
			private UITexture _uiShipTex;

			private List<Texture2D> _listShipTexture;

			private List<Vector3> _listShipOffs;

			private ObjectTinyShake _clsTinyShake;

			private ShipModel_Defender _clsDefender;

			private bool _isActive;

			public Transform transform
			{
				get
				{
					return this._traShip;
				}
				set
				{
					this._traShip = value;
				}
			}

			public UITexture shipTexture
			{
				get
				{
					return this._uiShipTex;
				}
				set
				{
					this._uiShipTex = value;
				}
			}

			public ObjectTinyShake tinyShake
			{
				get
				{
					if (this._clsTinyShake == null)
					{
						this._clsTinyShake = this._uiShipTex.GetComponent<ObjectTinyShake>();
					}
					return this._clsTinyShake;
				}
			}

			public ShipModel_Defender defender
			{
				get
				{
					return this._clsDefender;
				}
			}

			public bool isActive
			{
				get
				{
					return this._isActive;
				}
				set
				{
					this._isActive = value;
					this.transform.set_localScale((!this.isActive) ? Vector3.get_zero() : Vector3.get_one());
				}
			}

			public DamageShip(Transform parent, string objName)
			{
				Util.FindParentToChild<Transform>(ref this._traShip, parent, objName);
				Util.FindParentToChild<UITexture>(ref this._uiShipTex, this._traShip, "ShipTex");
				this._isActive = false;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._traShip);
				Mem.Del<UITexture>(ref this._uiShipTex);
				Mem.DelListSafe<Texture2D>(ref this._listShipTexture);
				Mem.DelListSafe<Vector3>(ref this._listShipOffs);
				Mem.Del<ObjectTinyShake>(ref this._clsTinyShake);
				Mem.Del<ShipModel_Defender>(ref this._clsDefender);
				Mem.Del<bool>(ref this._isActive);
			}

			public void SetShipInfos(ShipModel_Defender defender, List<Texture2D> tex, List<Vector3> offs)
			{
				this._listShipTexture = tex;
				this._listShipOffs = offs;
				this.SwitchMainTexture(false);
			}

			public void SetShipInfos(ShipModel_Defender defender, List<Texture2D> tex, List<Vector3> offs, bool isAfter)
			{
				this._listShipTexture = tex;
				this._listShipOffs = offs;
				this.SwitchMainTexture(isAfter);
			}

			public void SwitchMainTexture(bool isAfter)
			{
				if (this._listShipTexture == null || this._listShipOffs == null)
				{
					return;
				}
				int num = (!isAfter) ? 0 : 1;
				this._uiShipTex.mainTexture = this._listShipTexture.get_Item(num);
				this._uiShipTex.MakePixelPerfect();
				this._uiShipTex.get_transform().set_localPosition(this._listShipOffs.get_Item(num));
			}
		}

		[Button("InitMode", "[ModerateMode]", new object[]
		{
			ProdDamageCutIn.DamageCutInType.Moderate
		}), SerializeField]
		private int SetModerateButton;

		[Button("InitMode", "[HeavyMode]", new object[]
		{
			ProdDamageCutIn.DamageCutInType.Heavy
		}), SerializeField]
		private int SetHeavyButton;

		[SerializeField]
		private Transform _prefabModerateOrHeavy;

		[SerializeField]
		private List<ProdDamageCutIn.DamageShip> _listDamageShips;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiMask;

		[SerializeField]
		private Transform _traShipShakeAnchor;

		[SerializeField]
		private Transform _traShips;

		[SerializeField]
		private ParticleSystem _psModerateSmoke;

		[SerializeField]
		private ParticleSystem _psHeavyBack;

		[SerializeField]
		private List<ParticleSystem> _psHeavySmokes;

		[SerializeField]
		private ParticleSystem _psLargeExplosion;

		private int _nDrawShipNum;

		private UIPanel _uiPanel;

		private ShipModel_Defender _clsShipModel;

		private ProdDamageCutIn.DamageCutInType _iType;

		private ModerateOrHeavyController _ctrlModerateOrHeavy;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		public static ProdDamageCutIn Instantiate(ProdDamageCutIn prefab, Transform parent)
		{
			ProdDamageCutIn prodDamageCutIn = Object.Instantiate<ProdDamageCutIn>(prefab);
			prodDamageCutIn.get_transform().set_parent(parent);
			prodDamageCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodDamageCutIn.get_transform().set_localScale(Vector3.get_one());
			return prodDamageCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			if (this._uiBackground == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiBackground, base.get_transform(), "Background");
			}
			if (this._listDamageShips == null)
			{
				this._listDamageShips = new List<ProdDamageCutIn.DamageShip>();
				for (int i = 0; i < 3; i++)
				{
					this._listDamageShips.Add(new ProdDamageCutIn.DamageShip(base.get_transform(), string.Format("ShipShakeAnchor/Ships/Ship{0}", i + 1)));
				}
			}
			Transform transform = Object.Instantiate(this._prefabModerateOrHeavy, Vector3.get_zero(), Quaternion.get_identity()) as Transform;
			transform.set_parent(BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
			transform.get_transform().set_localPosition(new Vector3(0f, 0f, -30f));
			this._ctrlModerateOrHeavy = transform.GetComponent<ModerateOrHeavyController>();
			List<ParticleSystem> list = new List<ParticleSystem>(base.get_transform().GetComponentsInChildren<ParticleSystem>());
			list.ForEach(delegate(ParticleSystem x)
			{
				x.SetActive(false);
			});
			this._nDrawShipNum = 0;
			this._psModerateSmoke.Stop();
			this.panel.widgetsAreStatic = true;
			base.get_transform().set_localScale(Vector3.get_zero());
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<int>(ref this.SetModerateButton);
			Mem.Del<int>(ref this.SetHeavyButton);
			Mem.Del<Transform>(ref this._prefabModerateOrHeavy);
			if (this._listDamageShips != null)
			{
				this._listDamageShips.ForEach(delegate(ProdDamageCutIn.DamageShip x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe<ProdDamageCutIn.DamageShip>(ref this._listDamageShips);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiMask);
			Mem.Del<Transform>(ref this._traShipShakeAnchor);
			Mem.Del<Transform>(ref this._traShips);
			Mem.Del(ref this._psModerateSmoke);
			Mem.Del(ref this._psHeavyBack);
			Mem.DelListSafe<ParticleSystem>(ref this._psHeavySmokes);
			Mem.Del(ref this._psLargeExplosion);
			Mem.Del<int>(ref this._nDrawShipNum);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<ShipModel_Defender>(ref this._clsShipModel);
			Mem.Del<ProdDamageCutIn.DamageCutInType>(ref this._iType);
			Mem.DelComponentSafe<ModerateOrHeavyController>(ref this._ctrlModerateOrHeavy);
		}

		private void LateUpdate()
		{
			this._ctrlModerateOrHeavy.LateRun();
		}

		private void InitMode(ProdDamageCutIn.DamageCutInType iType)
		{
			this._iType = iType;
			Color color = (iType != ProdDamageCutIn.DamageCutInType.Moderate) ? new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(8f), KCVColor.ColorRate(20f), 1f) : new Color(KCVColor.ColorRate(62f), KCVColor.ColorRate(187f), KCVColor.ColorRate(229f), 1f);
			this._uiBackground.color = color;
			this._uiMask.SetActive(iType == ProdDamageCutIn.DamageCutInType.Moderate);
			this._psModerateSmoke.SetActive(iType == ProdDamageCutIn.DamageCutInType.Moderate);
			this._psHeavyBack.SetActive(iType == ProdDamageCutIn.DamageCutInType.Heavy);
		}

		public void SetShipData(List<ShipModel_Defender> defenderList, ProdDamageCutIn.DamageCutInType iType)
		{
			this.InitMode(iType);
			this._nDrawShipNum = Enumerable.Count<ShipModel_Defender>(Enumerable.Take<ShipModel_Defender>(defenderList, this._listDamageShips.get_Count()));
			this._clsShipModel = defenderList.get_Item(0);
			this.SetDamageShipData(defenderList);
		}

		private void SetDamageShipData(List<ShipModel_Defender> defenderList)
		{
			switch (defenderList.get_Count())
			{
			case 1:
			{
				List<Texture2D> shipTextures = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipTexture(defenderList.get_Item(0)) : this.GetModerateShipTexture(defenderList.get_Item(0));
				List<Vector3> shipOffes = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipOffs(defenderList.get_Item(0)) : this.GetModerateShipOffs(defenderList.get_Item(0));
				this._listDamageShips.ForEach(delegate(ProdDamageCutIn.DamageShip x)
				{
					x.SetShipInfos(defenderList.get_Item(0), shipTextures, shipOffes, this._iType == ProdDamageCutIn.DamageCutInType.Heavy);
				});
				break;
			}
			case 2:
			{
				List<Texture2D> shipTextures = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipTexture(defenderList.get_Item(0)) : this.GetModerateShipTexture(defenderList.get_Item(0));
				List<Vector3> shipOffes = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipOffs(defenderList.get_Item(0)) : this.GetModerateShipOffs(defenderList.get_Item(0));
				this._listDamageShips.get_Item(0).SetShipInfos(defenderList.get_Item(0), shipTextures, shipOffes, this._iType == ProdDamageCutIn.DamageCutInType.Heavy);
				shipTextures = ((this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipTexture(defenderList.get_Item(1)) : this.GetModerateShipTexture(defenderList.get_Item(1)));
				shipOffes = ((this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipOffs(defenderList.get_Item(1)) : this.GetModerateShipOffs(defenderList.get_Item(1)));
				Enumerable.Skip<ProdDamageCutIn.DamageShip>(this._listDamageShips, 1).ForEach(delegate(ProdDamageCutIn.DamageShip x)
				{
					x.SetShipInfos(defenderList.get_Item(1), shipTextures, shipOffes, this._iType == ProdDamageCutIn.DamageCutInType.Heavy);
				});
				break;
			}
			case 3:
			{
				int cnt = 0;
				this._listDamageShips.ForEach(delegate(ProdDamageCutIn.DamageShip x)
				{
					List<Texture2D> tex = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipTexture(defenderList.get_Item(cnt)) : this.GetModerateShipTexture(defenderList.get_Item(cnt));
					List<Vector3> offs = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? this.GetHeavyShipOffs(defenderList.get_Item(cnt)) : this.GetModerateShipOffs(defenderList.get_Item(cnt));
					this._listDamageShips.get_Item(cnt).SetShipInfos(defenderList.get_Item(cnt), tex, offs, this._iType == ProdDamageCutIn.DamageCutInType.Heavy);
					cnt++;
				});
				break;
			}
			default:
				this._listDamageShips.ForEach(delegate(ProdDamageCutIn.DamageShip x)
				{
					x.isActive = false;
				});
				break;
			}
		}

		public override void Play(Enum iEnum, Action callback)
		{
			if (this.isPlaying)
			{
				return;
			}
			this._iType = (ProdDamageCutIn.DamageCutInType)iEnum;
			this.panel.widgetsAreStatic = false;
			base.get_transform().set_localScale(Vector3.get_one());
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(true);
			cutInEffectCamera.motionBlur.blurAmount = 0.3f;
			cutInEffectCamera.glowEffect.set_enabled(false);
			cutInEffectCamera.isCulling = true;
			this._traShips.localPositionZero();
			ProdDamageCutIn.DamageCutInList damageCutInList = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? ProdDamageCutIn.DamageCutInList.ProdDamageCutInHeavyFirst : ProdDamageCutIn.DamageCutInList.ProdDamageCutInModerateFirst;
			if (this._iType == ProdDamageCutIn.DamageCutInType.Moderate)
			{
				this._psModerateSmoke.SetActive(true);
				this._psModerateSmoke.Play();
			}
			else
			{
				this._psHeavyBack.SetActive(true);
				this._psHeavyBack.Play();
			}
			base.Play(damageCutInList, callback);
		}

		public void Play(ProdDamageCutIn.DamageCutInType iType, Action onStart, Action onFinished)
		{
			if (this.isPlaying)
			{
				return;
			}
			this._iType = iType;
			this.panel.widgetsAreStatic = false;
			base.get_transform().set_localScale(Vector3.get_one());
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(true);
			cutInEffectCamera.motionBlur.blurAmount = 0.3f;
			cutInEffectCamera.glowEffect.set_enabled(false);
			cutInEffectCamera.isCulling = true;
			this._traShips.localPositionZero();
			ProdDamageCutIn.DamageCutInList damageCutInList = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? ProdDamageCutIn.DamageCutInList.ProdDamageCutInHeavyFirst : ProdDamageCutIn.DamageCutInList.ProdDamageCutInModerateFirst;
			if (this._iType == ProdDamageCutIn.DamageCutInType.Moderate)
			{
				this._psModerateSmoke.SetActive(true);
				this._psModerateSmoke.Play();
			}
			else
			{
				this._psHeavyBack.SetActive(true);
				this._psHeavyBack.Play();
			}
			Dlg.Call(ref onStart);
			base.Play(damageCutInList, onFinished);
		}

		private void onPlayDamageTextScaling()
		{
			ModerateOrHeavyController.Mode mode = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? ModerateOrHeavyController.Mode.Heavy : ModerateOrHeavyController.Mode.Moderate;
			this._ctrlModerateOrHeavy.ShakeObservable.Take(1).Subscribe(delegate(int x)
			{
				DebugUtils.dbgAssert(this._traShipShakeAnchor != null);
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_052);
				ObjectTinyShake component = this._traShipShakeAnchor.GetComponent<ObjectTinyShake>();
				component.PlayAnimation().Subscribe(delegate(int y)
				{
				});
			}).AddTo(base.get_gameObject());
			this._ctrlModerateOrHeavy.PlayAnimation(mode).Subscribe(delegate(int _)
			{
			}).AddTo(base.get_gameObject());
		}

		private void OnPlayHeavyExplosion(int nNum)
		{
			if (this._psHeavySmokes.get_Item(nNum) != null)
			{
				this._psHeavySmokes.get_Item(nNum).SetActive(true);
				this._psHeavySmokes.get_Item(nNum).Play();
				Observable.Timer(TimeSpan.FromSeconds((double)this._psHeavySmokes.get_Item(nNum).get_duration())).Subscribe(delegate(long _)
				{
					this._psHeavySmokes.get_Item(nNum).SetActive(false);
				}).AddTo(base.get_gameObject());
			}
			Observable.NextFrame(FrameCountType.Update).Subscribe(delegate(Unit _)
			{
				this._listDamageShips.get_Item(nNum).tinyShake.PlayAnimation().Subscribe<int>();
			}).AddTo(base.get_gameObject());
		}

		private void OnPlayLargeExplosion()
		{
			this._psLargeExplosion.SetActive(true);
			this._psLargeExplosion.Play();
			Observable.Timer(TimeSpan.FromSeconds((double)this._psLargeExplosion.get_duration())).Subscribe(delegate(long _)
			{
				this._psLargeExplosion.SetActive(false);
			}).AddTo(base.get_gameObject());
			this.OnSetBlur(1);
		}

		private void onFirstAnimationFinished()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.set_enabled(false);
			int num = 0;
			using (List<ProdDamageCutIn.DamageShip>.Enumerator enumerator = this._listDamageShips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProdDamageCutIn.DamageShip current = enumerator.get_Current();
					current.transform.set_localPosition(BattleDefines.DAMAGE_CUT_IN_SHIP_DRAW_POS.get_Item(this._nDrawShipNum).get_Item(num));
					current.transform.set_localScale(Vector3.get_one());
					current.shipTexture.alpha = 1f;
					num++;
				}
			}
			this._traShips.get_transform().set_localScale(Vector3.get_one() * 7.5f);
			this._traShips.get_transform().set_localPosition(Vector3.get_down() * 70f);
			if (this._clsShipModel != null)
			{
				KCV.Battle.Utils.ShipUtils.PlayDamageCutInVoice(this._clsShipModel);
			}
			ProdDamageCutIn.DamageCutInList damageCutInList = (this._iType != ProdDamageCutIn.DamageCutInType.Moderate) ? ProdDamageCutIn.DamageCutInList.ProdDamageCutInHeavySecond : ProdDamageCutIn.DamageCutInList.ProdDamageCutInModerateSecond;
			this._animAnimation.Play(damageCutInList.ToString());
		}

		private void OnSetBlur(int isEnabled)
		{
		}

		private void OnSwitchModerateDamageShipTexture()
		{
			this._listDamageShips.ForEach(delegate(ProdDamageCutIn.DamageShip x)
			{
				x.SwitchMainTexture(true);
			});
		}

		protected override void onAnimationFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				List<ParticleSystem> list = new List<ParticleSystem>(base.get_transform().GetComponentsInChildren<ParticleSystem>());
				list.ForEach(delegate(ParticleSystem x)
				{
					x.SetActive(false);
				});
				base.get_transform().localScaleZero();
				this.panel.widgetsAreStatic = true;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				GameObject gameObject = cutInEffectCamera.get_transform().Find("TorpedoLine/OverlayLine").get_gameObject();
				if (gameObject != null)
				{
					UITexture component = gameObject.GetComponent<UITexture>();
					if (component != null && component.alpha <= 0.1f)
					{
						cutInEffectCamera.isCulling = false;
					}
				}
				cutInEffectCamera.motionBlur.set_enabled(false);
				cutInEffectCamera.blur.set_enabled(false);
			});
			base.onAnimationFinished();
		}

		private List<Texture2D> GetModerateShipTexture(ShipModel_Defender defender)
		{
			List<Texture2D> list = new List<Texture2D>();
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, false));
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, true));
			return list;
		}

		private List<Vector3> GetModerateShipOffs(ShipModel_Defender defender)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, false, MstShipGraphColumn.CutInSp1));
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, true, MstShipGraphColumn.CutInSp1));
			return list;
		}

		private List<Texture2D> GetHeavyShipTexture(ShipModel_Defender defender)
		{
			List<Texture2D> list = new List<Texture2D>();
			list.Add(null);
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, true));
			return list;
		}

		private List<Vector3> GetHeavyShipOffs(ShipModel_Defender defender)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(Vector3.get_zero());
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, true, MstShipGraphColumn.CutInSp1));
			return list;
		}
	}
}
