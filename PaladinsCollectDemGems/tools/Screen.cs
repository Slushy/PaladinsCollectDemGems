namespace PaladinsCollectDemGems.tools
{
	/// <summary>
	/// Accessible properties of your computer screen
	/// </summary>
	public static class Screen
	{
		/// <summary>
		/// Width of your primary computer screen
		/// </summary>
		public static int Width { get { return System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width; } }

		/// <summary>
		/// Height of your primary computer screen
		/// </summary>
		public static int Height { get { return System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height; } }
	}
}
