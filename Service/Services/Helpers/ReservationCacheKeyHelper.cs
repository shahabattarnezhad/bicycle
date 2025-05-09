using Shared.Requests;

namespace Service.Services.Helpers;

public static class ReservationCacheKeyHelper
{
    public const string ReservationPrefix = "reservations";

    public static string GenerateReservationKey(Guid reservationId) =>
        $"{ReservationPrefix}_{reservationId}";

    public static string GenerateReservationListKey(ReservationParameters parameters)
    {
        return $"{ReservationPrefix}_page_{parameters.PageNumber}_size_{parameters.PageSize}" +
               $"_search_{parameters.SearchTerm}" +
               $"_status_{parameters.Status}" +
               $"_startFrom_{parameters.StartDateFrom:yyyyMMdd}" +
               $"_startTo_{parameters.StartDateTo:yyyyMMdd}" +
               $"_user_{parameters.UserId}" +
               $"_bicycle_{parameters.BicycleId}" +
               $"_order_{parameters.OrderBy}";
    }
}
