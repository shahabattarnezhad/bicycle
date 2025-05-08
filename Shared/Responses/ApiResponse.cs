namespace Shared.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; }

    public T? Data { get; init; }

    public int? TotalCount { get; init; }

    public List<string>? Errors { get; init; }

    public ApiResponse() { }

    public ApiResponse(T? data, string message = "", int? totalCount = null)
    {
        Success = true;
        Message = message;
        Data = data;
        TotalCount = totalCount;
    }

    public ApiResponse(string message, List<string>? errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }

    public ApiResponse(bool success, string message, T? data = default, int? totalCount = null, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        TotalCount = totalCount;
        Errors = errors;
    }
}
