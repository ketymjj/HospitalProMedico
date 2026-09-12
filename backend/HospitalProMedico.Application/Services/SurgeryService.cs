using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class SurgeryService : ISurgeryService
{
    private readonly IRepository<Surgery> _surgeries;
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Doctor> _doctors;

    public SurgeryService(IRepository<Surgery> surgeries, IRepository<Patient> patients, IRepository<Doctor> doctors)
    {
        _surgeries = surgeries;
        _patients = patients;
        _doctors = doctors;
    }

    public async Task<PagedResult<SurgeryDto>> GetAllAsync(PaginationParams pagination, string? status = null)
    {
        var query = _surgeries.Query().Include(s => s.Patient).Include(s => s.Doctor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(s => s.Patient.Name.ToLower().Contains(term) || s.Doctor.Name.ToLower().Contains(term) || s.Type.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<SurgeryStatus>(status, true, out var st))
        {
            query = query.Where(s => s.Status == st);
        }

        query = query.OrderByDescending(s => s.Date);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<SurgeryDto>
        {
            Items = result.Items.Select(s => s.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<SurgeryDto> GetByIdAsync(int id)
    {
        var surgery = await _surgeries.Query().Include(s => s.Patient).Include(s => s.Doctor)
            .FirstOrDefaultAsync(s => s.Id == id) ?? throw new NotFoundException("Cirurgia", id);
        return surgery.ToDto();
    }

    public async Task<SurgeryDto> CreateAsync(CreateSurgeryDto dto)
    {
        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        if (!await _doctors.ExistsAsync(d => d.Id == dto.DoctorId))
        {
            throw new BusinessRuleException("Médico informado não existe.");
        }

        var surgery = new Surgery
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Type = dto.Type,
            Date = dto.Date,
            Time = dto.Time,
            Room = dto.Room,
            EstimatedDurationMinutes = dto.EstimatedDurationMinutes,
            Notes = dto.Notes,
            Status = SurgeryStatus.AGENDADA
        };

        await _surgeries.AddAsync(surgery);
        await _surgeries.SaveChangesAsync();

        return await GetByIdAsync(surgery.Id);
    }

    public async Task<SurgeryDto> UpdateAsync(int id, UpdateSurgeryDto dto)
    {
        var surgery = await _surgeries.GetByIdAsync(id) ?? throw new NotFoundException("Cirurgia", id);

        surgery.PatientId = dto.PatientId;
        surgery.DoctorId = dto.DoctorId;
        surgery.Type = dto.Type;
        surgery.Date = dto.Date;
        surgery.Time = dto.Time;
        surgery.Room = dto.Room;
        surgery.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
        surgery.Notes = dto.Notes;
        surgery.Status = dto.Status;
        surgery.UpdatedAt = DateTime.UtcNow;

        _surgeries.Update(surgery);
        await _surgeries.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var surgery = await _surgeries.GetByIdAsync(id) ?? throw new NotFoundException("Cirurgia", id);
        _surgeries.Remove(surgery);
        await _surgeries.SaveChangesAsync();
    }
}
