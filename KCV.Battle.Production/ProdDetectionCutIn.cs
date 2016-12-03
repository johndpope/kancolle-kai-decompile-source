using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdDetectionCutIn : BaseAnimation
	{
		private enum AnimationList
		{
			ProdDetectionCutIn
		}

		private static readonly string BASE_PATH = "Textures/Battle/Detection";

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private List<UITexture> _listAircraft;

		[SerializeField]
		private UITexture _uiOverlayWhite;

		[SerializeField]
		private ParticleSystem _psCloud;

		[SerializeField]
		private UINoiseScaleOutLabel _uiDetectionLabel;

		[SerializeField]
		private List<Transform> _animatingAircrafts;

		private List<Vector3> _aircraftBasePositions = new List<Vector3>();

		[SerializeField]
		private float _bNoiseSize = 10f;

		[SerializeField]
		private float _bNoiseSpeed = 10f;

		[SerializeField]
		private float _sNoiseSize = 10f;

		[SerializeField]
		private float _sNoiseSpeed = 10f;

		private DetectionProductionType _iType;

		private bool _isAircraft;

		public DetectionProductionType detectionType
		{
			get
			{
				return this._iType;
			}
		}

		public bool isAircraft
		{
			get
			{
				return this._isAircraft;
			}
		}

		public static ProdDetectionCutIn Instantiate(ProdDetectionCutIn prefab, Transform parent, SakutekiModel model)
		{
			ProdDetectionCutIn prodDetectionCutIn = Object.Instantiate<ProdDetectionCutIn>(prefab);
			prodDetectionCutIn.get_transform().set_parent(parent);
			prodDetectionCutIn.get_transform().set_localScale(Vector3.get_zero());
			prodDetectionCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodDetectionCutIn.setAircraft(KCV.Battle.Utils.SlotItemUtils.GetDetectionScoutingPlane(model.planes_f));
			return prodDetectionCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			this._isAircraft = false;
			if (this._listAircraft == null)
			{
				this._listAircraft = new List<UITexture>();
				for (int i = 0; i < 3; i++)
				{
					this._listAircraft.Add(base.get_transform().FindChild(string.Format("AircraftAnchor{0}/Aircraft", i)).GetComponent<UITexture>());
				}
			}
			if (this._uiBackground == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiBackground, base.get_transform(), "Background");
			}
			if (this._uiOverlayWhite == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiOverlayWhite, base.get_transform(), "OverlayWhite");
			}
			this._uiOverlayWhite.alpha = 1f;
			if (this._psCloud == null)
			{
				Util.FindParentToChild<ParticleSystem>(ref this._psCloud, base.get_transform(), "PSCloud");
			}
			this._psCloud.Stop();
			this._animAnimation.Stop();
			for (int j = 0; j < this._animatingAircrafts.get_Count(); j++)
			{
				this._aircraftBasePositions.Add(this._animatingAircrafts.get_Item(j).get_localPosition());
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.DelListSafe<UITexture>(ref this._listAircraft);
			Mem.Del<UITexture>(ref this._uiOverlayWhite);
			Mem.Del(ref this._psCloud);
			Mem.Del<UINoiseScaleOutLabel>(ref this._uiDetectionLabel);
			Mem.DelListSafe<Transform>(ref this._animatingAircrafts);
			Mem.DelListSafe<Vector3>(ref this._aircraftBasePositions);
			Mem.Del<float>(ref this._bNoiseSize);
			Mem.Del<float>(ref this._bNoiseSpeed);
			Mem.Del<float>(ref this._sNoiseSize);
			Mem.Del<float>(ref this._sNoiseSpeed);
			Mem.Del<DetectionProductionType>(ref this._iType);
			Mem.Del<bool>(ref this._isAircraft);
		}

		private void Update()
		{
			for (int i = 0; i < this._animatingAircrafts.get_Count(); i++)
			{
				float num = (float)i * 46f;
				Vector3 vector = this._aircraftBasePositions.get_Item(i);
				Transform transform = this._animatingAircrafts.get_Item(i);
				float num2 = (Mathf.PerlinNoise(Time.get_time() * this._sNoiseSpeed, 0f + num) * 2f - 1f) * this._sNoiseSize;
				float num3 = (Mathf.PerlinNoise(Time.get_time() * this._bNoiseSpeed, 10f + num) * 2f - 1f) * this._bNoiseSize;
				float num4 = (Mathf.PerlinNoise(Time.get_time() * this._bNoiseSpeed, 20f + num) * 2f - 1f) * this._bNoiseSize;
				transform.set_localPosition(vector + new Vector3(0f, num2, 0f) + new Vector3(num3, num4, 0f));
			}
		}

		public override void Play(Action forceCallback, Action callback)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_946);
			base.get_transform().set_localScale(Vector3.get_one());
			this._uiDetectionLabel.Play();
			base.Play(forceCallback, callback);
		}

		private void setAircraft(SlotitemModel_Battle model)
		{
			if (model == null)
			{
				this._isAircraft = false;
				this._listAircraft.ForEach(delegate(UITexture x)
				{
					x.mainTexture = null;
				});
				return;
			}
			this._isAircraft = true;
			Texture2D mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadUniDirTexture(model);
			using (List<UITexture>.Enumerator enumerator = this._listAircraft.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UITexture current = enumerator.get_Current();
					current.mainTexture = mainTexture;
					current.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(6);
				}
			}
		}

		private SlotitemModel_Battle getProdPlane(SakutekiModel model)
		{
			using (List<List<SlotitemModel_Battle>>.Enumerator enumerator = model.planes_f.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<SlotitemModel_Battle> current = enumerator.get_Current();
					if (current != null)
					{
						using (List<SlotitemModel_Battle>.Enumerator enumerator2 = current.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SlotitemModel_Battle current2 = enumerator2.get_Current();
								if (current2 != null)
								{
									return current2;
								}
							}
						}
					}
				}
			}
			return null;
		}

		private void playCloudParticle()
		{
			this._psCloud.Play();
		}

		private void OnAnimationAfterDiscard()
		{
			base.onAnimationFinishedAfterDiscard();
		}
	}
}
