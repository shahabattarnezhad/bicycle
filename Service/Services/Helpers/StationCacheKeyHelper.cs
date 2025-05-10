namespace Shared.Helpers;

public static class StationCacheKeyHelper
{
    public static string StationPrefix => "stations";

    public static string GetPagedKey(string prefix, int page, int size)
        => $"{prefix}_page_{page}_size_{size}";

    public static string GetEntityKey(string prefix, Guid id)
        => $"{prefix}_{id}";

    public static string GetCustomKey(string prefix, string customSuffix)
        => $"{prefix}_{customSuffix}";

    
    public static (string key, string prefix) GetEntityCache(string prefix, Guid id)
        => ($"{prefix}_{id}", prefix);
}
