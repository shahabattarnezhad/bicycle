using Entities.Models;
using Shared.Enums;

namespace Repository.Extensions;

public static class ReservationRepositoryExtensions
{
    public static IQueryable<Reservation> FilterByStatus(this IQueryable<Reservation> query, ReservationStatus? status)
    {
        if (status is null)
            return query;

        return query.Where(r => r.ReservationStatus == status);
    }

    public static IQueryable<Reservation> FilterByUserId(this IQueryable<Reservation> query, string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return query;

        return query.Where(r => r.UserId == userId);
    }

    public static IQueryable<Reservation> FilterByBicycleId(this IQueryable<Reservation> query, Guid? bicycleId)
    {
        if (bicycleId is null || bicycleId == Guid.Empty)
            return query;

        return query.Where(r => r.BicycleId == bicycleId);
    }

    public static IQueryable<Reservation> FilterByStartDateRange(this IQueryable<Reservation> query, DateTime? from, DateTime? to)
    {
        if (from is not null)
            query = query.Where(r => r.StartTime >= from.Value);

        if (to is not null)
            query = query.Where(r => r.StartTime <= to.Value);

        return query;
    }

    //public static IQueryable<Reservation> Search(this IQueryable<Reservation> query, string? searchTerm)
    //{
    //    if (string.IsNullOrWhiteSpace(searchTerm))
    //        return query;

    //    var lowerTerm = searchTerm.Trim().ToLower();

    //    return query.Where(r =>
    //        r.Bicycle != null && r.Bicycle.Title.ToLower().Contains(lowerTerm) ||
    //        r.AppUser != null && r.AppUser.UserName.ToLower().Contains(lowerTerm));
    //}

    public static IQueryable<Reservation> Sort(this IQueryable<Reservation> query, string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "starttime" => query.OrderByDescending(r => r.StartTime),
            "endtime" => query.OrderByDescending(r => r.EndTime),
            "totalcost" => query.OrderByDescending(r => r.TotalCost),
            "status" => query.OrderBy(r => r.ReservationStatus),
            _ => query.OrderByDescending(r => r.StartTime)
        };
    }
}
