using System.ComponentModel;

namespace SteamController
{
    [Category("1. Settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal sealed partial class LizardSettings() : CommonHelpers.BaseSettings("LizardSettings")
    {
        public readonly static LizardSettings Default = new LizardSettings();

        [Browsable(true)]
        [Description("Use Lizard Buttons instead of emulated.")]
        public bool LizardButtons { get; set; } = true;

        [Browsable(true)]
        [Description("Use Lizard Mouse instead of emulated.")]
        public bool LizardMouse { get; set; } = true;

        public override string ToString()
        {
            return "";
        }
    }
}
