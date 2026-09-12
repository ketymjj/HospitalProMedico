using HospitalProMedico.Domain.Enums;

namespace HospitalProMedico.Application.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
}

public class CreatePaymentDto
{
    public int PatientId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public PaymentMethod Method { get; set; }
}

public class UpdatePaymentDto : CreatePaymentDto
{
    public DateTime? PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
}

public class FinancialSummaryDto
{
    public decimal TotalReceived { get; set; }
    public decimal TotalPending { get; set; }
    public decimal TotalOverdue { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
