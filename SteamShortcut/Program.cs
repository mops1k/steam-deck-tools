using CommonHelpers;
using SteamShortcut.VdfHelper;
namespace SteamShortcut;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        if (args.Length != 1)
        {
            Log.Error("Not enough or too many args!");
            return;
        }

        SteamManager manager = new SteamManager();
        if (!manager.InitialisePaths())
        {
            MessageBox.Show("Steam user not defined!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error("Could not initialise paths.");
            return;
        }

        Log.Info($"Accessing '{manager.VdfPath}'");

        if (!manager.Read())
        {
            var errorMessage = "Failed to read shortcuts.vdf. Does a shortcuts.vdf file exist? Try to add a non-steam game to steam first";
            Log.Error(errorMessage);
            MessageBox.Show(errorMessage, "Steam Shortcut Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            return;
        }

        if (!manager.AddExe(args[0], Path.GetFileName(args[0])))
        {
            Log.Error("Failed to add executable");
            MessageBox.Show("Failed to add executable", "Steam Shortcut Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            return;
        }

        if (!manager.Write())
        {
            Log.Error("Failed to write to the shortcuts.vdf file");
            MessageBox.Show("Failed to write to the shortcuts.vdf file", "Steam Shortcut Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            return;
        }
        
        Log.Info("Shorcut Added!");
        switch (SteamConfiguration.IsRunning)
        {
            case true:
                MessageBox.Show("Finished! Steam will be restarted to view the new shortcut", "Steam Shortcut Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Info("Restart Steam...");
                RestartSteam();
                break;
            case false:
                MessageBox.Show("Finished! You should start Steam to view the new shortcut", "Steam Shortcut Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
        }

        Thread.Sleep(5000);
    }

    private static void RestartSteam()
    {
        if (!SteamConfiguration.IsRunning)
        {
            return;
        }

        var isBigPictureMode = false;
        if (SteamConfiguration.IsRunning)
        {
            isBigPictureMode = SteamConfiguration.IsBigPictureMode.GetValueOrDefault(false);
            if (!SteamConfiguration.ShutdownSteam())
            {
                SteamConfiguration.KillSteam();
            }

            SteamConfiguration.WaitForSteamClose(5000);
            Log.Info("Steam Shutdown Success!");
            Thread.Sleep(5000);
        }

        if (SteamConfiguration.StartSteam(isBigPictureMode))
        {
            Log.Info("Steam Started!");
            return;
        }

        Log.Info("Steam starting failed!");
    }
}
