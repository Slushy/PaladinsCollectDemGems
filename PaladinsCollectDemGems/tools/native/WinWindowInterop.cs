using System;
using System.Runtime.InteropServices;

namespace PaladinsCollectDemGems.tools.native
{
	/// <summary>
	/// Provides access to the Windows native API for manipulating "windows"
	/// </summary>
	public static partial class WinWindowInterop
	{
		#region DLLImports

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, ShowWindowOption option);
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

		#endregion

		/// <summary>
		/// Checks to see if a window is the currently focused and foregrounded window
		/// </summary>
		/// <param name="windowHandle">Process.MainWindowHandle</param>
		/// <returns>true if the window is foregrounded, false otherwise</returns>
		public static bool IsActiveWindow(IntPtr windowHandle) {
			return GetForegroundWindow() == windowHandle;
		}

		/// <summary>
		/// Sets the window active by foregrounding it and maximizing it if minimized
		/// </summary>
		/// <param name="windowHandle">Process.MainWindowHandle</param>
		/// <returns>true if setting the window active succeeded, false otherwise</returns>
		public static bool SetWindowActive(IntPtr windowHandle) {
			return SetForegroundWindow(windowHandle) && ShowWindow(windowHandle, ShowWindowOption.Restore);
		}

		/// <summary>
		/// Moves a window to a position on the screen
		/// </summary>
		/// <param name="windowHandle">Process.MainWindowHandle</param>
		/// <param name="x">x position of the screen in coordinates</param>
		/// <param name="y">y position of the screen in coordinates</param>
		/// <returns>true if moving the window succeeded, false otherwise</returns>
		public static bool MoveWindow(IntPtr windowHandle, int x, int y) {
			return SetWindowPos(windowHandle, IntPtr.Zero, x, y, 0, 0, SetWindowPosFlags.DoNotChangeOwnerZOrder | SetWindowPosFlags.IgnoreResize);
		}

		/// <summary>
		/// Gets the windows position and stores it in the passed in structure
		/// </summary>
		/// <param name="windowHandle">Process.MainWindowHandle</param>
		/// <param name="rect">the structure to be updated with the window position</param>
		/// <returns>true if getting the window position succeeded, false otherwise</returns>
		public static bool GetWindowPosition(IntPtr windowHandle, out RECT rect) {
			return GetWindowRect(windowHandle, out rect);
		}
	}
}
