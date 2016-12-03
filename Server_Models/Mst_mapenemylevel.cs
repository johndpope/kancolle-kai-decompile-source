using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapenemylevel : Model_Base
	{
		private int _enemy_list_id;

		private DifficultKind _difficulty;

		private int _turns;

		private int _choose_rate;

		private int _deck_id;

		private static string _tableName = "mst_mapenemylevel";

		public int Enemy_list_id
		{
			get
			{
				return this._enemy_list_id;
			}
			private set
			{
				this._enemy_list_id = value;
			}
		}

		public DifficultKind Difficulty
		{
			get
			{
				return this._difficulty;
			}
			private set
			{
				this._difficulty = value;
			}
		}

		public int Turns
		{
			get
			{
				return this._turns;
			}
			private set
			{
				this._turns = value;
			}
		}

		public int Choose_rate
		{
			get
			{
				return this._choose_rate;
			}
			private set
			{
				this._choose_rate = value;
			}
		}

		public int Deck_id
		{
			get
			{
				return this._deck_id;
			}
			private set
			{
				this._deck_id = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapenemylevel._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Enemy_list_id = int.Parse(element.Element("Enemy_list_id").get_Value());
			this.Difficulty = (DifficultKind)int.Parse(element.Element("Difficulty").get_Value());
			this.Turns = int.Parse(element.Element("Turns").get_Value());
			this.Enemy_list_id = int.Parse(element.Element("Enemy_list_id").get_Value());
			this.Choose_rate = int.Parse(element.Element("Choose_rate").get_Value());
			this.Deck_id = int.Parse(element.Element("Deck_id").get_Value());
		}
	}
}
