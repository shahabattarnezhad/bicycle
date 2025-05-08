using Shared.Requests.Base;

namespace Shared.Requests;

public class StationParameters : RequestParameters
{
    public string? SearchTerm { get; set; }

    public string? OrderBy { get; set; } = "name";
}
