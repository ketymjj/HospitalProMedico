using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Domain.Entities;

namespace HospitalProMedico.Application.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User u) => new()
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        Role = u.Role,
        IsActive = u.IsActive,
        DoctorId = u.DoctorId,
        CreatedAt = u.CreatedAt
    };

    public static PatientDto ToDto(this Patient p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Cpf = p.Cpf,
        BirthDate = p.BirthDate,
        Gender = p.Gender,
        Phone = p.Phone,
        Email = p.Email,
        Address = p.Address,
        City = p.City,
        State = p.State,
        ZipCode = p.ZipCode,
        SusCardNumber = p.SusCardNumber,
        HealthPlanId = p.HealthPlanId,
        HealthPlanName = p.HealthPlan?.Name,
        HealthPlanCardNumber = p.HealthPlanCardNumber,
        BloodType = p.BloodType,
        Allergies = p.Allergies,
        Notes = p.Notes,
        CreatedAt = p.CreatedAt,
        Status = p.Status
    };

    public static PatientListItemDto ToListItemDto(this Patient p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Cpf = p.Cpf,
        Phone = p.Phone,
        HealthPlanName = p.HealthPlan?.Name,
        BirthDate = p.BirthDate,
        Status = p.Status
    };

    public static DoctorDto ToDto(this Doctor d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Crm = d.Crm,
        Specialty = d.Specialty,
        Cpf = d.Cpf,
        Phone = d.Phone,
        Email = d.Email,
        BirthDate = d.BirthDate,
        Address = d.Address,
        Status = d.Status,
        CreatedAt = d.CreatedAt
    };

    public static AppointmentDto ToDto(this Appointment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        PatientName = a.Patient?.Name ?? string.Empty,
        DoctorId = a.DoctorId,
        DoctorName = a.Doctor?.Name ?? string.Empty,
        Specialty = a.Doctor?.Specialty ?? default,
        Date = a.Date,
        Time = a.Time,
        Type = a.Type,
        Status = a.Status,
        Notes = a.Notes
    };

    public static HospitalizationDto ToDto(this Hospitalization h) => new()
    {
        Id = h.Id,
        PatientId = h.PatientId,
        PatientName = h.Patient?.Name ?? string.Empty,
        DoctorId = h.DoctorId,
        DoctorName = h.Doctor?.Name ?? string.Empty,
        AdmissionDate = h.AdmissionDate,
        ExpectedDischargeDate = h.ExpectedDischargeDate,
        DischargeDate = h.DischargeDate,
        Room = h.Room,
        Bed = h.Bed,
        Type = h.Type,
        Diagnosis = h.Diagnosis,
        Reason = h.Reason,
        Notes = h.Notes,
        Status = h.Status
    };

    public static SurgeryDto ToDto(this Surgery s) => new()
    {
        Id = s.Id,
        PatientId = s.PatientId,
        PatientName = s.Patient?.Name ?? string.Empty,
        DoctorId = s.DoctorId,
        DoctorName = s.Doctor?.Name ?? string.Empty,
        Type = s.Type,
        Date = s.Date,
        Time = s.Time,
        Room = s.Room,
        EstimatedDurationMinutes = s.EstimatedDurationMinutes,
        Status = s.Status,
        Notes = s.Notes
    };

    public static HealthPlanDto ToDto(this HealthPlan h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        Code = h.Code,
        Cnpj = h.Cnpj,
        Phone = h.Phone,
        Email = h.Email,
        Address = h.Address,
        Type = h.Type,
        Status = h.Status,
        PatientsCount = h.Patients?.Count ?? 0
    };

    public static PaymentDto ToDto(this Payment p) => new()
    {
        Id = p.Id,
        PatientId = p.PatientId,
        PatientName = p.Patient?.Name ?? string.Empty,
        Description = p.Description,
        Amount = p.Amount,
        DueDate = p.DueDate,
        PaymentDate = p.PaymentDate,
        Method = p.Method,
        Status = p.Status
    };

    public static MedicalRecordDto ToDto(this MedicalRecord m) => new()
    {
        Id = m.Id,
        PatientId = m.PatientId,
        PatientName = m.Patient?.Name ?? string.Empty,
        DoctorId = m.DoctorId,
        DoctorName = m.Doctor?.Name ?? string.Empty,
        Date = m.Date,
        ChiefComplaint = m.ChiefComplaint,
        History = m.History,
        Diagnosis = m.Diagnosis,
        Prescription = m.Prescription,
        Exams = m.Exams,
        Notes = m.Notes
    };

    public static HospitalDto ToDto(this Hospital h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        Cnpj = h.Cnpj,
        Phone = h.Phone,
        Email = h.Email,
        Address = h.Address,
        LogoUrl = h.LogoUrl
    };

    public static NotificationDto ToDto(this Notification n) => new()
    {
        Id = n.Id,
        Title = n.Title,
        Message = n.Message,
        Type = n.Type,
        Read = n.Read,
        CreatedAt = n.CreatedAt
    };
}
