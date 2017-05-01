using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.Paladins;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PaladinsCollectDemGems
{
	public partial class MainForm : Form
	{
		BackgroundWorker launchGameWorker = new BackgroundWorker();
		GameRunner gameRunner = new PaladinsGameRunner();

		BackgroundCover _backgroundCover = new BackgroundCover();

		public MainForm()
		{
			InitializeComponent();

			// Assigns event methods for the game thread
			launchGameWorker.DoWork += launchGameWorker_DoWork;
			launchGameWorker.ProgressChanged += LaunchGameWorker_ProgressChanged;
			launchGameWorker.RunWorkerCompleted += LaunchGameWorker_RunWorkerCompleted;
			launchGameWorker.WorkerReportsProgress = true;
			launchGameWorker.WorkerSupportsCancellation = true;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			ResetDefaultStyles();
		}

		/// <summary>
		/// Resets the form to default appearance
		/// </summary>
		private void ResetDefaultStyles()
		{
			// Hide the background and show this form
			_backgroundCover.Hide();
			Show();
		}

		// Temp button to get the game running fo testing
		private void button1_Click(object sender, EventArgs e)
		{
			if (gameRunner.IsRunning) {
				Console.WriteLine("The game runner is already running, cannot start it again.");
				return;
			}

			// Hide this form and show the background
			Hide();
			_backgroundCover.Show();

			// Starts the background thread to run the game runner
			launchGameWorker.RunWorkerAsync();
		}

		/// <summary>
		/// Starts the game runner in a separate thread
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void launchGameWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine("Starting the paladins game runner.");
			gameRunner.Start();
		}

		/// <summary>
		/// When the game thread decides to report info to main thread, this is called.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LaunchGameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// When the game thread has finished this is called in main thread
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LaunchGameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Console.WriteLine("Done with the game runner, resetting to default styles");
			gameRunner.Reset();
			ResetDefaultStyles();
		}
	}
}
