using CommonHelpers;

namespace SteamController.Managers
{
    public sealed class SteamConfigsManager : Manager
    {
        static readonly Dictionary<String, byte[]> lockedSteamControllerFiles = new Dictionary<string, byte[]>
        {
            // Use existing defaults in BasicUI and BigPicture
            // { "controller_base/basicui_neptune.vdf", Resources.basicui_neptune },
            // { "controller_base/bigpicture_neptune.vdf", Resources.bigpicture_neptune },
            { "controller_base/desktop_neptune.vdf", Resources.empty_neptune },
            { "controller_base/chord_neptune.vdf", Resources.chord_neptune }
        };
        static readonly Dictionary<String, byte[]> installedSteamControllerFiles = new Dictionary<string, byte[]>
        {
            { "controller_base/templates/controller_neptune_steamcontroller.vdf", Resources.empty_neptune },
        };

        private bool? filesLocked;

        public SteamConfigsManager()
        {
            // always unlock configs when changed
            Settings.Default.SettingChanging += UnlockControllerFiles;
            SetSteamControllerFilesLock(false);
        }

        private bool IsActive
        {
            get
            {
                return Settings.Default.SteamControllerConfigs == Settings.SteamControllerConfigsMode.Overwrite &&
                Settings.Default.EnableSteamDetection == true;
            }
        }

        public override void Dispose()
        {
            SetSteamControllerFilesLock(false);
            Settings.Default.SettingChanging -= UnlockControllerFiles;
        }

        private void UnlockControllerFiles(string key)
        {
            SetSteamControllerFilesLock(false);
        }

        public override void Tick(Context context)
        {
            if (!IsActive)
                return;

            bool running = SteamConfiguration.IsRunning;
            if (running == filesLocked)
                return;

            SetSteamControllerFilesLock(running);
        }

        private void SetSteamControllerFilesLock(bool lockConfigs)
        {
            if (!IsActive)
                return;

            Log.Info("SetSteamControllerFilesLock: {0}", lockConfigs);

            if (lockConfigs)
            {
                foreach (var config in lockedSteamControllerFiles)
                    SteamConfiguration.OverwriteConfigFile(config.Key, config.Value, true);
                foreach (var config in installedSteamControllerFiles)
                    SteamConfiguration.OverwriteConfigFile(config.Key, config.Value, false);
            }
            else
            {
                foreach (var config in lockedSteamControllerFiles)
                    SteamConfiguration.ResetConfigFile(config.Key);
            }
            filesLocked = lockConfigs;
        }
    }
}
