using ExternalHelpers;
using PowerControl.Helpers.WindowsMasterVolume;
namespace PowerControl.Options
{
    public static class Microphone
    {
        public static Menu.MenuItemWithOptions Instance = new Menu.MenuItemWithOptions()
        {
            Name = "Microphone",
            PersistentKey = "Microphone",
            PersistOnCreate = false,
            ApplyDelay = 500,
            Options = { "On", "EnabOffled" },
            ResetValue = () => { return "On"; },
            Visible = WindowsMasterVolume.IsDeviceConnected(MultimediaDeviceType.Microphone),
            CurrentValue = () =>
            {
                if (!WindowsMasterVolume.IsDeviceConnected(MultimediaDeviceType.Microphone))
                {
                    return "Off";
                }
                
                return WindowsMasterVolume.GetMute(MultimediaDeviceType.Microphone) ? "On" : "Off";
            },
            ApplyValue = (selected) =>
            {
                if (!WindowsMasterVolume.IsDeviceConnected(MultimediaDeviceType.Microphone))
                {
                    return "Off";
                }
                
                WindowsMasterVolume.SetMute(MultimediaDeviceType.Microphone,selected.ToString() == "On");

                return WindowsMasterVolume.GetMute(MultimediaDeviceType.Microphone) ? "On" : "Off";
            }
        };
    }
}
