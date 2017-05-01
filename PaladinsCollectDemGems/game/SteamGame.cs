using PaladinsCollectDemGems.tools.native;
using System;
using System.ComponentModel;
using System.Threading;

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
		private Window _gameWindow = null;

		/// <summary>
		/// Returns the Steam specific game ID that represents this game
		/// </summary>
		public int GameId { get; private set; }

		public bool IsInstalled { get { return CheckGameState(SteamGameState.Installed); } }
		public bool IsLaunching { get { return CheckGameState(SteamGameState.Launching); } }
		public bool IsRunning { get { return CheckGameState(SteamGameState.Running); } }
		public bool IsUpdating { get { return CheckGameState(SteamGameState.Updating); } }

		// TODO: Just to get it working plz
		public const string ProcessPhase1 = "SteamLauncherUI";
		public const string ProcessPhase2 = "Paladins";
		private readonly string[] _processPhases = { ProcessPhase1, ProcessPhase2 };

		// TODO: MOVE THIS TO SEPARATE FUNCTION

		/// <summary>
		/// Returns the game window of the currently running game, or null if not found or not yet loaded.
		/// This property will return up-to-date information when called.
		/// </summary>
		public Window Window
		{
			get
			{
				// For this instruction set we want to check 10 times
				int timesToCheck = 10;
				while (updateGameWindow() == null && --timesToCheck > 0)
				{
					Console.WriteLine("SteamGame.Window => is null, waiting 250 ms and trying again");
					Thread.Sleep(250);
				}
				if (timesToCheck <= 0)
				{
					Console.WriteLine("SteamGame.Window => FAILED to retrieve game window");
				}
				return _gameWindow;
			}
		}

		/// <summary>
		/// Constructs a new SteamGame object from the gameId
		/// </summary>
		/// <param name="gameId">Steam app ID</param>
		public SteamGame(int gameId)
		{
			GameId = gameId;
			_registryLocation = string.Format(GAME_REGISTRY_LOCATION, gameId);
		}

		/// <summary>
		/// Checks if the current game is in the specified state
		/// </summary>
		/// <param name="gameState">the state to check if the game is in</param>
		/// <param name="forceUpdate">whether to pull the latest state information from the registry</param>
		/// <returns>true if the game is in the specified state, otherwise false</returns>
		private bool CheckGameState(SteamGameState gameState, bool forceUpdate = true)
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

		// Updates and returns the game window process by returning the currently existing and valid window, or by trying to find the process for the game.
		private Window updateGameWindow()
		{
			// Check if the game is even running, but we do not pull latest from registry. This isn't very important
			// because if the game isn't even running it will never find a window in the next step. This is simply a
			// quick and easy cost-effective way to short-circuit the check
			if (!CheckGameState(SteamGameState.Running, false))
			{
				_gameWindow = null;
			}
			else if (_gameWindow == null || _gameWindow.HasExited)
			{
				// The process is running, but the game window isn't valid or isn't set, so try to find one that matches (will return null if cannot find)
				//_gameWindow = GameWindow.FindByProcessName("SteamLauncherUI", "Paladins");
				foreach (string phase in _processPhases)
				{
					_gameWindow = Window.FindByProcessName(phase);
					if (_gameWindow != null)
						break;
				}
			}

			return _gameWindow;
		}
	}
}
