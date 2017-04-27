using PaladinsCollectDemGems.game;
using System;
using System.Windows.Forms;

namespace PaladinsCollectDemGems
{
	public partial class MainForm : Form
	{
		// Move to separate config file or something
		private const int PALADINS_STEAM_ID = 444090;

		public MainForm()
		{
			InitializeComponent();

			try {
				// why & such?
				if (Steam.IsLoggedIn)
					Console.WriteLine("User is logged in: {0}", Steam.ActiveUserId);
				else
					Console.WriteLine("No valid steam user");
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("We did it reddit");
		}
	}
}
