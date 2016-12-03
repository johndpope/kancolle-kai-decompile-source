using System;

namespace Common.SaveManager
{
	public interface ISaveDataOperator
	{
		void SaveManOpen();

		void SaveManClose();

		void Canceled();

		void SaveError();

		void SaveComplete();

		void LoadError();

		void LoadComplete();

		void LoadNothing();

		void DeleteComplete();
	}
}
