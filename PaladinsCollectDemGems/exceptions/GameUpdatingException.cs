using System;

namespace PaladinsCollectDemGems.exceptions
{
	/// <summary>
	/// An exception for if the game's state is updating and unavailable for control
	/// </summary>
	public class GameUpdatingException : Exception
	{
		/// <summary>
		/// Initializes new instance of GameUpdatingException class
		/// </summary>
		public GameUpdatingException()
		{
		}

		/// <summary>
		/// Initializes new instance of GameUpdatingException class with a specified error message
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		public GameUpdatingException(string message)
		: base(message)
		{
		}

		/// <summary>
		/// Initializes new instance of GameUpdatingException class with a specified error message and 
		/// a reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		/// <param name="innerException">the inner exception that caused this exception</param>
		public GameUpdatingException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
