namespace CommonHelpers.OSDService
{
    public static class OSDOverlayListFacade
    {
        public static string[] List()
        {
            var source = new OSDFileManager();
            var entries = source.GetEntries().ToArray();
            var enumNames = Enum.GetNames<OverlayMode>();
            var values = new string[enumNames.Length + entries.Length];
            enumNames.CopyTo(values, 0);
            entries.CopyTo(values, enumNames.Length);

            return values.Distinct().ToArray();
        }
    }
}
