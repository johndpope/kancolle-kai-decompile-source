using System;

namespace KCV
{
	public class UIHowToDetail
	{
		public HowToKey key
		{
			get;
			private set;
		}

		public string label
		{
			get;
			private set;
		}

		public UIHowToDetail(HowToKey key, string label)
		{
			this.key = key;
			this.label = label;
		}

		public void Set(HowToKey key, string label)
		{
			this.key = key;
			this.label = label;
		}
	}
}
