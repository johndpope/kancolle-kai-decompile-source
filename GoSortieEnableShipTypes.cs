using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoSortieEnableShipTypes : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab_ShipTypeIcon;

	private List<GameObject> ShipTypeIcons;

	[SerializeField]
	private UILabel Message;

	public int Space = 10;

	public void Initialize(MapModel model, HashSet<SType> LimitTypeEnable, HashSet<SType> LimitTypeDisable)
	{
		if (this.ShipTypeIcons != null)
		{
			this.Release();
		}
		else
		{
			this.ShipTypeIcons = new List<GameObject>();
		}
		HashSet<SType> hashSet = (LimitTypeEnable == null) ? LimitTypeDisable : LimitTypeEnable;
		int num = 0;
		List<SType> list = Enumerable.ToList<SType>(hashSet);
		list.Sort((SType a, SType b) => a - b);
		using (List<SType>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SType current = enumerator.get_Current();
				UISprite component = Util.Instantiate(this.prefab_ShipTypeIcon, base.get_gameObject(), false, false).GetComponent<UISprite>();
				component.set_enabled(true);
				component.spriteName = "ship" + (int)current;
				component.MakePixelPerfect();
				component.get_transform().localPositionX((float)num);
				num += component.width + this.Space;
				this.ShipTypeIcons.Add(component.get_gameObject());
			}
		}
		this.Message.text = ((hashSet != LimitTypeEnable) ? "は[FF0000]出撃不可[-]です。" : "のみ[66ccff]出撃可能[-]です。");
		this.Message.get_transform().localPositionX((float)num);
	}

	public void Release()
	{
		using (List<GameObject>.Enumerator enumerator = this.ShipTypeIcons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.get_Current();
				Object.Destroy(current);
			}
		}
	}

	private void OnDestroy()
	{
		this.prefab_ShipTypeIcon = null;
		if (this.ShipTypeIcons != null)
		{
			this.ShipTypeIcons.Clear();
		}
		this.ShipTypeIcons = null;
		this.Message = null;
	}
}
