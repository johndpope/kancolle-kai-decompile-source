using System;

namespace KCV
{
	public class LogDrawer : SingletonMonoBehaviour<LogDrawer>
	{
		private UILabel myLabel;

		private int lineCount;

		protected override void Awake()
		{
			base.Awake();
			SingletonMonoBehaviour<LogDrawer>.instance = this;
			this.myLabel = base.GetComponent<UILabel>();
			this.myLabel.text = string.Empty;
			this.lineCount = 0;
		}

		private void Start()
		{
		}

		public void addDebugText(string s)
		{
			UILabel expr_06 = this.myLabel;
			expr_06.text = expr_06.text + s + "\n";
			this.lineCount++;
			if (this.lineCount > 20)
			{
				int num = this.myLabel.text.IndexOf("\n", 0) + 1;
				this.myLabel.text = this.myLabel.text.Remove(0, num);
			}
		}

		public static bool exist()
		{
			return SingletonMonoBehaviour<LogDrawer>.instance != null;
		}
	}
}
