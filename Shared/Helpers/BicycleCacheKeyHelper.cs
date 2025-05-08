namespace Shared.Helpers;

public static class BicycleCacheKeyHelper
{
    public static string BicyclePrefix(Guid stationId) => $"bicycles_station_{stationId}";

    public static string BicycleKey(Guid stationId, Guid bicycleId) => $"{BicyclePrefix(stationId)}_bicycle_{bicycleId}";

    public static string BicyclePageKey(Guid stationId, int pageNumber, int pageSize) => $"{BicyclePrefix(stationId)}_page_{pageNumber}_size_{pageSize}";

    public static string ElectricKey(Guid stationId) => $"{BicyclePrefix(stationId)}_electric";

    public static string InactiveKey(Guid stationId) => $"{BicyclePrefix(stationId)}_inactive";
}
