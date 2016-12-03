using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionFleetShipsList : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabRebellionShipBanner;

		[SerializeField]
		private float _fStartOffs = -850f;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(1000f, 201f, 0f);

		private List<UIRebellionOrgaizeShipBanner> _listShipBanners;

		private List<Vector3> _listBannerPos;

		public UIPanel panel
		{
			get
			{
				return base.GetComponent<UIPanel>();
			}
		}

		public static UIRebellionFleetShipsList Instantiate(UIRebellionFleetShipsList prefab, Transform parent)
		{
			UIRebellionFleetShipsList uIRebellionFleetShipsList = Object.Instantiate<UIRebellionFleetShipsList>(prefab);
			uIRebellionFleetShipsList.get_transform().set_parent(parent);
			uIRebellionFleetShipsList.get_transform().localPositionZero();
			uIRebellionFleetShipsList.get_transform().localScaleOne();
			return uIRebellionFleetShipsList;
		}

		private void Awake()
		{
			base.get_transform().set_localPosition(this._vOriginPos);
			List<Vector3> list = new List<Vector3>();
			list.Add(new Vector3(-337f, 66f, 0f));
			list.Add(new Vector3(-1f, 66f, 0f));
			list.Add(new Vector3(-337f, -64f, 0f));
			list.Add(new Vector3(-1f, -64f, 0f));
			list.Add(new Vector3(-337f, -194f, 0f));
			list.Add(new Vector3(-1f, -194f, 0f));
			this._listBannerPos = list;
			Observable.FromCoroutine(new Func<IEnumerator>(this.CreaetShipBanner), false).Subscribe<Unit>().AddTo(base.get_gameObject());
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabRebellionShipBanner);
			Mem.Del<float>(ref this._fStartOffs);
			Mem.Del<Vector3>(ref this._vOriginPos);
			Mem.DelListSafe<UIRebellionOrgaizeShipBanner>(ref this._listShipBanners);
			Mem.DelListSafe<Vector3>(ref this._listBannerPos);
		}

		public bool Init(DeckModel detailDeck)
		{
			List<ShipModel> list = new List<ShipModel>(detailDeck.GetShips(this._listShipBanners.get_Count()));
			int cnt = 0;
			list.ForEach(delegate(ShipModel x)
			{
				if (x != null)
				{
					this._listShipBanners.get_Item(cnt).SetShipData(x, detailDeck.GetShipIndex(x.MemId) + 1);
				}
				else
				{
					this._listShipBanners.get_Item(cnt).SetShipData(x, -1);
				}
				cnt++;
			});
			return false;
		}

		public void Show(Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			this._listShipBanners.ForEach(delegate(UIRebellionOrgaizeShipBanner x)
			{
				Action onComplete = null;
				if (x.index == this._listShipBanners.get_Count() - 1)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate(long _)
						{
							Dlg.Call(ref onFinished);
							this.panel.widgetsAreStatic = true;
						});
					};
				}
				Vector3 to = this._listBannerPos.get_Item(x.index);
				to.x += this._fStartOffs;
				x.get_transform().LTMoveLocal(to, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setDelay((float)x.index * 0.03f).setOnComplete(onComplete);
			});
		}

		public void Hide(Action onFinished)
		{
			this.panel.widgetsAreStatic = false;
			this._listShipBanners.ForEach(delegate(UIRebellionOrgaizeShipBanner x)
			{
				Action onComplete = null;
				if (x.index == this._listShipBanners.get_Count() - 1)
				{
					onComplete = delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(0.029999999329447746)).Subscribe(delegate(long _)
						{
							Dlg.Call(ref onFinished);
							this.panel.widgetsAreStatic = true;
						});
					};
				}
				x.get_transform().LTMoveLocal(this._listBannerPos.get_Item(x.index), 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setDelay((float)x.index * 0.03f).setOnComplete(onComplete);
			});
		}

		[DebuggerHidden]
		private IEnumerator CreaetShipBanner()
		{
			UIRebellionFleetShipsList.<CreaetShipBanner>c__Iterator16C <CreaetShipBanner>c__Iterator16C = new UIRebellionFleetShipsList.<CreaetShipBanner>c__Iterator16C();
			<CreaetShipBanner>c__Iterator16C.<>f__this = this;
			return <CreaetShipBanner>c__Iterator16C;
		}
	}
}
