using Common.Enum;
using KCV.Display;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class RotateMenu_Strategy : MonoBehaviour
	{
		public GameObject Menus;

		public GameObject[] menuButton;

		public bool[] MenuEnable;

		private string[] MenuButtonName;

		private UISprite[] MenuButtonSprite;

		public MissionStates missionState;

		public KeyControl keyContoroller;

		public List<GameObject> Buttons;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		private int myOffset;

		private int a;

		[SerializeField]
		private BoxCollider2D cancelTouch;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		private void Start()
		{
			this.swipeEvent.set_enabled(false);
			this.cancelTouch.set_enabled(false);
		}

		public void Init(KeyControl key)
		{
			this.keyContoroller = key;
			this.keyContoroller.setMinMaxIndex(0, this.menuButton.Length - 1);
			this.keyContoroller.Index = 0;
			this.keyContoroller.setChangeValue(-1f, 0f, 1f, 0f);
			this.MenuButtonName = new string[this.menuButton.Length];
			this.MenuButtonSprite = new UISprite[this.menuButton.Length];
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				this.MenuButtonSprite[i] = this.menuButton[i].GetComponentInChildren<UISprite>();
				this.MenuButtonName[i] = this.MenuButtonSprite[i].spriteName;
				int num = this.MenuButtonSprite[i].spriteName.LastIndexOf("_");
				this.MenuButtonName[i] = this.MenuButtonName[i].Substring(0, num + 1);
			}
			this.nowRotateZ = new float[this.menuButton.Length];
			this.MenuEnable = new bool[this.menuButton.Length];
			this.upperNo = this.menuButton.Length - 3;
			this.footerNo = this.menuButton.Length - 4;
		}

		public void MenuEnter(int offset)
		{
			this.swipeEvent.set_enabled(true);
			this.cancelTouch.set_enabled(true);
			this.myOffset = offset;
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, null);
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				if (i == 0 && this.MenuEnable[offset])
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = true;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[offset] + "on";
				}
				else
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = false;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[(int)Util.LoopValue(i + offset, 0f, 6f)] + "off";
				}
				TweenRotation component = this.menuButton[i].GetComponent<TweenRotation>();
				component.from = default(Vector3);
				this.menuButton[i].get_transform().set_eulerAngles(component.from);
				if (i <= this.footerNo)
				{
					component.to = new Vector3(0f, 0f, (float)(i * -25));
				}
				if (i >= this.upperNo)
				{
					component.to = new Vector3(0f, 0f, (float)(25 * this.footerNo + -25 * (i - this.upperNo)));
				}
				this.nowRotateZ[i] = component.to.z;
				component.delay = 0.3f;
				component.duration = 0.3f;
			}
			for (int j = 0; j < this.Buttons.get_Count(); j++)
			{
				this.Buttons.get_Item(j).GetComponent<UISprite>().set_enabled(true);
				this.Buttons.get_Item(j).GetComponent<TweenScale>().delay = 0.6f;
			}
			base.get_gameObject().GetComponent<UIPlayTween>().Play(true);
			for (int k = 0; k < this.Buttons.get_Count(); k++)
			{
				this.Buttons.get_Item(k).GetComponent<TweenScale>().delay = 0f;
				this.Buttons.get_Item(k).GetComponent<TweenScale>().from = new Vector3(0.7f, 0.7f, 0.7f);
			}
		}

		public void MenuExit()
		{
			this.swipeEvent.set_enabled(false);
			this.cancelTouch.set_enabled(false);
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				TweenRotation component = this.menuButton[i].GetComponent<TweenRotation>();
				component.from = default(Vector3);
				this.menuButton[i].get_transform().set_eulerAngles(component.from);
				if (i <= 3)
				{
					component.to = new Vector3(0f, 0f, (float)(i * -25));
				}
				if (i >= 4)
				{
					component.to = new Vector3(0f, 0f, (float)(75 + -25 * (i - 4)));
				}
				component.delay = 0f;
				component.duration = 0.3f;
			}
			for (int j = 0; j < this.Buttons.get_Count(); j++)
			{
				this.Buttons.get_Item(j).GetComponent<TweenScale>().delay = 0f;
				this.Buttons.get_Item(j).GetComponent<TweenScale>().from = new Vector3(0f, 0f, 0f);
			}
			base.get_gameObject().GetComponent<UIPlayTween>().Play(false);
		}

		public void moveCursol()
		{
			int num = (int)Util.LoopValue(this.keyContoroller.Index - this.myOffset, 0f, 6f);
			bool flag = this.keyContoroller.prevIndexChangeValue > 0;
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				int num2 = i - num;
				if (num2 < 0)
				{
					num2 = this.menuButton.Length + num2;
				}
				TweenRotation component = this.menuButton[i].GetComponent<TweenRotation>();
				Vector3 vector = new Vector3(0f, 0f, this.nowRotateZ[i]);
				if (num2 == 0 && this.MenuEnable[(int)Util.LoopValue(this.myOffset + i, 0f, 6f)])
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = true;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[(int)Util.LoopValue(this.myOffset + i, 0f, 6f)] + "on";
				}
				else
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = false;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[(int)Util.LoopValue(this.myOffset + i, 0f, 6f)] + "off";
				}
				int num3 = 25;
				if (flag)
				{
					if (num2 == this.footerNo)
					{
						num3 = 210;
					}
				}
				else
				{
					num3 *= -1;
					if (num2 == this.upperNo)
					{
						num3 = -210;
					}
				}
				component.from = new Vector3(0f, 0f, this.nowRotateZ[i]);
				component.to = new Vector3(0f, 0f, this.nowRotateZ[i] + (float)num3);
				this.nowRotateZ[i] = this.nowRotateZ[i] + (float)num3;
				component.ResetToBeginning();
				component.delay = 0f;
				component.PlayForward();
			}
		}

		public void changeButtonAlpha()
		{
		}

		public void SetMissionState(MissionStates state)
		{
			if (state == MissionStates.RUNNING)
			{
				this.MenuButtonName[3] = "menu_stop_";
			}
			else
			{
				this.MenuButtonName[3] = "menu_expedition_";
			}
			this.missionState = state;
			if (this.myOffset == 3)
			{
				string text = string.Empty;
				text = ((!this.MenuEnable[0]) ? "off" : "on");
				this.MenuButtonSprite[0].spriteName = this.MenuButtonName[3] + text;
			}
		}

		private void OnDestroy()
		{
			this.Menus = null;
			this.menuButton = null;
			this.MenuEnable = null;
			this.MenuButtonName = null;
			this.MenuButtonSprite = null;
			this.keyContoroller = null;
			if (this.Buttons != null)
			{
				this.Buttons.Clear();
			}
			this.Buttons = null;
			this.nowRotateZ = null;
			this.cancelTouch = null;
			this.swipeEvent = null;
		}
	}
}
