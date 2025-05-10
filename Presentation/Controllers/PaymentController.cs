using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Shared.DTOs.Payment;
using Shared.Requests;

namespace Presentation.Controllers;

[Route("api/payments")]
public class PaymentController : ApiControllerBase
{
    private readonly IServiceManager _service;
    public PaymentController(IServiceManager service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] PaymentParameters parameters, CancellationToken cancellationToken)
    {
        var response =
            await _service.PaymentService.GetAllAsync(parameters, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [HttpGet("{id:guid}", Name = "GetPaymentById")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.PaymentService.GetAsync(id, trackChanges: false, cancellationToken);

        return Success(response.Data, response.Message, response.TotalCount);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentForCreationDto paymentDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var response =
            await _service.PaymentService.CreateAsync(paymentDto, cancellationToken);

        return CreatedResponse("GetPaymentById", new { id = response.Data?.Id }, response.Data, response.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var response =
            await _service.PaymentService.DeleteAsync(id, trackChanges: true, cancellationToken);

        return Success(response.Data, response.Message);
    }
}
