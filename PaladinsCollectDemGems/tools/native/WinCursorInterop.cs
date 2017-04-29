using System.Runtime.InteropServices;

namespace PaladinsCollectDemGems.tools.native
{
	/// <summary>
	/// Provides access to the windows native layer
	/// </summary>
	public static partial class WinCursorInterop
	{
		#region DLLImports

		//This is a replacement for Cursor.Position in WinForms
		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		[DllImport("user32.dll")]
		private static extern bool GetCursorInfo(out CursorInfo pci);

		#endregion

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

		/// <summary>
		/// Retrieves the info for the current state of the cursor
		/// </summary>
		/// <returns>struct storing info about the current state of the cursor</returns>
		public static CursorInfo GetCursorInfo() {
			CursorInfo cursorInfo;
			cursorInfo.cbSize = Marshal.SizeOf(typeof(CursorInfo));
			GetCursorInfo(out cursorInfo);

			return cursorInfo;
		} 
	}
}
