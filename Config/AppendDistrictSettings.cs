using ColossalFramework;
using ColossalFramework.IO;
using ICities;

namespace AppendDistrict
{
    internal static class AppendDistrictSettings
    {
        private const string SettingsFileName = "AppendDistrict";
        private static bool _settingsFileRegistered;

        private static readonly SavedBool EnableServicePanels = new SavedBool(
            "EnableServicePanels",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool EnableAllVehiclePanels = new SavedBool(
            "EnableAllVehiclePanels",
            GetSettingsFileName(),
            false,
            true);

        private static readonly SavedBool EnableCitizenPanels = new SavedBool(
            "EnableCitizenPanels",
            GetSettingsFileName(),
            false,
            true);

        private static readonly SavedBool ServiceOwnerField = new SavedBool(
            "ServiceOwnerField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool ServiceTargetField = new SavedBool(
            "ServiceTargetField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool VehicleOwnerField = new SavedBool(
            "VehicleOwnerField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool VehicleTargetField = new SavedBool(
            "VehicleTargetField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool CitizenResidenceField = new SavedBool(
            "CitizenResidenceField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool CitizenWorkplaceField = new SavedBool(
            "CitizenWorkplaceField",
            GetSettingsFileName(),
            true,
            true);

        private static readonly SavedBool CitizenTargetField = new SavedBool(
            "CitizenTargetField",
            GetSettingsFileName(),
            true,
            true);

        public static bool IsServicePanelsEnabled => EnableServicePanels.value;
        public static bool IsAllVehiclePanelsEnabled => EnableAllVehiclePanels.value;
        public static bool IsCitizenPanelsEnabled => EnableCitizenPanels.value;
        public static bool IsServiceOwnerFieldEnabled => ServiceOwnerField.value;
        public static bool IsServiceTargetFieldEnabled => ServiceTargetField.value;
        public static bool IsVehicleOwnerFieldEnabled => VehicleOwnerField.value;
        public static bool IsVehicleTargetFieldEnabled => VehicleTargetField.value;
        public static bool IsCitizenResidenceFieldEnabled => CitizenResidenceField.value;
        public static bool IsCitizenWorkplaceFieldEnabled => CitizenWorkplaceField.value;
        public static bool IsCitizenTargetFieldEnabled => CitizenTargetField.value;

        /// <summary>
        /// Builds the mod configuration UI groups and toggles.
        /// </summary>
        /// <param name="helper">UI helper provided by the game.</param>
        public static void BuildSettingsUI(UIHelperBase helper)
        {
            if (helper == null)
                return;

            UIHelperBase serviceGroup = helper.AddGroup("Service Only");
            AddToggle(serviceGroup, "Enable service panels", EnableServicePanels, "Service panels enabled");
            AddToggle(serviceGroup, "Owner field", ServiceOwnerField, "Service owner field");
            AddToggle(serviceGroup, "Target / destination / responding field", ServiceTargetField, "Service target field");

            UIHelperBase vehicleGroup = helper.AddGroup("All Vehicles");
            AddToggle(vehicleGroup, "Enable all vehicle panels", EnableAllVehiclePanels, "All vehicle panels enabled");
            AddToggle(vehicleGroup, "Owner field (public transport)", VehicleOwnerField, "Vehicle owner field");
            AddToggle(vehicleGroup, "Target / destination / responding field", VehicleTargetField, "Vehicle target field");

            UIHelperBase citizensGroup = helper.AddGroup("Citizens");
            AddToggle(citizensGroup, "Enable citizen panels", EnableCitizenPanels, "Citizen panels enabled");
            AddToggle(citizensGroup, "Residence / stay-at field", CitizenResidenceField, "Citizen residence field");
            AddToggle(citizensGroup, "Workplace / operator-at field", CitizenWorkplaceField, "Citizen workplace field");
            AddToggle(citizensGroup, "Target / destination field", CitizenTargetField, "Citizen target field");
        }

        // Adds a checkbox bound to a SavedBool and persists the setting immediately.
        private static void AddToggle(UIHelperBase group, string label, SavedBool setting, string logLabel)
        {
            group.AddCheckbox(
                label,
                setting.value,
                isEnabled =>
                {
                    setting.value = isEnabled;
                    GameSettings.SaveAll();
                    AppendDistrictLog.Info("Settings", logLabel + " " + isEnabled);
                });
        }

        // Ensures the settings file is registered before returning its name.
        private static string GetSettingsFileName()
        {
            EnsureSettingsFileRegistered();
            return SettingsFileName;
        }

        // Registers the mod settings file once so SavedBool can persist values.
        private static void EnsureSettingsFileRegistered()
        {
            if (_settingsFileRegistered)
                return;

            _settingsFileRegistered = true;

            if (GameSettings.FindSettingsFileByName(SettingsFileName) == null)
                GameSettings.AddSettingsFile(new[] { new SettingsFile { fileName = SettingsFileName } });
        }
    }
}
