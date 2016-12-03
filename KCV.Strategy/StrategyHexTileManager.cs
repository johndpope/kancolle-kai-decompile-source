using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexTileManager : MonoBehaviour
	{
		private UIWidget Widget;

		private TweenPosition TweenPos;

		[SerializeField]
		private UISprite FocusObject;

		public StrategyHexTile RebellionTile;

		public StrategyHexTile[] Tiles
		{
			get;
			private set;
		}

		public int OpenAreaNum
		{
			get;
			private set;
		}

		public StrategyHexTile FocusTile
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.Tiles = new StrategyHexTile[18];
			for (int i = 1; i < this.Tiles.Length; i++)
			{
				this.Tiles[i] = base.get_transform().FindChild("Tile" + i.ToString("D2")).GetComponent<StrategyHexTile>();
			}
			this.Widget = base.GetComponent<UIWidget>();
			this.TweenPos = base.GetComponent<TweenPosition>();
		}

		public void Init()
		{
			int currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
		}

		public void setAreaModels(StrategyMapManager strategyManager)
		{
			for (int i = 1; i < this.Tiles.Length; i++)
			{
				this.Tiles[i].setAreaModel(strategyManager.Area.get_Item(i));
			}
			this.updateTilesColor();
			this.UpdateAllAreaDockIcons();
		}

		[DebuggerHidden]
		public IEnumerator StartTilesPopUp(int[] newOpenAreas, Action<bool> CallBack)
		{
			StrategyHexTileManager.<StartTilesPopUp>c__Iterator178 <StartTilesPopUp>c__Iterator = new StrategyHexTileManager.<StartTilesPopUp>c__Iterator178();
			<StartTilesPopUp>c__Iterator.newOpenAreas = newOpenAreas;
			<StartTilesPopUp>c__Iterator.CallBack = CallBack;
			<StartTilesPopUp>c__Iterator.<$>newOpenAreas = newOpenAreas;
			<StartTilesPopUp>c__Iterator.<$>CallBack = CallBack;
			<StartTilesPopUp>c__Iterator.<>f__this = this;
			return <StartTilesPopUp>c__Iterator;
		}

		private bool isPopUpTile(List<int> newOpenAreaList, int targetNo, ref int OpenCount)
		{
			if (this.Tiles[targetNo].isOpen)
			{
				OpenCount++;
			}
			if (this.Tiles[targetNo].isRebellionTile)
			{
				this.RebellionTile = this.Tiles[targetNo];
			}
			if (newOpenAreaList == null)
			{
				return this.Tiles[targetNo].isOpen || this.Tiles[targetNo].isRebellionTile;
			}
			return (this.Tiles[targetNo].isOpen || this.Tiles[targetNo].isRebellionTile) && !newOpenAreaList.Contains(targetNo);
		}

		public void setFocusObject(int areaID)
		{
			this.FocusObject.get_transform().set_position(this.FocusTile.get_transform().get_position());
			this.FocusObject.SetActive(true);
		}

		private void setFocusTile(int areaID)
		{
			this.FocusTile = this.Tiles[areaID];
		}

		public void changeFocus(int areaID)
		{
			this.setFocusTile(areaID);
			this.setFocusObject(areaID);
			this.updateTilesColor();
			for (int i = 1; i < this.Tiles.Length; i++)
			{
				this.Tiles[i].isFocus = false;
			}
			this.Tiles[areaID].isFocus = true;
		}

		public void setMovable(List<int> movableAreaID)
		{
			for (int i = 1; i < movableAreaID.get_Count(); i++)
			{
				this.Tiles[i].isMovable = movableAreaID.Contains(i);
			}
		}

		public void clearMovable()
		{
			for (int i = 1; i < this.Tiles.Length; i++)
			{
				this.Tiles[i].isMovable = false;
			}
		}

		public void updateTilesColor()
		{
			for (int i = 1; i < this.Tiles.Length; i++)
			{
				if (this.Tiles[i].isOpen)
				{
					this.Tiles[i].setTileColor();
				}
			}
		}

		public void SetVisible(bool isVisible)
		{
			float alpha = (float)((!isVisible) ? 0 : 1);
			TweenAlpha.Begin(this.Widget.get_gameObject(), 0.2f, alpha);
			this.FocusObject.SetActive(isVisible);
			this.FocusObject.GetComponent<TweenScale>().ResetToBeginning();
			this.FocusObject.GetComponent<TweenAlpha>().ResetToBeginning();
		}

		public void setVisibleFocusObject(bool isVisible)
		{
			this.FocusObject.set_enabled(isVisible);
		}

		public void setActivePositionAnimations(bool isActive)
		{
			this.TweenPos.set_enabled(isActive);
		}

		public void ChangeTileColorMove(List<int> areaIDs)
		{
			int i;
			for (i = 1; i < this.Tiles.Length; i++)
			{
				if (areaIDs != null && areaIDs.Exists((int x) => x == i))
				{
					this.Tiles[i].ChangeMoveTileColor();
				}
				else
				{
					this.Tiles[i].ClearTileColor();
				}
			}
		}

		public bool isExistRebellionTargetTile()
		{
			return Enumerable.Any<StrategyHexTile>(this.Tiles, (StrategyHexTile x) => x != null && x.isColorChanged);
		}

		public int GetColorChangedTileID()
		{
			return Enumerable.First<StrategyHexTile>(this.Tiles, (StrategyHexTile x) => x != null && x.isColorChanged).areaID;
		}

		public void UpdateAllAreaDockIcons()
		{
			StrategyHexTile[] tiles = this.Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				StrategyHexTile strategyHexTile = tiles[i];
				if (strategyHexTile != null)
				{
					strategyHexTile.UpdateDockIcons();
				}
			}
		}

		public void SetVisibleAllAreaDockIcons(bool isVisible)
		{
			StrategyHexTile[] tiles = this.Tiles;
			for (int i = 0; i < tiles.Length; i++)
			{
				StrategyHexTile strategyHexTile = tiles[i];
				if (strategyHexTile != null)
				{
					strategyHexTile.SetVisibleDockIcons(isVisible);
				}
			}
		}
	}
}
