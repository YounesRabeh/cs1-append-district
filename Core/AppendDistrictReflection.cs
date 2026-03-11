using System;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AppendDistrict
{
    internal enum AppendDistrictButtonKind
    {
        ServiceOwner,
        PublicTransportOwner,
        VehicleTarget,
        HumanWorkplace,
        HumanTarget,
        CitizenResidence,
        TouristHotel
    }

    internal static class AppendDistrictReflection
    {
        private const BindingFlags AnyInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly FieldInfo CityServiceOwnerButtonField = GetFieldRecursive(typeof(CityServiceVehicleWorldInfoPanel), "m_Owner");
        private static readonly FieldInfo PublicTransportOwnerButtonField = GetFieldRecursive(typeof(PublicTransportVehicleWorldInfoPanel), "m_Owner");
        private static readonly FieldInfo VehicleTargetButtonField = GetFieldRecursive(typeof(VehicleWorldInfoPanel), "m_Target");
        private static readonly FieldInfo HumanWorkplaceButtonField = GetFieldRecursive(typeof(HumanWorldInfoPanel), "m_Workplace");
        private static readonly FieldInfo HumanTargetButtonField = GetFieldRecursive(typeof(HumanWorldInfoPanel), "m_Target");
        private static readonly FieldInfo CitizenResidenceButtonField = GetFieldRecursive(typeof(CitizenWorldInfoPanel), "m_Residence");
        private static readonly FieldInfo TouristHotelButtonField = GetFieldRecursive(typeof(TouristWorldInfoPanel), "m_Hotel");
        private static readonly FieldInfo InstanceIdField = GetFieldRecursive(typeof(WorldInfoPanel), "m_InstanceID");
        private static readonly MethodInfo ShortenTextMethod = GetMethodRecursive(typeof(WorldInfoPanel), "ShortenTextToFitParent", new[] { typeof(UIButton) });

        private static bool _loggedShortenError;

        /// <summary>
        /// Tries to read the active <see cref="InstanceID"/> from a world info panel.
        /// </summary>
        /// <param name="panel">Panel instance to inspect.</param>
        /// <param name="instanceId">Resolved instance identifier when available.</param>
        /// <returns><c>true</c> when the instance ID is read successfully; otherwise <c>false</c>.</returns>
        public static bool TryGetCurrentInstanceId(WorldInfoPanel panel, out InstanceID instanceId)
        {
            instanceId = InstanceID.Empty;
            if (panel == null || !HasValue(InstanceIdField))
                return false;

            object boxed = InstanceIdField.GetValue(panel);
            if (!(boxed is InstanceID))
                return false;

            instanceId = (InstanceID)boxed;
            return true;
        }

        /// <summary>
        /// Tries to resolve the current vehicle identifier and data from a panel.
        /// </summary>
        /// <param name="panel">Panel instance to inspect.</param>
        /// <param name="vehicleId">Resolved vehicle identifier.</param>
        /// <param name="vehicleData">Resolved vehicle data buffer entry.</param>
        /// <returns><c>true</c> when a valid vehicle is available; otherwise <c>false</c>.</returns>
        public static bool TryGetCurrentVehicleData(WorldInfoPanel panel, out ushort vehicleId, out Vehicle vehicleData)
        {
            vehicleId = 0;
            vehicleData = default(Vehicle);

            InstanceID instanceId;
            if (!TryGetCurrentInstanceId(panel, out instanceId))
                return false;

            vehicleId = instanceId.Vehicle;
            if (vehicleId == 0)
                return false;
            if (!Singleton<VehicleManager>.exists)
                return false;

            VehicleManager vehicleManager = Singleton<VehicleManager>.instance;
            if (vehicleId >= vehicleManager.m_vehicles.m_buffer.Length)
                return false;

            vehicleData = vehicleManager.m_vehicles.m_buffer[vehicleId];
            if ((vehicleData.m_flags & Vehicle.Flags.Created) == 0)
                return false;
            if ((vehicleData.m_flags & Vehicle.Flags.Deleted) != 0)
                return false;

            return true;
        }

        /// <summary>
        /// Tries to get a panel button by logical button kind.
        /// </summary>
        /// <param name="panel">Panel instance that owns the button field.</param>
        /// <param name="buttonKind">Logical kind of button to resolve.</param>
        /// <param name="button">Resolved UI button when found.</param>
        /// <returns><c>true</c> when the button is found; otherwise <c>false</c>.</returns>
        public static bool TryGetButton(object panel, AppendDistrictButtonKind buttonKind, out UIButton button)
        {
            button = null;
            if (panel == null)
                return false;

            FieldInfo buttonField = ResolveButtonField(buttonKind);
            if (!HasValue(buttonField))
                return false;

            button = buttonField.GetValue(panel) as UIButton;
            return button != null;
        }

        /// <summary>
        /// Tries to extract a building identifier from button user data.
        /// </summary>
        /// <param name="button">Button to inspect.</param>
        /// <param name="buildingId">Resolved building identifier.</param>
        /// <returns><c>true</c> when user data contains a valid building ID; otherwise <c>false</c>.</returns>
        public static bool TryGetBuildingIdFromButtonUserData(UIButton button, out ushort buildingId)
        {
            buildingId = 0;
            if (button == null)
                return false;

            object objectUserData = button.objectUserData;
            if (!(objectUserData is InstanceID))
                return false;

            InstanceID instanceId = (InstanceID)objectUserData;
            buildingId = instanceId.Building;
            return buildingId != 0;
        }

        /// <summary>
        /// Tries to invoke the game's private text-shortening helper for a button.
        /// </summary>
        /// <param name="panel">Panel owning the shortening method.</param>
        /// <param name="button">Button whose text should be shortened.</param>
        public static void TryShortenButtonText(WorldInfoPanel panel, UIButton button)
        {
            if (panel == null || button == null || !HasValue(ShortenTextMethod))
                return;

            try
            {
                ShortenTextMethod.Invoke(panel, new object[] { button });
            }
            catch (Exception ex)
            {
                if (_loggedShortenError)
                    return;

                _loggedShortenError = true;
                AppendDistrictLog.Warn("Shorten", "Failed to shorten button text: " + ex.Message);
            }
        }

        // Maps logical button kinds to cached reflected fields.
        private static FieldInfo ResolveButtonField(AppendDistrictButtonKind buttonKind)
        {
            switch (buttonKind)
            {
                case AppendDistrictButtonKind.ServiceOwner:
                    return CityServiceOwnerButtonField;
                case AppendDistrictButtonKind.PublicTransportOwner:
                    return PublicTransportOwnerButtonField;
                case AppendDistrictButtonKind.VehicleTarget:
                    return VehicleTargetButtonField;
                case AppendDistrictButtonKind.HumanWorkplace:
                    return HumanWorkplaceButtonField;
                case AppendDistrictButtonKind.HumanTarget:
                    return HumanTargetButtonField;
                case AppendDistrictButtonKind.CitizenResidence:
                    return CitizenResidenceButtonField;
                case AppendDistrictButtonKind.TouristHotel:
                    return TouristHotelButtonField;
                default:
                    return null;
            }
        }

        // Searches a type hierarchy for an instance field.
        private static FieldInfo GetFieldRecursive(Type type, string fieldName)
        {
            while (HasValue(type))
            {
                FieldInfo fieldInfo = type.GetField(fieldName, AnyInstance);
                if (HasValue(fieldInfo))
                    return fieldInfo;

                type = type.BaseType;
            }

            return null;
        }

        // Searches a type hierarchy for an instance method with matching signature.
        private static MethodInfo GetMethodRecursive(Type type, string methodName, Type[] parameterTypes)
        {
            while (HasValue(type))
            {
                MethodInfo methodInfo = type.GetMethod(methodName, AnyInstance, null, parameterTypes, null);
                if (HasValue(methodInfo))
                    return methodInfo;

                type = type.BaseType;
            }

            return null;
        }

        // Null-safe helper used by reflection code paths.
        private static bool HasValue(object value)
        {
            return !object.ReferenceEquals(value, null);
        }
    }
}
