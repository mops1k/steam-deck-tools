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
            Visible = MicrophoneManager.IsMicrophoneConnected(),
            CurrentValue = () =>
            {
                if (!MicrophoneManager.IsMicrophoneConnected())
                {
                    return "Off";
                }
                
                return MicrophoneManager.GetMicrophoneMute() ? "On" : "Off";
            },
            ApplyValue = (selected) =>
            {
                if (!MicrophoneManager.IsMicrophoneConnected())
                {
                    return "Off";
                }
                
                MicrophoneManager.SetMicrophoneMute(selected.ToString() == "On");

                return MicrophoneManager.GetMicrophoneMute() ? "On" : "Off";
            }
        };
    }
}
