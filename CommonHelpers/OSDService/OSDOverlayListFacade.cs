namespace CommonHelpers.OSDService
{
    public static class OSDOverlayListFacade
    {
        private static OSDFileManager _source = new OSDFileManager();
        
        public static string[] List()
        {
            var entries = _source.GetEntries().ToArray();
            var enumNames = Enum.GetNames<OverlayMode>();
            var values = new string[enumNames.Length + entries.Length];
            enumNames.CopyTo(values, 0);
            entries.CopyTo(values, enumNames.Length);

            return values.Distinct().ToArray();
        }

        public static void Watch(Action action)
        {
            _source.Watch(action);//.WaitForChanged(WatcherChangeTypes.Created | WatcherChangeTypes.Deleted | WatcherChangeTypes.Renamed);
        }
    }
}
