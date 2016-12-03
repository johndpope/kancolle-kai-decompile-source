using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class TutorialModel
	{
		private Mem_basic _basic;

		public int GetStep()
		{
			return this._basic.GetTutorialStepLastNo();
		}

		public bool GetStepTutorialFlg(int step_count)
		{
			return this._basic.GetTutorialState(1, step_count);
		}

		public int SetStepTutorialFlg()
		{
			int step = this.GetStep();
			int num = step + 1;
			this._basic.AddTutorialProgress(1, num);
			return num;
		}

		public void SetStepTutorialFlg(int step_count)
		{
			this._basic.AddTutorialProgress(1, step_count);
		}

		public bool GetKeyTutorialFlg(int key)
		{
			return this._basic.GetTutorialState(0, key);
		}

		public List<int> GetTutorialKays()
		{
			return this._basic.GetTutorialProgress(0);
		}

		public void SetKeyTutorialFlg(int key)
		{
			this._basic.AddTutorialProgress(0, key);
		}

		public void __Update__(Mem_basic basic)
		{
			this._basic = basic;
		}

		public override string ToString()
		{
			string text = string.Format("Step:{0} Key:[", this.GetStep());
			List<int> tutorialKays = this.GetTutorialKays();
			for (int i = 0; i < tutorialKays.get_Count(); i++)
			{
				text += tutorialKays.get_Item(i);
				if (i < tutorialKays.get_Count() - 1)
				{
					text += ",";
				}
			}
			return text + "]";
		}
	}
}
