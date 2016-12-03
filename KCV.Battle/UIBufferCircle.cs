using KCV.Battle.Utils;
using KCV.Generic;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBufferCircle : InstantiateObject<UIBufferCircle>
	{
		[Serializable]
		private struct Param : IDisposable
		{
			public float gearRotateSpeed;

			public float circleRotateSpeed;

			public float lookAtLineSize;

			public float lookAtLineAnimTime;

			public float focusCircleScale;

			public float focusCircleScalingTime;

			public float focusCircleColorlingTime;

			public Color focusCircleColor;

			public void Dispose()
			{
				Mem.Del<float>(ref this.gearRotateSpeed);
				Mem.Del<float>(ref this.circleRotateSpeed);
				Mem.Del<float>(ref this.lookAtLineSize);
				Mem.Del<float>(ref this.lookAtLineAnimTime);
				Mem.Del<float>(ref this.focusCircleScale);
				Mem.Del<float>(ref this.focusCircleScalingTime);
				Mem.Del<float>(ref this.focusCircleColorlingTime);
				Mem.Del<Color>(ref this.focusCircleColor);
			}
		}

		[SerializeField]
		private float _fCircleScale = 50f;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private List<MeshRenderer> _listMeshRenderer;

		[SerializeField]
		private List<Texture2D> _listCircleTexture;

		[SerializeField]
		private MeshRenderer _mrGear;

		[SerializeField]
		private Texture2D _texGear;

		[SerializeField]
		private MeshRenderer _mrLine;

		[SerializeField]
		private Texture2D _texLine;

		[SerializeField]
		private UIBufferCircle.Param _strParam;

		[Button("ReflectionMaterial", "マテリアル反映", new object[]
		{

		}), SerializeField]
		private int _nReflectionMaterial;

		private Transform _traTarget;

		private bool _isLootAtLine;

		private Quaternion _quaStartLine;

		private Color _cDefaultBaseColor;

		private Color _cDefaultLineColor;

		public static UIBufferCircle Instantiate(UIBufferCircle prefab, Transform parent, FleetType iType, Transform target)
		{
			UIBufferCircle uIBufferCircle = InstantiateObject<UIBufferCircle>.Instantiate(prefab);
			uIBufferCircle.get_transform().set_localScale(new Vector3(uIBufferCircle._fCircleScale, 0f, uIBufferCircle._fCircleScale));
			uIBufferCircle.get_transform().set_parent(parent);
			uIBufferCircle.get_transform().localPositionZero();
			uIBufferCircle.get_transform().set_rotation(new Quaternion(0f, 0f, 0f, 0f));
			uIBufferCircle.Init(iType, target);
			return uIBufferCircle;
		}

		private bool Init(FleetType iType, Transform target)
		{
			this.ReflectionMaterial();
			this.SetCircleColor(iType);
			this.PlayGearAnimation();
			return true;
		}

		private void OnDestroy()
		{
			Transform transform = this._listMeshRenderer.get_Item(0).get_transform();
			Mem.DelMeshSafe(ref transform);
			transform = this._listMeshRenderer.get_Item(1).get_transform();
			Mem.DelMeshSafe(ref transform);
			transform = this._mrGear.get_transform();
			Mem.DelMeshSafe(ref transform);
			transform = this._mrLine.get_transform();
			Mem.DelMeshSafe(ref transform);
			Mem.Del<float>(ref this._fCircleScale);
			Mem.Del<Material>(ref this._material);
			Mem.DelListSafe<MeshRenderer>(ref this._listMeshRenderer);
			Mem.DelListSafe<Texture2D>(ref this._listCircleTexture);
			Mem.Del<MeshRenderer>(ref this._mrGear);
			Mem.Del<Texture2D>(ref this._texGear);
			Mem.Del<MeshRenderer>(ref this._mrLine);
			Mem.Del<Texture2D>(ref this._texLine);
			Mem.DelIDisposableSafe<UIBufferCircle.Param>(ref this._strParam);
			Mem.Del<int>(ref this._nReflectionMaterial);
			Mem.Del<Transform>(ref this._traTarget);
			Mem.Del<bool>(ref this._isLootAtLine);
			Mem.Del<Quaternion>(ref this._quaStartLine);
			Mem.Del<Color>(ref this._cDefaultBaseColor);
			Mem.Del<Color>(ref this._cDefaultLineColor);
			Mem.Del<Transform>(ref transform);
		}

		public bool Run()
		{
			if (this._isLootAtLine && this._mrLine != null)
			{
				this._mrLine.get_transform().LookAt(this._traTarget);
			}
			return true;
		}

		public void SetDefault()
		{
			this._listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.get_transform().set_localScale(Vector3.get_one());
				x.get_sharedMaterial().set_color(this._cDefaultBaseColor);
			});
			this._mrGear.get_transform().set_localScale(Vector3.get_one());
			this._mrGear.get_sharedMaterial().set_color(this._cDefaultBaseColor);
			this._mrLine.get_transform().set_localScale(new Vector3(0.6f, 1f, 0.8f));
			this._mrLine.get_sharedMaterial().set_color(this._cDefaultLineColor);
		}

		public void CalcInitLineRotation(Transform target)
		{
			this._traTarget = target;
			this._mrLine.get_transform().LookAt(target.get_position());
			this._quaStartLine = this._mrLine.get_transform().get_rotation();
		}

		public void PlayLineAnimation()
		{
			this._mrLine.get_transform().LTCancel();
			this._mrLine.get_transform().LTRotateAroundLocal(Vector3.get_up(), XorRandom.GetFLim(0.1f, 1f) * 50f, XorRandom.GetFLim(2.5f, 4.6f)).setEase(LeanTweenType.linear).setLoopPingPong();
		}

		public void PlayLookAtLine()
		{
			this._mrLine.get_transform().LTCancel();
			this._mrLine.get_transform().LTValue(this._cDefaultLineColor, new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(60f), KCVColor.ColorRate(175f)), this._strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint).setOnUpdate(delegate(Color color)
			{
				this._mrLine.get_sharedMaterial().set_color(color);
			});
			this._mrLine.get_transform().LookTo(this._traTarget.get_position(), this._strParam.lookAtLineAnimTime, iTween.EaseType.easeInQuint, delegate
			{
				this._isLootAtLine = true;
			});
			this._mrLine.get_transform().LTScale(Vector3.get_one() * this._strParam.lookAtLineSize, this._strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint);
		}

		public void PlayLookAtLine2Assult()
		{
			this._mrLine.get_transform().LTCancel();
			this._mrLine.get_transform().LookTo(this._traTarget.get_position(), this._strParam.lookAtLineAnimTime, iTween.EaseType.easeInQuint, delegate
			{
				this._isLootAtLine = true;
			});
			this._mrLine.get_transform().LTValue(this._cDefaultLineColor, new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), KCVColor.ColorRate(175f)), this._strParam.lookAtLineAnimTime).setEase(LeanTweenType.easeInQuint).setOnUpdate(delegate(Color color)
			{
				this._mrLine.get_sharedMaterial().set_color(color);
			});
		}

		public void StopLineAnimation()
		{
			this._mrLine.get_transform().LTCancel();
		}

		public void PlayGearAnimation()
		{
			this._listMeshRenderer.get_Item(1).get_transform().LTRotateAround(Vector3.get_up(), 360f, this._strParam.circleRotateSpeed).setEase(LeanTweenType.linear).setLoopClamp();
			this._mrGear.get_transform().LTRotateAround(Vector3.get_up(), -360f, this._strParam.gearRotateSpeed).setEase(LeanTweenType.linear).setLoopClamp();
		}

		public void StopGearAnimation()
		{
			this._mrLine.get_transform().LTCancel();
			this._mrGear.get_transform().LTCancel();
		}

		public void PlayFocusCircleAnimation(bool isFocus)
		{
			if (isFocus)
			{
				base.get_transform().LTValue(1f, this._strParam.focusCircleScale, this._strParam.focusCircleScalingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._listMeshRenderer.ForEach(delegate(MeshRenderer obj)
					{
						obj.get_transform().set_localScale(Vector3.get_one() * x);
					});
					this._mrGear.get_transform().set_localScale(Vector3.get_one() * x);
				});
				base.get_transform().LTValue(this._cDefaultBaseColor, this._strParam.focusCircleColor, this._strParam.focusCircleColorlingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
				{
					this._listMeshRenderer.ForEach(delegate(MeshRenderer y)
					{
						y.get_sharedMaterial().set_color(x);
					});
					this._mrGear.get_sharedMaterial().set_color(x);
				});
			}
			else
			{
				Color baseColor = this._cDefaultBaseColor;
				base.get_transform().LTValue(this._cDefaultBaseColor.a, Mathe.Rate(0f, 255f, 70f), this._strParam.focusCircleColorlingTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					baseColor.a = x;
					this._listMeshRenderer.ForEach(delegate(MeshRenderer y)
					{
						y.get_sharedMaterial().set_color(baseColor);
					});
					this._mrGear.get_sharedMaterial().set_color(baseColor);
				});
			}
		}

		private void ReflectionMaterial()
		{
			int cnt = 0;
			this._listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.set_material(new Material(this._material));
				x.get_sharedMaterial().set_mainTexture(this._listCircleTexture.get_Item(cnt));
				cnt++;
			});
			if (Application.get_isPlaying())
			{
				Mem.DelListSafe<Texture2D>(ref this._listCircleTexture);
			}
			this._mrGear.set_material(new Material(this._material));
			this._mrGear.get_sharedMaterial().set_mainTexture(this._texGear);
			if (Application.get_isPlaying())
			{
				Mem.Del<Texture2D>(ref this._texGear);
			}
			this._mrLine.set_material(new Material(this._material));
			this._mrLine.get_sharedMaterial().set_mainTexture(this._texLine);
			if (Application.get_isPlaying())
			{
				Mem.Del<Texture2D>(ref this._texLine);
			}
			if (Application.get_isPlaying())
			{
				Mem.Del<Material>(ref this._material);
			}
		}

		private void SetCircleColor(FleetType iType)
		{
			Color baseColor = (iType != FleetType.Friend) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f), 1f) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), 1f);
			this._cDefaultBaseColor = baseColor;
			this._listMeshRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.get_sharedMaterial().set_color(baseColor);
			});
			this._mrGear.get_sharedMaterial().set_color(baseColor);
			Color color = (iType != FleetType.Friend) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f), KCVColor.ColorRate(103f)) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f), KCVColor.ColorRate(103f));
			this._mrLine.get_sharedMaterial().set_color(color);
			this._cDefaultLineColor = color;
		}
	}
}
