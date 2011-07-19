namespace WhereIsMakkah.Lang
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources
        {
            get
            {
                return _localizedResources;
            }
        }
    }
}
