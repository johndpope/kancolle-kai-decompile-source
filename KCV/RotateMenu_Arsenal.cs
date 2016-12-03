using KCV.Display;
using System;
using UnityEngine;

namespace KCV
{
	public class RotateMenu_Arsenal : MonoBehaviour
	{
		private class MenuObject
		{
			public UISprite sprite;

			public int nowPos;

			public TweenRotation tr;

			public UIButton button;
		}

		public GameObject Menus;

		public GameObject[] menuButton;

		private RotateMenu_Arsenal.MenuObject[] menuObjects;

		private string[] posSpriteName;

		private int cursolPos;

		private string[] MenuButtonName;

		public KeyControl keyContoroller;

		private float[] nowRotateZ;

		private int upperNo;

		private int footerNo;

		private int posZeroNo;

		[SerializeField]
		private UIDisplaySwipeEventRegion swipeEvent;

		private void Start()
		{
			this.swipeEvent.set_enabled(false);
		}

		public void Init(KeyControl key)
		{
			this.keyContoroller = key;
			this.keyContoroller.setMinMaxIndex(0, this.menuButton.Length - 1);
			this.keyContoroller.Index = 0;
			this.keyContoroller.setChangeValue(-1f, 0f, 1f, 0f);
			this.MenuButtonName = new string[this.menuButton.Length];
			this.menuObjects = new RotateMenu_Arsenal.MenuObject[this.menuButton.Length];
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				this.menuObjects[i] = new RotateMenu_Arsenal.MenuObject();
				this.menuObjects[i].sprite = this.menuButton[i].GetComponentInChildren<UISprite>();
				this.MenuButtonName[i] = this.menuObjects[i].sprite.spriteName;
				this.menuObjects[i].nowPos = i;
				this.menuObjects[i].tr = this.menuButton[i].GetComponent<TweenRotation>();
				this.menuObjects[i].button = this.menuButton[i].GetComponent<UIButton>();
			}
			this.posSpriteName = new string[5];
			this.posSpriteName[0] = this.MenuButtonName[0];
			this.posSpriteName[1] = this.MenuButtonName[1];
			this.posSpriteName[2] = this.MenuButtonName[2];
			this.posSpriteName[3] = this.MenuButtonName[5];
			this.posSpriteName[4] = this.MenuButtonName[6];
			this.nowRotateZ = new float[this.menuButton.Length];
			this.upperNo = this.menuButton.Length - 3;
			this.footerNo = this.menuButton.Length - 4;
			this.posZeroNo = 0;
			this.cursolPos = 0;
		}

		private void Update()
		{
		}

		public void MenuEnter()
		{
			this.swipeEvent.set_enabled(true);
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				if (i == 0)
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = true;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[0] + "_on";
				}
				else
				{
					this.menuButton[i].GetComponent<UIButton>().isEnabled = false;
					this.menuButton[i].GetComponentInChildren<UISprite>().spriteName = this.MenuButtonName[i];
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
			base.get_gameObject().GetComponent<UIPlayTween>().DelayAction(0.8f, delegate
			{
				base.get_gameObject().GetComponent<UIPlayTween>().Play(true);
			});
		}

		public void MenuExit()
		{
			this.swipeEvent.set_enabled(false);
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
			base.get_gameObject().GetComponent<UIPlayTween>().Play(false);
		}

		public void moveCursol(bool isDown)
		{
			int num = (!isDown) ? -1 : 1;
			this.posZeroNo += num;
			this.cursolPos += num;
			this.posZeroNo = (int)Util.LoopValue(this.posZeroNo, 0f, 6f);
			this.cursolPos = (int)Util.LoopValue(this.cursolPos, 0f, 4f);
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				int num2 = i - this.posZeroNo;
				if (num2 < 0)
				{
					num2 = this.menuButton.Length + num2;
				}
				Vector3 vector = new Vector3(0f, 0f, this.nowRotateZ[i]);
				if (num2 == 0)
				{
					this.menuObjects[i].button.isEnabled = true;
					this.posZeroNo = i;
					this.menuObjects[i].sprite.spriteName = this.posSpriteName[this.cursolPos] + "_on";
				}
				else
				{
					this.menuObjects[i].button.isEnabled = false;
					this.menuObjects[i].sprite.spriteName = this.MenuButtonName[i];
					if (num2 == 1)
					{
						this.menuObjects[i].sprite.spriteName = this.posSpriteName[(int)Util.LoopValue(this.cursolPos + 1, 0f, 4f)];
					}
					if (num2 == 2)
					{
						this.menuObjects[i].sprite.spriteName = this.posSpriteName[(int)Util.LoopValue(this.cursolPos + 2, 0f, 4f)];
					}
					if (num2 == 5)
					{
						this.menuObjects[i].sprite.spriteName = this.posSpriteName[(int)Util.LoopValue(this.cursolPos + 3, 0f, 4f)];
					}
					if (num2 == 6)
					{
						this.menuObjects[i].sprite.spriteName = this.posSpriteName[(int)Util.LoopValue(this.cursolPos + 4, 0f, 4f)];
					}
				}
				int num3 = 25;
				if (isDown)
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
				this.menuObjects[i].tr.from = new Vector3(0f, 0f, this.nowRotateZ[i]);
				this.menuObjects[i].tr.to = new Vector3(0f, 0f, this.nowRotateZ[i] + (float)num3);
				this.nowRotateZ[i] = this.nowRotateZ[i] + (float)num3;
				this.menuObjects[i].tr.ResetToBeginning();
				this.menuObjects[i].tr.delay = 0f;
				this.menuObjects[i].tr.PlayForward();
			}
		}

		public void changeButtonAlpha()
		{
		}

		public void updateSpriteName()
		{
			for (int i = 0; i < this.menuButton.Length; i++)
			{
				this.menuObjects[i].sprite.spriteName = this.posSpriteName[this.cursolPos] + ((!this.menuObjects[i].button.isEnabled) ? string.Empty : "_on");
			}
		}
	}
}
