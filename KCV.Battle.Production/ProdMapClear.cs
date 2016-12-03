using KCV.Battle.Utils;
using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapClear : BaseAnimation
	{
		[SerializeField]
		private ParticleSystem _uiParticle;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private Animation _anime;

		private int _debugIndex;

		private bool _debugDamage;

		private bool _isControl;

		private KeyControl _keyControl;

		private ShipModel_BattleAll _ship;

		private void _init()
		{
			this._debugIndex = 0;
			this._debugDamage = false;
			this._isControl = false;
			this._isFinished = false;
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticle, base.get_transform(), "Particle");
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "ShipObj/Ship");
			if (this._anime == null)
			{
				this._anime = base.GetComponent<Animation>();
			}
			this._uiParticle.SetActive(false);
			this._anime.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiParticle);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del<Animation>(ref this._anime);
			this._keyControl = null;
			this._ship = null;
		}

		public bool Run()
		{
			if (this._isControl && this._keyControl != null && this._keyControl.keyState.get_Item(1).down)
			{
				this.onAnimationComp();
			}
			return this._isFinished;
		}

		private void setShipTexture()
		{
			if (this._ship == null)
			{
				return;
			}
			GameObject gameObject = base.get_transform().FindChild("ShipObj").get_gameObject();
			ShipModel_BattleResult model = (ShipModel_BattleResult)this._ship;
			float lovScaleMagnification = SortieBattleUtils.GetLovScaleMagnification(model);
			this._uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._ship.GetGraphicsMstId(), false);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
			this._uiShip.get_transform().set_localScale(new Vector3(lovScaleMagnification, lovScaleMagnification, 1f));
			float num = (lovScaleMagnification - 1f) * 120f;
			gameObject.get_transform().set_localPosition(new Vector3(1f, 140f - num, 1f));
		}

		private void debugShipTexture()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this._debugIndex))
			{
				GameObject gameObject = base.get_transform().FindChild("ShipObj").get_gameObject();
				float num = 1.5f;
				this._uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._debugIndex, this._debugDamage);
				this._uiShip.MakePixelPerfect();
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._debugIndex).GetShipDisplayCenter(this._debugDamage)));
				this._uiShip.get_transform().set_localScale(new Vector3(num, num, 1f));
				float num2 = (num - 1f) * 120f;
				gameObject.get_transform().set_localPosition(new Vector3(1f, 140f - num2, 1f));
			}
		}

		public override void Play(Action callback)
		{
			this._actCallback = callback;
			if (this._ship == null)
			{
				this.onAnimationComp();
			}
			this._animAnimation.Stop();
			this._animAnimation.Play("MapClear");
			this._uiParticle.SetActive(true);
			this._uiParticle.Stop();
			this._uiParticle.Play();
		}

		private void startControl()
		{
			this._isControl = true;
		}

		private void _playAnimationSE(int num)
		{
			if (num != 0)
			{
				if (num == 1)
				{
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_053);
				}
			}
			else
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.ClearAA);
			}
		}

		private void onAnimationComp()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this._isFinished = true;
		}

		public static ProdMapClear Instantiate(ProdMapClear prefab, Transform parent, KeyControl keyControl, ShipModel_BattleAll ship, int nPanelDepth)
		{
			ProdMapClear prodMapClear = Object.Instantiate<ProdMapClear>(prefab);
			prodMapClear.get_transform().set_parent(parent);
			prodMapClear.get_transform().set_localScale(Vector3.get_one());
			prodMapClear.get_transform().set_localPosition(Vector3.get_zero());
			prodMapClear._init();
			prodMapClear._keyControl = keyControl;
			prodMapClear._ship = ship;
			prodMapClear.setShipTexture();
			return prodMapClear;
		}
	}
}
