using PaladinsCollectDemGems.tools.native;

namespace PaladinsCollectDemGems.game
{
	/// <summary>
	/// Class that acts as the base for launching Steam games
	/// </summary>
	public static class Steam
	{
		public const string BASE_REGISTRY_LOCATION = "Software\\Valve\\Steam\\";
		public const string PROCESS_REGISTRY_LOCATION = BASE_REGISTRY_LOCATION + "ActiveProcess";
		
		/// <summary>
		/// Gets the currently logged in steam userId, or null if n/a
		/// </summary>
		public static int? ActiveUserId {
			get { 
				int? userId = WinRegistry.GetValueAsInt(PROCESS_REGISTRY_LOCATION, "ActiveUser");
				return userId.HasValue && userId > 0 ? userId : null;	
			}
		}

		/// <summary>
		/// Checks if we have a valid steam user logged in
		/// </summary>
		public static bool IsLoggedIn { get { return ActiveUserId.HasValue; } }
	}
}
