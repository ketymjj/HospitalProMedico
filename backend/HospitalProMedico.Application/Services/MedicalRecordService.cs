using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IRepository<MedicalRecord> _records;
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Doctor> _doctors;

    public MedicalRecordService(IRepository<MedicalRecord> records, IRepository<Patient> patients, IRepository<Doctor> doctors)
    {
        _records = records;
        _patients = patients;
        _doctors = doctors;
    }

    public async Task<PagedResult<MedicalRecordDto>> GetAllAsync(PaginationParams pagination, int? patientId = null)
    {
        var query = _records.Query().Include(r => r.Patient).Include(r => r.Doctor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(r => r.Patient.Name.ToLower().Contains(term) || r.Diagnosis.ToLower().Contains(term));
        }

        if (patientId.HasValue)
        {
            query = query.Where(r => r.PatientId == patientId);
        }

        query = query.OrderByDescending(r => r.Date);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<MedicalRecordDto>
        {
            Items = result.Items.Select(r => r.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<MedicalRecordDto> GetByIdAsync(int id)
    {
        var record = await _records.Query().Include(r => r.Patient).Include(r => r.Doctor)
            .FirstOrDefaultAsync(r => r.Id == id) ?? throw new NotFoundException("Prontuário", id);
        return record.ToDto();
    }

    public async Task<MedicalRecordDto> CreateAsync(CreateMedicalRecordDto dto)
    {
        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        if (!await _doctors.ExistsAsync(d => d.Id == dto.DoctorId))
        {
            throw new BusinessRuleException("Médico informado não existe.");
        }

        var record = new MedicalRecord
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Date = dto.Date,
            ChiefComplaint = dto.ChiefComplaint,
            History = dto.History,
            Diagnosis = dto.Diagnosis,
            Prescription = dto.Prescription,
            Exams = dto.Exams,
            Notes = dto.Notes
        };

        await _records.AddAsync(record);
        await _records.SaveChangesAsync();

        return await GetByIdAsync(record.Id);
    }

    public async Task<MedicalRecordDto> UpdateAsync(int id, UpdateMedicalRecordDto dto)
    {
        var record = await _records.GetByIdAsync(id) ?? throw new NotFoundException("Prontuário", id);

        record.PatientId = dto.PatientId;
        record.DoctorId = dto.DoctorId;
        record.Date = dto.Date;
        record.ChiefComplaint = dto.ChiefComplaint;
        record.History = dto.History;
        record.Diagnosis = dto.Diagnosis;
        record.Prescription = dto.Prescription;
        record.Exams = dto.Exams;
        record.Notes = dto.Notes;
        record.UpdatedAt = DateTime.UtcNow;

        _records.Update(record);
        await _records.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _records.GetByIdAsync(id) ?? throw new NotFoundException("Prontuário", id);
        _records.Remove(record);
        await _records.SaveChangesAsync();
    }
}
