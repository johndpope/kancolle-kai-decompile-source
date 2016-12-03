using Common.Enum;
using local.managers;
using local.utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployMaterials : MonoBehaviour
	{
		[SerializeField]
		private UILabel[] MaterialNums;

		public void setMaterials(int[] Nums)
		{
			for (int i = 0; i < this.MaterialNums.Length; i++)
			{
				this.MaterialNums[i].text = Nums[i].ToString();
			}
		}

		public void updateMaterials(int areaID, int tankerCount, EscortDeckManager manager)
		{
			Dictionary<enumMaterialCategory, int> areaResource = Utils.GetAreaResource(areaID, tankerCount, manager);
			this.MaterialNums[0].text = areaResource.get_Item(enumMaterialCategory.Fuel).ToString();
			this.MaterialNums[1].text = areaResource.get_Item(enumMaterialCategory.Steel).ToString();
			this.MaterialNums[2].text = areaResource.get_Item(enumMaterialCategory.Bull).ToString();
			this.MaterialNums[3].text = areaResource.get_Item(enumMaterialCategory.Bauxite).ToString();
		}
	}
}
