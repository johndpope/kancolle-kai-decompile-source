using local.models;
using System;

namespace KCV
{
	public interface CommonDeckSwitchHandler
	{
		void OnDeckChange(DeckModel deck);

		bool IsDeckSelectable(int index, DeckModel deck);
	}
}
