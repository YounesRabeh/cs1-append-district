using Harmony;
using ICities;

namespace AppendDistrict
{
    public class AppendDistrictMod : IUserMod
    {
        public string Name => "Append District";
        public string Description => AppendDistrictBuildInfo.ModDescription;

        /// <summary>
        /// Builds the mod settings UI shown in the Cities: Skylines options menu.
        /// </summary>
        /// <param name="helper">UI helper provided by the game.</param>
        public void OnSettingsUI(UIHelperBase helper)
        {
            AppendDistrictSettings.BuildSettingsUI(helper);
        }
    }

    public class AppendDistrictLoading : LoadingExtensionBase
    {
        private const string HarmonyId = "com.TheYuyuBoy.appenddistrict";
        private HarmonyInstance _harmony;

        /// <summary>
        /// Applies Harmony patches when a game level is loaded.
        /// </summary>
        /// <param name="mode">The load mode used by the game.</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (_harmony != null)
                return;

            _harmony = HarmonyInstance.Create(HarmonyId);
            _harmony.PatchAll();
            AppendDistrictLog.Info("Lifecycle", "Harmony patches applied. mode=" + mode);
        }

        /// <summary>
        /// Removes Harmony patches when the level is unloading.
        /// </summary>
        public override void OnLevelUnloading()
        {
            if (_harmony == null)
                return;

            _harmony.UnpatchAll(HarmonyId);
            _harmony = null;
            AppendDistrictLog.Info("Lifecycle", "Harmony patches removed");
        }
    }
}
