using KCV.CommandMenu;
using KCV.Display;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class RotateMenu_Arsenal2 : MonoBehaviour
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

		private int Index;

		public UIDisplaySwipeEventRegion swipeEvent;

		private void Awake()
		{
			this.Index = 0;
			this.menuParts = new CommandMenuParts[6];
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				this.menuParts[i] = this.Menus.get_transform().FindChild("menu0" + i).GetComponent<CommandMenuParts>();
			}
			this.enterMenuMove = this.Menus.GetComponent<TweenPosition>();
		}

		private void Start()
		{
			this.swipeEvent.set_enabled(false);
			base.get_gameObject().GetComponent<UIPlayTween>().Play(false);
		}

		public void Init(KeyControl key)
		{
			this.keyContoroller = key;
			this.nowRotateZ = new float[this.menuParts.Length];
			this.upperNo = this.menuParts.Length - 3;
			this.footerNo = this.menuParts.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			this.swipeEvent.set_enabled(true);
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
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(25 * this.footerNo + -25 * (num - this.upperNo - 1)));
				}
				this.nowRotateZ[i] = this.menuParts[i].tweenRot.to.z;
				this.menuParts[i].tweenRot.delay = 0.3f;
				this.menuParts[i].tweenRot.duration = 0.3f;
			}
			base.get_gameObject().GetComponent<UIPlayTween>().Play(true);
		}

		public void MenuExit()
		{
			this.swipeEvent.set_enabled(false);
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
					this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, (float)(25 * this.footerNo + -25 * (num - this.upperNo)));
				}
				this.menuParts[i].tweenRot.delay = 0f;
				this.menuParts[i].tweenRot.duration = 0.3f;
			}
			base.get_gameObject().GetComponent<UIPlayTween>().Play(false);
		}

		public void moveCursol(bool isDown)
		{
			this.Index = ((!isDown) ? (this.Index - 1) : (this.Index + 1));
			this.Index = (int)Util.LoopValue(this.Index, 0f, 5f);
			this.SetMenuStates(this.Index);
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(i - this.Index, 0f, 5f);
				if (num < 0)
				{
					num = this.menuParts.Length + num;
				}
				this.menuParts[i].tweenRot = this.menuParts[i].GetComponent<TweenRotation>();
				Vector3 vector = new Vector3(0f, 0f, this.nowRotateZ[i]);
				int num2 = 25;
				if (isDown)
				{
					if (num == this.footerNo)
					{
						num2 = 235;
					}
				}
				else
				{
					num2 *= -1;
					if (num == this.upperNo)
					{
						num2 = -235;
					}
				}
				this.menuParts[i].tweenRot.from = new Vector3(0f, 0f, this.nowRotateZ[i]);
				this.menuParts[i].tweenRot.to = new Vector3(0f, 0f, this.nowRotateZ[i] + (float)num2);
				this.nowRotateZ[i] = this.nowRotateZ[i] + (float)num2;
				this.menuParts[i].tweenRot.ResetToBeginning();
				this.menuParts[i].tweenRot.delay = 0f;
				this.menuParts[i].tweenRot.PlayForward();
			}
		}

		private void SetMenuStates(int offset)
		{
			for (int i = 0; i < this.menuParts.Length; i++)
			{
				int num = (int)Util.LoopValue(offset + i, 0f, (float)(this.menuParts.Length - 1));
				if (i == 0 && this.menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					this.menuParts[offset].SetMenuState(CommandMenuParts.MenuState.Forcus);
					this.menuParts[offset].setColider(true);
				}
				else if (this.menuParts[num].menuState != CommandMenuParts.MenuState.Disable)
				{
					this.menuParts[num].SetMenuState(CommandMenuParts.MenuState.NonForcus);
					this.menuParts[offset].setColider(true);
				}
			}
		}

		public void SetColidersEnable(bool isEnable)
		{
			this.menuParts[this.Index].setColider(isEnable);
			this.swipeEvent.set_enabled(isEnable);
		}
	}
}
