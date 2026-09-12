using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointments;
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Doctor> _doctors;

    public AppointmentService(IRepository<Appointment> appointments, IRepository<Patient> patients, IRepository<Doctor> doctors)
    {
        _appointments = appointments;
        _patients = patients;
        _doctors = doctors;
    }

    public async Task<PagedResult<AppointmentDto>> GetAllAsync(PaginationParams pagination, DateTime? date = null, int? doctorId = null, int? patientId = null, string? status = null)
    {
        var query = _appointments.Query().Include(a => a.Patient).Include(a => a.Doctor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(a => a.Patient.Name.ToLower().Contains(term) || a.Doctor.Name.ToLower().Contains(term));
        }

        if (date.HasValue)
        {
            query = query.Where(a => a.Date.Date == date.Value.Date);
        }

        if (doctorId.HasValue)
        {
            query = query.Where(a => a.DoctorId == doctorId);
        }

        if (patientId.HasValue)
        {
            query = query.Where(a => a.PatientId == patientId);
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<AppointmentStatus>(status, true, out var st))
        {
            query = query.Where(a => a.Status == st);
        }

        query = query.OrderByDescending(a => a.Date).ThenBy(a => a.Time);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<AppointmentDto>
        {
            Items = result.Items.Select(a => a.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<AppointmentDto> GetByIdAsync(int id)
    {
        var appointment = await _appointments.Query().Include(a => a.Patient).Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id) ?? throw new NotFoundException("Consulta", id);
        return appointment.ToDto();
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        if (!await _doctors.ExistsAsync(d => d.Id == dto.DoctorId))
        {
            throw new BusinessRuleException("Médico informado não existe.");
        }

        var conflict = await _appointments.ExistsAsync(a => a.DoctorId == dto.DoctorId
            && a.Date.Date == dto.Date.Date
            && a.Time == dto.Time
            && a.Status != AppointmentStatus.CANCELADA);

        if (conflict)
        {
            throw new BusinessRuleException("Já existe uma consulta agendada para este médico neste horário.");
        }

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Date = dto.Date,
            Time = dto.Time,
            Type = dto.Type,
            Notes = dto.Notes,
            Status = AppointmentStatus.AGENDADA
        };

        await _appointments.AddAsync(appointment);
        await _appointments.SaveChangesAsync();

        return await GetByIdAsync(appointment.Id);
    }

    public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointments.GetByIdAsync(id) ?? throw new NotFoundException("Consulta", id);

        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        if (!await _doctors.ExistsAsync(d => d.Id == dto.DoctorId))
        {
            throw new BusinessRuleException("Médico informado não existe.");
        }

        appointment.PatientId = dto.PatientId;
        appointment.DoctorId = dto.DoctorId;
        appointment.Date = dto.Date;
        appointment.Time = dto.Time;
        appointment.Type = dto.Type;
        appointment.Notes = dto.Notes;
        appointment.Status = dto.Status;
        appointment.UpdatedAt = DateTime.UtcNow;

        _appointments.Update(appointment);
        await _appointments.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto)
    {
        var appointment = await _appointments.GetByIdAsync(id) ?? throw new NotFoundException("Consulta", id);
        appointment.Status = dto.Status;
        appointment.UpdatedAt = DateTime.UtcNow;

        _appointments.Update(appointment);
        await _appointments.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _appointments.GetByIdAsync(id) ?? throw new NotFoundException("Consulta", id);
        _appointments.Remove(appointment);
        await _appointments.SaveChangesAsync();
    }
}
