using System;

namespace Server_Common.Formats
{
	public class EventMapInfo
	{
		public enum enumEventState
		{
			Close,
			Open,
			Clear
		}

		public int Event_hp;

		public int Event_maxhp;

		public EventMapInfo.enumEventState Event_state;

		public int Damage;

		public EventMapInfo()
		{
			this.Event_hp = 0;
			this.Event_maxhp = 0;
			this.Event_state = EventMapInfo.enumEventState.Close;
			this.Damage = 0;
		}
	}
}
