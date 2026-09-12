using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PaymentDto>>> GetAll([FromQuery] PaginationParams pagination, [FromQuery] string? status)
    {
        return Ok(await _paymentService.GetAllAsync(pagination, status));
    }

    [HttpGet("summary")]
    public async Task<ActionResult<FinancialSummaryDto>> GetSummary()
    {
        return Ok(await _paymentService.GetFinancialSummaryAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        return Ok(await _paymentService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,FINANCEIRO,RECEPCAO")]
    public async Task<ActionResult<PaymentDto>> Create(CreatePaymentDto dto)
    {
        var result = await _paymentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,FINANCEIRO")]
    public async Task<ActionResult<PaymentDto>> Update(int id, UpdatePaymentDto dto)
    {
        return Ok(await _paymentService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN,FINANCEIRO")]
    public async Task<IActionResult> Delete(int id)
    {
        await _paymentService.DeleteAsync(id);
        return NoContent();
    }
}
