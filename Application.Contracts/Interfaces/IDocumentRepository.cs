using Entities.Models;
using Shared.Requests.Base;
using Shared.Requests;

namespace Application.Contracts.Interfaces;

public interface IDocumentRepository
{
    Task<PagedList<Document>> GetAllAsync(DocumentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Document?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    void CreateEntity(Document document);

    void UpdateEntity(Document document);
}
