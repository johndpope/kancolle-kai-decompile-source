using Common.Struct;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyMonthWeekBonus : MonoBehaviour
	{
		[SerializeField]
		private UILabel Title;

		[SerializeField]
		private UILabel MonthName;

		[SerializeField]
		private UILabel[] MaterialNums;

		[SerializeField]
		private UISprite MaterialIcon;

		public void SetLabels(string monthName, MaterialInfo materialInfo)
		{
			this.Title.text = ((monthName.get_Length() != 2) ? "新しい月、\u3000\u3000\u3000\u3000となりました！" : "新しい月、\u3000\u3000\u3000となりました！");
			this.MonthName.text = monthName;
			UILabel[] materialNums = this.MaterialNums;
			for (int i = 0; i < materialNums.Length; i++)
			{
				UILabel uILabel = materialNums[i];
				uILabel.get_transform().get_parent().SetActive(true);
			}
			this.MaterialNums[0].text = " × " + materialInfo.Fuel.ToString();
			this.MaterialNums[1].text = " × " + materialInfo.Ammo.ToString();
			this.MaterialNums[2].text = " × " + materialInfo.Steel.ToString();
			this.MaterialNums[3].text = " × " + materialInfo.Baux.ToString();
			this.MaterialNums[4].text = " × " + materialInfo.Devkit.ToString();
		}

		public void SetLabelsWeek(MaterialInfo materialInfo)
		{
			int num = 0;
			int num2 = 0;
			if (materialInfo.Fuel > 0)
			{
				num = 1;
				num2 = materialInfo.Fuel;
			}
			else if (materialInfo.Ammo > 0)
			{
				num = 2;
				num2 = materialInfo.Ammo;
			}
			else if (materialInfo.Steel > 0)
			{
				num = 3;
				num2 = materialInfo.Steel;
			}
			this.MaterialNums[0].text = " × " + num2.ToString();
			this.MaterialIcon.spriteName = "icon2_m" + num;
		}
	}
}
