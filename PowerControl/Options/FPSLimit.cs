using CommonHelpers;
using PowerControl.Helpers;

namespace PowerControl.Options
{
    public static class FpsLimit
    {
        public static readonly Menu.MenuItemWithOptions Instance = new Menu.MenuItemWithOptions()
        {
            Name = "FPS Limit",
            PersistentKey = "FPSLimit",
            ApplyDelay = 500,
            ResetValue = () => "Off",
            OptionsValues = delegate()
            {
                var refreshRate = DisplayResolutionController.GetRefreshRate();
                var availableLimits = new string[refreshRate / 5 + 1];
                for (var i = 0; i < refreshRate / 5; i++)
                {
                    availableLimits[i] = $"{(i + 1) * 5}";
                }

                availableLimits[refreshRate / 5 + 1] = (refreshRate + 3).ToString();

                // dissalow to use fps limits lower than 15
                var allowedLimits = Array.FindAll(availableLimits, val => val != null && int.Parse(val) >= 15);

                return allowedLimits;
            },
            CurrentValue = delegate()
            {
                try
                {
                    if (!Dependencies.EnsureRTSS(null))
                        return "?";

                    RTSS.LoadProfile();
                    if (RTSS.GetProfileProperty("FramerateLimit", out int framerate))
                    {
                        return (framerate == 0) ? "Off" : framerate.ToString();
                    }

                    return null;
                }
                catch (Exception e)
                {
#if DEBUG
                    CommonHelpers.Log.TraceException("RTSS", e);
#endif
                    return "?";
                }
            },
            ApplyValue = (selected) =>
            {
                try
                {
                    if (!Dependencies.EnsureRTSS(Controller.TitleWithVersion))
                        return null;

                    var framerate = 0;
                    if (selected != "Off")
                    {
                        framerate = int.Parse(selected);
                    }

                    RTSS.LoadProfile();
                    if (!RTSS.SetProfileProperty("FramerateLimit", framerate))
                        return null;
                    if (!RTSS.GetProfileProperty("FramerateLimit", out framerate))
                        return null;
                    RTSS.SaveProfile();
                    RTSS.UpdateProfiles();

                    return (framerate == 0) ? "Off" : framerate.ToString();
                }
                catch (Exception e)
                {
                    CommonHelpers.Log.TraceException("RTSS", e);
                }

                return null;
            },
            ImpactedBy = (option, was, isNow) =>
            {
                if (Instance is null)
                    return;

                try
                {
                    if (!Dependencies.EnsureRTSS())
                        return;

                    var refreshRate = DisplayResolutionController.GetRefreshRate();
                    if (refreshRate <= 0)
                        return;

                    RTSS.LoadProfile();
                    RTSS.GetProfileProperty("FramerateLimit", out int fpsLimit);

                    if (fpsLimit <= 0)
                        return;

                    if (fpsLimit > refreshRate + 3)
                    {
                        fpsLimit = refreshRate + 3;
                    }

                    if (fpsLimit < refreshRate + 3 && fpsLimit >= refreshRate)
                    {
                        fpsLimit = refreshRate;
                    }

                    if (fpsLimit < 15)
                    {
                        fpsLimit = 15;
                    }

                    int ConvertFpsLimitDividedByFive(int limit, int refreshRateBase)
                    {
                        if (limit == refreshRateBase + 3)
                        {
                            return limit;
                        }

                        var leftOver = limit % 5;
                        return leftOver switch
                        {
                            0 => limit,
                            >= 3 => limit + (5 - leftOver),
                            _ => limit - leftOver
                        };
                    }

                    RTSS.SetProfileProperty("FramerateLimit", ConvertFpsLimitDividedByFive(fpsLimit, refreshRate));
                    RTSS.SaveProfile();
                    RTSS.UpdateProfiles();
                }
                catch (Exception e)
                {
#if DEBUG
                    CommonHelpers.Log.TraceException("RTSS", e);
#endif
                }
            }
        };
    }
}