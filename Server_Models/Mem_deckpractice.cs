using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_deckpractice", Namespace = "")]
	public class Mem_deckpractice : Model_Base
	{
		[DataMember]
		private List<bool> _practiceStatus;

		private static string _tableName = "mem_deckpractice";

		public bool this[DeckPracticeType kind]
		{
			get
			{
				int num = kind - DeckPracticeType.Normal;
				return this._practiceStatus.get_Item(num);
			}
			private set
			{
				int num = kind - DeckPracticeType.Normal;
				this._practiceStatus.set_Item(num, value);
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_deckpractice._tableName;
			}
		}

		public Mem_deckpractice()
		{
			this._practiceStatus = new List<bool>(6);
			this._practiceStatus.AddRange(Enumerable.Repeat<bool>(false, 6));
			this._practiceStatus.set_Item(0, true);
		}

		public void StateChange(DeckPracticeType type, bool state)
		{
			if (type == DeckPracticeType.Normal)
			{
				return;
			}
			this[type] = state;
		}

		protected override void setProperty(XElement element)
		{
			List<XElement> list = Enumerable.ToList<XElement>(element.Element("_practiceStatus").Elements());
			for (int i = 0; i < list.get_Count(); i++)
			{
				this._practiceStatus.set_Item(i, bool.Parse(list.get_Item(i).get_Value()));
			}
		}
	}
}
