using System;
using System.Collections;

namespace ModeProc
{
	public class Mode
	{
		public delegate void ModeRun();

		public delegate IEnumerator ModeChange();

		public string Name;

		public int ModeNo;

		public Mode.ModeRun Run;

		public Mode.ModeChange Enter;

		public Mode.ModeChange Exit;

		public Mode(Mode.ModeRun run, Mode.ModeChange enter, Mode.ModeChange exit)
		{
			this.Run = run;
			this.Enter = enter;
			this.Exit = exit;
		}
	}
}
