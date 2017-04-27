using Microsoft.Win32;
using System;

namespace PaladinsCollectDemGems.tools.native
{
	/// <summary>
	/// Provides access to the windows registry to get or set key/values
	/// </summary>
	public static class WinRegistry
	{
		/// <summary>
		/// Accesses the windows registry to get an integer value from a specific key and location
		/// </summary>
		/// <param name="keyLocation">the location of the key to search</param>
		/// <param name="key">the key of the value to return</param>
		/// <returns>integer value for the key at the specified location, null if invalid or doesn't exist</returns>
		public static int? GetValueAsInt(string keyLocation, string key)
		{
			string value = GetValue(keyLocation, key)?.ToString();

			int parsedValue = 0;
			return int.TryParse(value, out parsedValue) ? (int?)parsedValue : null;
		}

		/// <summary>
		/// Accesses the windows registry to get a string value from a specific key and location
		/// </summary>
		/// <param name="keyLocation">the location of the key to search</param>
		/// <param name="key">the key of the value to return</param>
		/// <returns>string value for the key at the specified location, null if invalid or doesn't exist</returns>
		public static string GetValueAsString(string keyLocation, string key)
		{
			return GetValue(keyLocation, key) as string;
		}

		/// <summary>
		/// Accesses the windows registry to get an arbitrary value from a specific key and location
		/// </summary>
		/// <param name="keyLocation">the location of the key to search</param>
		/// <param name="key">the key of the value to return</param>
		/// <returns>arbitrary value for the key at the specified location, null if invalid or doesn't exist</returns>
		public static object GetValue(string keyLocation, string key)
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(keyLocation))
				{
					object val = registryKey?.GetValue(key);
					return val;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error retrieving value at {0}\\{1}", keyLocation, key);
				throw ex;
			}
		}
	}
}
