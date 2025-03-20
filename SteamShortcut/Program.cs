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
        
        Log.Info("Finished! Exiting in 5 seconds. Restart steam to view the new shortcut");
        MessageBox.Show("Finished! Restart steam to view the new shortcut", "Steam Shortcut Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Thread.Sleep(5000);
    }
}
