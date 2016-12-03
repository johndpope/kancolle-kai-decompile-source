using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_SlotItemManager : MonoBehaviour
	{
		[SerializeField]
		private OrganizeDetail_SlotItem[] SlotItem;

		private UIGrid grid;

		public void SetSlotItems(ShipModel ship)
		{
			bool flag = ship.HasExSlot();
			for (int i = 0; i < this.SlotItem.Length; i++)
			{
				if (i < ship.SlotitemList.get_Count())
				{
					this.SlotItem[i].SetSlotItem(ship.SlotitemList.get_Item(i), ship, i, false);
				}
				else if (flag)
				{
					flag = false;
					this.SlotItem[i].SetSlotItem(ship.SlotitemEx, ship, i, true);
				}
				else
				{
					this.SlotItem[i].SetActive(false);
				}
			}
		}

		private void OnDestroy()
		{
			this.SlotItem = null;
		}
	}
}
