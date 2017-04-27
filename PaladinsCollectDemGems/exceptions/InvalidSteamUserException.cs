using System;

namespace PaladinsCollectDemGems.exceptions
{
	/// <summary>
	/// An exception for if there is current logged in steam user, or if the account is invalid or unretrievable
	/// </summary>
	public class InvalidSteamUserException : Exception
	{
		/// <summary>
		/// Initializes new instance of InvalidSteamUserException class
		/// </summary>
		public InvalidSteamUserException()
		{
		}

		/// <summary>
		/// Initializes new instance of InvalidSteamUserException class with a specified error message
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		public InvalidSteamUserException(string message)
		: base(message)
		{
		}

		/// <summary>
		/// Initializes new instance of InvalidSteamUserException class with a specified error message and 
		/// a reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">the message that explains the reason for the error</param>
		/// <param name="innerException">the inner exception that caused this exception</param>
		public InvalidSteamUserException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
