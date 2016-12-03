using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(UISprite))]
	public class UISortieMapCell : MonoBehaviour
	{
		private bool _isActiveBranchingTarget;

		private bool _isFocus2ActiveBranching;

		private UISprite _uiCell;

		private UISprite _uiGlowCell;

		private UISprite _uiFocusCell;

		private UIToggle _uiActiveBranchingToggle;

		private CellModel _clsCellModel;

		private ProdShipRipple _prodRipple;

		private CircleCollider2D _colCircle2D;

		private Vector3 _vOriginPos;

		public CellModel cellModel
		{
			get
			{
				return this._clsCellModel;
			}
		}

		private int colorNo
		{
			set
			{
				this._uiCell.spriteName = string.Format("mapIcon_color_{0}", value);
				this._uiCell.MakePixelPerfect();
				if (value == 5)
				{
					base.get_transform().localPositionY(this._vOriginPos.y + 5f);
				}
			}
		}

		private CircleCollider2D collider
		{
			get
			{
				return this.GetComponentThis(ref this._colCircle2D);
			}
		}

		public bool isPassedCell
		{
			set
			{
				if (this._clsCellModel == null)
				{
					return;
				}
				if (value)
				{
					this.SetPassedCellColor();
				}
				else
				{
					this.SetStartPassedState(this._clsCellModel);
				}
			}
		}

		public bool isActiveBranchingTarget
		{
			get
			{
				return this._isActiveBranchingTarget;
			}
			set
			{
				if (value)
				{
					bool flag = true;
					this.collider.set_enabled(flag);
					this._isActiveBranchingTarget = flag;
					this.SetActiveBranchingTargetCell(true);
					this.PlayActiveBranchingGlow();
				}
				else
				{
					bool flag = false;
					this.collider.set_enabled(flag);
					this._isActiveBranchingTarget = flag;
					this.SetActiveBranchingTargetCell(false);
					this.StopActiveBranchingGlow();
				}
			}
		}

		public bool isFocus2ActiveBranching
		{
			get
			{
				return this._isFocus2ActiveBranching;
			}
			set
			{
				if (value)
				{
					this._isFocus2ActiveBranching = true;
					this._uiActiveBranchingToggle.value = true;
					this._uiFocusCell.get_transform().LTCancel();
					this._uiFocusCell.get_transform().LTValue(this._uiFocusCell.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						this._uiFocusCell.alpha = x;
					});
					this._uiFocusCell.get_transform().localScaleOne();
					this._uiFocusCell.get_transform().LTScale(Vector3.get_one() * 1.15f, 0.75f).setEase(LeanTweenType.linear).setLoopClamp();
				}
				else
				{
					this._isFocus2ActiveBranching = false;
					this._uiActiveBranchingToggle.value = false;
					this._uiFocusCell.get_transform().LTCancel();
					this._uiFocusCell.get_transform().LTValue(this._uiFocusCell.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						this._uiFocusCell.alpha = x;
					});
				}
			}
		}

		public UISortieMapCell Startup()
		{
			this._vOriginPos = base.get_transform().get_localPosition();
			this.GetComponentThis(ref this._uiCell);
			this.collider.set_offset(Vector2.get_zero());
			this.collider.set_radius(30f);
			this.collider.set_enabled(false);
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiCell);
			Mem.Del(ref this._uiGlowCell);
			Mem.Del(ref this._uiFocusCell);
			Mem.Del<UIToggle>(ref this._uiActiveBranchingToggle);
			Mem.Del<CellModel>(ref this._clsCellModel);
			Mem.Del<ProdShipRipple>(ref this._prodRipple);
			Mem.Del<CircleCollider2D>(ref this._colCircle2D);
			Mem.Del<bool>(ref this._isActiveBranchingTarget);
		}

		public bool Init(CellModel cellModel)
		{
			this._clsCellModel = cellModel;
			this.SetStartPassedState(cellModel);
			return true;
		}

		public void SetPassedDefaultColor()
		{
			enumMapEventType eventType = this._clsCellModel.EventType;
			if (eventType == enumMapEventType.Stupid)
			{
				this.colorNo = 4;
			}
			this.ChkLinkCellAfterPassed();
		}

		private void SetStartPassedState(CellModel model)
		{
			bool isLinkPassed = this.IsAnyLinkCellPassed(model.GetLinkNo());
			switch (model.EventType)
			{
			case enumMapEventType.NOT_USE:
				this._uiCell.spriteName = string.Empty;
				break;
			case enumMapEventType.None:
				this.SetStartCellColor(model, isLinkPassed, 1, 0);
				break;
			case enumMapEventType.ItemGet:
				this.SetStartCellColor(model, isLinkPassed, 2, 0);
				break;
			case enumMapEventType.Uzushio:
				this.SetStartCellColor(model, isLinkPassed, 3, 0);
				break;
			case enumMapEventType.War_Normal:
				this.SetStartCellColor(model, isLinkPassed, 4, 0);
				break;
			case enumMapEventType.War_Boss:
				this.SetStartCellColor(model, isLinkPassed, 5, 4);
				break;
			case enumMapEventType.Stupid:
				this.SetStartCellColor(model, isLinkPassed, 4, 0);
				break;
			case enumMapEventType.AirReconnaissance:
				this.SetStartCellColor(model, isLinkPassed, 9, 0);
				break;
			case enumMapEventType.PortBackEo:
				this.SetStartCellColor(model, isLinkPassed, 8, 8);
				break;
			}
		}

		private bool IsAnyLinkCellPassed(List<int> linkList)
		{
			bool result = false;
			if (linkList.get_Count() != 0)
			{
				MapManager mapManager = SortieBattleTaskManager.GetMapManager();
				using (List<int>.Enumerator enumerator = linkList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						if (mapManager.Cells[current].Passed)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		private void SetStartCellColor(CellModel cellModel, bool isLinkPassed, int nPassedColor, int nNonPassedColor)
		{
			if (isLinkPassed)
			{
				this.colorNo = nPassedColor;
			}
			else
			{
				this.colorNo = ((!cellModel.Passed) ? nNonPassedColor : nPassedColor);
			}
		}

		private void SetPassedCellColor()
		{
			switch (this._clsCellModel.EventType)
			{
			case enumMapEventType.NOT_USE:
				this._uiCell.spriteName = string.Empty;
				break;
			case enumMapEventType.None:
				this.colorNo = 1;
				break;
			case enumMapEventType.ItemGet:
				this.colorNo = 2;
				break;
			case enumMapEventType.Uzushio:
				this.colorNo = 3;
				break;
			case enumMapEventType.War_Normal:
				this.colorNo = ((this._clsCellModel.WarType != enumMapWarType.AirBattle) ? 4 : 7);
				break;
			case enumMapEventType.War_Boss:
				this.colorNo = 5;
				break;
			case enumMapEventType.Stupid:
				this.colorNo = 1;
				break;
			case enumMapEventType.AirReconnaissance:
				this.colorNo = 9;
				break;
			case enumMapEventType.PortBackEo:
				this.colorNo = 8;
				break;
			}
			this.ChkLinkCellAfterPassed();
		}

		private void ChkLinkCellAfterPassed()
		{
			if (this._clsCellModel.GetLinkNo().get_Count() != 0)
			{
				UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
				this._clsCellModel.GetLinkNo().ForEach(delegate(int x)
				{
					uimm.cells.get_Item(x).SetActive(false);
				});
				this._uiCell.depth++;
			}
		}

		public void PlayMailstrom(UISortieShip sortieShip, MapEventHappeningModel eventHappeningModel, Action onFinished)
		{
			ProdShipRipple component = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabProdShipRipple.get_gameObject(), base.get_transform().get_gameObject(), false, false).GetComponent<ProdShipRipple>();
			ProdMailstrom prodMailstrom = ProdMailstrom.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabProdMaelstrom.GetComponent<ProdMailstrom>(), base.get_transform(), eventHappeningModel);
			prodMailstrom.PlayMailstrom(sortieShip, component, onFinished);
		}

		public void PlayRipple(Color color)
		{
			this._prodRipple = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabProdShipRipple.get_gameObject(), base.get_transform().get_gameObject(), false, false).GetComponent<ProdShipRipple>();
			this._prodRipple.Play(color);
			SoundUtils.PlaySE(SEFIleInfos.SE_032);
			Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate(long _)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_032);
			});
		}

		public void StopRipple()
		{
			this._prodRipple.Stop();
		}

		private void SetActiveBranchingTargetCell(bool isMake)
		{
			if (isMake)
			{
				if (this._uiFocusCell == null)
				{
					GameObject gameObject = new GameObject("ActiveBranchingFocusCell");
					this._uiFocusCell = gameObject.AddComponent<UISprite>();
					this._uiFocusCell.get_transform().set_parent(base.get_transform());
					this._uiFocusCell.atlas = this._uiCell.atlas;
					this._uiFocusCell.alpha = 0f;
					this._uiFocusCell.spriteName = "sail_mapIcon_white_30";
					this._uiFocusCell.MakePixelPerfect();
					this._uiFocusCell.get_transform().localScaleOne();
					this._uiFocusCell.get_transform().set_localPosition(new Vector3(-1f, -0.5f, 0f));
				}
				if (this._uiGlowCell == null)
				{
					GameObject gameObject2 = new GameObject("ActiveBranchingGlow");
					this._uiGlowCell = gameObject2.AddComponent<UISprite>();
					this._uiGlowCell.get_transform().set_parent(base.get_transform());
					this._uiGlowCell.get_transform().localScaleOne();
					this._uiGlowCell.get_transform().localPositionZero();
					this._uiGlowCell.atlas = this._uiCell.atlas;
					this._uiGlowCell.alpha = 0f;
					this._uiGlowCell.depth = this._uiCell.depth - 2;
					this._uiGlowCell.spriteName = "mapIcon_activebranching_glow";
					this._uiGlowCell.MakePixelPerfect();
				}
				if (this._uiActiveBranchingToggle == null)
				{
					this._uiActiveBranchingToggle = this.AddComponent<UIToggle>();
					this._uiActiveBranchingToggle.group = 10;
				}
			}
			else
			{
				if (this._uiFocusCell != null)
				{
					this._uiFocusCell.get_transform().LTCancel();
				}
				Mem.DelComponent<UISprite>(ref this._uiFocusCell);
				if (this._uiActiveBranchingToggle != null)
				{
					this._uiActiveBranchingToggle.onActive.Clear();
					this._uiActiveBranchingToggle.onDecide = null;
					Object.Destroy(this._uiActiveBranchingToggle);
				}
				Mem.Del<UIToggle>(ref this._uiActiveBranchingToggle);
			}
		}

		public void SetOnDecideActiveBranchingTarget(int nIndex, Action<int> onActive, Action<UISortieMapCell> onDecide)
		{
			this._uiActiveBranchingToggle.onActive.Clear();
			this._uiActiveBranchingToggle.onDecide = null;
			this._uiActiveBranchingToggle.onActive.Add(new EventDelegate(delegate
			{
				Dlg.Call<int>(ref onActive, nIndex);
			}));
			this._uiActiveBranchingToggle.onDecide = delegate
			{
				Dlg.Call<UISortieMapCell>(ref onDecide, this);
			};
		}

		private void PlayActiveBranchingGlow()
		{
			this._uiGlowCell.get_transform().LTValue(0f, 0.5f, 0.85f).setLoopPingPong().setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiGlowCell.alpha = x;
			});
		}

		private void StopActiveBranchingGlow()
		{
			if (this._uiGlowCell != null)
			{
				this._uiGlowCell.get_transform().LTCancel();
				this._uiGlowCell.get_transform().LTValue(this._uiGlowCell.alpha, 0f, 0.25f).setOnUpdate(delegate(float x)
				{
					this._uiGlowCell.alpha = x;
				}).setEase(LeanTweenType.linear).setOnComplete(delegate
				{
					Mem.DelComponent<UISprite>(ref this._uiGlowCell);
				});
			}
		}
	}
}
