using Entities.Exceptions.Base;

namespace Entities.Exceptions;

public sealed class EntityNotFoundException : NotFoundException
{
    public EntityNotFoundException(string entityName, Guid id)
        : base($"{entityName} with id: '{id}' was not found.")
    {
    }
}
