using KCV.Battle.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(Animation))]
	public class ProdDeathCry : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private Transform _traShipAnchor;

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psStar;

		[SerializeField]
		private ParticleSystem _psSmoke;

		private float _fVoiceLength;

		private UIPanel _uiPanel;

		private ShipModel_BattleAll _clsShipModel;

		public UIPanel panel
		{
			get
			{
				return this._uiPanel;
			}
		}

		public static ProdDeathCry Instantiate(ProdDeathCry prefab, Transform parent, ShipModel_BattleAll model)
		{
			ProdDeathCry prodDeathCry = Object.Instantiate<ProdDeathCry>(prefab);
			prodDeathCry.get_transform().set_parent(parent);
			prodDeathCry.get_transform().localPositionZero();
			prodDeathCry.get_transform().localScaleZero();
			prodDeathCry.Init(model);
			return prodDeathCry;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiForeground);
			Mem.Del<Transform>(ref this._traShipAnchor);
			Mem.Del<UITexture>(ref this._uiShipTexture);
			Mem.Del(ref this._psStar);
			Mem.Del(ref this._psSmoke);
			Mem.Del<float>(ref this._fVoiceLength);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<ShipModel_BattleAll>(ref this._clsShipModel);
		}

		private bool Init(ShipModel_BattleAll model)
		{
			this._clsShipModel = model;
			this._fVoiceLength = ShipUtils.GetVoiceLength(this._clsShipModel, 22);
			this._uiShipTexture.mainTexture = ShipUtils.LoadTexture(model, true);
			this._uiShipTexture.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(this._clsShipModel, false, MstShipGraphColumn.CutInSp1));
			this._psStar.Stop();
			this._psSmoke.Stop();
			return true;
		}

		public override void Play(Action callback)
		{
			this._traShipAnchor.get_transform().localScaleOne();
			float num = Mathe.Lerp(0f, this._fVoiceLength, 0.5f);
			this._psStar.Play();
			this._psSmoke.Play();
			ShipUtils.PlayBossDeathCryVoice(this._clsShipModel);
			this._uiForeground.alpha = 0f;
			this._traShipAnchor.get_transform().LTScale(Vector3.get_one() * 0.8f, this._fVoiceLength).setEase(LeanTweenType.linear).setOnComplete(new Action(this.onAnimationFinished));
			this._uiForeground.get_transform().LTValue(0f, 1f, num).setDelay(num).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this._uiForeground.alpha = x;
			});
			base.Play(callback);
		}

		protected override void onAnimationFinished()
		{
			this._psStar.Stop();
			this._psSmoke.Stop();
			base.onAnimationFinished();
		}
	}
}
