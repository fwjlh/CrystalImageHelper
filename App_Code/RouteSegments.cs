namespace CrystalImageHelper
{
    /// <summary>
    /// Immutable value holding the two URL segments used by the proxy.
    /// </summary>
    public sealed class RouteSegments
    {
        public string Key { get; private set; }
        public string FileName { get; private set; }

        public RouteSegments(string key, string fileName)
        {
            Key = key;
            FileName = fileName;
        }
    }
}
