using Entities.Models;
using Shared.Requests;

namespace Repository.Extensions;

public static class PaymentRepositoryExtensions
{
    public static IQueryable<Payment> Filter(this IQueryable<Payment> query, PaymentParameters parameters)
    {
        if (parameters.FromDate.HasValue)
            query = query.Where(p => p.PaymentDate >= parameters.FromDate.Value);

        if (parameters.ToDate.HasValue)
            query = query.Where(p => p.PaymentDate <= parameters.ToDate.Value);

        if (parameters.MinAmount.HasValue)
            query = query.Where(p => p.Amount >= parameters.MinAmount.Value);

        if (parameters.MaxAmount.HasValue)
            query = query.Where(p => p.Amount <= parameters.MaxAmount.Value);

        if (parameters.PaymentMethod.HasValue)
            query = query.Where(p => p.PaymentMethod == parameters.PaymentMethod.Value);

        if (!string.IsNullOrWhiteSpace(parameters.AppUserId))
            query = query.Where(p => p.AppUserId == parameters.AppUserId);

        if (parameters.ReservationId.HasValue)
            query = query.Where(p => p.ReservationId == parameters.ReservationId.Value);

        return query;
    }

    public static IQueryable<Payment> Sort(this IQueryable<Payment> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query.OrderByDescending(p => p.PaymentDate);

        return orderBy.ToLower() switch
        {
            "amount" => query.OrderByDescending(p => p.Amount),
            "date" => query.OrderByDescending(p => p.PaymentDate),
            _ => query.OrderByDescending(p => p.PaymentDate)
        };
    }
}
