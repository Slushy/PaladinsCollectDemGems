﻿using PaladinsCollectDemGems.exceptions;
using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System;
using System.Threading;
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
				SteamGame paladinsGame = Steam.LaunchGame(PALADINS_STEAM_GAME_ID);

				if (paladinsGame.Window.IsForegrounded)
					Console.WriteLine("Window is foregrounded");
				else
				{
					Console.WriteLine("Window is NOT foregrounded");

					if (paladinsGame.Window.SetForeground())
						Console.WriteLine("Set Window foreground success");
					else
						Console.WriteLine("Set Window foreground failed");

					if (paladinsGame.Window.IsForegrounded)
						Console.WriteLine("Window is now foregrounded");
				}

				paladinsGame.Window.CenterWindow();

				Thread.Sleep(500);

				Keyboard.Type("username");
				Keyboard.Type(Keyboard.SpecialKey.Tab);
				Keyboard.Type("testpass");
				Keyboard.Type(Keyboard.SpecialKey.Enter);

				Thread.Sleep(5000);
				//MessageBox.Show("Success");
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
				MessageBox.Show(string.Format("Unknown error occurred: {0}", ex.Message));
			}

			
			Console.WriteLine("We did it reddit");
		}
	}
}
