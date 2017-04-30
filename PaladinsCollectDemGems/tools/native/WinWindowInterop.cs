using System;
using System.Collections.Generic;
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
			var success = GetWindowRect(windowHandle, out rect);

			//if (!success) {
			//	var children = GetAllChildHandles(windowHandle);
			//	Console.WriteLine("fail");
			//}

			return success;
		}

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

		public const int WS_EX_TOPMOST = 0x00000008;
		public const int GWL_EXSTYLE = -20;
		public const int WS_EX_LAYERED = 0x80000;
		public const int LWA_ALPHA = 0x2;
		private const int LWA_COLORKEY = 0x1;

		public static bool SetColorThing(IntPtr windowHandle)
		{
			Console.WriteLine("WTF");
			SetWindowLong(windowHandle, GWL_EXSTYLE, GetWindowLong(windowHandle, GWL_EXSTYLE) ^ ( WS_EX_LAYERED | WS_EX_TOPMOST ));
			return SetLayeredWindowAttributes(windowHandle, 0, 128, LWA_ALPHA);
		}


		private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

		public static List<IntPtr> GetAllChildHandles(IntPtr windowHandle)
		{
			List<IntPtr> childHandles = new List<IntPtr>();

			GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
			IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

			try
			{
				EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
				EnumChildWindows(windowHandle, childProc, pointerChildHandlesList);
			}
			finally
			{
				gcChildhandlesList.Free();
			}

			return childHandles;
		}

		private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
		{
			GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

			if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
			{
				return false;
			}

			List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
			childHandles.Add(hWnd);

			return true;
		}
	}
}
