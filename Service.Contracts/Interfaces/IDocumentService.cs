using Shared.DTOs.Document;
using Shared.DTOs.Station;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IDocumentService
{
    Task<ApiResponse<DocumentDto>> CreateAsync(DocumentForCreationDto entityForCreation, CancellationToken cancellationToken = default);
}
