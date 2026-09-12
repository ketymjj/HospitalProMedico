namespace HospitalProMedico.Application.DTOs;

public class DashboardDto
{
    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int AppointmentsToday { get; set; }
    public int HospitalizedPatients { get; set; }
    public int ScheduledSurgeries { get; set; }
    public int PendingPayments { get; set; }

    public List<ChartPointDto> AppointmentsByMonth { get; set; } = new();
    public List<ChartPointDto> HospitalizationsByMonth { get; set; } = new();
    public List<ChartPointDto> PatientsByAgeRange { get; set; } = new();
    public List<ChartPointDto> PatientsByHealthPlan { get; set; } = new();
}

public class ChartPointDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
