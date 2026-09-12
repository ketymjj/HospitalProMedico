using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/patients")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PatientListItemDto>>> GetAll([FromQuery] PaginationParams pagination, [FromQuery] string? status, [FromQuery] int? healthPlanId)
    {
        return Ok(await _patientService.GetAllAsync(pagination, status, healthPlanId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        return Ok(await _patientService.GetByIdAsync(id));
    }

    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<PatientDetailDto>> GetDetails(int id)
    {
        return Ok(await _patientService.GetDetailByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,RECEPCAO")]
    public async Task<ActionResult<PatientDto>> Create(CreatePatientDto dto)
    {
        var result = await _patientService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,RECEPCAO")]
    public async Task<ActionResult<PatientDto>> Update(int id, UpdatePatientDto dto)
    {
        return Ok(await _patientService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }
}
