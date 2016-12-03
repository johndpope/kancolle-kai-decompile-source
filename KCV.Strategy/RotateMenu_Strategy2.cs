using Common.Enum;
using KCV.CommandMenu;
using KCV.Display;
using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class RotateMenu_Strategy2 : MonoBehaviour
	{
		private const int ONEROTATE = 25;

		public GameObject Menus;

		private TweenPosition enterMenuMove;

		public CommandMenuParts[] menuParts;

		public KeyControl keyContoroller;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		public int myOffset;

		[SerializeField]
		private BoxCollider2D cancelTouch;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		[SerializeField]
		private UIPlayTween PlayTween;

		public bool isOpen;

		[SerializeField]
		private UISprite MenuBaseSprite;

		private void Awake()
		{
			this.menuParts = new CommandMenuParts[8];
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				this.menuParts[i] = this.Menus.get_transform().FindChild("menu0" + i).GetComponent<CommandMenuParts>();
			}
			this.enterMenuMove = this.Menus.GetComponent<TweenPosition>();
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			RotateMenu_Strategy2.<Start>c__Iterator3F <Start>c__Iterator3F = new RotateMenu_Strategy2.<Start>c__Iterator3F();
			<Start>c__Iterator3F.<>f__this = this;
			return <Start>c__Iterator3F;
		}

		public void Init(KeyControl key)
		{
			this.keyContoroller = key;
			this.keyContoroller.setMinMaxIndex(0, this.menuParts.Length - 1);
			this.keyContoroller.Index = 0;
			this.nowRotateZ = new float[this.menuParts.Length];
			this.upperNo = this.menuParts.Length - 3;
			this.footerNo = this.menuParts.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			this.isOpen = true;
			this.Menus.SetActive(true);
			this.swipeEvent.set_enabled(true);
			this.cancelTouch.set_enabled(true);
			this.myOffset = offset;
			this.keyContoroller.Index = this.myOffset;
			this.SetMenuStates(this.myOffset);
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - this.myOffset, 0f, (float)(this.menuParts.Length - 1));
				this.menuParts[i].tweenRot.from = Vector3.get_zero();
				this.menuParts[i].get_transform().set_eulerAngles(this.menuParts[i].tweenRot.from);
				if (num <= this.footerNo)
				{
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(num * -25));
				}
				else if (num >= this.upperNo)
				{
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(25 * this.footerNo + -25 * (num - (this.upperNo - 1))));
				}
				this.nowRotateZ[i] = this.menuParts[i].tweenRot.to.z;
				this.menuParts[i].tweenRot.delay = 0.3f;
				this.menuParts[i].tweenRot.duration = 0.3f;
			}
			this.PlayTween.Play(true);
		}

		public void MenuExit()
		{
			this.isOpen = false;
			this.swipeEvent.set_enabled(false);
			this.cancelTouch.set_enabled(false);
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - this.keyContoroller.Index, 0f, (float)(this.menuParts.Length - 1));
				this.menuParts[i].tweenRot.from = this.menuParts[i].get_transform().get_eulerAngles();
				this.menuParts[i].tweenRot.from = Vector3.get_zero();
				this.menuParts[i].get_transform().set_eulerAngles(this.menuParts[i].tweenRot.from);
				if (num <= this.footerNo)
				{
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(num * -25));
				}
				if (num >= this.upperNo)
				{
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(25 * this.footerNo + -25 * (num - (this.upperNo - 1))));
				}
				this.menuParts[i].tweenRot.delay = 0f;
				this.menuParts[i].tweenRot.duration = 0.3f;
			}
			this.PlayTween.Play(false);
		}

		public void moveCursol()
		{
			int num = (int)Util.LoopValue(this.keyContoroller.Index, 0f, (float)(this.menuParts.Length - 1));
			bool flag = this.keyContoroller.prevIndexChangeValue > 0;
			this.SetMenuStates(num);
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num2 = i - num;
				if (num2 < 0)
				{
					num2 = this.menuParts.Length + num2;
				}
				this.menuParts[i].tweenRot = this.menuParts[i].GetComponent<TweenRotation>();
				Vector3 vector = new Vector3(0f, 0f, this.nowRotateZ[i]);
				int num3 = 25;
				if (flag)
				{
					if (num2 == this.footerNo)
					{
						num3 = 185;
					}
				}
				else
				{
					num3 *= -1;
					if (num2 == this.upperNo)
					{
						num3 = -185;
					}
				}
				this.menuParts[i].tweenRot.from = new Vector3(0f, 0f, this.nowRotateZ[i]);
				this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, this.nowRotateZ[i] + (float)num3);
				this.nowRotateZ[i] = this.nowRotateZ[i] + (float)num3;
				this.menuParts[i].tweenRot.ResetToBeginning();
				this.menuParts[i].tweenRot.delay = 0f;
				this.menuParts[i].tweenRot.PlayForward();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		private void SetMenuStates(int offset)
		{
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(offset + i, 0f, (float)(this.menuParts.Length - 1));
				if (i == 0 && this.menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					this.menuParts[offset].SetMenuState(CommandMenuParts.MenuState.Forcus);
				}
				else if (this.menuParts[num].menuState == CommandMenuParts.MenuState.Disable)
				{
					this.menuParts[num].updateSprite(false);
				}
				else
				{
					this.menuParts[num].SetMenuState(CommandMenuParts.MenuState.NonForcus);
				}
			}
		}

		public void SetMissionState(MissionStates state)
		{
			CommandMenuParts commandMenuParts = this.menuParts[3];
			if (state == MissionStates.RUNNING)
			{
				commandMenuParts.MenuNameStr = "menu_stop";
			}
			else
			{
				commandMenuParts.MenuNameStr = "menu_expedition";
			}
			this.SetMenuStates(this.myOffset);
		}

		public void setFocus()
		{
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				if (this.menuParts[i].menuState != CommandMenuParts.MenuState.Disable)
				{
					if (i == this.keyContoroller.Index)
					{
						this.menuParts[i].SetMenuState(CommandMenuParts.MenuState.Forcus);
					}
					else
					{
						this.menuParts[i].SetMenuState(CommandMenuParts.MenuState.NonForcus);
					}
				}
			}
		}

		private void SetMenuBaseParent(CommandMenuParts menu)
		{
			this.MenuBaseSprite.get_transform().SetParent(menu.get_transform().GetChild(0));
			this.MenuBaseSprite.spriteName = menu.MenuNameStr + "off";
		}

		private void OnDestroy()
		{
			this.Menus = null;
			this.enterMenuMove = null;
			this.menuParts = null;
			this.keyContoroller = null;
			this.nowRotateZ = null;
			this.cancelTouch = null;
			this.swipeEvent = null;
			this.PlayTween = null;
			this.MenuBaseSprite = null;
		}
	}
}
