using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class ReportService : IReportService
{
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Appointment> _appointments;
    private readonly IRepository<Hospitalization> _hospitalizations;
    private readonly IRepository<Surgery> _surgeries;
    private readonly IRepository<Payment> _payments;
    private readonly IRepository<HealthPlan> _healthPlans;

    public ReportService(
        IRepository<Patient> patients,
        IRepository<Appointment> appointments,
        IRepository<Hospitalization> hospitalizations,
        IRepository<Surgery> surgeries,
        IRepository<Payment> payments,
        IRepository<HealthPlan> healthPlans)
    {
        _patients = patients;
        _appointments = appointments;
        _hospitalizations = hospitalizations;
        _surgeries = surgeries;
        _payments = payments;
        _healthPlans = healthPlans;
    }

    public async Task<ReportsSummaryDto> GetReportAsync(ReportFilterDto filter)
    {
        var start = filter.StartDate?.Date ?? DateTime.MinValue;
        var end = filter.EndDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.MaxValue;

        var patients = await _patients.Query().Include(p => p.HealthPlan)
            .Where(p => p.CreatedAt >= start && p.CreatedAt <= end)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var appointments = await _appointments.Query().Include(a => a.Patient).Include(a => a.Doctor)
            .Where(a => a.Date >= start && a.Date <= end)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        var hospitalizations = await _hospitalizations.Query().Include(h => h.Patient).Include(h => h.Doctor)
            .Where(h => h.AdmissionDate >= start && h.AdmissionDate <= end)
            .OrderByDescending(h => h.AdmissionDate)
            .ToListAsync();

        var surgeries = await _surgeries.Query().Include(s => s.Patient).Include(s => s.Doctor)
            .Where(s => s.Date >= start && s.Date <= end)
            .OrderByDescending(s => s.Date)
            .ToListAsync();

        var payments = await _payments.Query().Include(p => p.Patient)
            .Where(p => p.DueDate >= start && p.DueDate <= end)
            .OrderByDescending(p => p.DueDate)
            .ToListAsync();

        var healthPlans = await _healthPlans.Query().Include(h => h.Patients).ToListAsync();

        return new ReportsSummaryDto
        {
            Patients = patients.Select(p => p.ToListItemDto()).ToList(),
            Appointments = appointments.Select(a => a.ToDto()).ToList(),
            Hospitalizations = hospitalizations.Select(h => h.ToDto()).ToList(),
            Surgeries = surgeries.Select(s => s.ToDto()).ToList(),
            Payments = payments.Select(p => p.ToDto()).ToList(),
            TotalRevenue = payments.Where(p => p.Status == PaymentStatus.PAGO).Sum(p => p.Amount),
            HealthPlans = healthPlans.Select(h => h.ToDto()).ToList()
        };
    }
}
