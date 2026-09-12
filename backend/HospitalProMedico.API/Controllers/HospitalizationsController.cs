using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/hospitalizations")]
[Authorize]
public class HospitalizationsController : ControllerBase
{
    private readonly IHospitalizationService _hospitalizationService;

    public HospitalizationsController(IHospitalizationService hospitalizationService)
    {
        _hospitalizationService = hospitalizationService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<HospitalizationDto>>> GetAll([FromQuery] PaginationParams pagination, [FromQuery] string? status)
    {
        return Ok(await _hospitalizationService.GetAllAsync(pagination, status));
    }

    [HttpGet("current")]
    public async Task<ActionResult<List<HospitalizationDto>>> GetCurrent()
    {
        return Ok(await _hospitalizationService.GetCurrentlyHospitalizedAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HospitalizationDto>> GetById(int id)
    {
        return Ok(await _hospitalizationService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,RECEPCAO,MEDICO")]
    public async Task<ActionResult<HospitalizationDto>> Create(CreateHospitalizationDto dto)
    {
        var result = await _hospitalizationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,RECEPCAO,MEDICO")]
    public async Task<ActionResult<HospitalizationDto>> Update(int id, UpdateHospitalizationDto dto)
    {
        return Ok(await _hospitalizationService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _hospitalizationService.DeleteAsync(id);
        return NoContent();
    }
}
