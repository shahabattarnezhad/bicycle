using Entities.Models;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions;

public static class BicycleRepositoryExtensions
{
    public static IQueryable<Bicycle> Filter(this IQueryable<Bicycle> bicycles, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return bicycles;

        var normalizedTerm = searchTerm.Trim().ToLower();

        return bicycles.Where(b =>
            b.BicycleStatus.ToString().ToLower().Contains(normalizedTerm));
    }

    public static IQueryable<Bicycle> Sort(this IQueryable<Bicycle> bicycles, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return bicycles.OrderByDescending(b => b.CreatedAt);

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Bicycle).GetProperties();
        var orderQuery = "";

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null) continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQuery += $"{objectProperty.Name} {direction}, ";
        }

        orderQuery = orderQuery.TrimEnd(',', ' ');

        return string.IsNullOrWhiteSpace(orderQuery)
            ? bicycles.OrderByDescending(b => b.CreatedAt)
            : bicycles.OrderBy(orderQuery);
    }
}
