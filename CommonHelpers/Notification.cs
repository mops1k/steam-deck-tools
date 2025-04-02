using Microsoft.Toolkit.Uwp.Notifications;
namespace CommonHelpers
{
    public static class Notification
    {
        public static void ShowNotification(string message)
        {
            new ToastContentBuilder()
                .AddText("SteamDeckTools: " + Instance.ApplicationName)
                .AddText(message)
                .SetToastDuration(ToastDuration.Short)
                .Show();
        }
    }
}
