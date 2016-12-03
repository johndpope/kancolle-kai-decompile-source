using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class DepthMenu : MonoBehaviour
	{
		public struct NextChecker
		{
			public bool enable;

			public int move;

			public NextChecker(bool b, int i)
			{
				this.enable = b;
				this.move = i;
			}
		}

		public struct MenuChip
		{
			public UIWidget widget;

			public int posNo;
		}

		protected struct MenuData
		{
			public int depth;

			public Color Color;

			public Vector3 Pos;

			public Vector3 Scale;

			public bool enable;
		}

		[SerializeField]
		private GameObject[] Menus;

		public DepthMenu.MenuChip[] MenuChips;

		protected DepthMenu.MenuData[] MenusData;

		public int MenuNum;

		private int[,] usePosNo;

		private int index;

		private bool moving;

		public KeyControl keyCon;

		public int currentPos
		{
			get;
			private set;
		}

		protected void Start()
		{
			this.MenusData = new DepthMenu.MenuData[this.Menus.Length];
			this.MenuChips = new DepthMenu.MenuChip[this.Menus.Length];
			for (int i = 0; i < this.Menus.Length; i++)
			{
				Vector3 localPosition = this.Menus[i].get_transform().get_localPosition();
				Vector3 localScale = this.Menus[i].get_transform().get_localScale();
				this.MenusData[i].Pos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				this.MenusData[i].Scale = new Vector3(localScale.x, localScale.y, localScale.z);
				this.MenuChips[i].posNo = i;
				this.MenuChips[i].widget = this.Menus[i].GetComponent<UIWidget>();
				this.MenusData[i].Color = this.MenuChips[i].widget.color;
				this.MenusData[i].depth = this.MenuChips[i].widget.depth;
				this.MenusData[i].enable = true;
			}
		}

		protected virtual void Init(int menuNum, KeyControl key)
		{
			this.keyCon = key;
			this.MenuNum = menuNum;
			this.usePosNo = new int[,]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					1,
					3,
					7,
					0,
					0,
					0,
					0,
					0
				},
				{
					1,
					3,
					5,
					7,
					0,
					0,
					0,
					0
				},
				{
					1,
					2,
					3,
					7,
					8,
					0,
					0,
					0
				},
				{
					1,
					2,
					3,
					5,
					7,
					8,
					0,
					0
				},
				{
					1,
					2,
					3,
					4,
					6,
					7,
					8,
					0
				},
				{
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8
				}
			};
			for (int i = 0; i < this.Menus.Length; i++)
			{
				if (i >= this.MenuNum)
				{
					this.Menus[i].SetActive(false);
				}
			}
		}

		protected void InitPosition()
		{
			this.currentPos = 0;
			for (int i = 0; i < this.MenuNum; i++)
			{
				int num = i;
				DepthMenu.MenuData menuData = this.MenusData[this.usePosNo[this.MenuNum, num] - 1];
				this.Menus[i].get_transform().set_localPosition(menuData.Pos);
				this.Menus[i].get_transform().set_localScale(menuData.Scale);
				this.MenuChips[i].widget.depth = menuData.depth;
				this.MenuChips[i].widget.color = ((!this.MenusData[i].enable) ? new Color(0.5f, 0.5f, 0.5f, menuData.Color.a) : menuData.Color);
				this.MenuChips[i].posNo = num;
			}
		}

		public void nextMenu(int move)
		{
			if (this.moving)
			{
				return;
			}
			this.moving = true;
			this.processNextMenu(move);
		}

		private void processNextMenu(int move)
		{
			this.currentPos += move;
			while (this.currentPos < 0)
			{
				this.currentPos += this.MenuNum;
			}
			this.currentPos %= this.MenuNum;
			for (int i = 0; i < this.MenuNum; i++)
			{
				int num = (i + this.MenuNum - this.currentPos) % this.MenuNum;
				DepthMenu.MenuData menuData = this.MenusData[this.usePosNo[this.MenuNum, num] - 1];
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", menuData.Pos);
				hashtable.Add("islocal", true);
				if (num == 0)
				{
					hashtable.Add("time", 0.03f);
					hashtable.Add("oncomplete", "OnCompleteHandler");
					hashtable.Add("easetype", iTween.EaseType.linear);
					hashtable.Add("oncompletetarget", base.get_gameObject());
					hashtable.Add("oncompleteparams", new DepthMenu.NextChecker(this.MenusData[i].enable, move));
				}
				else
				{
					hashtable.Add("time", 0.2f);
					hashtable.Add("easetype", iTween.EaseType.easeOutBack);
				}
				iTween.MoveTo(this.Menus[i], hashtable);
				TweenScale.Begin(this.Menus[i], 0.2f, menuData.Scale);
				this.MenuChips[i].widget.depth = menuData.depth;
				this.MenuChips[i].widget.color = ((!this.MenusData[i].enable) ? new Color(0.5f, 0.5f, 0.5f, menuData.Color.a) : menuData.Color);
				this.MenuChips[i].posNo = num;
			}
		}

		public void OnCompleteHandler(DepthMenu.NextChecker nextChecker)
		{
			if (nextChecker.enable)
			{
				this.moving = false;
			}
			else
			{
				this.processNextMenu(nextChecker.move);
				this.keyCon.Index -= nextChecker.move;
				Debug.Log("Index " + this.keyCon.Index);
			}
		}
	}
}
