using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/surgeries")]
[Authorize]
public class SurgeriesController : ControllerBase
{
    private readonly ISurgeryService _surgeryService;

    public SurgeriesController(ISurgeryService surgeryService)
    {
        _surgeryService = surgeryService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SurgeryDto>>> GetAll([FromQuery] PaginationParams pagination, [FromQuery] string? status)
    {
        return Ok(await _surgeryService.GetAllAsync(pagination, status));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SurgeryDto>> GetById(int id)
    {
        return Ok(await _surgeryService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,RECEPCAO,MEDICO")]
    public async Task<ActionResult<SurgeryDto>> Create(CreateSurgeryDto dto)
    {
        var result = await _surgeryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,RECEPCAO,MEDICO")]
    public async Task<ActionResult<SurgeryDto>> Update(int id, UpdateSurgeryDto dto)
    {
        return Ok(await _surgeryService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _surgeryService.DeleteAsync(id);
        return NoContent();
    }
}
