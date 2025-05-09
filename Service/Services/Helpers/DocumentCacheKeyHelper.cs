using Shared.Requests;

namespace Shared.Helpers;

public static class DocumentCacheKeyHelper
{
    public const string DocumentPrefix = "documents";

    public static string GenerateDocumentKey(Guid documentId) =>
        $"{DocumentPrefix}_{documentId}";

    public static string GenerateDocumentListKey(DocumentParameters parameters)
    {
        return $"{DocumentPrefix}_page_{parameters.PageNumber}_size_{parameters.PageSize}_search_{parameters.SearchTerm}_status_{parameters.Status}_order_{parameters.OrderBy}";
    }
}
