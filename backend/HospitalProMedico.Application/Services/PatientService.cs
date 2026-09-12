using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<HealthPlan> _healthPlans;

    public PatientService(IRepository<Patient> patients, IRepository<HealthPlan> healthPlans)
    {
        _patients = patients;
        _healthPlans = healthPlans;
    }

    public async Task<PagedResult<PatientListItemDto>> GetAllAsync(PaginationParams pagination, string? status = null, int? healthPlanId = null)
    {
        var query = _patients.Query().Include(p => p.HealthPlan).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term)
                || p.Cpf.Contains(term)
                || p.Email.ToLower().Contains(term)
                || p.Phone.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RegisterStatus>(status, true, out var st))
        {
            query = query.Where(p => p.Status == st);
        }

        if (healthPlanId.HasValue)
        {
            query = query.Where(p => p.HealthPlanId == healthPlanId);
        }

        query = pagination.SortBy?.ToLower() switch
        {
            "name" => pagination.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "birthdate" => pagination.SortDescending ? query.OrderByDescending(p => p.BirthDate) : query.OrderBy(p => p.BirthDate),
            "createdat" => pagination.SortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => query.OrderBy(p => p.Name)
        };

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<PatientListItemDto>
        {
            Items = result.Items.Select(p => p.ToListItemDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<PatientDto> GetByIdAsync(int id)
    {
        var patient = await _patients.Query().Include(p => p.HealthPlan).FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException("Paciente", id);
        return patient.ToDto();
    }

    public async Task<PatientDetailDto> GetDetailByIdAsync(int id)
    {
        var patient = await _patients.Query()
            .Include(p => p.HealthPlan)
            .Include(p => p.Appointments).ThenInclude(a => a.Doctor)
            .Include(p => p.Hospitalizations).ThenInclude(h => h.Doctor)
            .Include(p => p.Surgeries).ThenInclude(s => s.Doctor)
            .Include(p => p.MedicalRecords).ThenInclude(m => m.Doctor)
            .Include(p => p.Payments)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException("Paciente", id);

        var dto = patient.ToDto();

        return new PatientDetailDto
        {
            Id = dto.Id,
            Name = dto.Name,
            Cpf = dto.Cpf,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            SusCardNumber = dto.SusCardNumber,
            HealthPlanId = dto.HealthPlanId,
            HealthPlanName = dto.HealthPlanName,
            HealthPlanCardNumber = dto.HealthPlanCardNumber,
            BloodType = dto.BloodType,
            Allergies = dto.Allergies,
            Notes = dto.Notes,
            CreatedAt = dto.CreatedAt,
            Status = dto.Status,
            Appointments = patient.Appointments.OrderByDescending(a => a.Date).Select(a => a.ToDto()).ToList(),
            Hospitalizations = patient.Hospitalizations.OrderByDescending(h => h.AdmissionDate).Select(h => h.ToDto()).ToList(),
            Surgeries = patient.Surgeries.OrderByDescending(s => s.Date).Select(s => s.ToDto()).ToList(),
            MedicalRecords = patient.MedicalRecords.OrderByDescending(m => m.Date).Select(m => m.ToDto()).ToList(),
            Payments = patient.Payments.OrderByDescending(p => p.DueDate).Select(p => p.ToDto()).ToList()
        };
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        if (await _patients.ExistsAsync(p => p.Cpf == dto.Cpf))
        {
            throw new BusinessRuleException("Já existe um paciente cadastrado com este CPF.");
        }

        if (dto.HealthPlanId.HasValue && !await _healthPlans.ExistsAsync(h => h.Id == dto.HealthPlanId))
        {
            throw new BusinessRuleException("Plano de saúde informado não existe.");
        }

        var patient = new Patient
        {
            Name = dto.Name,
            Cpf = dto.Cpf,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            SusCardNumber = dto.SusCardNumber,
            HealthPlanId = dto.HealthPlanId,
            HealthPlanCardNumber = dto.HealthPlanCardNumber,
            BloodType = dto.BloodType,
            Allergies = dto.Allergies,
            Notes = dto.Notes,
            Status = RegisterStatus.ATIVO
        };

        await _patients.AddAsync(patient);
        await _patients.SaveChangesAsync();

        return await GetByIdAsync(patient.Id);
    }

    public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
    {
        var patient = await _patients.GetByIdAsync(id) ?? throw new NotFoundException("Paciente", id);

        if (await _patients.ExistsAsync(p => p.Id != id && p.Cpf == dto.Cpf))
        {
            throw new BusinessRuleException("Já existe um paciente cadastrado com este CPF.");
        }

        if (dto.HealthPlanId.HasValue && !await _healthPlans.ExistsAsync(h => h.Id == dto.HealthPlanId))
        {
            throw new BusinessRuleException("Plano de saúde informado não existe.");
        }

        patient.Name = dto.Name;
        patient.Cpf = dto.Cpf;
        patient.BirthDate = dto.BirthDate;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;
        patient.City = dto.City;
        patient.State = dto.State;
        patient.ZipCode = dto.ZipCode;
        patient.SusCardNumber = dto.SusCardNumber;
        patient.HealthPlanId = dto.HealthPlanId;
        patient.HealthPlanCardNumber = dto.HealthPlanCardNumber;
        patient.BloodType = dto.BloodType;
        patient.Allergies = dto.Allergies;
        patient.Notes = dto.Notes;
        patient.Status = dto.Status;
        patient.UpdatedAt = DateTime.UtcNow;

        _patients.Update(patient);
        await _patients.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _patients.GetByIdAsync(id) ?? throw new NotFoundException("Paciente", id);
        _patients.Remove(patient);
        await _patients.SaveChangesAsync();
    }
}
