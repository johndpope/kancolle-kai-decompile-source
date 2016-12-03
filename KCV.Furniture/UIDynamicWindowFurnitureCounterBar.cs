using KCV.Scene.Port;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureCounterBar : UIDynamicWindowFurniture
	{
		public enum FoodOrDring
		{
			None,
			Beer,
			Sake,
			ItalyWine,
			Wine,
			Food,
			Wisky,
			Juice
		}

		public enum Customer
		{
			A,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			Others
		}

		public enum MenuShift
		{
			A,
			B,
			C,
			D,
			E
		}

		[SerializeField]
		private UITexture mTexture_FoodOrDrink;

		[SerializeField]
		private Texture mTexture2d_Beer;

		[SerializeField]
		private Texture mTexture2d_Sake;

		[SerializeField]
		private Texture mTexture2d_ItalyWine;

		[SerializeField]
		private Texture mTexture2d_Wine;

		[SerializeField]
		private Texture mTexture2d_Food;

		[SerializeField]
		private Texture mTexture2d_Wisky;

		[SerializeField]
		private Texture mTexture2d_Juice;

		private Vector3 mVector3_Beer = new Vector3(-476f, -210f);

		private Vector3 mVector3_Sake = new Vector3(-480f, -195f);

		private Vector3 mVector3_ItalyWine = new Vector3(-485f, -204f);

		private Vector3 mVector3_Wine = new Vector3(-464f, -205f);

		private Vector3 mVector3_Food = new Vector3(-475f, -210f);

		private Vector3 mVector3_Wisky = new Vector3(-470f, -204f);

		private Vector3 mVector3_Juice = new Vector3(-445f, -225f);

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_FoodOrDrink, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Beer, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Sake, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_ItalyWine, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Wine, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Food, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Wisky, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Juice, false);
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			ShipModel flagShip = uiFurnitureModel.GetDeck().GetFlagShip();
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			UIDynamicWindowFurnitureCounterBar.FoodOrDring foodOrDrink = this.RequestFoodOrDring(flagShip, dateTime);
			this.InitializeFoodOrDringTexture(foodOrDrink);
		}

		private void InitializeFoodOrDringTexture(UIDynamicWindowFurnitureCounterBar.FoodOrDring foodOrDrink)
		{
			switch (foodOrDrink)
			{
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.None:
				this.mTexture_FoodOrDrink.mainTexture = null;
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Beer;
				this.mTexture_FoodOrDrink.width = 220;
				this.mTexture_FoodOrDrink.height = 142;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Beer);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Sake;
				this.mTexture_FoodOrDrink.width = 190;
				this.mTexture_FoodOrDrink.height = 105;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Sake);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.ItalyWine:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_ItalyWine;
				this.mTexture_FoodOrDrink.width = 286;
				this.mTexture_FoodOrDrink.height = 150;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_ItalyWine);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Wine;
				this.mTexture_FoodOrDrink.width = 208;
				this.mTexture_FoodOrDrink.height = 105;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Wine);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Food:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Food;
				this.mTexture_FoodOrDrink.width = 264;
				this.mTexture_FoodOrDrink.height = 124;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Food);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Wisky;
				this.mTexture_FoodOrDrink.width = 170;
				this.mTexture_FoodOrDrink.height = 76;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Wisky);
				break;
			case UIDynamicWindowFurnitureCounterBar.FoodOrDring.Juice:
				this.mTexture_FoodOrDrink.mainTexture = this.mTexture2d_Juice;
				this.mTexture_FoodOrDrink.width = 207;
				this.mTexture_FoodOrDrink.height = 68;
				this.mTexture_FoodOrDrink.get_transform().set_localPosition(this.mVector3_Juice);
				break;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDring(ShipModel customerShip, DateTime dateTime)
		{
			UIDynamicWindowFurnitureCounterBar.Customer customer = this.RequestCustomerType(customerShip);
			UIDynamicWindowFurnitureCounterBar.MenuShift menuShift = this.RequestMenuShift(dateTime);
			switch (customer)
			{
			case UIDynamicWindowFurnitureCounterBar.Customer.A:
				return this.RequestFoodOrDringFromMenuForCustomerA(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.B:
				return this.RequestFoodOrDringFromMenuForCustomerB(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.C:
				return this.RequestFoodOrDringFromMenuForCustomerC(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.D:
				return this.RequestFoodOrDringFromMenuForCustomerD(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.E:
				return this.RequestFoodOrDringFromMenuForCustomerE(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.F:
				return this.RequestFoodOrDringFromMenuForCustomerF(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.G:
				return this.RequestFoodOrDringFromMenuForCustomerG(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.H:
				return this.RequestFoodOrDringFromMenuForCustomerH(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.I:
				return this.RequestFoodOrDringFromMenuForCustomerI(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.J:
				return this.RequestFoodOrDringFromMenuForCustomerJ(menuShift);
			case UIDynamicWindowFurnitureCounterBar.Customer.Others:
				return this.RequestFoodOrDringFromMenuForOtherCustomer(menuShift);
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForOtherCustomer(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerJ(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Food;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Food;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerI(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.ItalyWine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.ItalyWine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.ItalyWine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerH(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Juice;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Juice;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerG(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerE(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerF(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wine;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerD(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerC(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Wisky;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerB(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		private UIDynamicWindowFurnitureCounterBar.FoodOrDring RequestFoodOrDringFromMenuForCustomerA(UIDynamicWindowFurnitureCounterBar.MenuShift menuShift)
		{
			switch (menuShift)
			{
			case UIDynamicWindowFurnitureCounterBar.MenuShift.A:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.B:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Beer;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.C:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.D:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			case UIDynamicWindowFurnitureCounterBar.MenuShift.E:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.Sake;
			default:
				return UIDynamicWindowFurnitureCounterBar.FoodOrDring.None;
			}
		}

		public UIDynamicWindowFurnitureCounterBar.Customer RequestCustomerType(ShipModel customerShip)
		{
			int mstId;
			if (customerShip.Yomi == "ひびき")
			{
				mstId = customerShip.MstId;
				if (mstId == 35)
				{
					return UIDynamicWindowFurnitureCounterBar.Customer.D;
				}
				if (mstId == 147 || mstId == 235)
				{
					return UIDynamicWindowFurnitureCounterBar.Customer.H;
				}
			}
			string yomi = customerShip.Yomi;
			if (yomi != null)
			{
				if (UIDynamicWindowFurnitureCounterBar.<>f__switch$map12 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(88);
					dictionary.Add("ちとせ", 0);
					dictionary.Add("きそ", 0);
					dictionary.Add("やましろ", 0);
					dictionary.Add("きりしま", 0);
					dictionary.Add("ほうしょう", 1);
					dictionary.Add("ずいほう", 1);
					dictionary.Add("やまと", 1);
					dictionary.Add("かが", 1);
					dictionary.Add("うんりゅう", 1);
					dictionary.Add("ひりゅう", 1);
					dictionary.Add("みょうこう", 1);
					dictionary.Add("はつはる", 1);
					dictionary.Add("あかし", 1);
					dictionary.Add("しょうほう", 1);
					dictionary.Add("あきつまる", 1);
					dictionary.Add("きくづき", 1);
					dictionary.Add("あまぎ", 1);
					dictionary.Add("いせ", 1);
					dictionary.Add("ひゅうが", 1);
					dictionary.Add("じんつう", 1);
					dictionary.Add("たいげい・りゅうほう", 1);
					dictionary.Add("なち", 2);
					dictionary.Add("あしがら", 2);
					dictionary.Add("むつ", 2);
					dictionary.Add("はやしも", 2);
					dictionary.Add("こんごう", 3);
					dictionary.Add("ちくま", 3);
					dictionary.Add("きさらぎ", 3);
					dictionary.Add("おおよど", 3);
					dictionary.Add("たいほう", 3);
					dictionary.Add("ゆうぐも", 3);
					dictionary.Add("ながなみ", 3);
					dictionary.Add("もがみ", 3);
					dictionary.Add("いそかぜ", 3);
					dictionary.Add("やはぎ", 3);
					dictionary.Add("ビスマルク", 4);
					dictionary.Add("かとり", 4);
					dictionary.Add("い8", 4);
					dictionary.Add("むらさめ", 4);
					dictionary.Add("たつた", 4);
					dictionary.Add("はるな", 5);
					dictionary.Add("ひよう", 5);
					dictionary.Add("くまの", 5);
					dictionary.Add("すずや", 5);
					dictionary.Add("そうりゅう", 5);
					dictionary.Add("ゆうばり", 5);
					dictionary.Add("むらくも", 5);
					dictionary.Add("あまつかぜ", 5);
					dictionary.Add("プリンツ・オイゲン", 5);
					dictionary.Add("ゆー511・ろ500", 5);
					dictionary.Add("じゅんよう", 6);
					dictionary.Add("むさし", 6);
					dictionary.Add("かこ", 6);
					dictionary.Add("あさしも", 6);
					dictionary.Add("い19", 6);
					dictionary.Add("むつき", 7);
					dictionary.Add("やよい", 7);
					dictionary.Add("うづき", 7);
					dictionary.Add("ふみづき", 7);
					dictionary.Add("あかつき", 7);
					dictionary.Add("いかづち", 7);
					dictionary.Add("いなづま", 7);
					dictionary.Add("あけぼの", 7);
					dictionary.Add("おぼろ", 7);
					dictionary.Add("あさしお", 7);
					dictionary.Add("おおしお", 7);
					dictionary.Add("てんりゅう", 7);
					dictionary.Add("しらつゆ", 7);
					dictionary.Add("はるさめ", 7);
					dictionary.Add("あさぐも", 7);
					dictionary.Add("やまぐも", 7);
					dictionary.Add("まいかぜ", 7);
					dictionary.Add("きよしも", 7);
					dictionary.Add("まきぐも", 7);
					dictionary.Add("ながと", 7);
					dictionary.Add("しまかぜ", 7);
					dictionary.Add("ゆきかぜ", 7);
					dictionary.Add("さかわ", 7);
					dictionary.Add("たかなみ", 7);
					dictionary.Add("リットリオ・イタリア", 8);
					dictionary.Add("ローマ", 8);
					dictionary.Add("あきづき", 9);
					dictionary.Add("かすみ", 9);
					dictionary.Add("しぐれ", 9);
					dictionary.Add("あかぎ", 9);
					dictionary.Add("あぶくま", 9);
					dictionary.Add("あきつしま", 9);
					dictionary.Add("かつらぎ", 9);
					UIDynamicWindowFurnitureCounterBar.<>f__switch$map12 = dictionary;
				}
				if (UIDynamicWindowFurnitureCounterBar.<>f__switch$map12.TryGetValue(yomi, ref mstId))
				{
					switch (mstId)
					{
					case 0:
						return UIDynamicWindowFurnitureCounterBar.Customer.A;
					case 1:
						return UIDynamicWindowFurnitureCounterBar.Customer.B;
					case 2:
						return UIDynamicWindowFurnitureCounterBar.Customer.C;
					case 3:
						return UIDynamicWindowFurnitureCounterBar.Customer.D;
					case 4:
						return UIDynamicWindowFurnitureCounterBar.Customer.E;
					case 5:
						return UIDynamicWindowFurnitureCounterBar.Customer.F;
					case 6:
						return UIDynamicWindowFurnitureCounterBar.Customer.G;
					case 7:
						return UIDynamicWindowFurnitureCounterBar.Customer.H;
					case 8:
						return UIDynamicWindowFurnitureCounterBar.Customer.I;
					case 9:
						return UIDynamicWindowFurnitureCounterBar.Customer.J;
					}
				}
			}
			return UIDynamicWindowFurnitureCounterBar.Customer.Others;
		}

		public UIDynamicWindowFurnitureCounterBar.MenuShift RequestMenuShift(DateTime dateTime)
		{
			if (5 <= dateTime.get_Hour() && dateTime.get_Hour() < 19)
			{
				return UIDynamicWindowFurnitureCounterBar.MenuShift.A;
			}
			if (19 <= dateTime.get_Hour() && dateTime.get_Hour() < 21)
			{
				return UIDynamicWindowFurnitureCounterBar.MenuShift.B;
			}
			if (21 <= dateTime.get_Hour() && dateTime.get_Hour() < 22)
			{
				return UIDynamicWindowFurnitureCounterBar.MenuShift.C;
			}
			if (22 <= dateTime.get_Hour() || dateTime.get_Hour() < 1)
			{
				return UIDynamicWindowFurnitureCounterBar.MenuShift.D;
			}
			if (1 <= dateTime.get_Hour() || dateTime.get_Hour() < 5)
			{
				return UIDynamicWindowFurnitureCounterBar.MenuShift.E;
			}
			Debug.Log("ERROR:Shift Error Exception RecoverMe X<");
			return UIDynamicWindowFurnitureCounterBar.MenuShift.A;
		}
	}
}
