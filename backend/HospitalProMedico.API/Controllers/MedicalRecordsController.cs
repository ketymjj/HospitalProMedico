using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/medical-records")]
[Authorize]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordsController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<MedicalRecordDto>>> GetAll([FromQuery] PaginationParams pagination, [FromQuery] int? patientId)
    {
        return Ok(await _medicalRecordService.GetAllAsync(pagination, patientId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicalRecordDto>> GetById(int id)
    {
        return Ok(await _medicalRecordService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,MEDICO")]
    public async Task<ActionResult<MedicalRecordDto>> Create(CreateMedicalRecordDto dto)
    {
        var result = await _medicalRecordService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "ADMIN,MEDICO")]
    public async Task<ActionResult<MedicalRecordDto>> Update(int id, UpdateMedicalRecordDto dto)
    {
        return Ok(await _medicalRecordService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _medicalRecordService.DeleteAsync(id);
        return NoContent();
    }
}
