using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AppointmentDto>>> GetAll(
        [FromQuery] PaginationParams pagination,
        [FromQuery] DateTime? date,
        [FromQuery] int? doctorId,
        [FromQuery] int? patientId,
        [FromQuery] string? status)
    {
        return Ok(await _appointmentService.GetAllAsync(pagination, date, doctorId, patientId, status));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        return Ok(await _appointmentService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,RECEPCAO")]
    public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
    {
        var result = await _appointmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,RECEPCAO")]
    public async Task<ActionResult<AppointmentDto>> Update(int id, UpdateAppointmentDto dto)
    {
        return Ok(await _appointmentService.UpdateAsync(id, dto));
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "ADMIN,RECEPCAO,MEDICO")]
    public async Task<ActionResult<AppointmentDto>> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
    {
        return Ok(await _appointmentService.UpdateStatusAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN,RECEPCAO")]
    public async Task<IActionResult> Delete(int id)
    {
        await _appointmentService.DeleteAsync(id);
        return NoContent();
    }
}
