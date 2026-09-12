namespace HospitalProMedico.Application.DTOs;

public class ReportFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ReportsSummaryDto
{
    public List<PatientListItemDto> Patients { get; set; } = new();
    public List<AppointmentDto> Appointments { get; set; } = new();
    public List<HospitalizationDto> Hospitalizations { get; set; } = new();
    public List<SurgeryDto> Surgeries { get; set; } = new();
    public List<PaymentDto> Payments { get; set; } = new();
    public decimal TotalRevenue { get; set; }
    public List<HealthPlanDto> HealthPlans { get; set; } = new();
}
