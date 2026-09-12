using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).NotEmpty();
        RuleFor(x => x.Method).IsInEnum();
    }
}

public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentDto>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).NotEmpty();
        RuleFor(x => x.Method).IsInEnum();
        RuleFor(x => x.Status).IsInEnum();
    }
}
