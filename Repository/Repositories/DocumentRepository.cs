using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Shared.Requests.Base;
using Shared.Requests;
using Repository.Extensions;

namespace Repository.Repositories;

public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Document>> GetAllAsync(DocumentParameters parameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var documents = FindAll(trackChanges)
                                         .FilterByStatus(parameters.Status)
                                         .FilterByUserId(parameters.UserId)
                                         .SearchNotes(parameters.SearchTerm)
                                         .Sort(parameters.OrderBy);


        return await documents.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<Document?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(entityId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public void CreateEntity(Document document) => Create(document);

    public void UpdateEntity(Document document) => Update(document);
}
