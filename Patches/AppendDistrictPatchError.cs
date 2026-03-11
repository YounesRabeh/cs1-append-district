using System;

namespace AppendDistrict
{
    internal static class AppendDistrictPatchError
    {
        /// <summary>
        /// Logs a patch exception only once for a given patch state flag.
        /// </summary>
        /// <param name="hasLoggedError">Tracks whether the patch error has already been logged.</param>
        /// <param name="patchName">Patch name used in the log entry.</param>
        /// <param name="ex">The captured exception.</param>
        public static void LogOnce(ref bool hasLoggedError, string patchName, Exception ex)
        {
            if (hasLoggedError)
                return;

            hasLoggedError = true;
            AppendDistrictLog.Error("Patch", patchName + " failed: " + ex.Message);
        }
    }
}
