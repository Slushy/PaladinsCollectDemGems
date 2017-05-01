using PaladinsCollectDemGems.game;
using PaladinsCollectDemGems.Paladins;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PaladinsCollectDemGems
{
	public partial class MainForm : Form
	{
		BackgroundWorker launchGameWorker = new BackgroundWorker();
		GameRunner gameRunner = new PaladinsGameRunner();

		FormBorderStyle _startingBorderStyle = FormBorderStyle.None;

		public MainForm()
		{
			InitializeComponent();

			// Assigns event methods for the game thread
			launchGameWorker.DoWork += launchGameWorker_DoWork;
			launchGameWorker.ProgressChanged += LaunchGameWorker_ProgressChanged;
			launchGameWorker.RunWorkerCompleted += LaunchGameWorker_RunWorkerCompleted;
			launchGameWorker.WorkerReportsProgress = true;
			launchGameWorker.WorkerSupportsCancellation = true;

			// Setting defaults
			_startingBorderStyle = FormBorderStyle;
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
			// Want to reset it back to normal before minimizing on purpose so then when re-opening 
			// the last size it was is normal, not maxed.
			WindowState = FormWindowState.Normal;
			WindowState = FormWindowState.Minimized;
			FormBorderStyle = _startingBorderStyle;
			BackColor = DefaultBackColor;
			button1.Show();
		}

		// Temp button to get the game running fo testing
		private void button1_Click(object sender, EventArgs e)
		{
			if (gameRunner.IsRunning) {
				Console.WriteLine("The game runner is already running, cannot start it again.");
				return;
			}

			// Setting styles of main form
			WindowState = FormWindowState.Maximized;
			FormBorderStyle = FormBorderStyle.None;
			BackColor = Color.Black;
			button1.Hide();

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
