namespace FamilyAppointmentsMobile.Helpers
{
    /// <summary>
    /// Helper to get Resources from mergedDictonary
    /// </summary>
    public static class ResourceHelper
    {
        public static T GetResource<T>(string key)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            foreach (var mergedDict in mergedDictionaries)
            {
                if (mergedDict.TryGetValue(key, out object resourceValue) && resourceValue is T)
                {
                    return (T)resourceValue;
                }
            }
            return default(T);
        }
    }
}