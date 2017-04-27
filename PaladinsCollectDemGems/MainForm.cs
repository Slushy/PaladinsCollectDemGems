using PaladinsCollectDemGems.exceptions;
using PaladinsCollectDemGems.game;
using System;
using System.Windows.Forms;

namespace PaladinsCollectDemGems
{
	public partial class MainForm : Form
	{
		// Move to separate config file or something
		private const int PALADINS_STEAM_GAME_ID = 444090;

		public MainForm()
		{
			InitializeComponent();

			try {
				SteamGame paladins = Steam.LaunchGame(PALADINS_STEAM_GAME_ID);

				MessageBox.Show("Success");
			}
			catch (InvalidSteamUserException ex) {
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
			catch (Exception ex) {
				MessageBox.Show("Unknown error occurred: {0}", ex.Message);
			}

			
			Console.WriteLine("We did it reddit");
		}
	}
}
