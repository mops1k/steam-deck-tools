using CommonHelpers;
using CommonHelpers.OSDService;

namespace PowerControl.Options
{
    public static class PerformanceOverlay
    {
        public static Menu.MenuItemWithOptions EnabledInstance = new Menu.MenuItemWithOptions()
        {
            Name = "OSD",
            PersistentKey = "PerformanceOverlay",
            ApplyDelay = 500,
            OptionsValues = delegate ()
            {
                return Enum.GetNames<OverlayEnabled>();
            },
            CurrentValue = delegate ()
            {
                if (SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return value.CurrentEnabled.ToString();
                return null;
            },
            ApplyValue = (selected) =>
            {
                if (!SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return null;
                value.DesiredEnabled = Enum.Parse<OverlayEnabled>(selected);
                if (!SharedData<OverlayModeSetting>.SetExistingValue(value))
                    return null;
                return selected;
            }
        };

        public static Menu.MenuItemWithOptions ModeInstance = new Menu.MenuItemWithOptions()
        {
            Name = "OSD Mode",
            PersistentKey = "PerformanceOverlayMode",
            ApplyDelay = 500,
            OptionsValues = delegate()
            {
                return OSDOverlayListFacade.List();
            },
            CurrentValue = delegate ()
            {
                if (SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return value.Current;
                return null;
            },
            ApplyValue = (selected) =>
            {
                if (!SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return null;
                value.Desired = selected;
                if (!SharedData<OverlayModeSetting>.SetExistingValue(value))
                    return null;
                return selected;
            }
        };

        public static Menu.MenuItemWithOptions KernelDriversInstance = new Menu.MenuItemWithOptions()
        {
            Name = "OSD Kernel Drivers",
            ApplyDelay = 500,
            OptionsValues = delegate ()
            {
                return Enum.GetNames<KernelDriversLoaded>();
            },
            CurrentValue = delegate ()
            {
                if (SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return value.KernelDriversLoaded.ToString();
                return null;
            },
            ApplyValue = (selected) =>
            {
                if (!SharedData<OverlayModeSetting>.GetExistingValue(out var value))
                    return null;
                value.DesiredKernelDriversLoaded = Enum.Parse<KernelDriversLoaded>(selected);
                if (!SharedData<OverlayModeSetting>.SetExistingValue(value))
                    return null;
                return selected;
            }
        };
    }
}
