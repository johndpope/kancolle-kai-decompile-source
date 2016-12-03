using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_SlotItem : MonoBehaviour
	{
		[SerializeField]
		private UILabel Name;

		[SerializeField]
		private UISprite Icon;

		[SerializeField]
		private Transform PlusParent;

		[SerializeField]
		private UILabel PlusNum;

		[SerializeField]
		private Transform PlusBase;

		[SerializeField]
		private Transform PlusMax;

		[SerializeField]
		private Transform PlaneNumParent;

		[SerializeField]
		private UILabel PlaneNum;

		[SerializeField]
		private UISprite PlaneSkill;

		[SerializeField]
		private int SlotItemNo;

		private Vector3 PlaneNumPos_NoSkill = new Vector3(158f, 127f, 0f);

		private Vector3 PlaneNumPos_SkillPos = new Vector3(158f, 113f, 0f);

		private float DefaultPosY;

		public void Start()
		{
			this.DefaultPosY = base.get_transform().get_localPosition().y;
		}

		private void OnDestroy()
		{
			this.Name = null;
			this.Icon = null;
			this.PlusParent = null;
			this.PlusNum = null;
			this.PlusBase = null;
			this.PlusMax = null;
			this.PlaneNumParent = null;
			this.PlaneNum = null;
			this.PlaneSkill = null;
		}

		public void SetSlotItem(SlotitemModel item, ShipModel ship, int index, bool isExtention)
		{
			this.DefaultPosY = (float)(index * -67);
			if (item != null)
			{
				this.Name.text = item.Name;
				this.Icon.spriteName = "icon_slot" + item.Type4;
				this.SetPlusIcon(item);
				this.SetPlaneNum(item, ship);
				this.SetPlaneSkill(item);
			}
			else
			{
				this.Name.text = "-";
				this.PlusParent.SetActive(false);
				this.PlaneNumParent.SetActive(false);
				this.PlaneSkill.SetActive(false);
				this.Icon.spriteName = string.Empty;
			}
			this.SetExtentionSlotMode(isExtention);
			base.get_gameObject().SetActive(true);
		}

		private void SetPlusIcon(SlotitemModel item)
		{
			if (item.Level <= 0)
			{
				this.PlusParent.SetActive(false);
			}
			else if (item.Level < 10)
			{
				this.PlusMax.SetActive(false);
				this.PlusNum.textInt = item.Level;
				this.PlusParent.SetActive(true);
			}
			else
			{
				this.PlusParent.SetActive(false);
			}
		}

		private void SetPlaneNum(SlotitemModel item, ShipModel ship)
		{
			if (item.IsPlane())
			{
				this.PlaneNum.text = ship.Tousai[this.SlotItemNo - 1].ToString();
				this.PlaneNumParent.SetActive(true);
			}
			else
			{
				this.PlaneNumParent.SetActive(false);
			}
		}

		private void SetPlaneSkill(SlotitemModel item)
		{
			if (item.IsPlane())
			{
				int skillLevel = item.SkillLevel;
				if (skillLevel == 0)
				{
					this.PlaneSkill.SetActive(false);
					this.PlaneNumParent.get_transform().set_localPosition(this.PlaneNumPos_NoSkill);
				}
				else
				{
					this.PlaneSkill.SetActive(true);
					this.PlaneSkill.spriteName = "skill_" + skillLevel;
					this.PlaneSkill.MakePixelPerfect();
					this.PlaneNumParent.get_transform().set_localPosition(this.PlaneNumPos_SkillPos);
				}
			}
			else
			{
				this.PlaneSkill.SetActive(false);
			}
		}

		private void SetExtentionSlotMode(bool isExtention)
		{
			if (isExtention)
			{
				base.get_transform().set_localScale(new Vector3(0.8f, 0.8f, 0.8f));
				base.get_transform().set_localPosition(new Vector3(-40f, this.DefaultPosY + 28f, 0f));
				this.Name.fontSize = 30;
			}
			else
			{
				base.get_transform().set_localScale(Vector3.get_one());
				base.get_transform().set_localPosition(new Vector3(0f, this.DefaultPosY, 0f));
				this.Name.fontSize = 24;
			}
		}
	}
}
