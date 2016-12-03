using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdMailstrom : MonoBehaviour
	{
		private UISprite _uiMailstrom;

		private Action _actOnFinished;

		private MapEventHappeningModel _clsEventHappeningModel;

		private UISprite mailstrom
		{
			get
			{
				return this.GetComponentThis(ref this._uiMailstrom);
			}
		}

		public static ProdMailstrom Instantiate(ProdMailstrom prefab, Transform parent, MapEventHappeningModel eventHappeningModel)
		{
			ProdMailstrom prodMailstrom = Object.Instantiate<ProdMailstrom>(prefab);
			prodMailstrom.get_transform().set_parent(parent);
			prodMailstrom.get_transform().localScaleOne();
			prodMailstrom.get_transform().localPositionZero();
			prodMailstrom.Init(eventHappeningModel);
			return prodMailstrom;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiMailstrom);
			Mem.Del<Action>(ref this._actOnFinished);
			Mem.Del<MapEventHappeningModel>(ref this._clsEventHappeningModel);
		}

		private bool Init(MapEventHappeningModel eventHappeningModel)
		{
			this._clsEventHappeningModel = eventHappeningModel;
			return true;
		}

		public void PlayMailstrom(UISortieShip sortieShip, ProdShipRipple ripple, Action onFinished)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_033);
			this.PlayRotation();
			this.PlayShipMoveAnim(sortieShip).setOnComplete(delegate
			{
				sortieShip.PlayLostMaterial(this._clsEventHappeningModel, null);
			});
			ripple.Play(Color.get_white());
			this._actOnFinished = onFinished;
			Observable.Timer(TimeSpan.FromSeconds(4.5)).Subscribe(delegate(long _)
			{
				this.OnFinished(ripple);
			});
		}

		private void PlayRotation()
		{
			base.get_transform().LTRotateAroundLocal(Vector3.get_back(), 1080f, 6f).setEase(LeanTweenType.easeOutQuad).setLoopClamp();
		}

		private LTDescr PlayShipMoveAnim(UISortieShip sortieShip)
		{
			Vector3 originPos = sortieShip.get_transform().get_localPosition();
			float rotCnt = 3f;
			return base.get_transform().LTValue(0f, rotCnt, 3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				float num = (x >= rotCnt / 2f) ? (30f * (rotCnt - x)) : (30f * x);
				Vector3 vector = new Vector2(num * Mathf.Sin(x % 1f * 3.14159274f * 2f), num * Mathf.Cos(x % 1f * 3.14159274f * 2f));
				sortieShip.get_transform().set_localPosition(vector + originPos);
			});
		}

		private void OnFinished(ProdShipRipple ripple)
		{
			ripple.Stop();
			base.get_transform().LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.mailstrom.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref this._actOnFinished);
				Object.Destroy(base.get_gameObject());
			});
		}
	}
}
