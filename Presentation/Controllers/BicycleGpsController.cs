using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.BicycleGps;
using Shared.Requests;

namespace Presentation.Controllers;

[Route("api/bicyclegps")]
public class BicycleGpsController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public BicycleGpsController(IServiceManager service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] BicycleGpsParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.BicycleGpsService.GetAllAsync(parameters, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet("{id:guid}", Name = "GetBicycleGpsById")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.BicycleGpsService.GetAsync(id, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BicycleGpsForCreationDto bicycleGpsDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response =
            await _service.BicycleGpsService.CreateAsync(bicycleGpsDto, cancellationToken);

        return CreatedResponse("GetBicycleGpsById", new { id = response.Data?.Id }, response.Data, response.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.BicycleGpsService.DeleteAsync(id, trackChanges: true, cancellationToken);

        return Success(response.Data, response.Message);
    }
}
