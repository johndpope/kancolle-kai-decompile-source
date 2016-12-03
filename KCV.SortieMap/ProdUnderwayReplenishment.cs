using Common.Struct;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdUnderwayReplenishment : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showTime;

			public LeanTweenType showEaseType;

			public float hideTime;

			public LeanTweenType hideEaseType;

			public Vector3 showFleetOilerPos;

			public Vector3 hideFleetOilerPos;

			public Vector3 showTargetShipPos;

			public Vector3 hideTargetShipPos;

			public Vector3 startOilerTargetParticlePos;

			public Vector3 endOilerTargetParticlePos;

			public float oilerTargetParticleMoveTime;

			public void Dispose()
			{
				Mem.Del<float>(ref this.showTime);
				Mem.Del<LeanTweenType>(ref this.showEaseType);
				Mem.Del<float>(ref this.hideTime);
				Mem.Del<LeanTweenType>(ref this.hideEaseType);
				Mem.Del<Vector3>(ref this.showFleetOilerPos);
				Mem.Del<Vector3>(ref this.hideFleetOilerPos);
				Mem.Del<Vector3>(ref this.showTargetShipPos);
				Mem.Del<Vector3>(ref this.hideTargetShipPos);
				Mem.Del<Vector3>(ref this.startOilerTargetParticlePos);
				Mem.Del<Vector3>(ref this.endOilerTargetParticlePos);
				Mem.Del<float>(ref this.oilerTargetParticleMoveTime);
			}
		}

		[SerializeField]
		private List<UITexture> _listShips;

		[SerializeField]
		private ParticleSystem _psFleetOilerMove;

		[SerializeField]
		private ParticleSystem _psOilerTargetHeal;

		[Header("[Animation Parameter]"), SerializeField]
		private ProdUnderwayReplenishment.Params _strParams = default(ProdUnderwayReplenishment.Params);

		private List<Tuple<UITexture, ShipModel>> _listShipInfos;

		private UIPanel _uiPanel;

		private bool _isPlaying;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isPlaying;
			}
			private set
			{
				this._isPlaying = value;
			}
		}

		public static ProdUnderwayReplenishment Instantiate(ProdUnderwayReplenishment prefab, Transform parent, MapSupplyModel model)
		{
			ProdUnderwayReplenishment prodUnderwayReplenishment = Object.Instantiate<ProdUnderwayReplenishment>(prefab);
			prodUnderwayReplenishment.get_transform().set_parent(parent);
			prodUnderwayReplenishment.get_transform().localScaleOne();
			prodUnderwayReplenishment.get_transform().localPositionZero();
			prodUnderwayReplenishment.Init(model);
			return prodUnderwayReplenishment;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<UITexture>(ref this._listShips);
			Mem.DelComponentSafe<ParticleSystem>(ref this._psFleetOilerMove);
			Mem.DelComponentSafe<ParticleSystem>(ref this._psOilerTargetHeal);
			Mem.DelIDisposableSafe<ProdUnderwayReplenishment.Params>(ref this._strParams);
			if (this._listShipInfos != null)
			{
				this._listShipInfos.ForEach(delegate(Tuple<UITexture, ShipModel> x)
				{
				});
			}
			Mem.DelListSafe<Tuple<UITexture, ShipModel>>(ref this._listShipInfos);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		private bool Init(MapSupplyModel model)
		{
			this.isPlaying = false;
			this._listShipInfos = new List<Tuple<UITexture, ShipModel>>(2);
			this.SetShipTexture(model.Ship, model.GivenShips);
			this.InitParticle();
			this.panel.widgetsAreStatic = true;
			return true;
		}

		private void SetShipTexture(ShipModel fleetOiler, List<ShipModel> targetShips)
		{
			this._listShips.get_Item(0).mainTexture = ShipUtils.LoadTexture(fleetOiler);
			this._listShips.get_Item(0).MakePixelPerfect();
			Point cutinSp1_InBattle = fleetOiler.Offsets.GetCutinSp1_InBattle(fleetOiler.IsDamaged());
			this._listShips.get_Item(0).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle.x, (float)cutinSp1_InBattle.y, 0f));
			this._listShips.get_Item(0).get_transform().get_parent().set_localPosition(this._strParams.hideFleetOilerPos);
			this._listShipInfos.Add(new Tuple<UITexture, ShipModel>(this._listShips.get_Item(0), fleetOiler));
			this._listShips.get_Item(1).mainTexture = ShipUtils.LoadTexture(targetShips.get_Item(0));
			this._listShips.get_Item(1).MakePixelPerfect();
			Point cutinSp1_InBattle2 = targetShips.get_Item(0).Offsets.GetCutinSp1_InBattle(targetShips.get_Item(0).IsDamaged());
			this._listShips.get_Item(1).get_transform().set_localPosition(new Vector3((float)cutinSp1_InBattle2.x, (float)cutinSp1_InBattle2.y, 0f));
			this._listShips.get_Item(1).get_transform().get_parent().set_localPosition(this._strParams.hideTargetShipPos);
			this._listShipInfos.Add(new Tuple<UITexture, ShipModel>(this._listShips.get_Item(1), targetShips.get_Item(0)));
			this._listShipInfos.ForEach(delegate(Tuple<UITexture, ShipModel> x)
			{
				x.get_Item1().alpha = 0f;
			});
		}

		private void InitParticle()
		{
			this._psFleetOilerMove.get_transform().set_localPosition(new Vector3(0f, 150f, 0f));
			this._psFleetOilerMove.SetActive(false);
			this._psOilerTargetHeal.get_transform().set_localPosition(this._strParams.startOilerTargetParticlePos);
			this._psOilerTargetHeal.SetActive(false);
		}

		public IObservable<bool> Play()
		{
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.AnimationObserver(observer));
		}

		[DebuggerHidden]
		private IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdUnderwayReplenishment.<AnimationObserver>c__Iterator121 <AnimationObserver>c__Iterator = new ProdUnderwayReplenishment.<AnimationObserver>c__Iterator121();
			<AnimationObserver>c__Iterator.observer = observer;
			<AnimationObserver>c__Iterator.<$>observer = observer;
			<AnimationObserver>c__Iterator.<>f__this = this;
			return <AnimationObserver>c__Iterator;
		}

		private LTDescr Hide(Tuple<UITexture, ShipModel> target, Vector3 vHidePos)
		{
			target.get_Item1().get_transform().LTValue(target.get_Item1().alpha, 0f, this._strParams.hideTime).setEase(this._strParams.hideEaseType).setOnUpdate(delegate(float x)
			{
				target.get_Item1().alpha = x;
			});
			return target.get_Item1().get_transform().get_parent().LTMoveLocalX(vHidePos.x, this._strParams.hideTime).setEase(this._strParams.hideEaseType);
		}
	}
}
