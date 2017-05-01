using System;
using System.Threading;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Represents a set of steps a game runner can execute
	/// </summary>
	public abstract class InstructionSet
	{
		protected readonly SteamGame Game;

		/// <summary>
		/// Constructs an instruction set
		/// </summary>
		/// <param name="game">the Steam game to run the instructions on</param>
		public InstructionSet(SteamGame game) {
			this.Game = game;
		}

		/// <summary>
		/// Begins execution of a specific instruction set's steps
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// Each instruction set has the ability to decide whether or not it should be ran next. This
		/// method is called from a game runner to decide whether or not to execute it next.
		/// </summary>
		/// <returns>true if the instruction set should be active</returns>
		public abstract bool ValidateAsActiveInstruction();

		/// <summary>
		/// Cleans up the instruction set
		/// </summary>
		public virtual void Dispose() { }

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
				if (Game?.Window == null)
				{
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
