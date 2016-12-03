using Server_Models;
using System;

namespace local.models
{
	public class __FStoreItemModel__ : FurnitureModel
	{
		public __FStoreItemModel__(Mst_furniture mst, string description) : base(mst, description)
		{
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}", (!base.IsPossession()) ? string.Empty : "[所持] ");
			text += base.ToString();
			text += string.Format(" レア度:{0} 必要コイン数:{1}", base.Rarity, base.Price);
			return text + string.Format("{0}", (!base.IsNeedWorker()) ? string.Empty : " [家具職人]");
		}
	}
}
