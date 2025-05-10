using Shared.Consts;

namespace Service.Services.Helpers;

public static class BicycleGpsCacheKeyHelper
{
    public static string BicycleGpsPrefix => $"{CacheKeyPrefixes.BicycleGps}";

    public static string GetEntityKey(Guid bicycleGpsId) =>
        $"{BicycleGpsPrefix}_entity_{bicycleGpsId}";

    public static string GetPagedKey(int pageNumber, int pageSize, Guid? bicycleId = null) =>
        bicycleId.HasValue
            ? $"{BicycleGpsPrefix}_bicycle_{bicycleId.Value}_page_{pageNumber}_size_{pageSize}"
            : $"{BicycleGpsPrefix}_page_{pageNumber}_size_{pageSize}";
}
