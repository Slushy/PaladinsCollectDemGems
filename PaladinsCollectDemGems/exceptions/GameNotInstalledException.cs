using System;

namespace PaladinsCollectDemGems.exceptions
{
	/// <summary>
	/// An exception for if a requested game is not installed
	/// </summary>
	public class GameNotInstalledException : Exception
	{
		/// <summary>
		/// Initializes new instance of GameNotInstalledException class
		/// </summary>
		public GameNotInstalledException()
		{
		}

		/// <summary>
		/// Initializes new instance of GameNotInstalledException class with a specified error message
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		public GameNotInstalledException(string message)
		: base(message)
		{
		}

		/// <summary>
		/// Initializes new instance of GameNotInstalledException class with a specified error message and 
		/// a reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		/// <param name="innerException">the inner exception that caused this exception</param>
		public GameNotInstalledException(string message, Exception innerException)
		: base(message, innerException)
		{
		}
	}
}
