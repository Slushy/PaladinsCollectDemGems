using System.Diagnostics;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Wrapper around a windowed system process that provides more friendly api (Should this be specific to game?)
	/// </summary>
	public class GameWindow
	{
		private readonly Process _process;

		public bool HasExited { get { return _process.HasExited; } }

		// Constructs a game window with a process, can only happen from within the class
		private GameWindow(Process windowProcess) {
			_process = windowProcess;
		}

		/// <summary>
		/// Finds a system process by the name and/or window title, and returns a GameWindow instance that provides access to the game process.
		/// If the process is not found from the arguments passed in or the process does not have a valid handle, null is returned
		/// </summary>
		/// <param name="processName">Process name to filter by</param>
		/// <param name="windowTitle">Additional filter applied that forces the process to also have a window title with this value</param>
		/// <returns>GameWindow instance if valid process found, otherwise null</returns>
		public static GameWindow FindByProcessName(string processName, string windowTitle = null) {
			Process[] candidates = Process.GetProcessesByName(processName);

			// Find a comparable process out of our candidates. If we have a window title to compare against
			// we loop until we find a matching title, otherwise we just get the first candidate.
			Process foundProcess = null;
			foreach (Process candidate in candidates)
			{
				if (candidate.MainWindowHandle == null) continue;
				
				if (string.IsNullOrEmpty(windowTitle))
					foundProcess = candidate;
				else if (!string.IsNullOrEmpty(candidate.MainWindowTitle) && candidate.MainWindowTitle.Equals(windowTitle))
					foundProcess = candidate;

				// we found the process, so break out of the loop
				if (foundProcess != null)
					break;
			}

			return foundProcess != null ? new GameWindow(foundProcess) : null;
		}
	}
}
