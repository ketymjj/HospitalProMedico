using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/health-plans")]
[Authorize]
public class HealthPlansController : ControllerBase
{
    private readonly IHealthPlanService _healthPlanService;

    public HealthPlansController(IHealthPlanService healthPlanService)
    {
        _healthPlanService = healthPlanService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<HealthPlanDto>>> GetAll([FromQuery] PaginationParams pagination)
    {
        return Ok(await _healthPlanService.GetAllAsync(pagination));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HealthPlanDto>> GetById(int id)
    {
        return Ok(await _healthPlanService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,FINANCEIRO")]
    public async Task<ActionResult<HealthPlanDto>> Create(CreateHealthPlanDto dto)
    {
        var result = await _healthPlanService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,FINANCEIRO")]
    public async Task<ActionResult<HealthPlanDto>> Update(int id, UpdateHealthPlanDto dto)
    {
        return Ok(await _healthPlanService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _healthPlanService.DeleteAsync(id);
        return NoContent();
    }
}
