using System.ComponentModel;

namespace SteamController
{
    [Category("1. Settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal sealed partial class Settings : CommonHelpers.BaseSettings
    {
        public static readonly Settings Default = new Settings();

        public Settings() : base("Settings")
        {
        }

        [Browsable(false)]
        public bool? EnableSteamDetection
        {
            get { return Get<bool?>("EnableSteamDetection", null); }
            set { Set("EnableSteamDetection", value); }
        }

        [Description("Keep X360 controller connected always - it is strongly advised to disable this option. Might be required by some games that do not like disonnecting controller. Will disable beep notifications.")]
        public bool KeepX360AlwaysConnected
        {
            get { return Get<bool>("KeepX360AlwaysConnected", false); }
            set { Set("KeepX360AlwaysConnected", value); }
        }

        [Description("Enable DS4 support. If disabled the DS4 will be hidden.")]
        public bool EnableDS4Support
        {
            get { return Get<bool>("EnableDS4Support", true); }
            set { Set("EnableDS4Support", value); }
        }

        [Description("If current foreground process uses overlay, treat it as a game.")]
        public bool DetectRTSSForeground
        {
            get { return Get<bool>("DetectRTSSForeground", false); }
            set { Set("DetectRTSSForeground", value); }
        }

        [Description("Default profile used when going back to Desktop mode")]
        [Browsable(true)]
        [TypeConverter(typeof(ProfilesSettings.Helpers.ProfileStringConverter))]
        public string DefaultProfile
        {
            get { return Get<string>("DefaultProfile", "Desktop"); }
            set { Set("DefaultProfile", value); }
        }

        public enum ScrollMode : int
        {
            DownScrollUp = -1,
            DownScrollDown = 1
        }

        [Description("Scroll direction for right pad and joystick.")]
        [Browsable(true)]
        public ScrollMode ScrollDirection
        {
            get { return Get<ScrollMode>("ScrollDirection", ScrollMode.DownScrollDown); }
            set { Set("ScrollDirection", value); }
        }

        public enum SteamControllerConfigsMode
        {
            DoNotTouch,
            Overwrite
        }

        [Browsable(true)]
        [Description("This does replace Steam configuration for controllers to prevent double inputs. " +
            "Might require going to Steam > Settings > Controller > Desktop to apply " +
            "'SteamController provided empty configuration'.")]
        public SteamControllerConfigsMode SteamControllerConfigs
        {
            get { return Get<SteamControllerConfigsMode>("SteamControllerConfigs", SteamControllerConfigsMode.Overwrite); }
            set { Set("SteamControllerConfigs", value); }
        }

        public enum KeyboardStyles
        {
            DoNotShow,
            WindowsTouch,
            CTRL_WIN_O
        }

        [Browsable(true)]
        [Description("Show Touch Keyboard or CTRL+WIN+O")]
        public KeyboardStyles KeyboardStyle
        {
            get { return Get<KeyboardStyles>("KeyboardStyle", KeyboardStyles.WindowsTouch); }
            set { Set("KeyboardStyle", value); }
        }

        [Browsable(false)]
        public short DesktopJoystickDeadzone
        {
            get { return 5000; }
        }

        [Browsable(true)]
        [Description("Right touchpad mouse movement sensitivity")]
        public short RightTouchpadMouseSensitivity
        {
            get { return Get<short>("RightTouchpadMouseSensitivity", 150); }
            set { Set("RightTouchpadMouseSensitivity", value); }
        }

        [Browsable(true)]
        [Description("Left touchpad mouse wheel sensitivity")]
        public short LeftTouchpadMouseWheelSensitivity
        {
            get { return Get<short>("LeftTouchpadMouseWheelSensitivity", 4); }
            set { Set("LeftTouchpadMouseWheelSensitivity", value); }
        }

        [Browsable(true)]
        [Description("Right thumbstick mouse movement sensitivity")]
        public short RightThumbStickMouseSensitivity
        {
            get { return Get<short>("RightThumbStickMouseSensitivity", 1200); }
            set { Set("RightThumbStickMouseSensitivity", value); }
        }

        [Browsable(true)]
        [Description("Left thumbstick mouse wheel sensitivity")]
        public short LeftThumbStickMouseWheelSensitivity
        {
            get { return Get<short>("LeftThumbStickMouseWheelSensitivity", 20); }
            set { Set("LeftThumbStickMouseWheelSensitivity", value); }
        }

        [Browsable(true)]
        [Description("Deadzone for Left stick. Enter a number between 0 and 32767. If this number is too small you may experience drift. 5000 or smaller is recommended.")]
        public short LeftJoystickDeadzone
        {
            get { return Get<short>("LeftJoystickDeadZone", 5000); }
            set { Set("LeftJoystickDeadZone", value); }
        }

        [Browsable(true)]
        [Description("Deadzone for Right stick. Enter a number between 0 and 32767. If this number is too small you may experience drift. 5000 or smaller is recommended.")]
        public short RightJoystickDeadzone
        {
            get { return Get<short>("RightJoystickDeadZone", 5000); }
            set { Set("RightJoystickDeadZone", value); }
        }

        public override string ToString()
        {
            return "";
        }
    }
}
