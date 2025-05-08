using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Bicycle;
using Shared.Requests;

namespace Presentation.Controllers;

[Route("api/bicycles")]
public class BicyclesController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public BicyclesController(IServiceManager service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.BicycleService.GetAllAsync(stationId, parameters, false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet]
    [Route("{bicycleId:guid}", Name = "GetBicycleById")]
    public async Task<IActionResult> Get(Guid stationId, Guid bicycleId, CancellationToken cancellationToken)
    {
        var response =
            await _service.BicycleService.GetAsync(stationId, bicycleId, false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid stationId, [FromBody] BicycleForCreationDto bicycleDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response =
            await _service.BicycleService.CreateAsync(stationId, bicycleDto, cancellationToken);

        return CreatedResponse("GetBicycleById", new { stationId, bicycleId = response.Data?.Id }, response.Data, response.Message);
    }

    [HttpPut]
    [Route("{bicycleId:guid}/activate")]
    public async Task<IActionResult> Activate(Guid stationId, Guid bicycleId, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.ActivateBicycleAsync(stationId, bicycleId, cancellationToken);
        return SuccessMessage(response.Message);
    }

    [HttpPut]
    [Route("{bicycleId:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid stationId, Guid bicycleId, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.DeactivateBicycleAsync(stationId, bicycleId, cancellationToken);
        return SuccessMessage(response.Message);
    }

    [HttpDelete]
    [Route("{bicycleId:guid}")]
    public async Task<IActionResult> Delete(Guid stationId, Guid bicycleId, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.DeleteAsync(stationId, bicycleId, false, cancellationToken);
        return SuccessMessage(response.Message);
    }

    [HttpGet]
    [Route("available")]
    public async Task<IActionResult> GetAvailable(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetAvailableAsync(stationId, parameters, false, cancellationToken);
        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet]
    [Route("electric")]
    public async Task<IActionResult> GetElectric(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetElectricBicyclesAsync(stationId, parameters, false, cancellationToken);
        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet]
    [Route("standard")]
    public async Task<IActionResult> GetStandard(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetStandardBicyclesAsync(stationId, parameters, false, cancellationToken);
        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet]
    [Route("inactive")]
    public async Task<IActionResult> GetInactive(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetInActiveAsync(stationId, parameters, false, cancellationToken);
        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet]
    [Route("serial/{serialNumber}")]
    public async Task<IActionResult> GetBySerialNumber(string serialNumber, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetBySerialNumberAsync(serialNumber, false, cancellationToken);
        return Success(response.Data, response.Message);
    }

    [HttpGet]
    [Route("gps")]
    public async Task<IActionResult> GetWithGpsRecords(Guid stationId, [FromQuery] BicycleParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetWithGpsRecordsAsync(stationId, parameters, false, cancellationToken);
        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpPut]
    [Route("{bicycleId:guid}")]
    public async Task<IActionResult> Update(Guid stationId, Guid bicycleId, [FromBody] BicycleForUpdationDto bicycleDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response = await _service.BicycleService.UpdateAsync(stationId, bicycleId, bicycleDto, false, cancellationToken);
        return SuccessMessage(response.Message);
    }

    [HttpGet]
    [Route("{bicycleId:guid}/details")]
    public async Task<IActionResult> GetWithDetails(Guid stationId, Guid bicycleId, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.GetWithDetailsAsync(stationId, bicycleId, false, cancellationToken);
        return Success(response.Data, response.Message);
    }

    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> Count(Guid stationId, CancellationToken cancellationToken)
    {
        var response = await _service.BicycleService.CountAsync(stationId, false, cancellationToken);
        return Success(response.Data, response.Message);
    }
}
