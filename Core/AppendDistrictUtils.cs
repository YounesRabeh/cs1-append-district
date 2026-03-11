using ColossalFramework.UI;

namespace AppendDistrict
{
    internal static class AppendDistrictUtils
    {
        /// <summary>
        /// Tries to resolve current vehicle data from the provided panel.
        /// </summary>
        /// <param name="panel">World info panel to inspect.</param>
        /// <param name="vehicleId">Resolved vehicle identifier.</param>
        /// <param name="vehicleData">Resolved vehicle data entry.</param>
        /// <returns><c>true</c> when a valid vehicle is found; otherwise <c>false</c>.</returns>
        public static bool TryGetCurrentVehicleData(WorldInfoPanel panel, out ushort vehicleId, out Vehicle vehicleData)
        {
            return AppendDistrictReflection.TryGetCurrentVehicleData(panel, out vehicleId, out vehicleData);
        }

        /// <summary>
        /// Applies district suffix formatting to the service owner field using current settings.
        /// </summary>
        /// <param name="panel">City service vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        public static void TryAppendDistrictToServiceOwner(CityServiceVehicleWorldInfoPanel panel, ref Vehicle vehicleData)
        {
            TryAppendDistrictToServiceOwner(panel, ref vehicleData, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the service owner field.
        /// </summary>
        /// <param name="panel">City service vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToServiceOwner(CityServiceVehicleWorldInfoPanel panel, ref Vehicle vehicleData, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsServiceOwnerFieldEnabled)
                return;

            UIButton ownerButton;
            if (!AppendDistrictReflection.TryGetButton(panel, AppendDistrictButtonKind.ServiceOwner, out ownerButton))
                return;

            ushort buildingId;
            if (!AppendDistrictReflection.TryGetBuildingIdFromButtonUserData(ownerButton, out buildingId))
                buildingId = vehicleData.m_sourceBuilding;

            AppendDistrictTextFormatter.TryApplyDistrictToButton(panel, ownerButton, buildingId);
        }

        /// <summary>
        /// Applies district suffix formatting to the service target field using current settings.
        /// </summary>
        /// <param name="panel">City service vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        public static void TryAppendDistrictToServiceTarget(CityServiceVehicleWorldInfoPanel panel, ref Vehicle vehicleData)
        {
            TryAppendDistrictToServiceTarget(panel, ref vehicleData, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the service target field.
        /// </summary>
        /// <param name="panel">City service vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToServiceTarget(CityServiceVehicleWorldInfoPanel panel, ref Vehicle vehicleData, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsServiceTargetFieldEnabled)
                return;

            UIButton targetButton;
            if (!AppendDistrictReflection.TryGetButton(panel, AppendDistrictButtonKind.VehicleTarget, out targetButton))
                return;

            ushort buildingId;
            if (!AppendDistrictReflection.TryGetBuildingIdFromButtonUserData(targetButton, out buildingId))
                buildingId = vehicleData.m_targetBuilding;

            AppendDistrictTextFormatter.TryApplyDistrictToButton(panel, targetButton, buildingId);
        }

        /// <summary>
        /// Applies district suffix formatting to the public transport owner field using current settings.
        /// </summary>
        /// <param name="panel">Public transport vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        public static void TryAppendDistrictToPublicTransportOwner(PublicTransportVehicleWorldInfoPanel panel, ref Vehicle vehicleData)
        {
            TryAppendDistrictToPublicTransportOwner(panel, ref vehicleData, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the public transport owner field.
        /// </summary>
        /// <param name="panel">Public transport vehicle panel instance.</param>
        /// <param name="vehicleData">Vehicle data associated with the active panel.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToPublicTransportOwner(PublicTransportVehicleWorldInfoPanel panel, ref Vehicle vehicleData, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsVehicleOwnerFieldEnabled)
                return;

            UIButton ownerButton;
            if (!AppendDistrictReflection.TryGetButton(panel, AppendDistrictButtonKind.PublicTransportOwner, out ownerButton))
                return;

            ushort buildingId;
            if (!AppendDistrictReflection.TryGetBuildingIdFromButtonUserData(ownerButton, out buildingId))
                buildingId = vehicleData.m_sourceBuilding;

            AppendDistrictTextFormatter.TryApplyDistrictToButton(panel, ownerButton, buildingId);
        }

        /// <summary>
        /// Applies district suffix formatting to a generic vehicle target field using current settings.
        /// </summary>
        /// <param name="panel">Vehicle panel instance.</param>
        public static void TryAppendDistrictToVehicleTarget(VehicleWorldInfoPanel panel)
        {
            TryAppendDistrictToVehicleTarget(panel, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to a generic vehicle target field.
        /// </summary>
        /// <param name="panel">Vehicle panel instance.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToVehicleTarget(VehicleWorldInfoPanel panel, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsVehicleTargetFieldEnabled)
                return;

            UIButton targetButton;
            if (!AppendDistrictReflection.TryGetButton(panel, AppendDistrictButtonKind.VehicleTarget, out targetButton))
                return;

            ushort buildingId;
            if (!AppendDistrictReflection.TryGetBuildingIdFromButtonUserData(targetButton, out buildingId))
            {
                ushort vehicleId;
                Vehicle vehicleData;
                if (!AppendDistrictReflection.TryGetCurrentVehicleData(panel, out vehicleId, out vehicleData))
                    return;

                buildingId = vehicleData.m_targetBuilding;
            }

            AppendDistrictTextFormatter.TryApplyDistrictToButton(panel, targetButton, buildingId);
        }

        /// <summary>
        /// Applies district suffix formatting to the human workplace field using current settings.
        /// </summary>
        /// <param name="panel">Human panel instance.</param>
        public static void TryAppendDistrictToHumanWorkplace(HumanWorldInfoPanel panel)
        {
            TryAppendDistrictToHumanWorkplace(panel, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the human workplace field.
        /// </summary>
        /// <param name="panel">Human panel instance.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToHumanWorkplace(HumanWorldInfoPanel panel, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsCitizenWorkplaceFieldEnabled)
                return;

            TryAppendDistrictFromButtonUserData(panel, AppendDistrictButtonKind.HumanWorkplace);
        }

        /// <summary>
        /// Applies district suffix formatting to the human target field using current settings.
        /// </summary>
        /// <param name="panel">Human panel instance.</param>
        public static void TryAppendDistrictToHumanTarget(HumanWorldInfoPanel panel)
        {
            TryAppendDistrictToHumanTarget(panel, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the human target field.
        /// </summary>
        /// <param name="panel">Human panel instance.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToHumanTarget(HumanWorldInfoPanel panel, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsCitizenTargetFieldEnabled)
                return;

            TryAppendDistrictFromButtonUserData(panel, AppendDistrictButtonKind.HumanTarget);
        }

        /// <summary>
        /// Applies district suffix formatting to the citizen residence field using current settings.
        /// </summary>
        /// <param name="panel">Citizen panel instance.</param>
        public static void TryAppendDistrictToCitizenResidence(CitizenWorldInfoPanel panel)
        {
            TryAppendDistrictToCitizenResidence(panel, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the citizen residence field.
        /// </summary>
        /// <param name="panel">Citizen panel instance.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToCitizenResidence(CitizenWorldInfoPanel panel, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsCitizenResidenceFieldEnabled)
                return;

            TryAppendDistrictFromButtonUserData(panel, AppendDistrictButtonKind.CitizenResidence);
        }

        /// <summary>
        /// Applies district suffix formatting to the tourist hotel field using current settings.
        /// </summary>
        /// <param name="panel">Tourist panel instance.</param>
        public static void TryAppendDistrictToTouristHotel(TouristWorldInfoPanel panel)
        {
            TryAppendDistrictToTouristHotel(panel, AppendDistrictConfig.Read());
        }

        /// <summary>
        /// Applies district suffix formatting to the tourist hotel field.
        /// </summary>
        /// <param name="panel">Tourist panel instance.</param>
        /// <param name="config">Configuration snapshot for this update pass.</param>
        public static void TryAppendDistrictToTouristHotel(TouristWorldInfoPanel panel, AppendDistrictConfigSnapshot config)
        {
            if (!config.IsCitizenResidenceFieldEnabled)
                return;

            TryAppendDistrictFromButtonUserData(panel, AppendDistrictButtonKind.TouristHotel);
        }

        // Resolves a building ID from button user data and applies district formatting.
        private static void TryAppendDistrictFromButtonUserData(WorldInfoPanel panel, AppendDistrictButtonKind buttonKind)
        {
            UIButton button;
            if (!AppendDistrictReflection.TryGetButton(panel, buttonKind, out button))
                return;

            ushort buildingId;
            if (!AppendDistrictReflection.TryGetBuildingIdFromButtonUserData(button, out buildingId))
                return;

            AppendDistrictTextFormatter.TryApplyDistrictToButton(panel, button, buildingId);
        }
    }
}
