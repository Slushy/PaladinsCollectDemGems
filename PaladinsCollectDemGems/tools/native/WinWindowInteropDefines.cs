using System;
using System.Runtime.InteropServices;

namespace PaladinsCollectDemGems.tools.native
{
	public static partial class WinWindowInterop
	{
		/// <summary>
		/// Defines a structure that holds the position for the 4 corners of a window
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			/// <summary>
			/// x position of upper-left corner
			/// </summary>
			public int Left;
			/// <summary>
			/// y position of upper-left corner
			/// </summary>
			public int Top;
			/// <summary>
			/// x position of lower-right corner
			/// </summary>
			public int Right;
			/// <summary>
			/// y position of lower-right corner
			/// </summary>
			public int Bottom;
		}

		/// <summary>
		/// See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx
		/// </summary>
		[Flags]
		public enum SetWindowPosFlags : uint
		{
			/// <summary>
			/// If the calling thread and the thread that owns the window are attached to different input queues,
			/// the system posts the request to the thread that owns the window. This prevents the calling thread from
			/// blocking its execution while other threads process the request.
			/// </summary>
			AsynchronousWindowPosition = 0x4000,
			/// <summary>
			/// Prevents generation of the WM_SYNCPAINT message.
			/// </summary>
			DeferErase = 0x2000,
			/// <summary>
			/// Draws a frame (defined in the window's class description) around the window.
			/// </summary>
			DrawFrame = 0x0020,
			/// <summary>
			/// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
			/// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
			/// is sent only when the window's size is being changed.
			/// </summary>
			FrameChanged = 0x0020,
			/// <summary>
			/// Hides the window.
			/// </summary>
			HideWindow = 0x0080,
			/// <summary>
			/// Does not activate the window. If this flag is not set, the window is activated and moved to the
			/// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
			/// parameter).
			/// </summary>
			DoNotActivate = 0x0010,
			/// <summary>
			/// Discards the entire contents of the client area. If this flag is not specified, the valid
			/// contents of the client area are saved and copied back into the client area after the window is sized or
			/// repositioned.
			/// </summary>
			DoNotCopyBits = 0x0100,
			/// <summary>
			/// Retains the current position (ignores X and Y parameters).
			/// </summary>
			IgnoreMove = 0x0002,
			/// <summary>
			/// Does not change the owner window's position in the Z order.
			/// </summary>
			DoNotChangeOwnerZOrder = 0x0200,
			/// <summary>
			/// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
			/// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
			/// window uncovered as a result of the window being moved. When this flag is set, the application must
			/// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
			/// </summary>
			DoNotRedraw = 0x0008,
			/// <summary>
			/// Same as the SWP_NOOWNERZORDER flag.
			/// </summary>
			DoNotReposition = 0x0200,
			/// <summary>
			/// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
			/// </summary>
			DoNotSendChangingEvent = 0x0400,
			/// <summary>
			/// Retains the current size (ignores the cx and cy parameters).
			/// </summary>
			IgnoreResize = 0x0001,
			/// <summary>
			/// Retains the current Z order (ignores the hWndInsertAfter parameter).
			/// </summary>
			IgnoreZOrder = 0x0004,
			/// <summary>
			/// Displays the window.
			/// </summary>
			ShowWindow = 0x0040,
		}

		/// <summary>
		/// See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx
		/// </summary>
		private enum ShowWindowOption
		{
			/// <summary>
			/// Hides the window and activates another window
			/// </summary>
			Hide = 0,
			/// <summary>
			/// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position.
			/// An application should specify this flag when displaying the window for the first time.
			/// </summary>
			ShowNormal = 1,
			/// <summary>
			/// Activates the window and displays it as a minimized window.
			/// </summary>
			ShowMinimized = 2,
			/// <summary>
			/// Activates the window and displays it as a maximized window.
			/// </summary>
			ShowMaximized = 3,
			/// <summary>
			/// Maximizes the specified window.
			/// </summary>
			Maximize = 3,
			/// <summary>
			/// Displays a window in its most recent size and position. Does not activate the window.
			/// </summary>
			ShowNormalNoActivate = 4,
			/// <summary>
			/// Activates the window and displays it in its current size and position.
			/// </summary>
			Show = 5,
			/// <summary>
			/// Minimizes the specified window and activates the next top-level window in the Z order.
			/// </summary>
			Minimize = 6,
			/// <summary>
			/// Displays the window as a minimized window. The window is not activated.
			/// </summary>
			ShowMinNoActivate = 7,
			/// <summary>
			/// Displays the window in its current size and position. The window is not activated.
			/// </summary>
			ShowNoActivate = 8,
			/// <summary>
			/// Activates and displays teh window. If the window is minimized or maximized, the system restores it to its original size and position.
			/// An application should specify this flag when restoring a minimized window.
			/// </summary>
			Restore = 9,
			/// <summary>
			/// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program
			/// that started the application.
			/// </summary>
			ShowDefault = 10,
			/// <summary>
			/// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows 
			/// from a different thread.
			/// </summary>
			ForceMinimized = 11
		};
	}
}
