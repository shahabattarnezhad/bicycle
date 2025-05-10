using Shared.Consts;

namespace Service.Services.Helpers;

public static class PaymentCacheKeyHelper
{
    public static string PaymentPrefix => $"{CacheKeyPrefixes.Payment}";

    public static string GetEntityKey(Guid paymentId) =>
        $"{PaymentPrefix}_entity_{paymentId}";

    public static string GetPagedKey(int pageNumber, int pageSize, string? userId = null) =>
        string.IsNullOrWhiteSpace(userId)
            ? $"{PaymentPrefix}_page_{pageNumber}_size_{pageSize}"
            : $"{PaymentPrefix}_user_{userId}_page_{pageNumber}_size_{pageSize}";
}
