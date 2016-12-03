using System;
using UnityEngine;

namespace UniRx
{
	public static class FrameCountTypeExtensions
	{
		public static YieldInstruction GetYieldInstruction(this FrameCountType frameCountType)
		{
			switch (frameCountType)
			{
			case FrameCountType.FixedUpdate:
				return new WaitForFixedUpdate();
			case FrameCountType.EndOfFrame:
				return new WaitForEndOfFrame();
			}
			return null;
		}
	}
}
