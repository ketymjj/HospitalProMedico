using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalProMedico.API.Controllers;

[ApiController]
[Route("api/settings/hospital")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public SettingsController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }

    [HttpGet]
    public async Task<ActionResult<HospitalDto>> Get()
    {
        return Ok(await _hospitalService.GetAsync());
    }

    [HttpPut]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<HospitalDto>> Update(UpdateHospitalDto dto)
    {
        return Ok(await _hospitalService.UpdateAsync(dto));
    }
}
