using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<Doctor> _doctors;

    public DoctorService(IRepository<Doctor> doctors)
    {
        _doctors = doctors;
    }

    public async Task<PagedResult<DoctorDto>> GetAllAsync(PaginationParams pagination, string? specialty = null, string? status = null)
    {
        var query = _doctors.Query().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(d => d.Name.ToLower().Contains(term) || d.Crm.Contains(term) || d.Email.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(specialty) && Enum.TryParse<Specialty>(specialty, true, out var spec))
        {
            query = query.Where(d => d.Specialty == spec);
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RegisterStatus>(status, true, out var st))
        {
            query = query.Where(d => d.Status == st);
        }

        query = query.OrderBy(d => d.Name);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<DoctorDto>
        {
            Items = result.Items.Select(d => d.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<DoctorDetailDto> GetByIdAsync(int id)
    {
        var doctor = await _doctors.Query()
            .Include(d => d.Appointments)
            .Include(d => d.Surgeries)
            .Include(d => d.Hospitalizations)
            .FirstOrDefaultAsync(d => d.Id == id)
            ?? throw new NotFoundException("Médico", id);

        var dto = doctor.ToDto();

        return new DoctorDetailDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Crm = dto.Crm,
            Specialty = dto.Specialty,
            Cpf = dto.Cpf,
            Phone = dto.Phone,
            Email = dto.Email,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt,
            AppointmentsCount = doctor.Appointments.Count,
            SurgeriesCount = doctor.Surgeries.Count,
            HospitalizationsCount = doctor.Hospitalizations.Count
        };
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
    {
        if (await _doctors.ExistsAsync(d => d.Crm == dto.Crm))
        {
            throw new BusinessRuleException("Já existe um médico cadastrado com este CRM.");
        }

        var doctor = new Doctor
        {
            Name = dto.Name,
            Crm = dto.Crm,
            Specialty = dto.Specialty,
            Cpf = dto.Cpf,
            Phone = dto.Phone,
            Email = dto.Email,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Status = RegisterStatus.ATIVO
        };

        await _doctors.AddAsync(doctor);
        await _doctors.SaveChangesAsync();

        return doctor.ToDto();
    }

    public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
    {
        var doctor = await _doctors.GetByIdAsync(id) ?? throw new NotFoundException("Médico", id);

        if (await _doctors.ExistsAsync(d => d.Id != id && d.Crm == dto.Crm))
        {
            throw new BusinessRuleException("Já existe um médico cadastrado com este CRM.");
        }

        doctor.Name = dto.Name;
        doctor.Crm = dto.Crm;
        doctor.Specialty = dto.Specialty;
        doctor.Cpf = dto.Cpf;
        doctor.Phone = dto.Phone;
        doctor.Email = dto.Email;
        doctor.BirthDate = dto.BirthDate;
        doctor.Address = dto.Address;
        doctor.Status = dto.Status;
        doctor.UpdatedAt = DateTime.UtcNow;

        _doctors.Update(doctor);
        await _doctors.SaveChangesAsync();

        return doctor.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var doctor = await _doctors.GetByIdAsync(id) ?? throw new NotFoundException("Médico", id);
        _doctors.Remove(doctor);
        await _doctors.SaveChangesAsync();
    }
}
