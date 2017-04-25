using System;

namespace PaladinsCollectDemGems.native
{
	/// <summary>
	/// Provides access to the windows native layer
	/// </summary>
	public static class WinCursorInterop
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

		//This is a replacement for Cursor.Position in WinForms
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		/// <summary>
		/// Moves the cursor to the specified screen coordinates. If the new coordinates are not within the screen rectangle set by the most
		/// recent ClipCursor function call, the system automatically adjusts the coordinates so that the cursor stays within the rectangle.
		/// </summary>
		/// <param name="x">new x-coordinate of the cursor, in screen coordinates</param>
		/// <param name="y">new y-coordinate of the cursor, in screen coordinates</param>
		/// <returns>true for success, false for failure</returns>
		public static bool SetCursorPosition(int x, int y)
		{
			return SetCursorPos(x, y);
		}

		/// <summary>
		/// Synthesizes mouse motion and button clicks. See <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx">Docs</see> 
		/// for more info 
		/// </summary>
		/// <param name="dwFlags">Controls various aspects of mouse motion and button clicking </param>
		/// <param name="dx">mouse's absolute position along the x-axis</param>
		/// <param name="dy">mouse's absolute position along the y-axis</param>
		/// <param name="dwData">specific data related to the type of event</param>
		/// <param name="dwExtraInfo">an additional value associated with the mouse event</param>
		public static void SendMouseEvent(int dwFlags, int dx, int dy, int dwData = 0, int dwExtraInfo = 0)
		{
			mouse_event(dwFlags, dx, dy, dwData, dwExtraInfo);
		}

		/// <summary>
		/// Synthesizes mouse motion and button clicks. See <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx">Docs</see> 
		/// for more info 
		/// </summary>
		/// <param name="mouseEvent">friendly bitwise value to pass as dwFlags to represent the mouse event</param>
		/// <param name="dx">mouse's absolute position along the x-axis</param>
		/// <param name="dy">mouse's absolute position along the y-axis</param>
		/// <param name="dwData">specific data related to the type of event</param>
		/// <param name="dwExtraInfo">an additional value associated with the mouse event</param>
		public static void SendMouseEvent(MouseEvent mouseEvent, int dx, int dy, int dwData = 0, int dwExtraInfo = 0)
		{
			SendMouseEvent((int)mouseEvent, dx, dy, dwData, dwExtraInfo);
		}
	}
}
