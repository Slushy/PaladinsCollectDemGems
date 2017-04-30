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
		PaladinsGameRunner gameRunner = new PaladinsGameRunner();

		FormBorderStyle _startingBorderStyle = FormBorderStyle.None;

		public MainForm()
		{
			InitializeComponent();

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

		private void launchGameWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine("Starting the paladins game runner.");
			gameRunner.Start();
		}

		private void LaunchGameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void LaunchGameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Console.WriteLine("Done with the game runner, resetting to default styles");
			gameRunner.Reset();
			ResetDefaultStyles();
		}
	}
}
