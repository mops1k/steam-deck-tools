namespace CommonHelpers.OSDService
{
    public static class OSDOverlayListFacade
    {
        public static string[] List()
        {
            var source = new OSDFileManager();
            var entries = source.GetEntries();
            var enumNames = Enum.GetNames<OverlayMode>();
            var values = new string[enumNames.Length + entries.Count];
            enumNames.CopyTo(values, 0);
            entries.Keys.CopyTo(values, enumNames.Length);

            return values.Distinct().ToArray();
        }
    }
}
