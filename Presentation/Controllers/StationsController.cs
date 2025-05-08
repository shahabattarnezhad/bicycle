using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Station;
using Shared.Requests;

namespace Presentation.Controllers;

[Route("api/stations")]
public class StationsController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public StationsController(IServiceManager service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] StationParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.StationService.GetAllAsync(parameters, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet("{id:guid}", Name = "GetStationById")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.StationService.GetAsync(id, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StationForCreationDto stationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response =
            await _service.StationService.CreateAsync(stationDto, cancellationToken);

        return CreatedResponse("GetStationById", new { id = response.Data?.Id }, response.Data, response.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StationForUpdationDto stationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        await _service.StationService.UpdateAsync(id, stationDto, trackChanges: true, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.StationService.DeleteAsync(id, trackChanges: true, cancellationToken);

        return Success(response.Data, response.Message);
    }
}
