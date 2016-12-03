using Common.Enum;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoConditionInfo : MonoBehaviour
{
	[SerializeField]
	private UILabel GoConditionText;

	[SerializeField]
	private GoSortieEnableShipTypes enableShipTypes;

	public void Initialize(MapModel model)
	{
		HashSet<SType> sortieLimit = Utils.GetSortieLimit(model.MstId, true);
		HashSet<SType> sortieLimit2 = Utils.GetSortieLimit(model.MstId, false);
		bool flag = sortieLimit == null && sortieLimit2 == null;
		if (flag)
		{
			this.GoConditionText.SetActive(true);
			this.enableShipTypes.SetActive(false);
		}
		else
		{
			this.GoConditionText.SetActive(false);
			this.enableShipTypes.SetActive(true);
			this.enableShipTypes.Initialize(model, sortieLimit, sortieLimit2);
		}
	}
}
