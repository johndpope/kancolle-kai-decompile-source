using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UniRx;

namespace KCV.SortieMap
{
	public class CtrlActiveBranching : IDisposable
	{
		private List<CellModel> _listCellModel;

		private List<Tuple<int, UISortieMapCell>> _listUIMapCell;

		private Action<int> _actOnDecideMapCell;

		private int _nSelectIndex;

		private bool _isInputPossible;

		public CtrlActiveBranching(List<CellModel> cells, Action<int> onDecide)
		{
			this.Init(cells, onDecide);
		}

		public void Dispose()
		{
			Mem.DelListSafe<CellModel>(ref this._listCellModel);
			Mem.DelListSafe<Tuple<int, UISortieMapCell>>(ref this._listUIMapCell);
			Mem.Del<Action<int>>(ref this._actOnDecideMapCell);
		}

		private bool Init(List<CellModel> cells, Action<int> onDecide)
		{
			this._nSelectIndex = 0;
			this._isInputPossible = false;
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			this._listCellModel = cells;
			this._actOnDecideMapCell = onDecide;
			UISortieShip ship = SortieMapTaskManager.GetUIMapManager().sortieShip;
			ship.PlayBalloon(enumMapEventType.Stupid, enumMapWarType.Midnight, delegate
			{
				ship.ShowInputIcon();
				uiamf.SetMessage("艦隊の針路を選択できます。\n提督、どちらの針路を選択しますか？");
				this.ActiveTargetCell(cells);
				Observable.NextFrame(FrameCountType.EndOfFrame).Subscribe(delegate(Unit _)
				{
					this._isInputPossible = true;
				});
			});
			return true;
		}

		private void ActiveTargetCell(List<CellModel> cells)
		{
			this._listUIMapCell = new List<Tuple<int, UISortieMapCell>>();
			int cnt = 0;
			cells.ForEach(delegate(CellModel x)
			{
				UISortieMapCell uISortieMapCell = SortieMapTaskManager.GetUIMapManager().cells.get_Item(x.CellNo);
				uISortieMapCell.isActiveBranchingTarget = true;
				uISortieMapCell.SetOnDecideActiveBranchingTarget(cnt, new Action<int>(this.OnActive), new Action<UISortieMapCell>(this.OnDecideMapCell));
				uISortieMapCell.isFocus2ActiveBranching = (this._nSelectIndex == cnt);
				this._listUIMapCell.Add(new Tuple<int, UISortieMapCell>(cnt, uISortieMapCell));
				cnt++;
			});
		}

		public bool Update()
		{
			if (!this._isInputPossible)
			{
				return true;
			}
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				this.PreparaNext(false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				this.PreparaNext(true);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				this.OnDecideMapCell(this._listUIMapCell.get_Item(this._nSelectIndex).get_Item2());
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			int nSelectIndex = this._nSelectIndex;
			this._nSelectIndex = Mathe.NextElementRev(this._nSelectIndex, 0, this._listCellModel.get_Count() - 1, isFoward);
			if (nSelectIndex != this._nSelectIndex)
			{
				this.ChangeFocus(this._nSelectIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			this._listUIMapCell.ForEach(delegate(Tuple<int, UISortieMapCell> x)
			{
				x.get_Item2().isFocus2ActiveBranching = (x.get_Item1() == nIndex);
			});
		}

		private void OnActive(int nIndex)
		{
			if (this._nSelectIndex != nIndex)
			{
				this._nSelectIndex = nIndex;
				this.ChangeFocus(this._nSelectIndex);
			}
		}

		private void OnDecideMapCell(UISortieMapCell cell)
		{
			this._isInputPossible = false;
			this._listUIMapCell.ForEach(delegate(Tuple<int, UISortieMapCell> x)
			{
				x.get_Item2().isActiveBranchingTarget = false;
			});
			UISortieShip sortieShip = SortieMapTaskManager.GetUIMapManager().sortieShip;
			sortieShip.HideInputIcon();
			UIAreaMapFrame uIAreaMapFrame = SortieMapTaskManager.GetUIAreaMapFrame();
			uIAreaMapFrame.ClearMessage();
			Dlg.Call<int>(ref this._actOnDecideMapCell, cell.cellModel.CellNo);
		}
	}
}
