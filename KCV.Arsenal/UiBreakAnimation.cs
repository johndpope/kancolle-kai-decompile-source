using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiBreakAnimation : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _parShipBreak;

		[SerializeField]
		private ParticleSystem _parShipBreak2;

		[SerializeField]
		private UiBreakMaterialIcon _breakMaterial;

		[SerializeField]
		private Animation _anim;

		public bool isAnimationPlayng;

		public void init()
		{
			this._breakMaterial = base.get_transform().FindChild("Material1").GetComponent<UiBreakMaterialIcon>();
			this._breakMaterial.init();
			this._parShipBreak = base.get_transform().FindChild("Smoke").GetComponent<ParticleSystem>();
			this._parShipBreak2 = base.get_transform().FindChild("Smoke2").GetComponent<ParticleSystem>();
			this._parShipBreak.SetActive(false);
			this._parShipBreak2.SetActive(false);
			this._anim = base.GetComponent<Animation>();
			this.changeItems();
			this._anim.Stop();
		}

		public void startAnimation()
		{
			this._anim.Stop();
			this._anim.Play("ShipBreak");
			SoundUtils.PlaySE(SEFIleInfos.SE_015);
			this._breakMaterial.startAnimation();
			this._parShipBreak.SetActive(true);
			this._parShipBreak.set_time(0f);
			this._parShipBreak.Play();
			this.isAnimationPlayng = true;
		}

		public void compShipBreakAnimation()
		{
			this._parShipBreak.Stop();
			this._parShipBreak.set_time(0f);
			this._parShipBreak.SetActive(false);
			this._anim.Stop();
			this._breakMaterial.endAnimation();
			this.isAnimationPlayng = false;
			ArsenalTaskManager._clsList.compBreakAnimation();
		}

		public void startItemAnimation()
		{
			this._anim.Stop();
			this._anim.Play("ItemBreak");
			SoundUtils.PlaySE(SEFIleInfos.SE_015);
			this._breakMaterial.startAnimation();
			this._parShipBreak2.SetActive(true);
			this._parShipBreak2.set_time(0f);
			this._parShipBreak2.Play();
			this.isAnimationPlayng = true;
		}

		public void compItemBreakAnimation()
		{
			this._parShipBreak2.Stop();
			this._parShipBreak2.set_time(0f);
			this._parShipBreak2.SetActive(false);
			this.changeItems();
			this._anim.Stop();
			this._breakMaterial.endAnimation();
			this.isAnimationPlayng = false;
			ArsenalTaskManager._clsList.compBreakAnimation();
		}

		public void changeItems()
		{
			this._breakMaterial.GetComponent<UISprite>().spriteName = "icon2_m" + Random.Range(1, 5);
		}

		private void OnDestroy()
		{
			this._parShipBreak = null;
			this._parShipBreak2 = null;
			this._breakMaterial = null;
			this._anim = null;
		}
	}
}
