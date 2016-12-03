using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Manager : MonoBehaviour
	{
		[SerializeField]
		private OrganizeDetail_Card card;

		[SerializeField]
		private OrganizeDetail_Status status;

		[SerializeField]
		private OrganizeDetail_StatusMaxIcons statusMaxIcons;

		[SerializeField]
		private OrganizeDetail_Paramerter parameter;

		[SerializeField]
		private OrganizeDetail_SlotItemManager slotItem;

		[SerializeField]
		private DialogAnimation DialogAnim;

		[SerializeField]
		private UIButtonMessage BackButton;

		public bool isShow;

		public OrganizeDetail_Buttons buttons;

		private BoxCollider2D MaskBg;

		public bool Init()
		{
			Util.FindParentToChild<OrganizeDetail_Card>(ref this.card, base.get_transform(), "CardPanel");
			Util.FindParentToChild<OrganizeDetail_Status>(ref this.status, base.get_transform(), "StatusPanel");
			Util.FindParentToChild<OrganizeDetail_StatusMaxIcons>(ref this.statusMaxIcons, base.get_transform(), "StatusMaxIcons");
			Util.FindParentToChild<OrganizeDetail_Paramerter>(ref this.parameter, base.get_transform(), "ParamaterPanel");
			Util.FindParentToChild<OrganizeDetail_SlotItemManager>(ref this.slotItem, base.get_transform(), "SlotItemPanel");
			if (this.DialogAnim == null)
			{
				this.DialogAnim = base.get_transform().GetComponent<DialogAnimation>();
			}
			Util.FindParentToChild<OrganizeDetail_Buttons>(ref this.buttons, base.get_transform(), "ButtonPanel");
			Util.FindParentToChild<BoxCollider2D>(ref this.MaskBg, base.get_transform(), "MaskBg");
			this.MaskBg.set_size(Vector2.get_right() * 1060f + Vector2.get_up() * 544f);
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<OrganizeDetail_Card>(ref this.card);
			Mem.Del<OrganizeDetail_Status>(ref this.status);
			Mem.Del<OrganizeDetail_StatusMaxIcons>(ref this.statusMaxIcons);
			Mem.Del<OrganizeDetail_Paramerter>(ref this.parameter);
			Mem.Del<OrganizeDetail_SlotItemManager>(ref this.slotItem);
			Mem.Del<DialogAnimation>(ref this.DialogAnim);
			Mem.Del<UIButtonMessage>(ref this.BackButton);
			Mem.Del<OrganizeDetail_Buttons>(ref this.buttons);
			Mem.Del<BoxCollider2D>(ref this.MaskBg);
		}

		public void SetDetailPanel(ShipModel ship, bool isFirstDitail, int SelectDeckId, IOrganizeManager manager, int ShipIndex, MonoBehaviour CallBackTarget)
		{
			this.card.SetShipCard(ship);
			this.status.SetStatus(ship);
			this.statusMaxIcons.SetMaxIcons(ship);
			this.parameter.SetParams(ship);
			this.slotItem.SetSlotItems(ship);
			if (isFirstDitail)
			{
				this.buttons.SetDeckShipDetailButtons(ship, manager, CallBackTarget);
			}
			else
			{
				this.buttons.SetListShipDetailButtons(ship, SelectDeckId, manager, ShipIndex, CallBackTarget);
			}
		}

		public void SetBackButton(GameObject target, string FunctionName)
		{
			this.BackButton.target = target;
			this.BackButton.functionName = FunctionName;
		}

		public void Open()
		{
			base.get_transform().GetComponent<UIPanel>().alpha = 1f;
			this.DialogAnim.FadeIn(0f);
			this.isShow = true;
			this.DialogAnim.CloseAction = null;
		}

		public void Close()
		{
			this.DialogAnim.CloseAction = delegate
			{
				base.get_transform().GetComponent<UIPanel>().alpha = 0f;
				this.card.Release();
			};
			this.DialogAnim.FadeOut();
			this.isShow = false;
		}

		[DebuggerHidden]
		public IEnumerator CloseAndDestroy()
		{
			OrganizeDetail_Manager.<CloseAndDestroy>c__Iterator9D <CloseAndDestroy>c__Iterator9D = new OrganizeDetail_Manager.<CloseAndDestroy>c__Iterator9D();
			<CloseAndDestroy>c__Iterator9D.<>f__this = this;
			return <CloseAndDestroy>c__Iterator9D;
		}
	}
}
