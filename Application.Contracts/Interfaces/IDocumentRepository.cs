using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IDocumentRepository
{
    void CreateEntity(Document document);
}
