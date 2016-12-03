using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Paramerter : MonoBehaviour
	{
		[SerializeField]
		private UILabel[] ParamLabels;

		public void SetParams(ShipModel ship)
		{
			this.ParamLabels[0].textInt = ship.MaxHp;
			this.ParamLabels[1].textInt = ship.Karyoku;
			this.ParamLabels[2].textInt = ship.Soukou;
			this.ParamLabels[3].textInt = ship.Raisou;
			this.ParamLabels[4].textInt = ship.Kaihi;
			this.ParamLabels[5].textInt = ship.Taiku;
			this.ParamLabels[6].textInt = ship.TousaiMaxAll;
			this.ParamLabels[7].textInt = ship.Taisen;
			this.ParamLabels[8].text = this.GetSokuText(ship.Soku);
			this.ParamLabels[9].textInt = ship.Sakuteki;
			this.ParamLabels[10].text = this.GetLengText(ship.Leng);
			this.ParamLabels[11].textInt = ship.Lucky;
		}

		private string GetSokuText(int value)
		{
			if (value == 10)
			{
				return "高速";
			}
			return "低速";
		}

		private string GetLengText(int value)
		{
			string result;
			switch (value)
			{
			case 0:
				result = "無";
				break;
			case 1:
				result = "短";
				break;
			case 2:
				result = "中";
				break;
			case 3:
				result = "長";
				break;
			case 4:
				result = "超長";
				break;
			default:
				result = string.Empty;
				break;
			}
			return result;
		}

		private void OnDestroy()
		{
			this.ParamLabels = null;
		}
	}
}
