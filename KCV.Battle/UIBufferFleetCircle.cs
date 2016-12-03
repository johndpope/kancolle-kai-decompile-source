using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Animation))]
	public class UIBufferFleetCircle : InstantiateObject<UIBufferFleetCircle>
	{
		[SerializeField]
		private float _fCircleScale = 200f;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private MeshRenderer _mrBufferCircle;

		[SerializeField]
		private Texture2D _texBufferCircle;

		[SerializeField]
		private List<Texture2D> _listRippleTexture;

		[SerializeField]
		private List<MeshRenderer> _listRippleRenderer;

		private Color _cBaseColor;

		public static UIBufferFleetCircle Instantiate(UIBufferFleetCircle prefab, Transform parent, FleetType iType)
		{
			UIBufferFleetCircle uIBufferFleetCircle = InstantiateObject<UIBufferFleetCircle>.Instantiate(prefab, parent);
			uIBufferFleetCircle.get_transform().set_localScale(new Vector3(uIBufferFleetCircle._fCircleScale, 0f, uIBufferFleetCircle._fCircleScale));
			uIBufferFleetCircle.Init(iType);
			return uIBufferFleetCircle;
		}

		private bool Init(FleetType iType)
		{
			this.ReflectionMaterial();
			this.SetCircleColor(iType);
			this.SetCircleSize(iType);
			return true;
		}

		private void OnDestroy()
		{
			Transform meshTrans = this._mrBufferCircle.get_transform();
			Mem.DelMeshSafe(ref meshTrans);
			if (this._listRippleRenderer != null)
			{
				this._listRippleRenderer.ForEach(delegate(MeshRenderer x)
				{
					meshTrans = x.get_transform();
					Mem.DelMeshSafe(ref meshTrans);
				});
			}
			Mem.Del<float>(ref this._fCircleScale);
			Mem.Del<Material>(ref this._material);
			Mem.Del<MeshRenderer>(ref this._mrBufferCircle);
			Mem.Del<Texture2D>(ref this._texBufferCircle);
			Mem.DelListSafe<Texture2D>(ref this._listRippleTexture);
			Mem.DelListSafe<MeshRenderer>(ref this._listRippleRenderer);
			Mem.Del<Color>(ref this._cBaseColor);
			Mem.Del<Transform>(ref meshTrans);
		}

		public void PlayRipple()
		{
			base.GetComponent<Animation>().Play();
			base.get_transform().LTValue(1f, 0f, base.GetComponent<Animation>().get_Item("ProdBufferFleetCircleRipple").get_length()).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._listRippleRenderer.ForEach(delegate(MeshRenderer renderer)
				{
					renderer.get_material().set_color(new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(60f), x));
				});
			});
		}

		public void PlayFocusCircleAnimation()
		{
			Color c = this._cBaseColor;
			base.get_transform().LTValue(this._cBaseColor.a, Mathe.Rate(0f, 255f, 70f), 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				c.a = x;
				this._mrBufferCircle.get_sharedMaterial().set_color(c);
			});
		}

		public void SetDefault()
		{
			this._mrBufferCircle.get_sharedMaterial().set_color(this._cBaseColor);
		}

		private void ReflectionMaterial()
		{
			this._mrBufferCircle.set_material(new Material(this._material));
			this._mrBufferCircle.get_sharedMaterial().set_mainTexture(this._texBufferCircle);
			if (Application.get_isPlaying())
			{
				Mem.Del<Texture2D>(ref this._texBufferCircle);
			}
			int cnt = 0;
			this._listRippleRenderer.ForEach(delegate(MeshRenderer x)
			{
				x.set_material(new Material(this._material));
				x.get_sharedMaterial().set_mainTexture(this._listRippleTexture.get_Item(cnt));
				this._listRippleRenderer.get_Item(cnt).get_sharedMaterial().set_color(new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f)));
				cnt++;
			});
			if (Application.get_isPlaying())
			{
				Mem.DelListSafe<Texture2D>(ref this._listRippleTexture);
			}
		}

		private void SetCircleSize(FleetType iType)
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			base.get_transform().set_localScale(Vector3.get_one() * this.CalcCircleSize((iType != FleetType.Friend) ? battleManager.ShipCount_e : battleManager.ShipCount_f, (iType != FleetType.Friend) ? battleManager.FormationId_e : battleManager.FormationId_f));
		}

		private float CalcCircleSize(int nVessels, BattleFormationKinds1 iKind)
		{
			if (nVessels <= 3)
			{
				return 150f;
			}
			switch (iKind)
			{
			case BattleFormationKinds1.TanJuu:
			case BattleFormationKinds1.TanOu:
				if (nVessels == 4)
				{
					return 190f;
				}
				if (nVessels == 5)
				{
					return 230f;
				}
				if (nVessels == 6)
				{
					return 256f;
				}
				break;
			case BattleFormationKinds1.Rinkei:
				if (nVessels == 4 || nVessels == 5)
				{
					return 150f;
				}
				if (nVessels == 6)
				{
					return 190f;
				}
				break;
			case BattleFormationKinds1.Teikei:
				if (nVessels == 4)
				{
					return 150f;
				}
				if (nVessels == 5)
				{
					return 200f;
				}
				if (nVessels == 6)
				{
					return 230f;
				}
				break;
			}
			return 150f;
		}

		private void SetCircleColor(FleetType iType)
		{
			Color color = (iType != FleetType.Friend) ? new Color(KCVColor.ColorRate(238f), KCVColor.ColorRate(35f), KCVColor.ColorRate(36f)) : new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(255f), KCVColor.ColorRate(255f));
			this._mrBufferCircle.get_sharedMaterial().set_color(color);
			this._cBaseColor = color;
		}
	}
}
