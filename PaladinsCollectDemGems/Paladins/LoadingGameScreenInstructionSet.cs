using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System.Drawing;
using System;

namespace PaladinsCollectDemGems.Paladins
{
	public class LoadingGameScreenInstructionSet : InstructionSet
	{
		private const double LEFT_EDGE_TO_PLAY_BUTTON_PERCENTAGE = 0.878799;
		private const double TOP_EDGE_TO_PLAY_BUTTON_PERCENTAGE = 0.692199;
		private readonly Bitmap _screenValidationImage;

		public LoadingGameScreenInstructionSet(SteamGame game) : base(game)
		{
			_screenValidationImage = ImageWizard.LoadImage("Paladins\\images\\ReadyToPlayScreen.png");
		}

		public override void Execute()
		{
			SelectPlayButton();
		}

		private void SelectPlayButton() {
			Window window = Game.Window;
			// This selects the play button
			int mouseX = window.Position.Left + (int)(window.Width * LEFT_EDGE_TO_PLAY_BUTTON_PERCENTAGE);
			int mouseY = window.Position.Top + (int)(window.Height * TOP_EDGE_TO_PLAY_BUTTON_PERCENTAGE);
			Mouse.LeftClick(mouseX, mouseY);
		}

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
