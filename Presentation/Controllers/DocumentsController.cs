using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Document;
using Shared.Responses;

namespace Presentation.Controllers;

[Route("api/documents")]
public class DocumentsController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public DocumentsController(IServiceManager service) => _service = service;

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] DocumentForCreationDto documentForCreation, CancellationToken cancellationToken)
    {
        var result = 
            await _service.DocumentService.CreateAsync(documentForCreation, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return StatusCode(201, result);
    }
}
