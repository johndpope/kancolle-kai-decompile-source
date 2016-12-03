using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_ship_resources : Model_Base
	{
		private const int _voiceMaxNo = 29;

		private int _id;

		private int _standing_id;

		private int _voicef;

		private List<int> Motions;

		private Dictionary<int, int> _voiceId;

		private int _voice_practice_no;

		private static string _tableName = "mst_ship_resources";

		public static int VoiceMaxNo
		{
			get
			{
				return 29;
			}
		}

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

		public int Standing_id
		{
			get
			{
				return (this._standing_id != 0) ? this._standing_id : this._id;
			}
			private set
			{
				this._standing_id = value;
			}
		}

		public int Voicef
		{
			get
			{
				return this._voicef;
			}
			private set
			{
				this._voicef = value;
			}
		}

		public int Motion1
		{
			get
			{
				if (this.Motions == null)
				{
					return 0;
				}
				return this.Motions.get_Item(0);
			}
		}

		public int Motion2
		{
			get
			{
				if (this.Motions == null)
				{
					return 0;
				}
				return this.Motions.get_Item(1);
			}
		}

		public int Motion3
		{
			get
			{
				if (this.Motions == null)
				{
					return 0;
				}
				return this.Motions.get_Item(2);
			}
		}

		public int Motion4
		{
			get
			{
				if (this.Motions == null)
				{
					return 0;
				}
				return this.Motions.get_Item(3);
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_ship_resources._tableName;
			}
		}

		public Mst_ship_resources()
		{
			this._voiceId = new Dictionary<int, int>();
		}

		public int GetVoiceId(int voiceNo)
		{
			int result = 0;
			this._voiceId.TryGetValue(voiceNo, ref result);
			return result;
		}

		public int GetDeckPracticeVoiceNo()
		{
			return this._voice_practice_no;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			int num = int.Parse(element.Element("Standing_id").get_Value());
			this.Standing_id = num;
			if (num == 0)
			{
				return;
			}
			this.Voicef = int.Parse(element.Element("Voicef").get_Value());
			if (element.Element("Voiceitem") == null)
			{
				return;
			}
			string[] array = element.Element("Voiceitem").get_Value().Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				int num2 = int.Parse(array[i]);
				if (num2 != 0)
				{
					this._voiceId.Add(i + 1, num2);
				}
			}
			this._voice_practice_no = int.Parse(array[array.Length - 1]);
			string[] array2 = element.Element("Motion").get_Value().Split(new char[]
			{
				','
			});
			this.Motions = Enumerable.ToList<int>(Array.ConvertAll<string, int>(array2, (string x) => int.Parse(x)));
		}

		public static List<int> GetRequireStandingIds(IEnumerable<Mst_ship_resources> resources)
		{
			return new List<int>();
		}

		public static List<int> GetRequireVoiceNo(int shipId)
		{
			return new List<int>();
		}
	}
}
