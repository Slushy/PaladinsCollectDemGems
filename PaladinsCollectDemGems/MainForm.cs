using PaladinsCollectDemGems.exceptions;
using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace PaladinsCollectDemGems
{
	public partial class MainForm : Form
	{
		// Move to separate config file or something
		private const int PALADINS_STEAM_GAME_ID = 444090;
		private const int IMAGE_SIZE = 256;
		private const int PIXEL_COUNT = IMAGE_SIZE * IMAGE_SIZE;

		// To find out which step we are on
		private Bitmap _step1Image = null;
		private Bitmap _step2Image = null;
		private SteamGame _paladinsGame = null;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Console.WriteLine("LOADED");
			this.WindowState = FormWindowState.Maximized;
			this.BackColor = Color.Black;
			this.FormBorderStyle = FormBorderStyle.None;

			_step1Image = (Bitmap)Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\resources\\thumbnail.png"));
			_step2Image = (Bitmap)Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\resources\\thumbnail2.png"));

			// Start the process
			var th = new Thread(startPaladinsProcess);
			th.Start();

			var list = Process.GetProcesses();

			Console.WriteLine("HI");
		}

		public void startPaladinsProcess()
		{
			try
			{
				_paladinsGame = Steam.LaunchGame(PALADINS_STEAM_GAME_ID);

				// We can do this multiple times
				Thread.Sleep(100);
				_paladinsGame.Window.SetForeground();
				Thread.Sleep(100);
				_paladinsGame.Window.CenterWindow();
				Thread.Sleep(100);

				// Capture the window image to figure out what step we are on
				Bitmap windowImage = null;
				if (_paladinsGame.Window.IsForegrounded)
				{
					windowImage = (Bitmap)_paladinsGame.Window.CaptureImage()
						.GetThumbnailImage(IMAGE_SIZE, IMAGE_SIZE, () => false, IntPtr.Zero);
					windowImage.Save(@"C:\\temp\\currentScreen.png", ImageFormat.Png);
				}
				else
					throw new Exception("App is not foregrounded after trying to foreground");

				List<bool> currPixels = GetHash(windowImage);
				List<bool> step1Pixels = GetHash(_step1Image);
				List<bool> step2Pixels = GetHash(_step2Image);

				double step1Percentage = pixelCompare(currPixels, step1Pixels);
				double step2Percentage = pixelCompare(currPixels, step2Pixels);

				if (step1Percentage > step2Percentage)
					executeStep1();

				// yaaaa
				executeStep2();

				executeStep3();
			}
			catch (InvalidSteamUserException ex)
			{
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
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Unknown error occurred: {0}", ex.Message));
			}

			Console.WriteLine("We did it reddit");
		}

		private void executeStep1()
		{
			Console.WriteLine("STARTING STEP 1");
			// TODO: Make sure input is selected

			Keyboard.Type("xxxxx");
			Keyboard.Type(Keyboard.SpecialKey.Tab);
			Keyboard.Type("xxxxx");
			Keyboard.Type(Keyboard.SpecialKey.Enter);

			// TODO: Verify i'm on step two
			Thread.Sleep(5000);

			Console.WriteLine("Step 1 complete");
		}

		private void executeStep2()
		{
			Console.WriteLine("STARTING STEP 2");

			// This selects the play button
			int mouseX = _paladinsGame.Window.Position.Left + (int)(_paladinsGame.Window.Width * 0.878799);
			int mouseY = _paladinsGame.Window.Position.Top + (int)(_paladinsGame.Window.Height * 0.692199);
			Mouse.LeftClick(mouseX, mouseY);

			Console.WriteLine("Step 2 complete");
		}

		private void executeStep3()
		{
			Console.WriteLine("STARTING STEP 3");
			Thread.Sleep(10000);

			for (int i = 0; i < 5; i++) {
				var wind = _paladinsGame.Window;
				Thread.Sleep(20000);
				_paladinsGame.Window.SetForeground();
				Thread.Sleep(5000);

				_paladinsGame.Window.UpdateWindowPosition();
				Console.WriteLine(string.Format("Width: {0}", _paladinsGame.Window.Width));
				Console.WriteLine(string.Format("Height: {0}", _paladinsGame.Window.Height));
			}
			

			Console.WriteLine("HI");
		}

		private double pixelCompare(List<bool> one, List<bool> two)
		{
			int equalElements = one.Zip(two, (i, j) => i == j).Count(eq => eq);
			double percentageEqual = (equalElements / (double)PIXEL_COUNT) * 100.0;
			return percentageEqual;
		}

		private List<bool> GetHash(Bitmap bmpSource)
		{
			List<bool> lResult = new List<bool>();
			for (int j = 0; j < bmpSource.Height; j++)
			{
				for (int i = 0; i < bmpSource.Width; i++)
				{
					//reduce colors to true / false                
					lResult.Add(bmpSource.GetPixel(i, j).GetBrightness() < 0.5f);
				}
			}
			return lResult;
		}
	}
}
