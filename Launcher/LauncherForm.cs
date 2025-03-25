using Launcher.Helper;
namespace Launcher
{
    public partial class LauncherForm : Form
    {
        public LauncherForm()
        {
            InitializeComponent();
        }
        
        private void startButton_Click(object sender, EventArgs e)
        {
            var checkedTools = GetCheckedTools();
            if (checkedTools.Count == 0)
            {
                MessageBox.Show("You must select at least one tool.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            var processHelper = new ProcessHelper();
            var toolManager = new ToolManager(processHelper, checkedTools);
            toolManager.StartTools();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            var checkedTools = GetCheckedTools();
            if (checkedTools.Count == 0)
            {
                MessageBox.Show("You must select at least one tool.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            var processHelper = new ProcessHelper();
            var toolManager = new ToolManager(processHelper, checkedTools);
            toolManager.StopTools();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            var checkedTools = GetCheckedTools();
            if (checkedTools.Count == 0)
            {
                MessageBox.Show("You must select at least one tool.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            var processHelper = new ProcessHelper();
            var toolManager = new ToolManager(processHelper, checkedTools);
            toolManager.RestartTools();
        }

        private void generateShortcuts_Click(object sender, EventArgs e)
        {
            var shortcutGenerator = new ShortcutGenerator();
            shortcutGenerator.GenerateShortcuts();
        }

        private List<string> GetCheckedTools()
        {
            var tools = new List<string>();
            if (fanControl.Checked)
            {
                tools.Add(Tools.FanControl.ToString());
            }

            if (performanceOverlay.Checked)
            {
                tools.Add(Tools.PerformanceOverlay.ToString());
            }

            if (steamController.Checked)
            {
                tools.Add(Tools.SteamController.ToString());
            }

            if (powerControl.Checked)
            {
                tools.Add(Tools.PowerControl.ToString());
            }
            
            return tools;
        }
    }
}

