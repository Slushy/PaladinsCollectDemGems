using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System;
using System.Drawing;

namespace PaladinsCollectDemGems.Paladins
{
	public class LoginInstructionSet : InstructionSet
	{
		private const double LEFT_EDGE_TO_INPUT_PERCENTAGE = 0.18281;
		private const double TOP_EDGE_TO_INPUT_PERCENTAGE = 0.27905;
		private readonly Bitmap _screenValidationImage;

		public LoginInstructionSet(SteamGame game) : base(game)
		{
			_screenValidationImage = ImageWizard.LoadImage("Paladins\\images\\LoginScreen.png");
		}

		public override void Execute()
		{
			SelectInputBox();
			InputUserCredentials();
			Login();
		}

		private void SelectInputBox() {
			Window window = Game.Window;
			int mousePosX = window.Position.Left + (int)(window.Width * LEFT_EDGE_TO_INPUT_PERCENTAGE);
			int mousePosY = window.Position.Top + (int)(window.Height * TOP_EDGE_TO_INPUT_PERCENTAGE);
			Mouse.LeftClick(mousePosX, mousePosY);
		}

		private void InputUserCredentials() {
			// 2.) Type username and password
			Keyboard.Type("slushnstuff");
			Wait(10);
			Keyboard.Type(Keyboard.SpecialKey.Tab);
			Wait(10);
			Keyboard.Type("Xnarules1281");
			Wait(10);
		}

		private void Login() {
			Keyboard.Type(Keyboard.SpecialKey.Enter);
			Wait(100); // Just making sure enter key was definitely registered here before continuing
		}

		public override bool ValidateAsActiveInstruction()
		{
			// For this instruction set we only want to check twice
			int timesToCheck = 2;
			while (!MatchesActiveScreen() && --timesToCheck > 0) {
				Wait(5000);
				BringGameToFront();
			}

			return timesToCheck > 0;
		}

		private bool MatchesActiveScreen() {
			// Capture the window image to figure out what step we are on
			Bitmap windowImage = ImageWizard.GetThumbnailOfWindow(Game.Window);
			double percentageToBeOnSet = ImageWizard.getPercentagePixelMatch(_screenValidationImage, windowImage);

			Console.WriteLine("Step Chance: {0}%", percentageToBeOnSet);
			return percentageToBeOnSet >= 90.0;
		}
	}
}
