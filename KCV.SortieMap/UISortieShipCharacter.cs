using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UISortieShipCharacter : BaseUISortieBattleShip<ShipModel>
	{
		[Serializable]
		private struct AnimParams
		{
			public Vector3 showPos;

			public Vector3 hidePos;

			public float animationTime;
		}

		[Header("[Animation Parameter]"), SerializeField]
		private UISortieShipCharacter.AnimParams _strAnimParams;

		private int _nDefaultDepth;

		private bool _isInDisplay;

		public bool isInDisplay
		{
			get
			{
				return this._isInDisplay;
			}
		}

		private void Awake()
		{
			this._nDefaultDepth = this.panel.depth;
			this._isInDisplay = false;
		}

		public void SetShipData(ShipModel model)
		{
			base.SetShipTexture(model);
			this._uiShipTex.get_transform().localPosition(Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged())));
		}

		public void DrawDefault()
		{
			this._uiShipTex.alpha = 1f;
			this._uiShipTex.get_transform().localPosition(Util.Poi2Vec(this.shipModel.Offsets.GetCutinSp1_InBattle(this.shipModel.IsDamaged())));
			this._isInDisplay = true;
		}

		public void SetInDisplayNextMove(bool isInDisplay)
		{
			this._isInDisplay = isInDisplay;
		}

		public void Show(Action callback)
		{
			this._isInDisplay = true;
			this.panel.depth = this._nDefaultDepth;
			this.ShowAnimation(callback);
		}

		public void ShowInItemGet(Action onFinished)
		{
			Observable.Timer(TimeSpan.FromSeconds(0.75)).Subscribe(delegate(long _)
			{
				ShipUtils.PlayShipVoice(this.shipModel, 26);
			});
			this.HideDelayAfterDisplay(onFinished);
		}

		public void ShowInFormation(int nPanelDepth, Action onFinished)
		{
			this.panel.depth = nPanelDepth;
			this._uiShipTex.get_transform().LTCancel();
			this._uiShipTex.get_transform().LTValue(this._uiShipTex.alpha, 1f, this._strAnimParams.animationTime).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				this._uiShipTex.alpha = x;
			});
			base.get_transform().set_localPosition(this._strAnimParams.hidePos);
			base.get_transform().LTMoveLocal(this._strAnimParams.showPos, this._strAnimParams.animationTime).setEase(LeanTweenType.easeOutCubic).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void ShowAnimation(Action onFinished)
		{
			base.get_transform().set_localPosition(this._strAnimParams.showPos);
			base.get_transform().LTCancel();
			base.get_transform().LTMoveLocal(this._strAnimParams.hidePos, this._strAnimParams.animationTime).setEase(LeanTweenType.linear).setDelay(0.6f);
			this._uiShipTex.get_transform().LTCancel();
			this._uiShipTex.get_transform().LTValue(this._uiShipTex.alpha, 1f, this._strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiShipTex.alpha = x;
			}).setOnComplete(delegate
			{
				this.Hide(delegate
				{
					Dlg.Call(ref onFinished);
				});
			});
		}

		public void Hide(Action callback)
		{
			base.get_transform().LTCancel();
			base.get_transform().LTMoveLocal(this._strAnimParams.hidePos, this._strAnimParams.animationTime).setEase(LeanTweenType.linear);
			this._uiShipTex.get_transform().LTCancel();
			this._uiShipTex.get_transform().LTValue(this._uiShipTex.alpha, 0f, this._strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiShipTex.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref callback);
			});
		}

		private void HideDelayAfterDisplay(Action onFinished)
		{
			float delayTime = this._strAnimParams.animationTime + 0.5f;
			float delayTime2 = this._strAnimParams.animationTime + delayTime;
			base.get_transform().LTCancel();
			base.get_transform().LTDelayedCall(delayTime2, delegate
			{
				Dlg.Call(ref onFinished);
			}).setOnStart(delegate
			{
				this.get_transform().set_localPosition(this._strAnimParams.showPos);
				this.get_transform().LTValue(this._uiShipTex.alpha, 1f, this._strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiShipTex.alpha = x;
				});
				this.get_transform().LTValue(1f, 0f, this._strAnimParams.animationTime).setDelay(delayTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiShipTex.alpha = x;
				});
				this.get_transform().LTMoveLocal(this._strAnimParams.hidePos, this._strAnimParams.animationTime).setDelay(delayTime).setEase(LeanTweenType.linear);
			});
		}
	}
}
