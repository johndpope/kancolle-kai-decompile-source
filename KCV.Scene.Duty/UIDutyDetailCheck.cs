using Common.Enum;
using KCV.Scene.Port;
using KCV.View;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyDetailCheck : BoardDialog
	{
		public enum SelectType
		{
			Positive,
			Negative
		}

		[SerializeField]
		private UILabel mLabelTitle;

		[SerializeField]
		private UILabel mLabelDescription;

		[SerializeField]
		private UILabel mLabelFuelValue;

		[SerializeField]
		private UILabel mLabelSteelValue;

		[SerializeField]
		private UILabel mLabelAmmoValue;

		[SerializeField]
		private UILabel mLabelBauxiteValue;

		[SerializeField]
		private UISprite[] mSprites_RewardMaterials;

		private Action mClosedCallBack;

		private DutyModel mDutyModel;

		private KeyControl mKeyController;

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.mKeyController = null;
					this.mClosedCallBack.Invoke();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.mKeyController = null;
					this.mClosedCallBack.Invoke();
				}
			}
		}

		public KeyControl Show()
		{
			base.Show();
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			return this.mKeyController;
		}

		public void SetDutyDetailCheckClosedCallBack(Action action)
		{
			this.mClosedCallBack = action;
		}

		public void Initialize(DutyModel dutyModel)
		{
			this.mDutyModel = dutyModel;
			this.mLabelTitle.text = dutyModel.Title;
			this.mLabelDescription.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(24, 1, dutyModel.Description);
			this.mLabelFuelValue.text = dutyModel.Fuel.ToString();
			this.mLabelSteelValue.text = dutyModel.Steel.ToString();
			this.mLabelAmmoValue.text = dutyModel.Ammo.ToString();
			this.mLabelBauxiteValue.text = dutyModel.Baux.ToString();
			int num = 0;
			enumMaterialCategory[] array = new enumMaterialCategory[]
			{
				enumMaterialCategory.Build_Kit,
				enumMaterialCategory.Dev_Kit,
				enumMaterialCategory.Repair_Kit,
				enumMaterialCategory.Revamp_Kit
			};
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = this.mDutyModel.RewardMaterials.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					if (Enumerable.Contains<enumMaterialCategory>(array, current.get_Key()) && 0 < current.get_Value() && num < this.mSprites_RewardMaterials.Length)
					{
						this.mSprites_RewardMaterials[num].spriteName = string.Format("item_{0}", this.MaterialEnumToMasterId(current.get_Key()));
						num++;
					}
				}
			}
			for (int i = num; i < this.mSprites_RewardMaterials.Length; i++)
			{
				this.mSprites_RewardMaterials[i].spriteName = "none";
			}
		}

		public void Hide(Action action)
		{
			base.Hide(action);
		}

		public void OnClickClose()
		{
			this.mKeyController = null;
			this.mClosedCallBack.Invoke();
		}

		private int MaterialEnumToMasterId(enumMaterialCategory category)
		{
			int result = 0;
			switch (category)
			{
			case enumMaterialCategory.Fuel:
				result = 31;
				break;
			case enumMaterialCategory.Bull:
				result = 32;
				break;
			case enumMaterialCategory.Steel:
				result = 33;
				break;
			case enumMaterialCategory.Bauxite:
				result = 34;
				break;
			case enumMaterialCategory.Build_Kit:
				result = 2;
				break;
			case enumMaterialCategory.Repair_Kit:
				result = 1;
				break;
			case enumMaterialCategory.Dev_Kit:
				result = 3;
				break;
			case enumMaterialCategory.Revamp_Kit:
				result = 4;
				break;
			}
			return result;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelDescription);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelFuelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelSteelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelAmmoValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabelBauxiteValue);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mSprites_RewardMaterials);
			this.mClosedCallBack = null;
			this.mDutyModel = null;
			this.mKeyController = null;
		}
	}
}
