using Common.Enum;
using KCV;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite)), RequireComponent(typeof(OnClickEventSender)), RequireComponent(typeof(UIWidget))]
public class UIShipSortButton : MonoBehaviour
{
	public delegate bool CheckClickable();

	private OnClickEventSender mOnClickEventSender;

	private UISprite mSpriteStatus;

	private UIWidget mWidgetThis;

	private Action<ShipModel[]> mOnSortedListener;

	private UIShipSortButton.CheckClickable mCheckClicable;

	private SortKey[] mSortKeys;

	private ShipModel[] mShipModels;

	public SortKey CurrentSortKey
	{
		get;
		private set;
	}

	public bool IsClickable
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.CurrentSortKey = SortKey.LEVEL;
		this.mOnClickEventSender = base.GetComponent<OnClickEventSender>();
		this.mSpriteStatus = base.GetComponent<UISprite>();
		this.mWidgetThis = base.GetComponent<UIWidget>();
	}

	public void SetSortKey(SortKey sortKey)
	{
		this.CurrentSortKey = sortKey;
		this.InitializeSortIcon(sortKey);
	}

	public void SetClickable(bool clickable)
	{
		this.IsClickable = clickable;
		if (this.IsClickable)
		{
			this.mOnClickEventSender.SetClickable(true);
		}
		else
		{
			this.mOnClickEventSender.SetClickable(false);
		}
	}

	public void Initialize(ShipModel[] shipModels)
	{
		this.mShipModels = shipModels;
		SortKey[] expr_0E = new SortKey[4];
		expr_0E[0] = SortKey.SHIPTYPE;
		expr_0E[1] = SortKey.NEW;
		expr_0E[2] = SortKey.DAMAGE;
		this.mSortKeys = expr_0E;
	}

	public void InitializeForOrganize(ShipModel[] shipModels)
	{
		this.mShipModels = shipModels;
		SortKey[] expr_0E = new SortKey[5];
		expr_0E[0] = SortKey.UNORGANIZED;
		expr_0E[1] = SortKey.SHIPTYPE;
		expr_0E[2] = SortKey.NEW;
		expr_0E[3] = SortKey.DAMAGE;
		this.mSortKeys = expr_0E;
	}

	public void SetOnSortedShipsListener(Action<ShipModel[]> onSortedListener)
	{
		this.mOnSortedListener = onSortedListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchSortButton()
	{
		this.OnClickSortButton();
	}

	public void SetCheckClicableDelegate(UIShipSortButton.CheckClickable checkClickable)
	{
		this.mCheckClicable = checkClickable;
	}

	public void OnClickSortButton()
	{
		if (this.IsClickable && this.CallCheckClickable())
		{
			this.SortNext();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}
	}

	private bool CallCheckClickable()
	{
		return this.mCheckClicable == null || this.mCheckClicable();
	}

	private void SortNext()
	{
		SortKey nextSortKey = this.GetNextSortKey(this.CurrentSortKey);
		this.ChangeSortKey(nextSortKey);
	}

	private void OnSortedShips(ShipModel[] sortedModels)
	{
		if (this.mOnSortedListener != null)
		{
			this.mOnSortedListener.Invoke(sortedModels);
		}
	}

	private SortKey GetNextSortKey(SortKey currentSortKey)
	{
		int num = Array.IndexOf<SortKey>(this.mSortKeys, this.CurrentSortKey);
		num++;
		if (num >= this.mSortKeys.Length)
		{
			num = 0;
		}
		return this.mSortKeys[num];
	}

	private void InitializeSortIcon(SortKey sortKey)
	{
		switch (sortKey)
		{
		case SortKey.LEVEL:
			this.mSpriteStatus.spriteName = "sort_lv";
			break;
		case SortKey.SHIPTYPE:
			this.mSpriteStatus.spriteName = "sort_ship";
			break;
		case SortKey.DAMAGE:
			this.mSpriteStatus.spriteName = "sort_crane";
			break;
		case SortKey.NEW:
			this.mSpriteStatus.spriteName = "sort_new";
			break;
		case SortKey.UNORGANIZED:
			this.mSpriteStatus.spriteName = "sort_flag";
			break;
		}
	}

	private void ChangeSortKey(SortKey sortKey)
	{
		this.CurrentSortKey = sortKey;
		this.InitializeSortIcon(sortKey);
		ShipModel[] sortedModels = DeckUtil.GetSortedList(new List<ShipModel>(this.mShipModels), sortKey).ToArray();
		this.OnSortedShips(sortedModels);
	}

	public void RefreshModels(ShipModel[] models)
	{
		this.mShipModels = models;
	}

	public void ReSort()
	{
		this.ChangeSortKey(this.CurrentSortKey);
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mSpriteStatus);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
		this.mOnClickEventSender = null;
		this.mOnSortedListener = null;
		this.mCheckClicable = null;
		this.mShipModels = null;
	}

	internal ShipModel[] SortModels(SortKey sortKey)
	{
		return DeckUtil.GetSortedList(new List<ShipModel>(this.mShipModels), sortKey).ToArray();
	}
}
