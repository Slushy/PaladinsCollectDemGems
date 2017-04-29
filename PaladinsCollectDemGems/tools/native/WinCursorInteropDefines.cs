using System;

namespace PaladinsCollectDemGems.tools.native
{
	public static partial class WinCursorInterop
	{
		/// <summary>
		/// Mouse actions you can perform
		/// </summary>
		[Flags]
		public enum MouseEvent
		{
			Move = 0x0001,
			LeftClickDown = 0x0002,
			LeftClickUp = 0x0004,
			RightClickDown = 0x0008,
			RightClickUp = 0x0010,
			MiddleClickDown = 0x0020,
			MiddleClickUp = 0x0040,
			Absolute = 0x8000
		};
	}
}
