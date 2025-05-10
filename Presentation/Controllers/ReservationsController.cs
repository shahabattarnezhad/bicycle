using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Reservation;
using Shared.Requests;

namespace Presentation.Controllers;

[Route("api/reservations")]
public class ReservationsController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public ReservationsController(IServiceManager service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] ReservationParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.ReservationService.GetAllAsync(parameters, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet("{id:guid}", Name = "GetReservationById")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.ReservationService.GetAsync(id, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationForCreationDto reservationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response =
            await _service.ReservationService.CreateAsync(reservationDto, cancellationToken);

        return CreatedResponse("GetReservationById", new { id = response.Data?.Id }, response.Data, response.Message);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ReturnBicycle(Guid id, [FromBody] ReservationForReturnDto returnDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        if (id != returnDto.ReservationId)
            return Error("Id in the route does not match the id in the body");

        var response = 
            await _service.ReservationService.ReturnBikeAsync(returnDto, cancellationToken);

        return Success(response.Data, response.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.ReservationService.DeleteAsync(id, trackChanges: true, cancellationToken);

        return Success(response.Data, response.Message);
    }
}
