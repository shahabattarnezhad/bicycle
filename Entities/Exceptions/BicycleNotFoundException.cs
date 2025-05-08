using Entities.Exceptions.Base;

namespace Entities.Exceptions;

public sealed class BicycleNotFoundException : NotFoundException
{
    public BicycleNotFoundException(string serialNumber)
       : base($"The bicycle with serial number: {serialNumber} doesn't exist in the database.")
    {
    }
}
