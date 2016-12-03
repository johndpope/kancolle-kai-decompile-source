using Common.Enum;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class SortManager : MonoBehaviour
	{
		private UISprite[] _uiSortBtn;

		public bool isControl;

		private KeyControl _keyController;

		private int maxIndex = 4;

		public int sortIndex;

		private string[] stateList;

		private SortKey _nowSortKey;

		public bool isDown;

		public bool isUp;

		private void Start()
		{
		}

		public void init(SortKey SortKey, KeyControl KeyController)
		{
			this.isControl = false;
			this.sortIndex = 0;
			this._nowSortKey = SortKey;
			this._keyController = KeyController;
			this.stateList = new string[4];
			this._uiSortBtn = new UISprite[4];
			for (int i = 0; i < 4; i++)
			{
				this._uiSortBtn[i] = base.get_transform().FindChild(this.stateList[i]).GetComponent<UISprite>();
			}
			this.SetListState();
			this.SetDepth();
		}

		public void ListDown()
		{
			if (!this.isDown)
			{
				this.SetListState();
				this.setSortBtn();
				for (int i = 1; i < 4; i++)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("time", 0.07f);
					hashtable.Add("delay", 0.05f * (float)(i - 1));
					hashtable.Add("y", -40f * (float)i);
					hashtable.Add("easeType", iTween.EaseType.linear);
					hashtable.Add("isLocal", true);
					this._uiSortBtn[i].get_gameObject().MoveTo(hashtable);
				}
				this.isDown = true;
			}
		}

		public void ListUp()
		{
			if (!this.isUp)
			{
				for (int i = 0; i < 4; i++)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("time", 0.1f);
					hashtable.Add("y", 0f);
					hashtable.Add("easeType", iTween.EaseType.linear);
					hashtable.Add("isLocal", true);
					hashtable.Add("oncomplete", "OnAnimationComp");
					hashtable.Add("oncompletetarget", base.get_gameObject());
					this._uiSortBtn[i].get_gameObject().MoveTo(hashtable);
				}
				this.isUp = true;
			}
		}

		public void OnAnimationComp()
		{
			this.unsetSortSelect();
			this.sortIndex = 0;
			this.isDown = false;
			this.isUp = false;
		}

		private void SetListState()
		{
			switch (this._nowSortKey)
			{
			case SortKey.LEVEL:
				this.stateList[0] = "Level";
				this.stateList[1] = "Ship";
				this.stateList[2] = "New";
				this.stateList[3] = "Damage";
				break;
			case SortKey.SHIPTYPE:
				this.stateList[0] = "Ship";
				this.stateList[1] = "New";
				this.stateList[2] = "Damage";
				this.stateList[3] = "Level";
				break;
			case SortKey.DAMAGE:
				this.stateList[0] = "Damage";
				this.stateList[1] = "Level";
				this.stateList[2] = "Ship";
				this.stateList[3] = "New";
				break;
			case SortKey.NEW:
				this.stateList[0] = "New";
				this.stateList[1] = "Damage";
				this.stateList[2] = "Level";
				this.stateList[3] = "Ship";
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				this._uiSortBtn[i] = base.get_transform().FindChild(this.stateList[i]).GetComponent<UISprite>();
			}
		}

		public void UpdateSortKey(SortKey SortKey)
		{
			this._nowSortKey = SortKey;
		}

		public string GetSortKey()
		{
			return this.stateList[this.sortIndex];
		}

		public void SetControl(bool enabled)
		{
			this.isControl = enabled;
		}

		private void SetDepth()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.sortIndex == i)
				{
					this._uiSortBtn[i].depth = 51;
				}
				else
				{
					this._uiSortBtn[i].depth = 50;
				}
			}
		}

		private void setSortBtn()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.sortIndex == i)
				{
					Color color = new Color(0.6f, 0.6f, 1f);
					this._uiSortBtn[i].color = color;
				}
				else
				{
					Color color2 = new Color(1f, 1f, 1f);
					this._uiSortBtn[i].color = color2;
				}
			}
		}

		private void unsetSortSelect()
		{
			Color color = new Color(1f, 1f, 1f);
			for (int i = 0; i < 4; i++)
			{
				this._uiSortBtn[i].color = color;
			}
		}

		public void MoveSelectSort(string type)
		{
			if (type == "Up")
			{
				this.sortIndex--;
			}
			else
			{
				this.sortIndex++;
			}
			this.sortIndex = Mathe.MinMax2Rev(this.sortIndex, 0, this.maxIndex - 1);
			this.SetDepth();
			this.setSortBtn();
		}
	}
}
