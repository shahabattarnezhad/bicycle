using Entities.Models;
using Shared.Enums;

namespace Repository.Extensions;

public static class DocumentRepositoryExtensions
{
    public static IQueryable<Document> FilterByStatus(this IQueryable<Document> query, DocumentStatus? status)
    {
        if (status is null)
            return query;

        return query.Where(d => d.Status == status);
    }

    public static IQueryable<Document> FilterByUserId(this IQueryable<Document> query, string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return query;

        return query.Where(d => d.AppUserId == userId);
    }

    public static IQueryable<Document> SearchNotes(this IQueryable<Document> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var lowerTerm = searchTerm.Trim().ToLower();
        return query.Where(d => d.Notes != null && d.Notes.ToLower().Contains(lowerTerm));
    }

    public static IQueryable<Document> Sort(this IQueryable<Document> query, string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "uploadedat" => query.OrderByDescending(d => d.UploadedAt),
            "verifiedat" => query.OrderByDescending(d => d.VerifiedAt),
            "status" => query.OrderBy(d => d.Status),
            _ => query.OrderByDescending(d => d.UploadedAt)
        };
    }
}
