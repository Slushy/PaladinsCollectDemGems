using PaladinsCollectDemGems.exceptions;
using PaladinsCollectDemGems.tools.native;
using System.Diagnostics;
using System.Threading;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Class that acts as the base for launching Steam games
	/// </summary>
	public static class Steam
	{
		// Move these to config file too, or who really cares?
		public  const string BASE_REGISTRY_LOCATION = "Software\\Valve\\Steam\\";

		private const string PROCESS_REGISTRY_LOCATION = BASE_REGISTRY_LOCATION + "ActiveProcess";
		private const string STEAM_LAUNCH_URI = "steam://rungameid/{0}";

		/// <summary>
		/// Gets the currently logged in steam userId, or null if n/a
		/// </summary>
		public static int? ActiveUserId {
			get { 
				int? userId = WinRegistry.GetValueAsInt(PROCESS_REGISTRY_LOCATION, "ActiveUser");
				return userId.HasValue && userId > 0 ? userId : null;	
			}
		}

		/// <summary>
		/// Checks if we have a valid steam user logged in
		/// </summary>
		public static bool IsLoggedIn { get { return ActiveUserId.HasValue; } }

		/// <summary>
		/// Launches a steam game if it is not already in the middle of launching or running. Creates a new SteamGame object which wraps
		/// the game process and window and returns it when the game is running. Should not be run on main thread.
		/// </summary>
		/// <param name="gameId">Steam app ID to launch</param>
		/// <returns>Game wrapper representing the new running steam game process and window</returns>
		public static SteamGame LaunchGame(int gameId)
		{
			if (!IsLoggedIn)
				throw new InvalidSteamUserException("User is not logged in");

			SteamGame game = new SteamGame(gameId);

			// Throw an exception if the game is not installed
			if (!game.IsInstalled)
				throw new GameNotInstalledException();

			// Throw an exception if the game is updating and not usable
			if (game.IsUpdating)
				throw new GameUpdatingException();

			// Launches the steam launcher process only if the game is not already launching
			if (!game.IsRunning && !game.IsLaunching)
			{
				string steamLauncherProcessUri = string.Format(STEAM_LAUNCH_URI, gameId);
				using (Process steamLauncherProcess = Process.Start(steamLauncherProcessUri))
				{
					// should almost be instant as the process' job is to
					// create the actual game launcher process. Process-ception.
					steamLauncherProcess.WaitForExit();
				}
			}

			// Wait until the game's process has updated to be running
			while (!game.IsRunning)
				Thread.Sleep(100);

			// At this point the game is running (possibly not yet foregrounded)
			return game;
		}
	}
}

