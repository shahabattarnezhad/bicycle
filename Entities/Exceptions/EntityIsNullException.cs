using Entities.Exceptions.Base;

namespace Entities.Exceptions;

public sealed class EntityIsNullException : BadRequestException
{
    public EntityIsNullException(string message) : base(message)
    {
    }
}
