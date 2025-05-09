using Entities.Exceptions;
using Shared.Responses;
using System.Net;
using FluentValidation;

namespace Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entity not found");
            await WriteResponseAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            await WriteResponseAsync(context, HttpStatusCode.BadRequest, "Validation error", errors);
        }
        catch (GeneralBadRequestException ex)
        {
            _logger.LogWarning(ex, "Validation failed");
            await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (NoAccessException ex)
        {
            _logger.LogWarning(ex, "Access forbidden");
            await WriteResponseAsync(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");
            await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "Unexpected error occurred");
        }
    }

    private async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message, List<string>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse<string>(message, errors);
        await context.Response.WriteAsJsonAsync(response);
    }
}
