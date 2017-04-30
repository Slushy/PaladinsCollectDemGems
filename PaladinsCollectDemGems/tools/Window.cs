using PaladinsCollectDemGems.tools;
using PaladinsCollectDemGems.tools.native;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Management;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Wrapper around a windowed system process that provides more friendly api
	/// </summary>
	public class Window
	{
		private readonly Process _process;
		private WinWindowInterop.RECT _windowPosition = new WinWindowInterop.RECT();

		private IntPtr Handle { get { return WindowProcess.MainWindowHandle; } }
		private Process WindowProcess { get { _process.Refresh(); return _process; } }

		#region Public Accessors

		public bool HasExited
		{
			get
			{
				return WindowProcess.HasExited;
			}
		}
		public string ProcessName { get { return WindowProcess.ProcessName; } }
		public bool IsForegrounded { get { return WinWindowInterop.IsActiveWindow(Handle); } }
		public int Height
		{
			get
			{
				return _windowPosition.Bottom - _windowPosition.Top;
			}
		}
		public int Width
		{
			get
			{
				return _windowPosition.Right - _windowPosition.Left;
			}
		}
		public WinWindowInterop.RECT Position { get { return _windowPosition; } }

		#endregion

		// Constructs a game window with a process, can only happen from within the class
		private Window(Process windowProcess)
		{
			_process = windowProcess;
			UpdateWindowPosition();
		}

		/// <summary>
		/// Foregrounds the window to the top if not already
		/// </summary>
		/// <returns>true if foregrounding succeeded, false otherwise</returns>
		public bool SetForeground()
		{
			return WinWindowInterop.SetWindowActive(Handle);
		}

		public void Close() {
			if (!WindowProcess.CloseMainWindow()) {
				Console.WriteLine("Failed to close main window, killing process");
				WindowProcess.Kill();
			}
			
			WindowProcess.WaitForExit();
		}

		public Bitmap CaptureImage()
		{
			Bitmap bitmap = new Bitmap(Width, Height);
			using (Graphics g = Graphics.FromImage(bitmap as Image))
			{
				Size size = new Size(Width, Height);
				g.CopyFromScreen(new Point(_windowPosition.Left, _windowPosition.Top), Point.Empty, size);
			}
			return bitmap;
		}

		/// <summary>
		/// Centers the window in the middle of the primary monitor. The window remains in its current z-index (i.e. if it was
		/// not foregrounded before this call, it will still not be foregrounded, but when shown it will be in the center of the screen).
		/// </summary>
		public void CenterWindow()
		{
			UpdateWindowPosition();

			int xPos = (Screen.Width / 2) - (Width / 2);
			int yPos = (Screen.Height / 2) - (Height / 2);
			WinWindowInterop.MoveWindow(Handle, xPos, yPos);

			UpdateWindowPosition();
		}

		/// <summary>
		/// Updates the screen coordinate positions of the window's four corners
		/// </summary>
		public void UpdateWindowPosition()
		{
			if (!WinWindowInterop.GetWindowPosition(Handle, out _windowPosition))
				throw new Exception("Failed to get window position.");
		}

		/// <summary>
		/// Finds a system process by the name and/or window title, and returns a GameWindow instance that provides access to the game process.
		/// If the process is not found from the arguments passed in or the process does not have a valid handle, null is returned
		/// </summary>
		/// <param name="processName">Process name to filter by</param>
		/// <param name="windowTitle">Additional filter applied that forces the process to also have a window title with this value</param>
		/// <returns>GameWindow instance if valid process found, otherwise null</returns>
		public static Window FindByProcessName(string processName, string windowTitle = null)
		{
			Process[] candidates = Process.GetProcessesByName(processName);
			Process[] tests = Process.GetProcesses();

			// Find a comparable process out of our candidates. If we have a window title to compare against
			// we loop until we find a matching title, otherwise we just get the first candidate.
			Process foundProcess = null;
			foreach (Process candidate in candidates)
			{
				if (candidate.MainWindowHandle == null || string.IsNullOrEmpty(candidate.MainWindowTitle)) continue;

				if (string.IsNullOrEmpty(windowTitle))
					foundProcess = candidate;
				else if (!string.IsNullOrEmpty(candidate.MainWindowTitle) && candidate.MainWindowTitle.Equals(windowTitle))
					foundProcess = candidate;

				// we found the process, so break out of the loop
				if (foundProcess != null)
					break;
			}

			return foundProcess != null ? new Window(foundProcess) : null;
		}
	}
}
