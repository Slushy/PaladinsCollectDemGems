using PaladinsCollectDemGems.exceptions;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Base class for any specific game runner to control the start/running/exiting process of the game.
	/// </summary>
	public abstract class GameRunner
	{
		private readonly int _gameId;
		private SteamGame _game;
		protected SteamGame Game { get { return _game; } }

		/// <summary>
		/// Checks to see if the game is currently running
		/// </summary>
		public bool IsRunning { get { return Game != null; } }

		/// <summary>
		/// Constructs a game runner
		/// </summary>
		/// <param name="gameId">The steam game ID of the game to run</param>
		public GameRunner(int gameId)
		{
			_gameId = gameId;
		}

		/// <summary>
		/// Starts the game execution sequence. Catches all exceptions and shows a message box (which currently doesn't work because run on separate thread).
		/// </summary>
		public void Start()
		{
			try
			{
				// We want to get the game running since we are.. you know.. the game runner. heh.
				_game = Steam.LaunchGame(_gameId);

				// Begin game runner execution
				Execute();
			}
			catch (InvalidSteamUserException ex)
			{
				MessageBox.Show("Invalid Steam User");
			}
			catch (GameNotInstalledException ex)
			{
				MessageBox.Show("Game is invalid or not installed");
			}
			catch (GameUpdatingException ex)
			{
				MessageBox.Show("Game is currently updating and in bad state");
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR");
				Console.WriteLine(ex.Message);
				Console.WriteLine("END ERROR");
				MessageBox.Show(string.Format("Unknown error occurred: {0}", ex.Message));
			}

			// finally close the game
			finally
			{
				try
				{
					if (IsRunning && !Steam.CloseGame(_game))
						throw new Exception("Failed to close game, process may still be running.");

					Console.WriteLine("Successfully closed game.");
				}
				catch (Exception ex) {
					MessageBox.Show("Failed to close game");
				}
				finally {
					_game = null;
					// Any child can override and do specific cleanup
					OnExiting();
				}
			}
		}

		/// <summary>
		/// TODO: close early, not sure what to do here yet
		/// </summary>
		public void Reset() {
			if (!IsRunning) return;

			Console.WriteLine("Closing when game runner is still running...");
		}

		/// <summary>
		/// Begins execution of a specific game runner's sequence
		/// </summary>
		protected abstract void Execute();

		/// <summary>
		/// Called when the game has been closed and we are exiting the runner
		/// </summary>
		protected virtual void OnExiting() { Console.WriteLine("OnExiting..."); }

		/// <summary>
		/// Wrapper around Thread.sleep that suspends the thread for x amount of milliseconds
		/// </summary>
		/// <param name="milliseconds">time to suspend the thread</param>
		protected void Wait(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

		/// <summary>
		/// Helper function that simply brings the game to the front of the screen and centers it
		/// </summary>
		/// <param name="maxTimes">max amount of timesd to try to bring game to front until failure</param>
		protected void BringGameToFront(int maxTimes = 3)
		{
			int count = 0;
			do
			{
				Console.WriteLine("Trying to foreground");
				if (Game.Window == null) {
					Console.WriteLine("Game.Window == null");
				}

				Wait(100);
				Console.WriteLine("SetForeground");
				Game.Window.SetForeground();
				Wait(100);
				Console.WriteLine("CenterWindow");
				Game.Window.CenterWindow();
			} while (!Game.Window.IsForegrounded && ++count < maxTimes);

			// If we tried to bring it to the front multiple times, but failed. There was definitely an issue
			if (!Game.Window.IsForegrounded)
				throw new Exception("App is not foregrounded after trying to foreground");
		}
	}
}
