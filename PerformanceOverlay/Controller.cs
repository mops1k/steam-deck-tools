using CommonHelpers;
using CommonHelpers.OSDService;
using ExternalHelpers;
using RTSSSharedMemoryNET;
using System.ComponentModel;

namespace PerformanceOverlay
{
    internal class Controller : IDisposable
    {
        private const string Title = "Performance Overlay";
        private readonly static string TitleWithVersion = Title + " v" + Application.ProductVersion;

        private readonly Container _components = new Container();
        private OSD? _osd;
        private ToolStripMenuItem _showItem;
        private readonly NotifyIcon _notifyIcon;
        private System.Windows.Forms.Timer _osdTimer;
        private readonly Sensors _sensors = new Sensors();
        private readonly StartupManager _startupManager = new StartupManager(
            Title,
            "Starts Performance Overlay on Windows startup."
        );

        private readonly SharedData<OverlayModeSetting> _sharedData = SharedData<OverlayModeSetting>.CreateNew();
        private readonly ContextMenuStrip _contextMenu;

        static Controller()
        {
            Dependencies.ValidateRTSSSharedMemoryNET(TitleWithVersion);
        }

        public Controller()
        {
            Instance.OnUninstall += () => _startupManager.Startup = false;;
            Instance.UninstallTrigger();

            _contextMenu = new ContextMenuStrip(_components);
            BuildContextMenu();

            _notifyIcon = new NotifyIcon(_components);
            _notifyIcon.Icon = WindowsDarkMode.IsDarkModeEnabled ? Resources.poll_light : Resources.poll;
            _notifyIcon.Text = TitleWithVersion;
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenuStrip = _contextMenu;

            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        private void BuildContextMenu()
        {
            _contextMenu.Items.Clear();

            SharedData_Update();
            Instance.Open(TitleWithVersion, Settings.Default.EnableKernelDrivers, "Global\\PerformanceOverlay");
            Instance.RunUpdater(TitleWithVersion);

            if (Instance.WantsRunOnStartup)
                _startupManager.Startup = true;
            var notRunningRtssItem = _contextMenu.Items.Add("&RTSS is not running");
            notRunningRtssItem.Enabled = false;
            _contextMenu.Opening += delegate { notRunningRtssItem.Visible = Dependencies.EnsureRTSS(null) && !OSDHelpers.IsLoaded; };

            _showItem = new ToolStripMenuItem("&Show OSD");
            _showItem.Click += ShowItem_Click;
            _showItem.Checked = Settings.Default.ShowOSD;
            _contextMenu.Items.Add(_showItem);
            _contextMenu.Items.Add(new ToolStripSeparator());

            var modesMenuItem = new ToolStripMenuItem("&Overlays");
            modesMenuItem.DropDownItems.AddRange(GetModeItems(OSDOverlayListFacade.List()).ToArray());
            modesMenuItem.DropDownOpening += delegate
            {
                modesMenuItem.DropDownItems.Clear();
                modesMenuItem.DropDownItems.AddRange(GetModeItems(OSDOverlayListFacade.List()).ToArray());
            };
            _contextMenu.Items.Add(modesMenuItem);
            UpdateContextItems(_contextMenu);

            _contextMenu.Items.Add(new ToolStripSeparator());

            var kernelDriversItem = new ToolStripMenuItem("Use &Kernel Drivers");
            kernelDriversItem.Click += delegate { SetKernelDrivers(!Instance.UseKernelDrivers); };
            _contextMenu.Opening += delegate { kernelDriversItem.Checked = Instance.UseKernelDrivers; };
            _contextMenu.Items.Add(kernelDriversItem);

            _contextMenu.Items.Add(new ToolStripSeparator());

            if (_startupManager.IsAvailable)
            {
                var startupItem = new ToolStripMenuItem("Run On Startup");
                startupItem.Checked = _startupManager.Startup;
                startupItem.Click += delegate
                {
                    _startupManager.Startup = !_startupManager.Startup;
                    startupItem.Checked = _startupManager.Startup;
                };
                _contextMenu.Items.Add(startupItem);
            }

            var missingRtssItem = _contextMenu.Items.Add("&Install missing RTSS");
            missingRtssItem.Click += delegate { Dependencies.OpenLink(Dependencies.RTSSURL); };
            _contextMenu.Opening += delegate { missingRtssItem.Visible = !Dependencies.EnsureRTSS(null); };

            var checkForUpdatesItem = _contextMenu.Items.Add("&Check for Updates");
            checkForUpdatesItem.Click += delegate { Instance.RunUpdater(TitleWithVersion, true); };

            var helpItem = _contextMenu.Items.Add("&Help");
            helpItem.Click += delegate { Dependencies.OpenLink(Dependencies.SDTURL); };

            _contextMenu.Items.Add(new ToolStripSeparator());

            var exitItem = _contextMenu.Items.Add("&Exit");
            exitItem.Click += ExitItem_Click;

            _osdTimer = new System.Windows.Forms.Timer(_components);
            _osdTimer.Tick += OsdTimer_Tick;
            _osdTimer.Interval = 250;
            _osdTimer.Enabled = true;

            if (Settings.Default.ShowOSDShortcut != "")
            {
                GlobalHotKey.RegisterHotKey(Settings.Default.ShowOSDShortcut, () =>
                {
                    Settings.Default.ShowOSD = !Settings.Default.ShowOSD;

                    UpdateContextItems(_contextMenu);
                });
            }

            if (Settings.Default.CycleOSDShortcut != "")
            {
                GlobalHotKey.RegisterHotKey(Settings.Default.CycleOSDShortcut, () =>
                {
                    var values = OSDOverlayListFacade.List();

                    var index = Array.IndexOf(values, Settings.Default.OSDMode);
                    Settings.Default.OSDMode = values[(index + 1) % values.Length];
                    Settings.Default.ShowOSD = true;

                    UpdateContextItems(_contextMenu);
                });
            }
        }

        private List<ToolStripItem> GetModeItems(string[] modes)
        {
            var toolStripMenuItems = new List<ToolStripItem>();

            foreach (var mode in modes)
            {
                var modeItem = new ToolStripMenuItem(mode);
                modeItem.Tag = mode;
                modeItem.Click += delegate
                {
                    Settings.Default.OSDMode = mode;
                };
                modeItem.Checked = Settings.Default.OSDMode == mode;
                toolStripMenuItems.Add(modeItem);
            }

            return toolStripMenuItems;
        }

        private void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            if (e.Mode == Microsoft.Win32.PowerModes.Resume)
            {
                Instance.HardwareComputer.Reset();
            }
        }

        private void UpdateContextItems(ContextMenuStrip contextMenuStrip)
        {
            foreach (ToolStripItem item in contextMenuStrip.Items)
            {
                if (item is not ToolStripMenuItem menuItem)
                {
                    continue;
                }

                if (menuItem.Tag is OverlayMode mode)
                {
                    menuItem.Tag = mode.ToString();
                }

                menuItem.Checked = (string)(menuItem.Tag ?? "") == Settings.Default.OSDMode;
            }

            _showItem.Checked = Settings.Default.ShowOSD;
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            ((NotifyIcon)sender).ContextMenuStrip.Show(Control.MousePosition);
        }

        private void ShowItem_Click(object? sender, EventArgs e)
        {
            Settings.Default.ShowOSD = !Settings.Default.ShowOSD;
            UpdateContextItems(new System.Windows.Forms.ContextMenuStrip(_components));
        }

        private bool AckAntiCheat()
        {
            return AntiCheatSettings.Default.AckAntiCheat(
                TitleWithVersion,
                "Usage of OSD Kernel Drivers might trigger anti-cheat protection in some games.",
                "Ensure that you set it to DISABLED when playing games with ANTI-CHEAT PROTECTION."
            );
        }

        private void SetKernelDrivers(bool value)
        {
            if (value && AckAntiCheat())
            {
                Instance.UseKernelDrivers = true;
                Settings.Default.EnableKernelDrivers = true;
            }
            else
            {
                Instance.UseKernelDrivers = false;
                Settings.Default.EnableKernelDrivers = false;
            }
        }

        private void SharedData_Update()
        {
            if (_sharedData.GetValue(out var value))
            {
                if (OSDOverlayListFacade.List().Contains(value.Desired))
                {
                    Settings.Default.OSDMode = value.Desired;
                    Settings.Default.ShowOSD = true;
                    UpdateContextItems(_contextMenu);
                }

                if (Enum.IsDefined(value.DesiredEnabled))
                {
                    Settings.Default.ShowOSD = value.DesiredEnabled == OverlayEnabled.Yes;
                    UpdateContextItems(_contextMenu);
                }

                if (Enum.IsDefined(value.DesiredKernelDriversLoaded))
                {
                    SetKernelDrivers(value.DesiredKernelDriversLoaded == KernelDriversLoaded.Yes);
                    UpdateContextItems(_contextMenu);
                }
            }

            _sharedData.SetValue(new OverlayModeSetting
            {
                Current = Settings.Default.OSDMode,
                CurrentEnabled = Settings.Default.ShowOSD ? OverlayEnabled.Yes : OverlayEnabled.No,
                KernelDriversLoaded = Instance.UseKernelDrivers ? KernelDriversLoaded.Yes : KernelDriversLoaded.No
            });
        }

        private void OsdTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _osdTimer.Enabled = false;
                SharedData_Update();
            }
            finally
            {
                _osdTimer.Enabled = true;
            }

            try
            {
                _notifyIcon.Text = TitleWithVersion + @". RTSS Version: " + OSD.Version;
                _notifyIcon.Icon = WindowsDarkMode.IsDarkModeEnabled ? Resources.poll_light : Resources.poll;
            }
            catch
            {
                _notifyIcon.Text = TitleWithVersion + @". RTSS Not Available.";
                _notifyIcon.Icon = Resources.poll_red;
                Notification.ShowNotification("RTSS Not Available. Please run RTSS.");
                OsdReset();
                return;
            }

            if (!Settings.Default.ShowOSD)
            {
                _osdTimer.Interval = 1000;
                OsdReset();
                return;
            }

            _osdTimer.Interval = 250;

            _sensors.Update();

            var osdMode = Settings.Default.OSDMode;

            // If Power Control is visible use temporarily full OSD
            if (Settings.Default.EnableFullOnPowerControl)
            {
                if (SharedData<PowerControlSetting>.GetExistingValue(out var value) && value.Current == PowerControlVisible.Yes)
                    osdMode = OverlayMode.Full.ToString();
            }

            // first try to load osd overlay by file, second by enum
            var osdOverlay = Overlays.GetOsd(osdMode, _sensors)
                ?? Overlays.GetOsd(
                    Enum.TryParse(osdMode, out OverlayMode mode) ? mode : OverlayMode.Full,
                    _sensors
                );

            try
            {
                // recreate OSD if not index 0
                if (OSDHelpers.OSDIndex("PerformanceOverlay") != 0)
                    OsdClose();
                _osd ??= new OSD("PerformanceOverlay");

                uint offset = 0;
                OsdEmbedGraph(ref offset, ref osdOverlay, "[OBJ_FT_SMALL]", -8, -1, 1, 0, 50000.0f, EMBEDDED_OBJECT_GRAPH.FLAG_FRAMETIME);
                OsdEmbedGraph(ref offset, ref osdOverlay, "[OBJ_FT_LARGE]", -32, -2, 1, 0, 50000.0f, EMBEDDED_OBJECT_GRAPH.FLAG_FRAMETIME);

                _osd.Update(osdOverlay);
            }
            catch (SystemException)
            {
            }
        }

        private void OsdReset()
        {
            try
            {
                if (_osd != null)
                    _osd.Update("");
            }
            catch (SystemException)
            {
            }
        }

        private void OsdClose()
        {
            try
            {
                if (_osd != null)
                    _osd.Dispose();
                _osd = null;
            }
            catch (SystemException)
            {
            }
        }

        private void OsdEmbedGraph(ref uint offset, ref string osdOverlay, string name, int dwWidth, int dwHeight, int dwMargin, float fltMin, float fltMax, EMBEDDED_OBJECT_GRAPH dwFlags)
        {
            var size = _osd?.EmbedGraph(offset, [], 0, dwWidth, dwHeight, dwMargin, fltMin, fltMax, dwFlags) ?? 0;
            if (size > 0)
                osdOverlay = osdOverlay.Replace(name, "<OBJ=" + offset.ToString("X") + ">");
            offset += size;
        }

        private void ExitItem_Click(object? sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        public void Dispose()
        {
            _components.Dispose();
            OsdClose();
            using (_sensors) { }
        }
    }
}
