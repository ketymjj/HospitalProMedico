using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class HospitalizationService : IHospitalizationService
{
    private readonly IRepository<Hospitalization> _hospitalizations;
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Doctor> _doctors;

    public HospitalizationService(IRepository<Hospitalization> hospitalizations, IRepository<Patient> patients, IRepository<Doctor> doctors)
    {
        _hospitalizations = hospitalizations;
        _patients = patients;
        _doctors = doctors;
    }

    public async Task<PagedResult<HospitalizationDto>> GetAllAsync(PaginationParams pagination, string? status = null)
    {
        var query = _hospitalizations.Query().Include(h => h.Patient).Include(h => h.Doctor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(h => h.Patient.Name.ToLower().Contains(term) || h.Doctor.Name.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<HospitalizationStatus>(status, true, out var st))
        {
            query = query.Where(h => h.Status == st);
        }

        query = query.OrderByDescending(h => h.AdmissionDate);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<HospitalizationDto>
        {
            Items = result.Items.Select(h => h.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<HospitalizationDto> GetByIdAsync(int id)
    {
        var hospitalization = await _hospitalizations.Query().Include(h => h.Patient).Include(h => h.Doctor)
            .FirstOrDefaultAsync(h => h.Id == id) ?? throw new NotFoundException("Internação", id);
        return hospitalization.ToDto();
    }

    public async Task<List<HospitalizationDto>> GetCurrentlyHospitalizedAsync()
    {
        var list = await _hospitalizations.Query().Include(h => h.Patient).Include(h => h.Doctor)
            .Where(h => h.Status == HospitalizationStatus.INTERNADO)
            .OrderBy(h => h.AdmissionDate)
            .ToListAsync();

        return list.Select(h => h.ToDto()).ToList();
    }

    public async Task<HospitalizationDto> CreateAsync(CreateHospitalizationDto dto)
    {
        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        if (!await _doctors.ExistsAsync(d => d.Id == dto.DoctorId))
        {
            throw new BusinessRuleException("Médico informado não existe.");
        }

        var hospitalization = new Hospitalization
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AdmissionDate = dto.AdmissionDate,
            ExpectedDischargeDate = dto.ExpectedDischargeDate,
            Room = dto.Room,
            Bed = dto.Bed,
            Type = dto.Type,
            Diagnosis = dto.Diagnosis,
            Reason = dto.Reason,
            Notes = dto.Notes,
            Status = HospitalizationStatus.INTERNADO
        };

        await _hospitalizations.AddAsync(hospitalization);
        await _hospitalizations.SaveChangesAsync();

        return await GetByIdAsync(hospitalization.Id);
    }

    public async Task<HospitalizationDto> UpdateAsync(int id, UpdateHospitalizationDto dto)
    {
        var hospitalization = await _hospitalizations.GetByIdAsync(id) ?? throw new NotFoundException("Internação", id);

        hospitalization.PatientId = dto.PatientId;
        hospitalization.DoctorId = dto.DoctorId;
        hospitalization.AdmissionDate = dto.AdmissionDate;
        hospitalization.ExpectedDischargeDate = dto.ExpectedDischargeDate;
        hospitalization.DischargeDate = dto.DischargeDate;
        hospitalization.Room = dto.Room;
        hospitalization.Bed = dto.Bed;
        hospitalization.Type = dto.Type;
        hospitalization.Diagnosis = dto.Diagnosis;
        hospitalization.Reason = dto.Reason;
        hospitalization.Notes = dto.Notes;
        hospitalization.Status = dto.Status;
        hospitalization.UpdatedAt = DateTime.UtcNow;

        if (dto.Status == HospitalizationStatus.ALTA && hospitalization.DischargeDate is null)
        {
            hospitalization.DischargeDate = DateTime.UtcNow;
        }

        _hospitalizations.Update(hospitalization);
        await _hospitalizations.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var hospitalization = await _hospitalizations.GetByIdAsync(id) ?? throw new NotFoundException("Internação", id);
        _hospitalizations.Remove(hospitalization);
        await _hospitalizations.SaveChangesAsync();
    }
}
