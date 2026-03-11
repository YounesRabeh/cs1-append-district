using System;

namespace AppendDistrict
{
    internal static class AppendDistrictBuildInfo
    {
        private const string Author = "TheYuyuBoy";
        private const string BaseDescription = "Appends district names to vehicle and citizen info fields.";

        public static string ModDescription => BaseDescription + " v" + GetVersionForDisplay() + " by " + Author;

        // Builds a stable major.minor.build version string for UI display.
        private static string GetVersionForDisplay()
        {
            Version version = typeof(AppendDistrictBuildInfo).Assembly.GetName().Version;
            if (version == null)
                return "dev";

            return version.Major + "." + version.Minor + "." + Math.Max(0, version.Build);
        }
    }
}
