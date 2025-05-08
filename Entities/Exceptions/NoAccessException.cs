using Entities.Exceptions.Base;

namespace Entities.Exceptions;

public sealed class NoAccessException : ForbiddenException
{
    public NoAccessException(string message) : base(message)
    {
    }
}
