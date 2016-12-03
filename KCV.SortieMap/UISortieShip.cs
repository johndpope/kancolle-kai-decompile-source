using Common.Enum;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UISortieShip : MonoBehaviour
	{
		public enum Direction
		{
			Left,
			Right,
			Same
		}

		private const int SPEED = 100;

		[SerializeField]
		private Transform _prefabProdExclamationPoint;

		[SerializeField]
		private Transform _prefabEventItem;

		[SerializeField]
		private Transform _prefabEventAircraftMove;

		[SerializeField]
		private Transform _prefabProdBalloon;

		[SerializeField]
		private Transform _prefabProdCommentBalloon;

		[SerializeField]
		private UISprite _uiShipSprite;

		[SerializeField]
		private UISprite _uiInputIcon;

		private UISortieShip.Direction _iDirection = UISortieShip.Direction.Right;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static UISortieShip Instantiate(UISortieShip prefab, Transform parent, UISortieShip.Direction iDirection)
		{
			UISortieShip uISortieShip = Object.Instantiate<UISortieShip>(prefab);
			uISortieShip.get_transform().set_parent(parent);
			uISortieShip.get_transform().localPositionZero();
			uISortieShip.get_transform().localScaleOne();
			return uISortieShip.VirtualCtor(iDirection);
		}

		private UISortieShip VirtualCtor(UISortieShip.Direction iDirection)
		{
			this._uiInputIcon.alpha = 0f;
			this._iDirection = iDirection;
			Vector3 vector = (this._iDirection != UISortieShip.Direction.Left) ? Vector3.get_zero() : (Vector3.get_down() * 180f);
			base.get_transform().set_localRotation(Quaternion.Euler(vector));
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabProdExclamationPoint);
			Mem.Del<Transform>(ref this._prefabEventItem);
			Mem.Del<Transform>(ref this._prefabEventAircraftMove);
			Mem.Del<Transform>(ref this._prefabProdBalloon);
			Mem.Del<Transform>(ref this._prefabProdCommentBalloon);
			Mem.Del(ref this._uiShipSprite);
			Mem.Del(ref this._uiInputIcon);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public void Move(UISortieMapCell NextCell, Action onFinishedAnimation)
		{
			Vector3 localPosition = NextCell.get_transform().get_localPosition();
			this.ChangeDirection(this.CalcDirection(base.get_transform().get_localPosition(), localPosition));
			base.get_transform().LTMoveLocal(localPosition, Math.Abs(Vector3.Distance(base.get_transform().get_localPosition(), localPosition)) / 100f).setEase(LeanTweenType.linear).setOnComplete(delegate
			{
				Dlg.Call(ref onFinishedAnimation);
			});
		}

		public void PlayGetMaterialOrItem(MapEventItemModel mapEventItemModel, Action onFinished)
		{
			ProdItem prodItem = ProdItem.Instantiate(this._prefabEventItem.GetComponent<ProdItem>(), base.get_transform(), mapEventItemModel);
			prodItem.PlayGetAnim(onFinished);
		}

		public void PlayLostMaterial(MapEventHappeningModel mapEventHappeningModel, Action onFinished)
		{
			ProdItem prodItem = ProdItem.Instantiate(this._prefabEventItem.GetComponent<ProdItem>(), base.get_transform(), mapEventHappeningModel);
			prodItem.PlayLostAnim(onFinished);
		}

		public IObservable<bool> PlayExclamationPoint()
		{
			return Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayExclamationPointAnimationObserver(observer));
		}

		[DebuggerHidden]
		private IEnumerator PlayExclamationPointAnimationObserver(IObserver<bool> observer)
		{
			UISortieShip.<PlayExclamationPointAnimationObserver>c__Iterator131 <PlayExclamationPointAnimationObserver>c__Iterator = new UISortieShip.<PlayExclamationPointAnimationObserver>c__Iterator131();
			<PlayExclamationPointAnimationObserver>c__Iterator.observer = observer;
			<PlayExclamationPointAnimationObserver>c__Iterator.<$>observer = observer;
			<PlayExclamationPointAnimationObserver>c__Iterator.<>f__this = this;
			return <PlayExclamationPointAnimationObserver>c__Iterator;
		}

		public void PlayDetectionAircraft(UISortieMapCell fromCell, UISortieMapCell toCell, Action onFinished)
		{
			ProdAircraftMove prodAircraftMove = ProdAircraftMove.Instantiate(this._prefabEventAircraftMove.GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().get_transform(), this.panel.depth + 1);
			prodAircraftMove.Move(fromCell.get_transform().get_position(), toCell.get_transform().get_position(), onFinished);
		}

		public void PlayAirReconnaissance(MapAirReconnaissanceKind iKind, Transform from, Transform airRecPoint, Action onFinished)
		{
			switch (iKind)
			{
			case MapAirReconnaissanceKind.Impossible:
				Observable.Timer(TimeSpan.FromSeconds(1.2000000476837158)).Subscribe(delegate(long _)
				{
					Dlg.Call(ref onFinished);
				});
				break;
			case MapAirReconnaissanceKind.LargePlane:
			{
				ProdAircraftMove prodAircraftMove = ProdAircraftMove.Instantiate(this._prefabEventAircraftMove.GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().get_transform(), this.panel.depth + 1);
				prodAircraftMove.Move(from.get_position(), airRecPoint.get_position(), MapAirReconnaissanceKind.LargePlane, onFinished);
				break;
			}
			case MapAirReconnaissanceKind.WarterPlane:
			{
				ProdAircraftMove prodAircraftMove2 = ProdAircraftMove.Instantiate(this._prefabEventAircraftMove.GetComponent<ProdAircraftMove>(), SortieMapTaskManager.GetUIMapManager().get_transform(), this.panel.depth + 1);
				prodAircraftMove2.Move(from.get_position(), airRecPoint.get_position(), MapAirReconnaissanceKind.WarterPlane, onFinished);
				break;
			}
			}
		}

		public void PlayBalloon(enumMapEventType iEventType, enumMapWarType iWarType, Action onFinished)
		{
			ProdBalloon balloon = ProdBalloon.Instantiate(this._prefabProdBalloon.GetComponent<ProdBalloon>(), base.get_transform(), this._iDirection, iEventType, iWarType);
			balloon.depth = this._uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(balloon.get_gameObject());
				Mem.Del<ProdBalloon>(ref balloon);
			});
		}

		public void PlayBalloon(MapEventItemModel model, Action onFinished)
		{
			ProdBalloon balloon = ProdBalloon.Instantiate(this._prefabProdBalloon.GetComponent<ProdBalloon>(), base.get_transform(), this._iDirection, model);
			balloon.depth = this._uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(balloon.get_gameObject());
				Mem.Del<ProdBalloon>(ref balloon);
			});
		}

		public void PlayBalloon(MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel, Action onFinished)
		{
			if (eventAirRecModel.AircraftType == MapAirReconnaissanceKind.Impossible)
			{
				Dlg.Call(ref onFinished);
				return;
			}
			ProdBalloon balloon = ProdBalloon.Instantiate(this._prefabProdBalloon.GetComponent<ProdBalloon>(), base.get_transform(), this._iDirection, eventAirRecModel, eventItemModel);
			balloon.depth = this._uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(balloon.get_gameObject());
				Mem.Del<ProdBalloon>(ref balloon);
			});
		}

		public void PlayBalloon(MapCommentKind iKind, Action onFinished)
		{
			if (iKind == MapCommentKind.None)
			{
				Dlg.Call(ref onFinished);
				return;
			}
			ProdCommentBalloon balloon = ProdCommentBalloon.Instantiate(this._prefabProdCommentBalloon.GetComponent<ProdCommentBalloon>(), base.get_transform(), this._iDirection, iKind);
			balloon.sprite.depth = this._uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(balloon.get_gameObject());
				Mem.Del<ProdCommentBalloon>(ref balloon);
			});
		}

		public void PlayBalloon(Action onFinished)
		{
			ProdCommentBalloon balloon = ProdCommentBalloon.Instantiate(this._prefabProdCommentBalloon.GetComponent<ProdCommentBalloon>(), base.get_transform(), this._iDirection);
			balloon.sprite.depth = this._uiShipSprite.depth + 1;
			balloon.ShowHide().setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(balloon.get_gameObject());
				Mem.Del<ProdCommentBalloon>(ref balloon);
			});
		}

		public LTDescr ShowInputIcon()
		{
			return this._uiInputIcon.get_transform().LTValue(this._uiInputIcon.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiInputIcon.alpha = x;
			});
		}

		public LTDescr HideInputIcon()
		{
			return this._uiInputIcon.get_transform().LTValue(this._uiInputIcon.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiInputIcon.alpha = x;
			});
		}

		private UISortieShip.Direction CalcDirection(Vector3 from, Vector3 to)
		{
			if (from.x < to.x)
			{
				return UISortieShip.Direction.Right;
			}
			if (to.x < from.x)
			{
				return UISortieShip.Direction.Left;
			}
			return UISortieShip.Direction.Same;
		}

		private void ChangeDirection(UISortieShip.Direction direction)
		{
			if (this._iDirection == direction)
			{
				return;
			}
			this._iDirection = direction;
			float time = 0.5f;
			UISortieShip.Direction iDirection = this._iDirection;
			if (iDirection != UISortieShip.Direction.Left)
			{
				if (iDirection == UISortieShip.Direction.Right)
				{
					base.get_transform().LTRotateLocal(Vector3.get_zero(), time);
				}
			}
			else
			{
				base.get_transform().LTRotateLocal(Vector3.get_down() * 180f, time);
			}
		}
	}
}
