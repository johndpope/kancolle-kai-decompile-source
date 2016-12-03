using System;
using UnityEngine;

namespace KCV.Supply
{
	public class UISupplyFuelIconManager : AbstractUISupplyIconManager
	{
		private const string iconObjName = "fuel";

		private const int ICON_OFFSET_X = 35;

		private const int ICON_OFFSET_Y = 200;

		private const int MAX_ICON_OBJ_COUNT = 40;

		private int[] showPoint = new int[]
		{
			0,
			4,
			3,
			5,
			2,
			1
		};

		private int[] showPointY = new int[]
		{
			2,
			5,
			0,
			6,
			1,
			8
		};

		private int[] showDepth = new int[]
		{
			3,
			2,
			5,
			1,
			4,
			0
		};

		private Vector3 startPos = new Vector3(-75f, 5f, 0f);

		private string[] textureNames = new string[]
		{
			"icon_sizai1_black",
			"icon_sizai1_brown",
			"icon_sizai1_green",
			"icon_sizai1_pink",
			"icon_sizai1_ygreen"
		};

		protected override int getMaxIconObjCount()
		{
			return 40;
		}

		protected override Vector3 getStartPos()
		{
			return this.startPos;
		}

		protected override string getIconObjName()
		{
			return "fuel";
		}

		protected override int calculateIconCount()
		{
			return (int)(3.0 * Math.Ceiling(this.amount / 50.0));
		}

		protected override GameObject createIconObj(int currentIconObjCount, int i)
		{
			Transform transform = base.get_transform().FindChild("fuel" + currentIconObjCount);
			GameObject gameObject;
			if (transform == null)
			{
				gameObject = base.InstantiateIconObj();
				UISprite component = gameObject.get_transform().FindChild("IconObject/Icon").GetComponent<UISprite>();
				component.spriteName = this.textureNames[Random.Range(0, this.textureNames.Length)];
				component.MakePixelPerfect();
			}
			else
			{
				gameObject = base.ResetSmoke(transform.get_gameObject());
			}
			int num = (int)Math.Floor((double)currentIconObjCount / 6.0);
			int num2 = this.showPoint[currentIconObjCount - 6 * num];
			Vector3 localPosition = new Vector3((float)(35 - 13 * num2), (float)(200 + this.showPointY[num2] + 24 * num));
			gameObject.get_transform().set_localPosition(localPosition);
			UISprite component2 = gameObject.get_transform().FindChild("IconObject/Icon").GetComponent<UISprite>();
			int depth = component2.depth;
			component2.depth = depth + this.showDepth[num2];
			int num3 = (int)Math.Floor((double)i / 6.0);
			base.SetIconAnimation(gameObject, i - 6 * num3);
			return gameObject;
		}
	}
}
