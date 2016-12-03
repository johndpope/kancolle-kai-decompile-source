using KCV;
using KCV.Organize;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class OrganizeTaskManager : MonoBehaviour
{
	public enum OrganizePhase
	{
		Phase_ST,
		Phase_BEF = -1,
		Phase_NONE = -1,
		Top,
		Detail,
		List,
		ListDetail,
		Phase_AFT,
		Phase_NUM = 4,
		Phase_ED = 3
	}

	protected static GameObject _uiCommon;

	protected static KeyControl _clsInputKey;

	protected static SceneTasksMono _clsTasks;

	protected static OrganizeTaskManager.OrganizePhase _iPhase;

	protected static OrganizeTaskManager.OrganizePhase _iPhaseReq;

	public static TaskOrganizeTop _clsTop;

	public static TaskOrganizeDetail _clsDetail;

	public static TaskOrganizeList _clsList;

	public static TaskOrganizeListDetail _clsListDetail;

	public static OrganizeManager logicManager;

	protected static BaseDialogPopup dialogPopUp;

	protected static DeckModel[] _deck;

	protected static ShipModel[] _ship;

	protected static ShipModel[] _allShip;

	protected static GameObject _mainObj;

	public static OrganizeTaskManager Instance;

	protected bool isRun;

	public virtual TaskOrganizeTop GetTopTask()
	{
		return OrganizeTaskManager._clsTop;
	}

	public virtual TaskOrganizeDetail GetDetailTask()
	{
		return OrganizeTaskManager._clsDetail;
	}

	public virtual TaskOrganizeList GetListTask()
	{
		return OrganizeTaskManager._clsList;
	}

	public virtual TaskOrganizeListDetail GetListDetailTask()
	{
		return OrganizeTaskManager._clsListDetail;
	}

	public virtual IOrganizeManager GetLogicManager()
	{
		return OrganizeTaskManager.logicManager;
	}

	public static BaseDialogPopup GetDialogPopUp()
	{
		return OrganizeTaskManager.dialogPopUp;
	}

	public static DeckModel[] GetDeck()
	{
		return OrganizeTaskManager._deck;
	}

	public static ShipModel[] GetShip()
	{
		return OrganizeTaskManager._ship;
	}

	public static ShipModel[] GetAllShip()
	{
		return OrganizeTaskManager._allShip;
	}

	public static GameObject GetMainObject()
	{
		return OrganizeTaskManager._mainObj;
	}

	protected virtual void Awake()
	{
		OrganizeTaskManager._clsInputKey = new KeyControl(0, 0, 0.4f, 0.1f);
		OrganizeTaskManager._clsInputKey.useDoubleIndex(0, 3);
		OrganizeTaskManager.dialogPopUp = new BaseDialogPopup();
		OrganizeTaskManager.Instance = this;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		OrganizeTaskManager.<Start>c__Iterator9E <Start>c__Iterator9E = new OrganizeTaskManager.<Start>c__Iterator9E();
		<Start>c__Iterator9E.<>f__this = this;
		return <Start>c__Iterator9E;
	}

	private void Update()
	{
		if (!this.isRun)
		{
			return;
		}
		OrganizeTaskManager._clsInputKey.Update();
		OrganizeTaskManager._clsTasks.Run();
		this.UpdateMode();
	}

	public static void ReqPhase(OrganizeTaskManager.OrganizePhase iPhase)
	{
		OrganizeTaskManager._iPhaseReq = iPhase;
	}

	public static OrganizeTaskManager.OrganizePhase GetPhase()
	{
		return OrganizeTaskManager._iPhase;
	}

	protected void UpdateMode()
	{
		if (OrganizeTaskManager._iPhaseReq == OrganizeTaskManager.OrganizePhase.Phase_BEF)
		{
			return;
		}
		switch (OrganizeTaskManager._iPhaseReq)
		{
		case OrganizeTaskManager.OrganizePhase.Phase_ST:
			if (OrganizeTaskManager._clsTasks.Open(OrganizeTaskManager._clsTop) < 0)
			{
				return;
			}
			break;
		case OrganizeTaskManager.OrganizePhase.Detail:
			if (OrganizeTaskManager._clsTasks.Open(OrganizeTaskManager._clsDetail) < 0)
			{
				return;
			}
			break;
		case OrganizeTaskManager.OrganizePhase.List:
			if (OrganizeTaskManager._clsTasks.Open(OrganizeTaskManager._clsList) < 0)
			{
				return;
			}
			break;
		case OrganizeTaskManager.OrganizePhase.ListDetail:
			if (OrganizeTaskManager._clsTasks.Open(OrganizeTaskManager._clsListDetail) < 0)
			{
				return;
			}
			break;
		}
		OrganizeTaskManager._iPhase = OrganizeTaskManager._iPhaseReq;
		OrganizeTaskManager._iPhaseReq = OrganizeTaskManager.OrganizePhase.Phase_BEF;
		OrganizeTaskManager._clsTop.UpdateByModeChanging();
	}

	public static KeyControl GetKeyControl()
	{
		return OrganizeTaskManager._clsInputKey;
	}
}
