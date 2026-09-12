using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
}

public interface IUserService
{
    Task<PagedResult<UserDto>> GetAllAsync(PaginationParams pagination);
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
    Task DeleteAsync(int id);
}

public interface IPatientService
{
    Task<PagedResult<PatientListItemDto>> GetAllAsync(PaginationParams pagination, string? status = null, int? healthPlanId = null);
    Task<PatientDto> GetByIdAsync(int id);
    Task<PatientDetailDto> GetDetailByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);
    Task DeleteAsync(int id);
}

public interface IDoctorService
{
    Task<PagedResult<DoctorDto>> GetAllAsync(PaginationParams pagination, string? specialty = null, string? status = null);
    Task<DoctorDetailDto> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto dto);
    Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);
    Task DeleteAsync(int id);
}

public interface IAppointmentService
{
    Task<PagedResult<AppointmentDto>> GetAllAsync(PaginationParams pagination, DateTime? date = null, int? doctorId = null, int? patientId = null, string? status = null);
    Task<AppointmentDto> GetByIdAsync(int id);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto);
    Task DeleteAsync(int id);
}

public interface IHospitalizationService
{
    Task<PagedResult<HospitalizationDto>> GetAllAsync(PaginationParams pagination, string? status = null);
    Task<HospitalizationDto> GetByIdAsync(int id);
    Task<List<HospitalizationDto>> GetCurrentlyHospitalizedAsync();
    Task<HospitalizationDto> CreateAsync(CreateHospitalizationDto dto);
    Task<HospitalizationDto> UpdateAsync(int id, UpdateHospitalizationDto dto);
    Task DeleteAsync(int id);
}

public interface ISurgeryService
{
    Task<PagedResult<SurgeryDto>> GetAllAsync(PaginationParams pagination, string? status = null);
    Task<SurgeryDto> GetByIdAsync(int id);
    Task<SurgeryDto> CreateAsync(CreateSurgeryDto dto);
    Task<SurgeryDto> UpdateAsync(int id, UpdateSurgeryDto dto);
    Task DeleteAsync(int id);
}

public interface IHealthPlanService
{
    Task<PagedResult<HealthPlanDto>> GetAllAsync(PaginationParams pagination);
    Task<HealthPlanDto> GetByIdAsync(int id);
    Task<HealthPlanDto> CreateAsync(CreateHealthPlanDto dto);
    Task<HealthPlanDto> UpdateAsync(int id, UpdateHealthPlanDto dto);
    Task DeleteAsync(int id);
}

public interface IPaymentService
{
    Task<PagedResult<PaymentDto>> GetAllAsync(PaginationParams pagination, string? status = null);
    Task<PaymentDto> GetByIdAsync(int id);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto dto);
    Task DeleteAsync(int id);
    Task<FinancialSummaryDto> GetFinancialSummaryAsync();
}

public interface IMedicalRecordService
{
    Task<PagedResult<MedicalRecordDto>> GetAllAsync(PaginationParams pagination, int? patientId = null);
    Task<MedicalRecordDto> GetByIdAsync(int id);
    Task<MedicalRecordDto> CreateAsync(CreateMedicalRecordDto dto);
    Task<MedicalRecordDto> UpdateAsync(int id, UpdateMedicalRecordDto dto);
    Task DeleteAsync(int id);
}

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}

public interface IReportService
{
    Task<ReportsSummaryDto> GetReportAsync(ReportFilterDto filter);
}

public interface IHospitalService
{
    Task<HospitalDto> GetAsync();
    Task<HospitalDto> UpdateAsync(UpdateHospitalDto dto);
}

public interface INotificationService
{
    Task<List<NotificationDto>> GetForCurrentUserAsync();
    Task MarkAsReadAsync(int id);
    Task MarkAllAsReadAsync();
}
