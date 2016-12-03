using System;

namespace Common.Struct
{
	public struct AreaBy2Point
	{
		public Point left_top;

		public Point right_bottom;

		public AreaBy2Point(int left, int top, int right, int bottom)
		{
			this.left_top = new Point(left, top);
			this.right_bottom = new Point(right, bottom);
		}
	}
}
