using System;
using UnityEngine;

namespace KCV.Supply
{
	public class UISupplyAmmoIconManager : AbstractUISupplyIconManager
	{
		private const string iconObjName = "ammo";

		private const int ICON_OFFSET_X = 45;

		private const int MAX_ICON_OBJ_COUNT = 80;

		private const int ICON_OFFSET_Y = 205;

		private string[] textureNames = new string[]
		{
			"icon_sizai2_gold",
			"icon_sizai2_red",
			"icon_sizai2_silver",
			"icon_sizai2_gold",
			"icon_sizai2_silver"
		};

		private Vector3 startPos = new Vector3(65f, 0f, 0f);

		protected override int getMaxIconObjCount()
		{
			return 80;
		}

		protected override Vector3 getStartPos()
		{
			return this.startPos;
		}

		protected override string getIconObjName()
		{
			return "ammo";
		}

		protected override int calculateIconCount()
		{
			return (int)(3.0 * Math.Ceiling(this.amount / 25.0));
		}

		protected override GameObject createIconObj(int currentIconObjCount, int i)
		{
			Transform transform = base.get_transform().FindChild("ammo" + currentIconObjCount);
			GameObject gameObject;
			if (transform == null)
			{
				gameObject = base.InstantiateIconObj();
				UISprite component = gameObject.get_transform().FindChild("IconObject/Icon").GetComponent<UISprite>();
				component.get_transform().set_localEulerAngles(new Vector3(0f, 0f, (float)(Random.Range(0, 51) - 25)));
				component.spriteName = this.textureNames[Random.Range(0, this.textureNames.Length)];
				component.MakePixelPerfect();
			}
			else
			{
				gameObject = base.ResetSmoke(transform.get_gameObject());
			}
			int num = (int)Math.Floor((double)currentIconObjCount / 7.0);
			int num2 = currentIconObjCount - 7 * num;
			int num3;
			if ((i >= 0 && i <= 1) || (i >= 6 && i <= 7))
			{
				num3 = Random.Range(0, 6);
			}
			else
			{
				num3 = Random.Range(0, 16);
			}
			Vector3 localPosition = new Vector3((float)(45 - 15 * num2), (float)(205 - num3 + 5 * num));
			gameObject.get_transform().set_localPosition(localPosition);
			int num4 = (int)Math.Floor((double)i / 7.0);
			base.SetIconAnimation(gameObject, i - 7 * num4);
			return gameObject;
		}
	}
}
