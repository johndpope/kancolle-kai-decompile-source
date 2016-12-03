using local.managers;
using System;
using UnityEngine;

namespace KCV.Mission.Header
{
	public class UIMissionHeader : MonoBehaviour
	{
		private const int CIRCLEMODE_ROT_SPD = 10;

		private Action mOnClickCircleButtonCallBack;

		[SerializeField]
		private Transform mHeaderCircle;

		[SerializeField]
		private UILabel mLabelDay;

		[SerializeField]
		private UILabel mLabelFuelValue;

		[SerializeField]
		private UILabel mLabelAmmoValue;

		[SerializeField]
		private UILabel mLabelSteelValue;

		[SerializeField]
		private UILabel mLabelBauxiteValue;

		[SerializeField]
		private UILabel mLabelBuildKitValue;

		[SerializeField]
		private UILabel mLabelRepairKitValue;

		[SerializeField]
		private UILabel mLabelTransportCraftValue;

		[SerializeField]
		private UISprite mSpriteAreaName;

		private void Update()
		{
			this.mHeaderCircle.get_transform().Rotate(new Vector3(0f, 0f, 10f) * -Time.get_deltaTime());
		}

		public void Initialize(MissionManager manager, Action OnClickCircleButtonCallBack)
		{
			this.mOnClickCircleButtonCallBack = OnClickCircleButtonCallBack;
			this.Refresh(manager);
		}

		public void Refresh(MissionManager manager)
		{
			this.mLabelDay.text = string.Concat(new string[]
			{
				manager.DatetimeString.Year,
				"の年\u3000",
				manager.DatetimeString.Month,
				" ",
				manager.DatetimeString.Day,
				" 日"
			});
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Fuel)
			{
				this.mLabelFuelValue.color = Color.get_yellow();
			}
			else
			{
				this.mLabelFuelValue.color = Color.get_white();
			}
			this.mLabelFuelValue.text = manager.Material.Fuel.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Ammo)
			{
				this.mLabelAmmoValue.color = Color.get_yellow();
			}
			else
			{
				this.mLabelAmmoValue.color = Color.get_white();
			}
			this.mLabelAmmoValue.text = manager.Material.Ammo.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Steel)
			{
				this.mLabelSteelValue.color = Color.get_yellow();
			}
			else
			{
				this.mLabelSteelValue.color = Color.get_white();
			}
			this.mLabelSteelValue.text = manager.Material.Steel.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Baux)
			{
				this.mLabelBauxiteValue.color = Color.get_yellow();
			}
			else
			{
				this.mLabelBauxiteValue.color = Color.get_white();
			}
			this.mLabelBauxiteValue.text = manager.Material.Baux.ToString();
			this.mLabelBuildKitValue.text = manager.Material.Devkit.ToString();
			this.mLabelRepairKitValue.text = manager.Material.RepairKit.ToString();
			this.mLabelTransportCraftValue.text = manager.TankerCount.ToString() + " 隻";
			this.mSpriteAreaName.spriteName = string.Format("map_txt{0:00}_on", manager.AreaId);
			this.mSpriteAreaName.MakePixelPerfect();
		}

		public void OnClickCircleButton()
		{
			if (this.mOnClickCircleButtonCallBack != null)
			{
				this.mOnClickCircleButtonCallBack.Invoke();
			}
		}
	}
}
