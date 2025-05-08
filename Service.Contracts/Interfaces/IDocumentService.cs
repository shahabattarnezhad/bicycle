using Shared.DTOs.Document;
using Shared.DTOs.Station;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IDocumentService
{
    Task<ApiResponse<IEnumerable<DocumentDto>>> GetAllAsync(DocumentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<DocumentDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<DocumentDto>> CreateAsync(DocumentForCreationDto entityForCreation, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> UpdateAsync(Guid entityId, DocumentForVerificationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default);
}
