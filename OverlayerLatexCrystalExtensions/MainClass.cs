using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.Param;

// TODO: Rename this namespace to your mod's name.
namespace OverlayerLatexCrystalExtensions
{
    /// <summary>
    /// The main class for the mod. Call other parts of your code from this
    /// class.
    /// </summary>
    public static class MainClass
    {
        static Settings Settings;

		/// <summary>
		/// Whether the mod is enabled. This is useful to have as a global
		/// property in case other parts of your mod's code needs to see if the
		/// mod is enabled.
		/// </summary>
		public static bool IsEnabled { get; private set; }

        /// <summary>
        /// UMM's logger instance. Use this to write logs to the UMM settings
        /// window under the "Logs" tab.
        /// </summary>
        public static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        private static Harmony harmony;

        /// <summary>
        /// Perform any initial setup with the mod here.
        /// </summary>
        /// <param name="modEntry">UMM's mod entry for the mod.</param>
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;


            // Add hooks to UMM event methods
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
        }

        /// <summary>
        /// Handler for toggling the mod on/off.
        /// </summary>
        /// <param name="modEntry">UMM's mod entry for the mod.</param>
        /// <param name="value">
        /// <c>true</c> if the mod is being toggled on, <c>false</c> if the mod
        /// is being toggled off.
        /// </param>
        /// <returns><c>true</c></returns>
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if (value)
            {
                StartMod(modEntry);
            }
            else
            {
                StopMod(modEntry);
            }
            return true;
        }

        /// <summary>
        /// Start the mod up. You can create Unity GameObjects, patch methods,
        /// etc.
        /// </summary>
        /// <param name="modEntry">UMM's mod entry for the mod.</param>
        private static void StartMod(UnityModManager.ModEntry modEntry)
        {
            Settings = ModSettings.Load<Settings>(modEntry);
            Regex.CacheSize = 4096;

			// Patch everything in this assembly
			harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log("StartMod");
            //Logger.Log(Assembly.GetExecutingAssembly().FullName);

            //Overlayer.Core.TagManager.Load(typeof(OverlayerAdditionalTags));
        }

        /// <summary>
        /// Stop the mod by cleaning up anything that you created in
        /// <see cref="StartMod(UnityModManager.ModEntry)"/>.
        /// </summary>
        /// <param name="modEntry">UMM's mod entry for the mod.</param>
        private static void StopMod(UnityModManager.ModEntry modEntry)
        {
            // Unpatch everything
            harmony.UnpatchAll(modEntry.Info.Id);
        }


        public static void OnGUI(ModEntry modEntry)
        {
            Settings.Draw();
        }
    }
}
