using Shared.Requests.Base;

namespace Shared.Requests;

public class BicycleParameters : RequestParameters
{
    public string? SearchTerm { get; set; }

    public string? OrderBy { get; set; } = "CreatedAt";
}
