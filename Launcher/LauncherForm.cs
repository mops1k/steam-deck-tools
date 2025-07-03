using AT.WinForm;
using CommonHelpers;
using Launcher.Helper;
using Timer = System.Windows.Forms.Timer;
namespace Launcher
{
    public partial class LauncherForm : Form
    {
        private readonly ToolTip _toolTip = new ToolTip();
        private static ToolManager ToolManager
        {
            get
            {
                var processHelper = new ProcessHelper();
                return new ToolManager(processHelper, Enum.GetNames<Tools>().ToList());
            }
        }
        
        public LauncherForm()
        {
            InitializeComponent();
            var timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                foreach (var tool in Enum.GetValues<Tools>())
                {
                    switch (tool)
                    {
                        case Tools.FanControl:
                            fanSwitch.Checked = ToolManager.IsToolRunning(tool.ToString());
                            break;
                        case Tools.PerformanceOverlay:
                            performanceSwitch.Checked = ToolManager.IsToolRunning(tool.ToString());
                            break;
                        case Tools.SteamController:
                            controllerSwitch.Checked = ToolManager.IsToolRunning(tool.ToString());
                            break;
                        case Tools.PowerControl:
                            powerSwitch.Checked = ToolManager.IsToolRunning(tool.ToString());
                            break;
                    }
                }
            };
            timer.Start();
        }

        private static void ToggleButtonOnCheckedChanged(object sender, Tools tool)
        {
            var button = (ToggleSwitch)sender;
            if (button.Checked)
            {
                ToolManager.StartTools(tool.ToString());
                button.Checked = true;
                return;
            }

            ToolManager.StopTools(tool.ToString());
            button.Checked = false;
        }
        
        private void fanSwitch_CheckedChanged(object sender, EventArgs e)
        {
            ToggleButtonOnCheckedChanged(sender, Tools.FanControl);
        }
        
        private void performanceSwitch_CheckedChanged(object sender, EventArgs e)
        {
            ToggleButtonOnCheckedChanged(sender, Tools.PerformanceOverlay);
        }

        private void controllerSwitch_CheckedChanged(object sender, EventArgs e)
        {
            ToggleButtonOnCheckedChanged(sender, Tools.SteamController);
        }

        private void powerSwitch_CheckedChanged(object sender, EventArgs e)
        {
            ToggleButtonOnCheckedChanged(sender, Tools.PowerControl);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var processHelper = new ProcessHelper();
            var tools = Enum.GetNames<Tools>();
            var toolManager = new ToolManager(processHelper, tools.ToList());

            toolManager.StartTools();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            var processHelper = new ProcessHelper();
            var tools = Enum.GetNames<Tools>();
            var toolManager = new ToolManager(processHelper, tools.ToList());

            toolManager.StopTools();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            var processHelper = new ProcessHelper();
            var tools = Enum.GetNames<Tools>();
            var toolManager = new ToolManager(processHelper, tools.ToList());

            toolManager.RestartTools();
        }

        private void shortcutButton_Click(object sender, EventArgs e)
        {
            var shortcutGenerator = new ShortcutGenerator();
            shortcutGenerator.GenerateShortcuts();
        }

        private void startButton_MouseHover(object sender, EventArgs e)
        {
            CreateToolTip(sender, "Start all tools");
        }

        private void stopButton_MouseHover(object sender, EventArgs e)
        {
            CreateToolTip(sender, "Stop all tools");
        }

        private void restartButton_MouseHover(object sender, EventArgs e)
        {
            CreateToolTip(sender, "Restart all tools");
        }

        private void restartButton_MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide((Control)sender);
        }

        private void stopButton_MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide((Control)sender);
        }

        private void startButton_MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide((Control)sender);
        }
        
        private void CreateToolTip(object sender, string text)
        {
            _toolTip.IsBalloon = true;
            var control = (Control)sender;
            var y = 0 - control.Height;
            var x = 0;
            _toolTip.Show(text, control, new Point(x, y));
        }
    }
}

