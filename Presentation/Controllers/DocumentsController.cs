using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Document;
using Shared.DTOs.Station;
using Shared.Requests;
using Shared.Responses;

namespace Presentation.Controllers;

[Route("api/documents")]
public class DocumentsController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public DocumentsController(IServiceManager service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] DocumentParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.DocumentService.GetAllAsync(parameters, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet("{id:guid}", Name = "GetDocumentById")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.DocumentService.GetAsync(id, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

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

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DocumentForVerificationDto documentDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        await _service.DocumentService.UpdateAsync(id, documentDto, trackChanges: true, cancellationToken);
        return NoContent();
    }
}
