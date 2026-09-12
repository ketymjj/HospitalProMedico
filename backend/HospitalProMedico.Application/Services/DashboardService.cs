using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IRepository<Patient> _patients;
    private readonly IRepository<Doctor> _doctors;
    private readonly IRepository<Appointment> _appointments;
    private readonly IRepository<Hospitalization> _hospitalizations;
    private readonly IRepository<Surgery> _surgeries;
    private readonly IRepository<Payment> _payments;

    public DashboardService(
        IRepository<Patient> patients,
        IRepository<Doctor> doctors,
        IRepository<Appointment> appointments,
        IRepository<Hospitalization> hospitalizations,
        IRepository<Surgery> surgeries,
        IRepository<Payment> payments)
    {
        _patients = patients;
        _doctors = doctors;
        _appointments = appointments;
        _hospitalizations = hospitalizations;
        _surgeries = surgeries;
        _payments = payments;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var today = DateTime.UtcNow.Date;

        var patients = await _patients.Query().Include(p => p.HealthPlan).ToListAsync();
        var appointments = await _appointments.Query().ToListAsync();
        var hospitalizations = await _hospitalizations.Query().ToListAsync();
        var surgeries = await _surgeries.Query().ToListAsync();
        var payments = await _payments.Query().ToListAsync();

        var totalPatients = patients.Count;
        var totalDoctors = await _doctors.Query().CountAsync(d => d.Status == RegisterStatus.ATIVO);
        var appointmentsToday = appointments.Count(a => a.Date.Date == today);
        var hospitalizedPatients = hospitalizations.Count(h => h.Status == HospitalizationStatus.INTERNADO);
        var scheduledSurgeries = surgeries.Count(s => s.Status == SurgeryStatus.AGENDADA);
        var pendingPayments = payments.Count(p => p.Status == PaymentStatus.PENDENTE || p.Status == PaymentStatus.VENCIDO);

        var last6Months = Enumerable.Range(0, 6)
            .Select(i => today.AddMonths(-i))
            .Reverse()
            .Select(d => new DateTime(d.Year, d.Month, 1))
            .ToList();

        var appointmentsByMonth = last6Months.Select(m => new ChartPointDto
        {
            Label = m.ToString("MMM/yyyy"),
            Value = appointments.Count(a => a.Date.Year == m.Year && a.Date.Month == m.Month)
        }).ToList();

        var hospitalizationsByMonth = last6Months.Select(m => new ChartPointDto
        {
            Label = m.ToString("MMM/yyyy"),
            Value = hospitalizations.Count(h => h.AdmissionDate.Year == m.Year && h.AdmissionDate.Month == m.Month)
        }).ToList();

        var ageRanges = new (string Label, int Min, int Max)[]
        {
            ("0-17", 0, 17),
            ("18-30", 18, 30),
            ("31-45", 31, 45),
            ("46-60", 46, 60),
            ("60+", 61, 200)
        };

        var patientsByAgeRange = ageRanges.Select(range => new ChartPointDto
        {
            Label = range.Label,
            Value = patients.Count(p =>
            {
                var age = today.Year - p.BirthDate.Year - (today.DayOfYear < p.BirthDate.DayOfYear ? 1 : 0);
                return age >= range.Min && age <= range.Max;
            })
        }).ToList();

        var patientsByHealthPlan = patients
            .GroupBy(p => p.HealthPlan != null ? p.HealthPlan.Name : "Particular")
            .Select(g => new ChartPointDto { Label = g.Key, Value = g.Count() })
            .OrderByDescending(g => g.Value)
            .ToList();

        return new DashboardDto
        {
            TotalPatients = totalPatients,
            TotalDoctors = totalDoctors,
            AppointmentsToday = appointmentsToday,
            HospitalizedPatients = hospitalizedPatients,
            ScheduledSurgeries = scheduledSurgeries,
            PendingPayments = pendingPayments,
            AppointmentsByMonth = appointmentsByMonth,
            HospitalizationsByMonth = hospitalizationsByMonth,
            PatientsByAgeRange = patientsByAgeRange,
            PatientsByHealthPlan = patientsByHealthPlan
        };
    }
}
