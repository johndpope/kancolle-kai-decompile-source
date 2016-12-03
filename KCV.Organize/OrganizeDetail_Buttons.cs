using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Buttons : MonoBehaviour
	{
		[SerializeField]
		protected UIButton LeftButton;

		[SerializeField]
		protected UIButton RightButton;

		public LockSwitch LockSwitch;

		[SerializeField]
		protected UIButtonMessage BackBG;

		protected ShipModel ship;

		[SerializeField]
		protected UIButtonManager buttonManager;

		protected bool isDeckShipDetail;

		public void SetDeckShipDetailButtons(ShipModel ship, IOrganizeManager manager = null, MonoBehaviour CallBackTarget = null)
		{
			this.isDeckShipDetail = true;
			this.ship = ship;
			bool flag = (manager != null) ? manager.IsValidShip(ship.MemId) : this.IsValidShip();
			this.LeftButton.SetActive(true);
			this.setChangeButton(this.LeftButton, flag, CallBackTarget);
			this.setUnsetButton(this.RightButton, manager, CallBackTarget);
			this.buttonManager.nowForcusButton = ((!flag) ? this.RightButton : this.LeftButton);
			this.LockSwitch.SetActive(false);
			if (ship.IsBling() && ship.IsInDeck() != -1)
			{
				this.buttonManager.setFocus(1);
			}
			GameObject backBG = (!(CallBackTarget == null)) ? CallBackTarget.get_gameObject() : null;
			this.setBackBG(backBG);
		}

		public void SetListShipDetailButtons(ShipModel ship, int deckId, IOrganizeManager manager = null, int ShipIndex = 0, MonoBehaviour CallBackTarget = null)
		{
			this.isDeckShipDetail = false;
			this.ship = ship;
			bool flag = (manager != null) ? manager.IsValidChange(deckId, ShipIndex, ship.MemId) : this.IsValidChange(deckId, ShipIndex, ship.MemId);
			this.LockSwitch.setIcon(ship);
			this.LeftButton.SetActive(false);
			this.setChangeButton(this.RightButton, flag, CallBackTarget);
			this.buttonManager.nowForcusButton = ((!flag) ? null : this.RightButton);
			this.LockSwitch.SetActive(true);
			GameObject backBG = (!(CallBackTarget == null)) ? CallBackTarget.get_gameObject() : null;
			this.setBackBG(backBG);
		}

		protected void setChangeButton(UIButton button, bool isValidChange, MonoBehaviour Target = null)
		{
			button.normalSprite = "btn_set";
			button.hoverSprite = "btn_set_on";
			button.pressedSprite = "btn_set_on";
			button.disabledSprite = "btn_set_off";
			button.isEnabled = isValidChange;
			if (isValidChange)
			{
				if (this.isDeckShipDetail)
				{
					MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask();
					button.onClick = Util.CreateEventDelegateList(target, "SetBtnEL", null);
				}
				else
				{
					MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetListDetailTask();
					button.onClick = Util.CreateEventDelegateList(target, "ChangeButtonEL", null);
				}
				button.SetState(UIButtonColor.State.Hover, false);
			}
		}

		protected virtual void setUnsetButton(UIButton button, IOrganizeManager manager = null, MonoBehaviour Target = null)
		{
			button.normalSprite = "btn_reset";
			button.hoverSprite = "btn_reset_on";
			button.pressedSprite = "btn_reset_on";
			button.disabledSprite = "btn_reset_off";
			MonoBehaviour target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask();
			button.onClick = Util.CreateEventDelegateList(target, "ResetBtnEL", null);
			button.isEnabled = ((manager != null) ? manager.IsValidUnset(this.ship.MemId) : this.IsValidUnset());
			button.SetState((!button.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}

		protected virtual void setBackBG(GameObject Target = null)
		{
			if (this.isDeckShipDetail)
			{
				GameObject target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetDetailTask().get_gameObject();
				this.BackBG.target = target;
				this.BackBG.functionName = "BackMaskEL";
				this.BackBG.trigger = UIButtonMessage.Trigger.OnClick;
			}
			else
			{
				GameObject target = (!(Target == null)) ? Target : OrganizeTaskManager.Instance.GetListDetailTask().get_gameObject();
				this.BackBG.target = target;
				this.BackBG.functionName = "BackDataEL";
				this.BackBG.trigger = UIButtonMessage.Trigger.OnClick;
			}
		}

		public virtual void UpdateButton(bool isLeft, OrganizeManager manager = null)
		{
			if (!((manager == null) ? this.IsValidShip() : manager.IsValidShip(this.ship.MemId)))
			{
				return;
			}
			if (!((manager == null) ? this.IsValidUnset() : manager.IsValidUnset(this.ship.MemId)))
			{
				return;
			}
			if (isLeft && this.buttonManager.nowForcusButton != this.LeftButton)
			{
				this.RightButton.SetState(UIButtonColor.State.Normal, true);
				this.LeftButton.SetState(UIButtonColor.State.Hover, true);
				this.buttonManager.nowForcusButton = this.LeftButton;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else if (!isLeft && this.buttonManager.nowForcusButton != this.RightButton)
			{
				this.RightButton.SetState(UIButtonColor.State.Hover, true);
				this.LeftButton.SetState(UIButtonColor.State.Normal, true);
				this.buttonManager.nowForcusButton = this.RightButton;
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public virtual void Decide()
		{
			this.buttonManager.Decide();
		}

		private bool IsValidShip()
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidShip(this.ship.MemId);
		}

		private bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidChange(deck_id, selected_index, ship_mem_id);
		}

		private bool IsValidUnset()
		{
			Debug.Log(this.ship.MemId);
			Debug.Log(this.ship.Name);
			return OrganizeTaskManager.Instance.GetLogicManager().IsValidUnset(this.ship.MemId);
		}
	}
}
