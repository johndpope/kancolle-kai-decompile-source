using KCV.Battle.Utils;
using local.models;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation)), RequireComponent(typeof(UIPanel))]
	public class ProdBossInsert : BaseAnimation
	{
		private enum AnimationList
		{
			ProdBossInsertMistIn,
			ProdBossInsertMistOut
		}

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psSmoke;

		private UIPanel _uiPanel;

		private float _fVoiceLength;

		private ShipModel_BattleAll _clsShipModel;

		private UIPanel panel
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

		public ShipModel_BattleAll shipmodel
		{
			get
			{
				return this._clsShipModel;
			}
		}

		public static ProdBossInsert Instantiate(ProdBossInsert prefab, Transform parent, ShipModel_BattleAll model)
		{
			ProdBossInsert prodBossInsert = Object.Instantiate<ProdBossInsert>(prefab);
			prodBossInsert.get_transform().set_parent(parent);
			prodBossInsert.get_transform().localScaleZero();
			prodBossInsert.get_transform().localPositionZero();
			prodBossInsert.Init(model);
			return prodBossInsert;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UITexture>(ref this._uiShipTexture);
			Mem.Del(ref this._psSmoke);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<float>(ref this._fVoiceLength);
			Mem.Del<ShipModel_BattleAll>(ref this._clsShipModel);
		}

		private bool Init(ShipModel_BattleAll model)
		{
			this._clsShipModel = model;
			this._fVoiceLength = ShipUtils.GetVoiceLength(this._clsShipModel, 1);
			this._psSmoke.Stop();
			this._uiShipTexture.mainTexture = ShipUtils.LoadTexture(model, true);
			this._uiShipTexture.get_transform().set_localPosition(ShipUtils.GetShipOffsPos(this._clsShipModel, false, MstShipGraphColumn.CutInSp1));
			this._uiShipTexture.get_transform().set_localScale(Vector3.get_one() * 0.95f);
			return true;
		}

		public override void Play(Action callback)
		{
			base.get_transform().localScaleOne();
			this._psSmoke.Play();
			this._uiShipTexture.get_transform().ScaleTo(Vector3.get_one(), this._fVoiceLength, iTween.EaseType.easeOutSine, null);
			base.Play(ProdBossInsert.AnimationList.ProdBossInsertMistIn, callback);
		}

		private void PlayBossVoice()
		{
			float voiceLength = ShipUtils.GetVoiceLength(this._clsShipModel, 1);
			ShipUtils.PlayBossInsertVoice(this._clsShipModel);
			Observable.Timer(TimeSpan.FromSeconds((double)Mathe.Lerp(0f, voiceLength, 0.9f))).Subscribe(delegate(long _)
			{
				base.animation.get_Item(ProdBossInsert.AnimationList.ProdBossInsertMistOut.ToString()).set_time(base.animation.get_Item(ProdBossInsert.AnimationList.ProdBossInsertMistOut.ToString()).get_clip().get_length());
				base.animation.get_Item(ProdBossInsert.AnimationList.ProdBossInsertMistOut.ToString()).set_speed(-1f);
				this.Play(ProdBossInsert.AnimationList.ProdBossInsertMistOut, null);
			});
		}

		protected override void onAnimationFinishedAfterDiscard()
		{
			base.onAnimationFinishedAfterDiscard();
		}
	}
}
