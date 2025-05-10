using Entities.Models;
using Shared.Requests;

namespace Repository.Extensions;

public static class BicycleGpsRepositoryExtensions
{
    public static IQueryable<BicycleGps> Filter(this IQueryable<BicycleGps> query, BicycleGpsParameters parameters)
    {
        if (parameters.BicycleId.HasValue)
            query = query.Where(g => g.BicycleId == parameters.BicycleId.Value);

        if (parameters.FromTimestamp.HasValue)
            query = query.Where(g => g.Timestamp >= parameters.FromTimestamp.Value);

        if (parameters.ToTimestamp.HasValue)
            query = query.Where(g => g.Timestamp <= parameters.ToTimestamp.Value);

        return query;
    }

    public static IQueryable<BicycleGps> Sort(this IQueryable<BicycleGps> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderByDescending(g => g.Timestamp);

        return orderBy.ToLower() switch
        {
            "timestamp" => query.OrderByDescending(g => g.Timestamp),
            _ => query.OrderByDescending(g => g.Timestamp)
        };
    }
}
