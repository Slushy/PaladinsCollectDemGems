using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System;
using System.Drawing;

namespace PaladinsCollectDemGems.Paladins
{
	/// <summary>
	/// Instruction set which Selects the daily reward from the game screen and notifies success or failure
	/// </summary>
	public class InGameRewardInstructionSet : InstructionSet
	{
		// pre-defined values which matches to all screen resolutions
		private const double LEFT_EDGE_TO_REWARD_BUTTON_PERCENTAGE = 0.42;
		private const double TOP_EDGE_TO_REWARD_BUTTON_PERCENTAGE = 0.68;
		// Used to calculate rewards
		private const double TOP_EDGE_TO_REWARD_LOW = 0.37109375;
		private const double TOP_EDGE_TO_REWARD_HIGH = 0.37890625;
		private const double LEFT_EDGE_TO_REWARD_START = 0.1758242;
		private const double LEFT_EDGE_TO_REWARD_INC = 0.081319;
		private readonly string[] REWARDS = {
			"100 Gold",
			"150 Gold",
			"225 Gold",
			"350 Gold",
			"500 Gold",
			"15 Crystals",
			"35 Crystals"
		};

		private int _dailyRewardIndex = -1;
		private readonly Bitmap _screenValidationImage;
		private Bitmap _matchingWindowImage;

		/// <summary>
		/// Constructs an instruction set to select the daily reward
		/// </summary>
		/// <param name="game"></param>
		public InGameRewardInstructionSet(SteamGame game) : base(game)
		{
			// Load an image to help validate if we are on the game screen with the rewards up
			_screenValidationImage = ImageWizard.LoadImage("Paladins\\images\\DailyRewardScreenThumbnail.png");
		}

		/// <summary>
		/// Executes the instruction set
		/// </summary>
		public override void Execute()
		{
			// Read the current image to see what the reward amount is
			GetDailyRewardAmount();

			// We try to collect the reward 3 times before throwing an exception that we couldn't get it
			int timesToCheck = 3;
			while (MatchesActiveScreen() && --timesToCheck > 0)
			{
				CollectReward();
				Wait(1000);
			}

			if (timesToCheck <= 0)
			{
				throw new Exception("Failed to collect gems");
			}

			Console.WriteLine("Successfully collected daily reward: {0}", REWARDS[_dailyRewardIndex]);
		}

		/// <summary>
		/// Parses the image to get the current day's reward amount
		/// </summary>
		private void GetDailyRewardAmount()
		{
			int ylow = (int)(TOP_EDGE_TO_REWARD_LOW * _matchingWindowImage.Height);
			int yhigh = (int)(TOP_EDGE_TO_REWARD_HIGH * _matchingWindowImage.Height);
			int baseX = (int)(LEFT_EDGE_TO_REWARD_START * _matchingWindowImage.Width);
			int xInc = (int)(LEFT_EDGE_TO_REWARD_INC * _matchingWindowImage.Width);

			int winnerIndex = 0;

			// (7) different rewards
			float[] pixelBrightness = new float[REWARDS.Length];
			for (int i = 0; i < pixelBrightness.Length; i++)
			{
				float maxBrightness = 0;

				for (int baseY = ylow; baseY <= yhigh; baseY++)
				{
					var pixel = _matchingWindowImage.GetPixel(baseX + (i * xInc), baseY);
					maxBrightness = Math.Max(maxBrightness, pixel.GetBrightness());
				}

				// Store the highest brightness index
				pixelBrightness[i] = maxBrightness;
				if (pixelBrightness[i] > pixelBrightness[winnerIndex])
					winnerIndex = i;
			}

			// Just for debugging purposes
			for (int i = 0; i < pixelBrightness.Length; i++)
			{
				Console.WriteLine("Day {0} brightness: {1}", i + 1, pixelBrightness[i]);
			}

			Console.WriteLine("Day {0} is the winner with {1} brightness. Reward: {2}", winnerIndex + 1, pixelBrightness[winnerIndex], REWARDS[winnerIndex]);
			_dailyRewardIndex = winnerIndex;
		}

		/// <summary>
		/// Clicks the button on the game to collect the gems
		/// </summary>
		private void CollectReward()
		{
			BringGameToFront();
			Window window = Game.Window;
			// This selects the play button
			int mouseX = window.Position.Left + (int)(window.Width * LEFT_EDGE_TO_REWARD_BUTTON_PERCENTAGE);
			int mouseY = window.Position.Top + (int)(window.Height * TOP_EDGE_TO_REWARD_BUTTON_PERCENTAGE);

			// The game for some reason doesn't really react when just doing an "in-place" left click,
			// so moving the mouse there, then moving it back
			var origMouseInfo = Mouse.GetMouseInfo();
			Mouse.LeftClick(mouseX, mouseY, true);
			Wait(100);
			Mouse.MoveTo(origMouseInfo.Position.X, origMouseInfo.Position.Y);
			Wait(1000);
		}

		/// <summary>
		/// Used to validate if this should be the active instruction set. Checks multiple times before returning.
		/// </summary>
		/// <returns>true if this instruction set should be active, false otherwise</returns>
		public override bool ValidateAsActiveInstruction()
		{
			// For this instruction set we want to check 10 times
			int timesToCheck = 10;
			while (!MatchesActiveScreen() && --timesToCheck > 0)
			{
				Wait(2500);
				BringGameToFront();
			}

			return timesToCheck > 0;
		}

		/// <summary>
		/// Takes a screenshot of the current game window and retrieves a thumbnail of it. Compare that image
		/// with an already preloaded thumbnail. Return whether or not they are most likely the same image.
		/// </summary>
		/// <returns>true if the current window image matches the current instruction set image, false otherwise</returns>
		private bool MatchesActiveScreen()
		{
			// Capture the window image to figure out what step we are on
			Bitmap windowImage = ImageWizard.GetThumbnailOfWindow(Game.Window);
			double percentageToBeOnSet = ImageWizard.getPercentagePixelMatch(_screenValidationImage, windowImage);

			Console.WriteLine("Reward Step Chance: {0}%", percentageToBeOnSet);
			if (percentageToBeOnSet >= 98.5)
			{
				_matchingWindowImage = windowImage;
				return true;
			}

			return false;
		}
	}
}
