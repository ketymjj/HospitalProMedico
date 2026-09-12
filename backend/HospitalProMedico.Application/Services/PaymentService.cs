using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _payments;
    private readonly IRepository<Patient> _patients;

    public PaymentService(IRepository<Payment> payments, IRepository<Patient> patients)
    {
        _payments = payments;
        _patients = patients;
    }

    public async Task<PagedResult<PaymentDto>> GetAllAsync(PaginationParams pagination, string? status = null)
    {
        var query = _payments.Query().Include(p => p.Patient).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(p => p.Patient.Name.ToLower().Contains(term) || p.Description.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<PaymentStatus>(status, true, out var st))
        {
            query = query.Where(p => p.Status == st);
        }

        query = query.OrderByDescending(p => p.DueDate);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<PaymentDto>
        {
            Items = result.Items.Select(p => p.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<PaymentDto> GetByIdAsync(int id)
    {
        var payment = await _payments.Query().Include(p => p.Patient).FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException("Pagamento", id);
        return payment.ToDto();
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        if (!await _patients.ExistsAsync(p => p.Id == dto.PatientId))
        {
            throw new BusinessRuleException("Paciente informado não existe.");
        }

        var payment = new Payment
        {
            PatientId = dto.PatientId,
            Description = dto.Description,
            Amount = dto.Amount,
            DueDate = dto.DueDate,
            Method = dto.Method,
            Status = PaymentStatus.PENDENTE
        };

        await _payments.AddAsync(payment);
        await _payments.SaveChangesAsync();

        return await GetByIdAsync(payment.Id);
    }

    public async Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto dto)
    {
        var payment = await _payments.GetByIdAsync(id) ?? throw new NotFoundException("Pagamento", id);

        payment.PatientId = dto.PatientId;
        payment.Description = dto.Description;
        payment.Amount = dto.Amount;
        payment.DueDate = dto.DueDate;
        payment.Method = dto.Method;
        payment.Status = dto.Status;
        payment.PaymentDate = dto.PaymentDate;
        payment.UpdatedAt = DateTime.UtcNow;

        if (dto.Status == PaymentStatus.PAGO && payment.PaymentDate is null)
        {
            payment.PaymentDate = DateTime.UtcNow;
        }

        _payments.Update(payment);
        await _payments.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var payment = await _payments.GetByIdAsync(id) ?? throw new NotFoundException("Pagamento", id);
        _payments.Remove(payment);
        await _payments.SaveChangesAsync();
    }

    public async Task<FinancialSummaryDto> GetFinancialSummaryAsync()
    {
        var payments = await _payments.Query().ToListAsync();
        var today = DateTime.UtcNow.Date;

        var totalReceived = payments.Where(p => p.Status == PaymentStatus.PAGO).Sum(p => p.Amount);
        var totalPending = payments.Where(p => p.Status == PaymentStatus.PENDENTE && p.DueDate.Date >= today).Sum(p => p.Amount);
        var totalOverdue = payments.Where(p => p.Status == PaymentStatus.VENCIDO || (p.Status == PaymentStatus.PENDENTE && p.DueDate.Date < today)).Sum(p => p.Amount);

        var monthlyRevenue = payments
            .Where(p => p.Status == PaymentStatus.PAGO && p.PaymentDate.HasValue)
            .GroupBy(p => new { p.PaymentDate!.Value.Year, p.PaymentDate.Value.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new MonthlyRevenueDto
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM/yyyy"),
                Total = g.Sum(p => p.Amount)
            })
            .ToList();

        return new FinancialSummaryDto
        {
            TotalReceived = totalReceived,
            TotalPending = totalPending,
            TotalOverdue = totalOverdue,
            MonthlyRevenue = monthlyRevenue
        };
    }
}
