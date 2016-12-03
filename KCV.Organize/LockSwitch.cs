using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class LockSwitch : MonoBehaviour
	{
		[SerializeField]
		private UISprite lockBtn;

		[SerializeField]
		private UISprite lockBg;

		private ShipModel ship;

		private Action ChangeListViewIcon;

		private void OnDestroy()
		{
			Mem.Del(ref this.lockBtn);
			Mem.Del(ref this.lockBg);
			Mem.Del<ShipModel>(ref this.ship);
			Mem.Del<Action>(ref this.ChangeListViewIcon);
		}

		public void setIcon(ShipModel ship)
		{
			this.ship = ship;
			this.moveIcon(0f);
		}

		public void setChangeListViewIcon(Action act)
		{
			this.ChangeListViewIcon = act;
		}

		public void MoveLockBtn()
		{
			this.ChangeListViewIcon.Invoke();
			this.moveIcon(0.2f);
		}

		private void moveIcon(float time)
		{
			if (time > 0f)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("time", time);
				hashtable.Add("x", (!this.ship.IsLocked()) ? 47f : -47f);
				hashtable.Add("easeType", iTween.EaseType.linear);
				hashtable.Add("isLocal", true);
				hashtable.Add("oncomplete", (!this.ship.IsLocked()) ? "compMoveUnLock" : "compMoveLock");
				hashtable.Add("oncompletetarget", base.get_gameObject());
				this.lockBtn.get_transform().get_gameObject().MoveTo(hashtable);
			}
			else
			{
				this.lockBtn.get_transform().localPositionX((!this.ship.IsLocked()) ? 47f : -47f);
				if (this.ship.IsLocked())
				{
					this.compMoveLock();
				}
				else
				{
					this.compMoveUnLock();
				}
			}
		}

		private void compMoveLock()
		{
			this.lockBg.spriteName = "switch_lock_on";
			this.lockBtn.spriteName = "switch_lock_on_pin";
		}

		private void compMoveUnLock()
		{
			this.lockBg.spriteName = "switch_lock";
			this.lockBtn.spriteName = "switch_lock_pin";
		}
	}
}
