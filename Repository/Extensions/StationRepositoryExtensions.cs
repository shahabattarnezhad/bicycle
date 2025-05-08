using Entities.Models;

namespace Repository.Extensions;

public static class StationRepositoryExtensions
{
    public static IQueryable<Station> Filter(this IQueryable<Station> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        return query.Where(s => s.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
    }

    public static IQueryable<Station> Sort(this IQueryable<Station> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderBy(s => s.Name);

        return orderBy.ToLower() switch
        {
            "name" => query.OrderBy(s => s.Name),
            "capacity" => query.OrderBy(s => s.Capacity),
            _ => query.OrderBy(s => s.Name)
        };
    }
}
