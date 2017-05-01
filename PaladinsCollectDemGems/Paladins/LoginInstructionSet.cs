using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PaladinsCollectDemGems.Paladins
{
	/// <summary>
	/// Instruction set which logs a user from the game launcher into the next screen.
	/// </summary>
	public class LoginInstructionSet : InstructionSet
	{
		// pre-defined values which matches to all screen resolutions
		private const double LEFT_EDGE_TO_INPUT_PERCENTAGE = 0.18281;
		private const double TOP_EDGE_TO_INPUT_PERCENTAGE = 0.27905;
		private readonly Bitmap _screenValidationImage;

		/// <summary>
		/// Constructs the login instruction set
		/// </summary>
		/// <param name="game">the Steam game to run the instructions on</param>
		public LoginInstructionSet(SteamGame game) : base(game)
		{
			// Load an image to help validate if we are on the login screen
			_screenValidationImage = ImageWizard.LoadImage("Paladins\\images\\LoginScreenThumbnail.png");
		}

		/// <summary>
		/// Executes the instruction set
		/// </summary>
		public override void Execute()
		{
			SelectInputBox();
			InputUserCredentials();
			Login();
		}

		/// <summary>
		/// Select the username text field so we can begin typing. TODO: Ctrl-a to clear prev input?
		/// </summary>
		private void SelectInputBox() {
			Window window = Game.Window;
			int mousePosX = window.Position.Left + (int)(window.Width * LEFT_EDGE_TO_INPUT_PERCENTAGE);
			int mousePosY = window.Position.Top + (int)(window.Height * TOP_EDGE_TO_INPUT_PERCENTAGE);
			Mouse.LeftClick(mousePosX, mousePosY);
		}

		/// <summary>
		/// Send commands to the keyboard to type in user credentials
		/// </summary>
		private void InputUserCredentials() {
			// 2.) Type username and password
			Keyboard.Type("xxxxxxxxxxxxx");
			Wait(10);
			Keyboard.Type(Keyboard.SpecialKey.Tab);
			Wait(10);
			Keyboard.Type("xxxxxxxxxxxxx");
			Wait(10);
		}

		/// <summary>
		/// Once credentials are typed in, press "ENTER" to login.
		/// </summary>
		private void Login() {
			Keyboard.Type(Keyboard.SpecialKey.Enter);
			Wait(100); // Just making sure enter key was definitely registered here before continuing
		}

		/// <summary>
		/// Used to validate if this should be the active instruction set. Checks multiple times before returning.
		/// </summary>
		/// <returns>true if this instruction set should be active, false otherwise</returns>
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

		/// <summary>
		/// Takes a screenshot of the current game window and retrieves a thumbnail of it. Compare that image
		/// with an already preloaded thumbnail of the login screen. Return whether or not they are most likely the same image.
		/// </summary>
		/// <returns>true if the current window image matches the preloaded login screen image, false otherwise</returns>
		private bool MatchesActiveScreen() {
			// Capture the window image to figure out what step we are on
			Bitmap windowImage = ImageWizard.GetThumbnailOfWindow(Game.Window);
			double percentageToBeOnSet = ImageWizard.getPercentagePixelMatch(_screenValidationImage, windowImage);

			Console.WriteLine("Step Chance: {0}%", percentageToBeOnSet);
			return percentageToBeOnSet >= 90.0;
		}
	}
}
