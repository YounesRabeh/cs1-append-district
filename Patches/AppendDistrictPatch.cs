using System;
using Harmony;

namespace AppendDistrict
{
    [HarmonyPatch(typeof(CityServiceVehicleWorldInfoPanel), "UpdateBindings")]
    internal static class CityServiceVehicleWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates service owner and target fields with district names.
        static void Postfix(CityServiceVehicleWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsServicePanelsEnabled)
                return;

            try
            {
                ushort vehicleId;
                Vehicle vehicleData;
                if (!AppendDistrictUtils.TryGetCurrentVehicleData(__instance, out vehicleId, out vehicleData))
                    return;

                AppendDistrictUtils.TryAppendDistrictToServiceOwner(__instance, ref vehicleData, config);
                AppendDistrictUtils.TryAppendDistrictToServiceTarget(__instance, ref vehicleData, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "CityServiceVehicleWorldInfoPanelPatch", ex);
            }
        }
    }

    [HarmonyPatch(typeof(PublicTransportVehicleWorldInfoPanel), "UpdateBindings")]
    internal static class PublicTransportVehicleWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates the public transport owner field with district names.
        static void Postfix(PublicTransportVehicleWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsAllVehiclePanelsEnabled)
                return;

            try
            {
                ushort vehicleId;
                Vehicle vehicleData;
                if (!AppendDistrictUtils.TryGetCurrentVehicleData(__instance, out vehicleId, out vehicleData))
                    return;

                AppendDistrictUtils.TryAppendDistrictToPublicTransportOwner(__instance, ref vehicleData, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "PublicTransportVehicleWorldInfoPanelPatch", ex);
            }
        }
    }

    [HarmonyPatch(typeof(VehicleWorldInfoPanel), "UpdateBindings")]
    internal static class VehicleWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates generic vehicle target fields with district names.
        static void Postfix(VehicleWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsAllVehiclePanelsEnabled)
                return;
            if (__instance is CityServiceVehicleWorldInfoPanel || __instance is PublicTransportVehicleWorldInfoPanel)
                return;

            try
            {
                AppendDistrictUtils.TryAppendDistrictToVehicleTarget(__instance, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "VehicleWorldInfoPanelPatch", ex);
            }
        }
    }

    [HarmonyPatch(typeof(HumanWorldInfoPanel), "UpdateBindings")]
    internal static class HumanWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates human workplace and target fields with district names.
        static void Postfix(HumanWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsCitizenPanelsEnabled)
                return;

            try
            {
                AppendDistrictUtils.TryAppendDistrictToHumanWorkplace(__instance, config);
                AppendDistrictUtils.TryAppendDistrictToHumanTarget(__instance, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "HumanWorldInfoPanelPatch", ex);
            }
        }
    }

    [HarmonyPatch(typeof(CitizenWorldInfoPanel), "UpdateBindings")]
    internal static class CitizenWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates citizen residence fields with district names.
        static void Postfix(CitizenWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsCitizenPanelsEnabled)
                return;

            try
            {
                AppendDistrictUtils.TryAppendDistrictToCitizenResidence(__instance, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "CitizenWorldInfoPanelPatch", ex);
            }
        }
    }

    [HarmonyPatch(typeof(TouristWorldInfoPanel), "UpdateBindings")]
    internal static class TouristWorldInfoPanelPatch
    {
        private static bool _loggedPatchError;

        // Harmony postfix that updates tourist hotel fields with district names.
        static void Postfix(TouristWorldInfoPanel __instance)
        {
            AppendDistrictConfigSnapshot config = AppendDistrictConfig.Read();
            if (!config.IsCitizenPanelsEnabled)
                return;

            try
            {
                AppendDistrictUtils.TryAppendDistrictToTouristHotel(__instance, config);
            }
            catch (Exception ex)
            {
                AppendDistrictPatchError.LogOnce(ref _loggedPatchError, "TouristWorldInfoPanelPatch", ex);
            }
        }
    }
}
