using System;
using System.Drawing;
using System.Runtime.InteropServices;

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

		/// <summary>
		/// Stores the information for current state of the cursor
		/// See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms648381(v=vs.85).aspx
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct CursorInfo
		{
			/// <summary>
			/// Must be set each time to the size of this struct
			/// </summary>
			public Int32 cbSize;

			/// <summary>
			/// Specifies the cursor state. (
			/// </summary>
			public Int32 flags;         
			/// <summary>
			/// Handle to the cursor
			/// </summary>
			public IntPtr hCursor;

			/// <summary>
			/// The screen coordinates of the cursor at this time
			/// </summary>
			public Point ptScreenPos;
		}
	}
}
