using Entities.Exceptions.Base;

namespace Entities.Exceptions;

public sealed class GeneralBadRequestException : BadRequestException
{
    public GeneralBadRequestException(string message) : base(message)
    {
    }
}
