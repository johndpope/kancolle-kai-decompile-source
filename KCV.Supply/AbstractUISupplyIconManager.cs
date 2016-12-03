using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Supply
{
	public abstract class AbstractUISupplyIconManager : MonoBehaviour
	{
		protected const int ORIGIN_DEPTH = 6;

		protected const int HIDE_ANIMATION_DEPTH = 20;

		protected double amount;

		private int lastIconObjCount;

		private UIPanel panel;

		protected List<GameObject> iconObjList = new List<GameObject>();

		protected abstract Vector3 getStartPos();

		protected abstract int calculateIconCount();

		protected abstract GameObject createIconObj(int currentIconObjCount, int i);

		protected abstract string getIconObjName();

		protected abstract int getMaxIconObjCount();

		public void Awake()
		{
			this.panel = base.GetComponent<UIPanel>();
		}

		public void init(int amount)
		{
			this.amount = (double)amount;
			this.panel.depth = 6;
			base.get_transform().localPosition(this.getStartPos());
			this.processIcons();
		}

		private void processIcons()
		{
			int num = this.calculateIconCount();
			if (num > this.lastIconObjCount)
			{
				int num2 = num - this.lastIconObjCount;
				for (int i = 0; i < num2; i++)
				{
					if (this.iconObjList.get_Count() < this.getMaxIconObjCount())
					{
						this.iconObjList.Add(this.createIconObj(this.iconObjList.get_Count(), i));
					}
				}
			}
			else if (num < this.lastIconObjCount)
			{
				for (int j = num; j < this.lastIconObjCount; j++)
				{
					if (j < this.getMaxIconObjCount())
					{
						int num3 = this.iconObjList.get_Count() - 1;
						GameObject iconObj = this.iconObjList.get_Item(num3);
						this.ProcessEachIconCancelAnimation(iconObj);
						this.iconObjList.RemoveAt(num3);
					}
				}
			}
			this.lastIconObjCount = num;
		}

		protected GameObject InstantiateIconObj()
		{
			GameObject gameObject = Util.InstantiateGameObject(Resources.Load("Prefabs/Supply/SupplyIcon") as GameObject, base.get_transform());
			gameObject.set_name(this.getIconObjName() + this.iconObjList.get_Count());
			return gameObject;
		}

		protected GameObject ResetSmoke(GameObject iconObj)
		{
			UISprite component = iconObj.get_transform().FindChild("IconObject/Smoke").GetComponent<UISprite>();
			component.alpha = 0f;
			return iconObj;
		}

		protected void SetIconAnimation(GameObject iconObj, int num)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Stop();
			component.Play("SupplyIcon" + (num + 1));
		}

		public void ProcessConsumingAnimation()
		{
			this.panel.depth = 20;
			base.Invoke("OnCompleteConsumingAnimation", 0f);
			this.iconObjList.ForEach(delegate(GameObject iconObj)
			{
				this.ProcessEachIconConsumingAnimation(iconObj);
			});
			this.iconObjList.Clear();
			this.lastIconObjCount = 0;
		}

		protected void ProcessEachIconConsumingAnimation(GameObject iconObj)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Play("SupplyIconEnd");
			UISprite component2 = iconObj.get_transform().FindChild("IconObject/Icon").GetComponent<UISprite>();
			Vector3[] array = new Vector3[]
			{
				new Vector3(0f, 0f, (float)XorRandom.GetILim(180, 270)),
				new Vector3(0f, 0f, (float)XorRandom.GetILim(-270, -180))
			};
			component2.get_transform().get_gameObject().RotatoTo(array[XorRandom.GetILim(0, 1)], 1f, null);
		}

		public void ProcessCancelAnimation()
		{
			this.iconObjList.ForEach(delegate(GameObject iconObj)
			{
				this.ProcessEachIconCancelAnimation(iconObj);
			});
			this.iconObjList.Clear();
			this.lastIconObjCount = 0;
		}

		protected void ProcessEachIconCancelAnimation(GameObject iconObj)
		{
			Animation component = iconObj.GetComponent<Animation>();
			component.Play("SupplyIconLost");
		}

		public void OnCompleteConsumingAnimation()
		{
			SupplyMainManager.Instance.change_2_SHIP_RECOVERY_ANIMATION();
		}
	}
}
