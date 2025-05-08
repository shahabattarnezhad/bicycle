using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using System.Net;

namespace Presentation.Controllers.Base;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Returns a successful response with data
    /// </summary>
    protected IActionResult Success<T>(T data, string message = "Operation successful", int? totalCount = null)
    {
        var response = new ApiResponse<T>(data, message, totalCount);
        return Ok(response);
    }

    /// <summary>
    /// Returns a created response with data and a 201 status code
    /// </summary>
    protected IActionResult CreatedResponse<T>(string routeName, object routeValues, T data, string message = "Resource created successfully")
    {
        var response = new ApiResponse<T>(data, message);
        return CreatedAtRoute(routeName, routeValues, response);
    }

    /// <summary>
    /// Returns an error response with any status code
    /// </summary>
    protected IActionResult Error(string message = "An error occurred", HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string>? errors = null)
    {
        var response = new ApiResponse<string>(message, errors);
        return StatusCode((int)statusCode, response);
    }

    /// <summary>
    /// Returns a succssful response with no data
    /// </summary>
    protected IActionResult SuccessMessage(string message = "Operation successful")
    {
        var response = new ApiResponse<string>(null, message);
        return Ok(response);
    }

    protected IActionResult InvalidModelResponse()
    {
        var errors = ModelState
            .Where(x => x.Value?.Errors?.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage))
            .ToList();

        return BadRequest(new ApiResponse<string>("Validation failed", errors));
    }
}
