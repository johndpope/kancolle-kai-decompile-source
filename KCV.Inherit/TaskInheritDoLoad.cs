using Common.SaveManager;
using System;

namespace KCV.Inherit
{
	public class TaskInheritDoLoad : SceneTaskMono, ISaveDataOperator
	{
		protected override bool Run()
		{
			return true;
		}

		public void SaveManOpen()
		{
		}

		public void SaveManClose()
		{
		}

		public void Canceled()
		{
		}

		public void SaveError()
		{
		}

		public void SaveComplete()
		{
		}

		public void LoadError()
		{
		}

		public void LoadComplete()
		{
		}

		public void LoadNothing()
		{
		}

		public void DeleteComplete()
		{
		}
	}
}
