using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System.Drawing;
using System;
using System.Drawing.Imaging;

namespace PaladinsCollectDemGems.Paladins
{
	/// <summary>
	/// Instruction set which presses the "Play" button on the pre-game scene, to launch the game.
	/// </summary>
	public class LoadingGameScreenInstructionSet : InstructionSet
	{
		// pre-defined values which matches to all screen resolutions
		private const double LEFT_EDGE_TO_PLAY_BUTTON_PERCENTAGE = 0.878799;
		private const double TOP_EDGE_TO_PLAY_BUTTON_PERCENTAGE = 0.692199;
		private readonly Bitmap _screenValidationImage;

		/// <summary>
		/// Constructs the instruction set
		/// </summary>
		/// <param name="game">the Steam game to run the instructions on</param>
		public LoadingGameScreenInstructionSet(SteamGame game) : base(game)
		{
			// Load an image to help validate if we are on the pre-game after-login screen
			_screenValidationImage = ImageWizard.LoadImage("Paladins\\images\\ReadyToPlayScreenThumbnail.png");
		}

		/// <summary>
		/// Executes the instruction set
		/// </summary>
		public override void Execute()
		{
			SelectPlayButton();
		}

		/// <summary>
		/// Moves the mouse to the Play button on the game, and press it.
		/// </summary>
		private void SelectPlayButton() {
			Window window = Game.Window;
			// This selects the play button
			int mouseX = window.Position.Left + (int)(window.Width * LEFT_EDGE_TO_PLAY_BUTTON_PERCENTAGE);
			int mouseY = window.Position.Top + (int)(window.Height * TOP_EDGE_TO_PLAY_BUTTON_PERCENTAGE);
			Mouse.LeftClick(mouseX, mouseY);
		}

		/// <summary>
		/// Used to validate if this should be the active instruction set. Checks multiple times before returning.
		/// </summary>
		/// <returns>true if this instruction set should be active, false otherwise</returns>
		public override bool ValidateAsActiveInstruction()
		{
			// For this instruction set we only want to check twice
			int timesToCheck = 5;
			while (!MatchesActiveScreen() && --timesToCheck > 0)
			{
				Wait(5000);
				BringGameToFront();
			}

			return timesToCheck > 0;
		}

		/// <summary>
		/// Takes a screenshot of the current game window and retrieves a thumbnail of it. Compare that image
		/// with an already preloaded thumbnail of the pre-game after-login screen. Return whether or not they are most likely the same image.
		/// </summary>
		/// <returns>true if the current window image matches the preloaded image, false otherwise</returns>
		private bool MatchesActiveScreen()
		{
			// Capture the window image to figure out what step we are on
			Bitmap windowImage = ImageWizard.GetThumbnailOfWindow(Game.Window);
			double percentageToBeOnSet = ImageWizard.getPercentagePixelMatch(_screenValidationImage, windowImage);

			Console.WriteLine("Step Chance: {0}%", percentageToBeOnSet);
			return percentageToBeOnSet >= 85.0;
		}
	}
}
