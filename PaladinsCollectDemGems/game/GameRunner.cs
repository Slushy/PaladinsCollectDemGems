using PaladinsCollectDemGems.exceptions;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PaladinsCollectDemGems.game
{
	public abstract class GameRunner
	{
		private readonly int _gameId;
		private SteamGame _game;

		protected SteamGame Game { get { return _game; } }

		public bool IsRunning { get { return Game != null; } }

		public GameRunner(int gameId)
		{
			_gameId = gameId;
		}

		public void Start()
		{
			try
			{
				// We want to get the game running since we are.. you know.. the game runner. heh.
				_game = Steam.LaunchGame(_gameId);

				// Begin game runner execution
				Execute();

				// Close the game
				if (!Steam.CloseGame(_game))
				{
					throw new Exception("Failed to close game, process may still be running.");
				}

				Console.WriteLine("Successfully closed game.");
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
				MessageBox.Show(string.Format("Unknown error occurred: {0}", ex.Message));
			}
			finally {
				_game = null;
				// Any child can override and do specific cleanup
				OnExiting();
			}
		}

		public void Reset() {
			if (!IsRunning) return;

			Console.WriteLine("Closing when game runner is still running...");
		}

		protected abstract void Execute();

		protected virtual void OnExiting() { Console.WriteLine("OnExiting..."); }

		protected void Wait(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

		protected void BringGameToFront(int maxTimes = 3)
		{
			int count = 0;
			do
			{
				Wait(100);
				Game.Window.SetForeground();
				Wait(100);
				Game.Window.CenterWindow();
			} while (!Game.Window.IsForegrounded && ++count < maxTimes);

			// If we tried to bring it to the front multiple times, but failed. There was definitely an issue
			if (!Game.Window.IsForegrounded)
				throw new Exception("App is not foregrounded after trying to foreground");
		}
	}
}
