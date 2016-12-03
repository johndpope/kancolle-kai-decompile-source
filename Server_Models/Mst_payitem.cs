using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_payitem : Model_Base
	{
		private int _id;

		private string _name;

		private string _description;

		private int _price;

		private List<PayItemEffectInfo> _items;

		private static string _tableName = "mst_payitem";

		public int Id
		{
			get
			{
				return this._id;
			}
			private set
			{
				this._id = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		public string Description
		{
			get
			{
				return this._description;
			}
			private set
			{
				this._description = value;
			}
		}

		public int Price
		{
			get
			{
				return this._price;
			}
			private set
			{
				this._price = value;
			}
		}

		public List<PayItemEffectInfo> Items
		{
			get
			{
				return this._items;
			}
			private set
			{
				this._items = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_payitem._tableName;
			}
		}

		public Mst_payitem()
		{
			this.Description = string.Empty;
			this.Items = new List<PayItemEffectInfo>();
		}

		public void setText(string info)
		{
			this.Description = info;
		}

		public int GetBuyNum()
		{
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			using (List<PayItemEffectInfo>.Enumerator enumerator = this.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PayItemEffectInfo current = enumerator.get_Current();
					int num = 0;
					int num2 = 0;
					if (current.Type == 1 && current.MstId == 53)
					{
						int num3 = instance.User_basic.GetPortMaxExtendNum() - instance.User_basic.Max_chara;
						int num4 = 0;
						Mem_useitem mem_useitem;
						if (instance.User_useItem.TryGetValue(53, ref mem_useitem))
						{
							num4 = mem_useitem.Value;
						}
						if (num3 > 0)
						{
							num = num3 / 10 - num4;
						}
					}
					else if (current.Type == 1)
					{
						Mem_useitem mem_useitem2 = null;
						if (Comm_UserDatas.Instance.User_useItem.TryGetValue(current.MstId, ref mem_useitem2))
						{
							num2 = mem_useitem2.Value;
						}
						int num5 = 3000 - num2;
						if (num5 > 0)
						{
							num = num5 / current.Count;
						}
					}
					else if (current.Type == 2)
					{
						num = 2147483647;
					}
					else if (current.Type == 3)
					{
						enumMaterialCategory mstId = (enumMaterialCategory)current.MstId;
						Dictionary<int, Mst_item_limit> mst_item_limit = Mst_DataManager.Instance.Mst_item_limit;
						int materialLimit = Mst_DataManager.Instance.Mst_item_limit.get_Item(1).GetMaterialLimit(mst_item_limit, mstId);
						num2 = Comm_UserDatas.Instance.User_material.get_Item(mstId).Value;
						int num6 = materialLimit - num2;
						if (num6 > 0)
						{
							num = num6 / current.Count;
						}
					}
					list.Add(num);
				}
			}
			int num7 = Enumerable.Min(list);
			return (num7 != 2147483647) ? num7 : -1;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Name = element.Element("Name").get_Value();
			this.Price = int.Parse(element.Element("Price").get_Value());
			using (IEnumerator<XElement> enumerator = element.Elements("Items").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					int[] itemData = Array.ConvertAll<string, int>(current.get_Value().Split(new char[]
					{
						','
					}), (string x) => int.Parse(x));
					PayItemEffectInfo payItemEffectInfo = new PayItemEffectInfo(itemData);
					this.Items.Add(payItemEffectInfo);
				}
			}
		}
	}
}
