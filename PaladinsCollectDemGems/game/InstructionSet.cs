using System;
using System.Threading;

namespace PaladinsCollectDemGems.game
{
	public abstract class InstructionSet
	{
		protected readonly SteamGame Game;

		public InstructionSet(SteamGame game) {
			this.Game = game;
		}

		public abstract void Execute();

		public abstract bool ValidateAsActiveInstruction();

		public virtual void Dispose() { }

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
