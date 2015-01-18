using System;

namespace Codice.CmdRunner
{
    public class PlatformIdentifier
    {
		[Serializable]
		public enum Platform
		{
			Windows,
			Mac,
			Linux,
			UnKnown
		}

		private static Platform actualPlatform = Platform.UnKnown;

		/// <summary>
		/// Determines if is mac OSystem.
		/// </summary>
		/// <returns><c>true</c> if is mac O; otherwise, <c>false</c>.</returns>
		private static bool IsMacOS()
		{
			bool bIsMac = false;
			/* The first versions of the framework (1.0 and 1.1)
			 * didn't include any PlatformID value for Unix,
			 * so Mono used the value 128. The newer framework 2.0
			 * added Unix to the PlatformID enum but,
			 * sadly, with a different value: 4 and newer versions of
			 * .NET distinguished between Unix and MacOS X,
			 * introducing yet another value 6 for MacOS X.
			 */
			System.Version v = Environment.Version;
			int p = (int)Environment.OSVersion.Platform;
			if ((v.Major >= 3 && v.Minor >= 5) || 
			    (IsRunningUnderMono () && v.Major >= 2 && v.Minor >= 2)) 
			{
				//MacOs X exist in the enumeration
				bIsMac = p == 6;
			}
			else {
				if ((p == 4) || (p == 128)) 
				{
					int major = Environment.OSVersion.Version.Major;
					// Darwin tiger is 8, darwin leopard is 9,
					// darwin snow leopard is 10
					// This is not very nice, as it may conflict
					// on other OS like Solaris or AIX.
					bIsMac = (major == 8 || major == 9 || major == 10);
				}
			}

			return bIsMac;
		}

		/// <summary>
		/// Gets the platform (Windows, Mac or Linux)
		/// </summary>
		/// <value>The get system platform.</value>
		public static Platform GetPlatform()
		{
			if (actualPlatform == Platform.UnKnown) 
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32Windows || 
					Environment.OSVersion.Platform == PlatformID.Win32NT || 
				    Environment.OSVersion.Platform == PlatformID.Win32S || 
				    Environment.OSVersion.Platform == PlatformID.WinCE) 
				{
					actualPlatform = Platform.Windows;
				} 
				else if (IsMacOS ())
				{
					actualPlatform = Platform.Mac;
				} 
				else 
				{
					actualPlatform = Platform.Linux;
				}
			}
			return actualPlatform;
		}

		/// <summary>
		/// Determines if is running under Mono Runtime.
		/// </summary>
		/// <returns><c>true</c> if is running under mono; otherwise, <c>false</c>.</returns>
        private static bool IsRunningUnderMono()
        {
            Type t = Type.GetType("Mono.Runtime");

            return (t != null);
        }

		/// <summary>
		/// Gets a value indicating whether this run under windows.
		/// </summary>
		/// <value><c>true</c> if is windows; otherwise, <c>false</c>.</value>
		public static bool isWindows
		{
			get
			{
				return (GetPlatform () == Platform.Windows);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this run under mac.
		/// </summary>
		/// <value><c>true</c> if is mac; otherwise, <c>false</c>.</value>
		public static bool isMac
		{
			get
			{
				return (GetPlatform () == Platform.Mac);
			}
		}        
    }
}
