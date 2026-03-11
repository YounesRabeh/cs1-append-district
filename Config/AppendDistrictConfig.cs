namespace AppendDistrict
{
    internal struct AppendDistrictConfigSnapshot
    {
        public readonly bool IsServicePanelsEnabled;
        public readonly bool IsAllVehiclePanelsEnabled;
        public readonly bool IsCitizenPanelsEnabled;
        public readonly bool IsServiceOwnerFieldEnabled;
        public readonly bool IsServiceTargetFieldEnabled;
        public readonly bool IsVehicleOwnerFieldEnabled;
        public readonly bool IsVehicleTargetFieldEnabled;
        public readonly bool IsCitizenResidenceFieldEnabled;
        public readonly bool IsCitizenWorkplaceFieldEnabled;
        public readonly bool IsCitizenTargetFieldEnabled;

        /// <summary>
        /// Initializes a point-in-time snapshot of all mod feature toggles.
        /// </summary>
        /// <param name="isServicePanelsEnabled">Whether service-only panels are enabled.</param>
        /// <param name="isAllVehiclePanelsEnabled">Whether all vehicle panels are enabled.</param>
        /// <param name="isCitizenPanelsEnabled">Whether citizen panels are enabled.</param>
        /// <param name="isServiceOwnerFieldEnabled">Whether service owner field updates are enabled.</param>
        /// <param name="isServiceTargetFieldEnabled">Whether service target field updates are enabled.</param>
        /// <param name="isVehicleOwnerFieldEnabled">Whether vehicle owner field updates are enabled.</param>
        /// <param name="isVehicleTargetFieldEnabled">Whether vehicle target field updates are enabled.</param>
        /// <param name="isCitizenResidenceFieldEnabled">Whether citizen residence field updates are enabled.</param>
        /// <param name="isCitizenWorkplaceFieldEnabled">Whether citizen workplace field updates are enabled.</param>
        /// <param name="isCitizenTargetFieldEnabled">Whether citizen target field updates are enabled.</param>
        public AppendDistrictConfigSnapshot(
            bool isServicePanelsEnabled,
            bool isAllVehiclePanelsEnabled,
            bool isCitizenPanelsEnabled,
            bool isServiceOwnerFieldEnabled,
            bool isServiceTargetFieldEnabled,
            bool isVehicleOwnerFieldEnabled,
            bool isVehicleTargetFieldEnabled,
            bool isCitizenResidenceFieldEnabled,
            bool isCitizenWorkplaceFieldEnabled,
            bool isCitizenTargetFieldEnabled)
        {
            IsServicePanelsEnabled = isServicePanelsEnabled;
            IsAllVehiclePanelsEnabled = isAllVehiclePanelsEnabled;
            IsCitizenPanelsEnabled = isCitizenPanelsEnabled;
            IsServiceOwnerFieldEnabled = isServiceOwnerFieldEnabled;
            IsServiceTargetFieldEnabled = isServiceTargetFieldEnabled;
            IsVehicleOwnerFieldEnabled = isVehicleOwnerFieldEnabled;
            IsVehicleTargetFieldEnabled = isVehicleTargetFieldEnabled;
            IsCitizenResidenceFieldEnabled = isCitizenResidenceFieldEnabled;
            IsCitizenWorkplaceFieldEnabled = isCitizenWorkplaceFieldEnabled;
            IsCitizenTargetFieldEnabled = isCitizenTargetFieldEnabled;
        }
    }

    internal static class AppendDistrictConfig
    {
        /// <summary>
        /// Reads the current settings and creates a config snapshot for one update pass.
        /// </summary>
        /// <returns>Current configuration snapshot.</returns>
        public static AppendDistrictConfigSnapshot Read()
        {
            return new AppendDistrictConfigSnapshot(
                AppendDistrictSettings.IsServicePanelsEnabled,
                AppendDistrictSettings.IsAllVehiclePanelsEnabled,
                AppendDistrictSettings.IsCitizenPanelsEnabled,
                AppendDistrictSettings.IsServiceOwnerFieldEnabled,
                AppendDistrictSettings.IsServiceTargetFieldEnabled,
                AppendDistrictSettings.IsVehicleOwnerFieldEnabled,
                AppendDistrictSettings.IsVehicleTargetFieldEnabled,
                AppendDistrictSettings.IsCitizenResidenceFieldEnabled,
                AppendDistrictSettings.IsCitizenWorkplaceFieldEnabled,
                AppendDistrictSettings.IsCitizenTargetFieldEnabled);
        }
    }
}
