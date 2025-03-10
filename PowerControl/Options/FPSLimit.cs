using CommonHelpers;
using PowerControl.Helpers;

namespace PowerControl.Options
{
    public static class FPSLimit
    {
        public static Menu.MenuItemWithOptions Instance = new Menu.MenuItemWithOptions()
        {
            Name = "FPS Limit",
            PersistentKey = "FPSLimit",
            ApplyDelay = 500,
            ResetValue = () => { return "Off"; },
            OptionsValues = delegate ()
            {
                int refreshRate = DisplayResolutionController.GetRefreshRate();
        		string[] availableLimits = new string[(refreshRate / 5) + 1];
        		for (int i = 0; i < refreshRate/5; i++) {
        			availableLimits[i] = string.Format("{0}", (i + 1) * 5);
        		}
                availableLimits[^1] = string.Format("{0}", refreshRate + 3);

                // dissalow to use fps limits lower than 15
                string[] allowedLimits = Array.FindAll(availableLimits, val => val != null && int.Parse(val) >= 15);
                Array.Resize(ref allowedLimits, allowedLimits.Length + 1);
                allowedLimits[^2] = "half";
                allowedLimits[^2] = "quarter";
                
                return allowedLimits;
            },
            CurrentValue = delegate ()
            {
                try
                {
                    if (!Dependencies.EnsureRTSS(null))
                        return "?";

                    RTSS.LoadProfile();
                    if (RTSS.GetProfileProperty("FramerateLimit", out int framerate))
                        return (framerate == 0) ? "Off" : framerate.ToString();
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

                    int framerate = 0;
                    int refreshRate = DisplayResolutionController.GetRefreshRate();
                    switch (selected)
                    {
                        case "Off":
                            framerate = 0;
                            break;
                        case "half":
                            framerate = refreshRate / 2;
                            break;
                        case "quarter":
                            framerate = refreshRate / 4;
                            break;
                        default:
                            framerate = int.Parse(selected);
                            break;
                    }

                    RTSS.LoadProfile();
                    if (!RTSS.SetProfileProperty("FramerateLimit", framerate))
                        return null;
                    if (!RTSS.GetProfileProperty("FramerateLimit", out framerate))
                        return null;
                    RTSS.SaveProfile();
                    RTSS.UpdateProfiles();
                    
                    switch (selected)
                    {
                        case "Off":
                            return "Off";
                        case "half":
                            return "half";
                        case "quarter":
                            return "quarter";
                        default:
                            return framerate.ToString();
                    }
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
                    if (!Dependencies.EnsureRTSS(null))
                        return;

                    var refreshRate = DisplayResolutionController.GetRefreshRate();
                    if (refreshRate <= 0)
                        return;

                    RTSS.LoadProfile();
                    RTSS.GetProfileProperty("FramerateLimit", out int fpsLimit);
                    if (fpsLimit <= 0)
                        return;

                    if (fpsLimit < 15) {
                        fpsLimit = 15;
                    }
                    
                    int convertFpsLimit(int limit, int rr) {
                        if (limit <= rr + 3 && limit > rr) {
                            return rr + 3;
                        }

                        if (limit > rr + 3) {
                            return rr + 3;
                        }
                        
                        var dig = rr / limit;
                        switch (dig)
                        {
                            case 2:
                                return rr / 2;
                            case 4:
                                return rr / 4;
                        }

                        var leftOver = limit % 5;
                        if (leftOver == 0) {
                            return limit;
                        }

                        if (leftOver >= 3) {
                            return limit + (5 - leftOver);
                        }

                        return limit - leftOver;
                    }
                    
                    RTSS.SetProfileProperty("FramerateLimit", convertFpsLimit(fpsLimit, refreshRate));
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
