using System;

namespace Common.Struct
{
	public struct TouchLovInfo
	{
		public bool BackTouch;

		public int VoiceType;

		public int AddValueOnce;

		public int AddValueSecond;

		public int SubValue;

		public int SubMoreThanLimitCount;

		public TouchLovInfo(int voicetype, bool backTouch)
		{
			this.BackTouch = backTouch;
			this.VoiceType = voicetype;
			this.AddValueOnce = 0;
			this.AddValueSecond = 0;
			this.SubValue = 0;
			this.SubMoreThanLimitCount = 0;
			if (backTouch)
			{
				this.setSumValueBackTouch(voicetype);
			}
			else
			{
				this.setSumValueNormalTouch(voicetype);
			}
		}

		private void setSumValueNormalTouch(int voicetype)
		{
			if (voicetype == 2)
			{
				this.AddValueOnce = 1;
			}
			else if (voicetype == 3)
			{
				this.AddValueOnce = 2;
			}
			else if (voicetype == 4)
			{
				this.AddValueOnce = 2;
				this.SubValue = -3;
				this.SubMoreThanLimitCount = 2;
			}
		}

		private void setSumValueBackTouch(int voicetype)
		{
			if (voicetype == 3)
			{
				this.AddValueOnce = 2;
			}
			else if (voicetype == 4)
			{
				this.SubValue = -3;
			}
			else if (voicetype == 2 || voicetype == 28)
			{
				this.AddValueOnce = 3;
				this.AddValueSecond = 2;
			}
		}

		public int GetSumValue(int nowTouchNum)
		{
			if (this.SubMoreThanLimitCount > 0 && this.SubMoreThanLimitCount <= nowTouchNum)
			{
				return this.SubValue;
			}
			if (this.SubValue > 0)
			{
				return this.SubValue;
			}
			if (nowTouchNum == 1)
			{
				return this.AddValueOnce;
			}
			if (nowTouchNum == 2)
			{
				return this.AddValueSecond;
			}
			return 0;
		}
	}
}
