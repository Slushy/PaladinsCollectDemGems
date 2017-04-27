using PaladinsCollectDemGems.tools.native;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Different states a game's process can be in according to the windows registry
	/// </summary>
	[Flags]
	public enum SteamGameState
	{
		[Description("Installed")]
		Installed = 0x01,
		[Description("Launching")]
		Launching = 0x02,
		[Description("Running")]
		Running = 0x04,
		[Description("Updating")]
		Updating = 0x08
	}

	/// <summary>
	/// A wrapper around a specific steam game process and window with access to info and states pertaining to the game
	/// </summary>
	public class SteamGame 
	{
		private const string GAME_REGISTRY_LOCATION = Steam.BASE_REGISTRY_LOCATION + "Apps\\{0}";

		private readonly string _registryLocation;
		private SteamGameState _gameStates = 0;
		private Process _gameProcess = null;

		/// <summary>
		/// Returns the Steam specific game ID that represents this game
		/// </summary>
		public int GameId { get; private set; }

		public bool IsInstalled { get { return CheckGameState(SteamGameState.Installed); } }
		public bool IsLaunching { get { return CheckGameState(SteamGameState.Launching); } }
		public bool IsRunning { get { return CheckGameState(SteamGameState.Running); } }
		public bool IsUpdating { get { return CheckGameState(SteamGameState.Updating); } }

		public bool HasWindowHandle { 
			get { return CheckIfValidProcessAndWindow(); }
		}

		/// <summary>
		/// Constructs a new SteamGame object from the gameId
		/// </summary>
		/// <param name="gameId">Steam app ID</param>
		public SteamGame(int gameId) {
			GameId = gameId;
			_registryLocation = string.Format(GAME_REGISTRY_LOCATION, gameId);
		}

		/// <summary>
		/// Checks if the current game is in the specified state
		/// </summary>
		/// <param name="gameState">the state to check if the game is in</param>
		/// <param name="forceUpdate">whether to pull the latest state information from the registry</param>
		/// <returns>true if the game is in the specified state, otherwise false</returns>
		public bool CheckGameState(SteamGameState gameState, bool forceUpdate = true)
		{
			// Update to latest registry value if forced update
			if (forceUpdate)
				UpdateGameState(gameState);
			
			// Game can be muliple states thanks to bitwise operators
			return (_gameStates & gameState) != 0;
		}

		// Gets the latest info of the specified state for the current game from the registry
		private bool UpdateGameState(SteamGameState gameState)
		{
			int? statusValue = WinRegistry.GetValueAsInt(_registryLocation, gameState.ToString());
			bool isValid = statusValue.HasValue && statusValue.Value == 1;

			if (isValid)
			{
				_gameStates |= gameState;
				Console.WriteLine("Game is " + gameState.ToString());
			}
			// else status key in registry is either invalid or false
			else
			{
				// Clears this bit flag from the status
				_gameStates &= ~gameState;
				Console.WriteLine("Game is NOT " + gameState.ToString());
			}

			return isValid;
		}

		private bool CheckIfValidProcessAndWindow() {
			// If the game isn't even running, just clear the process
			if ((_gameStates & SteamGameState.Running) == 0) { 
				_gameProcess = null;
				return false;
			}

			// For now if the game process is already set and hasn't exited we'll just return
			if (_gameProcess != null && !_gameProcess.HasExited)
				return true;
			
			// The game process isn't yet set and the process is running, so find.. er.. set it.
			Process[] processCandidates = Process.GetProcessesByName("SteamLauncherUI");
			foreach (Process process in processCandidates)
			{
				if (!string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowTitle.Equals("Paladins")) {
					Console.WriteLine("Found Game Process: {0}", process.Id);
					_gameProcess = process;
					return true;
				}
			}

			Console.WriteLine("Not yet found game process");
			_gameProcess = null;
			return false;
		}
	}
}
