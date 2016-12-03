using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(iTweenPath))]
	public class ProdAircraftMove : MonoBehaviour
	{
		[SerializeField]
		private float _fLength = 0.4f;

		[SerializeField]
		private UISprite _uiAircraft;

		private UISortieShip.Direction _iStartDirection;

		private bool _isTurn;

		private bool _isScale;

		private iTweenPath _tpTweenPath;

		private Action _actOnFinished;

		private LTSpline _clsSpline;

		private float _fPositionPercent;

		private UIPanel _uiPanel;

		private iTweenPath tweenPath
		{
			get
			{
				return this.GetComponentThis(ref this._tpTweenPath);
			}
		}

		private UISprite uiAircraft
		{
			get
			{
				return this._uiAircraft;
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdAircraftMove Instantiate(ProdAircraftMove prefab, Transform parent, int depth)
		{
			ProdAircraftMove prodAircraftMove = Object.Instantiate<ProdAircraftMove>(prefab);
			prodAircraftMove.get_transform().set_parent(parent);
			prodAircraftMove.get_transform().localScaleOne();
			prodAircraftMove.get_transform().localPositionZero();
			prodAircraftMove.Init(depth);
			return prodAircraftMove;
		}

		private bool Init(int nDepth)
		{
			if (!iTweenPath.paths.ContainsKey("SeachBossCell"))
			{
				base.GetComponent<iTweenPath>().set_enabled(false);
			}
			this.panel.depth = nDepth;
			this._uiAircraft.get_transform().localPositionZero();
			this._uiAircraft.get_transform().localScaleZero();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<float>(ref this._fPositionPercent);
			Mem.Del<float>(ref this._fLength);
			Mem.Del<bool>(ref this._isTurn);
			Mem.Del<bool>(ref this._isScale);
			Mem.Del<iTweenPath>(ref this._tpTweenPath);
			Mem.Del<Action>(ref this._actOnFinished);
		}

		public void Move(Vector3 fromCell, Vector3 targetCell, Action onFinished)
		{
			this.SetAircraft(MapAirReconnaissanceKind.WarterPlane);
			this.tweenPath.nodes.set_Item(0, fromCell);
			this.tweenPath.nodes.set_Item(2, targetCell);
			this._uiAircraft.get_transform().set_position(fromCell);
			this._actOnFinished = onFinished;
			this.CalcRoutePoint();
			this.CalcDirection(fromCell, targetCell);
			this._isTurn = false;
			this._isScale = false;
			this._uiAircraft.get_transform().localScaleZero();
			this._uiAircraft.get_transform().LTScale(Vector2.get_one(), 0.5f);
			base.get_transform().LTValue(0f, 1f, 3f).setOnUpdate(new Action<float>(this.UpdateHandler)).setOnComplete(new Action(this.OnCompleteHandler));
		}

		public void Move(Vector3 from, Vector3 to, MapAirReconnaissanceKind iKind, Action onFinished)
		{
			this.SetAircraft(iKind);
			this.tweenPath.nodes.set_Item(0, from);
			this.tweenPath.nodes.set_Item(2, to);
			this._uiAircraft.get_transform().set_position(from);
			this._actOnFinished = onFinished;
			this.CalcRoutePoint();
			this.CalcDirection(from, to);
			this._isTurn = false;
			this._isScale = false;
			this._uiAircraft.get_transform().localScaleZero();
			this._uiAircraft.get_transform().LTScale(Vector2.get_one(), 0.5f);
			base.get_transform().LTValue(0f, 1f, 3f).setOnUpdate(new Action<float>(this.UpdateHandler)).setOnComplete(new Action(this.OnCompleteHandler));
		}

		private void UpdateHandler(float value)
		{
			this._fPositionPercent = value;
			this._uiAircraft.get_transform().set_position(iTween.PointOnPath(this.tweenPath.nodes.ToArray(), this._fPositionPercent));
			if (0.3f < this._fPositionPercent && !this._isTurn)
			{
				this._isTurn = true;
				float to = (this._iStartDirection != UISortieShip.Direction.Right) ? 0f : -180f;
				this._uiAircraft.get_transform().LTRotateY(to, 0.8f);
			}
			if (0.8f < this._fPositionPercent && !this._isScale)
			{
				this._isScale = true;
				this._uiAircraft.get_transform().LTScale(Vector3.get_zero(), 0.5f);
			}
		}

		private void OnCompleteHandler()
		{
			Dlg.Call(ref this._actOnFinished);
			Object.Destroy(base.get_gameObject());
		}

		private void CalcRoutePoint()
		{
			Vector3[] array = MiscCalculateUtils.CalculateBezierPoint(this.tweenPath.nodes.get_Item(0), this.tweenPath.nodes.get_Item(2), this._fLength);
			this.tweenPath.nodes.set_Item(1, array[0]);
			this.tweenPath.nodes.set_Item(3, array[1]);
			this.tweenPath.nodes.set_Item(4, this.tweenPath.nodes.get_Item(0));
		}

		private void CalcDirection(Vector3 from, Vector3 to)
		{
			int num = Math.Sign(Mathe.Direction(from, to).x);
			if (num == -1)
			{
				this._iStartDirection = UISortieShip.Direction.Left;
				this._uiAircraft.get_transform().localEulerAnglesY(180f);
			}
			else if (num == 1)
			{
				this._iStartDirection = UISortieShip.Direction.Right;
				this._uiAircraft.get_transform().localEulerAnglesY(0f);
			}
		}

		private void SetAircraft(MapAirReconnaissanceKind iKind)
		{
			if (iKind != MapAirReconnaissanceKind.LargePlane)
			{
				if (iKind != MapAirReconnaissanceKind.WarterPlane)
				{
					this.uiAircraft.spriteName = string.Empty;
				}
				else
				{
					this.uiAircraft.spriteName = "icon_WaterPlane";
					this.uiAircraft.MakePixelPerfect();
				}
			}
			else
			{
				this.uiAircraft.spriteName = "icon_LargePlane";
				this.uiAircraft.MakePixelPerfect();
			}
		}
	}
}
