using System;
using ColossalFramework;
using ColossalFramework.UI;

namespace AppendDistrict
{
    internal static class AppendDistrictTextFormatter
    {
        /// <summary>
        /// Applies district name formatting to a world info panel button.
        /// </summary>
        /// <param name="panel">The panel that owns the button.</param>
        /// <param name="button">The button to update.</param>
        /// <param name="buildingId">The building identifier associated with the button.</param>
        public static void TryApplyDistrictToButton(WorldInfoPanel panel, UIButton button, ushort buildingId)
        {
            if (button == null || buildingId == 0)
                return;

            string districtName;
            if (!TryGetDistrictNameForBuilding(buildingId, out districtName))
                return;

            string buildingName;
            TryGetBuildingDisplayName(panel, buildingId, out buildingName);

            string sourceText = SelectSourceText(button, buildingName);
            string updatedText = InsertDistrictIntoText(sourceText, buildingName, districtName);
            if (string.IsNullOrEmpty(updatedText))
                return;

            button.text = updatedText;
            button.tooltip = updatedText;
            AppendDistrictReflection.TryShortenButtonText(panel, button);
        }

        /// <summary>
        /// Tries to resolve the district name for a valid building.
        /// </summary>
        /// <param name="buildingId">Building identifier to inspect.</param>
        /// <param name="districtName">Resolved district name when available.</param>
        /// <returns><c>true</c> when a non-empty district name is found; otherwise <c>false</c>.</returns>
        public static bool TryGetDistrictNameForBuilding(ushort buildingId, out string districtName)
        {
            districtName = string.Empty;
            if (buildingId == 0)
                return false;
            if (!Singleton<BuildingManager>.exists || !Singleton<DistrictManager>.exists)
                return false;

            BuildingManager buildingManager = Singleton<BuildingManager>.instance;
            if (buildingId >= buildingManager.m_buildings.m_buffer.Length)
                return false;

            Building building = buildingManager.m_buildings.m_buffer[buildingId];
            if ((building.m_flags & Building.Flags.Created) == 0)
                return false;
            if ((building.m_flags & Building.Flags.Deleted) != 0)
                return false;

            DistrictManager districtManager = Singleton<DistrictManager>.instance;
            byte district = districtManager.GetDistrict(building.m_position);
            if (district == 0)
                return false;

            districtName = districtManager.GetDistrictName(district);
            return !string.IsNullOrEmpty(districtName);
        }

        // Tries to resolve the localized building name for the active panel context.
        private static bool TryGetBuildingDisplayName(WorldInfoPanel panel, ushort buildingId, out string buildingName)
        {
            buildingName = string.Empty;
            if (buildingId == 0 || !Singleton<BuildingManager>.exists)
                return false;

            InstanceID instanceId;
            if (!AppendDistrictReflection.TryGetCurrentInstanceId(panel, out instanceId))
                instanceId = InstanceID.Empty;

            buildingName = Singleton<BuildingManager>.instance.GetBuildingName(buildingId, instanceId);
            return !string.IsNullOrEmpty(buildingName);
        }

        // Inserts district suffix near the building name while preventing duplicate tags.
        private static string InsertDistrictIntoText(string originalText, string buildingName, string districtName)
        {
            if (string.IsNullOrEmpty(originalText) || string.IsNullOrEmpty(districtName))
                return originalText;

            string suffix = "(" + districtName + ")";
            string textWithoutSuffix = TrimTrailingWhitespace(RemoveAllOccurrences(originalText, suffix));

            if (!string.IsNullOrEmpty(buildingName))
            {
                textWithoutSuffix = TrimTrailingWhitespace(StripParentheticalImmediatelyAfterBuildingName(textWithoutSuffix, buildingName));

                int nameIndex = textWithoutSuffix.IndexOf(buildingName, StringComparison.Ordinal);
                if (nameIndex >= 0)
                {
                    string replacement = buildingName + " " + suffix;
                    return textWithoutSuffix.Substring(0, nameIndex) +
                           replacement +
                           textWithoutSuffix.Substring(nameIndex + buildingName.Length);
                }
            }

            if (LooksTruncated(textWithoutSuffix))
                return textWithoutSuffix;

            return AppendSuffixIfMissing(textWithoutSuffix, suffix);
        }

        // Appends the district suffix only when it is not already present.
        private static string AppendSuffixIfMissing(string original, string suffix)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(suffix))
                return original;
            if (original.IndexOf(suffix, StringComparison.Ordinal) >= 0)
                return original;

            return original + " " + suffix;
        }

        // Removes every exact occurrence of a value from source text.
        private static string RemoveAllOccurrences(string source, string value)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
                return source;

            string result = source;
            int index = result.IndexOf(value, StringComparison.Ordinal);
            while (index >= 0)
            {
                result = result.Remove(index, value.Length);
                index = result.IndexOf(value, StringComparison.Ordinal);
            }

            return result;
        }

        // Removes the first parenthetical section immediately after the building name.
        private static string StripParentheticalImmediatelyAfterBuildingName(string source, string buildingName)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(buildingName))
                return source;

            int nameIndex = source.IndexOf(buildingName, StringComparison.Ordinal);
            if (nameIndex < 0)
                return source;

            int nextIndex = nameIndex + buildingName.Length;
            while (nextIndex < source.Length && char.IsWhiteSpace(source[nextIndex]))
                nextIndex++;

            if (nextIndex >= source.Length || source[nextIndex] != '(')
                return source;

            int endParenthesis = source.IndexOf(')', nextIndex);
            if (endParenthesis >= 0)
                return source.Remove(nextIndex, endParenthesis - nextIndex + 1);

            // Handles truncated text like "BuildingName(Distr..." by stripping the partial tag.
            return source.Substring(0, nextIndex);
        }

        // Trims trailing whitespace while preserving the original instance when unchanged.
        private static string TrimTrailingWhitespace(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            int end = source.Length;
            while (end > 0 && char.IsWhiteSpace(source[end - 1]))
                end--;

            return end == source.Length ? source : source.Substring(0, end);
        }

        // Picks the best source string (text or tooltip) to apply district formatting.
        private static string SelectSourceText(UIButton button, string buildingName)
        {
            string text = button.text ?? string.Empty;
            string tooltip = button.tooltip ?? string.Empty;
            if (string.IsNullOrEmpty(buildingName))
                return !string.IsNullOrEmpty(tooltip) ? tooltip : text;

            if (text.IndexOf(buildingName, StringComparison.Ordinal) >= 0)
                return text;
            if (tooltip.IndexOf(buildingName, StringComparison.Ordinal) >= 0)
                return tooltip;

            return !string.IsNullOrEmpty(text) ? text : tooltip;
        }

        // Detects text that was truncated by the game UI.
        private static bool LooksTruncated(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.IndexOf("...", StringComparison.Ordinal) >= 0 ||
                   value.IndexOf('\u2026') >= 0;
        }
    }
}
